# Scripts da base de dados m4-v03

O script [01-migracoes-idempotentes.sql](01-migracoes-idempotentes.sql) foi gerado pelo EF Core 10.0.13. O cabeçalho ativa `QUOTED_IDENTIFIER` para a coluna calculada persistida de `RegistoProfilaxia`. Aplica as migrações em falta numa base de dados vazia ou já gerida pelas migrações deste projeto. A tabela `__EFMigrationsHistory` indica as que já foram aplicadas.

A migração `20261113092251_ObservacaoEMotivoBloqueio` acrescenta `Utilizador.MotivoBloqueio` e `Marcacao.Observacao` como `NVARCHAR(500) NULL`. Os registos existentes ficam com `NULL` nestes campos. As colunas estão descritas no [Modelo de Dados v03](../../04.06-modelos-de-dados/m4-modelo-de-dados-clinica-veterinaria-v03.md).

Para uma instalação nova, criar a base de dados com a collation `Latin1_General_100_CI_AS` definida no modelo do EF Core. Executar a partir desta pasta, indicando a instância e a base de dados de destino já criada.

```sh
sqlcmd -S '<servidor>' -d '<base-de-dados>' -U '<utilizador>' -b -i 01-migracoes-idempotentes.sql
```

O script inclui os dados de referência das migrações. Os [dados de demonstração](../m3-v02/dados-demonstracao/02-dados-demonstracao.sql) são carregados à parte, depois das migrações.
