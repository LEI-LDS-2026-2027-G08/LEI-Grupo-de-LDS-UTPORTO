"""Verifica a atualização das fontes após correções da revisão, sem criar commits."""

import contextlib
import hashlib
import importlib.util
import io
import json
from pathlib import Path
import subprocess
import tempfile
import unittest
from unittest.mock import patch

spec = importlib.util.spec_from_file_location("arquivar_i137", Path(__file__).with_name("arquivar.py"))
arquivar = importlib.util.module_from_spec(spec)
spec.loader.exec_module(arquivar)


class ArquivoTests(unittest.TestCase):
    def setUp(self):
        temporary = tempfile.TemporaryDirectory()
        self.addCleanup(temporary.cleanup)
        self.dest = Path(temporary.name).resolve()
        self.enterContext(patch.object(arquivar, "DEST", self.dest))
        self.old = b"versao anterior\n"
        self.new = b"versao corrigida\n"
        self.name = "src/frontend-mobile/tests/sistema/executar.py"
        self.relative = "tests/sistema/executar.py"
        self.target = self.dest / self.relative
        self.target.parent.mkdir(parents=True)
        self.target.write_bytes(self.old)
        (self.dest / "m4-i137-fontes-executadas-v01.json").write_text(json.dumps({
            "ficheiros": {self.name: hashlib.sha256(self.new).hexdigest()}}), encoding="utf-8")
        (self.dest / "m4-i137-origem-fontes-v01.json").write_text(json.dumps({
            "ficheiros": {self.relative: hashlib.sha256(self.old).hexdigest()}}), encoding="utf-8")
        self.enterContext(patch.object(arquivar, "git", side_effect=self.git))
        self.show = self.enterContext(patch.object(arquivar.subprocess, "run", return_value=
                                      subprocess.CompletedProcess([], 0, self.new)))

    def git(self, *args):
        if args[0] == "rev-parse":
            return b"commit-da-correcao\n"
        if args[0] == "ls-tree":
            return (self.name + "\n").encode()
        if args[0] == "show":
            return self.new
        raise AssertionError(args)

    def test_substitui_so_uma_copia_que_corresponde_ao_manifesto_anterior(self):
        with contextlib.redirect_stdout(io.StringIO()):
            arquivar.main()
        self.assertEqual(self.new, self.target.read_bytes())
        origin = json.loads((self.dest / "m4-i137-origem-fontes-v01.json").read_text(encoding="utf-8"))
        self.assertEqual("commit-da-correcao", origin["commit"])
        self.assertEqual(hashlib.sha256(self.new).hexdigest(), origin["ficheiros"][self.relative])

    def test_preserva_uma_edicao_manual_no_destino(self):
        self.target.write_bytes(b"edicao do utilizador\n")
        with self.assertRaisesRegex(SystemExit, "nada foi escrito"):
            arquivar.main()
        self.assertEqual(b"edicao do utilizador\n", self.target.read_bytes())

    def test_recusa_versao_do_commit_diferente_da_testada(self):
        self.show.return_value = subprocess.CompletedProcess([], 0, b"codigo nao testado\n")
        with self.assertRaisesRegex(SystemExit, "versão testada"):
            arquivar.main()
        self.assertEqual(self.old, self.target.read_bytes())


if __name__ == "__main__":
    unittest.main()
