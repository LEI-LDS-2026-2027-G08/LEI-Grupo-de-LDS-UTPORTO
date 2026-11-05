``````text
﻿2026-11-04T18:55:38.0962660Z Current runner version: '2.338.2'
2026-11-04T18:55:38.0983006Z ##[group]Runner Image Provisioner
2026-11-04T18:55:38.0983849Z Hosted Compute Agent
2026-11-04T18:55:38.0984383Z Version: 20261021.618
2026-11-04T18:55:38.0984922Z Commit: 21505ad2541d5be6135ed84fc33697146cf75b8e
2026-11-04T18:55:38.0985594Z Build Date: 2026-10-21T00:45:21Z
2026-11-04T18:55:38.0986208Z Worker ID: {57e889fb-0703-4487-b5b1-a589e5d7db60}
2026-11-04T18:55:38.0986805Z Region: westus
2026-11-04T18:55:38.0987334Z Cloud: Azure
2026-11-04T18:55:38.0987814Z ##[endgroup]
2026-11-04T18:55:38.0988980Z ##[group]Operating System
2026-11-04T18:55:38.0989604Z Ubuntu
2026-11-04T18:55:38.0990042Z 24.04.5
2026-11-04T18:55:38.0990540Z LTS
2026-11-04T18:55:38.0990994Z ##[endgroup]
2026-11-04T18:55:38.0991484Z ##[group]Runner Image
2026-11-04T18:55:38.0992034Z Image: ubuntu-24.04
2026-11-04T18:55:38.0992517Z Version: 20261025.353.1
2026-11-04T18:55:38.0993832Z Included Software: https://github.com/actions/runner-images/blob/ubuntu24/20261025.353/images/ubuntu/Ubuntu2404-Readme.md
2026-11-04T18:55:38.0995104Z Image Release: https://github.com/actions/runner-images/releases/tag/ubuntu24%2F20261025.353
2026-11-04T18:55:38.0995948Z ##[endgroup]
2026-11-04T18:55:38.0996916Z ##[group]GITHUB_TOKEN Permissions
2026-11-04T18:55:38.0998443Z Contents: read
2026-11-04T18:55:38.0998946Z Metadata: read
2026-11-04T18:55:38.0999493Z ##[endgroup]
2026-11-04T18:55:38.1001149Z Secret source: Actions
2026-11-04T18:55:38.1002021Z Cache mode: write
2026-11-04T18:55:38.1002606Z Prepare workflow directory
2026-11-04T18:55:38.1319790Z Prepare all required actions
2026-11-04T18:55:38.1357647Z Getting action download info
2026-11-04T18:55:38.4968265Z Download action repository 'actions/checkout@3d3c42e5aac5ba805825da76410c181273ba90b1' (SHA:3d3c42e5aac5ba805825da76410c181273ba90b1)
2026-11-04T18:55:38.6190014Z Download action repository 'actions/upload-artifact@cf430e030ddbb5b0abf93d22962f4752f3646cd9' (SHA:cf430e030ddbb5b0abf93d22962f4752f3646cd9)
2026-11-04T18:55:38.8538905Z Complete job name: 7 · Collection Postman (Newman)
2026-11-04T18:55:38.9245057Z ##[group]Run actions/checkout@3d3c42e5aac5ba805825da76410c181273ba90b1
2026-11-04T18:55:38.9245847Z with:
2026-11-04T18:55:38.9246213Z   persist-credentials: false
2026-11-04T18:55:38.9246720Z   repository: LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:55:38.9249727Z   token: ***
2026-11-04T18:55:38.9250085Z   ssh-strict: true
2026-11-04T18:55:38.9250446Z   ssh-user: git
2026-11-04T18:55:38.9250787Z   clean: true
2026-11-04T18:55:38.9251145Z   sparse-checkout-cone-mode: true
2026-11-04T18:55:38.9251572Z   fetch-depth: 1
2026-11-04T18:55:38.9251932Z   fetch-tags: false
2026-11-04T18:55:38.9252304Z   show-progress: true
2026-11-04T18:55:38.9252661Z   lfs: false
2026-11-04T18:55:38.9253127Z   submodules: false
2026-11-04T18:55:38.9253497Z   set-safe-directory: true
2026-11-04T18:55:38.9253908Z   allow-unsafe-pr-checkout: false
2026-11-04T18:55:38.9254459Z env:
2026-11-04T18:55:38.9254790Z   DOTNET_NOLOGO: true
2026-11-04T18:55:38.9255238Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:38.9255668Z   CONFIGURACAO: Release
2026-11-04T18:55:38.9256080Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:38.9256816Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:38.9257969Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:38.9259173Z   RESULTADOS: resultados-ci
2026-11-04T18:55:38.9259579Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:38.9260029Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:38.9260518Z   BASE_DADOS: ClinicaVeterinaria_CI
2026-11-04T18:55:38.9260974Z   API_URL: http://localhost:5159
2026-11-04T18:55:38.9261420Z   CONTENTOR_SQL: sql-newman-ci
2026-11-04T18:55:38.9261947Z   IMAGEM_SQL: mcr.microsoft.com/mssql/server:2025-CU9-ubuntu-22.04
2026-11-04T18:55:38.9262507Z   DOTNET_EF_VERSAO: 10.0.13
2026-11-04T18:55:38.9263301Z   PROJETO_API: src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj
2026-11-04T18:55:38.9264433Z   PROJETO_INFRA: src/backend/ClinicaVeterinaria.Infrastructure/ClinicaVeterinaria.Infrastructure.csproj
2026-11-04T18:55:38.9265170Z ##[endgroup]
2026-11-04T18:55:39.4149320Z Syncing repository: LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:55:39.4151409Z ##[group]Getting Git version info
2026-11-04T18:55:39.4152533Z Working directory is '/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO'
2026-11-04T18:55:39.4154189Z [command]/usr/bin/git version
2026-11-04T18:55:39.4209379Z git version 2.56.0
2026-11-04T18:55:39.4225912Z ##[endgroup]
2026-11-04T18:55:39.4235414Z Temporarily overriding HOME='/home/runner/work/_temp/250e3985-5bd1-49ed-bd66-ebe8a348e3e2' before making global git config changes
2026-11-04T18:55:39.4237256Z Adding repository directory to the temporary git global config as a safe directory
2026-11-04T18:55:39.4240545Z [command]/usr/bin/git config --global --add safe.directory /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:55:39.4281615Z Deleting the contents of '/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO'
2026-11-04T18:55:39.4284549Z ##[group]Determining repository object format
2026-11-04T18:55:39.4285899Z ##[endgroup]
2026-11-04T18:55:39.4286819Z ##[group]Initializing the repository
2026-11-04T18:55:39.4290298Z [command]/usr/bin/git init /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:55:39.4362598Z hint: Using 'master' as the name for the initial branch. This default branch name
2026-11-04T18:55:39.4363630Z hint: will change to "main" in Git 3.0. To configure the initial branch name
2026-11-04T18:55:39.4364542Z hint: to use in all of your new repositories, which will suppress this warning,
2026-11-04T18:55:39.4365249Z hint: call:
2026-11-04T18:55:39.4365608Z hint:
2026-11-04T18:55:39.4366041Z hint: 	git config --global init.defaultBranch <name>
2026-11-04T18:55:39.4366796Z hint:
2026-11-04T18:55:39.4369597Z hint: Names commonly chosen instead of 'master' are 'main', 'trunk' and
2026-11-04T18:55:39.4370481Z hint: 'development'. The just-created branch can be renamed via this command:
2026-11-04T18:55:39.4371081Z hint:
2026-11-04T18:55:39.4371429Z hint: 	git branch -m <name>
2026-11-04T18:55:39.4371844Z hint:
2026-11-04T18:55:39.4381300Z hint: Disable this message with "git config set advice.defaultBranchName false"
2026-11-04T18:55:39.4383301Z Initialized empty Git repository in /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/
2026-11-04T18:55:39.4386231Z [command]/usr/bin/git remote add origin https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:55:39.4434730Z ##[endgroup]
2026-11-04T18:55:39.4435646Z ##[group]Disabling automatic garbage collection
2026-11-04T18:55:39.4436484Z [command]/usr/bin/git config --local gc.auto 0
2026-11-04T18:55:39.4474190Z ##[endgroup]
2026-11-04T18:55:39.4475123Z ##[group]Setting up auth
2026-11-04T18:55:39.4475845Z Removing SSH command configuration
2026-11-04T18:55:39.4479442Z [command]/usr/bin/git config --local --name-only --get-regexp core\.sshCommand
2026-11-04T18:55:39.4510240Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'core\.sshCommand' && git config --local --unset-all 'core.sshCommand' || :"
2026-11-04T18:55:39.4820197Z Removing HTTP extra header
2026-11-04T18:55:39.4825638Z [command]/usr/bin/git config --local --name-only --get-regexp http\.https\:\/\/github\.com\/\.extraheader
2026-11-04T18:55:39.4866011Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'http\.https\:\/\/github\.com\/\.extraheader' && git config --local --unset-all 'http.https://github.com/.extraheader' || :"
2026-11-04T18:55:39.5073472Z Removing includeIf entries pointing to credentials config files
2026-11-04T18:55:39.5077894Z [command]/usr/bin/git config --local --name-only --get-regexp ^includeIf\.gitdir:
2026-11-04T18:55:39.5114499Z [command]/usr/bin/git submodule foreach --recursive git config --local --show-origin --name-only --get-regexp remote.origin.url
2026-11-04T18:55:39.5394974Z [command]/usr/bin/git config --file /home/runner/work/_temp/git-credentials-e0f0e673-b6ff-472e-b3b5-5f2a0b89e36a.config http.https://github.com/.extraheader AUTHORIZATION: basic ***
2026-11-04T18:55:39.5399520Z [command]/usr/bin/git config --local includeIf.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path /home/runner/work/_temp/git-credentials-e0f0e673-b6ff-472e-b3b5-5f2a0b89e36a.config
2026-11-04T18:55:39.5408452Z [command]/usr/bin/git config --local includeIf.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path /home/runner/work/_temp/git-credentials-e0f0e673-b6ff-472e-b3b5-5f2a0b89e36a.config
2026-11-04T18:55:39.5445627Z [command]/usr/bin/git config --local includeIf.gitdir:/github/workspace/.git.path /github/runner_temp/git-credentials-e0f0e673-b6ff-472e-b3b5-5f2a0b89e36a.config
2026-11-04T18:55:39.5481575Z [command]/usr/bin/git config --local includeIf.gitdir:/github/workspace/.git/worktrees/*.path /github/runner_temp/git-credentials-e0f0e673-b6ff-472e-b3b5-5f2a0b89e36a.config
2026-11-04T18:55:39.5516055Z ##[endgroup]
2026-11-04T18:55:39.5516679Z ##[group]Fetching the repository
2026-11-04T18:55:39.5521416Z [command]/usr/bin/git -c protocol.version=2 fetch --no-tags --prune --no-recurse-submodules --depth=1 origin +904ebe1bb41fb9038081be81330a0758f3f6bae5:refs/remotes/pull/249/merge
2026-11-04T18:55:41.3935117Z From https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:55:41.3936727Z  * [new ref]         904ebe1bb41fb9038081be81330a0758f3f6bae5 -> pull/249/merge
2026-11-04T18:55:41.3940102Z ##[endgroup]
2026-11-04T18:55:41.3942205Z ##[group]Determining the checkout info
2026-11-04T18:55:41.3943389Z ##[endgroup]
2026-11-04T18:55:41.3963879Z [command]/usr/bin/git sparse-checkout disable
2026-11-04T18:55:41.4002375Z [command]/usr/bin/git config --local --unset-all extensions.worktreeConfig
2026-11-04T18:55:41.4040959Z ##[group]Checking out the ref
2026-11-04T18:55:41.4043644Z [command]/usr/bin/git checkout --progress --force refs/remotes/pull/249/merge
2026-11-04T18:55:41.5710394Z Note: switching to 'refs/remotes/pull/249/merge'.
2026-11-04T18:55:41.5711222Z 
2026-11-04T18:55:41.5711796Z You are in 'detached HEAD' state. You can look around, make experimental
2026-11-04T18:55:41.5713332Z changes and commit them, and you can discard any commits you make in this
2026-11-04T18:55:41.5714747Z state without impacting any branches by switching back to a branch.
2026-11-04T18:55:41.5715562Z 
2026-11-04T18:55:41.5716154Z If you want to create a new branch to retain commits you create, you may
2026-11-04T18:55:41.5717473Z do so (now or later) by using -c with the switch command. Example:
2026-11-04T18:55:41.5718276Z 
2026-11-04T18:55:41.5718642Z   git switch -c <new-branch-name>
2026-11-04T18:55:41.5719219Z 
2026-11-04T18:55:41.5719574Z Or undo this operation with:
2026-11-04T18:55:41.5720191Z 
2026-11-04T18:55:41.5720516Z   git switch -
2026-11-04T18:55:41.5720972Z 
2026-11-04T18:55:41.5739498Z Turn off this advice by setting config variable advice.detachedHead to false
2026-11-04T18:55:41.5740609Z 
2026-11-04T18:55:41.5741752Z HEAD is now at 904ebe1 Merge 6852c4461708be065c45ec293936ef6fea1e4a53 into 89d6b31c5e788b4e8e5cfc7178d991ef65856c06
2026-11-04T18:55:41.5744922Z ##[endgroup]
2026-11-04T18:55:41.5791785Z [command]/usr/bin/git log -1 --format=%H
2026-11-04T18:55:41.5824201Z 904ebe1bb41fb9038081be81330a0758f3f6bae5
2026-11-04T18:55:41.5831838Z ##[group]Removing auth
2026-11-04T18:55:41.5832965Z Removing SSH command configuration
2026-11-04T18:55:41.5834892Z [command]/usr/bin/git config --local --name-only --get-regexp core\.sshCommand
2026-11-04T18:55:41.5871459Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'core\.sshCommand' && git config --local --unset-all 'core.sshCommand' || :"
2026-11-04T18:55:41.6089751Z Removing HTTP extra header
2026-11-04T18:55:41.6091967Z [command]/usr/bin/git config --local --name-only --get-regexp http\.https\:\/\/github\.com\/\.extraheader
2026-11-04T18:55:41.6130775Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'http\.https\:\/\/github\.com\/\.extraheader' && git config --local --unset-all 'http.https://github.com/.extraheader' || :"
2026-11-04T18:55:41.6353630Z Removing includeIf entries pointing to credentials config files
2026-11-04T18:55:41.6356284Z [command]/usr/bin/git config --local --name-only --get-regexp ^includeIf\.gitdir:
2026-11-04T18:55:41.6392292Z includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path
2026-11-04T18:55:41.6396214Z includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path
2026-11-04T18:55:41.6398599Z includeif.gitdir:/github/workspace/.git.path
2026-11-04T18:55:41.6400508Z includeif.gitdir:/github/workspace/.git/worktrees/*.path
2026-11-04T18:55:41.6405967Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path
2026-11-04T18:55:41.6435124Z /home/runner/work/_temp/git-credentials-e0f0e673-b6ff-472e-b3b5-5f2a0b89e36a.config
2026-11-04T18:55:41.6453894Z [command]/usr/bin/git config --local --unset includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path \/home\/runner\/work\/_temp\/git\-credentials\-e0f0e673\-b6ff\-472e\-b3b5\-5f2a0b89e36a\.config
2026-11-04T18:55:41.6503343Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path
2026-11-04T18:55:41.6541123Z /home/runner/work/_temp/git-credentials-e0f0e673-b6ff-472e-b3b5-5f2a0b89e36a.config
2026-11-04T18:55:41.6550559Z [command]/usr/bin/git config --local --unset includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path \/home\/runner\/work\/_temp\/git\-credentials\-e0f0e673\-b6ff\-472e\-b3b5\-5f2a0b89e36a\.config
2026-11-04T18:55:41.6591748Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/github/workspace/.git.path
2026-11-04T18:55:41.6623295Z /github/runner_temp/git-credentials-e0f0e673-b6ff-472e-b3b5-5f2a0b89e36a.config
2026-11-04T18:55:41.6631527Z [command]/usr/bin/git config --local --unset includeif.gitdir:/github/workspace/.git.path \/github\/runner_temp\/git\-credentials\-e0f0e673\-b6ff\-472e\-b3b5\-5f2a0b89e36a\.config
2026-11-04T18:55:41.6672476Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/github/workspace/.git/worktrees/*.path
2026-11-04T18:55:41.6705578Z /github/runner_temp/git-credentials-e0f0e673-b6ff-472e-b3b5-5f2a0b89e36a.config
2026-11-04T18:55:41.6726922Z [command]/usr/bin/git config --local --unset includeif.gitdir:/github/workspace/.git/worktrees/*.path \/github\/runner_temp\/git\-credentials\-e0f0e673\-b6ff\-472e\-b3b5\-5f2a0b89e36a\.config
2026-11-04T18:55:41.6757366Z [command]/usr/bin/git submodule foreach --recursive git config --local --show-origin --name-only --get-regexp remote.origin.url
2026-11-04T18:55:41.6977756Z Removing credentials config '/home/runner/work/_temp/git-credentials-e0f0e673-b6ff-472e-b3b5-5f2a0b89e36a.config'
2026-11-04T18:55:41.6985301Z ##[endgroup]
2026-11-04T18:55:41.7287988Z Prepare all required actions
2026-11-04T18:55:41.7289444Z Getting action download info
2026-11-04T18:55:41.8766746Z Download action repository 'actions/setup-dotnet@a98b56852c35b8e3190ac28c8c2271da59106c68' (SHA:a98b56852c35b8e3190ac28c8c2271da59106c68)
2026-11-04T18:55:42.2945056Z Download action repository 'actions/cache@55cc8345863c7cc4c66a329aec7e433d2d1c52a9' (SHA:55cc8345863c7cc4c66a329aec7e433d2d1c52a9)
2026-11-04T18:55:42.5103241Z ##[group]Run ./.github/actions/preparar-dotnet
2026-11-04T18:55:42.5103564Z env:
2026-11-04T18:55:42.5103776Z   DOTNET_NOLOGO: true
2026-11-04T18:55:42.5104017Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:42.5104460Z   CONFIGURACAO: Release
2026-11-04T18:55:42.5104715Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:42.5105163Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:42.5105866Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:42.5106346Z   RESULTADOS: resultados-ci
2026-11-04T18:55:42.5106599Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:42.5106883Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:42.5107165Z   BASE_DADOS: ClinicaVeterinaria_CI
2026-11-04T18:55:42.5107444Z   API_URL: http://localhost:5159
2026-11-04T18:55:42.5107731Z   CONTENTOR_SQL: sql-newman-ci
2026-11-04T18:55:42.5108060Z   IMAGEM_SQL: mcr.microsoft.com/mssql/server:2025-CU9-ubuntu-22.04
2026-11-04T18:55:42.5108388Z   DOTNET_EF_VERSAO: 10.0.13
2026-11-04T18:55:42.5108739Z   PROJETO_API: src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj
2026-11-04T18:55:42.5109304Z   PROJETO_INFRA: src/backend/ClinicaVeterinaria.Infrastructure/ClinicaVeterinaria.Infrastructure.csproj
2026-11-04T18:55:42.5109755Z ##[endgroup]
2026-11-04T18:55:42.5155925Z ##[start-action display=Instalar o SDK do .NET 10;id=__self.__actions_setup-dotnet]
2026-11-04T18:55:42.5216322Z ##[group]Run actions/setup-dotnet@a98b56852c35b8e3190ac28c8c2271da59106c68
2026-11-04T18:55:42.5216683Z with:
2026-11-04T18:55:42.5216896Z   dotnet-version: 10.0.x
2026-11-04T18:55:42.5217131Z   cache: false
2026-11-04T18:55:42.5217340Z env:
2026-11-04T18:55:42.5217548Z   DOTNET_NOLOGO: true
2026-11-04T18:55:42.5217797Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:42.5218074Z   CONFIGURACAO: Release
2026-11-04T18:55:42.5218343Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:42.5218813Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:42.5219441Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:42.5219927Z   RESULTADOS: resultados-ci
2026-11-04T18:55:42.5220178Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:42.5220455Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:42.5220733Z   BASE_DADOS: ClinicaVeterinaria_CI
2026-11-04T18:55:42.5221015Z   API_URL: http://localhost:5159
2026-11-04T18:55:42.5221272Z   CONTENTOR_SQL: sql-newman-ci
2026-11-04T18:55:42.5221580Z   IMAGEM_SQL: mcr.microsoft.com/mssql/server:2025-CU9-ubuntu-22.04
2026-11-04T18:55:42.5221902Z   DOTNET_EF_VERSAO: 10.0.13
2026-11-04T18:55:42.5222243Z   PROJETO_API: src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj
2026-11-04T18:55:42.5222898Z   PROJETO_INFRA: src/backend/ClinicaVeterinaria.Infrastructure/ClinicaVeterinaria.Infrastructure.csproj
2026-11-04T18:55:42.5223347Z ##[endgroup]
2026-11-04T18:55:42.6355115Z [command]/home/runner/work/_actions/actions/setup-dotnet/a98b56852c35b8e3190ac28c8c2271da59106c68/externals/install-dotnet.sh --skip-non-versioned-files --runtime dotnet --channel LTS
2026-11-04T18:55:42.7797581Z dotnet-install: .NET Core Runtime with version '10.0.14' is already installed.
2026-11-04T18:55:42.7818258Z [command]/home/runner/work/_actions/actions/setup-dotnet/a98b56852c35b8e3190ac28c8c2271da59106c68/externals/install-dotnet.sh --skip-non-versioned-files --channel 10.0
2026-11-04T18:55:42.9136262Z dotnet-install: .NET Core SDK with version '10.0.403' is already installed.
2026-11-04T18:55:42.9255346Z ##[end-action id=__self.__actions_setup-dotnet;outcome=success;conclusion=success;duration_ms=409]
2026-11-04T18:55:42.9258039Z ##[start-action display=Reutilizar a cache dos pacotes NuGet;id=__self.__actions_cache]
2026-11-04T18:55:43.0525956Z ##[group]Run actions/cache@55cc8345863c7cc4c66a329aec7e433d2d1c52a9
2026-11-04T18:55:43.0526304Z with:
2026-11-04T18:55:43.0526523Z   path: ~/.nuget/packages
2026-11-04T18:55:43.0526889Z   key: nuget-Linux-c8252acdcad9b687c3131c725ee3c1b10105ceb23478247a43e8389c12c19830
2026-11-04T18:55:43.0527449Z   restore-keys: nuget-Linux-

2026-11-04T18:55:43.0527707Z   enableCrossOsArchive: false
2026-11-04T18:55:43.0527955Z   fail-on-cache-miss: false
2026-11-04T18:55:43.0528194Z   lookup-only: false
2026-11-04T18:55:43.0528412Z   save-always: false
2026-11-04T18:55:43.0528617Z env:
2026-11-04T18:55:43.0528816Z   DOTNET_NOLOGO: true
2026-11-04T18:55:43.0529065Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:43.0529336Z   CONFIGURACAO: Release
2026-11-04T18:55:43.0529588Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:43.0530045Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:43.0530691Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:43.0531166Z   RESULTADOS: resultados-ci
2026-11-04T18:55:43.0531414Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:43.0531680Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:43.0531973Z   BASE_DADOS: ClinicaVeterinaria_CI
2026-11-04T18:55:43.0532264Z   API_URL: http://localhost:5159
2026-11-04T18:55:43.0532521Z   CONTENTOR_SQL: sql-newman-ci
2026-11-04T18:55:43.0532990Z   IMAGEM_SQL: mcr.microsoft.com/mssql/server:2025-CU9-ubuntu-22.04
2026-11-04T18:55:43.0533321Z   DOTNET_EF_VERSAO: 10.0.13
2026-11-04T18:55:43.0533676Z   PROJETO_API: src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj
2026-11-04T18:55:43.0534377Z   PROJETO_INFRA: src/backend/ClinicaVeterinaria.Infrastructure/ClinicaVeterinaria.Infrastructure.csproj
2026-11-04T18:55:43.0534837Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:55:43.0535084Z ##[endgroup]
2026-11-04T18:55:43.4375013Z Cache hit for: nuget-Linux-c8252acdcad9b687c3131c725ee3c1b10105ceb23478247a43e8389c12c19830
2026-11-04T18:55:44.7477873Z Received 20971520 of 283633656 (7.4%), 20.0 MBs/sec
2026-11-04T18:55:45.7521036Z Received 134217728 of 283633656 (47.3%), 64.0 MBs/sec
2026-11-04T18:55:46.7493328Z Received 268435456 of 283633656 (94.6%), 85.3 MBs/sec
2026-11-04T18:55:47.0368536Z Received 283633656 of 283633656 (100.0%), 82.2 MBs/sec
2026-11-04T18:55:47.0370771Z Cache Size: ~270 MB (283633656 B)
2026-11-04T18:55:47.0484824Z [command]/usr/bin/tar -xf /home/runner/work/_temp/0035f532-a98b-4738-836d-636305dd3808/cache.tzst -P -C /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO --use-compress-program unzstd
2026-11-04T18:55:48.0007880Z Cache restored successfully
2026-11-04T18:55:48.0242109Z Cache restored from key: nuget-Linux-c8252acdcad9b687c3131c725ee3c1b10105ceb23478247a43e8389c12c19830
2026-11-04T18:55:48.0334343Z ##[end-action id=__self.__actions_cache;outcome=success;conclusion=success;duration_ms=5107]
2026-11-04T18:55:48.0337249Z ##[start-action display=Registar o SDK utilizado;id=__self.__run]
2026-11-04T18:55:48.0372577Z ##[group]Run echo "SDK do .NET: $(dotnet --version)"
2026-11-04T18:55:48.0373153Z [36;1mecho "SDK do .NET: $(dotnet --version)"[0m
2026-11-04T18:55:48.0373409Z [36;1m{[0m
2026-11-04T18:55:48.0373630Z [36;1m  echo "### Ambiente .NET"[0m
2026-11-04T18:55:48.0373899Z [36;1m  echo ""[0m
2026-11-04T18:55:48.0374162Z [36;1m  echo "| Elemento | Valor |"[0m
2026-11-04T18:55:48.0374386Z [36;1m  echo "|---|---|"[0m
2026-11-04T18:55:48.0374632Z [36;1m  echo "| SDK do .NET | \`$(dotnet --version)\` |"[0m
2026-11-04T18:55:48.0375166Z [36;1m  echo "| global.json | \`$(tr -d '\n\r ' < src/backend/global.json)\` |"[0m
2026-11-04T18:55:48.0375542Z [36;1m  echo "| Runner | \`${RUNNER_OS} ${ImageOS:-} ${ImageVersion:-}\` |"[0m
2026-11-04T18:55:48.0375824Z [36;1m  echo ""[0m
2026-11-04T18:55:48.0376021Z [36;1m} >> "$GITHUB_STEP_SUMMARY"[0m
2026-11-04T18:55:48.0397383Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:55:48.0397665Z env:
2026-11-04T18:55:48.0397836Z   DOTNET_NOLOGO: true
2026-11-04T18:55:48.0398027Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:48.0398240Z   CONFIGURACAO: Release
2026-11-04T18:55:48.0398448Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:48.0399020Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:48.0399621Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:48.0400061Z   RESULTADOS: resultados-ci
2026-11-04T18:55:48.0400285Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:48.0400519Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:48.0400761Z   BASE_DADOS: ClinicaVeterinaria_CI
2026-11-04T18:55:48.0401012Z   API_URL: http://localhost:5159
2026-11-04T18:55:48.0401230Z   CONTENTOR_SQL: sql-newman-ci
2026-11-04T18:55:48.0401504Z   IMAGEM_SQL: mcr.microsoft.com/mssql/server:2025-CU9-ubuntu-22.04
2026-11-04T18:55:48.0401786Z   DOTNET_EF_VERSAO: 10.0.13
2026-11-04T18:55:48.0402086Z   PROJETO_API: src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj
2026-11-04T18:55:48.0402585Z   PROJETO_INFRA: src/backend/ClinicaVeterinaria.Infrastructure/ClinicaVeterinaria.Infrastructure.csproj
2026-11-04T18:55:48.0403317Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:55:48.0403530Z ##[endgroup]
2026-11-04T18:55:50.3467977Z SDK do .NET: 10.0.403
2026-11-04T18:55:50.4197865Z ##[end-action id=__self.__run;outcome=success;conclusion=success;duration_ms=2386]
2026-11-04T18:55:50.4231535Z ##[group]Run dotnet restore "$SOLUCAO"
2026-11-04T18:55:50.4231803Z [36;1mdotnet restore "$SOLUCAO"[0m
2026-11-04T18:55:50.4249375Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:55:50.4249652Z env:
2026-11-04T18:55:50.4249817Z   DOTNET_NOLOGO: true
2026-11-04T18:55:50.4250019Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:50.4250235Z   CONFIGURACAO: Release
2026-11-04T18:55:50.4250452Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:50.4250849Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:50.4251441Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:50.4251880Z   RESULTADOS: resultados-ci
2026-11-04T18:55:50.4252095Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:50.4252319Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:50.4252563Z   BASE_DADOS: ClinicaVeterinaria_CI
2026-11-04T18:55:50.4252969Z   API_URL: http://localhost:5159
2026-11-04T18:55:50.4253227Z   CONTENTOR_SQL: sql-newman-ci
2026-11-04T18:55:50.4253498Z   IMAGEM_SQL: mcr.microsoft.com/mssql/server:2025-CU9-ubuntu-22.04
2026-11-04T18:55:50.4253774Z   DOTNET_EF_VERSAO: 10.0.13
2026-11-04T18:55:50.4254084Z   PROJETO_API: src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj
2026-11-04T18:55:50.4254585Z   PROJETO_INFRA: src/backend/ClinicaVeterinaria.Infrastructure/ClinicaVeterinaria.Infrastructure.csproj
2026-11-04T18:55:50.4254983Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:55:50.4255263Z ##[endgroup]
2026-11-04T18:55:53.3568484Z   Determining projects to restore...
2026-11-04T18:55:55.7183985Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Infrastructure/ClinicaVeterinaria.Infrastructure.csproj (in 679 ms).
2026-11-04T18:55:55.7185754Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj (in 695 ms).
2026-11-04T18:55:55.7199338Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj (in 679 ms).
2026-11-04T18:55:55.7205678Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj (in 679 ms).
2026-11-04T18:55:55.7300709Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Domain/ClinicaVeterinaria.Domain.csproj (in 2 ms).
2026-11-04T18:55:55.7318339Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Application/ClinicaVeterinaria.Application.csproj (in 2 ms).
2026-11-04T18:55:55.7842411Z ##[group]Run dotnet build "$PROJETO_API" --configuration "$CONFIGURACAO" --no-restore
2026-11-04T18:55:55.7843138Z [36;1mdotnet build "$PROJETO_API" --configuration "$CONFIGURACAO" --no-restore[0m
2026-11-04T18:55:55.7860414Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:55:55.7860698Z env:
2026-11-04T18:55:55.7860863Z   DOTNET_NOLOGO: true
2026-11-04T18:55:55.7861056Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:55.7861280Z   CONFIGURACAO: Release
2026-11-04T18:55:55.7861487Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:55.7861885Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:55.7862478Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:55.7863093Z   RESULTADOS: resultados-ci
2026-11-04T18:55:55.7863324Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:55.7863580Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:55.7863849Z   BASE_DADOS: ClinicaVeterinaria_CI
2026-11-04T18:55:55.7864089Z   API_URL: http://localhost:5159
2026-11-04T18:55:55.7864301Z   CONTENTOR_SQL: sql-newman-ci
2026-11-04T18:55:55.7864572Z   IMAGEM_SQL: mcr.microsoft.com/mssql/server:2025-CU9-ubuntu-22.04
2026-11-04T18:55:55.7864861Z   DOTNET_EF_VERSAO: 10.0.13
2026-11-04T18:55:55.7865165Z   PROJETO_API: src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj
2026-11-04T18:55:55.7865668Z   PROJETO_INFRA: src/backend/ClinicaVeterinaria.Infrastructure/ClinicaVeterinaria.Infrastructure.csproj
2026-11-04T18:55:55.7866068Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:55:55.7866287Z ##[endgroup]
2026-11-04T18:56:01.5424451Z   ClinicaVeterinaria.Domain -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Domain/bin/Release/net10.0/ClinicaVeterinaria.Domain.dll
2026-11-04T18:56:02.8467973Z   ClinicaVeterinaria.Application -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Application/bin/Release/net10.0/ClinicaVeterinaria.Application.dll
2026-11-04T18:56:04.8000511Z   ClinicaVeterinaria.Infrastructure -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Infrastructure/bin/Release/net10.0/ClinicaVeterinaria.Infrastructure.dll
2026-11-04T18:56:07.7504564Z   ClinicaVeterinaria.Api -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Api/bin/Release/net10.0/ClinicaVeterinaria.Api.dll
2026-11-04T18:56:07.7700302Z 
2026-11-04T18:56:07.7713764Z Build succeeded.
2026-11-04T18:56:07.7714166Z     0 Warning(s)
2026-11-04T18:56:07.7714462Z     0 Error(s)
2026-11-04T18:56:07.7714615Z 
2026-11-04T18:56:07.7714853Z Time Elapsed 00:00:11.83
2026-11-04T18:56:07.7997976Z ##[group]Run dotnet tool install dotnet-ef --version "$DOTNET_EF_VERSAO" --tool-path .ferramentas
2026-11-04T18:56:07.7998826Z [36;1mdotnet tool install dotnet-ef --version "$DOTNET_EF_VERSAO" --tool-path .ferramentas[0m
2026-11-04T18:56:07.8016511Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:56:07.8016793Z env:
2026-11-04T18:56:07.8016963Z   DOTNET_NOLOGO: true
2026-11-04T18:56:07.8017158Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:56:07.8017381Z   CONFIGURACAO: Release
2026-11-04T18:56:07.8017596Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:56:07.8017997Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:56:07.8018587Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:56:07.8019019Z   RESULTADOS: resultados-ci
2026-11-04T18:56:07.8019234Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:56:07.8019493Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:56:07.8019877Z   BASE_DADOS: ClinicaVeterinaria_CI
2026-11-04T18:56:07.8020119Z   API_URL: http://localhost:5159
2026-11-04T18:56:07.8020349Z   CONTENTOR_SQL: sql-newman-ci
2026-11-04T18:56:07.8020628Z   IMAGEM_SQL: mcr.microsoft.com/mssql/server:2025-CU9-ubuntu-22.04
2026-11-04T18:56:07.8020912Z   DOTNET_EF_VERSAO: 10.0.13
2026-11-04T18:56:07.8021220Z   PROJETO_API: src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj
2026-11-04T18:56:07.8021736Z   PROJETO_INFRA: src/backend/ClinicaVeterinaria.Infrastructure/ClinicaVeterinaria.Infrastructure.csproj
2026-11-04T18:56:07.8022146Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:56:07.8022350Z ##[endgroup]
2026-11-04T18:56:09.3614800Z You can invoke the tool using the following command: dotnet-ef
2026-11-04T18:56:09.3615667Z Tool 'dotnet-ef' (version '10.0.13') was successfully installed.
2026-11-04T18:56:09.3742985Z ##[group]Run SA="Sa-$(openssl rand -hex 16)-Aa1!"
2026-11-04T18:56:09.3743450Z [36;1mSA="Sa-$(openssl rand -hex 16)-Aa1!"[0m
2026-11-04T18:56:09.3743753Z [36;1mJWT="$(openssl rand -base64 48 | tr -d '\n')"[0m
2026-11-04T18:56:09.3744012Z [36;1mecho "::add-mask::$SA"[0m
2026-11-04T18:56:09.3744227Z [36;1mecho "::add-mask::$JWT"[0m
2026-11-04T18:56:09.3744439Z [36;1m{[0m
2026-11-04T18:56:09.3744616Z [36;1m  echo "SQL_SA_PASSWORD=$SA"[0m
2026-11-04T18:56:09.3744844Z [36;1m  echo "SQLCMDPASSWORD=$SA"[0m
2026-11-04T18:56:09.3745060Z [36;1m  echo "Jwt__Key=$JWT"[0m
2026-11-04T18:56:09.3745264Z [36;1m} >> "$GITHUB_ENV"[0m
2026-11-04T18:56:09.3745483Z [36;1mmkdir -p "$RESULTADOS/newman"[0m
2026-11-04T18:56:09.3762536Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:56:09.3763371Z env:
2026-11-04T18:56:09.3763545Z   DOTNET_NOLOGO: true
2026-11-04T18:56:09.3763753Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:56:09.3764010Z   CONFIGURACAO: Release
2026-11-04T18:56:09.3764228Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:56:09.3764628Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:56:09.3765236Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:56:09.3765665Z   RESULTADOS: resultados-ci
2026-11-04T18:56:09.3765880Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:56:09.3766113Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:56:09.3766366Z   BASE_DADOS: ClinicaVeterinaria_CI
2026-11-04T18:56:09.3766601Z   API_URL: http://localhost:5159
2026-11-04T18:56:09.3766834Z   CONTENTOR_SQL: sql-newman-ci
2026-11-04T18:56:09.3767105Z   IMAGEM_SQL: mcr.microsoft.com/mssql/server:2025-CU9-ubuntu-22.04
2026-11-04T18:56:09.3767393Z   DOTNET_EF_VERSAO: 10.0.13
2026-11-04T18:56:09.3767693Z   PROJETO_API: src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj
2026-11-04T18:56:09.3768199Z   PROJETO_INFRA: src/backend/ClinicaVeterinaria.Infrastructure/ClinicaVeterinaria.Infrastructure.csproj
2026-11-04T18:56:09.3768602Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:56:09.3768819Z ##[endgroup]
2026-11-04T18:56:09.3913987Z ##[group]Run # O repositório e a pasta temporária do runner ficam visíveis no
2026-11-04T18:56:09.3914470Z [36;1m# O repositório e a pasta temporária do runner ficam visíveis no[0m
2026-11-04T18:56:09.3914857Z [36;1m# contentor (só leitura), nos mesmos caminhos, para o sqlcmd do[0m
2026-11-04T18:56:09.3915184Z [36;1m# contentor poder ler os scripts.[0m
2026-11-04T18:56:09.3915446Z [36;1mdocker run -d --name "$CONTENTOR_SQL" \[0m
2026-11-04T18:56:09.3915773Z [36;1m  -e ACCEPT_EULA=Y -e MSSQL_SA_PASSWORD="$SQL_SA_PASSWORD" \[0m
2026-11-04T18:56:09.3916062Z [36;1m  -p 1433:1433 \[0m
2026-11-04T18:56:09.3916304Z [36;1m  -v "$GITHUB_WORKSPACE:$GITHUB_WORKSPACE:ro" \[0m
2026-11-04T18:56:09.3916590Z [36;1m  -v "$RUNNER_TEMP:$RUNNER_TEMP:ro" \[0m
2026-11-04T18:56:09.3916828Z [36;1m  "$IMAGEM_SQL"[0m
2026-11-04T18:56:09.3917039Z [36;1mfor i in $(seq 1 60); do[0m
2026-11-04T18:56:09.3917473Z [36;1m  if docker logs "$CONTENTOR_SQL" 2>&1 | grep -q "SQL Server is now ready for client connections"; then[0m
2026-11-04T18:56:09.3918056Z [36;1m    echo "SQL Server pronto ao fim de $((i * 2)) s."[0m
2026-11-04T18:56:09.3918327Z [36;1m    exit 0[0m
2026-11-04T18:56:09.3918515Z [36;1m  fi[0m
2026-11-04T18:56:09.3918688Z [36;1m  sleep 2[0m
2026-11-04T18:56:09.3918873Z [36;1mdone[0m
2026-11-04T18:56:09.3919056Z [36;1mdocker logs "$CONTENTOR_SQL"[0m
2026-11-04T18:56:09.3919385Z [36;1mecho "::error title=Newman::O SQL Server não ficou disponível em 120 s."[0m
2026-11-04T18:56:09.3919698Z [36;1mexit 1[0m
2026-11-04T18:56:09.3935391Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:56:09.3935674Z env:
2026-11-04T18:56:09.3935844Z   DOTNET_NOLOGO: true
2026-11-04T18:56:09.3936051Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:56:09.3936275Z   CONFIGURACAO: Release
2026-11-04T18:56:09.3936500Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:56:09.3936921Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:56:09.3937514Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:56:09.3937957Z   RESULTADOS: resultados-ci
2026-11-04T18:56:09.3938172Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:56:09.3938410Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:56:09.3938652Z   BASE_DADOS: ClinicaVeterinaria_CI
2026-11-04T18:56:09.3938923Z   API_URL: http://localhost:5159
2026-11-04T18:56:09.3939198Z   CONTENTOR_SQL: sql-newman-ci
2026-11-04T18:56:09.3939481Z   IMAGEM_SQL: mcr.microsoft.com/mssql/server:2025-CU9-ubuntu-22.04
2026-11-04T18:56:09.3939772Z   DOTNET_EF_VERSAO: 10.0.13
2026-11-04T18:56:09.3940088Z   PROJETO_API: src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj
2026-11-04T18:56:09.3940600Z   PROJETO_INFRA: src/backend/ClinicaVeterinaria.Infrastructure/ClinicaVeterinaria.Infrastructure.csproj
2026-11-04T18:56:09.3941003Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:56:09.3941572Z   SQL_SA_PASSWORD: ***
2026-11-04T18:56:09.3941823Z   SQLCMDPASSWORD: ***
2026-11-04T18:56:09.3942138Z   Jwt__Key: ***
2026-11-04T18:56:09.3942320Z ##[endgroup]
2026-11-04T18:56:09.4632480Z Unable to find image 'mcr.microsoft.com/mssql/server:2025-CU9-ubuntu-22.04' locally
2026-11-04T18:56:09.6286453Z 2025-CU9-ubuntu-22.04: Pulling from mssql/server
2026-11-04T18:56:09.6287280Z cb5e374be662: Pulling fs layer
2026-11-04T18:56:09.6288064Z 97ae914ba1ec: Pulling fs layer
2026-11-04T18:56:09.6288678Z 66b18d851f2f: Pulling fs layer
2026-11-04T18:56:09.7746220Z cb5e374be662: Verifying Checksum
2026-11-04T18:56:09.7746666Z cb5e374be662: Download complete
2026-11-04T18:56:10.0008288Z 66b18d851f2f: Verifying Checksum
2026-11-04T18:56:10.0016727Z 66b18d851f2f: Download complete
2026-11-04T18:56:10.8791438Z cb5e374be662: Pull complete
2026-11-04T18:56:11.7503174Z 97ae914ba1ec: Verifying Checksum
2026-11-04T18:56:11.7503810Z 97ae914ba1ec: Download complete
2026-11-04T18:56:24.2309978Z 97ae914ba1ec: Pull complete
2026-11-04T18:56:27.7919755Z 66b18d851f2f: Pull complete
2026-11-04T18:56:27.7945490Z Digest: sha256:b61a2ce00b3b23307efeff8e16af07c964ef64270c3fa0109cb5802403cf9e98
2026-11-04T18:56:27.7959216Z Status: Downloaded newer image for mcr.microsoft.com/mssql/server:2025-CU9-ubuntu-22.04
2026-11-04T18:56:27.8110456Z 15704f90c63ff6bd74e26c1479a074cbfde43bfea4382850b4e4c6f727d5bfa7
2026-11-04T18:56:36.0583304Z SQL Server pronto ao fim de 10 s.
2026-11-04T18:56:36.0615845Z ##[group]Run NO_CONTENTOR="$(docker exec "$CONTENTOR_SQL" bash -c 'ls /opt/mssql-tools18/bin/sqlcmd /opt/mssql-tools/bin/sqlcmd 2>/dev/null | head -n 1' || true)"
2026-11-04T18:56:36.0617190Z [36;1mNO_CONTENTOR="$(docker exec "$CONTENTOR_SQL" bash -c 'ls /opt/mssql-tools18/bin/sqlcmd /opt/mssql-tools/bin/sqlcmd 2>/dev/null | head -n 1' || true)"[0m
2026-11-04T18:56:36.0617708Z [36;1mif [ -n "$NO_CONTENTOR" ]; then[0m
2026-11-04T18:56:36.0617997Z [36;1m  mkdir -p "$RUNNER_TEMP/bin"[0m
2026-11-04T18:56:36.0618472Z [36;1m  printf '#!/usr/bin/env bash\nexec docker exec -i -e SQLCMDPASSWORD "%s" "%s" "$@"\n' "$CONTENTOR_SQL" "$NO_CONTENTOR" > "$RUNNER_TEMP/bin/sqlcmd"[0m
2026-11-04T18:56:36.0619170Z [36;1m  chmod +x "$RUNNER_TEMP/bin/sqlcmd"[0m
2026-11-04T18:56:36.0619456Z [36;1m  echo "$RUNNER_TEMP/bin" >> "$GITHUB_PATH"[0m
2026-11-04T18:56:36.0619761Z [36;1m  echo "A usar o sqlcmd do contentor: $NO_CONTENTOR"[0m
2026-11-04T18:56:36.0620134Z [36;1melif [ -x /opt/mssql-tools18/bin/sqlcmd ]; then[0m
2026-11-04T18:56:36.0620436Z [36;1m  echo "/opt/mssql-tools18/bin" >> "$GITHUB_PATH"[0m
2026-11-04T18:56:36.0620769Z [36;1m  echo "A usar o sqlcmd do runner: /opt/mssql-tools18/bin/sqlcmd"[0m
2026-11-04T18:56:36.0621097Z [36;1melif command -v sqlcmd >/dev/null 2>&1; then[0m
2026-11-04T18:56:36.0621403Z [36;1m  echo "A usar o sqlcmd do runner: $(command -v sqlcmd)"[0m
2026-11-04T18:56:36.0621674Z [36;1melse[0m
2026-11-04T18:56:36.0622008Z [36;1m  echo "::error title=Newman::Não há sqlcmd no contentor do SQL Server nem no runner."[0m
2026-11-04T18:56:36.0622381Z [36;1m  exit 1[0m
2026-11-04T18:56:36.0622672Z [36;1mfi[0m
2026-11-04T18:56:36.0640657Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:56:36.0640943Z env:
2026-11-04T18:56:36.0641114Z   DOTNET_NOLOGO: true
2026-11-04T18:56:36.0641322Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:56:36.0641547Z   CONFIGURACAO: Release
2026-11-04T18:56:36.0641781Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:56:36.0642191Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:56:36.0642775Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:56:36.0643570Z   RESULTADOS: resultados-ci
2026-11-04T18:56:36.0643790Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:56:36.0644031Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:56:36.0644283Z   BASE_DADOS: ClinicaVeterinaria_CI
2026-11-04T18:56:36.0644552Z   API_URL: http://localhost:5159
2026-11-04T18:56:36.0644787Z   CONTENTOR_SQL: sql-newman-ci
2026-11-04T18:56:36.0645067Z   IMAGEM_SQL: mcr.microsoft.com/mssql/server:2025-CU9-ubuntu-22.04
2026-11-04T18:56:36.0645362Z   DOTNET_EF_VERSAO: 10.0.13
2026-11-04T18:56:36.0645680Z   PROJETO_API: src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj
2026-11-04T18:56:36.0646198Z   PROJETO_INFRA: src/backend/ClinicaVeterinaria.Infrastructure/ClinicaVeterinaria.Infrastructure.csproj
2026-11-04T18:56:36.0646617Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:56:36.0647245Z   SQL_SA_PASSWORD: ***
2026-11-04T18:56:36.0647500Z   SQLCMDPASSWORD: ***
2026-11-04T18:56:36.0647810Z   Jwt__Key: ***
2026-11-04T18:56:36.0647999Z ##[endgroup]
2026-11-04T18:56:36.1170098Z A usar o sqlcmd do contentor: /opt/mssql-tools18/bin/sqlcmd
2026-11-04T18:56:36.1200402Z ##[group]Run LIGACAO="Server=localhost,1433;Database=$BASE_DADOS;User Id=sa;***;TrustServerCertificate=True;Encrypt=True"
2026-11-04T18:56:36.1201151Z [36;1mLIGACAO="Server=localhost,1433;Database=$BASE_DADOS;User Id=sa;***;TrustServerCertificate=True;Encrypt=True"[0m
2026-11-04T18:56:36.1201653Z [36;1mecho "ConnectionStrings__ClinicaDb=$LIGACAO" >> "$GITHUB_ENV"[0m
2026-11-04T18:56:36.1202005Z [36;1m./.ferramentas/dotnet-ef database update \[0m
2026-11-04T18:56:36.1202282Z [36;1m  --project "$PROJETO_INFRA" \[0m
2026-11-04T18:56:36.1202545Z [36;1m  --startup-project "$PROJETO_API" \[0m
2026-11-04T18:56:36.1203009Z [36;1m  --context ClinicaDbContext \[0m
2026-11-04T18:56:36.1203380Z [36;1m  --configuration "$CONFIGURACAO" \[0m
2026-11-04T18:56:36.1203622Z [36;1m  --no-build \[0m
2026-11-04T18:56:36.1203825Z [36;1m  --connection "$LIGACAO"[0m
2026-11-04T18:56:36.1220977Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:56:36.1221256Z env:
2026-11-04T18:56:36.1221462Z   DOTNET_NOLOGO: true
2026-11-04T18:56:36.1221663Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:56:36.1221885Z   CONFIGURACAO: Release
2026-11-04T18:56:36.1222266Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:56:36.1222673Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:56:36.1223480Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:56:36.1223932Z   RESULTADOS: resultados-ci
2026-11-04T18:56:36.1224151Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:56:36.1224392Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:56:36.1224667Z   BASE_DADOS: ClinicaVeterinaria_CI
2026-11-04T18:56:36.1224902Z   API_URL: http://localhost:5159
2026-11-04T18:56:36.1225120Z   CONTENTOR_SQL: sql-newman-ci
2026-11-04T18:56:36.1225401Z   IMAGEM_SQL: mcr.microsoft.com/mssql/server:2025-CU9-ubuntu-22.04
2026-11-04T18:56:36.1225692Z   DOTNET_EF_VERSAO: 10.0.13
2026-11-04T18:56:36.1226012Z   PROJETO_API: src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj
2026-11-04T18:56:36.1226526Z   PROJETO_INFRA: src/backend/ClinicaVeterinaria.Infrastructure/ClinicaVeterinaria.Infrastructure.csproj
2026-11-04T18:56:36.1226943Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:56:36.1227236Z   SQL_SA_PASSWORD: ***
2026-11-04T18:56:36.1227493Z   SQLCMDPASSWORD: ***
2026-11-04T18:56:36.1227821Z   Jwt__Key: ***
2026-11-04T18:56:36.1228005Z ##[endgroup]
2026-11-04T18:56:40.5027356Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.5027917Z       Executed DbCommand (248ms) [Parameters=[], CommandType='Text', CommandTimeout='60']
2026-11-04T18:56:40.5028341Z       CREATE DATABASE [ClinicaVeterinaria_CI]
2026-11-04T18:56:40.5028617Z       COLLATE Latin1_General_100_CI_AS;
2026-11-04T18:56:40.5648041Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.5648966Z       Executed DbCommand (60ms) [Parameters=[], CommandType='Text', CommandTimeout='60']
2026-11-04T18:56:40.5649623Z       IF SERVERPROPERTY('EngineEdition') <> 5
2026-11-04T18:56:40.5650024Z       BEGIN
2026-11-04T18:56:40.5650516Z           ALTER DATABASE [ClinicaVeterinaria_CI] SET READ_COMMITTED_SNAPSHOT ON;
2026-11-04T18:56:40.5651072Z       END;
2026-11-04T18:56:40.6335369Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.6336215Z       Executed DbCommand (4ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.6336755Z       SELECT 1
2026-11-04T18:56:40.6356575Z info: Microsoft.EntityFrameworkCore.Migrations[20411]
2026-11-04T18:56:40.6357689Z       Acquiring an exclusive lock for migration application. See https://aka.ms/efcore-docs-migrations-lock for more information if this takes too long.
2026-11-04T18:56:40.6402237Z Acquiring an exclusive lock for migration application. See https://aka.ms/efcore-docs-migrations-lock for more information if this takes too long.
2026-11-04T18:56:40.6534384Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.6535221Z       Executed DbCommand (12ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.6535858Z       DECLARE @result int;
2026-11-04T18:56:40.6536610Z       EXEC @result = sp_getapplock @Resource = '__EFMigrationsLock', @LockOwner = 'Session', @LockMode = 'Exclusive';
2026-11-04T18:56:40.6537327Z       SELECT @result
2026-11-04T18:56:40.6944273Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.6945414Z       Executed DbCommand (5ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.6946327Z       IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
2026-11-04T18:56:40.6946995Z       BEGIN
2026-11-04T18:56:40.6947568Z           CREATE TABLE [__EFMigrationsHistory] (
2026-11-04T18:56:40.6947954Z               [MigrationId] nvarchar(150) NOT NULL,
2026-11-04T18:56:40.6948438Z               [ProductVersion] nvarchar(32) NOT NULL,
2026-11-04T18:56:40.6948966Z               CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
2026-11-04T18:56:40.6949404Z           );
2026-11-04T18:56:40.6949634Z       END;
2026-11-04T18:56:40.7007551Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7008053Z       Executed DbCommand (0ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7013958Z       SELECT 1
2026-11-04T18:56:40.7014427Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7015028Z       Executed DbCommand (0ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7015432Z       SELECT OBJECT_ID(N'[__EFMigrationsHistory]');
2026-11-04T18:56:40.7069999Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7070493Z       Executed DbCommand (4ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7070892Z       SELECT [MigrationId], [ProductVersion]
2026-11-04T18:56:40.7071190Z       FROM [__EFMigrationsHistory]
2026-11-04T18:56:40.7071467Z       ORDER BY [MigrationId];
2026-11-04T18:56:40.7153037Z info: Microsoft.EntityFrameworkCore.Migrations[20402]
2026-11-04T18:56:40.7153554Z       Applying migration '20261025165126_Inicial'.
2026-11-04T18:56:40.7166399Z Applying migration '20261025165126_Inicial'.
2026-11-04T18:56:40.7636913Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7637450Z       Executed DbCommand (2ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7637826Z       CREATE TABLE [Clinica] (
2026-11-04T18:56:40.7638062Z           [Id] tinyint NOT NULL,
2026-11-04T18:56:40.7638315Z           [Nome] nvarchar(150) NOT NULL,
2026-11-04T18:56:40.7638576Z           [NIF] char(9) NOT NULL,
2026-11-04T18:56:40.7638832Z           [Morada] nvarchar(250) NOT NULL,
2026-11-04T18:56:40.7639103Z           [Telefone] nvarchar(20) NOT NULL,
2026-11-04T18:56:40.7639361Z           [Email] nvarchar(256) NOT NULL,
2026-11-04T18:56:40.7639683Z           [DataUltimaAlteracao] datetime2(0) NOT NULL,
2026-11-04T18:56:40.7639997Z           CONSTRAINT [PK_Clinica] PRIMARY KEY ([Id]),
2026-11-04T18:56:40.7640338Z           CONSTRAINT [CK_Clinica_LinhaUnica] CHECK ([Id] = 1)
2026-11-04T18:56:40.7640609Z       );
2026-11-04T18:56:40.7650589Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7651293Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7651913Z       CREATE TABLE [ConfiguracaoSistema] (
2026-11-04T18:56:40.7652347Z           [Chave] nvarchar(100) NOT NULL,
2026-11-04T18:56:40.7652750Z           [Valor] nvarchar(500) NOT NULL,
2026-11-04T18:56:40.7653276Z           [Descricao] nvarchar(250) NULL,
2026-11-04T18:56:40.7653697Z           [DataUltimaAlteracao] datetime2(0) NOT NULL,
2026-11-04T18:56:40.7654204Z           CONSTRAINT [PK_ConfiguracaoSistema] PRIMARY KEY ([Chave])
2026-11-04T18:56:40.7654656Z       );
2026-11-04T18:56:40.7661602Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7662352Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7663075Z       CREATE TABLE [Especie] (
2026-11-04T18:56:40.7663427Z           [Id] int NOT NULL IDENTITY,
2026-11-04T18:56:40.7663991Z           [Nome] nvarchar(50) NOT NULL,
2026-11-04T18:56:40.7664406Z           CONSTRAINT [PK_Especie] PRIMARY KEY ([Id])
2026-11-04T18:56:40.7664778Z       );
2026-11-04T18:56:40.7675464Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7676116Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7676766Z       CREATE TABLE [HorarioFuncionamento] (
2026-11-04T18:56:40.7677187Z           [Id] int NOT NULL IDENTITY,
2026-11-04T18:56:40.7677574Z           [DiaSemana] tinyint NOT NULL,
2026-11-04T18:56:40.7677992Z           [HoraAbertura] time(0) NOT NULL,
2026-11-04T18:56:40.7678408Z           [HoraFecho] time(0) NOT NULL,
2026-11-04T18:56:40.7679062Z           [Ativo] bit NOT NULL CONSTRAINT [DF_HorarioFuncionamento_Ativo] DEFAULT CAST(1 AS bit),
2026-11-04T18:56:40.7679812Z           CONSTRAINT [PK_HorarioFuncionamento] PRIMARY KEY ([Id]),
2026-11-04T18:56:40.7680793Z           CONSTRAINT [CK_Horario_DiaSemana] CHECK ([DiaSemana] BETWEEN 1 AND 7),
2026-11-04T18:56:40.7681489Z           CONSTRAINT [CK_Horario_Intervalo] CHECK ([HoraFecho] > [HoraAbertura])
2026-11-04T18:56:40.7681982Z       );
2026-11-04T18:56:40.7690955Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7691681Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7692209Z       CREATE TABLE [Utilizador] (
2026-11-04T18:56:40.7692456Z           [Id] int NOT NULL IDENTITY,
2026-11-04T18:56:40.7692747Z           [Email] nvarchar(256) NOT NULL,
2026-11-04T18:56:40.7693452Z           [PasswordHash] nvarchar(256) NOT NULL,
2026-11-04T18:56:40.7705925Z           [Perfil] nvarchar(25) NOT NULL,
2026-11-04T18:56:40.7706652Z           [Estado] nvarchar(10) NOT NULL CONSTRAINT [DF_Utilizador_Estado] DEFAULT N'Ativo',
2026-11-04T18:56:40.7707584Z           [DataCriacao] datetime2(0) NOT NULL CONSTRAINT [DF_Utilizador_DataCriacao] DEFAULT (SYSUTCDATETIME()),
2026-11-04T18:56:40.7708152Z           [DataUltimoAcesso] datetime2(0) NULL,
2026-11-04T18:56:40.7708474Z           CONSTRAINT [PK_Utilizador] PRIMARY KEY ([Id]),
2026-11-04T18:56:40.7708870Z           CONSTRAINT [CK_Utilizador_Estado] CHECK ([Estado] IN ('Ativo','Bloqueado')),
2026-11-04T18:56:40.7709514Z           CONSTRAINT [CK_Utilizador_Perfil] CHECK ([Perfil] IN ('Administrador','Rececionista','Veterinario','AuxiliarVeterinario','Cliente'))
2026-11-04T18:56:40.7709994Z       );
2026-11-04T18:56:40.7712077Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7713051Z       Executed DbCommand (2ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7713504Z       CREATE TABLE [Raca] (
2026-11-04T18:56:40.7713857Z           [Id] int NOT NULL IDENTITY,
2026-11-04T18:56:40.7714109Z           [EspecieId] int NOT NULL,
2026-11-04T18:56:40.7714417Z           [Nome] nvarchar(80) NOT NULL,
2026-11-04T18:56:40.7714736Z           CONSTRAINT [PK_Raca] PRIMARY KEY ([Id]),
2026-11-04T18:56:40.7715112Z           CONSTRAINT [UQ_Raca_Especie_Id] UNIQUE ([EspecieId], [Id]),
2026-11-04T18:56:40.7715706Z           CONSTRAINT [FK_Raca_Especie_EspecieId] FOREIGN KEY ([EspecieId]) REFERENCES [Especie] ([Id]) ON DELETE NO ACTION
2026-11-04T18:56:40.7716157Z       );
2026-11-04T18:56:40.7730022Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7730753Z       Executed DbCommand (2ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7731350Z       CREATE TABLE [Cliente] (
2026-11-04T18:56:40.7731727Z           [Id] int NOT NULL IDENTITY,
2026-11-04T18:56:40.7732138Z           [UtilizadorId] int NOT NULL,
2026-11-04T18:56:40.7732961Z           [Nome] nvarchar(150) NOT NULL,
2026-11-04T18:56:40.7733406Z           [NIF] char(9) NULL,
2026-11-04T18:56:40.7733796Z           [Telefone] nvarchar(20) NOT NULL,
2026-11-04T18:56:40.7734219Z           [Morada] nvarchar(250) NULL,
2026-11-04T18:56:40.7734927Z           [DataRegisto] datetime2(0) NOT NULL CONSTRAINT [DF_Cliente_DataRegisto] DEFAULT (SYSUTCDATETIME()),
2026-11-04T18:56:40.7735653Z           CONSTRAINT [PK_Cliente] PRIMARY KEY ([Id]),
2026-11-04T18:56:40.7736559Z           CONSTRAINT [FK_Cliente_Utilizador_UtilizadorId] FOREIGN KEY ([UtilizadorId]) REFERENCES [Utilizador] ([Id]) ON DELETE NO ACTION
2026-11-04T18:56:40.7737307Z       );
2026-11-04T18:56:40.7743272Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7743991Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7744585Z       CREATE TABLE [Funcionario] (
2026-11-04T18:56:40.7744972Z           [UtilizadorId] int NOT NULL,
2026-11-04T18:56:40.7745550Z           [Nome] nvarchar(150) NOT NULL,
2026-11-04T18:56:40.7745956Z           [Telefone] nvarchar(20) NULL,
2026-11-04T18:56:40.7746820Z           [PodeRegistarApoio] bit NOT NULL CONSTRAINT [DF_Funcionario_PodeRegistarApoio] DEFAULT CAST(0 AS bit),
2026-11-04T18:56:40.7747934Z           CONSTRAINT [PK_Funcionario] PRIMARY KEY ([UtilizadorId]),
2026-11-04T18:56:40.7748915Z           CONSTRAINT [FK_Funcionario_Utilizador_UtilizadorId] FOREIGN KEY ([UtilizadorId]) REFERENCES [Utilizador] ([Id]) ON DELETE NO ACTION
2026-11-04T18:56:40.7749718Z       );
2026-11-04T18:56:40.7756110Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7756846Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7757440Z       CREATE TABLE [TokenRevogado] (
2026-11-04T18:56:40.7757857Z           [Jti] nvarchar(64) NOT NULL,
2026-11-04T18:56:40.7758241Z           [UtilizadorId] int NOT NULL,
2026-11-04T18:56:40.7758674Z           [DataExpiracao] datetime2(0) NOT NULL,
2026-11-04T18:56:40.7759465Z           [DataRevogacao] datetime2(0) NOT NULL CONSTRAINT [DF_TokenRevogado_DataRevogacao] DEFAULT (SYSUTCDATETIME()),
2026-11-04T18:56:40.7760292Z           CONSTRAINT [PK_TokenRevogado] PRIMARY KEY ([Jti]),
2026-11-04T18:56:40.7761230Z           CONSTRAINT [FK_TokenRevogado_Utilizador_UtilizadorId] FOREIGN KEY ([UtilizadorId]) REFERENCES [Utilizador] ([Id]) ON DELETE NO ACTION
2026-11-04T18:56:40.7762020Z       );
2026-11-04T18:56:40.7775992Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7776784Z       Executed DbCommand (2ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7777371Z       CREATE TABLE [Paciente] (
2026-11-04T18:56:40.7777765Z           [Id] int NOT NULL IDENTITY,
2026-11-04T18:56:40.7778171Z           [ClienteId] int NOT NULL,
2026-11-04T18:56:40.7778563Z           [Nome] nvarchar(80) NOT NULL,
2026-11-04T18:56:40.7779026Z           [EspecieId] int NOT NULL,
2026-11-04T18:56:40.7779405Z           [RacaId] int NULL,
2026-11-04T18:56:40.7780057Z           [Sexo] nvarchar(12) NOT NULL CONSTRAINT [DF_Paciente_Sexo] DEFAULT N'Desconhecido',
2026-11-04T18:56:40.7780672Z           [DataNascimento] date NULL,
2026-11-04T18:56:40.7781086Z           [Peso] decimal(6,2) NULL,
2026-11-04T18:56:40.7781818Z           [DataRegisto] datetime2(0) NOT NULL CONSTRAINT [DF_Paciente_DataRegisto] DEFAULT (SYSUTCDATETIME()),
2026-11-04T18:56:40.7782549Z           [DataUltimaAlteracao] datetime2(0) NULL,
2026-11-04T18:56:40.7783187Z           CONSTRAINT [PK_Paciente] PRIMARY KEY ([Id]),
2026-11-04T18:56:40.7783765Z           CONSTRAINT [CK_Paciente_Peso] CHECK ([Peso] IS NULL OR [Peso] > 0),
2026-11-04T18:56:40.7784491Z           CONSTRAINT [CK_Paciente_Sexo] CHECK ([Sexo] IN ('Macho','Femea','Desconhecido')),
2026-11-04T18:56:40.7785469Z           CONSTRAINT [FK_Paciente_Cliente_ClienteId] FOREIGN KEY ([ClienteId]) REFERENCES [Cliente] ([Id]) ON DELETE NO ACTION,
2026-11-04T18:56:40.7786819Z           CONSTRAINT [FK_Paciente_Especie_EspecieId] FOREIGN KEY ([EspecieId]) REFERENCES [Especie] ([Id]) ON DELETE NO ACTION,
2026-11-04T18:56:40.7788029Z           CONSTRAINT [FK_Paciente_Raca] FOREIGN KEY ([EspecieId], [RacaId]) REFERENCES [Raca] ([EspecieId], [Id]) ON DELETE NO ACTION
2026-11-04T18:56:40.7788742Z       );
2026-11-04T18:56:40.7799874Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7800648Z       Executed DbCommand (2ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7801220Z       CREATE TABLE [Marcacao] (
2026-11-04T18:56:40.7801601Z           [Id] int NOT NULL IDENTITY,
2026-11-04T18:56:40.7801996Z           [PacienteId] int NOT NULL,
2026-11-04T18:56:40.7802406Z           [VeterinarioId] int NOT NULL,
2026-11-04T18:56:40.7802994Z           [DataHoraInicio] datetime2(0) NOT NULL,
2026-11-04T18:56:40.7803444Z           [DuracaoMinutos] smallint NOT NULL,
2026-11-04T18:56:40.7803876Z           [Motivo] nvarchar(250) NULL,
2026-11-04T18:56:40.7804500Z           [Estado] nvarchar(12) NOT NULL CONSTRAINT [DF_Marcacao_Estado] DEFAULT N'Pendente',
2026-11-04T18:56:40.7805112Z           [DataHoraChegada] datetime2(0) NULL,
2026-11-04T18:56:40.7805746Z           [CriadaPorId] int NOT NULL,
2026-11-04T18:56:40.7806470Z           [DataCriacao] datetime2(0) NOT NULL CONSTRAINT [DF_Marcacao_DataCriacao] DEFAULT (SYSUTCDATETIME()),
2026-11-04T18:56:40.7807178Z           [DataUltimaAlteracao] datetime2(0) NULL,
2026-11-04T18:56:40.7807639Z           [RowVersion] rowversion NOT NULL,
2026-11-04T18:56:40.7808107Z           CONSTRAINT [PK_Marcacao] PRIMARY KEY ([Id]),
2026-11-04T18:56:40.7808679Z           CONSTRAINT [CK_Marcacao_Duracao] CHECK ([DuracaoMinutos] > 0),
2026-11-04T18:56:40.7809513Z           CONSTRAINT [CK_Marcacao_Estado] CHECK ([Estado] IN ('Pendente','Confirmada','Realizada','Cancelada')),
2026-11-04T18:56:40.7810730Z           CONSTRAINT [FK_Marcacao_Funcionario_CriadaPorId] FOREIGN KEY ([CriadaPorId]) REFERENCES [Funcionario] ([UtilizadorId]) ON DELETE NO ACTION,
2026-11-04T18:56:40.7812149Z           CONSTRAINT [FK_Marcacao_Funcionario_VeterinarioId] FOREIGN KEY ([VeterinarioId]) REFERENCES [Funcionario] ([UtilizadorId]) ON DELETE NO ACTION,
2026-11-04T18:56:40.7813656Z           CONSTRAINT [FK_Marcacao_Paciente_PacienteId] FOREIGN KEY ([PacienteId]) REFERENCES [Paciente] ([Id]) ON DELETE NO ACTION
2026-11-04T18:56:40.7814381Z       );
2026-11-04T18:56:40.7817869Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7818624Z       Executed DbCommand (2ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7819180Z       CREATE TABLE [InformacaoApoio] (
2026-11-04T18:56:40.7819458Z           [Id] int NOT NULL IDENTITY,
2026-11-04T18:56:40.7819729Z           [MarcacaoId] int NOT NULL,
2026-11-04T18:56:40.7819976Z           [AuxiliarId] int NOT NULL,
2026-11-04T18:56:40.7820249Z           [Descricao] nvarchar(1000) NOT NULL,
2026-11-04T18:56:40.7820746Z           [DataRegisto] datetime2(0) NOT NULL CONSTRAINT [DF_InformacaoApoio_DataRegisto] DEFAULT (SYSUTCDATETIME()),
2026-11-04T18:56:40.7821229Z           CONSTRAINT [PK_InformacaoApoio] PRIMARY KEY ([Id]),
2026-11-04T18:56:40.7821854Z           CONSTRAINT [FK_InformacaoApoio_Funcionario_AuxiliarId] FOREIGN KEY ([AuxiliarId]) REFERENCES [Funcionario] ([UtilizadorId]) ON DELETE NO ACTION,
2026-11-04T18:56:40.7822638Z           CONSTRAINT [FK_InformacaoApoio_Marcacao_MarcacaoId] FOREIGN KEY ([MarcacaoId]) REFERENCES [Marcacao] ([Id]) ON DELETE NO ACTION
2026-11-04T18:56:40.7823254Z       );
2026-11-04T18:56:40.7833281Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7834226Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7834723Z       CREATE TABLE [RegistoClinico] (
2026-11-04T18:56:40.7834992Z           [Id] int NOT NULL IDENTITY,
2026-11-04T18:56:40.7835234Z           [MarcacaoId] int NOT NULL,
2026-11-04T18:56:40.7835678Z           [VeterinarioId] int NOT NULL,
2026-11-04T18:56:40.7835959Z           [NotasClinicas] nvarchar(4000) NULL,
2026-11-04T18:56:40.7836229Z           [Diagnostico] nvarchar(1000) NULL,
2026-11-04T18:56:40.7836685Z           [CriadoEm] datetime2(0) NOT NULL CONSTRAINT [DF_RegistoClinico_CriadoEm] DEFAULT (SYSUTCDATETIME()),
2026-11-04T18:56:40.7837107Z           [AtualizadoEm] datetime2(0) NULL,
2026-11-04T18:56:40.7837374Z           [AtualizadoPorId] int NULL,
2026-11-04T18:56:40.7837763Z           CONSTRAINT [PK_RegistoClinico] PRIMARY KEY ([Id]),
2026-11-04T18:56:40.7838497Z           CONSTRAINT [CK_RegistoClinico_Atualizacao] CHECK (([AtualizadoEm] IS NULL AND [AtualizadoPorId] IS NULL) OR ([AtualizadoEm] IS NOT NULL AND [AtualizadoPorId] IS NOT NULL)),
2026-11-04T18:56:40.7839424Z           CONSTRAINT [FK_RegistoClinico_Funcionario_AtualizadoPorId] FOREIGN KEY ([AtualizadoPorId]) REFERENCES [Funcionario] ([UtilizadorId]) ON DELETE NO ACTION,
2026-11-04T18:56:40.7840304Z           CONSTRAINT [FK_RegistoClinico_Funcionario_VeterinarioId] FOREIGN KEY ([VeterinarioId]) REFERENCES [Funcionario] ([UtilizadorId]) ON DELETE NO ACTION,
2026-11-04T18:56:40.7841091Z           CONSTRAINT [FK_RegistoClinico_Marcacao_MarcacaoId] FOREIGN KEY ([MarcacaoId]) REFERENCES [Marcacao] ([Id]) ON DELETE NO ACTION
2026-11-04T18:56:40.7841665Z       );
2026-11-04T18:56:40.7855786Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7856699Z       Executed DbCommand (2ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7857250Z       CREATE TABLE [RegistoProfilaxia] (
2026-11-04T18:56:40.7857697Z           [Id] int NOT NULL IDENTITY,
2026-11-04T18:56:40.7858143Z           [PacienteId] int NOT NULL,
2026-11-04T18:56:40.7858567Z           [VeterinarioId] int NOT NULL,
2026-11-04T18:56:40.7858989Z           [MarcacaoId] int NULL,
2026-11-04T18:56:40.7859402Z           [Tipo] nvarchar(15) NOT NULL,
2026-11-04T18:56:40.7859854Z           [Produto] nvarchar(150) NOT NULL,
2026-11-04T18:56:40.7860309Z           [Lote] nvarchar(50) NOT NULL,
2026-11-04T18:56:40.7860763Z           [DataAdministracao] date NOT NULL,
2026-11-04T18:56:40.7861405Z           [IntervaloDias] smallint NULL,
2026-11-04T18:56:40.7862262Z           [DataProximaAdministracao] AS CASE WHEN [IntervaloDias] IS NULL THEN NULL ELSE DATEADD(DAY, [IntervaloDias], [DataAdministracao]) END PERSISTED,
2026-11-04T18:56:40.7863352Z           [DataRegisto] datetime2(0) NOT NULL CONSTRAINT [DF_RegistoProfilaxia_DataRegisto] DEFAULT (SYSUTCDATETIME()),
2026-11-04T18:56:40.7864019Z           CONSTRAINT [PK_RegistoProfilaxia] PRIMARY KEY ([Id]),
2026-11-04T18:56:40.7864735Z           CONSTRAINT [CK_Profilaxia_Intervalo] CHECK ([IntervaloDias] IS NULL OR [IntervaloDias] > 0),
2026-11-04T18:56:40.7865434Z           CONSTRAINT [CK_Profilaxia_Tipo] CHECK ([Tipo] IN ('Vacina','Desparasitacao')),
2026-11-04T18:56:40.7866344Z           CONSTRAINT [FK_RegistoProfilaxia_Funcionario_VeterinarioId] FOREIGN KEY ([VeterinarioId]) REFERENCES [Funcionario] ([UtilizadorId]) ON DELETE NO ACTION,
2026-11-04T18:56:40.7867366Z           CONSTRAINT [FK_RegistoProfilaxia_Marcacao_MarcacaoId] FOREIGN KEY ([MarcacaoId]) REFERENCES [Marcacao] ([Id]) ON DELETE NO ACTION,
2026-11-04T18:56:40.7868341Z           CONSTRAINT [FK_RegistoProfilaxia_Paciente_PacienteId] FOREIGN KEY ([PacienteId]) REFERENCES [Paciente] ([Id]) ON DELETE NO ACTION
2026-11-04T18:56:40.7868887Z       );
2026-11-04T18:56:40.7884181Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7884894Z       Executed DbCommand (3ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7885474Z       CREATE TABLE [Prescricao] (
2026-11-04T18:56:40.7885724Z           [Id] int NOT NULL IDENTITY,
2026-11-04T18:56:40.7885989Z           [RegistoClinicoId] int NOT NULL,
2026-11-04T18:56:40.7886277Z           [Medicamento] nvarchar(150) NOT NULL,
2026-11-04T18:56:40.7886558Z           [Dosagem] nvarchar(100) NOT NULL,
2026-11-04T18:56:40.7887040Z           [Frequencia] nvarchar(100) NOT NULL,
2026-11-04T18:56:40.7887312Z           [DuracaoDias] smallint NULL,
2026-11-04T18:56:40.7887640Z           CONSTRAINT [PK_Prescricao] PRIMARY KEY ([Id]),
2026-11-04T18:56:40.7888085Z           CONSTRAINT [CK_Prescricao_Duracao] CHECK ([DuracaoDias] IS NULL OR [DuracaoDias] > 0),
2026-11-04T18:56:40.7888806Z           CONSTRAINT [FK_Prescricao_RegistoClinico_RegistoClinicoId] FOREIGN KEY ([RegistoClinicoId]) REFERENCES [RegistoClinico] ([Id]) ON DELETE NO ACTION
2026-11-04T18:56:40.7889321Z       );
2026-11-04T18:56:40.7894935Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7895659Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7896226Z       CREATE TABLE [Tratamento] (
2026-11-04T18:56:40.7896599Z           [Id] int NOT NULL IDENTITY,
2026-11-04T18:56:40.7896991Z           [RegistoClinicoId] int NOT NULL,
2026-11-04T18:56:40.7897419Z           [Descricao] nvarchar(1000) NOT NULL,
2026-11-04T18:56:40.7897843Z           [DataAplicacao] date NOT NULL,
2026-11-04T18:56:40.7898204Z           CONSTRAINT [PK_Tratamento] PRIMARY KEY ([Id]),
2026-11-04T18:56:40.7898832Z           CONSTRAINT [FK_Tratamento_RegistoClinico_RegistoClinicoId] FOREIGN KEY ([RegistoClinicoId]) REFERENCES [RegistoClinico] ([Id]) ON DELETE NO ACTION
2026-11-04T18:56:40.7899515Z       );
2026-11-04T18:56:40.7901978Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7902687Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7903346Z       CREATE INDEX [IX_Cliente_Nome] ON [Cliente] ([Nome]);
2026-11-04T18:56:40.7909521Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7910262Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7910786Z       CREATE UNIQUE INDEX [UQ_Cliente_NIF] ON [Cliente] ([NIF]) WHERE [NIF] IS NOT NULL;
2026-11-04T18:56:40.7916562Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7917273Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7917958Z       CREATE UNIQUE INDEX [UQ_Cliente_UtilizadorId] ON [Cliente] ([UtilizadorId]);
2026-11-04T18:56:40.7923079Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7923789Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7924324Z       CREATE UNIQUE INDEX [UQ_Especie_Nome] ON [Especie] ([Nome]);
2026-11-04T18:56:40.7930340Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7931060Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7931815Z       CREATE INDEX [IX_InformacaoApoio_AuxiliarId] ON [InformacaoApoio] ([AuxiliarId]);
2026-11-04T18:56:40.7938254Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7939663Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7940202Z       CREATE INDEX [IX_InformacaoApoio_MarcacaoId] ON [InformacaoApoio] ([MarcacaoId]);
2026-11-04T18:56:40.7945573Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7946254Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7946831Z       CREATE INDEX [IX_Marcacao_CriadaPorId] ON [Marcacao] ([CriadaPorId]);
2026-11-04T18:56:40.7951972Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7952461Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7953153Z       CREATE INDEX [IX_Marcacao_Data] ON [Marcacao] ([DataHoraInicio]);
2026-11-04T18:56:40.7959756Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7960479Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7961307Z       CREATE INDEX [IX_Marcacao_Paciente_Data] ON [Marcacao] ([PacienteId], [DataHoraInicio]);
2026-11-04T18:56:40.7966539Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7967041Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7967597Z       CREATE INDEX [IX_Marcacao_Veterinario_Data] ON [Marcacao] ([VeterinarioId], [DataHoraInicio]);
2026-11-04T18:56:40.7974525Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7975220Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7980969Z       CREATE INDEX [IX_Paciente_ClienteId] ON [Paciente] ([ClienteId]);
2026-11-04T18:56:40.7981591Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7982211Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7982730Z       CREATE INDEX [IX_Paciente_EspecieId_RacaId] ON [Paciente] ([EspecieId], [RacaId]);
2026-11-04T18:56:40.7988142Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7988870Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7989731Z       CREATE INDEX [IX_Paciente_Nome] ON [Paciente] ([Nome]);
2026-11-04T18:56:40.7996334Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.7997048Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.7997645Z       CREATE INDEX [IX_Prescricao_Registo] ON [Prescricao] ([RegistoClinicoId]);
2026-11-04T18:56:40.8003935Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.8004551Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.8005162Z       CREATE UNIQUE INDEX [UQ_Raca_Especie_Nome] ON [Raca] ([EspecieId], [Nome]);
2026-11-04T18:56:40.8010773Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.8011253Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.8011806Z       CREATE INDEX [IX_RegistoClinico_AtualizadoPorId] ON [RegistoClinico] ([AtualizadoPorId]);
2026-11-04T18:56:40.8018686Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.8019435Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.8020060Z       CREATE INDEX [IX_RegistoClinico_VeterinarioId] ON [RegistoClinico] ([VeterinarioId]);
2026-11-04T18:56:40.8026603Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.8027284Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.8027775Z       CREATE UNIQUE INDEX [UQ_RegistoClinico_Marcacao] ON [RegistoClinico] ([MarcacaoId]);
2026-11-04T18:56:40.8035013Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.8035710Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.8036398Z       CREATE INDEX [IX_Profilaxia_Paciente_Data] ON [RegistoProfilaxia] ([PacienteId], [DataAdministracao]);
2026-11-04T18:56:40.8042452Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.8043305Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.8044052Z       CREATE INDEX [IX_RegistoProfilaxia_MarcacaoId] ON [RegistoProfilaxia] ([MarcacaoId]);
2026-11-04T18:56:40.8049875Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.8050565Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.8051308Z       CREATE INDEX [IX_RegistoProfilaxia_VeterinarioId] ON [RegistoProfilaxia] ([VeterinarioId]);
2026-11-04T18:56:40.8058089Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.8058795Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.8059373Z       CREATE INDEX [IX_TokenRevogado_Utilizador] ON [TokenRevogado] ([UtilizadorId], [DataExpiracao]);
2026-11-04T18:56:40.8065665Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.8066373Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.8067000Z       CREATE INDEX [IX_Tratamento_Registo] ON [Tratamento] ([RegistoClinicoId]);
2026-11-04T18:56:40.8073227Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.8073925Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.8074392Z       CREATE UNIQUE INDEX [UQ_Utilizador_Email] ON [Utilizador] ([Email]);
2026-11-04T18:56:40.8103689Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.8104441Z       Executed DbCommand (3ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.8105118Z       INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
2026-11-04T18:56:40.8105827Z       VALUES (N'20261025165126_Inicial', N'10.0.13');
2026-11-04T18:56:40.8115973Z info: Microsoft.EntityFrameworkCore.Migrations[20402]
2026-11-04T18:56:40.8116537Z       Applying migration '20261025184243_I062DadosReferencia'.
2026-11-04T18:56:40.8116922Z Applying migration '20261025184243_I062DadosReferencia'.
2026-11-04T18:56:40.8650571Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.8651262Z       Executed DbCommand (21ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.8652512Z       IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DataUltimaAlteracao', N'Email', N'Morada', N'NIF', N'Nome', N'Telefone') AND [object_id] = OBJECT_ID(N'[Clinica]'))
2026-11-04T18:56:40.8653679Z           SET IDENTITY_INSERT [Clinica] ON;
2026-11-04T18:56:40.8654402Z       INSERT INTO [Clinica] ([Id], [DataUltimaAlteracao], [Email], [Morada], [NIF], [Nome], [Telefone])
2026-11-04T18:56:40.8656071Z       VALUES (CAST(1 AS tinyint), '2026-10-25T00:00:00', N'geral@clinica-g08.test', N'Rua do Exemplo, 100, 4470-000 Maia', '501234560', N'Clínica Veterinária G08', N'229000000');
2026-11-04T18:56:40.8657660Z       IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DataUltimaAlteracao', N'Email', N'Morada', N'NIF', N'Nome', N'Telefone') AND [object_id] = OBJECT_ID(N'[Clinica]'))
2026-11-04T18:56:40.8658670Z           SET IDENTITY_INSERT [Clinica] OFF;
2026-11-04T18:56:40.8732944Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.8733710Z       Executed DbCommand (8ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.8734939Z       IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Chave', N'DataUltimaAlteracao', N'Descricao', N'Valor') AND [object_id] = OBJECT_ID(N'[ConfiguracaoSistema]'))
2026-11-04T18:56:40.8735956Z           SET IDENTITY_INSERT [ConfiguracaoSistema] ON;
2026-11-04T18:56:40.8736650Z       INSERT INTO [ConfiguracaoSistema] ([Chave], [DataUltimaAlteracao], [Descricao], [Valor])
2026-11-04T18:56:40.8738071Z       VALUES (N'DuracaoPadraoConsultaMinutos', '2026-10-25T00:00:00', N'Duração por omissão de uma consulta, em minutos (EP-009 e EP-010).', N'30');
2026-11-04T18:56:40.8739586Z       IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Chave', N'DataUltimaAlteracao', N'Descricao', N'Valor') AND [object_id] = OBJECT_ID(N'[ConfiguracaoSistema]'))
2026-11-04T18:56:40.8740642Z           SET IDENTITY_INSERT [ConfiguracaoSistema] OFF;
2026-11-04T18:56:40.8813322Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.8814019Z       Executed DbCommand (8ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.8815914Z       IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nome') AND [object_id] = OBJECT_ID(N'[Especie]'))
2026-11-04T18:56:40.8816702Z           SET IDENTITY_INSERT [Especie] ON;
2026-11-04T18:56:40.8817129Z       INSERT INTO [Especie] ([Id], [Nome])
2026-11-04T18:56:40.8817642Z       VALUES (1, N'Cão'),
2026-11-04T18:56:40.8817981Z       (2, N'Gato');
2026-11-04T18:56:40.8818692Z       IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nome') AND [object_id] = OBJECT_ID(N'[Especie]'))
2026-11-04T18:56:40.8819472Z           SET IDENTITY_INSERT [Especie] OFF;
2026-11-04T18:56:40.8921686Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.8922384Z       Executed DbCommand (11ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.8923542Z       IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'EspecieId', N'Nome') AND [object_id] = OBJECT_ID(N'[Raca]'))
2026-11-04T18:56:40.8924311Z           SET IDENTITY_INSERT [Raca] ON;
2026-11-04T18:56:40.8924803Z       INSERT INTO [Raca] ([Id], [EspecieId], [Nome])
2026-11-04T18:56:40.8925306Z       VALUES (1, 1, N'Labrador Retriever'),
2026-11-04T18:56:40.8925865Z       (2, 1, N'Pastor Alemão'),
2026-11-04T18:56:40.8926596Z       (3, 1, N'Sem raça definida'),
2026-11-04T18:56:40.8926946Z       (4, 2, N'Europeu Comum'),
2026-11-04T18:56:40.8927345Z       (5, 2, N'Siamês');
2026-11-04T18:56:40.8928088Z       IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'EspecieId', N'Nome') AND [object_id] = OBJECT_ID(N'[Raca]'))
2026-11-04T18:56:40.8928863Z           SET IDENTITY_INSERT [Raca] OFF;
2026-11-04T18:56:40.8929260Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.8929719Z       Executed DbCommand (0ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.8930391Z       INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
2026-11-04T18:56:40.8930793Z       VALUES (N'20261025184243_I062DadosReferencia', N'10.0.13');
2026-11-04T18:56:40.8950562Z info: Microsoft.EntityFrameworkCore.Database.Command[20101]
2026-11-04T18:56:40.8951236Z       Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
2026-11-04T18:56:40.8951637Z       DECLARE @result int;
2026-11-04T18:56:40.8952023Z       EXEC @result = sp_releaseapplock @Resource = '__EFMigrationsLock', @LockOwner = 'Session';
2026-11-04T18:56:40.8952419Z       SELECT @result
2026-11-04T18:56:40.8969090Z Done.
2026-11-04T18:56:40.9223870Z ##[group]Run DADOS="$(jq -r '.Newman.dadosDemonstracao' "$QUALITY_GATES")"
2026-11-04T18:56:40.9224291Z [36;1mDADOS="$(jq -r '.Newman.dadosDemonstracao' "$QUALITY_GATES")"[0m
2026-11-04T18:56:40.9224615Z [36;1mbash "$DADOS/executar-dados.sh" \[0m
2026-11-04T18:56:40.9224862Z [36;1m  --base-dados "$BASE_DADOS" \[0m
2026-11-04T18:56:40.9225108Z [36;1m  --confiar-certificado \[0m
2026-11-04T18:56:40.9225399Z [36;1m  --pasta-registos "$GITHUB_WORKSPACE/$RESULTADOS/newman"[0m
2026-11-04T18:56:40.9242639Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:56:40.9243181Z env:
2026-11-04T18:56:40.9243400Z   DOTNET_NOLOGO: true
2026-11-04T18:56:40.9243600Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:56:40.9243833Z   CONFIGURACAO: Release
2026-11-04T18:56:40.9244061Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:56:40.9244464Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:56:40.9245084Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:56:40.9245528Z   RESULTADOS: resultados-ci
2026-11-04T18:56:40.9245745Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:56:40.9245982Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:56:40.9246235Z   BASE_DADOS: ClinicaVeterinaria_CI
2026-11-04T18:56:40.9246472Z   API_URL: http://localhost:5159
2026-11-04T18:56:40.9246704Z   CONTENTOR_SQL: sql-newman-ci
2026-11-04T18:56:40.9247029Z   IMAGEM_SQL: mcr.microsoft.com/mssql/server:2025-CU9-ubuntu-22.04
2026-11-04T18:56:40.9247334Z   DOTNET_EF_VERSAO: 10.0.13
2026-11-04T18:56:40.9247653Z   PROJETO_API: src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj
2026-11-04T18:56:40.9248214Z   PROJETO_INFRA: src/backend/ClinicaVeterinaria.Infrastructure/ClinicaVeterinaria.Infrastructure.csproj
2026-11-04T18:56:40.9248627Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:56:40.9248987Z   SQL_SA_PASSWORD: ***
2026-11-04T18:56:40.9249245Z   SQLCMDPASSWORD: ***
2026-11-04T18:56:40.9249576Z   Jwt__Key: ***
2026-11-04T18:56:40.9250234Z   ConnectionStrings__ClinicaDb: Server=localhost,1433;Database=ClinicaVeterinaria_CI;User Id=sa;***;TrustServerCertificate=True;Encrypt=True
2026-11-04T18:56:40.9250733Z ##[endgroup]
2026-11-04T18:56:40.9378139Z Execução dos scripts de dados iniciais e de demonstração (I061)
2026-11-04T18:56:40.9399108Z Data e hora      : 2026-11-04 18:56:40 +0000
2026-11-04T18:56:40.9418409Z Computador       : runnervm48cm3
2026-11-04T18:56:40.9429296Z Servidor         : localhost
2026-11-04T18:56:40.9440518Z Base de dados    : ClinicaVeterinaria_CI
2026-11-04T18:56:40.9451478Z Autenticação     : SQL Server (login sa)
2026-11-04T18:56:40.9498147Z Scripts          : 02-dados-demonstracao.sql 02-dados-demonstracao.sql verificar-dados-demonstracao.sql 
2026-11-04T18:56:41.0675271Z SQL Server       : Microsoft SQL Server 2025 (RTM-CU9) (KB5122048) - 17.0.5005.3 (X64) 
2026-11-04T18:56:41.0687259Z 
2026-11-04T18:56:41.0720765Z === 02-dados-demonstracao.sql — início 18:56:41
2026-11-04T18:56:41.3354062Z  Utilizador: 10 linha(s) inserida(s).
2026-11-04T18:56:41.3354387Z  Funcionario: 7 linha(s) inserida(s).
2026-11-04T18:56:41.3354687Z  HorarioFuncionamento: 11 linha(s) inserida(s).
2026-11-04T18:56:41.3354975Z  Cliente: 3 linha(s) inserida(s).
2026-11-04T18:56:41.3355222Z  Paciente: 4 linha(s) inserida(s).
2026-11-04T18:56:41.3355469Z  Marcacao: 5 linha(s) inserida(s).
2026-11-04T18:56:41.3355753Z  InformacaoApoio: 1 linha(s) inserida(s).
2026-11-04T18:56:41.3356142Z  RegistoClinico: 1 linha(s) inserida(s).
2026-11-04T18:56:41.3356526Z  Tratamento: 1 linha(s) inserida(s).
2026-11-04T18:56:41.3356880Z  Prescricao: 1 linha(s) inserida(s).
2026-11-04T18:56:41.3357324Z  RegistoProfilaxia: 4 linha(s) inserida(s).
2026-11-04T18:56:41.3357972Z  Dados de demonstração concluídos.
2026-11-04T18:56:41.3430271Z === 02-dados-demonstracao.sql — fim 18:56:41 — código de saída 0
2026-11-04T18:56:41.3442091Z 
2026-11-04T18:56:41.3476566Z === 02-dados-demonstracao.sql — início 18:56:41
2026-11-04T18:56:41.5598701Z  Utilizador: 0 linha(s) inserida(s).
2026-11-04T18:56:41.5599197Z  Funcionario: 0 linha(s) inserida(s).
2026-11-04T18:56:41.5599627Z  HorarioFuncionamento: 0 linha(s) inserida(s).
2026-11-04T18:56:41.5600042Z  Cliente: 0 linha(s) inserida(s).
2026-11-04T18:56:41.5600443Z  Paciente: 0 linha(s) inserida(s).
2026-11-04T18:56:41.5600835Z  Marcacao: 0 linha(s) inserida(s).
2026-11-04T18:56:41.5601221Z  InformacaoApoio: 0 linha(s) inserida(s).
2026-11-04T18:56:41.5601615Z  RegistoClinico: 0 linha(s) inserida(s).
2026-11-04T18:56:41.5602006Z  Tratamento: 0 linha(s) inserida(s).
2026-11-04T18:56:41.5602377Z  Prescricao: 0 linha(s) inserida(s).
2026-11-04T18:56:41.5602708Z  RegistoProfilaxia: 0 linha(s) inserida(s).
2026-11-04T18:56:41.5603557Z  Dados de demonstração concluídos.
2026-11-04T18:56:41.5683580Z === 02-dados-demonstracao.sql — fim 18:56:41 — código de saída 0
2026-11-04T18:56:41.5694236Z 
2026-11-04T18:56:41.5726954Z === verificar-dados-demonstracao.sql — início 18:56:41
2026-11-04T18:56:41.7598028Z Código Verificação                                                                                                                                                                                              Esperado                                                                                             Obtido                                                                                               Resultado 
2026-11-04T18:56:41.7600156Z ------ -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ---------------------------------------------------------------------------------------------------- ---------------------------------------------------------------------------------------------------- ----------
2026-11-04T18:56:41.7602475Z D01    Conta ativa no perfil Administrador                                                                                                                                                                      >= 1                                                                                                 1                                                                                                    OK        
2026-11-04T18:56:41.7604455Z D01    Conta ativa no perfil Rececionista                                                                                                                                                                       >= 1                                                                                                 1                                                                                                    OK        
2026-11-04T18:56:41.7606735Z D01    Conta ativa no perfil Veterinario                                                                                                                                                                        >= 1                                                                                                 2                                                                                                    OK        
2026-11-04T18:56:41.7608567Z D01    Conta ativa no perfil AuxiliarVeterinario                                                                                                                                                                >= 1                                                                                                 2                                                                                                    OK        
2026-11-04T18:56:41.7610770Z D01    Conta ativa no perfil Cliente                                                                                                                                                                            >= 1                                                                                                 3                                                                                                    OK        
2026-11-04T18:56:41.7612727Z D02    Conta bloqueada                                                                                                                                                                                          >= 1                                                                                                 1                                                                                                    OK        
2026-11-04T18:56:41.7615229Z D03    Auxiliar Veterinário ativo COM autorização de registo de apoio                                                                                                                                           >= 1                                                                                                 1                                                                                                    OK        
2026-11-04T18:56:41.7617999Z D04    Auxiliar Veterinário ativo SEM autorização de registo de apoio                                                                                                                                           >= 1                                                                                                 1                                                                                                    OK        
2026-11-04T18:56:41.7620196Z D05    Contas internas sem linha em Funcionario                                                                                                                                                                 0                                                                                                    0                                                                                                    OK        
2026-11-04T18:56:41.7622319Z D06    Contas de Cliente sem linha em Cliente                                                                                                                                                                   0                                                                                                    0                                                                                                    OK        
2026-11-04T18:56:41.7624902Z D07    Contas de demonstração com hash fora do formato PasswordHasher v3                                                                                                                                        0                                                                                                    0                                                                                                    OK        
2026-11-04T18:56:41.7628077Z D08    Clientes com mais de um animal                                                                                                                                                                           >= 1                                                                                                 1                                                                                                    OK        
2026-11-04T18:56:41.7630247Z D09    Marcações no estado Pendente                                                                                                                                                                             >= 1                                                                                                 2                                                                                                    OK        
2026-11-04T18:56:41.7632525Z D09    Marcações no estado Confirmada                                                                                                                                                                           >= 1                                                                                                 1                                                                                                    OK        
2026-11-04T18:56:41.7634953Z D09    Marcações no estado Realizada                                                                                                                                                                            >= 1                                                                                                 1                                                                                                    OK        
2026-11-04T18:56:41.7637185Z D09    Marcações no estado Cancelada                                                                                                                                                                            >= 1                                                                                                 1                                                                                                    OK        
2026-11-04T18:56:41.7639509Z D10    Registo clínico com tratamento e prescrição                                                                                                                                                              >= 1                                                                                                 1                                                                                                    OK        
2026-11-04T18:56:41.7641953Z D11    Paciente com boletim de vacinação e de desparasitação                                                                                                                                                    >= 1                                                                                                 1                                                                                                    OK        
2026-11-04T18:56:41.7644557Z D12    Marcação com registo de chegada                                                                                                                                                                          >= 1                                                                                                 1                                                                                                    OK        
2026-11-04T18:56:41.7646829Z D13    Informação de apoio registada                                                                                                                                                                            >= 1                                                                                                 1                                                                                                    OK        
2026-11-04T18:56:41.7648744Z D14    Dias com horário de funcionamento por omissão                                                                                                                                                            6                                                                                                    6                                                                                                    OK        
2026-11-04T18:56:41.7650285Z D15    Registos clínicos em consultas não realizadas                                                                                                                                                            0                                                                                                    0                                                                                                    OK        
2026-11-04T18:56:41.7651921Z D16    Registos clínicos de outro Veterinário que não o da consulta                                                                                                                                             0                                                                                                    0                                                                                                    OK        
2026-11-04T18:56:41.7653615Z D17    Profilaxias ligadas a consulta de outro paciente                                                                                                                                                         0                                                                                                    0                                                                                                    OK        
2026-11-04T18:56:41.7655148Z D18    Marcações sobrepostas do mesmo Veterinário (RF021)                                                                                                                                                       0                                                                                                    0                                                                                                    OK        
2026-11-04T18:56:41.7656634Z D19    Marcações atribuídas a quem não é Veterinário                                                                                                                                                            0                                                                                                    0                                                                                                    OK        
2026-11-04T18:56:41.7658084Z D20    Informação de apoio registada por quem não tem autorização                                                                                                                                               0                                                                                                    0                                                                                                    OK        
2026-11-04T18:56:41.7659562Z D21    Contas fora do domínio reservado .test (demonstração)                                                                                                                                                    0                                                                                                    0                                                                                                    OK        
2026-11-04T18:56:41.7660254Z Tabela               Linhas     
2026-11-04T18:56:41.7660517Z -------------------- -----------
2026-11-04T18:56:41.7660747Z Utilizador                    10
2026-11-04T18:56:41.7660962Z Funcionario                    7
2026-11-04T18:56:41.7661183Z Cliente                        3
2026-11-04T18:56:41.7661394Z Paciente                       4
2026-11-04T18:56:41.7661610Z HorarioFuncionamento          11
2026-11-04T18:56:41.7661842Z Marcacao                       5
2026-11-04T18:56:41.7662216Z InformacaoApoio                1
2026-11-04T18:56:41.7662443Z RegistoClinico                 1
2026-11-04T18:56:41.7662659Z Tratamento                     1
2026-11-04T18:56:41.7663068Z Prescricao                     1
2026-11-04T18:56:41.7663313Z RegistoProfilaxia              4
2026-11-04T18:56:41.7663811Z Verificação dos dados de demonstração: 28 verificações, 28 OK, 0 falhadas.
2026-11-04T18:56:41.7664382Z Resultado: os dados de demonstração cobrem os critérios 2 e 3 da I061 e são coerentes.
2026-11-04T18:56:41.7685408Z === verificar-dados-demonstracao.sql — fim 18:56:41 — código de saída 0
2026-11-04T18:56:41.7696666Z 
2026-11-04T18:56:41.7709321Z Concluído sem erros. Registo: /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/resultados-ci/newman/dados-execucao-20261104-185640.log
2026-11-04T18:56:41.7751938Z ##[group]Run AMBIENTE="$(jq -r '.Newman.ambiente' "$QUALITY_GATES")"
2026-11-04T18:56:41.7752584Z [36;1mAMBIENTE="$(jq -r '.Newman.ambiente' "$QUALITY_GATES")"[0m
2026-11-04T18:56:41.7753301Z [36;1mpython3 .github/scripts/preparar-newman.py \[0m
2026-11-04T18:56:41.7753744Z [36;1m  "$QUALITY_GATES" "$AMBIENTE" \[0m
2026-11-04T18:56:41.7754363Z [36;1m  "$RUNNER_TEMP/newman-ambiente.json" "$RUNNER_TEMP/palavra-passe-demonstracao.sql" \[0m
2026-11-04T18:56:41.7754932Z [36;1m  "$API_URL"[0m
2026-11-04T18:56:41.7755502Z [36;1msqlcmd -S localhost -d "$BASE_DADOS" -U sa -C -b -h -1 -W -i "$RUNNER_TEMP/palavra-passe-demonstracao.sql"[0m
2026-11-04T18:56:41.7756231Z [36;1mrm -f "$RUNNER_TEMP/palavra-passe-demonstracao.sql"[0m
2026-11-04T18:56:41.7780187Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:56:41.7780640Z env:
2026-11-04T18:56:41.7780904Z   DOTNET_NOLOGO: true
2026-11-04T18:56:41.7781260Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:56:41.7781694Z   CONFIGURACAO: Release
2026-11-04T18:56:41.7782057Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:56:41.7782719Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:56:41.7783989Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:56:41.7784746Z   RESULTADOS: resultados-ci
2026-11-04T18:56:41.7785091Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:56:41.7785454Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:56:41.7785843Z   BASE_DADOS: ClinicaVeterinaria_CI
2026-11-04T18:56:41.7786227Z   API_URL: http://localhost:5159
2026-11-04T18:56:41.7786586Z   CONTENTOR_SQL: sql-newman-ci
2026-11-04T18:56:41.7787034Z   IMAGEM_SQL: mcr.microsoft.com/mssql/server:2025-CU9-ubuntu-22.04
2026-11-04T18:56:41.7787518Z   DOTNET_EF_VERSAO: 10.0.13
2026-11-04T18:56:41.7788002Z   PROJETO_API: src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj
2026-11-04T18:56:41.7788875Z   PROJETO_INFRA: src/backend/ClinicaVeterinaria.Infrastructure/ClinicaVeterinaria.Infrastructure.csproj
2026-11-04T18:56:41.7789529Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:56:41.7790049Z   SQL_SA_PASSWORD: ***
2026-11-04T18:56:41.7790444Z   SQLCMDPASSWORD: ***
2026-11-04T18:56:41.7790955Z   Jwt__Key: ***
2026-11-04T18:56:41.7792003Z   ConnectionStrings__ClinicaDb: Server=localhost,1433;Database=ClinicaVeterinaria_CI;User Id=sa;***;TrustServerCertificate=True;Encrypt=True
2026-11-04T18:56:41.7792998Z ##[endgroup]
2026-11-04T18:56:41.8600442Z Variáveis preenchidas na cópia do ambiente: passwordAdmin, passwordRececionista, passwordVeterinario, passwordAuxiliar, passwordCliente, passwordBloqueada, passwordAuxiliarSemApoio; baseUrl = http://localhost:5159
2026-11-04T18:56:41.8602009Z Contas atualizadas: e-mails terminados em @clinica-g08.test, só na base de dados temporária do runner.
2026-11-04T18:56:41.9745814Z Contas de demonstração com a palavra-passe descartável: 10
2026-11-04T18:56:41.9824591Z ##[group]Run SAIDA="$(dirname "$PROJETO_API")/bin/$CONFIGURACAO/net10.0"
2026-11-04T18:56:41.9825080Z [36;1mSAIDA="$(dirname "$PROJETO_API")/bin/$CONFIGURACAO/net10.0"[0m
2026-11-04T18:56:41.9825713Z [36;1m([0m
2026-11-04T18:56:41.9825895Z [36;1m  cd "$SAIDA"[0m
2026-11-04T18:56:41.9826186Z [36;1m  ASPNETCORE_ENVIRONMENT=Development ASPNETCORE_URLS="$API_URL" \[0m
2026-11-04T18:56:41.9826668Z [36;1m    nohup dotnet ClinicaVeterinaria.Api.dll > "$GITHUB_WORKSPACE/$RESULTADOS/newman/api.log" 2>&1 &[0m
2026-11-04T18:56:41.9827090Z [36;1m  echo $! > "$RUNNER_TEMP/api.pid"[0m
2026-11-04T18:56:41.9827324Z [36;1m)[0m
2026-11-04T18:56:41.9827507Z [36;1mfor i in $(seq 1 60); do[0m
2026-11-04T18:56:41.9827833Z [36;1m  CODIGO="$(curl -s -o /dev/null -w '%{http_code}' "$API_URL/clinica" || true)"[0m
2026-11-04T18:56:41.9828172Z [36;1m  if [ "$CODIGO" != "000" ]; then[0m
2026-11-04T18:56:41.9828553Z [36;1m    echo "API a responder ao fim de $((i * 2)) s (GET /clinica sem token: HTTP $CODIGO)."[0m
2026-11-04T18:56:41.9828895Z [36;1m    exit 0[0m
2026-11-04T18:56:41.9829085Z [36;1m  fi[0m
2026-11-04T18:56:41.9829336Z [36;1m  if ! kill -0 "$(cat "$RUNNER_TEMP/api.pid")" 2>/dev/null; then[0m
2026-11-04T18:56:41.9829626Z [36;1m    break[0m
2026-11-04T18:56:41.9829830Z [36;1m  fi[0m
2026-11-04T18:56:41.9830008Z [36;1m  sleep 2[0m
2026-11-04T18:56:41.9830179Z [36;1mdone[0m
2026-11-04T18:56:41.9830437Z [36;1mtail -n 50 "$GITHUB_WORKSPACE/$RESULTADOS/newman/api.log"[0m
2026-11-04T18:56:41.9830899Z [36;1mecho "::error title=Newman::A API não arrancou (ver api.log no artefacto resultado-newman)."[0m
2026-11-04T18:56:41.9831307Z [36;1mexit 1[0m
2026-11-04T18:56:41.9848995Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:56:41.9849289Z env:
2026-11-04T18:56:41.9849465Z   DOTNET_NOLOGO: true
2026-11-04T18:56:41.9849680Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:56:41.9849909Z   CONFIGURACAO: Release
2026-11-04T18:56:41.9850148Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:56:41.9850566Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:56:41.9851163Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:56:41.9851607Z   RESULTADOS: resultados-ci
2026-11-04T18:56:41.9851836Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:56:41.9852067Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:56:41.9852320Z   BASE_DADOS: ClinicaVeterinaria_CI
2026-11-04T18:56:41.9852554Z   API_URL: http://localhost:5159
2026-11-04T18:56:41.9852957Z   CONTENTOR_SQL: sql-newman-ci
2026-11-04T18:56:41.9853247Z   IMAGEM_SQL: mcr.microsoft.com/mssql/server:2025-CU9-ubuntu-22.04
2026-11-04T18:56:41.9853564Z   DOTNET_EF_VERSAO: 10.0.13
2026-11-04T18:56:41.9853889Z   PROJETO_API: src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj
2026-11-04T18:56:41.9854416Z   PROJETO_INFRA: src/backend/ClinicaVeterinaria.Infrastructure/ClinicaVeterinaria.Infrastructure.csproj
2026-11-04T18:56:41.9854833Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:56:41.9855209Z   SQL_SA_PASSWORD: ***
2026-11-04T18:56:41.9855480Z   SQLCMDPASSWORD: ***
2026-11-04T18:56:41.9855821Z   Jwt__Key: ***
2026-11-04T18:56:41.9856474Z   ConnectionStrings__ClinicaDb: Server=localhost,1433;Database=ClinicaVeterinaria_CI;User Id=sa;***;TrustServerCertificate=True;Encrypt=True
2026-11-04T18:56:41.9856976Z ##[endgroup]
2026-11-04T18:56:44.1316947Z API a responder ao fim de 4 s (GET /clinica sem token: HTTP 401).
2026-11-04T18:56:44.1346578Z ##[group]Run VERSAO="$(jq -r '.Newman.versaoNewman' "$QUALITY_GATES")"
2026-11-04T18:56:44.1347002Z [36;1mVERSAO="$(jq -r '.Newman.versaoNewman' "$QUALITY_GATES")"[0m
2026-11-04T18:56:44.1347366Z [36;1mCOLLECTION="$(jq -r '.Newman.collection' "$QUALITY_GATES")"[0m
2026-11-04T18:56:44.1347661Z [36;1mset +e[0m
2026-11-04T18:56:44.1347898Z [36;1mnpx --yes "newman@$VERSAO" run "$COLLECTION" \[0m
2026-11-04T18:56:44.1348208Z [36;1m  -e "$RUNNER_TEMP/newman-ambiente.json" \[0m
2026-11-04T18:56:44.1348479Z [36;1m  --reporters cli,junit \[0m
2026-11-04T18:56:44.1348807Z [36;1m  --reporter-junit-export "$RESULTADOS/newman/newman-junit.xml" \[0m
2026-11-04T18:56:44.1349298Z [36;1m  --timeout-request 30000 \[0m
2026-11-04T18:56:44.1349554Z [36;1m  --color off --disable-unicode \[0m
2026-11-04T18:56:44.1349838Z [36;1m  | tee "$RESULTADOS/newman/resultado-newman.txt"[0m
2026-11-04T18:56:44.1350160Z [36;1mecho "codigo=${PIPESTATUS[0]}" >> "$GITHUB_OUTPUT"[0m
2026-11-04T18:56:44.1350479Z [36;1mexit 0[0m
2026-11-04T18:56:44.1368183Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:56:44.1368481Z env:
2026-11-04T18:56:44.1368658Z   DOTNET_NOLOGO: true
2026-11-04T18:56:44.1368877Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:56:44.1369107Z   CONFIGURACAO: Release
2026-11-04T18:56:44.1369435Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:56:44.1369868Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:56:44.1370482Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:56:44.1370935Z   RESULTADOS: resultados-ci
2026-11-04T18:56:44.1371169Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:56:44.1371417Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:56:44.1371677Z   BASE_DADOS: ClinicaVeterinaria_CI
2026-11-04T18:56:44.1371925Z   API_URL: http://localhost:5159
2026-11-04T18:56:44.1372173Z   CONTENTOR_SQL: sql-newman-ci
2026-11-04T18:56:44.1372469Z   IMAGEM_SQL: mcr.microsoft.com/mssql/server:2025-CU9-ubuntu-22.04
2026-11-04T18:56:44.1372773Z   DOTNET_EF_VERSAO: 10.0.13
2026-11-04T18:56:44.1373196Z   PROJETO_API: src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj
2026-11-04T18:56:44.1373729Z   PROJETO_INFRA: src/backend/ClinicaVeterinaria.Infrastructure/ClinicaVeterinaria.Infrastructure.csproj
2026-11-04T18:56:44.1374154Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:56:44.1374563Z   SQL_SA_PASSWORD: ***
2026-11-04T18:56:44.1374824Z   SQLCMDPASSWORD: ***
2026-11-04T18:56:44.1375148Z   Jwt__Key: ***
2026-11-04T18:56:44.1375818Z   ConnectionStrings__ClinicaDb: Server=localhost,1433;Database=ClinicaVeterinaria_CI;User Id=sa;***;TrustServerCertificate=True;Encrypt=True
2026-11-04T18:56:44.1376331Z ##[endgroup]
2026-11-04T18:56:49.3929736Z npm warn deprecated har-validator@5.1.5: this library is no longer supported
2026-11-04T18:56:49.6161802Z npm warn deprecated uuid@3.4.0: uuid@10 and below is no longer supported.  For ESM codebases, update to uuid@latest.  For CommonJS codebases, use uuid@11 (but be aware this version will likely be deprecated in 2028).
2026-11-04T18:56:49.9498764Z npm warn deprecated uuid@8.3.2: uuid@10 and below is no longer supported.  For ESM codebases, update to uuid@latest.  For CommonJS codebases, use uuid@11 (but be aware this version will likely be deprecated in 2028).
2026-11-04T18:56:50.8486192Z npm warn deprecated @faker-js/faker@5.5.3: Please update to a newer version.
2026-11-04T18:56:51.4569324Z newman
2026-11-04T18:56:51.4569557Z 
2026-11-04T18:56:51.4570040Z Clínica Veterinária G08 — API m3
2026-11-04T18:56:51.4607118Z 
2026-11-04T18:56:51.4607900Z Folder 00 · Preparação — sessões por perfil
2026-11-04T18:56:51.4608350Z Sub-folder EP-001 · 200 — iniciar sessão (Administrador)
2026-11-04T18:56:51.9303226Z   POST http://localhost:5159/auth/login [200 OK, 714B, 463ms]
2026-11-04T18:56:51.9438512Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:51.9515738Z   Pass  Devolve 200
2026-11-04T18:56:51.9520747Z   Pass  Devolve um token JWT e a data de expiração
2026-11-04T18:56:51.9523613Z   Pass  O utilizador tem o perfil Administrador
2026-11-04T18:56:51.9527051Z   Pass  O Administrador não tem clienteId
2026-11-04T18:56:51.9578416Z 
2026-11-04T18:56:51.9578872Z Sub-folder EP-001 · 200 — iniciar sessão (Rececionista)
2026-11-04T18:56:52.0751810Z   POST http://localhost:5159/auth/login [200 OK, 719B, 112ms]
2026-11-04T18:56:52.0807853Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:52.0905435Z   Pass  Devolve 200
2026-11-04T18:56:52.0911772Z   Pass  Devolve um token JWT e a data de expiração
2026-11-04T18:56:52.0913837Z   Pass  O utilizador tem o perfil Rececionista
2026-11-04T18:56:52.0965072Z 
2026-11-04T18:56:52.0965598Z Sub-folder EP-001 · 200 — iniciar sessão (Veterinário)
2026-11-04T18:56:52.1664686Z   POST http://localhost:5159/auth/login [200 OK, 747B, 66ms]
2026-11-04T18:56:52.1715640Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:52.1783587Z   Pass  Devolve 200
2026-11-04T18:56:52.1787399Z   Pass  Devolve um token JWT e a data de expiração
2026-11-04T18:56:52.1789503Z   Pass  O utilizador tem o perfil Veterinario
2026-11-04T18:56:52.1837773Z 
2026-11-04T18:56:52.1838691Z Sub-folder EP-001 · 200 — iniciar sessão (Auxiliar Veterinário)
2026-11-04T18:56:52.2557413Z   POST http://localhost:5159/auth/login [200 OK, 732B, 68ms]
2026-11-04T18:56:52.2604656Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:52.2704035Z   Pass  Devolve 200
2026-11-04T18:56:52.2708431Z   Pass  Devolve um token JWT e a data de expiração
2026-11-04T18:56:52.2710248Z   Pass  O utilizador tem o perfil AuxiliarVeterinario
2026-11-04T18:56:52.2806263Z 
2026-11-04T18:56:52.2807105Z Sub-folder EP-001 · 200 — iniciar sessão (Cliente)
2026-11-04T18:56:52.3495060Z   POST http://localhost:5159/auth/login [200 OK, 731B, 66ms]
2026-11-04T18:56:52.3545865Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:52.3605569Z   Pass  Devolve 200
2026-11-04T18:56:52.3608898Z   Pass  Devolve um token JWT e a data de expiração
2026-11-04T18:56:52.3610352Z   Pass  O utilizador tem o perfil Cliente
2026-11-04T18:56:52.3612402Z   Pass  O Cliente tem clienteId
2026-11-04T18:56:52.3653198Z 
2026-11-04T18:56:52.3653639Z Folder 01 · Autenticação (EP-001 a EP-003)
2026-11-04T18:56:52.3654168Z Sub-folder EP-001 · 400 — e-mail em falta
2026-11-04T18:56:52.3733270Z   POST http://localhost:5159/auth/login [400 Bad Request, 236B, 5ms]
2026-11-04T18:56:52.3765782Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:52.3768163Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:52.3813682Z   Pass  Devolve 400
2026-11-04T18:56:52.3854812Z 
2026-11-04T18:56:52.3855177Z Sub-folder EP-001 · 400 — palavra-passe em falta
2026-11-04T18:56:52.3916759Z   POST http://localhost:5159/auth/login [400 Bad Request, 243B, 2ms]
2026-11-04T18:56:52.3952419Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:52.3954967Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:52.3989109Z   Pass  Devolve 400
2026-11-04T18:56:52.4026310Z 
2026-11-04T18:56:52.4026668Z Sub-folder EP-001 · 401 — palavra-passe incorreta
2026-11-04T18:56:52.4896203Z   POST http://localhost:5159/auth/login [401 Unauthorized, 246B, 83ms]
2026-11-04T18:56:52.4939043Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:52.4941568Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:52.4989426Z   Pass  Devolve 401
2026-11-04T18:56:52.4990935Z   Pass  Não devolve token
2026-11-04T18:56:52.5051566Z 
2026-11-04T18:56:52.5052139Z Sub-folder EP-001 · 401 — e-mail inexistente (mesma resposta)
2026-11-04T18:56:52.5728138Z   POST http://localhost:5159/auth/login [401 Unauthorized, 246B, 63ms]
2026-11-04T18:56:52.5777839Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:52.5780739Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:52.5826898Z   Pass  Devolve 401
2026-11-04T18:56:52.5871049Z 
2026-11-04T18:56:52.5871863Z Sub-folder EP-001 · 403 — conta bloqueada (palavra-passe correta)
2026-11-04T18:56:52.6457926Z   POST http://localhost:5159/auth/login [403 Forbidden, 296B, 56ms]
2026-11-04T18:56:52.6494105Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:52.6496436Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:52.6536881Z   Pass  Devolve 403
2026-11-04T18:56:52.6566849Z 
2026-11-04T18:56:52.6567541Z Sub-folder EP-003 · 200 — utilizador autenticado (Administrador)
2026-11-04T18:56:52.7250247Z   GET http://localhost:5159/auth/me [200 OK, 273B, 64ms]
2026-11-04T18:56:52.7286589Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:52.7344080Z   Pass  Devolve 200
2026-11-04T18:56:52.7348011Z   Pass  Devolve o Administrador do token
2026-11-04T18:56:52.7391626Z 
2026-11-04T18:56:52.7392145Z Sub-folder EP-003 · 200 — utilizador autenticado (Cliente, com clienteId)
2026-11-04T18:56:52.7490366Z   GET http://localhost:5159/auth/me [200 OK, 267B, 7ms]
2026-11-04T18:56:52.7523464Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:52.7573544Z   Pass  Devolve 200
2026-11-04T18:56:52.7576188Z   Pass  Devolve o Cliente com clienteId
2026-11-04T18:56:52.7618860Z 
2026-11-04T18:56:52.7619166Z Sub-folder EP-003 · 401 — sem token
2026-11-04T18:56:52.7669275Z   GET http://localhost:5159/auth/me [401 Unauthorized, 269B, 1ms]
2026-11-04T18:56:52.7693930Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:52.7696555Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:52.7740345Z   Pass  Devolve 401
2026-11-04T18:56:52.7783628Z 
2026-11-04T18:56:52.7783992Z Sub-folder EP-003 · 401 — token adulterado
2026-11-04T18:56:52.7862365Z   GET http://localhost:5159/auth/me [401 Unauthorized, 288B, 5ms]
2026-11-04T18:56:52.7901951Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:52.7902612Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:52.7952405Z   Pass  Devolve 401
2026-11-04T18:56:52.7996049Z 
2026-11-04T18:56:52.7996641Z Sub-folder EP-003 · 401 — token assinado com outra chave
2026-11-04T18:56:52.8043503Z   GET http://localhost:5159/auth/me [401 Unauthorized, 288B, 2ms]
2026-11-04T18:56:52.8074558Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:52.8077164Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:52.8124016Z   Pass  Devolve 401
2026-11-04T18:56:52.8160873Z 
2026-11-04T18:56:52.8161410Z Sub-folder EP-002 · preparação — iniciar a sessão a terminar (EP-001 · 200)
2026-11-04T18:56:52.8877837Z   POST http://localhost:5159/auth/login [200 OK, 714B, 69ms]
2026-11-04T18:56:52.8974396Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:52.8994683Z   Pass  Devolve 200
2026-11-04T18:56:52.8997937Z   Pass  Devolve um token JWT e a data de expiração
2026-11-04T18:56:52.8999271Z   Pass  O utilizador tem o perfil Administrador
2026-11-04T18:56:52.9037196Z 
2026-11-04T18:56:52.9037507Z Sub-folder EP-002 · 204 — terminar sessão
2026-11-04T18:56:52.9459495Z   POST http://localhost:5159/auth/logout [204 No Content, 81B, 39ms]
2026-11-04T18:56:52.9544099Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:52.9573903Z   Pass  Devolve 204
2026-11-04T18:56:52.9574417Z   Pass  Sem corpo
2026-11-04T18:56:52.9608135Z 
2026-11-04T18:56:52.9608616Z Sub-folder EP-002 · 401 — o token terminado deixa de ser aceite (EP-003)
2026-11-04T18:56:52.9686315Z   GET http://localhost:5159/auth/me [401 Unauthorized, 288B, 5ms]
2026-11-04T18:56:52.9717971Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:52.9720255Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:52.9773678Z   Pass  Devolve 401
2026-11-04T18:56:52.9809623Z 
2026-11-04T18:56:52.9810361Z Sub-folder EP-002 · 401 — terminar sessão outra vez com o token revogado
2026-11-04T18:56:52.9888464Z   POST http://localhost:5159/auth/logout [401 Unauthorized, 288B, 4ms]
2026-11-04T18:56:52.9919500Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:52.9922030Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:52.9952650Z   Pass  Devolve 401
2026-11-04T18:56:52.9988308Z 
2026-11-04T18:56:52.9988580Z Sub-folder EP-002 · 401 — sem token
2026-11-04T18:56:53.0037480Z   POST http://localhost:5159/auth/logout [401 Unauthorized, 269B, 1ms]
2026-11-04T18:56:53.0069699Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.0071548Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:53.0101226Z   Pass  Devolve 401
2026-11-04T18:56:53.0136470Z 
2026-11-04T18:56:53.0136960Z Folder 02 · Contas de funcionário (EP-004 a EP-008)
2026-11-04T18:56:53.0137562Z Sub-folder EP-004 · 200 — listar contas de funcionário
2026-11-04T18:56:53.1103208Z   GET http://localhost:5159/utilizadores [200 OK, 1.3kB, 93ms]
2026-11-04T18:56:53.1127855Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.1172010Z   Pass  Devolve 200
2026-11-04T18:56:53.1176050Z   Pass  Envelope paginado (itens, pagina, tamanho, total)
2026-11-04T18:56:53.1179637Z   Pass  Nunca lista contas de Cliente
2026-11-04T18:56:53.1194999Z   Pass  Cada conta tem id, nome, e-mail, perfil e estado
2026-11-04T18:56:53.1226673Z 
2026-11-04T18:56:53.1227192Z Sub-folder EP-004 · 200 — filtrar por perfil e estado
2026-11-04T18:56:53.1485238Z   GET http://localhost:5159/utilizadores?perfil=Veterinario&estado=Ativo [200 OK, 512B, 22ms]
2026-11-04T18:56:53.1524166Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.1575870Z   Pass  Devolve 200
2026-11-04T18:56:53.1579531Z   Pass  Só Veterinários ativos
2026-11-04T18:56:53.1616077Z 
2026-11-04T18:56:53.1616520Z Sub-folder EP-004 · 400 — perfil fora da enumeração
2026-11-04T18:56:53.1718717Z   GET http://localhost:5159/utilizadores?perfil=Gestor [400 Bad Request, 341B, 7ms]
2026-11-04T18:56:53.1751486Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.1753554Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:53.1785682Z   Pass  Devolve 400
2026-11-04T18:56:53.1821043Z 
2026-11-04T18:56:53.1821360Z Sub-folder EP-004 · 401 — sem token
2026-11-04T18:56:53.1871950Z   GET http://localhost:5159/utilizadores [401 Unauthorized, 269B, 2ms]
2026-11-04T18:56:53.1901095Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.1903368Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:53.1996405Z   Pass  Devolve 401
2026-11-04T18:56:53.2001976Z 
2026-11-04T18:56:53.2002296Z Sub-folder EP-004 · 403 — Rececionista
2026-11-04T18:56:53.2086753Z   GET http://localhost:5159/utilizadores [403 Forbidden, 311B, 6ms]
2026-11-04T18:56:53.2118740Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.2120613Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:53.2151695Z   Pass  Devolve 403
2026-11-04T18:56:53.2186048Z 
2026-11-04T18:56:53.2186504Z Sub-folder EP-005 · 200 — consultar conta de funcionário
2026-11-04T18:56:53.2438155Z   GET http://localhost:5159/utilizadores/1 [200 OK, 356B, 21ms]
2026-11-04T18:56:53.2470538Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.2501077Z   Pass  Devolve 200
2026-11-04T18:56:53.2503537Z   Pass  Devolve a conta do Administrador
2026-11-04T18:56:53.2545646Z 
2026-11-04T18:56:53.2546224Z Sub-folder EP-005 · 404 — conta de Cliente não é conta de funcionário
2026-11-04T18:56:53.2639081Z   GET http://localhost:5159/utilizadores/8 [404 Not Found, 258B, 6ms]
2026-11-04T18:56:53.2669640Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.2682743Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:53.2712543Z   Pass  Devolve 404
2026-11-04T18:56:53.2747625Z 
2026-11-04T18:56:53.2747934Z Sub-folder EP-005 · 404 — conta inexistente
2026-11-04T18:56:53.2838612Z   GET http://localhost:5159/utilizadores/999999 [404 Not Found, 258B, 5ms]
2026-11-04T18:56:53.2873065Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.2883735Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:53.2923034Z   Pass  Devolve 404
2026-11-04T18:56:53.2955750Z 
2026-11-04T18:56:53.2956033Z Sub-folder EP-005 · 401 — sem token
2026-11-04T18:56:53.3006713Z   GET http://localhost:5159/utilizadores/1 [401 Unauthorized, 269B, 1ms]
2026-11-04T18:56:53.3035843Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.3038201Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:53.3066809Z   Pass  Devolve 401
2026-11-04T18:56:53.3102215Z 
2026-11-04T18:56:53.3102552Z Sub-folder EP-005 · 403 — Veterinário
2026-11-04T18:56:53.3180410Z   GET http://localhost:5159/utilizadores/1 [403 Forbidden, 311B, 4ms]
2026-11-04T18:56:53.3209985Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.3212616Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:53.3247526Z   Pass  Devolve 403
2026-11-04T18:56:53.3290332Z 
2026-11-04T18:56:53.3290854Z Sub-folder EP-006 · 201 — criar conta de funcionário (Rececionista)
2026-11-04T18:56:53.4818826Z   POST http://localhost:5159/utilizadores [201 Created, 356B, 145ms]
2026-11-04T18:56:53.4854134Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.4887936Z   Pass  Devolve 201
2026-11-04T18:56:53.4890391Z   Pass  Conta ativa com os dados enviados
2026-11-04T18:56:53.4893911Z   Pass  Location indica a conta criada
2026-11-04T18:56:53.4934996Z 
2026-11-04T18:56:53.4935499Z Sub-folder EP-006 · 400 — palavra-passe fraca
2026-11-04T18:56:53.5012725Z   POST http://localhost:5159/utilizadores [400 Bad Request, 348B, 5ms]
2026-11-04T18:56:53.5042699Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.5044905Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:53.5076189Z   Pass  Devolve 400
2026-11-04T18:56:53.5105033Z 
2026-11-04T18:56:53.5105437Z Sub-folder EP-006 · 409 — e-mail já registado
2026-11-04T18:56:53.5189814Z   POST http://localhost:5159/utilizadores [409 Conflict, 258B, 5ms]
2026-11-04T18:56:53.5220468Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.5237358Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:53.5264388Z   Pass  Devolve 409
2026-11-04T18:56:53.5316314Z 
2026-11-04T18:56:53.5316709Z Sub-folder EP-006 · 422 — perfil Cliente
2026-11-04T18:56:53.5403326Z   POST http://localhost:5159/utilizadores [422 Unprocessable Entity, 367B, 5ms]
2026-11-04T18:56:53.5423116Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.5425365Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:53.5456237Z   Pass  Devolve 422
2026-11-04T18:56:53.5493031Z 
2026-11-04T18:56:53.5493536Z Sub-folder EP-006 · 422 — podeRegistarApoio para um Veterinário
2026-11-04T18:56:53.5573713Z   POST http://localhost:5159/utilizadores [422 Unprocessable Entity, 344B, 5ms]
2026-11-04T18:56:53.5593483Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.5595946Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:53.5627768Z   Pass  Devolve 422
2026-11-04T18:56:53.5654953Z 
2026-11-04T18:56:53.5655299Z Sub-folder EP-006 · 401 — sem token
2026-11-04T18:56:53.5700628Z   POST http://localhost:5159/utilizadores [401 Unauthorized, 269B, 1ms]
2026-11-04T18:56:53.5729945Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.5732172Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:53.5773684Z   Pass  Devolve 401
2026-11-04T18:56:53.5816877Z 
2026-11-04T18:56:53.5817301Z Sub-folder EP-006 · 403 — Rececionista
2026-11-04T18:56:53.5888941Z   POST http://localhost:5159/utilizadores [403 Forbidden, 311B, 4ms]
2026-11-04T18:56:53.5918275Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.5920553Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:53.5950707Z   Pass  Devolve 403
2026-11-04T18:56:53.5986047Z 
2026-11-04T18:56:53.5986458Z Sub-folder EP-006 · 200 — a conta criada inicia sessão (EP-001)
2026-11-04T18:56:53.6588540Z   POST http://localhost:5159/auth/login [200 OK, 741B, 57ms]
2026-11-04T18:56:53.6622708Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.6657581Z   Pass  Devolve 200
2026-11-04T18:56:53.6660126Z   Pass  Devolve um token JWT e a data de expiração
2026-11-04T18:56:53.6661090Z   Pass  O utilizador tem o perfil Rececionista
2026-11-04T18:56:53.6696222Z 
2026-11-04T18:56:53.6696794Z Sub-folder EP-007 · 200 — atualizar e atribuir o perfil Auxiliar com apoio
2026-11-04T18:56:53.7017715Z   PUT http://localhost:5159/utilizadores/11 [200 OK, 380B, 29ms]
2026-11-04T18:56:53.7054768Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.7113511Z   Pass  Devolve 200
2026-11-04T18:56:53.7118056Z   Pass  Devolve a conta atualizada (formato de EP-005)
2026-11-04T18:56:53.7141900Z 
2026-11-04T18:56:53.7142659Z Sub-folder EP-007 · 401 — o token com o perfil antigo deixa de ser aceite (EP-003)
2026-11-04T18:56:53.7208249Z   GET http://localhost:5159/auth/me [401 Unauthorized, 288B, 4ms]
2026-11-04T18:56:53.7239140Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.7240928Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:53.7278341Z   Pass  Devolve 401
2026-11-04T18:56:53.7313349Z 
2026-11-04T18:56:53.7313730Z Sub-folder EP-007 · 400 — dados inválidos
2026-11-04T18:56:53.7390737Z   PUT http://localhost:5159/utilizadores/11 [400 Bad Request, 344B, 4ms]
2026-11-04T18:56:53.7420504Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.7422642Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:53.7460927Z   Pass  Devolve 400
2026-11-04T18:56:53.7496593Z 
2026-11-04T18:56:53.7496969Z Sub-folder EP-007 · 400 — perfil Cliente
2026-11-04T18:56:53.7578227Z   PUT http://localhost:5159/utilizadores/11 [400 Bad Request, 334B, 5ms]
2026-11-04T18:56:53.7610655Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.7612509Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:53.7642365Z   Pass  Devolve 400
2026-11-04T18:56:53.7678265Z 
2026-11-04T18:56:53.7678695Z Sub-folder EP-007 · 404 — conta inexistente
2026-11-04T18:56:53.7775036Z   PUT http://localhost:5159/utilizadores/999999 [404 Not Found, 258B, 6ms]
2026-11-04T18:56:53.7805884Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.7807762Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:53.7837818Z   Pass  Devolve 404
2026-11-04T18:56:53.7865054Z 
2026-11-04T18:56:53.7865383Z Sub-folder EP-007 · 409 — e-mail usado por outra conta
2026-11-04T18:56:53.7966716Z   PUT http://localhost:5159/utilizadores/11 [409 Conflict, 262B, 6ms]
2026-11-04T18:56:53.7997196Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.7999359Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:53.8030471Z   Pass  Devolve 409
2026-11-04T18:56:53.8065943Z 
2026-11-04T18:56:53.8066430Z Sub-folder EP-007 · 422 — o Administrador remove o próprio perfil
2026-11-04T18:56:53.8159282Z   PUT http://localhost:5159/utilizadores/1 [422 Unprocessable Entity, 322B, 5ms]
2026-11-04T18:56:53.8190383Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.8192541Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:53.8222754Z   Pass  Devolve 422
2026-11-04T18:56:53.8260440Z 
2026-11-04T18:56:53.8260999Z Sub-folder EP-007 · 422 — podeRegistarApoio para um perfil diferente de Auxiliar
2026-11-04T18:56:53.8352434Z   PUT http://localhost:5159/utilizadores/11 [422 Unprocessable Entity, 344B, 7ms]
2026-11-04T18:56:53.8382714Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.8385380Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:53.8415271Z   Pass  Devolve 422
2026-11-04T18:56:53.8446204Z 
2026-11-04T18:56:53.8446509Z Sub-folder EP-007 · 401 — sem token
2026-11-04T18:56:53.8491319Z   PUT http://localhost:5159/utilizadores/11 [401 Unauthorized, 269B, 1ms]
2026-11-04T18:56:53.8520304Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.8522567Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:53.8551718Z   Pass  Devolve 401
2026-11-04T18:56:53.8586665Z 
2026-11-04T18:56:53.8586933Z Sub-folder EP-007 · 403 — Rececionista
2026-11-04T18:56:53.8660471Z   PUT http://localhost:5159/utilizadores/11 [403 Forbidden, 311B, 4ms]
2026-11-04T18:56:53.8690337Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.8692551Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:53.8728753Z   Pass  Devolve 403
2026-11-04T18:56:53.8764132Z 
2026-11-04T18:56:53.8764745Z Sub-folder EP-008 · preparação — sessão da conta com o perfil atual (EP-001 · 200)
2026-11-04T18:56:53.9440655Z   POST http://localhost:5159/auth/login [200 OK, 771B, 64ms]
2026-11-04T18:56:53.9476720Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.9508483Z   Pass  Devolve 200
2026-11-04T18:56:53.9511710Z   Pass  Devolve um token JWT e a data de expiração
2026-11-04T18:56:53.9513258Z   Pass  O utilizador tem o perfil AuxiliarVeterinario
2026-11-04T18:56:53.9545258Z 
2026-11-04T18:56:53.9545712Z Sub-folder EP-008 · 400 — estado diferente de Bloqueado
2026-11-04T18:56:53.9696379Z   PATCH http://localhost:5159/utilizadores/11/estado [400 Bad Request, 296B, 11ms]
2026-11-04T18:56:53.9726861Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.9729001Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:53.9761307Z   Pass  Devolve 400
2026-11-04T18:56:53.9796171Z 
2026-11-04T18:56:53.9796510Z Sub-folder EP-008 · 404 — conta inexistente
2026-11-04T18:56:53.9949453Z   PATCH http://localhost:5159/utilizadores/999999/estado [404 Not Found, 258B, 12ms]
2026-11-04T18:56:53.9982626Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:53.9984988Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:54.0016460Z   Pass  Devolve 404
2026-11-04T18:56:54.0056253Z 
2026-11-04T18:56:54.0056701Z Sub-folder EP-008 · 422 — o Administrador bloqueia a própria conta
2026-11-04T18:56:54.0148472Z   PATCH http://localhost:5159/utilizadores/1/estado [422 Unprocessable Entity, 305B, 5ms]
2026-11-04T18:56:54.0183703Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.0185373Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:54.0216344Z   Pass  Devolve 422
2026-11-04T18:56:54.0245081Z 
2026-11-04T18:56:54.0245577Z Sub-folder EP-008 · 422 — conta de Cliente
2026-11-04T18:56:54.0328197Z   PATCH http://localhost:5159/utilizadores/8/estado [422 Unprocessable Entity, 350B, 5ms]
2026-11-04T18:56:54.0358246Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.0360475Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:54.0391027Z   Pass  Devolve 422
2026-11-04T18:56:54.0416239Z 
2026-11-04T18:56:54.0416644Z Sub-folder EP-008 · 401 — sem token
2026-11-04T18:56:54.0461089Z   PATCH http://localhost:5159/utilizadores/11/estado [401 Unauthorized, 269B, 1ms]
2026-11-04T18:56:54.0489788Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.0492000Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:54.0522011Z   Pass  Devolve 401
2026-11-04T18:56:54.0555195Z 
2026-11-04T18:56:54.0555561Z Sub-folder EP-008 · 403 — Rececionista
2026-11-04T18:56:54.0629755Z   PATCH http://localhost:5159/utilizadores/11/estado [403 Forbidden, 311B, 3ms]
2026-11-04T18:56:54.0659576Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.0662034Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:54.0702709Z   Pass  Devolve 403
2026-11-04T18:56:54.0735099Z 
2026-11-04T18:56:54.0735560Z Sub-folder EP-008 · 204 — bloquear a conta
2026-11-04T18:56:54.0934650Z   PATCH http://localhost:5159/utilizadores/11/estado [204 No Content, 81B, 17ms]
2026-11-04T18:56:54.0966590Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.0998076Z   Pass  Devolve 204
2026-11-04T18:56:54.0999437Z   Pass  Sem corpo
2026-11-04T18:56:54.1036752Z 
2026-11-04T18:56:54.1037384Z Sub-folder EP-008 · 401 — o token emitido antes do bloqueio deixa de ser aceite (EP-003)
2026-11-04T18:56:54.1099707Z   GET http://localhost:5159/auth/me [401 Unauthorized, 288B, 3ms]
2026-11-04T18:56:54.1127970Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.1129844Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:54.1158564Z   Pass  Devolve 401
2026-11-04T18:56:54.1185873Z 
2026-11-04T18:56:54.1186375Z Sub-folder EP-008 · 403 — a conta bloqueada já não inicia sessão (EP-001)
2026-11-04T18:56:54.1765122Z   POST http://localhost:5159/auth/login [403 Forbidden, 296B, 54ms]
2026-11-04T18:56:54.1796962Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.1799012Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:54.1828386Z   Pass  Devolve 403
2026-11-04T18:56:54.1855374Z 
2026-11-04T18:56:54.1855773Z Sub-folder EP-008 · 422 — conta já bloqueada
2026-11-04T18:56:54.1947599Z   PATCH http://localhost:5159/utilizadores/11/estado [422 Unprocessable Entity, 281B, 5ms]
2026-11-04T18:56:54.1976677Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.1978537Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:54.2008321Z   Pass  Devolve 422
2026-11-04T18:56:54.2035830Z 
2026-11-04T18:56:54.2036131Z Folder 03 · Clínica (EP-009, EP-010)
2026-11-04T18:56:54.2036572Z Sub-folder EP-009 · 200 — consultar dados, configurações e horário
2026-11-04T18:56:54.2374201Z   GET http://localhost:5159/clinica [200 OK, 993B, 31ms]
2026-11-04T18:56:54.2406906Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.2437838Z   Pass  Devolve 200
2026-11-04T18:56:54.2443260Z   Pass  Tem os dados gerais e a duração das consultas
2026-11-04T18:56:54.2453059Z   Pass  Cada período tem dia, abertura e fecho
2026-11-04T18:56:54.2485691Z 
2026-11-04T18:56:54.2485958Z Sub-folder EP-009 · 401 — sem token
2026-11-04T18:56:54.2535903Z   GET http://localhost:5159/clinica [401 Unauthorized, 269B, 1ms]
2026-11-04T18:56:54.2560560Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.2562937Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:54.2590722Z   Pass  Devolve 401
2026-11-04T18:56:54.2624503Z 
2026-11-04T18:56:54.2624777Z Sub-folder EP-009 · 403 — Rececionista
2026-11-04T18:56:54.2693780Z   GET http://localhost:5159/clinica [403 Forbidden, 311B, 4ms]
2026-11-04T18:56:54.2722028Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.2724098Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:54.2753207Z   Pass  Devolve 403
2026-11-04T18:56:54.2784997Z 
2026-11-04T18:56:54.2785470Z Sub-folder EP-010 · 200 — atualizar a duração e o horário (vários períodos, fora de ordem)
2026-11-04T18:56:54.3748975Z   PUT http://localhost:5159/clinica [200 OK, 590B, 91ms]
2026-11-04T18:56:54.3782704Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.3814218Z   Pass  Devolve 200
2026-11-04T18:56:54.3816216Z   Pass  Guarda a nova duração
2026-11-04T18:56:54.3821917Z   Pass  Devolve o horário ordenado por dia e hora
2026-11-04T18:56:54.3857603Z 
2026-11-04T18:56:54.3857970Z Sub-folder EP-010 · 400 — períodos sobrepostos no mesmo dia
2026-11-04T18:56:54.3944815Z   PUT http://localhost:5159/clinica [400 Bad Request, 280B, 6ms]
2026-11-04T18:56:54.3975218Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.3977354Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:54.4008245Z   Pass  Devolve 400
2026-11-04T18:56:54.4046891Z 
2026-11-04T18:56:54.4047431Z Sub-folder EP-010 · 400 — hora de fecho igual à de abertura
2026-11-04T18:56:54.4118716Z   PUT http://localhost:5159/clinica [400 Bad Request, 270B, 4ms]
2026-11-04T18:56:54.4148499Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.4150607Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:54.4180403Z   Pass  Devolve 400
2026-11-04T18:56:54.4214779Z 
2026-11-04T18:56:54.4215160Z Sub-folder EP-010 · 400 — campos inválidos
2026-11-04T18:56:54.4284911Z   PUT http://localhost:5159/clinica [400 Bad Request, 608B, 4ms]
2026-11-04T18:56:54.4315969Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.4318192Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:54.4348416Z   Pass  Devolve 400
2026-11-04T18:56:54.4374910Z 
2026-11-04T18:56:54.4375225Z Sub-folder EP-010 · 401 — sem token
2026-11-04T18:56:54.4423093Z   PUT http://localhost:5159/clinica [401 Unauthorized, 269B, 1ms]
2026-11-04T18:56:54.4440608Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.4442494Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:54.4471445Z   Pass  Devolve 401
2026-11-04T18:56:54.4495757Z 
2026-11-04T18:56:54.4496062Z Sub-folder EP-010 · 403 — Rececionista
2026-11-04T18:56:54.4559258Z   PUT http://localhost:5159/clinica [403 Forbidden, 311B, 3ms]
2026-11-04T18:56:54.4588152Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.4590117Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:54.4619833Z   Pass  Devolve 403
2026-11-04T18:56:54.4644943Z 
2026-11-04T18:56:54.4645275Z Sub-folder EP-010 · 200 — repor os dados originais
2026-11-04T18:56:54.4840942Z   PUT http://localhost:5159/clinica [200 OK, 993B, 16ms]
2026-11-04T18:56:54.4878350Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.4934227Z   Pass  Devolve 200
2026-11-04T18:56:54.4939629Z   Pass  A clínica volta ao estado inicial
2026-11-04T18:56:54.4976002Z 
2026-11-04T18:56:54.4976332Z Folder 04 · Clientes (EP-011 a EP-014)
2026-11-04T18:56:54.4976683Z Sub-folder EP-011 · 200 — pesquisar clientes
2026-11-04T18:56:54.5445877Z   GET http://localhost:5159/clientes?pesquisa=silva [200 OK, 340B, 43ms]
2026-11-04T18:56:54.5480625Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.5512365Z   Pass  Devolve 200
2026-11-04T18:56:54.5515586Z   Pass  Envelope paginado (itens, pagina, tamanho, total)
2026-11-04T18:56:54.5519097Z   Pass  Encontra clientes e indica o número de animais
2026-11-04T18:56:54.5545873Z 
2026-11-04T18:56:54.5546153Z Sub-folder EP-011 · 400 — página inválida
2026-11-04T18:56:54.5627506Z   GET http://localhost:5159/clientes?pagina=0 [400 Bad Request, 252B, 5ms]
2026-11-04T18:56:54.5657533Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.5659711Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:54.5690718Z   Pass  Devolve 400
2026-11-04T18:56:54.5715932Z 
2026-11-04T18:56:54.5716211Z Sub-folder EP-011 · 401 — sem token
2026-11-04T18:56:54.5764351Z   GET http://localhost:5159/clientes [401 Unauthorized, 269B, 1ms]
2026-11-04T18:56:54.5792568Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.5794987Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:54.5822478Z   Pass  Devolve 401
2026-11-04T18:56:54.5856860Z 
2026-11-04T18:56:54.5857151Z Sub-folder EP-011 · 403 — Auxiliar Veterinário
2026-11-04T18:56:54.5940982Z   GET http://localhost:5159/clientes [403 Forbidden, 311B, 4ms]
2026-11-04T18:56:54.5969548Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.5971704Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:54.6008167Z   Pass  Devolve 403
2026-11-04T18:56:54.6043233Z 
2026-11-04T18:56:54.6043510Z Sub-folder EP-011 · 403 — Cliente
2026-11-04T18:56:54.6101717Z   GET http://localhost:5159/clientes [403 Forbidden, 311B, 4ms]
2026-11-04T18:56:54.6129997Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.6132209Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:54.6160082Z   Pass  Devolve 403
2026-11-04T18:56:54.6194459Z 
2026-11-04T18:56:54.6194824Z Sub-folder EP-012 · 200 — consultar cliente com os animais
2026-11-04T18:56:54.6598154Z   GET http://localhost:5159/clientes/1 [200 OK, 448B, 37ms]
2026-11-04T18:56:54.6631853Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.6664678Z   Pass  Devolve 200
2026-11-04T18:56:54.6667470Z   Pass  Devolve o Cliente do seed
2026-11-04T18:56:54.6670854Z   Pass  Lista os animais do Cliente
2026-11-04T18:56:54.6707481Z 
2026-11-04T18:56:54.6707970Z Sub-folder EP-012 · 404 — cliente inexistente
2026-11-04T18:56:54.6795127Z   GET http://localhost:5159/clientes/999999 [404 Not Found, 240B, 6ms]
2026-11-04T18:56:54.6824378Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.6826835Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:54.6854945Z   Pass  Devolve 404
2026-11-04T18:56:54.6924702Z 
2026-11-04T18:56:54.6953616Z Sub-folder EP-012 · 401 — sem token
2026-11-04T18:56:54.6983374Z   GET http://localhost:5159/clientes/1 [401 Unauthorized, 269B, 2ms]
2026-11-04T18:56:54.7006076Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.7006790Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:54.7018579Z   Pass  Devolve 401
2026-11-04T18:56:54.7077742Z 
2026-11-04T18:56:54.7078559Z Sub-folder EP-012 · 403 — Auxiliar Veterinário
2026-11-04T18:56:54.7159428Z   GET http://localhost:5159/clientes/1 [403 Forbidden, 311B, 4ms]
2026-11-04T18:56:54.7191248Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.7193332Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:54.7222515Z   Pass  Devolve 403
2026-11-04T18:56:54.7256201Z 
2026-11-04T18:56:54.7256554Z Sub-folder EP-013 · 201 — criar conta de Cliente
2026-11-04T18:56:54.8213736Z   POST http://localhost:5159/clientes [201 Created, 395B, 90ms]
2026-11-04T18:56:54.8247614Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.8279739Z   Pass  Devolve 201
2026-11-04T18:56:54.8282645Z   Pass  Cliente criado com a conta ativa
2026-11-04T18:56:54.8284822Z   Pass  Location indica a ficha
2026-11-04T18:56:54.8320630Z 
2026-11-04T18:56:54.8321119Z Sub-folder EP-013 · 200 — o Cliente criado inicia sessão (EP-001)
2026-11-04T18:56:54.8928387Z   POST http://localhost:5159/auth/login [200 OK, 750B, 57ms]
2026-11-04T18:56:54.8959924Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.8990573Z   Pass  Devolve 200
2026-11-04T18:56:54.8993850Z   Pass  Devolve um token JWT e a data de expiração
2026-11-04T18:56:54.8995277Z   Pass  O utilizador tem o perfil Cliente
2026-11-04T18:56:54.8996973Z   Pass  O token é do Cliente criado
2026-11-04T18:56:54.9029121Z 
2026-11-04T18:56:54.9029640Z Sub-folder EP-013 · 400 — campos em falta, NIF e e-mail inválidos
2026-11-04T18:56:54.9099829Z   POST http://localhost:5159/clientes [400 Bad Request, 418B, 4ms]
2026-11-04T18:56:54.9130473Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.9132682Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:54.9164229Z   Pass  Devolve 400
2026-11-04T18:56:54.9194953Z 
2026-11-04T18:56:54.9195243Z Sub-folder EP-013 · 409 — NIF já registado
2026-11-04T18:56:54.9446947Z   POST http://localhost:5159/clientes [409 Conflict, 256B, 22ms]
2026-11-04T18:56:54.9478756Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.9480603Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:54.9510461Z   Pass  Devolve 409
2026-11-04T18:56:54.9534739Z 
2026-11-04T18:56:54.9535276Z Sub-folder EP-013 · 409 — e-mail já registado (conta de funcionário)
2026-11-04T18:56:54.9617227Z   POST http://localhost:5159/clientes [409 Conflict, 258B, 5ms]
2026-11-04T18:56:54.9646235Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.9648098Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:54.9677693Z   Pass  Devolve 409
2026-11-04T18:56:54.9713470Z 
2026-11-04T18:56:54.9713893Z Sub-folder EP-013 · 401 — sem token
2026-11-04T18:56:54.9761038Z   POST http://localhost:5159/clientes [401 Unauthorized, 269B, 1ms]
2026-11-04T18:56:54.9790013Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.9792126Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:54.9821560Z   Pass  Devolve 401
2026-11-04T18:56:54.9866701Z 
2026-11-04T18:56:54.9867023Z Sub-folder EP-013 · 403 — Veterinário
2026-11-04T18:56:54.9935014Z   POST http://localhost:5159/clientes [403 Forbidden, 311B, 4ms]
2026-11-04T18:56:54.9964638Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:54.9967167Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:54.9995839Z   Pass  Devolve 403
2026-11-04T18:56:55.0031022Z 
2026-11-04T18:56:55.0031416Z Sub-folder EP-014 · 200 — atualizar cliente (e o e-mail da conta)
2026-11-04T18:56:55.0535629Z   PUT http://localhost:5159/clientes/4 [200 OK, 364B, 45ms]
2026-11-04T18:56:55.0568660Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.0599806Z   Pass  Devolve 200
2026-11-04T18:56:55.0602606Z   Pass  Devolve o cliente atualizado
2026-11-04T18:56:55.0636146Z 
2026-11-04T18:56:55.0636567Z Sub-folder EP-014 · 400 — campos inválidos
2026-11-04T18:56:55.0717666Z   PUT http://localhost:5159/clientes/4 [400 Bad Request, 366B, 4ms]
2026-11-04T18:56:55.0748953Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.0751093Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:55.0783572Z   Pass  Devolve 400
2026-11-04T18:56:55.0816055Z 
2026-11-04T18:56:55.0816545Z Sub-folder EP-014 · 404 — cliente inexistente
2026-11-04T18:56:55.0898580Z   PUT http://localhost:5159/clientes/999999 [404 Not Found, 240B, 5ms]
2026-11-04T18:56:55.0928440Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.0930297Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:55.0960361Z   Pass  Devolve 404
2026-11-04T18:56:55.0986518Z 
2026-11-04T18:56:55.0986935Z Sub-folder EP-014 · 409 — NIF de outro cliente
2026-11-04T18:56:55.1119300Z   PUT http://localhost:5159/clientes/4 [409 Conflict, 256B, 10ms]
2026-11-04T18:56:55.1149505Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.1151295Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:55.1180621Z   Pass  Devolve 409
2026-11-04T18:56:55.1215917Z 
2026-11-04T18:56:55.1216250Z Sub-folder EP-014 · 401 — sem token
2026-11-04T18:56:55.1257115Z   PUT http://localhost:5159/clientes/4 [401 Unauthorized, 269B, 1ms]
2026-11-04T18:56:55.1285375Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.1287184Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:55.1316053Z   Pass  Devolve 401
2026-11-04T18:56:55.1350680Z 
2026-11-04T18:56:55.1351059Z Sub-folder EP-014 · 403 — Veterinário
2026-11-04T18:56:55.1408911Z   PUT http://localhost:5159/clientes/4 [403 Forbidden, 311B, 3ms]
2026-11-04T18:56:55.1437938Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.1439753Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:55.1469446Z   Pass  Devolve 403
2026-11-04T18:56:55.1505457Z 
2026-11-04T18:56:55.1505806Z Folder 05 · Pacientes (EP-015 a EP-018)
2026-11-04T18:56:55.1506337Z Sub-folder EP-015 · 200 — pacientes de um Cliente
2026-11-04T18:56:55.1915707Z   GET http://localhost:5159/animais?clienteId=1 [200 OK, 490B, 38ms]
2026-11-04T18:56:55.1947817Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.1978653Z   Pass  Devolve 200
2026-11-04T18:56:55.1981435Z   Pass  Envelope paginado (itens, pagina, tamanho, total)
2026-11-04T18:56:55.1983253Z   Pass  Só animais do Cliente indicado
2026-11-04T18:56:55.1985644Z   Pass  Inclui o Bobi e a Mia
2026-11-04T18:56:55.2021260Z 
2026-11-04T18:56:55.2021706Z Sub-folder EP-015 · 200 — pesquisar por nome do animal
2026-11-04T18:56:55.2258215Z   GET http://localhost:5159/animais?pesquisa=Kiko [200 OK, 345B, 21ms]
2026-11-04T18:56:55.2289356Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.2319815Z   Pass  Devolve 200
2026-11-04T18:56:55.2322277Z   Pass  Encontra o Kiko, de outro Cliente
2026-11-04T18:56:55.2356024Z 
2026-11-04T18:56:55.2356415Z Sub-folder EP-015 · 400 — paginação inválida
2026-11-04T18:56:55.2441640Z   GET http://localhost:5159/animais?pagina=0&tamanho=101 [400 Bad Request, 299B, 4ms]
2026-11-04T18:56:55.2471838Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.2473422Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:55.2502670Z   Pass  Devolve 400
2026-11-04T18:56:55.2534729Z 
2026-11-04T18:56:55.2535356Z Sub-folder EP-015 · 401 — sem token
2026-11-04T18:56:55.2582727Z   GET http://localhost:5159/animais [401 Unauthorized, 269B, 2ms]
2026-11-04T18:56:55.2610866Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.2613283Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:55.2647983Z   Pass  Devolve 401
2026-11-04T18:56:55.2681920Z 
2026-11-04T18:56:55.2682182Z Sub-folder EP-015 · 403 — Cliente
2026-11-04T18:56:55.2753857Z   GET http://localhost:5159/animais [403 Forbidden, 311B, 4ms]
2026-11-04T18:56:55.2782110Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.2784534Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:55.2812387Z   Pass  Devolve 403
2026-11-04T18:56:55.2845697Z 
2026-11-04T18:56:55.2846131Z Sub-folder EP-016 · 200 — ficha administrativa (Auxiliar Veterinário)
2026-11-04T18:56:55.3192195Z   GET http://localhost:5159/animais/1 [200 OK, 387B, 31ms]
2026-11-04T18:56:55.3225133Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.3256207Z   Pass  Devolve 200
2026-11-04T18:56:55.3260715Z   Pass  Devolve a ficha com o proprietário
2026-11-04T18:56:55.3264197Z   Pass  Sem dados clínicos
2026-11-04T18:56:55.3299368Z 
2026-11-04T18:56:55.3299648Z Sub-folder EP-016 · 404 — animal inexistente
2026-11-04T18:56:55.3390321Z   GET http://localhost:5159/animais/999999 [404 Not Found, 239B, 5ms]
2026-11-04T18:56:55.3419656Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.3421837Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:55.3450168Z   Pass  Devolve 404
2026-11-04T18:56:55.3481065Z 
2026-11-04T18:56:55.3481326Z Sub-folder EP-016 · 401 — sem token
2026-11-04T18:56:55.3529936Z   GET http://localhost:5159/animais/1 [401 Unauthorized, 269B, 1ms]
2026-11-04T18:56:55.3558284Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.3560444Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:55.3587631Z   Pass  Devolve 401
2026-11-04T18:56:55.3621490Z 
2026-11-04T18:56:55.3621745Z Sub-folder EP-016 · 403 — Cliente
2026-11-04T18:56:55.3682459Z   GET http://localhost:5159/animais/1 [403 Forbidden, 311B, 4ms]
2026-11-04T18:56:55.3710246Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.3712473Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:55.3740233Z   Pass  Devolve 403
2026-11-04T18:56:55.3774714Z 
2026-11-04T18:56:55.3774968Z Sub-folder EP-017 · 201 — registar paciente
2026-11-04T18:56:55.4638352Z   POST http://localhost:5159/animais [201 Created, 433B, 82ms]
2026-11-04T18:56:55.4672110Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.4703550Z   Pass  Devolve 201
2026-11-04T18:56:55.4705691Z   Pass  Ficha criada para o Cliente indicado
2026-11-04T18:56:55.4707891Z   Pass  Location indica a ficha
2026-11-04T18:56:55.4745128Z 
2026-11-04T18:56:55.4826499Z Sub-folder EP-017 · 400 — peso igual a zero
2026-11-04T18:56:55.4826986Z   POST http://localhost:5159/animais [400 Bad Request, 240B, 4ms]
2026-11-04T18:56:55.4856529Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.4858699Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:55.4888093Z   Pass  Devolve 400
2026-11-04T18:56:55.4931693Z 
2026-11-04T18:56:55.4932163Z Sub-folder EP-017 · 400 — data de nascimento futura
2026-11-04T18:56:55.5037533Z   POST http://localhost:5159/animais [400 Bad Request, 246B, 4ms]
2026-11-04T18:56:55.5066995Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.5069139Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:55.5097652Z   Pass  Devolve 400
2026-11-04T18:56:55.5132983Z 
2026-11-04T18:56:55.5133468Z Sub-folder EP-017 · 400 — clienteId em falta
2026-11-04T18:56:55.5197922Z   POST http://localhost:5159/animais [400 Bad Request, 245B, 3ms]
2026-11-04T18:56:55.5226936Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.5228803Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:55.5259283Z   Pass  Devolve 400
2026-11-04T18:56:55.5285278Z 
2026-11-04T18:56:55.5285669Z Sub-folder EP-017 · 404 — cliente inexistente
2026-11-04T18:56:55.5364660Z   POST http://localhost:5159/animais [404 Not Found, 240B, 5ms]
2026-11-04T18:56:55.5394069Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.5395920Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:55.5425760Z   Pass  Devolve 404
2026-11-04T18:56:55.5455039Z 
2026-11-04T18:56:55.5455425Z Sub-folder EP-017 · 401 — sem token
2026-11-04T18:56:55.5503376Z   POST http://localhost:5159/animais [401 Unauthorized, 269B, 1ms]
2026-11-04T18:56:55.5525733Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.5527595Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:55.5556266Z   Pass  Devolve 401
2026-11-04T18:56:55.5584807Z 
2026-11-04T18:56:55.5585117Z Sub-folder EP-017 · 403 — Auxiliar Veterinário
2026-11-04T18:56:55.5652405Z   POST http://localhost:5159/animais [403 Forbidden, 311B, 4ms]
2026-11-04T18:56:55.5682037Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.5684457Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:55.5713076Z   Pass  Devolve 403
2026-11-04T18:56:55.5746118Z 
2026-11-04T18:56:55.5746381Z Sub-folder EP-017 · 403 — Cliente
2026-11-04T18:56:55.5809465Z   POST http://localhost:5159/animais [403 Forbidden, 311B, 3ms]
2026-11-04T18:56:55.5838420Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.5840519Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:55.5869237Z   Pass  Devolve 403
2026-11-04T18:56:55.5904688Z 
2026-11-04T18:56:55.5905155Z Sub-folder EP-018 · 200 — atualizar dados administrativos
2026-11-04T18:56:55.6197619Z   PUT http://localhost:5159/animais/5 [200 OK, 413B, 25ms]
2026-11-04T18:56:55.6230072Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.6261477Z   Pass  Devolve 200
2026-11-04T18:56:55.6264297Z   Pass  Devolve a ficha atualizada
2026-11-04T18:56:55.6307805Z 
2026-11-04T18:56:55.6308242Z Sub-folder EP-018 · 400 — peso inválido
2026-11-04T18:56:55.6374309Z   PUT http://localhost:5159/animais/5 [400 Bad Request, 240B, 4ms]
2026-11-04T18:56:55.6403356Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.6405614Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:55.6434946Z   Pass  Devolve 400
2026-11-04T18:56:55.6464969Z 
2026-11-04T18:56:55.6465311Z Sub-folder EP-018 · 400 — data de nascimento futura
2026-11-04T18:56:55.6577032Z   PUT http://localhost:5159/animais/5 [400 Bad Request, 246B, 5ms]
2026-11-04T18:56:55.6607255Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.6609408Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:55.6639414Z   Pass  Devolve 400
2026-11-04T18:56:55.6665058Z 
2026-11-04T18:56:55.6665489Z Sub-folder EP-018 · 404 — animal inexistente
2026-11-04T18:56:55.6738410Z   PUT http://localhost:5159/animais/999999 [404 Not Found, 239B, 4ms]
2026-11-04T18:56:55.6769003Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.6770992Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:55.6800021Z   Pass  Devolve 404
2026-11-04T18:56:55.6825022Z 
2026-11-04T18:56:55.6825408Z Sub-folder EP-018 · 404 — novo cliente inexistente
2026-11-04T18:56:55.6915364Z   PUT http://localhost:5159/animais/5 [404 Not Found, 240B, 5ms]
2026-11-04T18:56:55.6945462Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.6947564Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:55.6978590Z   Pass  Devolve 404
2026-11-04T18:56:55.7015674Z 
2026-11-04T18:56:55.7015967Z Sub-folder EP-018 · 401 — sem token
2026-11-04T18:56:55.7056070Z   PUT http://localhost:5159/animais/5 [401 Unauthorized, 269B, 1ms]
2026-11-04T18:56:55.7085284Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.7087451Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:55.7124485Z   Pass  Devolve 401
2026-11-04T18:56:55.7156440Z 
2026-11-04T18:56:55.7156783Z Sub-folder EP-018 · 403 — Cliente
2026-11-04T18:56:55.7229757Z   PUT http://localhost:5159/animais/5 [403 Forbidden, 311B, 3ms]
2026-11-04T18:56:55.7258957Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.7260705Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:55.7290170Z   Pass  Devolve 403
2026-11-04T18:56:55.7324726Z 
2026-11-04T18:56:55.7325203Z Folder 06 · Animais do Cliente — aplicação móvel (EP-019, EP-020)
2026-11-04T18:56:55.7325805Z Sub-folder EP-019 · 200 — listar os meus animais
2026-11-04T18:56:55.7556779Z   GET http://localhost:5159/me/animais [200 OK, 332B, 20ms]
2026-11-04T18:56:55.7587793Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.7617317Z   Pass  Devolve 200
2026-11-04T18:56:55.7620184Z   Pass  Devolve uma lista com os animais do Cliente
2026-11-04T18:56:55.7622019Z   Pass  Não inclui animais de outro Cliente
2026-11-04T18:56:55.7656136Z 
2026-11-04T18:56:55.7656761Z Sub-folder EP-019 · 200 — o Cliente criado vê só o animal registado em EP-017
2026-11-04T18:56:55.7746819Z   GET http://localhost:5159/me/animais [200 OK, 260B, 5ms]
2026-11-04T18:56:55.7775978Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.7804412Z   Pass  Devolve 200
2026-11-04T18:56:55.7807500Z   Pass  Um único animal: o criado nesta execução
2026-11-04T18:56:55.7841971Z 
2026-11-04T18:56:55.7842372Z Sub-folder EP-019 · 200 — o clienteId no pedido é ignorado
2026-11-04T18:56:55.7919438Z   GET http://localhost:5159/me/animais?clienteId=4 [200 OK, 332B, 4ms]
2026-11-04T18:56:55.7947992Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.7977143Z   Pass  Devolve 200
2026-11-04T18:56:55.7980003Z   Pass  Continua a devolver os animais do Cliente do token
2026-11-04T18:56:55.8015206Z 
2026-11-04T18:56:55.8015515Z Sub-folder EP-019 · 401 — sem token
2026-11-04T18:56:55.8063263Z   GET http://localhost:5159/me/animais [401 Unauthorized, 269B, 1ms]
2026-11-04T18:56:55.8080484Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.8082428Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:55.8110803Z   Pass  Devolve 401
2026-11-04T18:56:55.8134465Z 
2026-11-04T18:56:55.8134786Z Sub-folder EP-019 · 403 — perfil interno (Rececionista)
2026-11-04T18:56:55.8206639Z   GET http://localhost:5159/me/animais [403 Forbidden, 311B, 4ms]
2026-11-04T18:56:55.8235886Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.8238102Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:55.8273949Z   Pass  Devolve 403
2026-11-04T18:56:55.8309311Z 
2026-11-04T18:56:55.8309638Z Sub-folder EP-020 · 200 — consultar o meu animal
2026-11-04T18:56:55.8548086Z   GET http://localhost:5159/me/animais/1 [200 OK, 287B, 20ms]
2026-11-04T18:56:55.8579626Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.8610241Z   Pass  Devolve 200
2026-11-04T18:56:55.8614610Z   Pass  Devolve os dados básicos do animal
2026-11-04T18:56:55.8617491Z   Pass  Sem dados do proprietário nem clínicos
2026-11-04T18:56:55.8652160Z 
2026-11-04T18:56:55.8652516Z Sub-folder EP-020 · 403 — animal de outro Cliente
2026-11-04T18:56:55.8741380Z   GET http://localhost:5159/me/animais/2 [403 Forbidden, 302B, 5ms]
2026-11-04T18:56:55.8769931Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.8772124Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:55.8800923Z   Pass  Devolve 403
2026-11-04T18:56:55.8803151Z   Pass  Não revela dados do animal
2026-11-04T18:56:55.8837932Z 
2026-11-04T18:56:55.8838259Z Sub-folder EP-020 · 404 — animal inexistente
2026-11-04T18:56:55.8912743Z   GET http://localhost:5159/me/animais/999999 [404 Not Found, 239B, 5ms]
2026-11-04T18:56:55.8930536Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.8932675Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:55.8970152Z   Pass  Devolve 404
2026-11-04T18:56:55.9000573Z 
2026-11-04T18:56:55.9000897Z Sub-folder EP-020 · 401 — sem token
2026-11-04T18:56:55.9041935Z   GET http://localhost:5159/me/animais/1 [401 Unauthorized, 269B, 1ms]
2026-11-04T18:56:55.9069939Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.9072358Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:55.9100155Z   Pass  Devolve 401
2026-11-04T18:56:55.9134730Z 
2026-11-04T18:56:55.9135280Z Sub-folder EP-020 · 403 — perfil interno (Veterinário)
2026-11-04T18:56:55.9206350Z   GET http://localhost:5159/me/animais/1 [403 Forbidden, 311B, 4ms]
2026-11-04T18:56:55.9235325Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.9237326Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:55.9265263Z   Pass  Devolve 403
2026-11-04T18:56:55.9295057Z 
2026-11-04T18:56:55.9295426Z Folder 07 · Agenda e Consultas (EP-021 a EP-030)
2026-11-04T18:56:55.9295912Z Sub-folder EP-023 · 200 — agenda do Veterinário da sessão (só as suas marcações)
2026-11-04T18:56:55.9795522Z   GET http://localhost:5159/consultas?dataInicio=2026-10-01&dataFim=2026-12-31&tamanho=100 [200 OK, 1.2kB, 46ms]
2026-11-04T18:56:55.9828337Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:55.9859779Z   Pass  Devolve 200
2026-11-04T18:56:55.9863963Z   Pass  Resposta paginada (itens, pagina, tamanho, total)
2026-11-04T18:56:55.9865989Z   Pass  Só aparecem marcações do próprio Veterinário
2026-11-04T18:56:55.9870001Z   Pass  Cada marcação tem data, estado, animal, cliente e Veterinário
2026-11-04T18:56:55.9895713Z 
2026-11-04T18:56:55.9896073Z Sub-folder EP-021 · 200 — listar Veterinários (Rececionista)
2026-11-04T18:56:56.0103137Z   GET http://localhost:5159/veterinarios [200 OK, 264B, 17ms]
2026-11-04T18:56:56.0125079Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.0154640Z   Pass  Devolve 200
2026-11-04T18:56:56.0159212Z   Pass  Lista os Veterinários ativos com id e nome
2026-11-04T18:56:56.0161118Z   Pass  Inclui o Veterinário da sessão
2026-11-04T18:56:56.0205806Z 
2026-11-04T18:56:56.0206187Z Sub-folder EP-021 · 401 — sem token
2026-11-04T18:56:56.0254944Z   GET http://localhost:5159/veterinarios [401 Unauthorized, 269B, 1ms]
2026-11-04T18:56:56.0282711Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.0284838Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.0312130Z   Pass  Devolve 401
2026-11-04T18:56:56.0334693Z 
2026-11-04T18:56:56.0334998Z Sub-folder EP-021 · 403 — Veterinário
2026-11-04T18:56:56.0401039Z   GET http://localhost:5159/veterinarios [403 Forbidden, 311B, 3ms]
2026-11-04T18:56:56.0429374Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.0431154Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.0459453Z   Pass  Devolve 403
2026-11-04T18:56:56.0494185Z 
2026-11-04T18:56:56.0494787Z Sub-folder EP-023 · 200 — o Veterinário só vê as suas, mesmo pedindo outro veterinarioId
2026-11-04T18:56:56.0599446Z   GET http://localhost:5159/consultas?dataInicio=2026-10-01&dataFim=2026-12-31&tamanho=100&veterinarioId=5 [200 OK, 1.2kB, 7ms]
2026-11-04T18:56:56.0629420Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.0660512Z   Pass  Devolve 200
2026-11-04T18:56:56.0663945Z   Pass  O filtro veterinarioId é ignorado para o Veterinário
2026-11-04T18:56:56.0698800Z 
2026-11-04T18:56:56.0699154Z Sub-folder EP-023 · 200 — agenda da Rececionista (todos os Veterinários)
2026-11-04T18:56:56.0918473Z   GET http://localhost:5159/consultas?dataInicio=2026-10-01&dataFim=2026-12-31&tamanho=100 [200 OK, 1.84kB, 18ms]
2026-11-04T18:56:56.0950997Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.0989318Z   Pass  Devolve 200
2026-11-04T18:56:56.0992373Z   Pass  A agenda tem marcações de mais do que um Veterinário
2026-11-04T18:56:56.1028159Z 
2026-11-04T18:56:56.1028650Z Sub-folder EP-023 · 200 — filtrar por Veterinário e estado
2026-11-04T18:56:56.1264078Z   GET http://localhost:5159/consultas?dataInicio=2026-10-01&dataFim=2026-12-31&tamanho=100&veterinarioId=4&estado=Confirmada [200 OK, 518B, 20ms]
2026-11-04T18:56:56.1285674Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.1315792Z   Pass  Devolve 200
2026-11-04T18:56:56.1318589Z   Pass  Só marcações Confirmadas do Veterinário pedido
2026-11-04T18:56:56.1344979Z 
2026-11-04T18:56:56.1345369Z Sub-folder EP-023 · 200 — agenda do Auxiliar Veterinário
2026-11-04T18:56:56.1444096Z   GET http://localhost:5159/consultas?dataInicio=2026-10-01&dataFim=2026-12-31&tamanho=100 [200 OK, 1.84kB, 6ms]
2026-11-04T18:56:56.1473597Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.1505171Z   Pass  Devolve 200
2026-11-04T18:56:56.1507519Z   Pass  O Auxiliar vê a agenda
2026-11-04T18:56:56.1534810Z 
2026-11-04T18:56:56.1535273Z Sub-folder EP-023 · 400 — dataFim anterior a dataInicio
2026-11-04T18:56:56.1613367Z   GET http://localhost:5159/consultas?dataInicio=2026-12-02&dataFim=2026-12-01 [400 Bad Request, 290B, 4ms]
2026-11-04T18:56:56.1631570Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.1633553Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.1662421Z   Pass  Devolve 400
2026-11-04T18:56:56.1695594Z 
2026-11-04T18:56:56.1695901Z Sub-folder EP-023 · 401 — sem token
2026-11-04T18:56:56.1736264Z   GET http://localhost:5159/consultas [401 Unauthorized, 269B, 1ms]
2026-11-04T18:56:56.1765065Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.1766795Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.1794239Z   Pass  Devolve 401
2026-11-04T18:56:56.1828298Z 
2026-11-04T18:56:56.1828622Z Sub-folder EP-023 · 403 — Cliente
2026-11-04T18:56:56.1894633Z   GET http://localhost:5159/consultas [403 Forbidden, 311B, 4ms]
2026-11-04T18:56:56.1922672Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.1924788Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.1953295Z   Pass  Devolve 403
2026-11-04T18:56:56.1987573Z 
2026-11-04T18:56:56.1988000Z Sub-folder EP-022 · 200 — horários livres do Veterinário nos próximos 31 dias
2026-11-04T18:56:56.2466336Z   GET http://localhost:5159/veterinarios/4/disponibilidade?dataInicio=2026-11-05&dataFim=2026-12-05 [200 OK, 4.96kB, 41ms]
2026-11-04T18:56:56.2501486Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.2536589Z   Pass  Devolve 200
2026-11-04T18:56:56.2541361Z   Pass  Identifica o Veterinário, a duração e os dias do período
2026-11-04T18:56:56.2606105Z   Pass  Há pelo menos três horários livres (HH:MM)
2026-11-04T18:56:56.2652419Z 
2026-11-04T18:56:56.2652966Z Sub-folder EP-022 · 400 — dataInicio em falta
2026-11-04T18:56:56.2738594Z   GET http://localhost:5159/veterinarios/4/disponibilidade [400 Bad Request, 281B, 5ms]
2026-11-04T18:56:56.2768723Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.2770544Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.2800577Z   Pass  Devolve 400
2026-11-04T18:56:56.2835146Z 
2026-11-04T18:56:56.2835676Z Sub-folder EP-022 · 400 — período superior a 31 dias
2026-11-04T18:56:56.2909784Z   GET http://localhost:5159/veterinarios/4/disponibilidade?dataInicio=2026-12-01&dataFim=2027-01-15 [400 Bad Request, 252B, 3ms]
2026-11-04T18:56:56.2938643Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.2940987Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.2970668Z   Pass  Devolve 400
2026-11-04T18:56:56.3005436Z 
2026-11-04T18:56:56.3005812Z Sub-folder EP-022 · 404 — Veterinário inexistente
2026-11-04T18:56:56.3090914Z   GET http://localhost:5159/veterinarios/999999/disponibilidade?dataInicio=2026-11-05 [404 Not Found, 277B, 4ms]
2026-11-04T18:56:56.3111857Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.3114200Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.3143521Z   Pass  Devolve 404
2026-11-04T18:56:56.3178482Z 
2026-11-04T18:56:56.3178789Z Sub-folder EP-022 · 401 — sem token
2026-11-04T18:56:56.3226625Z   GET http://localhost:5159/veterinarios/4/disponibilidade?dataInicio=2026-11-05 [401 Unauthorized, 269B, 1ms]
2026-11-04T18:56:56.3256207Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.3258202Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.3294805Z   Pass  Devolve 401
2026-11-04T18:56:56.3329605Z 
2026-11-04T18:56:56.3329974Z Sub-folder EP-022 · 403 — Auxiliar Veterinário
2026-11-04T18:56:56.3401362Z   GET http://localhost:5159/veterinarios/4/disponibilidade?dataInicio=2026-11-05 [403 Forbidden, 311B, 3ms]
2026-11-04T18:56:56.3433607Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.3435763Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.3465883Z   Pass  Devolve 403
2026-11-04T18:56:56.3495030Z 
2026-11-04T18:56:56.3495507Z Sub-folder EP-025 · 201 — criar marcação (Pendente)
2026-11-04T18:56:56.4396012Z   POST http://localhost:5159/consultas [201 Created, 570B, 86ms]
2026-11-04T18:56:56.4430256Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.4463802Z   Pass  Devolve 201
2026-11-04T18:56:56.4466867Z   Pass  Cabeçalho Location aponta para a marcação criada
2026-11-04T18:56:56.4468659Z   Pass  Devolve o ETag da versão criada
2026-11-04T18:56:56.4471734Z   Pass  Marcação Pendente com o animal, o Veterinário e a hora pedidos
2026-11-04T18:56:56.4504886Z 
2026-11-04T18:56:56.4505364Z Sub-folder EP-022 · 200 — o horário marcado deixa de estar livre
2026-11-04T18:56:56.4609110Z   GET http://localhost:5159/veterinarios/4/disponibilidade?dataInicio=2026-11-05 [200 OK, 373B, 6ms]
2026-11-04T18:56:56.4637935Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.4668366Z   Pass  Devolve 200
2026-11-04T18:56:56.4672504Z   Pass  A hora da marcação criada já não aparece nos horários livres
2026-11-04T18:56:56.4715020Z 
2026-11-04T18:56:56.4715425Z Sub-folder EP-025 · 400 — campos em falta
2026-11-04T18:56:56.4787920Z   POST http://localhost:5159/consultas [400 Bad Request, 327B, 3ms]
2026-11-04T18:56:56.4817913Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.4819817Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.4850303Z   Pass  Devolve 400
2026-11-04T18:56:56.4885697Z 
2026-11-04T18:56:56.4886082Z Sub-folder EP-025 · 400 — data no passado
2026-11-04T18:56:56.4966587Z   POST http://localhost:5159/consultas [400 Bad Request, 245B, 5ms]
2026-11-04T18:56:56.4996474Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.4998526Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.5028247Z   Pass  Devolve 400
2026-11-04T18:56:56.5063561Z 
2026-11-04T18:56:56.5063940Z Sub-folder EP-025 · 404 — animal inexistente
2026-11-04T18:56:56.5147716Z   POST http://localhost:5159/consultas [404 Not Found, 239B, 5ms]
2026-11-04T18:56:56.5177297Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.5179423Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.5209369Z   Pass  Devolve 404
2026-11-04T18:56:56.5234740Z 
2026-11-04T18:56:56.5235027Z Sub-folder EP-025 · 404 — Veterinário inexistente
2026-11-04T18:56:56.5325321Z   POST http://localhost:5159/consultas [404 Not Found, 277B, 5ms]
2026-11-04T18:56:56.5355150Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.5356881Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.5387249Z   Pass  Devolve 404
2026-11-04T18:56:56.5415123Z 
2026-11-04T18:56:56.5415585Z Sub-folder EP-025 · 422 — Veterinário ocupado nesse horário
2026-11-04T18:56:56.5546558Z   POST http://localhost:5159/consultas [422 Unprocessable Entity, 340B, 9ms]
2026-11-04T18:56:56.5576688Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.5578515Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.5609336Z   Pass  Devolve 422
2026-11-04T18:56:56.5644825Z 
2026-11-04T18:56:56.5645337Z Sub-folder EP-025 · 422 — fora do horário de funcionamento
2026-11-04T18:56:56.5746690Z   POST http://localhost:5159/consultas [422 Unprocessable Entity, 397B, 7ms]
2026-11-04T18:56:56.5777326Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.5779496Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.5810105Z   Pass  Devolve 422
2026-11-04T18:56:56.5835745Z 
2026-11-04T18:56:56.5836137Z Sub-folder EP-025 · 401 — sem token
2026-11-04T18:56:56.5879979Z   POST http://localhost:5159/consultas [401 Unauthorized, 269B, 1ms]
2026-11-04T18:56:56.5908852Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.5910580Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.5940911Z   Pass  Devolve 401
2026-11-04T18:56:56.5983285Z 
2026-11-04T18:56:56.5983642Z Sub-folder EP-025 · 403 — Veterinário
2026-11-04T18:56:56.6044278Z   POST http://localhost:5159/consultas [403 Forbidden, 311B, 4ms]
2026-11-04T18:56:56.6073114Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.6075097Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.6105062Z   Pass  Devolve 403
2026-11-04T18:56:56.6140107Z 
2026-11-04T18:56:56.6140600Z Sub-folder EP-024 · 200 — consultar a marcação (Rececionista)
2026-11-04T18:56:56.6272759Z   GET http://localhost:5159/consultas/6 [200 OK, 520B, 11ms]
2026-11-04T18:56:56.6302322Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.6333568Z   Pass  Devolve 200
2026-11-04T18:56:56.6339858Z   Pass  Devolve a marcação com animal, cliente, Veterinário e notas de preparação
2026-11-04T18:56:56.6340313Z   Pass  O ETag é o da versão criada
2026-11-04T18:56:56.6365678Z 
2026-11-04T18:56:56.6366004Z Sub-folder EP-024 · 200 — o Veterinário da marcação consulta-a
2026-11-04T18:56:56.6453742Z   GET http://localhost:5159/consultas/6 [200 OK, 520B, 6ms]
2026-11-04T18:56:56.6482901Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.6513474Z   Pass  Devolve 200
2026-11-04T18:56:56.6515320Z   Pass  É a marcação do próprio Veterinário
2026-11-04T18:56:56.6545903Z 
2026-11-04T18:56:56.6546424Z Sub-folder EP-024 · 403 — marcação não atribuída ao Veterinário autenticado
2026-11-04T18:56:56.6636170Z   GET http://localhost:5159/consultas/2 [403 Forbidden, 330B, 5ms]
2026-11-04T18:56:56.6666849Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.6669041Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.6707799Z   Pass  Devolve 403
2026-11-04T18:56:56.6709968Z   Pass  Não devolve dados da marcação
2026-11-04T18:56:56.6803168Z 
2026-11-04T18:56:56.6803997Z Sub-folder EP-024 · 404 — marcação inexistente
2026-11-04T18:56:56.6903559Z   GET http://localhost:5159/consultas/999999 [404 Not Found, 251B, 12ms]
2026-11-04T18:56:56.6927836Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.6929558Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.6967816Z   Pass  Devolve 404
2026-11-04T18:56:56.7002327Z 
2026-11-04T18:56:56.7002645Z Sub-folder EP-024 · 401 — sem token
2026-11-04T18:56:56.7051775Z   GET http://localhost:5159/consultas/6 [401 Unauthorized, 269B, 2ms]
2026-11-04T18:56:56.7080412Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.7082935Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.7114334Z   Pass  Devolve 401
2026-11-04T18:56:56.7151617Z 
2026-11-04T18:56:56.7152011Z Sub-folder EP-024 · 403 — Cliente
2026-11-04T18:56:56.7215722Z   GET http://localhost:5159/consultas/6 [403 Forbidden, 311B, 4ms]
2026-11-04T18:56:56.7244381Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.7246093Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.7275878Z   Pass  Devolve 403
2026-11-04T18:56:56.7323255Z 
2026-11-04T18:56:56.7324055Z Sub-folder EP-026 · 200 — alterar a data e o motivo com o ETag atual (If-Match)
2026-11-04T18:56:56.7719114Z   PUT http://localhost:5159/consultas/6 [200 OK, 530B, 37ms]
2026-11-04T18:56:56.7753483Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.7786653Z   Pass  Devolve 200
2026-11-04T18:56:56.7789271Z   Pass  A marcação tem a nova data e o novo motivo e continua Pendente
2026-11-04T18:56:56.7790727Z   Pass  Devolve um ETag novo
2026-11-04T18:56:56.7826070Z 
2026-11-04T18:56:56.7826792Z Sub-folder EP-026 · 412 — If-Match com uma versão desatualizada
2026-11-04T18:56:56.7917593Z   PUT http://localhost:5159/consultas/6 [412 Precondition Failed, 386B, 6ms]
2026-11-04T18:56:56.7948080Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.7949652Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.7980571Z   Pass  Devolve 412
2026-11-04T18:56:56.8017443Z 
2026-11-04T18:56:56.8017846Z Sub-folder EP-026 · 400 — dataHora em falta
2026-11-04T18:56:56.8092167Z   PUT http://localhost:5159/consultas/6 [400 Bad Request, 266B, 3ms]
2026-11-04T18:56:56.8121589Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.8124209Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.8154237Z   Pass  Devolve 400
2026-11-04T18:56:56.8185776Z 
2026-11-04T18:56:56.8186118Z Sub-folder EP-026 · 404 — marcação inexistente
2026-11-04T18:56:56.8264041Z   PUT http://localhost:5159/consultas/999999 [404 Not Found, 251B, 5ms]
2026-11-04T18:56:56.8295566Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.8297989Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.8335902Z   Pass  Devolve 404
2026-11-04T18:56:56.8366165Z 
2026-11-04T18:56:56.8714538Z Sub-folder EP-026 · 422 — novo horário fora do horário de funcionamento
2026-11-04T18:56:56.8715070Z   PUT http://localhost:5159/consultas/6 [422 Unprocessable Entity, 397B, 31ms]
2026-11-04T18:56:56.8748476Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.8752430Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.8783238Z   Pass  Devolve 422
2026-11-04T18:56:56.8818429Z 
2026-11-04T18:56:56.8818752Z Sub-folder EP-026 · 401 — sem token
2026-11-04T18:56:56.8859002Z   PUT http://localhost:5159/consultas/6 [401 Unauthorized, 269B, 1ms]
2026-11-04T18:56:56.8887381Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.8889762Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.8920203Z   Pass  Devolve 401
2026-11-04T18:56:56.8957970Z 
2026-11-04T18:56:56.8958334Z Sub-folder EP-026 · 403 — Veterinário
2026-11-04T18:56:56.9029032Z   PUT http://localhost:5159/consultas/6 [403 Forbidden, 311B, 4ms]
2026-11-04T18:56:56.9058307Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.9060227Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.9089492Z   Pass  Devolve 403
2026-11-04T18:56:56.9114807Z 
2026-11-04T18:56:56.9115227Z Sub-folder EP-027 · 400 — Pendente não pode ser escolhido manualmente
2026-11-04T18:56:56.9267864Z   PATCH http://localhost:5159/consultas/6/estado [400 Bad Request, 258B, 11ms]
2026-11-04T18:56:56.9298940Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.9300659Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.9338382Z   Pass  Devolve 400
2026-11-04T18:56:56.9365819Z 
2026-11-04T18:56:56.9366437Z Sub-folder EP-027 · 400 — estado inexistente
2026-11-04T18:56:56.9447653Z   PATCH http://localhost:5159/consultas/6/estado [400 Bad Request, 303B, 5ms]
2026-11-04T18:56:56.9476725Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.9478529Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.9507717Z   Pass  Devolve 400
2026-11-04T18:56:56.9535264Z 
2026-11-04T18:56:56.9535738Z Sub-folder EP-028 · 422 — chegada numa marcação que não está Confirmada
2026-11-04T18:56:56.9732167Z   PATCH http://localhost:5159/consultas/6/chegada [422 Unprocessable Entity, 328B, 15ms]
2026-11-04T18:56:56.9763469Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:56.9765461Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:56.9794776Z   Pass  Devolve 422
2026-11-04T18:56:56.9825878Z 
2026-11-04T18:56:56.9826446Z Sub-folder EP-027 · 204 — confirmar a marcação (Pendente → Confirmada) com If-Match
2026-11-04T18:56:57.0102051Z   PATCH http://localhost:5159/consultas/6/estado [204 No Content, 103B, 23ms]
2026-11-04T18:56:57.0136787Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.0172015Z   Pass  Devolve 204
2026-11-04T18:56:57.0173986Z   Pass  Devolve um ETag novo
2026-11-04T18:56:57.0215297Z 
2026-11-04T18:56:57.0215870Z Sub-folder EP-027 · 412 — If-Match com uma versão desatualizada
2026-11-04T18:56:57.0308946Z   PATCH http://localhost:5159/consultas/6/estado [412 Precondition Failed, 386B, 5ms]
2026-11-04T18:56:57.0339234Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.0340991Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:57.0371938Z   Pass  Devolve 412
2026-11-04T18:56:57.0404678Z 
2026-11-04T18:56:57.0405136Z Sub-folder EP-028 · 412 — If-Match com uma versão desatualizada
2026-11-04T18:56:57.0486891Z   PATCH http://localhost:5159/consultas/6/chegada [412 Precondition Failed, 386B, 5ms]
2026-11-04T18:56:57.0515395Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.0517236Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:57.0546400Z   Pass  Devolve 412
2026-11-04T18:56:57.0549091Z   Pass  Erro no formato da API (I058): code 412 e message preenchida
2026-11-04T18:56:57.0574825Z 
2026-11-04T18:56:57.0575240Z Sub-folder EP-028 · 200 — registar a chegada com If-Match
2026-11-04T18:56:57.0697625Z   PATCH http://localhost:5159/consultas/6/chegada [200 OK, 213B, 8ms]
2026-11-04T18:56:57.0727599Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.0758454Z   Pass  Devolve 200
2026-11-04T18:56:57.0761390Z   Pass  Devolve a marcação e a hora de chegada do servidor
2026-11-04T18:56:57.0763716Z   Pass  Devolve um ETag novo
2026-11-04T18:56:57.0795887Z 
2026-11-04T18:56:57.0796269Z Sub-folder EP-028 · 422 — a chegada já foi registada
2026-11-04T18:56:57.0888636Z   PATCH http://localhost:5159/consultas/6/chegada [422 Unprocessable Entity, 302B, 5ms]
2026-11-04T18:56:57.0917960Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.0920117Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:57.0949934Z   Pass  Devolve 422
2026-11-04T18:56:57.0984309Z 
2026-11-04T18:56:57.0984714Z Sub-folder EP-028 · 404 — marcação inexistente
2026-11-04T18:56:57.1064692Z   PATCH http://localhost:5159/consultas/999999/chegada [404 Not Found, 251B, 5ms]
2026-11-04T18:56:57.1093587Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.1095521Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:57.1123805Z   Pass  Devolve 404
2026-11-04T18:56:57.1158456Z 
2026-11-04T18:56:57.1158798Z Sub-folder EP-028 · 401 — sem token
2026-11-04T18:56:57.1195487Z   PATCH http://localhost:5159/consultas/6/chegada [401 Unauthorized, 269B, 1ms]
2026-11-04T18:56:57.1223197Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.1225646Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:57.1254696Z   Pass  Devolve 401
2026-11-04T18:56:57.1289636Z 
2026-11-04T18:56:57.1290304Z Sub-folder EP-028 · 403 — Auxiliar Veterinário
2026-11-04T18:56:57.1351202Z   PATCH http://localhost:5159/consultas/6/chegada [403 Forbidden, 311B, 3ms]
2026-11-04T18:56:57.1379538Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.1381453Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:57.1410243Z   Pass  Devolve 403
2026-11-04T18:56:57.1445973Z 
2026-11-04T18:56:57.1446382Z Sub-folder EP-029 · 201 — registar nota de preparação (Auxiliar com apoio)
2026-11-04T18:56:57.1814612Z   POST http://localhost:5159/consultas/6/notas-preparacao [201 Created, 355B, 34ms]
2026-11-04T18:56:57.1847496Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.1879781Z   Pass  Devolve 201
2026-11-04T18:56:57.1883243Z   Pass  Devolve a nota com a marcação, o texto, o autor e a data
2026-11-04T18:56:57.1916015Z 
2026-11-04T18:56:57.1916647Z Sub-folder EP-024 · 200 — a marcação está Confirmada, com a chegada e a nota de preparação
2026-11-04T18:56:57.2018132Z   GET http://localhost:5159/consultas/6 [200 OK, 710B, 6ms]
2026-11-04T18:56:57.2047948Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.2077757Z   Pass  Devolve 200
2026-11-04T18:56:57.2080663Z   Pass  Estado Confirmada e chegada registada
2026-11-04T18:56:57.2082217Z   Pass  A nota de preparação aparece na marcação
2026-11-04T18:56:57.2083693Z   Pass  O registo da nota não mudou a versão da marcação (ETag)
2026-11-04T18:56:57.2115798Z 
2026-11-04T18:56:57.2116133Z Sub-folder EP-029 · 400 — texto vazio
2026-11-04T18:56:57.2188385Z   POST http://localhost:5159/consultas/6/notas-preparacao [400 Bad Request, 243B, 3ms]
2026-11-04T18:56:57.2217310Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.2219137Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:57.2248384Z   Pass  Devolve 400
2026-11-04T18:56:57.2283629Z 
2026-11-04T18:56:57.2284050Z Sub-folder EP-029 · 400 — texto com mais de 1000 caracteres
2026-11-04T18:56:57.2383663Z   POST http://localhost:5159/consultas/6/notas-preparacao [400 Bad Request, 265B, 4ms]
2026-11-04T18:56:57.2402242Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.2404365Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:57.2435235Z   Pass  Devolve 400
2026-11-04T18:56:57.2470711Z 
2026-11-04T18:56:57.2471419Z Sub-folder EP-029 · preparação — sessão do Auxiliar sem autorização de apoio (EP-001 · 200)
2026-11-04T18:56:57.3062080Z   POST http://localhost:5159/auth/login [200 OK, 732B, 56ms]
2026-11-04T18:56:57.3094445Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.3127752Z   Pass  Devolve 200
2026-11-04T18:56:57.3129489Z   Pass  É uma conta de Auxiliar Veterinário
2026-11-04T18:56:57.3165669Z 
2026-11-04T18:56:57.3166186Z Sub-folder EP-029 · 403 — Auxiliar sem autorização de registo de apoio
2026-11-04T18:56:57.3260983Z   POST http://localhost:5159/consultas/6/notas-preparacao [403 Forbidden, 362B, 5ms]
2026-11-04T18:56:57.3291973Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.3294319Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:57.3324687Z   Pass  Devolve 403
2026-11-04T18:56:57.3360059Z 
2026-11-04T18:56:57.3464499Z Sub-folder EP-029 · 404 — marcação inexistente
2026-11-04T18:56:57.3465170Z   POST http://localhost:5159/consultas/999999/notas-preparacao [404 Not Found, 251B, 7ms]
2026-11-04T18:56:57.3497264Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.3498683Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:57.3531400Z   Pass  Devolve 404
2026-11-04T18:56:57.3566984Z 
2026-11-04T18:56:57.3567411Z Sub-folder EP-029 · 401 — sem token
2026-11-04T18:56:57.3613242Z   POST http://localhost:5159/consultas/6/notas-preparacao [401 Unauthorized, 269B, 2ms]
2026-11-04T18:56:57.3631925Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.3649113Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:57.3690346Z   Pass  Devolve 401
2026-11-04T18:56:57.3715673Z 
2026-11-04T18:56:57.3716306Z Sub-folder EP-029 · 403 — Rececionista
2026-11-04T18:56:57.3794204Z   POST http://localhost:5159/consultas/6/notas-preparacao [403 Forbidden, 311B, 5ms]
2026-11-04T18:56:57.3824563Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.3826132Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:57.3858855Z   Pass  Devolve 403
2026-11-04T18:56:57.3895086Z 
2026-11-04T18:56:57.3895666Z Sub-folder EP-027 · 204 — marcar como Realizada (Confirmada → Realizada)
2026-11-04T18:56:57.4024801Z   PATCH http://localhost:5159/consultas/6/estado [204 No Content, 103B, 10ms]
2026-11-04T18:56:57.4056909Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.4098591Z   Pass  Devolve 204
2026-11-04T18:56:57.4134645Z 
2026-11-04T18:56:57.4135228Z Sub-folder EP-027 · 422 — transição inválida (Realizada → Confirmada)
2026-11-04T18:56:57.4233046Z   PATCH http://localhost:5159/consultas/6/estado [422 Unprocessable Entity, 346B, 7ms]
2026-11-04T18:56:57.4265399Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.4266747Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:57.4297711Z   Pass  Devolve 422
2026-11-04T18:56:57.4333990Z 
2026-11-04T18:56:57.4334588Z Sub-folder EP-026 · 422 — marcação Realizada não pode ser alterada
2026-11-04T18:56:57.4430404Z   PUT http://localhost:5159/consultas/6 [422 Unprocessable Entity, 318B, 6ms]
2026-11-04T18:56:57.4462045Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.4463491Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:57.4494296Z   Pass  Devolve 422
2026-11-04T18:56:57.4531051Z 
2026-11-04T18:56:57.4531416Z Sub-folder EP-025 · preparação — marcação a cancelar
2026-11-04T18:56:57.4712534Z   POST http://localhost:5159/consultas [201 Created, 573B, 15ms]
2026-11-04T18:56:57.4750001Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.4782192Z   Pass  Devolve 201
2026-11-04T18:56:57.4825184Z 
2026-11-04T18:56:57.4826237Z Sub-folder EP-027 · 204 — cancelar com observação (Pendente → Cancelada)
2026-11-04T18:56:57.4949919Z   PATCH http://localhost:5159/consultas/7/estado [204 No Content, 103B, 8ms]
2026-11-04T18:56:57.4981194Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.5012935Z   Pass  Devolve 204
2026-11-04T18:56:57.5045841Z 
2026-11-04T18:56:57.5046478Z Sub-folder EP-027 · 422 — transição inválida (Cancelada → Confirmada)
2026-11-04T18:56:57.5124668Z   PATCH http://localhost:5159/consultas/7/estado [422 Unprocessable Entity, 346B, 5ms]
2026-11-04T18:56:57.5154482Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.5156306Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:57.5187487Z   Pass  Devolve 422
2026-11-04T18:56:57.5214760Z 
2026-11-04T18:56:57.5215166Z Sub-folder EP-028 · 422 — chegada numa marcação Cancelada
2026-11-04T18:56:57.5298385Z   PATCH http://localhost:5159/consultas/7/chegada [422 Unprocessable Entity, 328B, 4ms]
2026-11-04T18:56:57.5329537Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.5331470Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:57.5384115Z   Pass  Devolve 422
2026-11-04T18:56:57.5415792Z 
2026-11-04T18:56:57.5416214Z Sub-folder EP-029 · 422 — nota de preparação numa marcação Cancelada
2026-11-04T18:56:57.5519416Z   POST http://localhost:5159/consultas/7/notas-preparacao [422 Unprocessable Entity, 348B, 6ms]
2026-11-04T18:56:57.5548869Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.5551001Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:57.5580724Z   Pass  Devolve 422
2026-11-04T18:56:57.5615658Z 
2026-11-04T18:56:57.5615966Z Sub-folder EP-027 · 404 — marcação inexistente
2026-11-04T18:56:57.5696492Z   PATCH http://localhost:5159/consultas/999999/estado [404 Not Found, 251B, 4ms]
2026-11-04T18:56:57.5725513Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.5727502Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:57.5757756Z   Pass  Devolve 404
2026-11-04T18:56:57.5793403Z 
2026-11-04T18:56:57.5793750Z Sub-folder EP-027 · 401 — sem token
2026-11-04T18:56:57.5841841Z   PATCH http://localhost:5159/consultas/6/estado [401 Unauthorized, 269B, 1ms]
2026-11-04T18:56:57.5871083Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.5873645Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:57.5903464Z   Pass  Devolve 401
2026-11-04T18:56:57.5939010Z 
2026-11-04T18:56:57.5939394Z Sub-folder EP-027 · 403 — Veterinário
2026-11-04T18:56:57.5999534Z   PATCH http://localhost:5159/consultas/6/estado [403 Forbidden, 311B, 3ms]
2026-11-04T18:56:57.6029935Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.6031875Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:57.6068952Z   Pass  Devolve 403
2026-11-04T18:56:57.6095680Z 
2026-11-04T18:56:57.6096133Z Sub-folder EP-030 · 200 — marcações futuras do Cliente
2026-11-04T18:56:57.6431371Z   GET http://localhost:5159/me/consultas [200 OK, 316B, 29ms]
2026-11-04T18:56:57.6464200Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.6496283Z   Pass  Devolve 200
2026-11-04T18:56:57.6499664Z   Pass  Só marcações Pendentes ou Confirmadas a partir de agora, por ordem de data
2026-11-04T18:56:57.6502191Z   Pass  Só informação administrativa: sem motivo nem dados clínicos
2026-11-04T18:56:57.6535149Z 
2026-11-04T18:56:57.6535657Z Sub-folder EP-030 · 200 — histórico com a marcação Realizada e a Cancelada
2026-11-04T18:56:57.6710615Z   GET http://localhost:5159/me/consultas?tipo=historico [200 OK, 821B, 14ms]
2026-11-04T18:56:57.6742582Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.6775044Z   Pass  Devolve 200
2026-11-04T18:56:57.6778323Z   Pass  O histórico tem a marcação Realizada desta pasta
2026-11-04T18:56:57.6780050Z   Pass  O histórico tem a marcação Cancelada, mesmo sendo futura
2026-11-04T18:56:57.6782655Z   Pass  Por ordem de data decrescente
2026-11-04T18:56:57.6814879Z 
2026-11-04T18:56:57.6815298Z Sub-folder EP-030 · 200 — filtrar por um animal próprio
2026-11-04T18:56:57.7005964Z   GET http://localhost:5159/me/consultas?tipo=historico&animalId=1 [200 OK, 653B, 16ms]
2026-11-04T18:56:57.7037463Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.7068297Z   Pass  Devolve 200
2026-11-04T18:56:57.7071596Z   Pass  Só marcações desse animal
2026-11-04T18:56:57.7105925Z 
2026-11-04T18:56:57.7106222Z Sub-folder EP-030 · 400 — tipo inválido
2026-11-04T18:56:57.7182025Z   GET http://localhost:5159/me/consultas?tipo=proximas [400 Bad Request, 256B, 4ms]
2026-11-04T18:56:57.7210919Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.7212902Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:57.7242055Z   Pass  Devolve 400
2026-11-04T18:56:57.7275824Z 
2026-11-04T18:56:57.7276311Z Sub-folder EP-030 · 400 — animalId inferior a 1
2026-11-04T18:56:57.7344828Z   GET http://localhost:5159/me/consultas?animalId=0 [400 Bad Request, 244B, 4ms]
2026-11-04T18:56:57.7373849Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.7376002Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:57.7404985Z   Pass  Devolve 400
2026-11-04T18:56:57.7439996Z 
2026-11-04T18:56:57.7440363Z Sub-folder EP-030 · 403 — animal de outro Cliente
2026-11-04T18:56:57.7521388Z   GET http://localhost:5159/me/consultas?animalId=2 [403 Forbidden, 302B, 4ms]
2026-11-04T18:56:57.7550631Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.7553170Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:57.7583323Z   Pass  Devolve 403
2026-11-04T18:56:57.7585760Z   Pass  Não devolve marcações
2026-11-04T18:56:57.7614449Z 
2026-11-04T18:56:57.7614742Z Sub-folder EP-030 · 401 — sem token
2026-11-04T18:56:57.7656194Z   GET http://localhost:5159/me/consultas [401 Unauthorized, 269B, 1ms]
2026-11-04T18:56:57.7684637Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.7687070Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:57.7723115Z   Pass  Devolve 401
2026-11-04T18:56:57.7758205Z 
2026-11-04T18:56:57.7758674Z Sub-folder EP-030 · 403 — perfil interno (Rececionista)
2026-11-04T18:56:57.7827391Z   GET http://localhost:5159/me/consultas [403 Forbidden, 311B, 4ms]
2026-11-04T18:56:57.7856426Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.7858812Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:57.7887143Z   Pass  Devolve 403
2026-11-04T18:56:57.7922344Z 
2026-11-04T18:56:57.7922759Z Folder 08 · Registo clínico (EP-031 a EP-034)
2026-11-04T18:56:57.7923448Z Sub-folder EP-023 · preparação — identificar o Veterinário da sessão
2026-11-04T18:56:57.8017718Z   GET http://localhost:5159/consultas?dataInicio=2026-10-01&dataFim=2026-12-31&tamanho=100 [200 OK, 1.89kB, 6ms]
2026-11-04T18:56:57.8049510Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.8080632Z   Pass  Devolve 200
2026-11-04T18:56:57.8083980Z   Pass  A agenda do Veterinário tem marcações (dados da I061)
2026-11-04T18:56:57.8086376Z   Pass  Só aparecem marcações do próprio Veterinário
2026-11-04T18:56:57.8115004Z 
2026-11-04T18:56:57.8115432Z Sub-folder EP-023 · preparação — marcação de outro Veterinário
2026-11-04T18:56:57.8205251Z   GET http://localhost:5159/consultas?dataInicio=2026-11-04&dataFim=2026-11-04&tamanho=100 [200 OK, 516B, 5ms]
2026-11-04T18:56:57.8234264Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.8265037Z   Pass  Devolve 200
2026-11-04T18:56:57.8267702Z   Pass  Existe uma marcação de outro Veterinário (M4 da I061)
2026-11-04T18:56:57.8309724Z 
2026-11-04T18:56:57.8310240Z Sub-folder EP-022 · preparação — próximos horários livres do Veterinário
2026-11-04T18:56:57.8442416Z   GET http://localhost:5159/veterinarios/4/disponibilidade?dataInicio=2026-11-05&dataFim=2026-12-05 [200 OK, 4.95kB, 7ms]
2026-11-04T18:56:57.8473500Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.8506280Z   Pass  Devolve 200
2026-11-04T18:56:57.8511024Z   Pass  Há pelo menos dois horários livres nos próximos 31 dias
2026-11-04T18:56:57.8547908Z 
2026-11-04T18:56:57.8548386Z Sub-folder EP-025 · preparação — marcação para o registo clínico
2026-11-04T18:56:57.8696048Z   POST http://localhost:5159/consultas [201 Created, 584B, 12ms]
2026-11-04T18:56:57.8728145Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.8759730Z   Pass  Devolve 201
2026-11-04T18:56:57.8762013Z   Pass  Marcação Pendente do Veterinário da sessão
2026-11-04T18:56:57.8794839Z 
2026-11-04T18:56:57.8795231Z Sub-folder EP-027 · preparação — confirmar a marcação
2026-11-04T18:56:57.8890065Z   PATCH http://localhost:5159/consultas/8/estado [204 No Content, 103B, 5ms]
2026-11-04T18:56:57.8920301Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.8951295Z   Pass  Devolve 204
2026-11-04T18:56:57.8994992Z 
2026-11-04T18:56:57.8995550Z Sub-folder EP-025 · preparação — marcação que fica Pendente
2026-11-04T18:56:57.9167109Z   POST http://localhost:5159/consultas [201 Created, 571B, 13ms]
2026-11-04T18:56:57.9200373Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:57.9232607Z   Pass  Devolve 201
2026-11-04T18:56:57.9269949Z 
2026-11-04T18:56:57.9270558Z Sub-folder EP-031 · 201 — criar registo clínico com tratamento e prescrição
2026-11-04T18:56:58.0277996Z   POST http://localhost:5159/registos-clinicos [201 Created, 675B, 98ms]
2026-11-04T18:56:58.0312003Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.0347814Z   Pass  Devolve 201
2026-11-04T18:56:58.0351203Z   Pass  Registo associado à consulta e ao paciente
2026-11-04T18:56:58.0354597Z   Pass  Um tratamento e uma prescrição, com identificador
2026-11-04T18:56:58.0356062Z   Pass  Location indica o registo
2026-11-04T18:56:58.0385823Z 
2026-11-04T18:56:58.0386171Z Sub-folder EP-031 · 400 — nenhum elemento clínico
2026-11-04T18:56:58.0472071Z   POST http://localhost:5159/registos-clinicos [400 Bad Request, 339B, 4ms]
2026-11-04T18:56:58.0502999Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.0505581Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.0534895Z   Pass  Devolve 400
2026-11-04T18:56:58.0566817Z 
2026-11-04T18:56:58.0567280Z Sub-folder EP-031 · 400 — prescrição incompleta
2026-11-04T18:56:58.0646780Z   POST http://localhost:5159/registos-clinicos [400 Bad Request, 300B, 5ms]
2026-11-04T18:56:58.0677058Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.0679563Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.0710293Z   Pass  Devolve 400
2026-11-04T18:56:58.0745733Z 
2026-11-04T18:56:58.0746089Z Sub-folder EP-031 · 404 — consulta inexistente
2026-11-04T18:56:58.0838611Z   POST http://localhost:5159/registos-clinicos [404 Not Found, 241B, 5ms]
2026-11-04T18:56:58.0869551Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.0871679Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.0902113Z   Pass  Devolve 404
2026-11-04T18:56:58.0937668Z 
2026-11-04T18:56:58.0938115Z Sub-folder EP-031 · 403 — consulta de outro Veterinário
2026-11-04T18:56:58.1018276Z   POST http://localhost:5159/registos-clinicos [403 Forbidden, 320B, 5ms]
2026-11-04T18:56:58.1050318Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.1051947Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.1082650Z   Pass  Devolve 403
2026-11-04T18:56:58.1118792Z 
2026-11-04T18:56:58.1119143Z Sub-folder EP-031 · 422 — marcação Pendente
2026-11-04T18:56:58.1207785Z   POST http://localhost:5159/registos-clinicos [422 Unprocessable Entity, 342B, 5ms]
2026-11-04T18:56:58.1238841Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.1241194Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.1279790Z   Pass  Devolve 422
2026-11-04T18:56:58.1315459Z 
2026-11-04T18:56:58.1315829Z Sub-folder EP-031 · 409 — a consulta já tem registo clínico
2026-11-04T18:56:58.1407039Z   POST http://localhost:5159/registos-clinicos [409 Conflict, 300B, 6ms]
2026-11-04T18:56:58.1437765Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.1439487Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.1470465Z   Pass  Devolve 409
2026-11-04T18:56:58.1506520Z 
2026-11-04T18:56:58.1506870Z Sub-folder EP-031 · 401 — sem token
2026-11-04T18:56:58.1555730Z   POST http://localhost:5159/registos-clinicos [401 Unauthorized, 269B, 1ms]
2026-11-04T18:56:58.1581574Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.1583539Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.1612498Z   Pass  Devolve 401
2026-11-04T18:56:58.1645039Z 
2026-11-04T18:56:58.1645502Z Sub-folder EP-031 · 403 — Auxiliar Veterinário (só leitura)
2026-11-04T18:56:58.1718462Z   POST http://localhost:5159/registos-clinicos [403 Forbidden, 311B, 3ms]
2026-11-04T18:56:58.1747957Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.1749877Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.1780049Z   Pass  Devolve 403
2026-11-04T18:56:58.1822008Z 
2026-11-04T18:56:58.1822641Z Sub-folder EP-032 · 200 — atualizar notas, diagnóstico, tratamentos e prescrições
2026-11-04T18:56:58.2322634Z   PUT http://localhost:5159/registos-clinicos/2 [200 OK, 764B, 46ms]
2026-11-04T18:56:58.2356070Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.2388795Z   Pass  Devolve 200
2026-11-04T18:56:58.2390880Z   Pass  Notas e diagnóstico atualizados; consulta e paciente mantêm-se
2026-11-04T18:56:58.2393476Z   Pass  O tratamento indicado mantém-se e o novo é acrescentado
2026-11-04T18:56:58.2395472Z   Pass  A prescrição indicada é alterada
2026-11-04T18:56:58.2397764Z   Pass  Regista atualizadoEm e atualizadoPor
2026-11-04T18:56:58.2434287Z 
2026-11-04T18:56:58.2434768Z Sub-folder EP-032 · 400 — o registo ficaria sem elemento clínico
2026-11-04T18:56:58.2513304Z   PUT http://localhost:5159/registos-clinicos/2 [400 Bad Request, 339B, 4ms]
2026-11-04T18:56:58.2533274Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.2535838Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.2565906Z   Pass  Devolve 400
2026-11-04T18:56:58.2595021Z 
2026-11-04T18:56:58.2595471Z Sub-folder EP-032 · 400 — tratamento que não é deste registo
2026-11-04T18:56:58.2694278Z   PUT http://localhost:5159/registos-clinicos/2 [400 Bad Request, 273B, 6ms]
2026-11-04T18:56:58.2723873Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.2726041Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.2755909Z   Pass  Devolve 400
2026-11-04T18:56:58.2784939Z 
2026-11-04T18:56:58.2785256Z Sub-folder EP-032 · 404 — registo inexistente
2026-11-04T18:56:58.2863814Z   PUT http://localhost:5159/registos-clinicos/999999 [404 Not Found, 253B, 5ms]
2026-11-04T18:56:58.2893872Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.2895675Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.2927270Z   Pass  Devolve 404
2026-11-04T18:56:58.2954780Z 
2026-11-04T18:56:58.2955068Z Sub-folder EP-032 · 401 — sem token
2026-11-04T18:56:58.3001694Z   PUT http://localhost:5159/registos-clinicos/2 [401 Unauthorized, 269B, 0µs]
2026-11-04T18:56:58.3020511Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.3022713Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.3052252Z   Pass  Devolve 401
2026-11-04T18:56:58.3085817Z 
2026-11-04T18:56:58.3086178Z Sub-folder EP-032 · 403 — Auxiliar Veterinário (só leitura)
2026-11-04T18:56:58.3152458Z   PUT http://localhost:5159/registos-clinicos/2 [403 Forbidden, 311B, 4ms]
2026-11-04T18:56:58.3180721Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.3182517Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.3212372Z   Pass  Devolve 403
2026-11-04T18:56:58.3245544Z 
2026-11-04T18:56:58.3245987Z Sub-folder EP-033 · 200 — consultar o registo (Veterinário)
2026-11-04T18:56:58.3450430Z   GET http://localhost:5159/registos-clinicos/2 [200 OK, 764B, 16ms]
2026-11-04T18:56:58.3482529Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.3522251Z   Pass  Devolve 200
2026-11-04T18:56:58.3526152Z   Pass  Registo completo, já com a atualização
2026-11-04T18:56:58.3561782Z 
2026-11-04T18:56:58.3562354Z Sub-folder EP-033 · 200 — consultar o registo (Auxiliar Veterinário, só leitura)
2026-11-04T18:56:58.3656781Z   GET http://localhost:5159/registos-clinicos/2 [200 OK, 764B, 7ms]
2026-11-04T18:56:58.3686197Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.3716447Z   Pass  Devolve 200
2026-11-04T18:56:58.3721307Z   Pass  Registo completo, já com a atualização
2026-11-04T18:56:58.3744953Z 
2026-11-04T18:56:58.3745357Z Sub-folder EP-033 · 404 — registo inexistente
2026-11-04T18:56:58.3820833Z   GET http://localhost:5159/registos-clinicos/999999 [404 Not Found, 253B, 4ms]
2026-11-04T18:56:58.3849673Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.3851964Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.3880658Z   Pass  Devolve 404
2026-11-04T18:56:58.3905703Z 
2026-11-04T18:56:58.3906057Z Sub-folder EP-033 · 401 — sem token
2026-11-04T18:56:58.3951426Z   GET http://localhost:5159/registos-clinicos/2 [401 Unauthorized, 269B, 0µs]
2026-11-04T18:56:58.3978866Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.3981025Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.4009457Z   Pass  Devolve 401
2026-11-04T18:56:58.4050093Z 
2026-11-04T18:56:58.4050374Z Sub-folder EP-033 · 403 — Rececionista
2026-11-04T18:56:58.4120913Z   GET http://localhost:5159/registos-clinicos/2 [403 Forbidden, 311B, 3ms]
2026-11-04T18:56:58.4173946Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.4218175Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.4255558Z   Pass  Devolve 403
2026-11-04T18:56:58.4266209Z 
2026-11-04T18:56:58.4266583Z Sub-folder EP-033 · 403 — Cliente
2026-11-04T18:56:58.4359194Z   GET http://localhost:5159/registos-clinicos/2 [403 Forbidden, 311B, 6ms]
2026-11-04T18:56:58.4391567Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.4393637Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.4424015Z   Pass  Devolve 403
2026-11-04T18:56:58.4459013Z 
2026-11-04T18:56:58.4459473Z Sub-folder EP-034 · 200 — histórico clínico do paciente
2026-11-04T18:56:58.4824720Z   GET http://localhost:5159/animais/1/historico-clinico [200 OK, 569B, 34ms]
2026-11-04T18:56:58.4857088Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.4887997Z   Pass  Devolve 200
2026-11-04T18:56:58.4890719Z   Pass  Histórico do animal pedido, com o registo criado
2026-11-04T18:56:58.4891926Z   Pass  Do registo mais recente para o mais antigo
2026-11-04T18:56:58.4893842Z   Pass  O resumo conta os tratamentos e as prescrições
2026-11-04T18:56:58.4928905Z 
2026-11-04T18:56:58.4929309Z Sub-folder EP-034 · 200 — histórico filtrado por datas
2026-11-04T18:56:58.5144048Z   GET http://localhost:5159/animais/1/historico-clinico?dataInicio=2026-11-05&dataFim=2026-11-05 [200 OK, 396B, 18ms]
2026-11-04T18:56:58.5175583Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.5206483Z   Pass  Devolve 200
2026-11-04T18:56:58.5209717Z   Pass  Só registos do dia pedido, incluindo o registo criado
2026-11-04T18:56:58.5244745Z 
2026-11-04T18:56:58.5245212Z Sub-folder EP-034 · 200 — animal sem registos (Auxiliar Veterinário)
2026-11-04T18:56:58.5337898Z   GET http://localhost:5159/animais/5/historico-clinico [200 OK, 228B, 5ms]
2026-11-04T18:56:58.5367530Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.5398166Z   Pass  Devolve 200
2026-11-04T18:56:58.5400273Z   Pass  Lista de registos vazia
2026-11-04T18:56:58.5435920Z 
2026-11-04T18:56:58.5436419Z Sub-folder EP-034 · 400 — dataFim anterior a dataInicio
2026-11-04T18:56:58.5508502Z   GET http://localhost:5159/animais/1/historico-clinico?dataInicio=2026-10-15&dataFim=2026-10-14 [400 Bad Request, 290B, 3ms]
2026-11-04T18:56:58.5538279Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.5540527Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.5570245Z   Pass  Devolve 400
2026-11-04T18:56:58.5605390Z 
2026-11-04T18:56:58.5605719Z Sub-folder EP-034 · 404 — animal inexistente
2026-11-04T18:56:58.5689388Z   GET http://localhost:5159/animais/999999/historico-clinico [404 Not Found, 239B, 4ms]
2026-11-04T18:56:58.5718987Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.5721826Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.5751423Z   Pass  Devolve 404
2026-11-04T18:56:58.5786235Z 
2026-11-04T18:56:58.5786613Z Sub-folder EP-034 · 401 — sem token
2026-11-04T18:56:58.5826801Z   GET http://localhost:5159/animais/1/historico-clinico [401 Unauthorized, 269B, 1ms]
2026-11-04T18:56:58.5855162Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.5857357Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.5885437Z   Pass  Devolve 401
2026-11-04T18:56:58.5919875Z 
2026-11-04T18:56:58.5920216Z Sub-folder EP-034 · 403 — Rececionista
2026-11-04T18:56:58.5991744Z   GET http://localhost:5159/animais/1/historico-clinico [403 Forbidden, 311B, 3ms]
2026-11-04T18:56:58.6020827Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.6023081Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.6060999Z   Pass  Devolve 403
2026-11-04T18:56:58.6096707Z 
2026-11-04T18:56:58.6097221Z Sub-folder EP-027 · preparação — cancelar a marcação Pendente (limpeza)
2026-11-04T18:56:58.6193119Z   PATCH http://localhost:5159/consultas/9/estado [204 No Content, 103B, 6ms]
2026-11-04T18:56:58.6223514Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.6253624Z   Pass  Devolve 204
2026-11-04T18:56:58.6285946Z 
2026-11-04T18:56:58.6286456Z Folder 09 · Boletim de vacinas e desparasitações (EP-042 a EP-044)
2026-11-04T18:56:58.6286941Z Sub-folder EP-043 · 201 — registar vacinação com intervalo
2026-11-04T18:56:58.6796450Z   POST http://localhost:5159/animais/1/vacinacoes [201 Created, 415B, 47ms]
2026-11-04T18:56:58.6829518Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.6861885Z   Pass  Devolve 201
2026-11-04T18:56:58.6864712Z   Pass  Vacinação registada com os dados enviados
2026-11-04T18:56:58.6866178Z   Pass  Próxima administração = data + intervalo (2026-10-14 + 365 dias)
2026-11-04T18:56:58.6867554Z   Pass  Location indica a administração do animal
2026-11-04T18:56:58.6895774Z 
2026-11-04T18:56:58.6896228Z Sub-folder EP-043 · 201 — registar desparasitação sem intervalo
2026-11-04T18:56:58.7009788Z   POST http://localhost:5159/animais/1/vacinacoes [201 Created, 421B, 7ms]
2026-11-04T18:56:58.7040860Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.7072184Z   Pass  Devolve 201
2026-11-04T18:56:58.7074891Z   Pass  Sem intervalo, não há próxima administração
2026-11-04T18:56:58.7118986Z 
2026-11-04T18:56:58.7119344Z Sub-folder EP-043 · 400 — data de administração futura
2026-11-04T18:56:58.7206093Z   POST http://localhost:5159/animais/1/vacinacoes [400 Bad Request, 259B, 6ms]
2026-11-04T18:56:58.7235999Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.7238139Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.7268007Z   Pass  Devolve 400
2026-11-04T18:56:58.7296114Z 
2026-11-04T18:56:58.7296473Z Sub-folder EP-043 · 400 — intervaloDias igual a zero
2026-11-04T18:56:58.7359795Z   POST http://localhost:5159/animais/1/vacinacoes [400 Bad Request, 284B, 3ms]
2026-11-04T18:56:58.7388445Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.7390625Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.7419968Z   Pass  Devolve 400
2026-11-04T18:56:58.7455356Z 
2026-11-04T18:56:58.7455667Z Sub-folder EP-043 · 400 — tipo inválido e lote em falta
2026-11-04T18:56:58.7526428Z   POST http://localhost:5159/animais/1/vacinacoes [400 Bad Request, 276B, 3ms]
2026-11-04T18:56:58.7556850Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.7558670Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.7589373Z   Pass  Devolve 400
2026-11-04T18:56:58.7621581Z 
2026-11-04T18:56:58.7621968Z Sub-folder EP-043 · 404 — animal inexistente
2026-11-04T18:56:58.7705266Z   POST http://localhost:5159/animais/999999/vacinacoes [404 Not Found, 239B, 5ms]
2026-11-04T18:56:58.7735081Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.7737145Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.7768001Z   Pass  Devolve 404
2026-11-04T18:56:58.7803755Z 
2026-11-04T18:56:58.7804087Z Sub-folder EP-043 · 401 — sem token
2026-11-04T18:56:58.7849405Z   POST http://localhost:5159/animais/1/vacinacoes [401 Unauthorized, 269B, 1ms]
2026-11-04T18:56:58.7881974Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.7884579Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.7916355Z   Pass  Devolve 401
2026-11-04T18:56:58.7945951Z 
2026-11-04T18:56:58.7946384Z Sub-folder EP-043 · 403 — Auxiliar Veterinário
2026-11-04T18:56:58.8016204Z   POST http://localhost:5159/animais/1/vacinacoes [403 Forbidden, 311B, 4ms]
2026-11-04T18:56:58.8045830Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.8047530Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.8083441Z   Pass  Devolve 403
2026-11-04T18:56:58.8118840Z 
2026-11-04T18:56:58.8119171Z Sub-folder EP-043 · 403 — Cliente
2026-11-04T18:56:58.8187926Z   POST http://localhost:5159/animais/1/vacinacoes [403 Forbidden, 311B, 3ms]
2026-11-04T18:56:58.8216878Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.8219085Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.8255883Z   Pass  Devolve 403
2026-11-04T18:56:58.8285557Z 
2026-11-04T18:56:58.8285886Z Sub-folder EP-042 · 200 — boletim completo do paciente
2026-11-04T18:56:58.8570733Z   GET http://localhost:5159/animais/1/vacinacoes [200 OK, 1.68kB, 24ms]
2026-11-04T18:56:58.8603820Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.8635789Z   Pass  Devolve 200
2026-11-04T18:56:58.8639385Z   Pass  Boletim do animal pedido, com as duas administrações registadas
2026-11-04T18:56:58.8651153Z   Pass  Cada administração tem lote, intervalo, próxima e Veterinário
2026-11-04T18:56:58.8653356Z   Pass  Administrações da mais recente para a mais antiga
2026-11-04T18:56:58.8655407Z   Pass  Próximas da mais próxima para a mais distante
2026-11-04T18:56:58.8690669Z 
2026-11-04T18:56:58.8691099Z Sub-folder EP-042 · 200 — filtrar por tipo (Desparasitacao)
2026-11-04T18:56:58.8858643Z   GET http://localhost:5159/animais/1/vacinacoes?tipo=Desparasitacao [200 OK, 811B, 14ms]
2026-11-04T18:56:58.8891081Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.8921808Z   Pass  Devolve 200
2026-11-04T18:56:58.8925773Z   Pass  Só desparasitações, nas administrações e nas próximas
2026-11-04T18:56:58.8960281Z 
2026-11-04T18:56:58.8960645Z Sub-folder EP-042 · 400 — tipo inválido
2026-11-04T18:56:58.9037151Z   GET http://localhost:5159/animais/1/vacinacoes?tipo=Antibiotico [400 Bad Request, 252B, 4ms]
2026-11-04T18:56:58.9066586Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.9068849Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.9098835Z   Pass  Devolve 400
2026-11-04T18:56:58.9142418Z 
2026-11-04T18:56:58.9142972Z Sub-folder EP-042 · 404 — animal inexistente
2026-11-04T18:56:58.9226473Z   GET http://localhost:5159/animais/999999/vacinacoes [404 Not Found, 239B, 5ms]
2026-11-04T18:56:58.9255949Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.9258045Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.9287214Z   Pass  Devolve 404
2026-11-04T18:56:58.9315384Z 
2026-11-04T18:56:58.9315694Z Sub-folder EP-042 · 401 — sem token
2026-11-04T18:56:58.9356622Z   GET http://localhost:5159/animais/1/vacinacoes [401 Unauthorized, 269B, 1ms]
2026-11-04T18:56:58.9385041Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.9387224Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.9415605Z   Pass  Devolve 401
2026-11-04T18:56:58.9444512Z 
2026-11-04T18:56:58.9444881Z Sub-folder EP-042 · 403 — Auxiliar Veterinário
2026-11-04T18:56:58.9516160Z   GET http://localhost:5159/animais/1/vacinacoes [403 Forbidden, 311B, 4ms]
2026-11-04T18:56:58.9544952Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.9547046Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.9575614Z   Pass  Devolve 403
2026-11-04T18:56:58.9610010Z 
2026-11-04T18:56:58.9610317Z Sub-folder EP-042 · 403 — Cliente
2026-11-04T18:56:58.9669942Z   GET http://localhost:5159/animais/1/vacinacoes [403 Forbidden, 311B, 3ms]
2026-11-04T18:56:58.9698864Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:58.9701052Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:58.9736061Z   Pass  Devolve 403
2026-11-04T18:56:58.9765105Z 
2026-11-04T18:56:58.9765611Z Sub-folder RF056 · 200 — atualizar a data e o intervalo e recalcular a próxima
2026-11-04T18:56:59.0039540Z   PUT http://localhost:5159/animais/1/vacinacoes/5 [200 OK, 374B, 23ms]
2026-11-04T18:56:59.0071983Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:59.0106223Z   Pass  Devolve 200
2026-11-04T18:56:59.0108937Z   Pass  Próxima recalculada (2026-10-01 + 90 dias)
2026-11-04T18:56:59.0147066Z 
2026-11-04T18:56:59.0147526Z Sub-folder RF056 · 200 — acrescentar um intervalo a uma desparasitação
2026-11-04T18:56:59.0428843Z   PUT http://localhost:5159/animais/1/vacinacoes/6 [200 OK, 387B, 25ms]
2026-11-04T18:56:59.0460139Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:59.0491983Z   Pass  Devolve 200
2026-11-04T18:56:59.0495067Z   Pass  Passa a ter próxima administração (2026-10-14 + 30 dias)
2026-11-04T18:56:59.0525714Z 
2026-11-04T18:56:59.0526069Z Sub-folder RF056 · 400 — data de administração futura
2026-11-04T18:56:59.0606608Z   PUT http://localhost:5159/animais/1/vacinacoes/5 [400 Bad Request, 259B, 4ms]
2026-11-04T18:56:59.0636625Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:59.0638778Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:59.0670280Z   Pass  Devolve 400
2026-11-04T18:56:59.0705267Z 
2026-11-04T18:56:59.0705640Z Sub-folder RF056 · 404 — administração inexistente
2026-11-04T18:56:59.0795978Z   PUT http://localhost:5159/animais/1/vacinacoes/999999 [404 Not Found, 293B, 5ms]
2026-11-04T18:56:59.0825517Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:59.0827623Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:59.0856996Z   Pass  Devolve 404
2026-11-04T18:56:59.0892057Z 
2026-11-04T18:56:59.0892432Z Sub-folder RF056 · 404 — administração de outro animal
2026-11-04T18:56:59.0978984Z   PUT http://localhost:5159/animais/5/vacinacoes/5 [404 Not Found, 293B, 5ms]
2026-11-04T18:56:59.1009396Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:59.1011829Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:59.1041762Z   Pass  Devolve 404
2026-11-04T18:56:59.1077509Z 
2026-11-04T18:56:59.1077863Z Sub-folder RF056 · 401 — sem token
2026-11-04T18:56:59.1116709Z   PUT http://localhost:5159/animais/1/vacinacoes/5 [401 Unauthorized, 269B, 1ms]
2026-11-04T18:56:59.1145410Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:59.1147533Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:59.1176473Z   Pass  Devolve 401
2026-11-04T18:56:59.1211868Z 
2026-11-04T18:56:59.1212197Z Sub-folder RF056 · 403 — Auxiliar Veterinário
2026-11-04T18:56:59.1279374Z   PUT http://localhost:5159/animais/1/vacinacoes/5 [403 Forbidden, 311B, 3ms]
2026-11-04T18:56:59.1308741Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:59.1311070Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:59.1341160Z   Pass  Devolve 403
2026-11-04T18:56:59.1376925Z 
2026-11-04T18:56:59.1377374Z Sub-folder EP-044 · 200 — boletim do meu animal
2026-11-04T18:56:59.1655385Z   GET http://localhost:5159/me/animais/1/vacinacoes [200 OK, 1.07kB, 24ms]
2026-11-04T18:56:59.1688002Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:59.1719570Z   Pass  Devolve 200
2026-11-04T18:56:59.1725550Z   Pass  Histórico com o registo atualizado e sem lote nem Veterinário
2026-11-04T18:56:59.1727511Z   Pass  Próximas da mais próxima para a mais distante
2026-11-04T18:56:59.1762654Z 
2026-11-04T18:56:59.1763281Z Sub-folder EP-044 · 200 — outro Cliente vê o boletim do seu animal (vazio)
2026-11-04T18:56:59.1853353Z   GET http://localhost:5159/me/animais/5/vacinacoes [200 OK, 178B, 6ms]
2026-11-04T18:56:59.1871596Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:59.1901933Z   Pass  Devolve 200
2026-11-04T18:56:59.1904956Z   Pass  Sem registos, as duas listas vêm vazias
2026-11-04T18:56:59.1935561Z 
2026-11-04T18:56:59.1935929Z Sub-folder EP-044 · 403 — animal de outro Cliente
2026-11-04T18:56:59.2016165Z   GET http://localhost:5159/me/animais/5/vacinacoes [403 Forbidden, 302B, 4ms]
2026-11-04T18:56:59.2045060Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:59.2047203Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:59.2084438Z   Pass  Devolve 403
2026-11-04T18:56:59.2086593Z   Pass  A resposta não tem dados do animal
2026-11-04T18:56:59.2121356Z 
2026-11-04T18:56:59.2121788Z Sub-folder EP-044 · 403 — o outro Cliente pede um animal que não é seu
2026-11-04T18:56:59.2190672Z   GET http://localhost:5159/me/animais/1/vacinacoes [403 Forbidden, 302B, 4ms]
2026-11-04T18:56:59.2219392Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:59.2221493Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:59.2251138Z   Pass  Devolve 403
2026-11-04T18:56:59.2253484Z   Pass  A resposta não tem dados do animal
2026-11-04T18:56:59.2288275Z 
2026-11-04T18:56:59.2288716Z Sub-folder EP-044 · 404 — animal inexistente
2026-11-04T18:56:59.2380302Z   GET http://localhost:5159/me/animais/999999/vacinacoes [404 Not Found, 239B, 6ms]
2026-11-04T18:56:59.2408681Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:59.2410881Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:59.2445035Z   Pass  Devolve 404
2026-11-04T18:56:59.2480164Z 
2026-11-04T18:56:59.2480523Z Sub-folder EP-044 · 401 — sem token
2026-11-04T18:56:59.2530440Z   GET http://localhost:5159/me/animais/1/vacinacoes [401 Unauthorized, 269B, 1ms]
2026-11-04T18:56:59.2551214Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:59.2553173Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:59.2582482Z   Pass  Devolve 401
2026-11-04T18:56:59.2624486Z 
2026-11-04T18:56:59.2624959Z Sub-folder EP-044 · 403 — perfil interno (Veterinário)
2026-11-04T18:56:59.2698039Z   GET http://localhost:5159/me/animais/1/vacinacoes [403 Forbidden, 311B, 4ms]
2026-11-04T18:56:59.2727542Z   Pass  Não expõe palavras-passe nem hashes
2026-11-04T18:56:59.2729710Z   Pass  Erro no formato único da API (code, message, detail)
2026-11-04T18:56:59.2759046Z   Pass  Devolve 403
2026-11-04T18:56:59.3127919Z 
2026-11-04T18:56:59.3128447Z ------------------------------------------------------------------
2026-11-04T18:56:59.3128979Z |                         |          executed |           failed |
2026-11-04T18:56:59.3129435Z --------------------------+-------------------+-------------------
2026-11-04T18:56:59.3129902Z |              iterations |                 1 |                0 |
2026-11-04T18:56:59.3130341Z --------------------------+-------------------+-------------------
2026-11-04T18:56:59.3130796Z |                requests |               269 |                0 |
2026-11-04T18:56:59.3131415Z --------------------------+-------------------+-------------------
2026-11-04T18:56:59.3131934Z |            test-scripts |               538 |                0 |
2026-11-04T18:56:59.3132430Z --------------------------+-------------------+-------------------
2026-11-04T18:56:59.3133197Z |      prerequest-scripts |                 9 |                0 |
2026-11-04T18:56:59.3133624Z --------------------------+-------------------+-------------------
2026-11-04T18:56:59.3133988Z |              assertions |               862 |                0 |
2026-11-04T18:56:59.3134319Z ------------------------------------------------------------------
2026-11-04T18:56:59.3134697Z | total run duration: 7.8s                                       |
2026-11-04T18:56:59.3135097Z ------------------------------------------------------------------
2026-11-04T18:56:59.3135495Z | total data received: 69.38kB (approx)                          |
2026-11-04T18:56:59.3135869Z ------------------------------------------------------------------
2026-11-04T18:56:59.3136278Z | average response time: 15ms [min: 1ms, max: 463ms, s.d.: 34ms] |
2026-11-04T18:56:59.3136640Z ------------------------------------------------------------------
2026-11-04T18:56:59.3858429Z ##[group]Run # Defesa adicional: os resultados anexados não podem conter a
2026-11-04T18:56:59.3859198Z [36;1m# Defesa adicional: os resultados anexados não podem conter a[0m
2026-11-04T18:56:59.3859806Z [36;1m# palavra-passe descartável (já escondida nos registos).[0m
2026-11-04T18:56:59.3860578Z [36;1mSEGREDO="$(jq -r '.values[] | select(.key == "passwordAdmin") | .value' "$RUNNER_TEMP/newman-ambiente.json")"[0m
2026-11-04T18:56:59.3861400Z [36;1mif [ -n "$SEGREDO" ] && grep -rqF -- "$SEGREDO" "$RESULTADOS/newman"; then[0m
2026-11-04T18:56:59.3862311Z [36;1m  echo "::error title=Newman::Um ficheiro de resultados contém a palavra-passe descartável; os resultados não são publicados."[0m
2026-11-04T18:56:59.3863542Z [36;1m  rm -rf "$RESULTADOS/newman"[0m
2026-11-04T18:56:59.3863902Z [36;1m  exit 1[0m
2026-11-04T18:56:59.3864191Z [36;1mfi[0m
2026-11-04T18:56:59.3864473Z [36;1mpython3 - <<'PYTHON'[0m
2026-11-04T18:56:59.3864823Z [36;1mimport json, os, sys[0m
2026-11-04T18:56:59.3865226Z [36;1mimport xml.etree.ElementTree as ET[0m
2026-11-04T18:56:59.3865787Z [36;1mcfg = json.load(open(os.environ["QUALITY_GATES"], encoding="utf-8"))["Newman"][0m
2026-11-04T18:56:59.3866388Z [36;1mmaximo = int(cfg["maximoFalhas"])[0m
2026-11-04T18:56:59.3866834Z [36;1mcodigo = int(os.environ.get("CODIGO_NEWMAN") or 1)[0m
2026-11-04T18:56:59.3867462Z [36;1mcaminho = os.path.join(os.environ["RESULTADOS"], "newman", "newman-junit.xml")[0m
2026-11-04T18:56:59.3868089Z [36;1mlinhas = ["### 7 · Collection Postman (Newman)", ""][0m
2026-11-04T18:56:59.3868561Z [36;1mif not os.path.exists(caminho):[0m
2026-11-04T18:56:59.3869253Z [36;1m    linhas.append(f"❌ **Collection não avaliada:** o Newman terminou com o código {codigo} sem relatório JUnit.")[0m
2026-11-04T18:56:59.3869948Z [36;1m    falhou = True[0m
2026-11-04T18:56:59.3870255Z [36;1melse:[0m
2026-11-04T18:56:59.3870575Z [36;1m    raiz = ET.parse(caminho).getroot()[0m
2026-11-04T18:56:59.3871122Z [36;1m    suites = [raiz] if raiz.tag == "testsuite" else raiz.findall("testsuite")[0m
2026-11-04T18:56:59.3871666Z [36;1m    pedidos = len(suites)[0m
2026-11-04T18:56:59.3872104Z [36;1m    testes = sum(int(s.get("tests", 0)) for s in suites)[0m
2026-11-04T18:56:59.3872723Z [36;1m    falhas = sum(int(s.get("failures", 0)) + int(s.get("errors", 0)) for s in suites)[0m
2026-11-04T18:56:59.3873713Z [36;1m    pedidos_falhados = [s.get("name", "?") for s in suites if int(s.get("failures", 0)) + int(s.get("errors", 0)) > 0][0m
2026-11-04T18:56:59.3874428Z [36;1m    linhas += ["| Medida | Valor |", "|---|---|",[0m
2026-11-04T18:56:59.3874877Z [36;1m               f"| Pedidos executados | {pedidos} |",[0m
2026-11-04T18:56:59.3875356Z [36;1m               f"| Verificações (`pm.test`) | {testes} |",[0m
2026-11-04T18:56:59.3875878Z [36;1m               f"| **Verificações falhadas** | **{falhas}** |",[0m
2026-11-04T18:56:59.3876481Z [36;1m               f"| Máximo permitido (`.github/quality-gates.json`) | {maximo} |",[0m
2026-11-04T18:56:59.3877065Z [36;1m               f"| Código de saída do Newman | {codigo} |", ""][0m
2026-11-04T18:56:59.3877533Z [36;1m    if pedidos_falhados:[0m
2026-11-04T18:56:59.3878073Z [36;1m        linhas += ["Pedidos com falhas:", ""] + [f"- {p}" for p in pedidos_falhados[:30]] + [""][0m
2026-11-04T18:56:59.3878737Z [36;1m    falhou = falhas > maximo or (codigo != 0 and falhas == 0)[0m
2026-11-04T18:56:59.3879228Z [36;1m    if codigo != 0 and falhas == 0:[0m
2026-11-04T18:56:59.3879978Z [36;1m        linhas.append("❌ O Newman terminou com erro sem verificações falhadas (pedido sem resposta ou collection inválida).")[0m
2026-11-04T18:56:59.3881116Z [36;1mlinhas.append("❌ **A Collection Postman falhou: o merge fica bloqueado.**" if falhou else "✅ **Collection Postman executada sem falhas.**")[0m
2026-11-04T18:56:59.3882071Z [36;1mwith open(os.environ["GITHUB_STEP_SUMMARY"], "a", encoding="utf-8") as resumo:[0m
2026-11-04T18:56:59.3883392Z [36;1m    resumo.write("\n".join(linhas) + "\n")[0m
2026-11-04T18:56:59.3883764Z [36;1mprint("\n".join(linhas))[0m
2026-11-04T18:56:59.3884012Z [36;1mif falhou:[0m
2026-11-04T18:56:59.3884382Z [36;1m    print("::error title=Newman::A Collection Postman tem pedidos ou verificações falhadas.")[0m
2026-11-04T18:56:59.3884780Z [36;1m    sys.exit(1)[0m
2026-11-04T18:56:59.3884978Z [36;1mPYTHON[0m
2026-11-04T18:56:59.3902426Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:56:59.3902726Z env:
2026-11-04T18:56:59.3903070Z   DOTNET_NOLOGO: true
2026-11-04T18:56:59.3903281Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:56:59.3903519Z   CONFIGURACAO: Release
2026-11-04T18:56:59.3903746Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:56:59.3904302Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:56:59.3904893Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:56:59.3905337Z   RESULTADOS: resultados-ci
2026-11-04T18:56:59.3905551Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:56:59.3905789Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:56:59.3906031Z   BASE_DADOS: ClinicaVeterinaria_CI
2026-11-04T18:56:59.3906276Z   API_URL: http://localhost:5159
2026-11-04T18:56:59.3906506Z   CONTENTOR_SQL: sql-newman-ci
2026-11-04T18:56:59.3906791Z   IMAGEM_SQL: mcr.microsoft.com/mssql/server:2025-CU9-ubuntu-22.04
2026-11-04T18:56:59.3907083Z   DOTNET_EF_VERSAO: 10.0.13
2026-11-04T18:56:59.3907404Z   PROJETO_API: src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj
2026-11-04T18:56:59.3907918Z   PROJETO_INFRA: src/backend/ClinicaVeterinaria.Infrastructure/ClinicaVeterinaria.Infrastructure.csproj
2026-11-04T18:56:59.3908343Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:56:59.3908737Z   SQL_SA_PASSWORD: ***
2026-11-04T18:56:59.3909031Z   SQLCMDPASSWORD: ***
2026-11-04T18:56:59.3909351Z   Jwt__Key: ***
2026-11-04T18:56:59.3910017Z   ConnectionStrings__ClinicaDb: Server=localhost,1433;Database=ClinicaVeterinaria_CI;User Id=sa;***;TrustServerCertificate=True;Encrypt=True
2026-11-04T18:56:59.3910520Z   CODIGO_NEWMAN: 0
2026-11-04T18:56:59.3910712Z ##[endgroup]
2026-11-04T18:56:59.4211597Z ### 7 · Collection Postman (Newman)
2026-11-04T18:56:59.4212037Z 
2026-11-04T18:56:59.4212302Z | Medida | Valor |
2026-11-04T18:56:59.4212593Z |---|---|
2026-11-04T18:56:59.4213053Z | Pedidos executados | 269 |
2026-11-04T18:56:59.4213526Z | Verificações (`pm.test`) | 862 |
2026-11-04T18:56:59.4214027Z | **Verificações falhadas** | **0** |
2026-11-04T18:56:59.4214603Z | Máximo permitido (`.github/quality-gates.json`) | 0 |
2026-11-04T18:56:59.4215150Z | Código de saída do Newman | 0 |
2026-11-04T18:56:59.4215390Z 
2026-11-04T18:56:59.4215687Z ✅ **Collection Postman executada sem falhas.**
2026-11-04T18:56:59.4335757Z ##[group]Run actions/upload-artifact@cf430e030ddbb5b0abf93d22962f4752f3646cd9
2026-11-04T18:56:59.4336308Z with:
2026-11-04T18:56:59.4336592Z   name: resultado-newman
2026-11-04T18:56:59.4336923Z   path: resultados-ci/newman
2026-11-04T18:56:59.4337269Z   if-no-files-found: warn
2026-11-04T18:56:59.4337578Z   retention-days: 90
2026-11-04T18:56:59.4337870Z   compression-level: 6
2026-11-04T18:56:59.4338164Z   overwrite: false
2026-11-04T18:56:59.4338437Z   include-hidden-files: false
2026-11-04T18:56:59.4338765Z   archive: true
2026-11-04T18:56:59.4339024Z env:
2026-11-04T18:56:59.4339272Z   DOTNET_NOLOGO: true
2026-11-04T18:56:59.4339564Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:56:59.4339918Z   CONFIGURACAO: Release
2026-11-04T18:56:59.4340264Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:56:59.4340934Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:56:59.4341990Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:56:59.4342731Z   RESULTADOS: resultados-ci
2026-11-04T18:56:59.4343263Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:56:59.4343652Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:56:59.4344050Z   BASE_DADOS: ClinicaVeterinaria_CI
2026-11-04T18:56:59.4344430Z   API_URL: http://localhost:5159
2026-11-04T18:56:59.4344780Z   CONTENTOR_SQL: sql-newman-ci
2026-11-04T18:56:59.4345217Z   IMAGEM_SQL: mcr.microsoft.com/mssql/server:2025-CU9-ubuntu-22.04
2026-11-04T18:56:59.4345678Z   DOTNET_EF_VERSAO: 10.0.13
2026-11-04T18:56:59.4346165Z   PROJETO_API: src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj
2026-11-04T18:56:59.4347050Z   PROJETO_INFRA: src/backend/ClinicaVeterinaria.Infrastructure/ClinicaVeterinaria.Infrastructure.csproj
2026-11-04T18:56:59.4347741Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:56:59.4348265Z   SQL_SA_PASSWORD: ***
2026-11-04T18:56:59.4348909Z   SQLCMDPASSWORD: ***
2026-11-04T18:56:59.4349415Z   Jwt__Key: ***
2026-11-04T18:56:59.4350452Z   ConnectionStrings__ClinicaDb: Server=localhost,1433;Database=ClinicaVeterinaria_CI;User Id=sa;***;TrustServerCertificate=True;Encrypt=True
2026-11-04T18:56:59.4351246Z ##[endgroup]
2026-11-04T18:56:59.5636625Z With the provided path, there will be 4 files uploaded
2026-11-04T18:56:59.5637108Z Artifact name is valid!
2026-11-04T18:56:59.5637438Z Root directory input is valid!
2026-11-04T18:56:59.9148080Z Uploading artifact: resultado-newman.zip
2026-11-04T18:56:59.9179853Z Beginning upload of artifact content to blob storage
2026-11-04T18:57:00.3837366Z Uploaded bytes 52952
2026-11-04T18:57:00.4744605Z Finished uploading artifact content to blob storage!
2026-11-04T18:57:00.4745844Z SHA256 digest of uploaded artifact is b6309d992dafc75400b762d87442e61b91fd5853cbf1e5c1ead3cb3d09a8d5a9
2026-11-04T18:57:00.4747990Z Finalizing artifact upload
2026-11-04T18:57:01.1390353Z Artifact resultado-newman successfully finalized. Artifact ID 13355280035
2026-11-04T18:57:01.1391490Z Artifact resultado-newman has been successfully uploaded! Final size is 52952 bytes. Artifact ID is 13355280035
2026-11-04T18:57:01.1394704Z Artifact download URL: https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/actions/runs/41891774877/artifacts/13355280035
2026-11-04T18:57:01.1527124Z ##[group]Run if [ -f "$RUNNER_TEMP/api.pid" ]; then kill "$(cat "$RUNNER_TEMP/api.pid")" 2>/dev/null || true; fi
2026-11-04T18:57:01.1527727Z [36;1mif [ -f "$RUNNER_TEMP/api.pid" ]; then kill "$(cat "$RUNNER_TEMP/api.pid")" 2>/dev/null || true; fi[0m
2026-11-04T18:57:01.1528162Z [36;1mdocker rm -f "$CONTENTOR_SQL" >/dev/null 2>&1 || true[0m
2026-11-04T18:57:01.1528493Z [36;1mrm -f "$RUNNER_TEMP/newman-ambiente.json"[0m
2026-11-04T18:57:01.1528914Z [36;1mecho "API parada, contentor $CONTENTOR_SQL destruído e cópia do ambiente apagada."[0m
2026-11-04T18:57:01.1546603Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:57:01.1546913Z env:
2026-11-04T18:57:01.1547103Z   DOTNET_NOLOGO: true
2026-11-04T18:57:01.1547321Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:57:01.1547555Z   CONFIGURACAO: Release
2026-11-04T18:57:01.1547785Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:57:01.1548239Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:57:01.1548847Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:57:01.1549288Z   RESULTADOS: resultados-ci
2026-11-04T18:57:01.1549519Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:57:01.1549758Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:57:01.1550020Z   BASE_DADOS: ClinicaVeterinaria_CI
2026-11-04T18:57:01.1550274Z   API_URL: http://localhost:5159
2026-11-04T18:57:01.1550506Z   CONTENTOR_SQL: sql-newman-ci
2026-11-04T18:57:01.1550799Z   IMAGEM_SQL: mcr.microsoft.com/mssql/server:2025-CU9-ubuntu-22.04
2026-11-04T18:57:01.1551099Z   DOTNET_EF_VERSAO: 10.0.13
2026-11-04T18:57:01.1551418Z   PROJETO_API: src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj
2026-11-04T18:57:01.1551942Z   PROJETO_INFRA: src/backend/ClinicaVeterinaria.Infrastructure/ClinicaVeterinaria.Infrastructure.csproj
2026-11-04T18:57:01.1552372Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:57:01.1552709Z   SQL_SA_PASSWORD: ***
2026-11-04T18:57:01.1553128Z   SQLCMDPASSWORD: ***
2026-11-04T18:57:01.1553448Z   Jwt__Key: ***
2026-11-04T18:57:01.1554117Z   ConnectionStrings__ClinicaDb: Server=localhost,1433;Database=ClinicaVeterinaria_CI;User Id=sa;***;TrustServerCertificate=True;Encrypt=True
2026-11-04T18:57:01.1554617Z ##[endgroup]
2026-11-04T18:57:01.9644101Z API parada, contentor sql-newman-ci destruído e cópia do ambiente apagada.
2026-11-04T18:57:01.9712355Z Post job cleanup.
2026-11-04T18:57:01.9716167Z ##[start-action display=Reutilizar a cache dos pacotes NuGet;id=__06049e98-03fd-4757-bd12-642487fc99fe.__actions_cache]
2026-11-04T18:57:02.1171619Z Post job cleanup.
2026-11-04T18:57:02.2198984Z Cache hit occurred on the primary key nuget-Linux-c8252acdcad9b687c3131c725ee3c1b10105ceb23478247a43e8389c12c19830, not saving cache.
2026-11-04T18:57:02.2269749Z ##[end-action id=__06049e98-03fd-4757-bd12-642487fc99fe.__actions_cache;outcome=success;conclusion=success;duration_ms=255]
2026-11-04T18:57:02.2272296Z ##[start-action display=Instalar o SDK do .NET 10;id=__06049e98-03fd-4757-bd12-642487fc99fe.__actions_setup-dotnet]
2026-11-04T18:57:02.2307223Z Post job cleanup.
2026-11-04T18:57:02.3336927Z ##[end-action id=__06049e98-03fd-4757-bd12-642487fc99fe.__actions_setup-dotnet;outcome=success;conclusion=success;duration_ms=106]
2026-11-04T18:57:02.3403060Z Post job cleanup.
2026-11-04T18:57:02.4132496Z [command]/usr/bin/git version
2026-11-04T18:57:02.4201457Z git version 2.56.0
2026-11-04T18:57:02.4230576Z Temporarily overriding HOME='/home/runner/work/_temp/5cc2f2d7-0b3a-446a-a6fb-8521e44953b7' before making global git config changes
2026-11-04T18:57:02.4231695Z Adding repository directory to the temporary git global config as a safe directory
2026-11-04T18:57:02.4235802Z [command]/usr/bin/git config --global --add safe.directory /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:57:02.4270118Z Removing SSH command configuration
2026-11-04T18:57:02.4273902Z [command]/usr/bin/git config --local --name-only --get-regexp core\.sshCommand
2026-11-04T18:57:02.4314200Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'core\.sshCommand' && git config --local --unset-all 'core.sshCommand' || :"
2026-11-04T18:57:02.4533495Z Removing HTTP extra header
2026-11-04T18:57:02.4538814Z [command]/usr/bin/git config --local --name-only --get-regexp http\.https\:\/\/github\.com\/\.extraheader
2026-11-04T18:57:02.4575455Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'http\.https\:\/\/github\.com\/\.extraheader' && git config --local --unset-all 'http.https://github.com/.extraheader' || :"
2026-11-04T18:57:02.4799009Z Removing includeIf entries pointing to credentials config files
2026-11-04T18:57:02.4806989Z [command]/usr/bin/git config --local --name-only --get-regexp ^includeIf\.gitdir:
2026-11-04T18:57:02.4851718Z [command]/usr/bin/git submodule foreach --recursive git config --local --show-origin --name-only --get-regexp remote.origin.url
2026-11-04T18:57:02.5188360Z Cleaning up orphan processes
2026-11-04T18:57:02.5468968Z Terminate orphan process: pid (2422) (dotnet)
2026-11-04T18:57:02.5492963Z Terminate orphan process: pid (2423) (dotnet)
2026-11-04T18:57:02.5514004Z Terminate orphan process: pid (2424) (dotnet)
2026-11-04T18:57:02.5534963Z Terminate orphan process: pid (2531) (VBCSCompiler)
``````
