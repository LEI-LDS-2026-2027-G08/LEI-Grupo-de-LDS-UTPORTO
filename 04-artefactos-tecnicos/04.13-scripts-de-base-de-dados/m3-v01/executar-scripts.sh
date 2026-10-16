#!/usr/bin/env bash
# =============================================================================
# Projeto        : Gestão de Clínica Veterinária — Grupo G08 — LDS 2026/2027
# Issue          : I060 — Escrever o script de criação do esquema da base de dados
# Ficheiro       : executar-scripts.sh
# Versão         : m3-v01
# Finalidade     : Executar, por ordem numérica, os scripts 00-*.sql a 08-*.sql
#                  (Linux, macOS ou pipeline GitHub Actions), parar no primeiro
#                  erro e guardar o registo da execução (evidência da I060).
# Configuração   : Variáveis de ambiente (nenhum segredo fica em ficheiros):
#                    SQLCMD_SERVIDOR    instância (por omissão: localhost)
#                    SQLCMD_UTILIZADOR  login SQL Server (por omissão: sa)
#                    SQLCMDPASSWORD     palavra-passe do login (obrigatória)
# Opções         : --verificar             executa verificacao/verificar-esquema.sql
#                  --testar                executa verificacao/testar-restricoes.sql
#                  --confiar-certificado   acrescenta -C (sqlcmd 18, certificado autoassinado)
#                  --pasta-registos DIR    pasta do registo (por omissão: pasta atual)
# Exemplo        : SQLCMDPASSWORD='...' ./executar-scripts.sh --confiar-certificado --verificar --testar
# =============================================================================
set -euo pipefail

SERVIDOR="${SQLCMD_SERVIDOR:-localhost}"
UTILIZADOR="${SQLCMD_UTILIZADOR:-sa}"
VERIFICAR=0
TESTAR=0
CERTIFICADO=()
PASTA_REGISTOS="$(pwd)"

while [[ $# -gt 0 ]]; do
    case "$1" in
        --verificar)           VERIFICAR=1 ;;
        --testar)              TESTAR=1 ;;
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
SCRIPTS=( "$PASTA"/[0-9][0-9]-*.sql )
[[ $VERIFICAR -eq 1 ]] && SCRIPTS+=( "$PASTA/verificacao/verificar-esquema.sql" )
[[ $TESTAR -eq 1 ]]    && SCRIPTS+=( "$PASTA/verificacao/testar-restricoes.sql" )

mkdir -p "$PASTA_REGISTOS"
REGISTO="$PASTA_REGISTOS/i060-execucao-$(date +%Y%m%d-%H%M%S).log"
escrever() { printf '%s\n' "$*" | tee -a "$REGISTO"; }

escrever "I060 — Execução dos scripts do esquema da base de dados ClinicaVeterinaria"
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
