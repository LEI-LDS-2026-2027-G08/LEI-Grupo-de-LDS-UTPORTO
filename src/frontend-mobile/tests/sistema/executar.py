#!/usr/bin/env python3
"""I137: duas execuções instrumentadas, com API e SQL Server descartáveis.

Não usa contas privadas nem se liga ao Azure. Cada cenário repõe a sua base
e prepara PD-01 a PD-04 pela API real. Credenciais geradas ficam em memória.
"""

import argparse
import base64
import datetime as dt
import hashlib
import http.server
import json
import os
from pathlib import Path
import secrets
import shutil
import signal
import socket
import struct
import subprocess
import threading
import time
import urllib.error
import urllib.request
import uuid
import xml.etree.ElementTree as ET
from zoneinfo import ZoneInfo

MOBILE = Path(__file__).resolve().parents[2]
ROOT = MOBILE.parents[1]
SQL_DATA = ROOT / "04-artefactos-tecnicos/04.13-scripts-de-base-de-dados/m3-v02/dados-demonstracao/02-dados-demonstracao.sql"
CLASS = "pt.ipp.estg.g08.clinica.sistema.FluxosClienteSistemaTest"
PACKAGE = "pt.ipp.estg.g08.clinica"


class Ambiente:
    def __init__(self, args):
        self.args = args
        self.id = uuid.uuid4().hex[:12]
        self.name = "i137-" + self.id
        self.database = "I137_" + self.id
        self.output = MOBILE / "tests/sistema/resultados" / self.id
        self.output.mkdir(parents=True)
        sdk = Path(os.environ.get("ANDROID_HOME", str(Path.home() / "Library/Android/sdk")))
        self.adb = str(sdk / "platform-tools/adb")
        self.emulator = str(sdk / "emulator/emulator")
        self.serial = args.serial or "emulator-5580"
        self.api = self.emu = self.server = None
        self.container = False
        self.tunnels = False
        self.sa = "Aa1!" + secrets.token_hex(20)
        self.key = secrets.token_hex(48)
        self.control = secrets.token_hex(32)
        self.passwords = {key: "Aa1!" + secrets.token_hex(12) for key in (
            "cliente.joao", "cliente.maria", "rececao.bloqueada", "rececao", "vet.ana"
        )}
        self.private = [self.sa, self.key, self.control, *self.passwords.values()]
        self.env = {**os.environ, "MSSQL_SA_PASSWORD": self.sa, "SQLCMDPASSWORD": self.sa}
        self.repetitions = []

    def clean_text(self, value):
        for value_private in self.private:
            value = value.replace(value_private, "[omitido]")
        return value

    def run(self, command, *, input=None, env=None, check=True):
        result = subprocess.run(command, input=input, text=True, capture_output=True,
                                env=env or self.env, cwd=ROOT)
        if check and result.returncode:
            raise RuntimeError(self.clean_text(result.stdout + result.stderr))
        return result

    def adb_run(self, *command, check=True):
        return self.run([self.adb, "-s", self.serial, *command], check=check)

    def sql(self, sql, database="master"):
        return self.run(["docker", "exec", "-i", "-e", "SQLCMDPASSWORD", self.name,
                         "/opt/mssql-tools18/bin/sqlcmd", "-S", "localhost", "-U", "sa",
                         "-C", "-b", "-I", "-f", "65001", "-d", database], input=sql)

    def api_request(self, path, *, token=None, data=None, expected=200):
        headers = {"Content-Type": "application/json"}
        if token:
            headers["Authorization"] = "Bearer " + token
        request = urllib.request.Request("http://127.0.0.1:5159" + path, headers=headers,
                                         data=json.dumps(data).encode() if data is not None else None)
        try:
            with urllib.request.urlopen(request, timeout=40) as response:
                code, content = response.status, response.read()
        except urllib.error.HTTPError as error:
            code, content = error.code, error.read()
        if code != expected:
            raise RuntimeError(f"Preparação {path}: esperado {expected}, obtido {code}: " +
                               self.clean_text(content.decode(errors="replace")))
        return json.loads(content) if content else None

    def login(self, account):
        response = self.api_request("/auth/login", data={
            "email": account + "@clinica-g08.test", "password": self.passwords[account]
        })
        return response["token"]

    @staticmethod
    def until(condition, timeout, description):
        deadline = time.monotonic() + timeout
        while time.monotonic() < deadline:
            try:
                if condition():
                    return
            except (OSError, RuntimeError, urllib.error.URLError, subprocess.SubprocessError):
                pass
        raise RuntimeError("Tempo esgotado: " + description)

    def start(self):
        # Nunca reutilizar nem parar uma API ou serviço do utilizador.
        for port in (5159, 5160):
            with socket.socket() as probe:
                probe.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
                try:
                    probe.bind(("127.0.0.1", port))
                    probe.listen(1)
                except OSError as error:
                    raise RuntimeError(f"Porta {port} ocupada. Termine o seu serviço antes dos testes.") from error
        self.run(["docker", "run", "--detach", "--rm", "--name", self.name,
                  "--env", "ACCEPT_EULA=Y", "--env", "MSSQL_SA_PASSWORD",
                  "--publish", "127.0.0.1::1433", self.args.sql_image])
        self.container = True
        self.until(lambda: self.sql("SELECT 1;").returncode == 0, 180, "SQL de teste")
        port = self.run(["docker", "port", self.name, "1433/tcp"]).stdout.strip().rsplit(":", 1)[1]
        connection = (f"Server=127.0.0.1,{port};Database={self.database};User Id=sa;"
                      f"Password={self.sa};TrustServerCertificate=True;Encrypt=True")
        environment = {**self.env, "ConnectionStrings__ClinicaDb": connection,
                       "Jwt__Key": self.key, "Jwt__Issuer": "I137", "Jwt__Audience": "I137",
                       "Jwt__ExpirationHours": "8", "ASPNETCORE_ENVIRONMENT": "Development",
                       "BaseDados__AplicarMigracoesNoArranque": "true"}
        self.api_log = (self.output / "backend.log").open("w")
        self.api = subprocess.Popen([
            "dotnet", "run", "--no-launch-profile", "--project",
            str(ROOT / "src/backend/ClinicaVeterinaria.Api"), "--urls", "http://127.0.0.1:5159"
        ], cwd=ROOT, env=environment, stdout=self.api_log, stderr=subprocess.STDOUT,
            start_new_session=True)
        self.until(lambda: self.api.poll() is None and self.api_request("/auth/me", expected=401) is not None,
                   240, "API com migrações aplicadas")
        self.carga = dt.datetime.now(ZoneInfo("Europe/Lisbon")).date()
        self.sql(SQL_DATA.read_text(encoding="utf-8-sig"), self.database)
        for account, password in self.passwords.items():
            salt = secrets.token_bytes(16)
            hashed = base64.b64encode(b"\x01" + struct.pack(">III", 2, 100000, len(salt)) + salt +
                                      hashlib.pbkdf2_hmac("sha512", password.encode(), salt, 100000, 32)).decode()
            self.sql(f"UPDATE dbo.Utilizador SET PasswordHash=N'{hashed}' WHERE Email=N'{account}@clinica-g08.test';",
                     self.database)
        self.login("cliente.joao")  # Confirma o formato do hash com a API, não com uma cópia do algoritmo.
        self.sql(f"BACKUP DATABASE [{self.database}] TO DISK=N'/tmp/i137-baseline.bak' WITH INIT;")
        self.start_control()
        if not self.args.serial:
            if self.serial in self.run([self.adb, "devices"]).stdout:
                raise RuntimeError("A porta do emulador de testes já está em uso: " + self.serial)
            self.emu_log = (self.output / "emulador.log").open("w")
            self.emu = subprocess.Popen([
                self.emulator, "-avd", self.args.avd, "-port", "5580", "-no-window",
                "-no-audio", "-no-snapshot", "-no-boot-anim", "-timezone", "Europe/Lisbon"
            ], stdout=self.emu_log, stderr=subprocess.STDOUT)
        self.until(lambda: self.adb_run("get-state", check=False).stdout.strip() == "device",
                   240, "ligação ao emulador")
        self.until(lambda: self.adb_run("shell", "getprop", "sys.boot_completed").stdout.strip() == "1",
                   240, "emulador Android")
        for setting in ("window_animation_scale", "transition_animation_scale", "animator_duration_scale"):
            self.adb_run("shell", "settings", "put", "global", setting, "0")
        timezone = self.adb_run("shell", "getprop", "persist.sys.timezone").stdout.strip()
        if timezone != "Europe/Lisbon":
            self.adb_run("shell", "cmd", "alarm", "set-timezone", "Europe/Lisbon")
            timezone = self.adb_run("shell", "getprop", "persist.sys.timezone").stdout.strip()
        if timezone != "Europe/Lisbon":
            raise RuntimeError("O emulador tem de usar o fuso Europe/Lisbon.")
        self.adb_run("reverse", "tcp:5159", "tcp:5159")
        self.adb_run("reverse", "tcp:5160", "tcp:5160")
        self.tunnels = True

    def prepare_case(self):
        self.sql(f"ALTER DATABASE [{self.database}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;\n"
                 f"RESTORE DATABASE [{self.database}] FROM DISK=N'/tmp/i137-baseline.bak' WITH REPLACE;\n"
                 f"ALTER DATABASE [{self.database}] SET MULTI_USER;")
        token = self.login("rececao")
        today = dt.datetime.now(ZoneInfo("Europe/Lisbon")).date()
        day = today + dt.timedelta(days=7)
        while day.weekday() >= 5:
            day += dt.timedelta(days=1)
        execution = uuid.uuid4().hex[:12]
        for animal, hour in ((1, 10), (2, 11)):
            when = dt.datetime.combine(day, dt.time(hour), tzinfo=ZoneInfo("Europe/Lisbon"))
            result = self.api_request("/consultas", token=token, data={
                "animalId": animal, "veterinarioId": 4,
                "dataHora": when.astimezone(dt.timezone.utc).isoformat(),
                "motivo": "Teste ST-MOB " + execution
            }, expected=201)
            if animal == 1:
                appointment = result["id"]
        empty_email = f"st.mob.{execution}@clinica-g08.test"
        empty_password = "Aa1!" + secrets.token_hex(12)
        self.private.append(empty_password)
        self.api_request("/clientes", token=token, data={
            "nome": "Cliente ST-MOB " + execution, "telefone": "+351 900 000 000",
            "email": empty_email, "passwordInicial": empty_password
        }, expected=201)
        product = f"Vacina ST-MOB {execution} (fictícia)"
        self.api_request("/animais/1/vacinacoes", token=self.login("vet.ana"), data={
            "tipo": "Vacina", "produto": product, "lote": "ST-" + execution,
            "dataAdministracao": today.isoformat(), "intervaloDias": 30
        }, expected=201)
        history = self.api_request("/me/consultas?tipo=historico", token=self.login("cliente.joao"))
        return {"bobi": 1, "mia": 3, "kiko": 2, "execucao": execution,
                "consultaBobi": appointment, "consultaPassadaBobi": history[0]["id"],
                "emailVazio": empty_email, "senhaVazio": empty_password, "produtoVacina": product,
                "hoje": today.isoformat(), "carga": self.carga.isoformat(),
                "dataConsultaTexto": day.strftime("%d/%m/%Y")}

    def start_control(self):
        environment = self

        class Control(http.server.BaseHTTPRequestHandler):
            def do_POST(self):
                if self.path != "/reset" or self.headers.get("Authorization") != "Bearer " + environment.control:
                    self.send_error(403)
                    return
                try:
                    data = json.dumps(environment.prepare_case()).encode()
                    self.send_response(200)
                    self.send_header("Content-Type", "application/json")
                    self.end_headers()
                    self.wfile.write(data)
                except Exception as error:
                    print(environment.clean_text(str(error)), flush=True)
                    self.send_error(500, "Preparacao isolada falhou")

            def log_message(self, *_):
                pass

        self.server = http.server.HTTPServer(("127.0.0.1", 5160), Control)
        threading.Thread(target=self.server.serve_forever, daemon=True).start()

    def test(self, number):
        command = [str(MOBILE / "gradlew"), "connectedDebugAndroidTest", "--no-configuration-cache",
                   "--console=plain", "-PapiBaseUrl=http://localhost:5159",
                   f"-Pandroid.testInstrumentationRunnerArguments.class={CLASS}",
                   f"-Pandroid.testInstrumentationRunnerArguments.tokenPreparacao={self.control}",
                   f"-Pandroid.testInstrumentationRunnerArguments.idExecucao={self.id}-{number}"]
        names = {"senhaClienteJoao": "cliente.joao", "senhaClienteMaria": "cliente.maria",
                 "senhaRececaoBloqueada": "rececao.bloqueada"}
        command += [f"-Pandroid.testInstrumentationRunnerArguments.{arg}={self.passwords[account]}"
                    for arg, account in names.items()]
        if self.args.case:
            command[5] += "#" + self.args.case
        environment = {**self.env, "ANDROID_SERIAL": self.serial}
        process = subprocess.Popen(command, cwd=MOBILE, env=environment, stdout=subprocess.PIPE,
                                   stderr=subprocess.STDOUT, text=True)
        folder = self.output / f"execucao-{number}"
        folder.mkdir()
        with (folder / "gradle.log").open("w") as log:
            for line in process.stdout:
                safe = self.clean_text(line)
                print(safe, end="", flush=True)
                log.write(safe)
        code = process.wait()
        results = MOBILE / "app/build/outputs/androidTest-results/connected/debug"
        suites = []
        for p in results.rglob("TEST-*.xml"):
            root = ET.parse(p).getroot()
            if any(CLASS in case.get("classname", "") for case in root.iter("testcase")):
                suites.append({"tests": int(root.get("tests", 0)), "failures": int(root.get("failures", 0)),
                               "errors": int(root.get("errors", 0)), "skipped": int(root.get("skipped", 0))})
        for path in results.rglob("*.xml"):
            target = folder / "junit" / path.relative_to(results)
            target.parent.mkdir(parents=True, exist_ok=True)
            shutil.copy2(path, target)
        capture_path = f"/sdcard/Download/i137/{self.id}-{number}"
        self.adb_run("pull", capture_path, str(folder / "falhas"), check=False)
        if code and not list((folder / "falhas").glob("*.png")):
            # Também cobrir uma falha do próprio runner, anterior ao corpo do teste.
            (folder / "falhas").mkdir(exist_ok=True)
            with (folder / "falhas/falha-instrumentacao.png").open("wb") as capture:
                subprocess.run([self.adb, "-s", self.serial, "exec-out", "screencap", "-p"],
                               stdout=capture, stderr=subprocess.PIPE, check=False)
        total = {key: sum(s[key] for s in suites) for key in ("tests", "failures", "errors", "skipped")}
        expected = 1 if self.args.case else 18
        ok = code == 0 and total == {"tests": expected, "failures": 0, "errors": 0, "skipped": 0}
        self.repetitions.append({"execucao": number, "codigo": code, **total, "passou": ok})
        if not ok:
            raise RuntimeError("Execução instrumentada sem aprovação: " + json.dumps(self.repetitions[-1]))

    def close(self):
        if self.server:
            self.server.shutdown()
            self.server.server_close()
        if self.api and self.api.poll() is None:
            os.killpg(self.api.pid, signal.SIGTERM)
            try:
                self.api.wait(timeout=15)
            except subprocess.TimeoutExpired:
                os.killpg(self.api.pid, signal.SIGKILL)
                self.api.wait()
        if hasattr(self, "api_log"):
            self.api_log.close()
        if self.tunnels:
            for port in (5159, 5160):
                self.adb_run("reverse", "--remove", f"tcp:{port}", check=False)
        if self.emu:
            self.adb_run("emu", "kill", check=False)
            try:
                self.emu.wait(timeout=20)
            except subprocess.TimeoutExpired:
                self.emu.kill()
            self.emu_log.close()
        if self.container:
            self.run(["docker", "rm", "-f", self.name], check=False)
        report = {"base": self.run(["git", "rev-parse", "HEAD"]).stdout.strip(),
                  "data": dt.datetime.now(ZoneInfo("Europe/Lisbon")).isoformat(),
                  "sql_image": self.args.sql_image, "execucoes": self.repetitions,
                  "ambiente": "API e contentor SQL descartáveis; emulador Android sem interface"}
        (self.output / "resultado.json").write_text(json.dumps(report, ensure_ascii=False, indent=2) + "\n")
        # Inclui eventuais mensagens de erro do runner Android, sem deixar credenciais no artefacto.
        for root in (self.output, MOBILE / "app/build/outputs/androidTest-results", MOBILE / "app/build/reports/androidTests"):
            if root.exists():
                for path in root.rglob("*"):
                    if path.is_file() and path.suffix in (".log", ".xml", ".json", ".html", ".txt"):
                        path.write_text(self.clean_text(path.read_text(errors="replace")))
        print("Resultados: " + str(self.output), flush=True)


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--avd", default="Medium_Phone_API_37.0")
    parser.add_argument("--serial", help="Emulador já iniciado; o script não o fecha")
    parser.add_argument("--sql-image", default="mcr.microsoft.com/mssql/server:2025-latest")
    parser.add_argument("--execucoes", type=int, default=2, choices=(1, 2))
    parser.add_argument("--case", help="Um identificador ST-MOB para diagnóstico; não substitui a suite")
    args = parser.parse_args()
    environment = Ambiente(args)
    try:
        environment.start()
        for number in range(1, args.execucoes + 1):
            environment.test(number)
    finally:
        environment.close()


if __name__ == "__main__":
    main()
