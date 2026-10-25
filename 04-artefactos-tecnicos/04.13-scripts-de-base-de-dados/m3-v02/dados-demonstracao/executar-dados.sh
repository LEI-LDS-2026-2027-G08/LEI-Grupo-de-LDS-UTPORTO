#!/usr/bin/env bash
# =============================================================================
# Projeto        : Gestão de Clínica Veterinária — Grupo G08 — LDS 2026/2027
# Issue          : I061 — Escrever os scripts de dados iniciais e de demonstração
# Ficheiro       : dados-demonstracao/executar-dados.sh
# Versão         : m3-v02
# Finalidade     : Executar os scripts de dados (Linux, macOS ou pipeline):
#                  01-dados-referencia-provisorios.sql (só com
#                  --dados-referencia-provisorios), 02-dados-demonstracao.sql duas
#                  vezes (a segunda prova a reexecução sem duplicar dados) e
#                  verificar-dados-demonstracao.sql; parar no primeiro erro e
#                  guardar o registo da execução.
# Configuração   : SQLCMD_SERVIDOR (localhost), SQLCMD_UTILIZADOR (sa) e
#                  SQLCMDPASSWORD (obrigatória; nunca fica em ficheiros).
# Opções         : --dados-referencia-provisorios, --confiar-certificado,
#                  --pasta-registos DIR
# =============================================================================
set -euo pipefail

SERVIDOR="${SQLCMD_SERVIDOR:-localhost}"
UTILIZADOR="${SQLCMD_UTILIZADOR:-sa}"
REFERENCIA=0
CERTIFICADO=()
PASTA_REGISTOS="$(pwd)"

while [[ $# -gt 0 ]]; do
    case "$1" in
        --dados-referencia-provisorios) REFERENCIA=1 ;;
        --confiar-certificado) CERTIFICADO=(-C) ;;
        --pasta-registos)      PASTA_REGISTOS="$2"; shift ;;
        *) echo "Opção desconhecida: $1" >&2; exit 2 ;;
    esac
    shift
done

if ! command -v sqlcmd >/dev/null 2>&1; then
    for candidato in /opt/mssql-tools18/bin /opt/mssql-tools/bin; do
        [[ -x "$candidato/sqlcmd" ]] && PATH="$candidato:$PATH"
    done
fi
command -v sqlcmd >/dev/null 2>&1 || { echo "ERRO: sqlcmd não encontrado (instalar mssql-tools18 ou go-sqlcmd)." >&2; exit 1; }
: "${SQLCMDPASSWORD:?Defina SQLCMDPASSWORD com a palavra-passe do login (não é guardada em ficheiros).}"
export SQLCMDPASSWORD

PASTA="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
SCRIPTS=()
[[ $REFERENCIA -eq 1 ]] && SCRIPTS+=( "$PASTA/01-dados-referencia-provisorios.sql" )
SCRIPTS+=( "$PASTA/02-dados-demonstracao.sql" "$PASTA/02-dados-demonstracao.sql" "$PASTA/verificar-dados-demonstracao.sql" )

mkdir -p "$PASTA_REGISTOS"
REGISTO="$PASTA_REGISTOS/dados-execucao-$(date +%Y%m%d-%H%M%S).log"
escrever() { printf '%s\n' "$*" | tee -a "$REGISTO"; }

escrever "Execução dos scripts de dados iniciais e de demonstração (I061) — ClinicaVeterinaria"
escrever "Data e hora      : $(date '+%Y-%m-%d %H:%M:%S %z')"
escrever "Computador       : $(hostname)"
escrever "Servidor         : $SERVIDOR"
escrever "Autenticação     : SQL Server (login $UTILIZADOR)"
escrever "Scripts          : $(for s in "${SCRIPTS[@]}"; do printf '%s ' "$(basename "$s")"; done)"

if ! VERSAO="$(sqlcmd -S "$SERVIDOR" -U "$UTILIZADOR" ${CERTIFICADO[@]+"${CERTIFICADO[@]}"} -b -h -1 -W -Q 'SET NOCOUNT ON; SELECT @@VERSION;' 2>&1)"; then
    escrever "$VERSAO"
    escrever "ERRO: não foi possível ligar ao servidor. Nenhum script foi executado."
    exit 1
fi
escrever "SQL Server       : $(printf '%s' "$VERSAO" | head -n 1 | tr -s '[:space:]' ' ')"

for s in "${SCRIPTS[@]}"; do
    escrever ""
    escrever "=== $(basename "$s") — início $(date +%T)"
    set +e
    sqlcmd -S "$SERVIDOR" -U "$UTILIZADOR" ${CERTIFICADO[@]+"${CERTIFICADO[@]}"} -b -I -f 65001 -i "$s" 2>&1 | tee -a "$REGISTO"
    codigo=${PIPESTATUS[0]}
    set -e
    escrever "=== $(basename "$s") — fim $(date +%T) — código de saída $codigo"
    if [[ $codigo -ne 0 ]]; then
        escrever "ERRO: execução interrompida em $(basename "$s"). Registo: $REGISTO"
        exit "$codigo"
    fi
done

escrever ""
escrever "Concluído sem erros. Registo: $REGISTO"
