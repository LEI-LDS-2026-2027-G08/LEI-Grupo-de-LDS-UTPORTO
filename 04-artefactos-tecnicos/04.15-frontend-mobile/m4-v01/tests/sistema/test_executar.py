"""Regressões do orquestrador; não substituem os 18 cenários no emulador."""

import argparse
import contextlib
import datetime as dt
import importlib.util
import io
import json
import os
from pathlib import Path
import subprocess
import tempfile
import unittest
from unittest.mock import Mock, patch
import xml.etree.ElementTree as ET

spec = importlib.util.spec_from_file_location("executar_i137", Path(__file__).with_name("executar.py"))
executar = importlib.util.module_from_spec(spec)
spec.loader.exec_module(executar)


class OrquestradorTests(unittest.TestCase):
    def setUp(self):
        temporary = tempfile.TemporaryDirectory()
        self.addCleanup(temporary.cleanup)
        self.root = Path(temporary.name)
        self.mobile = self.root / "frontend mobile"
        self.mobile.mkdir()
        self.args = argparse.Namespace(serial=None, avd="Teste", sql_image="sql-teste", case=None)
        for name, value in (("MOBILE", self.mobile), ("ROOT", self.root)):
            self.enterContext(patch.object(executar, name, value))

    def ambiente(self):
        with patch.object(executar, "fuso_lisboa", return_value=dt.timezone.utc):
            return executar.Ambiente(self.args)

    def test_windows_descobre_sdk_e_executaveis_com_espacos_no_caminho(self):
        local = self.root / "Utilizador de Teste" / "AppData" / "Local"
        with patch.object(executar, "WINDOWS", True), patch.dict(os.environ, {"LOCALAPPDATA": str(local)}, clear=True):
            environment = self.ambiente()
        sdk = local / "Android/Sdk"
        self.assertEqual(str(sdk), environment.env["ANDROID_HOME"])
        self.assertEqual(str(sdk / "platform-tools/adb.exe"), environment.adb)
        self.assertEqual(str(sdk / "emulator/emulator.exe"), environment.emulator)
        self.assertEqual(str(self.mobile / "gradlew.bat"), environment.gradle_wrapper)

    def test_sdk_configurado_tem_prioridade_sobre_a_pasta_predefinida(self):
        with patch.dict(os.environ, {"ANDROID_HOME": str(self.root / "SDK próprio"),
                                     "ANDROID_SDK_ROOT": "/outro/sdk"}, clear=True):
            self.assertEqual(self.root / "SDK próprio", executar.localizar_sdk())

    def test_sdk_root_tambem_e_respeitado(self):
        with patch.dict(os.environ, {"ANDROID_SDK_ROOT": str(self.root / "SDK")}, clear=True):
            self.assertEqual(self.root / "SDK", executar.localizar_sdk())

    def test_macos_e_linux_mantem_as_pastas_respetivas(self):
        with patch.object(executar, "WINDOWS", False), patch.dict(os.environ, {}, clear=True), \
                patch.object(executar.Path, "home", return_value=self.root):
            with patch.object(executar.sys, "platform", "darwin"):
                self.assertEqual(self.root / "Library/Android/sdk", executar.localizar_sdk())
            with patch.object(executar.sys, "platform", "linux"):
                self.assertEqual(self.root / "Android/Sdk", executar.localizar_sdk())

    def test_sem_tzdata_falha_antes_de_criar_recursos_com_instrucao_util(self):
        stderr = io.StringIO()
        with patch.object(executar, "ZoneInfo", side_effect=executar.ZoneInfoNotFoundError("Europe/Lisbon")), \
                patch.object(executar.sys, "argv", ["executar.py"]), \
                patch.object(executar.subprocess, "run") as run, \
                patch.object(executar.subprocess, "Popen") as popen, \
                contextlib.redirect_stderr(stderr):
            with self.assertRaises(SystemExit) as error:
                executar.main()
        self.assertEqual(2, error.exception.code)
        self.assertIn("python -m pip install tzdata", stderr.getvalue())
        self.assertFalse((self.mobile / "tests").exists())
        run.assert_not_called()
        popen.assert_not_called()

    def test_windows_invoca_o_bat_e_nao_publica_a_senha_no_registo(self):
        with patch.object(executar, "WINDOWS", True):
            environment = self.ambiente()
        results = self.mobile / "app/build/outputs/androidTest-results/connected/debug"
        results.mkdir(parents=True)
        suite = ET.Element("testsuite", tests="18", failures="0", errors="0", skipped="0")
        for index in range(18):
            ET.SubElement(suite, "testcase", classname=executar.CLASS, name=str(index))
        ET.ElementTree(suite).write(results / "TEST-suite.xml", encoding="utf-8")
        process = Mock()
        process.stdout = [environment.passwords["cliente.joao"] + "\n"]
        process.wait.return_value = 0
        environment.adb_run = Mock()
        with patch.object(executar, "WINDOWS", True), patch.object(executar.subprocess, "Popen", return_value=process) as popen, \
                contextlib.redirect_stdout(io.StringIO()):
            environment.test(1)
        self.assertEqual(str(self.mobile / "gradlew.bat"), popen.call_args.args[0][0])
        self.assertFalse(popen.call_args.kwargs["start_new_session"])
        self.assertEqual(environment.env["ANDROID_HOME"], popen.call_args.kwargs["env"]["ANDROID_HOME"])
        self.assertTrue(environment.repetitions[0]["passou"])
        log = (environment.output / "execucao-1/gradle.log").read_text(encoding="utf-8")
        self.assertNotIn(environment.passwords["cliente.joao"], log)
        self.assertIn("[omitido]", log)

    def test_windows_termina_so_a_arvore_do_pid_criado(self):
        process = Mock(pid=701)
        process.poll.return_value = None
        with patch.object(executar, "WINDOWS", True), \
                patch.object(executar.subprocess, "run", return_value=Mock(returncode=0)) as run, \
                patch.object(executar.os, "killpg", create=True) as killpg:
            executar.terminar_arvore(process)
        self.assertEqual(["taskkill", "/PID", "701", "/T", "/F"], run.call_args.args[0])
        process.wait.assert_called_once_with(timeout=15)
        killpg.assert_not_called()

    def test_windows_taskkill_falhado_e_reportado(self):
        process = Mock(pid=702)
        process.poll.return_value = None
        with patch.object(executar, "WINDOWS", True), \
                patch.object(executar.subprocess, "run", return_value=Mock(returncode=1)):
            with self.assertRaisesRegex(RuntimeError, "702"):
                executar.terminar_arvore(process)

    def test_posix_termina_grupo_e_forca_apenas_se_exceder_o_prazo(self):
        process = Mock(pid=703)
        process.poll.return_value = None
        process.wait.side_effect = [subprocess.TimeoutExpired("api", 15), 0]
        with patch.object(executar, "WINDOWS", False), patch.object(executar.os, "killpg", create=True) as killpg, \
                patch.object(executar.signal, "SIGKILL", 9, create=True):
            executar.terminar_arvore(process)
        self.assertEqual([(703, executar.signal.SIGTERM), (703, 9)], [call.args for call in killpg.call_args_list])

    def test_processo_ja_terminado_nao_e_morto_novamente(self):
        process = Mock()
        process.poll.return_value = 0
        with patch.object(executar.subprocess, "run") as run, patch.object(executar.os, "killpg", create=True) as killpg:
            executar.terminar_arvore(process)
            executar.terminar_arvore(None)
        run.assert_not_called()
        killpg.assert_not_called()

    def test_falha_ao_fechar_api_nao_impede_tuneis_sql_e_relatorio(self):
        environment = self.ambiente()
        environment.api = Mock()
        environment.api_log = io.StringIO()
        environment.server = Mock()
        environment.container = environment.tunnels = True
        environment.adb_run = Mock()
        environment.run = Mock(return_value=Mock(stdout="commit-teste\n"))

        def terminate(process):
            if process is environment.api:
                raise OSError("Falha provocada ao terminar API")

        with patch.object(executar, "terminar_arvore", side_effect=terminate), \
                contextlib.redirect_stdout(io.StringIO()):
            with self.assertRaisesRegex(RuntimeError, "Limpeza incompleta"):
                environment.close()
        self.assertTrue(environment.api_log.closed)
        environment.server.server_close.assert_called_once()
        self.assertEqual(2, environment.adb_run.call_count)
        environment.run.assert_any_call(["docker", "rm", "-f", environment.name])
        report = json.loads((environment.output / "resultado.json").read_text(encoding="utf-8"))
        self.assertIn("API", report["erros_limpeza"][0])


if __name__ == "__main__":
    unittest.main()
