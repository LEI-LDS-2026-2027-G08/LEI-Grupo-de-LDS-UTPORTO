# I137 — Regressões de portabilidade e arquivo

Data: 25/11/2026. Executados no macOS. Os ramos Windows usam processos simulados; não é evidência de execução completa da aplicação em Windows.

Comando na raiz: `python3 -m unittest discover -s src/frontend-mobile/tests/sistema -p "test_*.py" -v`.

```text
test_preserva_uma_edicao_manual_no_destino (test_arquivar.ArquivoTests.test_preserva_uma_edicao_manual_no_destino) ... ok
test_recusa_versao_do_commit_diferente_da_testada (test_arquivar.ArquivoTests.test_recusa_versao_do_commit_diferente_da_testada) ... ok
test_substitui_so_uma_copia_que_corresponde_ao_manifesto_anterior (test_arquivar.ArquivoTests.test_substitui_so_uma_copia_que_corresponde_ao_manifesto_anterior) ... ok
test_falha_ao_fechar_api_nao_impede_tuneis_sql_e_relatorio (test_executar.OrquestradorTests.test_falha_ao_fechar_api_nao_impede_tuneis_sql_e_relatorio) ... ok
test_macos_e_linux_mantem_as_pastas_respetivas (test_executar.OrquestradorTests.test_macos_e_linux_mantem_as_pastas_respetivas) ... ok
test_posix_termina_grupo_e_forca_apenas_se_exceder_o_prazo (test_executar.OrquestradorTests.test_posix_termina_grupo_e_forca_apenas_se_exceder_o_prazo) ... ok
test_processo_ja_terminado_nao_e_morto_novamente (test_executar.OrquestradorTests.test_processo_ja_terminado_nao_e_morto_novamente) ... ok
test_sdk_configurado_tem_prioridade_sobre_a_pasta_predefinida (test_executar.OrquestradorTests.test_sdk_configurado_tem_prioridade_sobre_a_pasta_predefinida) ... ok
test_sdk_root_tambem_e_respeitado (test_executar.OrquestradorTests.test_sdk_root_tambem_e_respeitado) ... ok
test_sem_tzdata_falha_antes_de_criar_recursos_com_instrucao_util (test_executar.OrquestradorTests.test_sem_tzdata_falha_antes_de_criar_recursos_com_instrucao_util) ... ok
test_windows_descobre_sdk_e_executaveis_com_espacos_no_caminho (test_executar.OrquestradorTests.test_windows_descobre_sdk_e_executaveis_com_espacos_no_caminho) ... ok
test_windows_invoca_o_bat_e_nao_publica_a_senha_no_registo (test_executar.OrquestradorTests.test_windows_invoca_o_bat_e_nao_publica_a_senha_no_registo) ... ok
test_windows_taskkill_falhado_e_reportado (test_executar.OrquestradorTests.test_windows_taskkill_falhado_e_reportado) ... ok
test_windows_termina_so_a_arvore_do_pid_criado (test_executar.OrquestradorTests.test_windows_termina_so_a_arvore_do_pid_criado) ... ok

----------------------------------------------------------------------
Ran 14 tests in 0.033s

OK
```

Também foi verificada a terminação de um grupo de processos POSIX real, criado exclusivamente para este ensaio.
