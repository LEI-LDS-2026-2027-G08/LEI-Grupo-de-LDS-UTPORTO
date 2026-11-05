``````text
﻿2026-11-04T18:55:38.2747702Z Current runner version: '2.338.2'
2026-11-04T18:55:38.2768858Z ##[group]Runner Image Provisioner
2026-11-04T18:55:38.2769688Z Hosted Compute Agent
2026-11-04T18:55:38.2770228Z Version: 20261021.618
2026-11-04T18:55:38.2770752Z Commit: 21505ad2541d5be6135ed84fc33697146cf75b8e
2026-11-04T18:55:38.2771722Z Build Date: 2026-10-21T00:45:21Z
2026-11-04T18:55:38.2772301Z Worker ID: {60161235-9acb-40d1-a2da-0c94cb480643}
2026-11-04T18:55:38.2772921Z Region: westus2
2026-11-04T18:55:38.2773441Z Cloud: Azure
2026-11-04T18:55:38.2773902Z ##[endgroup]
2026-11-04T18:55:38.2775113Z ##[group]Operating System
2026-11-04T18:55:38.2775645Z Ubuntu
2026-11-04T18:55:38.2776173Z 24.04.5
2026-11-04T18:55:38.2776691Z LTS
2026-11-04T18:55:38.2777137Z ##[endgroup]
2026-11-04T18:55:38.2777652Z ##[group]Runner Image
2026-11-04T18:55:38.2778250Z Image: ubuntu-24.04
2026-11-04T18:55:38.2778712Z Version: 20261025.353.1
2026-11-04T18:55:38.2779873Z Included Software: https://github.com/actions/runner-images/blob/ubuntu24/20261025.353/images/ubuntu/Ubuntu2404-Readme.md
2026-11-04T18:55:38.2781419Z Image Release: https://github.com/actions/runner-images/releases/tag/ubuntu24%2F20261025.353
2026-11-04T18:55:38.2782240Z ##[endgroup]
2026-11-04T18:55:38.2783267Z ##[group]GITHUB_TOKEN Permissions
2026-11-04T18:55:38.2784809Z Contents: read
2026-11-04T18:55:38.2785291Z Metadata: read
2026-11-04T18:55:38.2785867Z ##[endgroup]
2026-11-04T18:55:38.2787643Z Secret source: Actions
2026-11-04T18:55:38.2788556Z Cache mode: write
2026-11-04T18:55:38.2789148Z Prepare workflow directory
2026-11-04T18:55:38.3110609Z Prepare all required actions
2026-11-04T18:55:38.3149305Z Getting action download info
2026-11-04T18:55:38.6375138Z Download action repository 'actions/checkout@3d3c42e5aac5ba805825da76410c181273ba90b1' (SHA:3d3c42e5aac5ba805825da76410c181273ba90b1)
2026-11-04T18:55:38.7205649Z Download action repository 'actions/upload-artifact@cf430e030ddbb5b0abf93d22962f4752f3646cd9' (SHA:cf430e030ddbb5b0abf93d22962f4752f3646cd9)
2026-11-04T18:55:38.8985522Z Complete job name: 2 · Testes unitários (QG-02)
2026-11-04T18:55:38.9662897Z ##[group]Run actions/checkout@3d3c42e5aac5ba805825da76410c181273ba90b1
2026-11-04T18:55:38.9663694Z with:
2026-11-04T18:55:38.9664053Z   persist-credentials: false
2026-11-04T18:55:38.9664566Z   repository: LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:55:38.9667590Z   token: ***
2026-11-04T18:55:38.9667949Z   ssh-strict: true
2026-11-04T18:55:38.9668316Z   ssh-user: git
2026-11-04T18:55:38.9668660Z   clean: true
2026-11-04T18:55:38.9669020Z   sparse-checkout-cone-mode: true
2026-11-04T18:55:38.9669435Z   fetch-depth: 1
2026-11-04T18:55:38.9669779Z   fetch-tags: false
2026-11-04T18:55:38.9670137Z   show-progress: true
2026-11-04T18:55:38.9670497Z   lfs: false
2026-11-04T18:55:38.9670830Z   submodules: false
2026-11-04T18:55:38.9671319Z   set-safe-directory: true
2026-11-04T18:55:38.9671725Z   allow-unsafe-pr-checkout: false
2026-11-04T18:55:38.9672279Z env:
2026-11-04T18:55:38.9672614Z   DOTNET_NOLOGO: true
2026-11-04T18:55:38.9673035Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:38.9673459Z   CONFIGURACAO: Release
2026-11-04T18:55:38.9673874Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:38.9674602Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:38.9675648Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:38.9676432Z   RESULTADOS: resultados-ci
2026-11-04T18:55:38.9676833Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:38.9677262Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:38.9677698Z ##[endgroup]
2026-11-04T18:55:39.2527517Z Syncing repository: LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:55:39.2529976Z ##[group]Getting Git version info
2026-11-04T18:55:39.2530926Z Working directory is '/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO'
2026-11-04T18:55:39.2532826Z [command]/usr/bin/git version
2026-11-04T18:55:39.2618458Z git version 2.56.0
2026-11-04T18:55:39.2637303Z ##[endgroup]
2026-11-04T18:55:39.2647088Z Temporarily overriding HOME='/home/runner/work/_temp/c7b752ad-5111-40f1-a894-51282072719a' before making global git config changes
2026-11-04T18:55:39.2648994Z Adding repository directory to the temporary git global config as a safe directory
2026-11-04T18:55:39.2652302Z [command]/usr/bin/git config --global --add safe.directory /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:55:39.2712010Z Deleting the contents of '/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO'
2026-11-04T18:55:39.2713580Z ##[group]Determining repository object format
2026-11-04T18:55:39.2714355Z ##[endgroup]
2026-11-04T18:55:39.2715652Z ##[group]Initializing the repository
2026-11-04T18:55:39.2716891Z [command]/usr/bin/git init /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:55:39.2809747Z hint: Using 'master' as the name for the initial branch. This default branch name
2026-11-04T18:55:39.2810933Z hint: will change to "main" in Git 3.0. To configure the initial branch name
2026-11-04T18:55:39.2812406Z hint: to use in all of your new repositories, which will suppress this warning,
2026-11-04T18:55:39.2813691Z hint: call:
2026-11-04T18:55:39.2814283Z hint:
2026-11-04T18:55:39.2814847Z hint: 	git config --global init.defaultBranch <name>
2026-11-04T18:55:39.2815381Z hint:
2026-11-04T18:55:39.2815917Z hint: Names commonly chosen instead of 'master' are 'main', 'trunk' and
2026-11-04T18:55:39.2816710Z hint: 'development'. The just-created branch can be renamed via this command:
2026-11-04T18:55:39.2817346Z hint:
2026-11-04T18:55:39.2817713Z hint: 	git branch -m <name>
2026-11-04T18:55:39.2818145Z hint:
2026-11-04T18:55:39.2818678Z hint: Disable this message with "git config set advice.defaultBranchName false"
2026-11-04T18:55:39.2820777Z Initialized empty Git repository in /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/
2026-11-04T18:55:39.2831808Z [command]/usr/bin/git remote add origin https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:55:39.2876507Z ##[endgroup]
2026-11-04T18:55:39.2877377Z ##[group]Disabling automatic garbage collection
2026-11-04T18:55:39.2879673Z [command]/usr/bin/git config --local gc.auto 0
2026-11-04T18:55:39.2910739Z ##[endgroup]
2026-11-04T18:55:39.2912142Z ##[group]Setting up auth
2026-11-04T18:55:39.2912852Z Removing SSH command configuration
2026-11-04T18:55:39.2915654Z [command]/usr/bin/git config --local --name-only --get-regexp core\.sshCommand
2026-11-04T18:55:39.2950570Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'core\.sshCommand' && git config --local --unset-all 'core.sshCommand' || :"
2026-11-04T18:55:39.3284654Z Removing HTTP extra header
2026-11-04T18:55:39.3290759Z [command]/usr/bin/git config --local --name-only --get-regexp http\.https\:\/\/github\.com\/\.extraheader
2026-11-04T18:55:39.3336325Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'http\.https\:\/\/github\.com\/\.extraheader' && git config --local --unset-all 'http.https://github.com/.extraheader' || :"
2026-11-04T18:55:39.3559105Z Removing includeIf entries pointing to credentials config files
2026-11-04T18:55:39.3563431Z [command]/usr/bin/git config --local --name-only --get-regexp ^includeIf\.gitdir:
2026-11-04T18:55:39.3603907Z [command]/usr/bin/git submodule foreach --recursive git config --local --show-origin --name-only --get-regexp remote.origin.url
2026-11-04T18:55:39.3839123Z [command]/usr/bin/git config --file /home/runner/work/_temp/git-credentials-3e3f13fa-7a85-4be2-9d84-0068f0bf0d98.config http.https://github.com/.extraheader AUTHORIZATION: basic ***
2026-11-04T18:55:39.3883239Z [command]/usr/bin/git config --local includeIf.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path /home/runner/work/_temp/git-credentials-3e3f13fa-7a85-4be2-9d84-0068f0bf0d98.config
2026-11-04T18:55:39.3921964Z [command]/usr/bin/git config --local includeIf.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path /home/runner/work/_temp/git-credentials-3e3f13fa-7a85-4be2-9d84-0068f0bf0d98.config
2026-11-04T18:55:39.3958943Z [command]/usr/bin/git config --local includeIf.gitdir:/github/workspace/.git.path /github/runner_temp/git-credentials-3e3f13fa-7a85-4be2-9d84-0068f0bf0d98.config
2026-11-04T18:55:39.3995506Z [command]/usr/bin/git config --local includeIf.gitdir:/github/workspace/.git/worktrees/*.path /github/runner_temp/git-credentials-3e3f13fa-7a85-4be2-9d84-0068f0bf0d98.config
2026-11-04T18:55:39.4031602Z ##[endgroup]
2026-11-04T18:55:39.4036343Z ##[group]Fetching the repository
2026-11-04T18:55:39.4037576Z [command]/usr/bin/git -c protocol.version=2 fetch --no-tags --prune --no-recurse-submodules --depth=1 origin +904ebe1bb41fb9038081be81330a0758f3f6bae5:refs/remotes/pull/249/merge
2026-11-04T18:55:40.8660375Z From https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:55:40.8662171Z  * [new ref]         904ebe1bb41fb9038081be81330a0758f3f6bae5 -> pull/249/merge
2026-11-04T18:55:40.8663946Z ##[endgroup]
2026-11-04T18:55:40.8664506Z ##[group]Determining the checkout info
2026-11-04T18:55:40.8665113Z ##[endgroup]
2026-11-04T18:55:40.8669027Z [command]/usr/bin/git sparse-checkout disable
2026-11-04T18:55:40.8725905Z [command]/usr/bin/git config --local --unset-all extensions.worktreeConfig
2026-11-04T18:55:40.8763828Z ##[group]Checking out the ref
2026-11-04T18:55:40.8767262Z [command]/usr/bin/git checkout --progress --force refs/remotes/pull/249/merge
2026-11-04T18:55:41.1160658Z Note: switching to 'refs/remotes/pull/249/merge'.
2026-11-04T18:55:41.1161354Z 
2026-11-04T18:55:41.1161660Z You are in 'detached HEAD' state. You can look around, make experimental
2026-11-04T18:55:41.1162323Z changes and commit them, and you can discard any commits you make in this
2026-11-04T18:55:41.1163234Z state without impacting any branches by switching back to a branch.
2026-11-04T18:55:41.1163524Z 
2026-11-04T18:55:41.1163719Z If you want to create a new branch to retain commits you create, you may
2026-11-04T18:55:41.1164121Z do so (now or later) by using -c with the switch command. Example:
2026-11-04T18:55:41.1164331Z 
2026-11-04T18:55:41.1164444Z   git switch -c <new-branch-name>
2026-11-04T18:55:41.1164603Z 
2026-11-04T18:55:41.1164720Z Or undo this operation with:
2026-11-04T18:55:41.1164865Z 
2026-11-04T18:55:41.1164971Z   git switch -
2026-11-04T18:55:41.1165120Z 
2026-11-04T18:55:41.1165337Z Turn off this advice by setting config variable advice.detachedHead to false
2026-11-04T18:55:41.1165638Z 
2026-11-04T18:55:41.1165923Z HEAD is now at 904ebe1 Merge 6852c4461708be065c45ec293936ef6fea1e4a53 into 89d6b31c5e788b4e8e5cfc7178d991ef65856c06
2026-11-04T18:55:41.1208297Z ##[endgroup]
2026-11-04T18:55:41.1259833Z [command]/usr/bin/git log -1 --format=%H
2026-11-04T18:55:41.1292455Z 904ebe1bb41fb9038081be81330a0758f3f6bae5
2026-11-04T18:55:41.1300065Z ##[group]Removing auth
2026-11-04T18:55:41.1300396Z Removing SSH command configuration
2026-11-04T18:55:41.1303500Z [command]/usr/bin/git config --local --name-only --get-regexp core\.sshCommand
2026-11-04T18:55:41.1340527Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'core\.sshCommand' && git config --local --unset-all 'core.sshCommand' || :"
2026-11-04T18:55:41.1548550Z Removing HTTP extra header
2026-11-04T18:55:41.1552332Z [command]/usr/bin/git config --local --name-only --get-regexp http\.https\:\/\/github\.com\/\.extraheader
2026-11-04T18:55:41.1588907Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'http\.https\:\/\/github\.com\/\.extraheader' && git config --local --unset-all 'http.https://github.com/.extraheader' || :"
2026-11-04T18:55:41.1795344Z Removing includeIf entries pointing to credentials config files
2026-11-04T18:55:41.1798775Z [command]/usr/bin/git config --local --name-only --get-regexp ^includeIf\.gitdir:
2026-11-04T18:55:41.1832092Z includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path
2026-11-04T18:55:41.1832842Z includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path
2026-11-04T18:55:41.1833545Z includeif.gitdir:/github/workspace/.git.path
2026-11-04T18:55:41.1833990Z includeif.gitdir:/github/workspace/.git/worktrees/*.path
2026-11-04T18:55:41.1840469Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path
2026-11-04T18:55:41.1870387Z /home/runner/work/_temp/git-credentials-3e3f13fa-7a85-4be2-9d84-0068f0bf0d98.config
2026-11-04T18:55:41.1879156Z [command]/usr/bin/git config --local --unset includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path \/home\/runner\/work\/_temp\/git\-credentials\-3e3f13fa\-7a85\-4be2\-9d84\-0068f0bf0d98\.config
2026-11-04T18:55:41.1926500Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path
2026-11-04T18:55:41.1956876Z /home/runner/work/_temp/git-credentials-3e3f13fa-7a85-4be2-9d84-0068f0bf0d98.config
2026-11-04T18:55:41.1964341Z [command]/usr/bin/git config --local --unset includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path \/home\/runner\/work\/_temp\/git\-credentials\-3e3f13fa\-7a85\-4be2\-9d84\-0068f0bf0d98\.config
2026-11-04T18:55:41.2005393Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/github/workspace/.git.path
2026-11-04T18:55:41.2034837Z /github/runner_temp/git-credentials-3e3f13fa-7a85-4be2-9d84-0068f0bf0d98.config
2026-11-04T18:55:41.2041262Z [command]/usr/bin/git config --local --unset includeif.gitdir:/github/workspace/.git.path \/github\/runner_temp\/git\-credentials\-3e3f13fa\-7a85\-4be2\-9d84\-0068f0bf0d98\.config
2026-11-04T18:55:41.2079229Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/github/workspace/.git/worktrees/*.path
2026-11-04T18:55:41.2108330Z /github/runner_temp/git-credentials-3e3f13fa-7a85-4be2-9d84-0068f0bf0d98.config
2026-11-04T18:55:41.2115774Z [command]/usr/bin/git config --local --unset includeif.gitdir:/github/workspace/.git/worktrees/*.path \/github\/runner_temp\/git\-credentials\-3e3f13fa\-7a85\-4be2\-9d84\-0068f0bf0d98\.config
2026-11-04T18:55:41.2154104Z [command]/usr/bin/git submodule foreach --recursive git config --local --show-origin --name-only --get-regexp remote.origin.url
2026-11-04T18:55:41.2376714Z Removing credentials config '/home/runner/work/_temp/git-credentials-3e3f13fa-7a85-4be2-9d84-0068f0bf0d98.config'
2026-11-04T18:55:41.2387876Z ##[endgroup]
2026-11-04T18:55:41.2709022Z Prepare all required actions
2026-11-04T18:55:41.2709476Z Getting action download info
2026-11-04T18:55:41.4083537Z Download action repository 'actions/setup-dotnet@a98b56852c35b8e3190ac28c8c2271da59106c68' (SHA:a98b56852c35b8e3190ac28c8c2271da59106c68)
2026-11-04T18:55:41.7078104Z Download action repository 'actions/cache@55cc8345863c7cc4c66a329aec7e433d2d1c52a9' (SHA:55cc8345863c7cc4c66a329aec7e433d2d1c52a9)
2026-11-04T18:55:41.9084090Z ##[group]Run ./.github/actions/preparar-dotnet
2026-11-04T18:55:41.9084348Z env:
2026-11-04T18:55:41.9084515Z   DOTNET_NOLOGO: true
2026-11-04T18:55:41.9084699Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:41.9084906Z   CONFIGURACAO: Release
2026-11-04T18:55:41.9085109Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:41.9085508Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:41.9086125Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:41.9086551Z   RESULTADOS: resultados-ci
2026-11-04T18:55:41.9086747Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:41.9086959Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:41.9087348Z ##[endgroup]
2026-11-04T18:55:41.9132584Z ##[start-action display=Instalar o SDK do .NET 10;id=__self.__actions_setup-dotnet]
2026-11-04T18:55:41.9181352Z ##[group]Run actions/setup-dotnet@a98b56852c35b8e3190ac28c8c2271da59106c68
2026-11-04T18:55:41.9181654Z with:
2026-11-04T18:55:41.9181821Z   dotnet-version: 10.0.x
2026-11-04T18:55:41.9182010Z   cache: false
2026-11-04T18:55:41.9182169Z env:
2026-11-04T18:55:41.9182321Z   DOTNET_NOLOGO: true
2026-11-04T18:55:41.9182505Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:41.9182709Z   CONFIGURACAO: Release
2026-11-04T18:55:41.9182908Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:41.9183318Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:41.9183891Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:41.9184312Z   RESULTADOS: resultados-ci
2026-11-04T18:55:41.9184523Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:41.9184735Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:41.9184948Z ##[endgroup]
2026-11-04T18:55:42.0319831Z [command]/home/runner/work/_actions/actions/setup-dotnet/a98b56852c35b8e3190ac28c8c2271da59106c68/externals/install-dotnet.sh --skip-non-versioned-files --runtime dotnet --channel LTS
2026-11-04T18:55:42.3940898Z dotnet-install: .NET Core Runtime with version '10.0.14' is already installed.
2026-11-04T18:55:42.3961787Z [command]/home/runner/work/_actions/actions/setup-dotnet/a98b56852c35b8e3190ac28c8c2271da59106c68/externals/install-dotnet.sh --skip-non-versioned-files --channel 10.0
2026-11-04T18:55:42.6288892Z dotnet-install: .NET Core SDK with version '10.0.403' is already installed.
2026-11-04T18:55:42.6399678Z ##[end-action id=__self.__actions_setup-dotnet;outcome=success;conclusion=success;duration_ms=726]
2026-11-04T18:55:42.6402811Z ##[start-action display=Reutilizar a cache dos pacotes NuGet;id=__self.__actions_cache]
2026-11-04T18:55:42.7727128Z ##[group]Run actions/cache@55cc8345863c7cc4c66a329aec7e433d2d1c52a9
2026-11-04T18:55:42.7727427Z with:
2026-11-04T18:55:42.7727613Z   path: ~/.nuget/packages
2026-11-04T18:55:42.7727936Z   key: nuget-Linux-c8252acdcad9b687c3131c725ee3c1b10105ceb23478247a43e8389c12c19830
2026-11-04T18:55:42.7728279Z   restore-keys: nuget-Linux-

2026-11-04T18:55:42.7728481Z   enableCrossOsArchive: false
2026-11-04T18:55:42.7728683Z   fail-on-cache-miss: false
2026-11-04T18:55:42.7728872Z   lookup-only: false
2026-11-04T18:55:42.7729041Z   save-always: false
2026-11-04T18:55:42.7729203Z env:
2026-11-04T18:55:42.7729359Z   DOTNET_NOLOGO: true
2026-11-04T18:55:42.7729545Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:42.7729753Z   CONFIGURACAO: Release
2026-11-04T18:55:42.7730256Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:42.7730652Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:42.7731487Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:42.7731987Z   RESULTADOS: resultados-ci
2026-11-04T18:55:42.7732188Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:42.7732402Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:42.7732625Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:55:42.7732818Z ##[endgroup]
2026-11-04T18:55:43.1083783Z Cache hit for: nuget-Linux-c8252acdcad9b687c3131c725ee3c1b10105ceb23478247a43e8389c12c19830
2026-11-04T18:55:44.3844929Z Received 20971520 of 283633656 (7.4%), 20.0 MBs/sec
2026-11-04T18:55:45.3855932Z Received 134217728 of 283633656 (47.3%), 63.9 MBs/sec
2026-11-04T18:55:46.3892803Z Received 275245048 of 283633656 (97.0%), 87.4 MBs/sec
2026-11-04T18:55:46.4735096Z Received 283633656 of 283633656 (100.0%), 87.5 MBs/sec
2026-11-04T18:55:46.4736770Z Cache Size: ~270 MB (283633656 B)
2026-11-04T18:55:46.4770986Z [command]/usr/bin/tar -xf /home/runner/work/_temp/4d3fe55c-4260-4ab9-afd9-e7355fb4e068/cache.tzst -P -C /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO --use-compress-program unzstd
2026-11-04T18:55:47.4696738Z Cache restored successfully
2026-11-04T18:55:47.5088154Z Cache restored from key: nuget-Linux-c8252acdcad9b687c3131c725ee3c1b10105ceb23478247a43e8389c12c19830
2026-11-04T18:55:47.5283963Z ##[end-action id=__self.__actions_cache;outcome=success;conclusion=success;duration_ms=4888]
2026-11-04T18:55:47.5286424Z ##[start-action display=Registar o SDK utilizado;id=__self.__run]
2026-11-04T18:55:47.5316679Z ##[group]Run echo "SDK do .NET: $(dotnet --version)"
2026-11-04T18:55:47.5317036Z [36;1mecho "SDK do .NET: $(dotnet --version)"[0m
2026-11-04T18:55:47.5317281Z [36;1m{[0m
2026-11-04T18:55:47.5317457Z [36;1m  echo "### Ambiente .NET"[0m
2026-11-04T18:55:47.5317687Z [36;1m  echo ""[0m
2026-11-04T18:55:47.5317886Z [36;1m  echo "| Elemento | Valor |"[0m
2026-11-04T18:55:47.5318104Z [36;1m  echo "|---|---|"[0m
2026-11-04T18:55:47.5318337Z [36;1m  echo "| SDK do .NET | \`$(dotnet --version)\` |"[0m
2026-11-04T18:55:47.5318677Z [36;1m  echo "| global.json | \`$(tr -d '\n\r ' < src/backend/global.json)\` |"[0m
2026-11-04T18:55:47.5319041Z [36;1m  echo "| Runner | \`${RUNNER_OS} ${ImageOS:-} ${ImageVersion:-}\` |"[0m
2026-11-04T18:55:47.5319311Z [36;1m  echo ""[0m
2026-11-04T18:55:47.5319493Z [36;1m} >> "$GITHUB_STEP_SUMMARY"[0m
2026-11-04T18:55:47.5341786Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:55:47.5342074Z env:
2026-11-04T18:55:47.5342240Z   DOTNET_NOLOGO: true
2026-11-04T18:55:47.5342433Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:47.5342653Z   CONFIGURACAO: Release
2026-11-04T18:55:47.5342872Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:47.5343281Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:47.5343850Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:47.5344280Z   RESULTADOS: resultados-ci
2026-11-04T18:55:47.5344487Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:47.5344714Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:47.5344955Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:55:47.5345153Z ##[endgroup]
2026-11-04T18:55:49.4842424Z SDK do .NET: 10.0.403
2026-11-04T18:55:49.6015685Z ##[end-action id=__self.__run;outcome=success;conclusion=success;duration_ms=2072]
2026-11-04T18:55:49.7095364Z ##[group]Run dotnet restore "$SOLUCAO"
2026-11-04T18:55:49.7095731Z [36;1mdotnet restore "$SOLUCAO"[0m
2026-11-04T18:55:49.7117046Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:55:49.7117329Z env:
2026-11-04T18:55:49.7117486Z   DOTNET_NOLOGO: true
2026-11-04T18:55:49.7117674Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:49.7117889Z   CONFIGURACAO: Release
2026-11-04T18:55:49.7118105Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:49.7118508Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:49.7119093Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:49.7119514Z   RESULTADOS: resultados-ci
2026-11-04T18:55:49.7119724Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:49.7119952Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:49.7120191Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:55:49.7120432Z ##[endgroup]
2026-11-04T18:55:53.0502795Z   Determining projects to restore...
2026-11-04T18:55:55.0263266Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Application/ClinicaVeterinaria.Application.csproj (in 741 ms).
2026-11-04T18:55:55.0265061Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Infrastructure/ClinicaVeterinaria.Infrastructure.csproj (in 748 ms).
2026-11-04T18:55:55.0266937Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj (in 771 ms).
2026-11-04T18:55:55.0291734Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj (in 754 ms).
2026-11-04T18:55:55.0293484Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Domain/ClinicaVeterinaria.Domain.csproj (in 1 ms).
2026-11-04T18:55:55.0759508Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj (in 44 ms).
2026-11-04T18:55:55.1297352Z ##[group]Run dotnet build "$TESTES_UNITARIOS" --configuration "$CONFIGURACAO" --no-restore
2026-11-04T18:55:55.1297913Z [36;1mdotnet build "$TESTES_UNITARIOS" --configuration "$CONFIGURACAO" --no-restore[0m
2026-11-04T18:55:55.1316167Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:55:55.1316446Z env:
2026-11-04T18:55:55.1316606Z   DOTNET_NOLOGO: true
2026-11-04T18:55:55.1316807Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:55.1317097Z   CONFIGURACAO: Release
2026-11-04T18:55:55.1317311Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:55.1317709Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:55.1318332Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:55.1318777Z   RESULTADOS: resultados-ci
2026-11-04T18:55:55.1318994Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:55.1319253Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:55.1319493Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:55:55.1319694Z ##[endgroup]
2026-11-04T18:56:00.7972465Z   ClinicaVeterinaria.Domain -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Domain/bin/Release/net10.0/ClinicaVeterinaria.Domain.dll
2026-11-04T18:56:01.9923386Z   ClinicaVeterinaria.Application -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Application/bin/Release/net10.0/ClinicaVeterinaria.Application.dll
2026-11-04T18:56:03.7703183Z   ClinicaVeterinaria.Infrastructure -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Infrastructure/bin/Release/net10.0/ClinicaVeterinaria.Infrastructure.dll
2026-11-04T18:56:06.5981952Z   ClinicaVeterinaria.Api -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Api/bin/Release/net10.0/ClinicaVeterinaria.Api.dll
2026-11-04T18:56:10.3722931Z   ClinicaVeterinaria.UnitTests -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.UnitTests/bin/Release/net10.0/ClinicaVeterinaria.UnitTests.dll
2026-11-04T18:56:10.3959019Z 
2026-11-04T18:56:10.3966011Z Build succeeded.
2026-11-04T18:56:10.3966764Z     0 Warning(s)
2026-11-04T18:56:10.3971869Z     0 Error(s)
2026-11-04T18:56:10.3972085Z 
2026-11-04T18:56:10.3972277Z Time Elapsed 00:00:15.10
2026-11-04T18:56:10.4209149Z ##[group]Run dotnet test "$TESTES_UNITARIOS" \
2026-11-04T18:56:10.4209643Z [36;1mdotnet test "$TESTES_UNITARIOS" \[0m
2026-11-04T18:56:10.4210069Z [36;1m  --configuration "$CONFIGURACAO" \[0m
2026-11-04T18:56:10.4210424Z [36;1m  --no-build \[0m
2026-11-04T18:56:10.4210806Z [36;1m  --results-directory "$RESULTADOS/testes-unitarios" \[0m
2026-11-04T18:56:10.4211550Z [36;1m  --logger "trx;LogFileName=testes-unitarios.trx" \[0m
2026-11-04T18:56:10.4211995Z [36;1m  --logger "console;verbosity=normal" \[0m
2026-11-04T18:56:10.4212391Z [36;1m  --collect:"XPlat Code Coverage" \[0m
2026-11-04T18:56:10.4212763Z [36;1m  --blame-hang-timeout 5m[0m
2026-11-04T18:56:10.4237057Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:56:10.4237494Z env:
2026-11-04T18:56:10.4237975Z   DOTNET_NOLOGO: true
2026-11-04T18:56:10.4238278Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:56:10.4238632Z   CONFIGURACAO: Release
2026-11-04T18:56:10.4239005Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:56:10.4239681Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:56:10.4240700Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:56:10.4241667Z   RESULTADOS: resultados-ci
2026-11-04T18:56:10.4242004Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:56:10.4242376Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:56:10.4242748Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:56:10.4243059Z ##[endgroup]
2026-11-04T18:56:10.9050888Z Test run for /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.UnitTests/bin/Release/net10.0/ClinicaVeterinaria.UnitTests.dll (.NETCoreApp,Version=v10.0)
2026-11-04T18:56:11.3112105Z A total of 1 test files matched the specified pattern.
2026-11-04T18:56:12.4829786Z [xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v2.5.3.1+6b60a9e56a (64-bit .NET 10.0.14)
2026-11-04T18:56:12.9250508Z [xUnit.net 00:00:00.45]   Discovering: ClinicaVeterinaria.UnitTests
2026-11-04T18:56:13.0442319Z [xUnit.net 00:00:00.57]   Discovered:  ClinicaVeterinaria.UnitTests
2026-11-04T18:56:13.0466051Z [xUnit.net 00:00:00.57]   Starting:    ClinicaVeterinaria.UnitTests
2026-11-04T18:56:13.1862992Z   Passed I064 — Uma claim exp em falta ou inválida não dá data de expiração(valor: "1791993600.5") [4 ms]
2026-11-04T18:56:13.1864524Z   Passed I064 — Uma claim exp em falta ou inválida não dá data de expiração(valor: null) [< 1 ms]
2026-11-04T18:56:13.1865877Z   Passed I064 — Uma claim exp em falta ou inválida não dá data de expiração(valor: "-1") [< 1 ms]
2026-11-04T18:56:13.1867065Z   Passed I064 — Uma claim exp em falta ou inválida não dá data de expiração(valor: "abc") [< 1 ms]
2026-11-04T18:56:13.1868406Z   Passed I064 — Uma claim exp em falta ou inválida não dá data de expiração(valor: "253402300800") [< 1 ms]
2026-11-04T18:56:13.1869483Z   Passed I064 — Uma claim exp em falta ou inválida não dá data de expiração(valor: "") [< 1 ms]
2026-11-04T18:56:13.1893125Z   Passed I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido(email: "joao@mail.") [67 ms]
2026-11-04T18:56:13.1894376Z   Passed I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido(email: "joao@@mail.pt") [< 1 ms]
2026-11-04T18:56:13.1895588Z   Passed I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido(email: "joao@mail..pt") [< 1 ms]
2026-11-04T18:56:13.1896496Z   Passed I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido(email: "") [< 1 ms]
2026-11-04T18:56:13.1897188Z   Passed I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido(email: "joao@") [< 1 ms]
2026-11-04T18:56:13.1897942Z   Passed I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido(email: " joao@mail.pt") [< 1 ms]
2026-11-04T18:56:13.1899013Z   Passed I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido(email: "@mail.pt") [< 1 ms]
2026-11-04T18:56:13.1899709Z   Passed I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido(email: "joao@.pt") [< 1 ms]
2026-11-04T18:56:13.1900733Z   Passed I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido(email: "joao@costa@mail.pt") [< 1 ms]
2026-11-04T18:56:13.1918978Z   Passed I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido(email: null) [< 1 ms]
2026-11-04T18:56:13.1935622Z   Passed UT-F001-068 — Um e-mail sem @ é inválido [< 1 ms]
2026-11-04T18:56:13.1936545Z   Passed UT-F001-070 — Um e-mail com espaços é inválido [< 1 ms]
2026-11-04T18:56:13.1937463Z   Passed I088 (critério 5) — o resumo do histórico (EP-034) só tem os campos do resumo [68 ms]
2026-11-04T18:56:13.1939186Z   Passed I065 — No pedido, um valor vazio fica para o [Required](email: null) [< 1 ms]
2026-11-04T18:56:13.1940313Z   Passed I065 — No pedido, um valor vazio fica para o [Required](email: "") [< 1 ms]
2026-11-04T18:56:13.1941465Z   Passed UT-F001-067 — "joao.costa@mail.pt" é um e-mail válido [< 1 ms]
2026-11-04T18:56:13.1942421Z   Passed UT-F001-071 — Um e-mail válido com 256 caracteres é aceite [2 ms]
2026-11-04T18:56:13.3715963Z   Passed UT-F001-069 — Um e-mail sem ponto no domínio é inválido [< 1 ms]
2026-11-04T18:56:13.3726687Z   Passed UT-F001-072 — Um e-mail com 257 caracteres é inválido [< 1 ms]
2026-11-04T18:56:13.3732802Z   Passed I088 (critério 5, PA-08) — a leitura não depende do perfil: o serviço não recebe o utilizador em EP-033 nem em EP-034 [8 ms]
2026-11-04T18:56:13.3734154Z   Passed I088 (critério 5, PA-08) — o registo de EP-033 tem os campos do contrato v02 [< 1 ms]
2026-11-04T18:56:13.3735517Z   Passed I088 (critério 5) — o resumo do histórico (EP-034) não expõe as notas clínicas nem o detalhe dos tratamentos e prescrições [< 1 ms]
2026-11-04T18:56:13.3737179Z   Passed I077 e I078 — EP-031 e EP-032 exigem também a política Veterinário (o Auxiliar recebe 403)(nomeAcao: "Criar") [60 ms]
2026-11-04T18:56:13.3738602Z   Passed I077 e I078 — EP-031 e EP-032 exigem também a política Veterinário (o Auxiliar recebe 403)(nomeAcao: "Atualizar") [< 1 ms]
2026-11-04T18:56:13.3739764Z   Passed I077 — EP-032 devolve 200 com o registo e usa o Veterinário do token [142 ms]
2026-11-04T18:56:13.3851870Z   Passed I078 — o Auxiliar Veterinário só tem operações de leitura (GET) neste controlador [2 ms]
2026-11-04T18:56:13.3862067Z   Passed I077 — As rotas são POST /registos-clinicos e PUT /registos-clinicos/{id} [1 ms]
2026-11-04T18:56:13.3875431Z   Passed I077 — um token de Veterinário sem a claim veterinarioId dá 403 sem chamar o serviço [5 ms]
2026-11-04T18:56:13.3876834Z   Passed I078 — o controlador aceita só o Veterinário e o Auxiliar Veterinário e nenhuma ação dispensa a autenticação [< 1 ms]
2026-11-04T18:56:13.3878244Z   Passed I077 — EP-031 devolve 201 com Location /registos-clinicos/{id} e usa o Veterinário do token [1 ms]
2026-11-04T18:56:13.3879313Z   Passed I078 — EP-033 devolve 200 com o registo do serviço [1 ms]
2026-11-04T18:56:13.3880527Z   Passed I078 — EP-033 e EP-034 não acrescentam restrições: Veterinário e Auxiliar Veterinário leem(nomeAcao: "Obter") [< 1 ms]
2026-11-04T18:56:13.3886078Z   Passed I078 — EP-033 e EP-034 não acrescentam restrições: Veterinário e Auxiliar Veterinário leem(nomeAcao: "ObterHistorico") [< 1 ms]
2026-11-04T18:56:13.3889134Z   Passed I078 — EP-034 devolve 200 com o histórico e passa o animal e as datas ao serviço [1 ms]
2026-11-04T18:56:13.4202000Z   Passed I078 — As rotas são GET /registos-clinicos/{id} e GET /animais/{id}/historico-clinico [< 1 ms]
2026-11-04T18:56:13.4203583Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.HorarioVazioSignificaClinicaFechada [3 ms]
2026-11-04T18:56:13.4205089Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.PeriodosSobrepostosNoMesmoDiaSaoRejeitados [6 ms]
2026-11-04T18:56:13.4206963Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.MesmasHorasEmDiasDiferentesNaoSeSobrepoem [< 1 ms]
2026-11-04T18:56:13.4208647Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.DiaOuHoraInvalidosSaoRejeitados(dia: "", abertura: "09:00", fecho: "13:00") [< 1 ms]
2026-11-04T18:56:13.4210447Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.DiaOuHoraInvalidosSaoRejeitados(dia: "Segunda", abertura: "9h", fecho: "13:00") [1 ms]
2026-11-04T18:56:13.4212402Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.DiaOuHoraInvalidosSaoRejeitados(dia: "Segunda", abertura: "09:00", fecho: "25:00") [< 1 ms]
2026-11-04T18:56:13.4214178Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.DiaOuHoraInvalidosSaoRejeitados(dia: "Feriado", abertura: "09:00", fecho: "13:00") [< 1 ms]
2026-11-04T18:56:13.4216317Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.FechoIgualOuAnteriorAAberturaERejeitado(abertura: "13:00", fecho: "09:00") [< 1 ms]
2026-11-04T18:56:13.4349672Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.FechoIgualOuAnteriorAAberturaERejeitado(abertura: "13:00", fecho: "13:00") [< 1 ms]
2026-11-04T18:56:13.4368451Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.PeriodosContiguosSaoAceites [< 1 ms]
2026-11-04T18:56:13.4389046Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.VariosPeriodosNoMesmoDiaSaoOrdenadosPorDiaEHora [3 ms]
2026-11-04T18:56:13.4414248Z   Passed UT-F006-001, UT-F006-003, UT-F006-004, UT-F006-005, UT-F006-006 e UT-F006-007 — data de administração + intervalo em dias de calendário(dataAdministracao: "2026-10-14", intervaloDias: 90, esperada: "2027-01-12") [3 ms]
2026-11-04T18:56:13.4431844Z   Passed UT-F006-001, UT-F006-003, UT-F006-004, UT-F006-005, UT-F006-006 e UT-F006-007 — data de administração + intervalo em dias de calendário(dataAdministracao: "2026-01-31", intervaloDias: 1, esperada: "2026-02-01") [< 1 ms]
2026-11-04T18:56:13.4434459Z   Passed UT-F006-001, UT-F006-003, UT-F006-004, UT-F006-005, UT-F006-006 e UT-F006-007 — data de administração + intervalo em dias de calendário(dataAdministracao: "2027-03-01", intervaloDias: 365, esperada: "2028-02-29") [< 1 ms]
2026-11-04T18:56:13.4436839Z   Passed UT-F006-001, UT-F006-003, UT-F006-004, UT-F006-005, UT-F006-006 e UT-F006-007 — data de administração + intervalo em dias de calendário(dataAdministracao: "2026-12-31", intervaloDias: 1, esperada: "2027-01-01") [< 1 ms]
2026-11-04T18:56:13.4439325Z   Passed UT-F006-001, UT-F006-003, UT-F006-004, UT-F006-005, UT-F006-006 e UT-F006-007 — data de administração + intervalo em dias de calendário(dataAdministracao: "2028-02-28", intervaloDias: 1, esperada: "2028-02-29") [< 1 ms]
2026-11-04T18:56:13.4441943Z   Passed UT-F006-001, UT-F006-003, UT-F006-004, UT-F006-005, UT-F006-006 e UT-F006-007 — data de administração + intervalo em dias de calendário(dataAdministracao: "2027-02-28", intervaloDias: 1, esperada: "2027-03-01") [< 1 ms]
2026-11-04T18:56:13.4520467Z   Passed I082 — intervalo indicado e não superior a 0 é recusado(intervaloDias: -30) [< 1 ms]
2026-11-04T18:56:13.4540654Z   Passed I082 — intervalo indicado e não superior a 0 é recusado(intervaloDias: 0) [< 1 ms]
2026-11-04T18:56:13.4572194Z   Passed UT-F006-002 — sem intervalo não há próxima administração (null) [< 1 ms]
2026-11-04T18:56:13.4581067Z   Passed I072 — sem versão (null) é um erro de programação [< 1 ms]
2026-11-04T18:56:13.4582361Z   Passed UT-F003-087 — If-Match com versão diferente não corresponde [< 1 ms]
2026-11-04T18:56:13.4583660Z   Passed I074 — Sem If-Match a operação não fica condicionada à versão [< 1 ms]
2026-11-04T18:56:13.4584791Z   Passed I072 — versões diferentes dão ETags diferentes [1 ms]
2026-11-04T18:56:13.4585855Z   Passed UT-F003-089 — If-Match asterisco corresponde a recurso existente [< 1 ms]
2026-11-04T18:56:13.4586935Z   Passed UT-F003-086 — If-Match com a versão atual corresponde [< 1 ms]
2026-11-04T18:56:13.4657368Z   Passed UT-F003-088 — If-Match fraco não corresponde [< 1 ms]
2026-11-04T18:56:13.4663039Z   Passed UT-F003-085 — RowVersion 00 00 00 00 00 00 07 D1 dá o ETag "AAAAAAAAB9E=" [< 1 ms]
2026-11-04T18:56:13.4664115Z   Passed UT-F003-090 — If-Match sem aspas não corresponde [< 1 ms]
2026-11-04T18:56:13.4665162Z   Passed UT-F003-016 — Sem data de início o pedido é recusado (obrigatória) [< 1 ms]
2026-11-04T18:56:13.4666471Z   Passed I071 — No pedido, sem dataInicio o [ApiController] responde 400 com a mensagem em português [3 ms]
2026-11-04T18:56:13.4667775Z   Passed I071 — Datas fora de 2000-01-01 a 9998-12-31 são recusadas(ano: 1, mes: 1, dia: 1) [< 1 ms]
2026-11-04T18:56:13.4668942Z   Passed I071 — Datas fora de 2000-01-01 a 9998-12-31 são recusadas(ano: 1999, mes: 12, dia: 31) [< 1 ms]
2026-11-04T18:56:13.4670130Z   Passed I071 — Datas fora de 2000-01-01 a 9998-12-31 são recusadas(ano: 9999, mes: 1, dia: 1) [< 1 ms]
2026-11-04T18:56:13.4671594Z   Passed I071 — Sem data de fim, o período é só o dia de início [1 ms]
2026-11-04T18:56:13.4912792Z   Passed UT-F003-014 — Um período de 32 dias é recusado [< 1 ms]
2026-11-04T18:56:13.4932348Z   Passed I071 — No pedido, um período válido não tem erros [1 ms]
2026-11-04T18:56:13.4960175Z   Passed UT-F003-015 — Uma data de fim anterior à de início é recusada [< 1 ms]
2026-11-04T18:56:13.4992002Z   Passed I071 — No pedido, um período de 32 dias tem erro no campo dataFim [1 ms]
2026-11-04T18:56:13.4993331Z   Passed UT-F003-013 — Um período de 31 dias é aceite [< 1 ms]
2026-11-04T18:56:13.5004231Z   Passed I072 — NotaPreparacaoDto e as referências ao animal, ao proprietário e ao Veterinário [< 1 ms]
2026-11-04T18:56:13.5051611Z   Passed I072 — nenhum DTO da agenda tem um campo clínico(dto: typeof(ClinicaVeterinaria.Application.DTOs.Comum.AnimalRefDto)) [< 1 ms]
2026-11-04T18:56:13.5092305Z   Passed I072 — nenhum DTO da agenda tem um campo clínico(dto: typeof(ClinicaVeterinaria.Application.DTOs.Comum.ClienteRefDto)) [< 1 ms]
2026-11-04T18:56:13.5122425Z   Passed I072 — nenhum DTO da agenda tem um campo clínico(dto: typeof(ClinicaVeterinaria.Application.DTOs.Comum.VeterinarioRefDto)) [< 1 ms]
2026-11-04T18:56:13.5123883Z   Passed I072 — nenhum DTO da agenda tem um campo clínico(dto: typeof(ClinicaVeterinaria.Application.DTOs.Consultas.ConsultaListItemDto)) [< 1 ms]
2026-11-04T18:56:13.5125174Z   Passed I072 — nenhum DTO da agenda tem um campo clínico(dto: typeof(ClinicaVeterinaria.Application.DTOs.Consultas.ConsultaDto)) [< 1 ms]
2026-11-04T18:56:13.5126418Z   Passed I072 — nenhum DTO da agenda tem um campo clínico(dto: typeof(ClinicaVeterinaria.Application.DTOs.Consultas.NotaPreparacaoDto)) [< 1 ms]
2026-11-04T18:56:13.5127495Z   Passed I072 — ConsultaListItemDto (EP-023): só dados administrativos [< 1 ms]
2026-11-04T18:56:13.5128051Z   Passed I072 — ConsultaDto (EP-024): só dados administrativos e as notas de preparação [< 1 ms]
2026-11-04T18:56:13.5128677Z   Passed I082 — EP-044 usa a política de propriedade do Cliente (ClienteAutenticadoControllerBase) [< 1 ms]
2026-11-04T18:56:13.5129495Z   Passed I082 — As rotas são /animais/{id}/vacinacoes (GET, POST), /animais/{id}/vacinacoes/{administracaoId} (PUT) e /me/animais/{id}/vacinacoes (GET) [< 1 ms]
2026-11-04T18:56:13.5130184Z   Passed I082 — EP-044 devolve 200 e usa o Cliente do token, nunca do pedido [2 ms]
2026-11-04T18:56:13.5130813Z   Passed I082 — EP-043 devolve 201 com Location /animais/{id}/vacinacoes/{administracaoId} e usa o Veterinário do token [3 ms]
2026-11-04T18:56:14.0415748Z   Passed I082 — RF056 devolve 200 com a administração e passa o animal, o registo e o Veterinário do token [1 ms]
2026-11-04T18:56:14.0432618Z   Passed I082 — um token de Veterinário sem a claim veterinarioId dá 403 sem chamar o serviço [< 1 ms]
2026-11-04T18:56:14.0462742Z   Passed I082 — EP-042, EP-043 e RF056 exigem a política Veterinário e nenhuma ação a dispensa [< 1 ms]
2026-11-04T18:56:14.0485529Z   Passed I082 — EP-044 com um token de Cliente sem a claim clienteId dá 403 sem chamar o serviço [< 1 ms]
2026-11-04T18:56:14.0512776Z   Passed I082 — EP-042 devolve 200 com o boletim e passa o animal e o filtro ao serviço [1 ms]
2026-11-04T18:56:14.0536996Z   Passed I074 — PreconditionFailedException é convertida em HTTP 412 [361 ms]
2026-11-04T18:56:14.0546013Z   Passed I064 — A expiração lida do token validado pela API é a do token emitido [940 ms]
2026-11-04T18:56:14.0632763Z   Passed I064 — A claim exp, em segundos desde 1970, é convertida para UTC [2 ms]
2026-11-04T18:56:14.0663063Z   Passed I065 — Uma palavra-passe forte pode ter espaços [< 1 ms]
2026-11-04T18:56:14.0692600Z   Passed UT-F001-059 — Sem palavra-passe inicial, o pedido de EP-006 é inválido (obrigatória)(palavraPasse: null) [3 ms]
2026-11-04T18:56:14.0712810Z   Passed UT-F001-059 — Sem palavra-passe inicial, o pedido de EP-006 é inválido (obrigatória)(palavraPasse: "") [< 1 ms]
2026-11-04T18:56:14.0714525Z   Passed UT-F001-055 — Sem letra maiúscula, a palavra-passe é inválida [< 1 ms]
2026-11-04T18:56:14.0716254Z   Passed UT-F001-052 — Uma palavra-passe com 7 caracteres é inválida [< 1 ms]
2026-11-04T18:56:14.0717361Z   Passed UT-F001-051 — Uma palavra-passe com 8 caracteres e as quatro classes é válida [< 1 ms]
2026-11-04T18:56:14.0718476Z   Passed UT-F001-050 — "Temp#2026" (exemplo da API) é uma palavra-passe válida [< 1 ms]
2026-11-04T18:56:14.0719468Z   Passed I065 — O espaço não conta como símbolo [< 1 ms]
2026-11-04T18:56:14.6833056Z   Passed UT-F001-056 — Sem letra minúscula, a palavra-passe é inválida [< 1 ms]
2026-11-04T18:56:14.6842109Z   Passed UT-F001-057 — Sem algarismo, a palavra-passe é inválida [< 1 ms]
2026-11-04T18:56:14.6864410Z   Passed I065 — No atributo, um valor vazio fica para o [Required] [< 1 ms]
2026-11-04T18:56:14.6882127Z   Passed UT-F001-053 — Uma palavra-passe com 128 caracteres e as quatro classes é válida [< 1 ms]
2026-11-04T18:56:14.6897846Z   Passed UT-F001-058 — Sem símbolo, a palavra-passe é inválida [< 1 ms]
2026-11-04T18:56:14.6899048Z   Passed UT-F001-054 — Uma palavra-passe com 129 caracteres é inválida (máximo 128) [< 1 ms]
2026-11-04T18:56:14.6900048Z   Passed I071 — EP-021 sem Veterinários devolve uma lista vazia [632 ms]
2026-11-04T18:56:14.6901459Z   Passed UT-F003-032 — veterinarioId inexistente ou de uma conta que não é de Veterinário devolve 404 [1 s]
2026-11-04T18:56:14.7363490Z   Passed UT-F003-048 — segunda chegada devolve 422 e mantém a primeira hora [1 s]
2026-11-04T18:56:14.7385017Z   Passed UT-F003-076 — Auxiliar com apoio cria a nota com o autor e a hora do servidor [1 s]
2026-11-04T18:56:14.7405153Z   Passed UT-F003-049 — If-Match desatualizado devolve 412 e não regista a chegada [11 ms]
2026-11-04T18:56:14.7406478Z   Passed UT-F003-084 — marcação inexistente devolve 404 [3 ms]
2026-11-04T18:56:14.7415803Z   Passed UT-F003-046 e 047 — chegada numa marcação não Confirmada devolve 422(estado: "Pendente") [1 ms]
2026-11-04T18:56:14.7432074Z   Passed UT-F003-046 e 047 — chegada numa marcação não Confirmada devolve 422(estado: "Cancelada") [14 ms]
2026-11-04T18:56:14.7467482Z   Passed I075 — EP-028 marcação inexistente devolve 404 [1 ms]
2026-11-04T18:56:14.7489992Z   Passed UT-F003-045 — chegada numa marcação Confirmada grava a hora do servidor [2 ms]
2026-11-04T18:56:14.7516904Z   Passed UT-F003-078, 079 e 081 — texto vazio, só espaços ou com 1001 caracteres devolve 400(texto: "") [22 ms]
2026-11-04T18:56:14.7542046Z   Passed UT-F003-078, 079 e 081 — texto vazio, só espaços ou com 1001 caracteres devolve 400(texto: "   ") [< 1 ms]
2026-11-04T18:56:14.7571850Z   Passed UT-F003-078, 079 e 081 — texto vazio, só espaços ou com 1001 caracteres devolve 400(texto: null) [< 1 ms]
2026-11-04T18:56:14.7611638Z   Passed UT-F003-083 — marcação Realizada é aceite: só Cancelada impede o registo [3 ms]
2026-11-04T18:56:14.7633612Z   Passed UT-F003-077 — Auxiliar sem autorização de apoio recebe 403 e nada é gravado [1 ms]
2026-11-04T18:56:14.7663352Z   Passed I072 — EP-024 devolve a marcação no corpo e a versão no cabeçalho ETag [7 ms]
2026-11-04T18:56:14.7693411Z   Passed UT-F003-080 e 081 — 1000 caracteres é aceite e 1001 devolve 400(comprimento: 1001, aceite: False) [3 ms]
2026-11-04T18:56:14.7728084Z   Passed I073 — EP-025 devolve 201 com Location, ETag e o utilizador do token como autor [3 ms]
2026-11-04T18:56:14.7729285Z   Passed UT-F003-080 e 081 — 1000 caracteres é aceite e 1001 devolve 400(comprimento: 1000, aceite: True) [1 ms]
2026-11-04T18:56:14.7730238Z   Passed I074 — EP-026 passa If-Match ao serviço e devolve 200 com ETag [1 ms]
2026-11-04T18:56:14.7731101Z   Passed I073 — EP-025 é POST /consultas e exige também a política Rececionista [< 1 ms]
2026-11-04T18:56:14.7732261Z   Passed I073 — EP-025 com um token sem identificador de utilizador dá 403 sem chamar o serviço [< 1 ms]
2026-11-04T18:56:14.7733271Z   Passed I072 — EP-023 e EP-024 só aceitam Rececionista, Veterinário e Auxiliar Veterinário [< 1 ms]
2026-11-04T18:56:14.7734128Z   Passed I074 — EP-027 passa If-Match ao serviço e devolve 204 [1 ms]
2026-11-04T18:56:14.7755585Z   Passed I074 — EP-026 é PUT /consultas/{id} e exige Rececionista [< 1 ms]
2026-11-04T18:56:14.7756282Z   Passed I075 — EP-029 não altera a marcação [3 ms]
2026-11-04T18:56:14.7757017Z   Passed I074 — EP-027 é PATCH /consultas/{id}/estado e exige Rececionista [< 1 ms]
2026-11-04T18:56:14.7757736Z   Passed UT-F003-082 — marcação Cancelada devolve 422 [2 ms]
2026-11-04T18:56:14.7758534Z   Passed I072 — EP-023 passa ao serviço o Veterinário do token, e não o do pedido [1 ms]
2026-11-04T18:56:14.7759350Z   Passed I072 — As rotas são GET /consultas e GET /consultas/{id} [< 1 ms]
2026-11-04T18:56:14.7760135Z   Passed I072 — perfil sem acesso à agenda dá 403 antes de chamar o serviço [1 ms]
2026-11-04T18:56:14.7761746Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClienteTests.TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(telefone: "123456789012345678901") [6 ms]
2026-11-04T18:56:14.7763812Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClienteTests.TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(telefone: "+12345678901234567890") [< 1 ms]
2026-11-04T18:56:14.7765826Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClienteTests.TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(telefone: "12345678") [< 1 ms]
2026-11-04T18:56:14.7767698Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClienteTests.TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(telefone: "91234567a") [< 1 ms]
2026-11-04T18:56:14.7832756Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClienteTests.TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(telefone: "91+2345678") [< 1 ms]
2026-11-04T18:56:14.7852815Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClienteTests.TelefoneAte20CaracteresEAceite(telefone: "+1234567890123456789") [< 1 ms]
2026-11-04T18:56:14.7883050Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClienteTests.TelefoneAte20CaracteresEAceite(telefone: "912345678") [< 1 ms]
2026-11-04T18:56:14.7902907Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClienteTests.TelefoneAte20CaracteresEAceite(telefone: "+351 912 345 678") [< 1 ms]
2026-11-04T18:56:14.7904931Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClienteTests.TelefoneAte20CaracteresEAceite(telefone: "12345678901234567890") [< 1 ms]
2026-11-04T18:56:14.7906802Z   Passed UT-F001-035 — Sem chave, a validação falha e indica Jwt:Key e os User Secrets [< 1 ms]
2026-11-04T18:56:14.7908139Z   Passed UT-F001-038 — Uma chave que não está em Base64 falha com mensagem sobre Base64 [< 1 ms]
2026-11-04T18:56:14.7909402Z   Passed UT-F001-036 — Uma chave com 31 bytes falha: o mínimo é 32 bytes (256 bits) [< 1 ms]
2026-11-04T18:56:14.7910724Z   Passed UT-F001-037 — Chave com 32 bytes, Issuer, Audience e validade de 8 horas: a configuração é aceite [< 1 ms]
2026-11-04T18:56:14.8062544Z   Passed UT-F001-039 — Uma validade de 0 horas falha [< 1 ms]
2026-11-04T18:56:14.8064256Z   Passed UT-F003-013 — Um período de 31 dias é aceite e devolve 31 dias [96 ms]
2026-11-04T18:56:14.8092665Z   Passed UT-F003-028 — marcação das 10:15 às 10:45 locais: nova às 10:30 devolve 422 [100 ms]
2026-11-04T18:56:14.8112985Z   Passed I071 — EP-022 devolve os horários livres em HH:mm, com a duração e o Veterinário [5 ms]
2026-11-04T18:56:14.8128773Z   Passed I071 — EP-022 retira os horários das marcações guardadas, mas não os das canceladas [10 ms]
2026-11-04T18:56:14.8152433Z   Passed UT-TRV-010 — O registo do pedido tem método, caminho, código e duração, mas nunca o token nem o corpo [39 ms]
2026-11-04T18:56:14.8182232Z   Passed UT-F003-018 — O id de quem não é Veterinário, ou inexistente, dá 404(conta: "REC_A") [5 ms]
2026-11-04T18:56:14.8192280Z [xUnit.net 00:00:02.32]     UT-TRV-007 — Uma exceção inesperada dá 500 com mensagem genérica, sem a mensagem interna [SKIP]
2026-11-04T18:56:14.8222823Z [xUnit.net 00:00:02.32]       Defeito D-01 (I028, PA-07): o ExceptionHandlingMiddleware copia para detail a mensagem de qualquer exceção, também no 500. Reativar quando o defeito for corrigido.
2026-11-04T18:56:14.8252182Z   Skipped UT-TRV-007 — Uma exceção inesperada dá 500 com mensagem genérica, sem a mensagem interna [1 ms]
2026-11-04T18:56:14.8282632Z   Passed UT-TRV-006 — Cada exceção prevista dá o seu código HTTP, com detail igual à mensagem e o formato único de erro(tipo: "KeyNotFoundException", esperado: 404) [2 ms]
2026-11-04T18:56:14.8312197Z   Passed UT-F003-018 — O id de quem não é Veterinário, ou inexistente, dá 404(conta: "999") [6 ms]
2026-11-04T18:56:14.8313814Z   Passed UT-TRV-006 — Cada exceção prevista dá o seu código HTTP, com detail igual à mensagem e o formato único de erro(tipo: "UnprocessableEntityException", esperado: 422) [3 ms]
2026-11-04T18:56:14.8342725Z   Passed UT-TRV-006 — Cada exceção prevista dá o seu código HTTP, com detail igual à mensagem e o formato único de erro(tipo: "InvalidOperationException", esperado: 409) [< 1 ms]
2026-11-04T18:56:14.8344610Z   Passed UT-TRV-006 — Cada exceção prevista dá o seu código HTTP, com detail igual à mensagem e o formato único de erro(tipo: "UnauthorizedAccessException", esperado: 401) [< 1 ms]
2026-11-04T18:56:14.8346550Z   Passed UT-TRV-006 — Cada exceção prevista dá o seu código HTTP, com detail igual à mensagem e o formato único de erro(tipo: "ForbiddenException", esperado: 403) [< 1 ms]
2026-11-04T18:56:14.8348446Z   Passed UT-TRV-006 — Cada exceção prevista dá o seu código HTTP, com detail igual à mensagem e o formato único de erro(tipo: "ArgumentException", esperado: 400) [< 1 ms]
2026-11-04T18:56:14.8562842Z   Passed I071 — EP-022 usa a duração configurada em EP-010 e ignora os períodos inativos [7 ms]
2026-11-04T18:56:14.8584211Z   Passed UT-F003-014, UT-F003-015 e UT-F003-016 — Período de 32 dias, invertido ou sem data de início dá 400(dataInicio: "2026-10-01", dataFim: "2026-11-01", mensagem: "O período não pode ter mais de 31 dias.") [4 ms]
2026-11-04T18:56:14.8592925Z   Passed UT-F003-014, UT-F003-015 e UT-F003-016 — Período de 32 dias, invertido ou sem data de início dá 400(dataInicio: "2026-10-15", dataFim: "2026-10-14", mensagem: "A data de fim (dataFim) não pode ser anterior à da"···) [3 ms]
2026-11-04T18:56:14.8595316Z   Passed UT-F003-014, UT-F003-015 e UT-F003-016 — Período de 32 dias, invertido ou sem data de início dá 400(dataInicio: null, dataFim: "2026-10-15", mensagem: "A data de início (dataInicio) é obrigatória, no fo"···) [2 ms]
2026-11-04T18:56:14.8596844Z   Passed I071 — Um Veterinário com a conta bloqueada dá 404 (não está disponível para marcação) [5 ms]
2026-11-04T18:56:14.8597778Z   Passed I071 — EP-021 lista só os Veterinários com conta ativa, por nome [7 ms]
2026-11-04T18:56:14.8598625Z   Passed UT-F003-010 — Uma marcação de outro Veterinário não ocupa o horário [6 ms]
2026-11-04T18:56:14.8599749Z   Passed UT-TRV-009 — Corpo em falta ou JSON mal formado dão 400 com a mensagem genérica, sem mensagens internas(chave: "") [42 ms]
2026-11-04T18:56:14.8729282Z   Passed UT-TRV-009 — Corpo em falta ou JSON mal formado dão 400 com a mensagem genérica, sem mensagens internas(chave: "$.email") [< 1 ms]
2026-11-04T18:56:14.8732509Z   Passed UT-TRV-008 — Erros de validação em email e password dão 400 com as duas mensagens em português no detail [< 1 ms]
2026-11-04T18:56:14.8753598Z   Passed I071 — Um intervalo fora do horário não está disponível(inicio: "2026-10-15T11:45:00Z", duracao: 30) [< 1 ms]
2026-11-04T18:56:14.8754829Z   Passed UT-F003-017 — Sem dataFim, a disponibilidade tem um único dia [9 ms]
2026-11-04T18:56:14.8756470Z   Passed I071 — Um intervalo fora do horário não está disponível(inicio: "2026-10-18T09:00:00Z", duracao: 30) [3 ms]
2026-11-04T18:56:14.8757976Z   Passed I071 — Um intervalo fora do horário não está disponível(inicio: "2026-10-15T07:30:00Z", duracao: 60) [< 1 ms]
2026-11-04T18:56:14.8759378Z   Passed I071 — Um intervalo fora do horário não está disponível(inicio: "2026-10-26T08:30:00Z", duracao: 30) [< 1 ms]
2026-11-04T18:56:14.8760256Z   Passed I071 — EP-021 com Veterinários com o mesmo nome ordena pelo identificador [2 ms]
2026-11-04T18:56:14.9063046Z   Passed I071 — Um período inválido dá 400 antes de se procurar o Veterinário [< 1 ms]
2026-11-04T18:56:14.9064823Z   Passed UT-F003-008 — Com duração de 45 minutos a grelha é de 45 em 45 minutos e só se a consulta couber [3 ms]
2026-11-04T18:56:14.9122382Z   Passed I071 — Uma consulta mais longa do que o período não tem horários [< 1 ms]
2026-11-04T18:56:14.9182802Z   Passed I071 — A hora local de Lisboa é convertida para UTC conforme a hora de verão(data: "2026-10-26", hora: "09:00", esperado: "2026-10-26T09:00:00Z") [< 1 ms]
2026-11-04T18:56:14.9223360Z   Passed I071 — A hora local de Lisboa é convertida para UTC conforme a hora de verão(data: "2026-10-25", hora: "01:30", esperado: "2026-10-25T00:30:00Z") [3 ms]
2026-11-04T18:56:14.9310460Z   Passed I071 — A hora local de Lisboa é convertida para UTC conforme a hora de verão(data: "2026-10-15", hora: "10:00", esperado: "2026-10-15T09:00:00Z") [< 1 ms]
2026-11-04T18:56:14.9382562Z   Passed I071 — O dia da semana segue a base de dados: 1 = segunda … 7 = domingo(data: "2026-10-15", esperado: 4) [< 1 ms]
2026-11-04T18:56:14.9402445Z   Passed I071 — O dia da semana segue a base de dados: 1 = segunda … 7 = domingo(data: "2026-10-12", esperado: 1) [< 1 ms]
2026-11-04T18:56:14.9462652Z   Passed I071 — O dia da semana segue a base de dados: 1 = segunda … 7 = domingo(data: "2026-10-18", esperado: 7) [< 1 ms]
2026-11-04T18:56:14.9492416Z   Passed I071 — O dia da semana segue a base de dados: 1 = segunda … 7 = domingo(data: "2026-10-17", esperado: 6) [< 1 ms]
2026-11-04T18:56:14.9549809Z   Passed UT-F003-012 — Os horários que já começaram não são devolvidos [< 1 ms]
2026-11-04T18:56:14.9582132Z   Passed I071 — Um dia inteiramente no passado não tem horários livres [< 1 ms]
2026-11-04T18:56:14.9612260Z   Passed UT-F003-005 — Uma marcação Cancelada não ocupa horário [1 ms]
2026-11-04T18:56:14.9662340Z   Passed UT-F003-003 — Duas marcações seguidas ocupam os dois horários e libertam os vizinhos [< 1 ms]
2026-11-04T18:56:14.9717192Z   Passed UT-F003-004 — O último horário de cada período acaba no fecho [< 1 ms]
2026-11-04T18:56:14.9742205Z   Passed UT-F003-011 — Na hora de inverno, 09:00Z é 09:00 em Lisboa [< 1 ms]
2026-11-04T18:56:14.9772196Z   Passed I071 — Um intervalo que se sobrepõe a uma marcação não cancelada não está disponível [< 1 ms]
2026-11-04T18:56:14.9802253Z   Passed UT-F003-009 — Uma marcação de 60 minutos às 10:15 ocupa todos os horários que se sobrepõem [< 1 ms]
2026-11-04T18:56:14.9832239Z   Passed UT-F003-006 — Um domingo sem períodos é devolvido com a lista vazia [< 1 ms]
2026-11-04T18:56:14.9856950Z   Passed I071 — Uma marcação cancelada não impede um intervalo [< 1 ms]
2026-11-04T18:56:14.9857929Z   Passed I071 — Vários dias: um elemento por dia, por ordem, incluindo sábado e domingo [1 ms]
2026-11-04T18:56:14.9858930Z   Passed UT-F003-002 — Uma marcação Confirmada às 10:00 locais ocupa só esse horário [< 1 ms]
2026-11-04T18:56:14.9860214Z   Passed I071 — Um intervalo livre e dentro de um período está disponível, mesmo fora da grelha(inicio: "2026-10-15T09:15:00Z", duracao: 30) [< 1 ms]
2026-11-04T18:56:14.9862016Z   Passed I071 — Um intervalo livre e dentro de um período está disponível, mesmo fora da grelha(inicio: "2026-10-15T11:30:00Z", duracao: 30) [< 1 ms]
2026-11-04T18:56:14.9933191Z   Passed I071 — Um intervalo livre e dentro de um período está disponível, mesmo fora da grelha(inicio: "2026-10-15T08:00:00Z", duracao: 30) [< 1 ms]
2026-11-04T18:56:14.9962553Z   Passed I071 — Um intervalo livre e dentro de um período está disponível, mesmo fora da grelha(inicio: "2026-10-26T09:00:00Z", duracao: 30) [< 1 ms]
2026-11-04T18:56:14.9992195Z   Passed UT-F003-007 — Entre os dois períodos do dia (13:00–14:00) não há horários [3 ms]
2026-11-04T18:56:15.0022144Z   Passed I073 — a duração vem da configuração: com 45 minutos, 12:30 locais já não cabe (422) [95 ms]
2026-11-04T18:56:15.0040756Z   Passed I071 — Uma duração não positiva ou um período invertido são recusados [17 ms]
2026-11-04T18:56:15.0082251Z   Passed UT-F003-001 — Sem marcações, um dia útil tem 18 horários de 30 em 30 minutos [< 1 ms]
2026-11-04T18:56:15.0142283Z   Passed I071 — Um horário que começa no próprio instante atual ainda é devolvido [< 1 ms]
2026-11-04T18:56:15.0172341Z   Passed I071 — Uma hora que não existe (mudança para a hora de verão) não tem UTC [< 1 ms]
2026-11-04T18:56:15.0199894Z   Passed I071 — No dia da mudança para a hora de verão, a hora que não existe não é devolvida [< 1 ms]
2026-11-04T18:56:15.0222489Z   Passed I071 — Uma marcação que acaba quando o horário começa não o ocupa (R-HORARIO) [< 1 ms]
2026-11-04T18:56:15.0252591Z   Passed I076 — EP-030 usa a política de propriedade do Cliente e nenhuma ação a dispensa [< 1 ms]
2026-11-04T18:56:15.0312448Z   Passed I073 — motivo com espaços é aparado e motivo vazio fica nulo(motivo: "  Vacinação  ", esperado: "Vacinação") [21 ms]
2026-11-04T18:56:15.0322149Z   Passed I073 — motivo com espaços é aparado e motivo vazio fica nulo(motivo: "   ", esperado: null) [4 ms]
2026-11-04T18:56:15.0325308Z   Passed I076 — EP-030 devolve 200 com a lista do serviço e usa o Cliente do token [8 ms]
2026-11-04T18:56:15.0326599Z   Passed I076 — a única operação é GET /me/consultas (sem criar, alterar, confirmar ou cancelar) [< 1 ms]
2026-11-04T18:56:15.0327813Z   Passed I073 — motivo com espaços é aparado e motivo vazio fica nulo(motivo: null, esperado: null) [8 ms]
2026-11-04T18:56:15.0328906Z   Passed UT-F003-030 — marcação Cancelada às 10:00 locais não ocupa o horário [9 ms]
2026-11-04T18:56:15.0329992Z   Passed UT-F003-022 — animalId = 0 ou veterinarioId = 0 devolve 400(animalId: 0, veterinarioId: 1) [1 ms]
2026-11-04T18:56:15.0331359Z   Passed UT-F003-022 — animalId = 0 ou veterinarioId = 0 devolve 400(animalId: 1, veterinarioId: 0) [< 1 ms]
2026-11-04T18:56:15.0332439Z   Passed UT-F003-025 — início às 13:00 locais (hora de fecho) devolve 422 [7 ms]
2026-11-04T18:56:15.0333623Z   Passed UT-F001-041 — A política Rececionista só aceita o perfil Rececionista(perfil: "Veterinario", aceite: False) [28 ms]
2026-11-04T18:56:15.0334978Z   Passed I073 — hora de inverno: 09:00Z de 2026-10-26 são 09:00 em Lisboa (aceite) e 08:30Z são 08:30 (422) [10 ms]
2026-11-04T18:56:15.0336076Z   Passed I073 — marcação de outro Veterinário à mesma hora não ocupa o horário [5 ms]
2026-11-04T18:56:15.0337752Z   Passed UT-F001-041 — A política Rececionista só aceita o perfil Rececionista(perfil: "Rececionista", aceite: True) [15 ms]
2026-11-04T18:56:15.0338878Z   Passed I073 — motivo com mais de 250 caracteres devolve 400 [2 ms]
2026-11-04T18:56:15.0340092Z   Passed UT-F001-041 — A política Rececionista só aceita o perfil Rececionista(perfil: "AuxiliarVeterinario", aceite: False) [< 1 ms]
2026-11-04T18:56:15.0341677Z   Passed UT-F001-041 — A política Rececionista só aceita o perfil Rececionista(perfil: "Cliente", aceite: False) [< 1 ms]
2026-11-04T18:56:15.0343078Z   Passed UT-F001-041 — A política Rececionista só aceita o perfil Rececionista(perfil: "Administrador", aceite: False) [< 1 ms]
2026-11-04T18:56:15.0344585Z   Passed UT-F001-040 — A política Administrador só aceita o perfil Administrador(perfil: "AuxiliarVeterinario", aceite: False) [< 1 ms]
2026-11-04T18:56:15.0346366Z   Passed UT-F003-021 — data igual a RELOGIO devolve 400: tem de ser estritamente futura [2 ms]
2026-11-04T18:56:15.0347385Z   Passed UT-F003-024 — início às 12:45 locais (acaba às 13:15) devolve 422 [3 ms]
2026-11-04T18:56:15.0348571Z   Passed UT-F001-040 — A política Administrador só aceita o perfil Administrador(perfil: "Cliente", aceite: False) [6 ms]
2026-11-04T18:56:15.0350001Z   Passed UT-F001-040 — A política Administrador só aceita o perfil Administrador(perfil: "Administrador", aceite: True) [< 1 ms]
2026-11-04T18:56:15.0351641Z   Passed UT-F001-040 — A política Administrador só aceita o perfil Administrador(perfil: "Veterinario", aceite: False) [< 1 ms]
2026-11-04T18:56:15.0353243Z   Passed UT-F001-040 — A política Administrador só aceita o perfil Administrador(perfil: "Rececionista", aceite: False) [< 1 ms]
2026-11-04T18:56:15.0354843Z   Passed UT-F001-045 — A política Funcionario aceita os quatro perfis internos e recusa o Cliente(perfil: "AuxiliarVeterinario", aceite: True) [< 1 ms]
2026-11-04T18:56:15.0356522Z   Passed UT-F001-045 — A política Funcionario aceita os quatro perfis internos e recusa o Cliente(perfil: "Administrador", aceite: True) [6 ms]
2026-11-04T18:56:15.0358124Z   Passed UT-F001-045 — A política Funcionario aceita os quatro perfis internos e recusa o Cliente(perfil: "Veterinario", aceite: True) [< 1 ms]
2026-11-04T18:56:15.0359698Z   Passed UT-F001-045 — A política Funcionario aceita os quatro perfis internos e recusa o Cliente(perfil: "Rececionista", aceite: True) [< 1 ms]
2026-11-04T18:56:15.0361433Z   Passed UT-F001-045 — A política Funcionario aceita os quatro perfis internos e recusa o Cliente(perfil: "Cliente", aceite: False) [< 1 ms]
2026-11-04T18:56:15.0362781Z   Passed UT-F003-027 — marcação Confirmada do mesmo Veterinário às 10:00 locais devolve 422 [12 ms]
2026-11-04T18:56:15.0364093Z   Passed UT-F001-044 — A política Cliente só aceita o perfil Cliente(perfil: "AuxiliarVeterinario", aceite: False) [< 1 ms]
2026-11-04T18:56:15.0365485Z   Passed UT-F001-044 — A política Cliente só aceita o perfil Cliente(perfil: "Rececionista", aceite: False) [< 1 ms]
2026-11-04T18:56:15.0366702Z   Passed UT-F003-029 — marcação das 10:00 às 10:30 locais: nova às 10:30 é aceite (só se tocam) [6 ms]
2026-11-04T18:56:15.0367904Z   Passed UT-F001-044 — A política Cliente só aceita o perfil Cliente(perfil: "Cliente", aceite: True) [6 ms]
2026-11-04T18:56:15.0369163Z   Passed UT-F001-044 — A política Cliente só aceita o perfil Cliente(perfil: "Veterinario", aceite: False) [< 1 ms]
2026-11-04T18:56:15.0370434Z   Passed UT-F001-044 — A política Cliente só aceita o perfil Cliente(perfil: "Administrador", aceite: False) [< 1 ms]
2026-11-04T18:56:15.0371636Z   Passed UT-F003-020 — data no passado (RELOGIO − 1 min) devolve 400 [3 ms]
2026-11-04T18:56:15.0372828Z   Passed UT-F001-048 — A política de propriedade recusa os perfis internos mesmo com clienteId(perfil: "Rececionista") [1 ms]
2026-11-04T18:56:15.0374264Z   Passed UT-F001-048 — A política de propriedade recusa os perfis internos mesmo com clienteId(perfil: "AuxiliarVeterinario") [6 ms]
2026-11-04T18:56:15.0375977Z   Passed UT-F001-048 — A política de propriedade recusa os perfis internos mesmo com clienteId(perfil: "Veterinario") [< 1 ms]
2026-11-04T18:56:15.0377443Z   Passed UT-F001-048 — A política de propriedade recusa os perfis internos mesmo com clienteId(perfil: "Administrador") [< 1 ms]
2026-11-04T18:56:15.0378715Z   Passed UT-F001-046 — A política de propriedade aceita um Cliente com a claim clienteId [4 ms]
2026-11-04T18:56:15.0379990Z   Passed UT-F001-042 — A política Veterinario só aceita o perfil Veterinario(perfil: "Cliente", aceite: False) [< 1 ms]
2026-11-04T18:56:15.0381589Z   Passed UT-F001-042 — A política Veterinario só aceita o perfil Veterinario(perfil: "Administrador", aceite: False) [< 1 ms]
2026-11-04T18:56:15.0383014Z   Passed UT-F001-042 — A política Veterinario só aceita o perfil Veterinario(perfil: "Rececionista", aceite: False) [< 1 ms]
2026-11-04T18:56:15.0384822Z   Passed UT-F001-042 — A política Veterinario só aceita o perfil Veterinario(perfil: "AuxiliarVeterinario", aceite: False) [< 1 ms]
2026-11-04T18:56:15.0386161Z   Passed I073 — Veterinário com a conta bloqueada devolve 404, como em EP-021 e EP-022 [19 ms]
2026-11-04T18:56:15.0387546Z   Passed UT-F001-042 — A política Veterinario só aceita o perfil Veterinario(perfil: "Veterinario", aceite: True) [8 ms]
2026-11-04T18:56:15.0388994Z   Passed UT-F001-043 — A política AuxiliarVeterinario só aceita o perfil AuxiliarVeterinario(perfil: "Cliente", aceite: False) [< 1 ms]
2026-11-04T18:56:15.0390638Z   Passed UT-F001-043 — A política AuxiliarVeterinario só aceita o perfil AuxiliarVeterinario(perfil: "Rececionista", aceite: False) [< 1 ms]
2026-11-04T18:56:15.0392552Z   Passed UT-F001-043 — A política AuxiliarVeterinario só aceita o perfil AuxiliarVeterinario(perfil: "Veterinario", aceite: False) [< 1 ms]
2026-11-04T18:56:15.0394235Z   Passed UT-F001-043 — A política AuxiliarVeterinario só aceita o perfil AuxiliarVeterinario(perfil: "Administrador", aceite: False) [< 1 ms]
2026-11-04T18:56:15.0395743Z   Passed I073 — animalId, veterinarioId e dataHora em falta devolvem 400 com as três mensagens [9 ms]
2026-11-04T18:56:15.0397261Z   Passed UT-F001-043 — A política AuxiliarVeterinario só aceita o perfil AuxiliarVeterinario(perfil: "AuxiliarVeterinario", aceite: True) [6 ms]
2026-11-04T18:56:15.0398684Z   Passed UT-F001-047 — A política de propriedade recusa um Cliente sem a claim clienteId [< 1 ms]
2026-11-04T18:56:15.0853187Z   Passed I073 — marcação mais longa que começou antes ocupa o horário (60 min às 09:45 locais) [10 ms]
2026-11-04T18:56:15.0854660Z   Passed UT-F001-049 — Sem autenticação, a política por omissão recusa o acesso (401) [7 ms]
2026-11-04T18:56:15.0883215Z   Passed I065 — Uma conta de Cliente dá 404 (EP-005 só consulta contas de funcionário) [275 ms]
2026-11-04T18:56:15.0912348Z   Passed I073 — dataHora sem indicação de fuso é lida como UTC e os segundos fracionários são retirados [14 ms]
2026-11-04T18:56:15.0942252Z   Passed UT-F003-019 — cria a marcação Pendente, com a duração configurada e a Rececionista como autora [38 ms]
2026-11-04T18:56:15.0945371Z   Passed UT-F003-023 — domingo (clínica fechada) devolve 422 [4 ms]
2026-11-04T18:56:15.0967483Z   Passed UT-F003-031 — animal inexistente devolve 404 [3 ms]
2026-11-04T18:56:15.1132413Z   Passed UT-F003-026 — início às 12:30 locais (acaba às 13:00) é aceite [5 ms]
2026-11-04T18:56:15.1162713Z   Passed UT-F001-002 — Login de um Cliente devolve o clienteId e passa-o ao IJwtTokenService [69 ms]
2026-11-04T18:56:15.1222312Z   Passed UT-F001-021 — O fim de sessão regista o jti como revogado até à expiração do token [232 ms]
2026-11-04T18:56:15.1242593Z   Passed I064 — Um jti vazio é recusado (400) e nada é gravado(jti: "   ") [1 ms]
2026-11-04T18:56:15.1282094Z   Passed I064 — Um jti vazio é recusado (400) e nada é gravado(jti: "") [1 ms]
2026-11-04T18:56:15.1312278Z   Passed UT-F001-006 — O e-mail é comparado sem os espaços iniciais e finais [15 ms]
2026-11-04T18:56:15.1341741Z   Passed I064 — O fim de sessão não afeta os outros tokens do mesmo utilizador [15 ms]
2026-11-04T18:56:15.1403199Z   Passed I064 — A revogação fica na base de dados e continua a valer num novo arranque da API [3 ms]
2026-11-04T18:56:15.1422333Z   Passed UT-F001-093 — Um estado diferente de Bloqueado dá 400 e a conta mantém-se ativa(estado: "Suspenso") [86 ms]
2026-11-04T18:56:15.1492801Z   Passed UT-F001-022 — Terminar a sessão duas vezes com o mesmo jti não falha nem repete a revogação [4 ms]
2026-11-04T18:56:15.1502044Z   Passed UT-F001-020 — Conta bloqueada em /auth/me devolve 401 [17 ms]
2026-11-04T18:56:15.1522241Z   Passed UT-F001-010 — Conta bloqueada com a palavra-passe errada devolve 401 e não 403 [3 ms]
2026-11-04T18:56:15.1523580Z   Passed UT-F001-001 — Login do Administrador devolve o token do IJwtTokenService e regista o último acesso [7 ms]
2026-11-04T18:56:15.1525173Z   Passed UT-F001-013 — Conta ativa, com o perfil do token e jti não revogado: a sessão é válida [2 ms]
2026-11-04T18:56:15.1528911Z   Passed I064 — A limpeza apaga só as revogações de tokens já expirados [23 ms]
2026-11-04T18:56:15.1529887Z   Passed UT-F001-016 — Um token com o jti revogado deixa de ser uma sessão válida [1 ms]
2026-11-04T18:56:15.1530885Z   Passed UT-F001-014 — Conta bloqueada depois da emissão do token: a sessão deixa de ser válida [8 ms]
2026-11-04T18:56:15.1588258Z   Passed UT-F001-011 — Com SuccessRehashNeeded, o login aceita e grava o novo hash [1 ms]
2026-11-04T18:56:15.1622171Z   Passed I064 — Depois do fim de sessão, o mesmo token deixa de ser uma sessão válida [4 ms]
2026-11-04T18:56:15.1652253Z   Passed UT-F001-093 — Um estado diferente de Bloqueado dá 400 e a conta mantém-se ativa(estado: "Ativo") [30 ms]
2026-11-04T18:56:15.1679758Z   Passed UT-F001-012 — O registo da tentativa falhada tem o e-mail e o motivo, mas nunca a palavra-passe [5 ms]
2026-11-04T18:56:15.1725008Z   Passed UT-F001-082 — Um nome com 151 caracteres dá 400 [7 ms]
2026-11-04T18:56:15.1729993Z   Passed UT-F001-009 — Conta bloqueada com a palavra-passe certa devolve 403 sem emitir token [6 ms]
2026-11-04T18:56:15.1762045Z   Passed I064 — A limpeza mantém a revogação de um token que expira no próprio instante [13 ms]
2026-11-04T18:56:15.1771916Z   Passed UT-F001-008 — Palavra-passe errada devolve 401 com a mesma mensagem, sem token e sem alterar o último acesso [3 ms]
2026-11-04T18:56:15.1792058Z   Passed UT-F001-019 — Cliente autenticado: clienteId e nome do registo Cliente [1 ms]
2026-11-04T18:56:15.1812343Z   Passed I064 — A data de revogação é gravada sem frações de segundo (DATETIME2(0)) [5 ms]
2026-11-04T18:56:15.1852013Z   Passed I064 — Sem revogações expiradas, a limpeza não apaga nada [1 ms]
2026-11-04T18:56:15.2027369Z   Passed UT-F001-003 — E-mail vazio devolve 400 «O e-mail é obrigatório.» sem consultar o hasher nem a base de dados [12 ms]
2026-11-04T18:56:15.2042029Z   Passed UT-F002-025 — Nascido a 2022-05-10, em 2026-10-14 tem 4 anos [90 ms]
2026-11-04T18:56:15.2072132Z   Passed UT-F001-018 — Veterinário autenticado: nome do Funcionario, perfil Veterinario e sem clienteId [2 ms]
2026-11-04T18:56:15.2101966Z   Passed UT-F002-029 — Nascido a 2024-02-29, a 2025-03-01 tem 1 ano [4 ms]
2026-11-04T18:56:15.2132122Z   Passed UT-F001-017 — Utilizador inexistente: a sessão não é válida [6 ms]
2026-11-04T18:56:15.2162049Z   Passed UT-F001-005 — Um e-mail só com espaços conta como em falta (400) [7 ms]
2026-11-04T18:56:15.2186602Z   Passed UT-F001-004 — Palavra-passe vazia devolve 400 «A palavra-passe é obrigatória.» [1 ms]
2026-11-04T18:56:15.2211936Z   Passed UT-F001-084 — O perfil Cliente dá 400 (e não 422) e nada muda [44 ms]
2026-11-04T18:56:15.2287921Z   Passed UT-F001-015 — Perfil alterado depois da emissão do token: a sessão deixa de ser válida [5 ms]
2026-11-04T18:56:15.2302337Z   Passed UT-F001-007 — E-mail inexistente devolve 401 «Credenciais inválidas.», verifica um hash de referência e regista o evento 3001 [5 ms]
2026-11-04T18:56:15.2332147Z   Passed I065 — Um perfil fora da enumeração dá 400 (e não 422)(perfil: "Gestor") [14 ms]
2026-11-04T18:56:15.2362377Z   Passed UT-TRV-002/003 (EP-023) — tamanho entre 1 e 100 e página a partir de 1(pagina: 0, tamanho: 20, valido: False) [6 ms]
2026-11-04T18:56:15.2392802Z   Passed I065 — Um perfil fora da enumeração dá 400 (e não 422)(perfil: "Auxiliar") [< 1 ms]
2026-11-04T18:56:15.2432384Z   Passed UT-TRV-002/003 (EP-023) — tamanho entre 1 e 100 e página a partir de 1(pagina: 1, tamanho: 0, valido: False) [< 1 ms]
2026-11-04T18:56:15.2447947Z   Passed UT-TRV-002/003 (EP-023) — tamanho entre 1 e 100 e página a partir de 1(pagina: 1, tamanho: 101, valido: False) [< 1 ms]
2026-11-04T18:56:15.2492362Z   Passed UT-TRV-002/003 (EP-023) — tamanho entre 1 e 100 e página a partir de 1(pagina: 1, tamanho: 1, valido: True) [< 1 ms]
2026-11-04T18:56:15.2535035Z   Passed UT-TRV-002/003 (EP-023) — tamanho entre 1 e 100 e página a partir de 1(pagina: 2147483647, tamanho: 20, valido: True) [< 1 ms]
2026-11-04T18:56:15.2552311Z   Passed UT-TRV-002/003 (EP-023) — tamanho entre 1 e 100 e página a partir de 1(pagina: 1, tamanho: 100, valido: True) [< 1 ms]
2026-11-04T18:56:15.2554243Z   Passed I072 — o estado tem de ser um valor de EstadoConsulta, sem distinguir maiúsculas(estado: "Confirmadas", valido: False) [< 1 ms]
2026-11-04T18:56:15.2555614Z   Passed I072 — o estado tem de ser um valor de EstadoConsulta, sem distinguir maiúsculas(estado: "CANCELADA", valido: True) [< 1 ms]
2026-11-04T18:56:15.2556911Z   Passed I072 — o estado tem de ser um valor de EstadoConsulta, sem distinguir maiúsculas(estado: "", valido: True) [< 1 ms]
2026-11-04T18:56:15.2558205Z   Passed I072 — o estado tem de ser um valor de EstadoConsulta, sem distinguir maiúsculas(estado: "Pendente", valido: True) [< 1 ms]
2026-11-04T18:56:15.2559529Z   Passed I072 — o estado tem de ser um valor de EstadoConsulta, sem distinguir maiúsculas(estado: "confirmada", valido: True) [< 1 ms]
2026-11-04T18:56:15.2560901Z   Passed I072 — o estado tem de ser um valor de EstadoConsulta, sem distinguir maiúsculas(estado: " Realizada ", valido: True) [< 1 ms]
2026-11-04T18:56:15.2562357Z   Passed I072 — o estado tem de ser um valor de EstadoConsulta, sem distinguir maiúsculas(estado: "Faturada", valido: False) [< 1 ms]
2026-11-04T18:56:15.2702647Z   Passed UT-F003-052 (validação do pedido) — dataFim anterior a dataInicio é inválida [< 1 ms]
2026-11-04T18:56:15.2722075Z   Passed I072 — datas fora de 2000-01-01 a 9998-12-31 são inválidas; os limites são aceites(ano: 9999, mes: 1, dia: 1, valida: False) [< 1 ms]
2026-11-04T18:56:15.2732339Z   Passed I072 — datas fora de 2000-01-01 a 9998-12-31 são inválidas; os limites são aceites(ano: 9998, mes: 12, dia: 31, valida: True) [< 1 ms]
2026-11-04T18:56:15.2733789Z   Passed I072 — datas fora de 2000-01-01 a 9998-12-31 são inválidas; os limites são aceites(ano: 1999, mes: 12, dia: 31, valida: False) [< 1 ms]
2026-11-04T18:56:15.2735161Z   Passed I072 — datas fora de 2000-01-01 a 9998-12-31 são inválidas; os limites são aceites(ano: 2000, mes: 1, dia: 1, valida: True) [< 1 ms]
2026-11-04T18:56:15.2736234Z   Passed UT-TRV-001 — sem pagina nem tamanho, pagina = 1 e tamanho = 20 [< 1 ms]
2026-11-04T18:56:15.2737422Z   Passed I072 — veterinarioId e animalId, quando indicados, têm de ser superiores a 0(veterinarioId: null, animalId: 0) [< 1 ms]
2026-11-04T18:56:15.2738572Z   Passed I072 — veterinarioId e animalId, quando indicados, têm de ser superiores a 0(veterinarioId: 0, animalId: null) [6 ms]
2026-11-04T18:56:15.2739748Z   Passed I072 — veterinarioId e animalId, quando indicados, têm de ser superiores a 0(veterinarioId: -1, animalId: null) [< 1 ms]
2026-11-04T18:56:15.2852319Z   Passed I066 — O estado Bloqueado é aceite sem distinguir maiúsculas e sem motivo(estado: " BLOQUEADO ") [18 ms]
2026-11-04T18:56:15.2882107Z   Passed I072 — todos os filtros são opcionais e um só dia (dataInicio = dataFim) é válido [4 ms]
2026-11-04T18:56:15.2883275Z   Passed I066 — O estado Bloqueado é aceite sem distinguir maiúsculas e sem motivo(estado: "bloqueado") [4 ms]
2026-11-04T18:56:15.2912021Z   Passed UT-F001-115 — Um e-mail sem @ é recusado no pedido (400) [1 ms]
2026-11-04T18:56:15.2942127Z   Passed UT-F001-114 — Um NIF com o dígito de controlo errado é recusado no pedido (400) [< 1 ms]
2026-11-04T18:56:15.2943706Z   Passed UT-F001-116 — Sem telefone, o pedido é recusado (400) [< 1 ms]
2026-11-04T18:56:15.2944532Z   Passed UT-F002-011 — Data de nascimento de hoje é aceite e a idade é 0 [65 ms]
2026-11-04T18:56:15.3192825Z   Passed I065 — Um estado fora da enumeração dá 400 [15 ms]
2026-11-04T18:56:15.3222287Z   Passed UT-F002-010 — Data de nascimento de amanhã (2026-10-15) devolve 400 [4 ms]
2026-11-04T18:56:15.3223546Z   Passed UT-F002-028 — Nascido a 2024-02-29, a 2025-02-28 ainda tem 0 anos [8 ms]
2026-11-04T18:56:15.3272315Z   Passed UT-F002-003 — clienteId = 0 (em falta) é recusado no pedido (400) [9 ms]
2026-11-04T18:56:15.3332258Z   Passed UT-F001-087 — podeRegistarApoio num Veterinário dá 422 e nada muda [33 ms]
2026-11-04T18:56:15.3372105Z   Passed UT-F001-089 — Manter o próprio e-mail (com outras maiúsculas) não é conflito [9 ms]
2026-11-04T18:56:15.3402190Z   Passed UT-F002-002 — Sem raça, data de nascimento e peso, o paciente é registado com esses campos vazios [38 ms]
2026-11-04T18:56:15.3413520Z   Passed UT-F001-092 — Bloquear REC_A ativo passa o estado a Bloqueado [16 ms]
2026-11-04T18:56:15.3589566Z   Passed UT-F001-079 — Um e-mail sem formato válido dá 400 com o erro no campo email [< 1 ms]
2026-11-04T18:56:15.3642217Z   Passed UT-F001-078 — Um nome vazio dá 400 com o erro no campo nome [4 ms]
2026-11-04T18:56:15.3673558Z   Passed I065 — Os DTOs de resposta não têm campos de palavra-passe nem de hash (critério 4) [< 1 ms]
2026-11-04T18:56:15.3732118Z   Passed I066 — Um pedido sem estado dá 400 na validação do DTO [< 1 ms]
2026-11-04T18:56:15.3756255Z   Passed I082 — «hoje» é o dia em Lisboa: às 23:30 UTC de 13/10 (00:30 de 14/10 em Lisboa) aceita 2026-10-14 [157 ms]
2026-11-04T18:56:15.3792140Z   Passed UT-F002-023 — O CLIENTE_A pede o ANIMAL_B1 e recebe 403 sem dados do animal [37 ms]
2026-11-04T18:56:15.3832108Z   Passed UT-F002-013 — Uma raça de gato (Siamês) num cão devolve 400 com erro na raça [7 ms]
2026-11-04T18:56:15.3913864Z   Passed UT-F002-024 — O CLIENTE_A pede um animal inexistente e recebe 404 [1 ms]
2026-11-04T18:56:15.3938104Z   Passed UT-F006-021 — próxima prevista para ontem (2026-10-13) não aparece em proximas [36 ms]
2026-11-04T18:56:15.3982588Z   Passed UT-F006-010 — sem tipo, produto, lote ou data de administração devolve 400 com o campo em falta(campo: "tipo", mensagem: "O tipo é obrigatório.") [5 ms]
2026-11-04T18:56:15.3984408Z   Passed UT-F006-010 — sem tipo, produto, lote ou data de administração devolve 400 com o campo em falta(campo: "dataAdministracao", mensagem: "A data de administração é obrigatória.") [1 ms]
2026-11-04T18:56:15.3986246Z   Passed UT-F006-010 — sem tipo, produto, lote ou data de administração devolve 400 com o campo em falta(campo: "produto", mensagem: "O produto é obrigatório.") [3 ms]
2026-11-04T18:56:15.3987848Z   Passed UT-F006-010 — sem tipo, produto, lote ou data de administração devolve 400 com o campo em falta(campo: "lote", mensagem: "O lote é obrigatório.") [1 ms]
2026-11-04T18:56:15.3989035Z   Passed UT-F006-017 — animal inexistente (id = 999) devolve 404 [4 ms]
2026-11-04T18:56:15.3989809Z   Passed I082 — próximas por ordem crescente, incluindo a de hoje [6 ms]
2026-11-04T18:56:15.4352782Z   Passed UT-F002-020 — Na área do Cliente, o CLIENTE_A vê só os seus animais [41 ms]
2026-11-04T18:56:15.4408846Z   Passed I065 — O e-mail é guardado em minúsculas e um repetido com outras maiúsculas dá 409 [93 ms]
2026-11-04T18:56:15.4472524Z   Passed I089 — a atualização recalcula a próxima nos casos de calendário: fim de mês, fim de ano e 29 de fevereiro(dataAdministracao: "2025-12-31", intervaloDias: 1, esperada: "2026-01-01") [25 ms]
2026-11-04T18:56:15.4522720Z   Passed I065 — podeRegistarApoio em qualquer perfil que não seja AuxiliarVeterinario dá 422(perfil: "Veterinario") [1 ms]
2026-11-04T18:56:15.4583687Z   Passed I065 — podeRegistarApoio em qualquer perfil que não seja AuxiliarVeterinario dá 422(perfil: "Administrador") [< 1 ms]
2026-11-04T18:56:15.4629169Z   Passed I089 — a atualização recalcula a próxima nos casos de calendário: fim de mês, fim de ano e 29 de fevereiro(dataAdministracao: "2026-01-31", intervaloDias: 1, esperada: "2026-02-01") [6 ms]
2026-11-04T18:56:15.4702732Z   Passed I089 — a atualização recalcula a próxima nos casos de calendário: fim de mês, fim de ano e 29 de fevereiro(dataAdministracao: "2024-02-28", intervaloDias: 1, esperada: "2024-02-29") [3 ms]
2026-11-04T18:56:15.4732198Z   Passed UT-F001-119 — Um novo e-mail atualiza o Cliente e o e-mail da conta Utilizador [188 ms]
2026-11-04T18:56:15.4751702Z   Passed I065 — Com perfil=Cliente a lista fica vazia (EP-004 só lista funcionários) [13 ms]
2026-11-04T18:56:15.4806355Z   Passed I089 — a atualização recalcula a próxima nos casos de calendário: fim de mês, fim de ano e 29 de fevereiro(dataAdministracao: "2023-03-01", intervaloDias: 365, esperada: "2024-02-29") [7 ms]
2026-11-04T18:56:15.4834228Z   Passed I082 — o boletim não inclui registos de outros animais [4 ms]
2026-11-04T18:56:15.4882348Z   Passed UT-F006-018 — administrações da mais recente para a mais antiga; próximas só com a vacina de 2027-10-14 [5 ms]
2026-11-04T18:56:15.4912311Z   Passed UT-F002-014 — Atualizar o peso do ANIMAL_A1 grava o novo peso e DataUltimaAlteracao = RELOGIO [45 ms]
2026-11-04T18:56:15.4992288Z   Passed I082 — atualizar a data de administração recalcula a próxima (2026-09-01 + 365 = 2027-09-01) [7 ms]
2026-11-04T18:56:15.5052308Z   Passed UT-F002-001 — Registar o Bobi cria o paciente do CLIENTE_A com DataRegisto = RELOGIO e idadeAnos = 4 [6 ms]
2026-11-04T18:56:15.5061967Z   Passed UT-F002-017 — Atualizar para um Cliente inexistente devolve 404 [1 ms]
2026-11-04T18:56:15.5082392Z   Passed I082 — atualizar um registo feito por outro Veterinário devolve 403 [5 ms]
2026-11-04T18:56:15.5102203Z   Passed UT-F001-112 — Criar um Cliente cria a conta Utilizador (Cliente, Ativo) e o Cliente ligado a ela; o e-mail fica só na conta [30 ms]
2026-11-04T18:56:15.5103066Z   Passed UT-F001-122 — Um NIF com 5 algarismos é recusado no pedido de atualização (400) [< 1 ms]
2026-11-04T18:56:15.5103930Z   Passed I065 — A pesquisa procura no nome e no e-mail e nunca devolve Clientes(pesquisa: "Vet", esperados: "Vasco Vet|Vera Vet|Vitor Vet") [30 ms]
2026-11-04T18:56:15.5104801Z   Passed UT-F001-120 — Atualizar um Cliente com o NIF de outro Cliente devolve 409 [2 ms]
2026-11-04T18:56:15.5105423Z   Passed UT-F002-022 — O CLIENTE_A consulta o ANIMAL_A1 e recebe o MeuAnimalDto [8 ms]
2026-11-04T18:56:15.5106021Z   Passed UT-F002-006 — Um nome com 81 caracteres é recusado (máximo 80) [< 1 ms]
2026-11-04T18:56:15.5106789Z   Passed I065 — A pesquisa procura no nome e no e-mail e nunca devolve Clientes(pesquisa: "carla.aux@", esperados: "Carla Aux") [4 ms]
2026-11-04T18:56:15.5107503Z   Passed UT-F002-021 — Um Cliente sem animais recebe uma lista vazia, sem erro [1 ms]
2026-11-04T18:56:15.5108137Z   Passed UT-F006-011 — data de administração de amanhã (2026-10-15) devolve 400 [8 ms]
2026-11-04T18:56:15.5108874Z   Passed I065 — A pesquisa procura no nome e no e-mail e nunca devolve Clientes(pesquisa: "costa", esperados: "") [3 ms]
2026-11-04T18:56:15.5109595Z   Passed UT-F001-091 — Um e-mail sem formato válido dá 400 com o erro no campo email [< 1 ms]
2026-11-04T18:56:15.5110191Z   Passed UT-F001-121 — Manter o próprio NIF não conta como conflito [8 ms]
2026-11-04T18:56:15.5110807Z   Passed I066 — Um perfil fora da enumeração dá 400 na atualização(perfil: "Auxiliar") [1 ms]
2026-11-04T18:56:15.5111540Z   Passed I066 — Um perfil fora da enumeração dá 400 na atualização(perfil: "Gestor") [< 1 ms]
2026-11-04T18:56:15.5112120Z   Passed UT-F001-123 — Atualizar um Cliente inexistente devolve 404 [4 ms]
2026-11-04T18:56:15.5112751Z   Passed UT-F006-019 — tipo filtra as administrações e as próximas(tipo: "Desparasitacao") [29 ms]
2026-11-04T18:56:15.5199694Z   Passed UT-F006-019 — tipo filtra as administrações e as próximas(tipo: "Vacina") [5 ms]
2026-11-04T18:56:15.5201948Z   Passed I065 — Contas com o mesmo nome ficam ordenadas pelo identificador (ordem estável) [29 ms]
2026-11-04T18:56:15.5203380Z   Passed I065 — Um identificador inexistente dá 404 [1 ms]
2026-11-04T18:56:15.5204258Z   Passed I082 — EP-042 com animal inexistente devolve 404 [3 ms]
2026-11-04T18:56:15.5205707Z   Passed I065 — A paginação devolve a página pedida; depois do fim, ou numa página muito alta, a lista fica vazia(pagina: 2147483647, tamanho: 100, esperados: "") [3 ms]
2026-11-04T18:56:15.5207590Z   Passed I065 — A paginação devolve a página pedida; depois do fim, ou numa página muito alta, a lista fica vazia(pagina: 3, tamanho: 3, esperados: "Vera Vet|Vitor Vet") [10 ms]
2026-11-04T18:56:15.5209342Z   Passed I065 — A paginação devolve a página pedida; depois do fim, ou numa página muito alta, a lista fica vazia(pagina: 4, tamanho: 3, esperados: "") [2 ms]
2026-11-04T18:56:15.5210556Z   Passed I082 — tipo fora de Vacina/Desparasitacao devolve 400 [16 ms]
2026-11-04T18:56:15.5563126Z   Passed I065 — A paginação devolve a página pedida; depois do fim, ou numa página muito alta, a lista fica vazia(pagina: 2, tamanho: 3, esperados: "Rita Rececao|Rui Bloqueado|Vasco Vet") [5 ms]
2026-11-04T18:56:15.5569533Z   Passed I082 — animal sem registos: boletim com as duas listas vazias [6 ms]
2026-11-04T18:56:15.5602071Z   Passed I082 — atualizar o intervalo recalcula a próxima, e retirá-lo deixa-a nula [7 ms]
2026-11-04T18:56:15.5603592Z   Passed UT-F001-124 — A pesquisa "Costa" devolve só o CLIENTE_A, com numeroAnimais = 2 [66 ms]
2026-11-04T18:56:15.5632166Z   Passed UT-F006-013 e UT-F006-015 — intervaloDias = 0 ou negativo devolve 400(intervaloDias: -30) [5 ms]
2026-11-04T18:56:15.5662152Z   Passed UT-F006-013 e UT-F006-015 — intervaloDias = 0 ou negativo devolve 400(intervaloDias: 0) [2 ms]
2026-11-04T18:56:15.5679047Z   Passed UT-F001-117 — O NIF de outro Cliente devolve 409 e nada é gravado [11 ms]
2026-11-04T18:56:15.5814576Z   Passed UT-F001-118 — Um e-mail já registado devolve 409 e nada é gravado [1 ms]
2026-11-04T18:56:15.5842239Z   Passed I082 — a atualização com dados inválidos devolve 400 e o registo fica como estava [9 ms]
2026-11-04T18:56:15.5872137Z   Passed UT-F002-019 — Filtrar por clienteId do CLIENTE_A devolve só o ANIMAL_A1 e o ANIMAL_A2 [100 ms]
2026-11-04T18:56:15.5873123Z   Passed UT-F001-113 — Sem NIF, o Cliente é criado (NIF opcional) [12 ms]
2026-11-04T18:56:15.5902423Z   Passed I089 — dar intervalo a uma desparasitação sem próxima (2026-08-01 + 90) cria a próxima de 2026-10-30, que entra nas próximas do boletim [10 ms]
2026-11-04T18:56:15.5932116Z   Passed UT-F002-012 — Uma espécie que não existe devolve 400 com erro na espécie [5 ms]
2026-11-04T18:56:15.5955992Z   Passed UT-F002-030 — Sem data de nascimento, idadeAnos fica vazio (divergência registada; a I028 propunha 0) [2 ms]
2026-11-04T18:56:15.6099130Z   Passed UT-F002-008 — Peso 0,01 é aceite [7 ms]
2026-11-04T18:56:15.6122161Z   Passed UT-F002-015 — Atualizar com peso 0 devolve 400 e nada é alterado [8 ms]
2026-11-04T18:56:15.6133066Z   Passed UT-F002-026 — Nascido a 2022-10-14, faz 4 anos no próprio dia [2 ms]
2026-11-04T18:56:15.6162612Z   Passed I082 — EP-044 com animal inexistente devolve 404 [24 ms]
2026-11-04T18:56:15.6183273Z   Passed UT-F002-016 — Atualizar um animal inexistente devolve 404 [5 ms]
2026-11-04T18:56:15.6222135Z   Passed UT-F002-018 — A ficha do ANIMAL_A1 tem só dados administrativos e o telefone do Cliente [7 ms]
2026-11-04T18:56:15.6223124Z   Passed UT-F002-007 — Peso 0 devolve 400 «O peso deve ser superior a 0.» [1 ms]
2026-11-04T18:56:15.6230850Z   Passed UT-F006-023 — o CLIENTE_A pede o boletim do ANIMAL_B1: 403 [8 ms]
2026-11-04T18:56:15.6282974Z   Passed UT-TRV-005 — Com 57 Clientes, a página 3 de 20 tem 17 elementos e total = 57 [43 ms]
2026-11-04T18:56:15.6301851Z   Passed UT-F006-020 — tipo = "Vacinas" devolve 400 [5 ms]
2026-11-04T18:56:15.6312288Z   Passed UT-F002-009 — Peso negativo devolve 400 [7 ms]
2026-11-04T18:56:15.6314950Z   Passed UT-F002-027 — Nascido a 2022-10-15, ainda tem 3 anos na véspera do aniversário [2 ms]
2026-11-04T18:56:15.6317300Z   Passed UT-F001-074 — Criar um Veterinário sem podeRegistarApoio guarda a autorização a false [92 ms]
2026-11-04T18:56:15.6319045Z   Passed UT-F001-094 — Bloquear uma conta já bloqueada dá 422 [1 ms]
2026-11-04T18:56:15.6482595Z   Passed UT-F002-005 — Sem nome, o pedido é recusado (400) [4 ms]
2026-11-04T18:56:15.6493669Z   Passed UT-TRV-001 — Sem pagina nem tamanho, a lista usa a página 1 com 20 elementos [4 ms]
2026-11-04T18:56:15.6522039Z   Passed UT-F006-024 — ANIMAL_A2 sem registos: historico = [] e proximas = [] [13 ms]
2026-11-04T18:56:15.6552073Z   Passed UT-F006-016 — lote com 51 caracteres devolve 400 (máximo 50) [3 ms]
2026-11-04T18:56:15.6582133Z   Passed UT-F002-004 — Um Cliente inexistente devolve 404 e nada é gravado [9 ms]
2026-11-04T18:56:15.6583334Z   Passed I066 — Depois da mudança de perfil, o token antigo deixa de ser aceite (EP-003) [10 ms]
2026-11-04T18:56:15.6624539Z   Passed UT-F001-097 — Bloquear um identificador inexistente dá 404 [1 ms]
2026-11-04T18:56:15.6652344Z   Passed UT-TRV-004 — tamanho = 0 ou pagina = 0 são recusados (400)(pagina: 0, tamanho: 20, campo: "Pagina") [3 ms]
2026-11-04T18:56:15.6682291Z   Passed UT-TRV-004 — tamanho = 0 ou pagina = 0 são recusados (400)(pagina: 1, tamanho: 0, campo: "Tamanho") [4 ms]
2026-11-04T18:56:15.6702068Z   Passed UT-TRV-003 — tamanho = 101 é recusado (400) [< 1 ms]
2026-11-04T18:56:15.6712142Z   Passed UT-F001-096 — Bloquear uma conta de Cliente dá 422 e a conta mantém-se ativa [5 ms]
2026-11-04T18:56:15.6748603Z   Passed UT-F001-125 — O ClienteDto tem o e-mail da conta, o estado da conta e os dois animais [35 ms]
2026-11-04T18:56:15.6772101Z   Passed UT-F001-024 — O token de um Veterinário tem veterinarioId e não tem clienteId [1 ms]
2026-11-04T18:56:15.6802241Z   Passed UT-F001-032 — Um token de Cliente com o payload de um Administrador e a assinatura original é rejeitado [3 ms]
2026-11-04T18:56:15.6803486Z   Passed UT-F001-029 — Um token emitido há 8 h 01 min é rejeitado por ter expirado [3 ms]
2026-11-04T18:56:15.6832140Z   Passed UT-F006-022 — o Cliente vê o mesmo histórico e as mesmas próximas do boletim web [25 ms]
2026-11-04T18:56:15.6862180Z   Passed UT-F001-033 — Um token sem assinatura (alg = none) é rejeitado: só HS256 é aceite [5 ms]
2026-11-04T18:56:15.6899182Z   Passed UT-F001-028 — Um token emitido agora é aceite pela configuração da API e a claim role é lida com o nome curto [1 ms]
2026-11-04T18:56:15.6900450Z   Passed UT-F001-031 — Um token assinado com outra chave de 256 bits é rejeitado [1 ms]
2026-11-04T18:56:15.6932331Z   Passed UT-F001-025 — O token expira exatamente 8 horas depois, sem frações de segundo, e expiraEm é igual a exp [< 1 ms]
2026-11-04T18:56:15.6962108Z   Passed UT-F006-009 — desparasitação sem intervalo: dataProximaPrevista = null [7 ms]
2026-11-04T18:56:15.6971974Z   Passed UT-F001-027 — O cabeçalho do token indica o algoritmo HS256 [< 1 ms]
2026-11-04T18:56:15.6992287Z   Passed UT-F001-023 — O token de um Cliente tem sub, email, role, clienteId e jti, sem veterinarioId [< 1 ms]
2026-11-04T18:56:15.7022107Z   Passed UT-F001-030 — Um token emitido há 7 h 59 min ainda é aceite (sem tolerância de relógio) [1 ms]
2026-11-04T18:56:15.7050850Z   Passed I089 — vacinação sem intervalo e desparasitação com intervalo: a próxima depende só do intervalo, não do tipo(tipo: "Vacina", intervaloDias: null, esperada: null) [3 ms]
2026-11-04T18:56:15.7082139Z   Passed UT-F001-034 — Um token assinado com a chave certa mas com outro emissor é rejeitado [5 ms]
2026-11-04T18:56:15.7092021Z   Passed UT-F001-026 — Dois tokens do mesmo utilizador têm jti diferentes [< 1 ms]
2026-11-04T18:56:15.7123037Z   Passed UT-F006-013, UT-F006-014, UT-F006-015 e UT-F006-016 — intervaloDias de 1 a 32767 quando indicado; lote até 50 e produto até 150 caracteres(intervaloDias: 32767, tamanhoLote: 50, tamanhoProduto: 150, valido: True) [< 1 ms]
2026-11-04T18:56:15.7152949Z   Passed UT-F006-013, UT-F006-014, UT-F006-015 e UT-F006-016 — intervaloDias de 1 a 32767 quando indicado; lote até 50 e produto até 150 caracteres(intervaloDias: 32768, tamanhoLote: 50, tamanhoProduto: 150, valido: False) [< 1 ms]
2026-11-04T18:56:15.7173649Z   Passed UT-F006-013, UT-F006-014, UT-F006-015 e UT-F006-016 — intervaloDias de 1 a 32767 quando indicado; lote até 50 e produto até 150 caracteres(intervaloDias: 1, tamanhoLote: 50, tamanhoProduto: 150, valido: True) [< 1 ms]
2026-11-04T18:56:15.7183064Z   Passed UT-F006-013, UT-F006-014, UT-F006-015 e UT-F006-016 — intervaloDias de 1 a 32767 quando indicado; lote até 50 e produto até 150 caracteres(intervaloDias: 90, tamanhoLote: 51, tamanhoProduto: 150, valido: False) [< 1 ms]
2026-11-04T18:56:15.7212805Z   Passed UT-F006-013, UT-F006-014, UT-F006-015 e UT-F006-016 — intervaloDias de 1 a 32767 quando indicado; lote até 50 e produto até 150 caracteres(intervaloDias: -30, tamanhoLote: 50, tamanhoProduto: 150, valido: False) [< 1 ms]
2026-11-04T18:56:15.7242973Z   Passed UT-F006-013, UT-F006-014, UT-F006-015 e UT-F006-016 — intervaloDias de 1 a 32767 quando indicado; lote até 50 e produto até 150 caracteres(intervaloDias: null, tamanhoLote: 50, tamanhoProduto: 150, valido: True) [< 1 ms]
2026-11-04T18:56:15.7263040Z   Passed UT-F006-013, UT-F006-014, UT-F006-015 e UT-F006-016 — intervaloDias de 1 a 32767 quando indicado; lote até 50 e produto até 150 caracteres(intervaloDias: 0, tamanhoLote: 50, tamanhoProduto: 150, valido: False) [< 1 ms]
2026-11-04T18:56:15.7273001Z   Passed UT-F006-013, UT-F006-014, UT-F006-015 e UT-F006-016 — intervaloDias de 1 a 32767 quando indicado; lote até 50 e produto até 150 caracteres(intervaloDias: 90, tamanhoLote: 50, tamanhoProduto: 151, valido: False) [< 1 ms]
2026-11-04T18:56:15.7302186Z   Passed I082 — tipo só aceita Vacina ou Desparasitacao(tipo: "Desparasitação", valido: False) [< 1 ms]
2026-11-04T18:56:15.7339108Z   Passed I082 — tipo só aceita Vacina ou Desparasitacao(tipo: "Vacinas", valido: False) [< 1 ms]
2026-11-04T18:56:15.7352126Z   Passed I082 — tipo só aceita Vacina ou Desparasitacao(tipo: "Vacina", valido: True) [< 1 ms]
2026-11-04T18:56:15.7372132Z   Passed I082 — tipo só aceita Vacina ou Desparasitacao(tipo: "vacina", valido: False) [< 1 ms]
2026-11-04T18:56:15.7373268Z   Passed I082 — tipo só aceita Vacina ou Desparasitacao(tipo: "Desparasitacao", valido: True) [< 1 ms]
2026-11-04T18:56:15.7392161Z   Passed UT-F006-020 — BoletimQuery: tipo opcional, só Vacina ou Desparasitacao(tipo: "Vacinas", valido: False) [< 1 ms]
2026-11-04T18:56:15.7393863Z   Passed I089 — vacinação sem intervalo e desparasitação com intervalo: a próxima depende só do intervalo, não do tipo(tipo: "Desparasitacao", intervaloDias: 90, esperada: "2027-01-12") [5 ms]
2026-11-04T18:56:15.7395364Z   Passed UT-F006-020 — BoletimQuery: tipo opcional, só Vacina ou Desparasitacao(tipo: null, valido: True) [3 ms]
2026-11-04T18:56:15.7396597Z   Passed UT-F006-020 — BoletimQuery: tipo opcional, só Vacina ou Desparasitacao(tipo: "Desparasitacao", valido: True) [< 1 ms]
2026-11-04T18:56:15.7397898Z   Passed UT-F006-020 — BoletimQuery: tipo opcional, só Vacina ou Desparasitacao(tipo: "Vacina", valido: True) [< 1 ms]
2026-11-04T18:56:15.7422620Z   Passed UT-F006-010 — tipo, produto, lote e data de administração são obrigatórios(campo: "dataAdministracao") [< 1 ms]
2026-11-04T18:56:15.7452123Z   Passed UT-F006-010 — tipo, produto, lote e data de administração são obrigatórios(campo: "tipo") [< 1 ms]
2026-11-04T18:56:15.7512229Z   Passed UT-F006-010 — tipo, produto, lote e data de administração são obrigatórios(campo: "lote") [< 1 ms]
2026-11-04T18:56:15.7542202Z   Passed UT-F006-010 — tipo, produto, lote e data de administração são obrigatórios(campo: "produto") [< 1 ms]
2026-11-04T18:56:15.7551923Z   Passed I082 — o pedido de exemplo da API (EP-043) é válido [< 1 ms]
2026-11-04T18:56:15.7582281Z   Passed UT-F006-008 — vacina com intervalo de 365 dias: registo do VET_A e próxima a 2027-10-14 [19 ms]
2026-11-04T18:56:15.7611917Z   Passed I082 — produto e lote são guardados sem espaços nos extremos [6 ms]
2026-11-04T18:56:15.7641995Z   Passed I082 — atualizar um registo de outro animal, ou inexistente, devolve 404 [10 ms]
2026-11-04T18:56:15.7662000Z   Passed UT-F003-034 — uma marcação Confirmada também pode ser reagendada [44 ms]
2026-11-04T18:56:15.7691904Z   Passed UT-F003-039 — reagendar para data passada devolve 400 [3 ms]
2026-11-04T18:56:15.7738625Z   Passed UT-F003-043 — sem If-Match a alteração é aceite [5 ms]
2026-11-04T18:56:15.7765225Z   Passed UT-F003-035/036 — Realizada ou Cancelada não podem ser alteradas(estado: "Cancelada") [3 ms]
2026-11-04T18:56:15.7799525Z   Passed UT-F003-035/036 — Realizada ou Cancelada não podem ser alteradas(estado: "Realizada") [2 ms]
2026-11-04T18:56:15.7818594Z   Passed I089 — o Veterinário que não fez o registo recebe 403 e o registo fica como estava [28 ms]
2026-11-04T18:56:15.7841873Z   Passed UT-F003-040 — marcação inexistente devolve 404 [2 ms]
2026-11-04T18:56:15.7842620Z   Passed UT-F003-037 — reagendar para horário ocupado devolve 422 [4 ms]
2026-11-04T18:56:15.7843454Z   Passed UT-F003-033 — altera Veterinário e horário de uma marcação Pendente [8 ms]
2026-11-04T18:56:15.7844442Z   Passed I082 — a atualização substitui produto e lote e mantém o animal e o Veterinário [15 ms]
2026-11-04T18:56:15.8232713Z   Passed UT-F003-038 — a própria marcação não conta como ocupação [5 ms]
2026-11-04T18:56:15.8262168Z   Passed UT-F006-014 — intervaloDias = 1 é aceite e a próxima é 2026-10-15 [7 ms]
2026-11-04T18:56:15.8292323Z   Passed UT-F003-041 — If-Match desatualizado devolve 412 e não altera a marcação [3 ms]
2026-11-04T18:56:15.8322152Z   Passed UT-F003-042 — If-Match com a versão atual permite alterar a marcação [5 ms]
2026-11-04T18:56:15.8352524Z   Passed UT-F003-066 a 073 — As transições não permitidas devolvem 422(estadoAtual: "Realizada", novoEstado: "Confirmada") [< 1 ms]
2026-11-04T18:56:15.8375264Z   Passed UT-F006-012 — data de administração de hoje (2026-10-14) é aceite [8 ms]
2026-11-04T18:56:15.8402311Z   Passed UT-F003-066 a 073 — As transições não permitidas devolvem 422(estadoAtual: "Cancelada", novoEstado: "Realizada") [< 1 ms]
2026-11-04T18:56:15.8452406Z   Passed UT-F003-066 a 073 — As transições não permitidas devolvem 422(estadoAtual: "Pendente", novoEstado: "Realizada") [< 1 ms]
2026-11-04T18:56:15.8482395Z   Passed UT-F003-066 a 073 — As transições não permitidas devolvem 422(estadoAtual: "Cancelada", novoEstado: "Cancelada") [< 1 ms]
2026-11-04T18:56:15.8490858Z   Passed UT-F003-066 a 073 — As transições não permitidas devolvem 422(estadoAtual: "Realizada", novoEstado: "Cancelada") [< 1 ms]
2026-11-04T18:56:15.8492883Z   Passed UT-F003-066 a 073 — As transições não permitidas devolvem 422(estadoAtual: "Confirmada", novoEstado: "Confirmada") [< 1 ms]
2026-11-04T18:56:15.8494351Z   Passed UT-F003-066 a 073 — As transições não permitidas devolvem 422(estadoAtual: "Cancelada", novoEstado: "Confirmada") [< 1 ms]
2026-11-04T18:56:15.8495810Z   Passed UT-F003-066 a 073 — As transições não permitidas devolvem 422(estadoAtual: "Realizada", novoEstado: "Realizada") [< 1 ms]
2026-11-04T18:56:15.8497019Z   Passed UT-F001-062 — "501234567" tem o dígito de controlo errado e é inválido [< 1 ms]
2026-11-04T18:56:15.8498605Z   Passed I067 — Sem valor, o atributo aceita: a obrigatoriedade fica para o [Required](valor: "") [< 1 ms]
2026-11-04T18:56:15.8499994Z   Passed UT-F003-062 a 065 — As quatro transições permitidas são aceites(estadoAtual: "Pendente", novoEstado: "Confirmada") [< 1 ms]
2026-11-04T18:56:15.8501455Z   Passed I067 — Sem valor, o atributo aceita: a obrigatoriedade fica para o [Required](valor: "   ") [< 1 ms]
2026-11-04T18:56:15.8502666Z   Passed I067 — Sem valor, o atributo aceita: a obrigatoriedade fica para o [Required](valor: null) [< 1 ms]
2026-11-04T18:56:15.8503636Z   Passed UT-F001-063 — Um NIF com 8 algarismos é inválido [< 1 ms]
2026-11-04T18:56:15.8504800Z   Passed UT-F003-062 a 065 — As quatro transições permitidas são aceites(estadoAtual: "Confirmada", novoEstado: "Realizada") [< 1 ms]
2026-11-04T18:56:15.8506234Z   Passed UT-F003-062 a 065 — As quatro transições permitidas são aceites(estadoAtual: "Pendente", novoEstado: "Cancelada") [< 1 ms]
2026-11-04T18:56:15.8507986Z   Passed UT-F003-062 a 065 — As quatro transições permitidas são aceites(estadoAtual: "Confirmada", novoEstado: "Cancelada") [< 1 ms]
2026-11-04T18:56:15.8509405Z   Passed I074 — O estado é normalizado sem distinguir maiúsculas ou espaços(recebido: "confirmada", esperado: "Confirmada") [< 1 ms]
2026-11-04T18:56:15.8510770Z   Passed I074 — O estado é normalizado sem distinguir maiúsculas ou espaços(recebido: "cancelada", esperado: "Cancelada") [< 1 ms]
2026-11-04T18:56:15.8518094Z   Passed I074 — O estado é normalizado sem distinguir maiúsculas ou espaços(recebido: " REALIZADA ", esperado: "Realizada") [< 1 ms]
2026-11-04T18:56:15.8519452Z   Passed UT-F003-074 — Pendente nunca pode ser escolhido manualmente(estadoAtual: "Cancelada") [< 1 ms]
2026-11-04T18:56:15.8542255Z   Passed UT-F003-074 — Pendente nunca pode ser escolhido manualmente(estadoAtual: "Confirmada") [< 1 ms]
2026-11-04T18:56:15.8572112Z   Passed UT-F003-074 — Pendente nunca pode ser escolhido manualmente(estadoAtual: "Pendente") [< 1 ms]
2026-11-04T18:56:15.8602066Z   Passed UT-F003-074 — Pendente nunca pode ser escolhido manualmente(estadoAtual: "Realizada") [< 1 ms]
2026-11-04T18:56:15.8603003Z   Passed UT-F003-075 — Um estado inexistente devolve 400 [< 1 ms]
2026-11-04T18:56:15.8632578Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClinicaTests.TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(telefone: "22900000") [1 ms]
2026-11-04T18:56:15.8642663Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClinicaTests.TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(telefone: "22900000a") [< 1 ms]
2026-11-04T18:56:15.8692409Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClinicaTests.TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(telefone: "22+9000000") [< 1 ms]
2026-11-04T18:56:15.8712545Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClinicaTests.TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(telefone: "+12345678901234567890") [< 1 ms]
2026-11-04T18:56:15.8722583Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClinicaTests.TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(telefone: "123456789012345678901") [< 1 ms]
2026-11-04T18:56:15.8752388Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClinicaTests.TelefoneAte20CaracteresEAceite(telefone: "+351 229 000 000") [< 1 ms]
2026-11-04T18:56:15.8782165Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClinicaTests.TelefoneAte20CaracteresEAceite(telefone: "229000000") [< 1 ms]
2026-11-04T18:56:15.8812268Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClinicaTests.TelefoneAte20CaracteresEAceite(telefone: "+1234567890123456789") [< 1 ms]
2026-11-04T18:56:15.8814121Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClinicaTests.TelefoneAte20CaracteresEAceite(telefone: "12345678901234567890") [< 1 ms]
2026-11-04T18:56:15.8842106Z   Passed UT-F001-064 — Um NIF com 10 algarismos é inválido [< 1 ms]
2026-11-04T18:56:15.8864566Z   Passed I067 — Outros NIF com o dígito de controlo correto são válidos(nif: "999999990") [< 1 ms]
2026-11-04T18:56:15.8892537Z   Passed I067 — Outros NIF com o dígito de controlo correto são válidos(nif: "123456789") [< 1 ms]
2026-11-04T18:56:15.8902115Z   Passed I067 — No pedido, os espaços à volta do NIF são ignorados [< 1 ms]
2026-11-04T18:56:15.8932354Z   Passed UT-F001-061 — "234567899" (resto 2, dígito de controlo 9) é um NIF válido [< 1 ms]
2026-11-04T18:56:15.8942960Z   Passed UT-F001-066 — Um NIF com espaços no meio é inválido: não há normalização [< 1 ms]
2026-11-04T18:56:15.8971926Z   Passed UT-F001-065 — Um NIF com caracteres não numéricos é inválido [< 1 ms]
2026-11-04T18:56:15.9002328Z   Passed I064 — Um intervalo de limpeza entre 1 e 1440 minutos é aceite (60 por omissão)(intervalo: "1440", esperado: 1440) [1 ms]
2026-11-04T18:56:15.9022222Z   Passed UT-F001-060 — "501234560" (resto 1, dígito de controlo 0) é um NIF válido [< 1 ms]
2026-11-04T18:56:15.9032232Z   Passed I064 — Um intervalo de limpeza entre 1 e 1440 minutos é aceite (60 por omissão)(intervalo: "1", esperado: 1) [< 1 ms]
2026-11-04T18:56:15.9053638Z   Passed I064 — Um intervalo de limpeza entre 1 e 1440 minutos é aceite (60 por omissão)(intervalo: null, esperado: 60) [< 1 ms]
2026-11-04T18:56:15.9082384Z   Passed I065 — A conta criada inicia sessão em EP-001 com a palavra-passe inicial (critério 6) [217 ms]
2026-11-04T18:56:15.9112215Z   Passed I088 — diagnóstico com exatamente 1000 caracteres é aceite (fronteira de UT-F004-012) [235 ms]
2026-11-04T18:56:15.9122329Z   Passed UT-F003-056 — futuras: só marcações Pendentes ou Confirmadas futuras dos animais do Cliente, por data crescente [99 ms]
2026-11-04T18:56:15.9162067Z   Passed UT-F004-024 — o resumo indica numeroTratamentos = 1 e numeroPrescricoes = 2 [73 ms]
2026-11-04T18:56:15.9172990Z   Passed I078 — HistoricoClinicoQuery: a validação do pedido aceita intervalos válidos e rejeita dataFim anterior a dataInicio(dataInicio: "1999-12-31", dataFim: null, valido: False) [< 1 ms]
2026-11-04T18:56:15.9175286Z   Passed I078 — HistoricoClinicoQuery: a validação do pedido aceita intervalos válidos e rejeita dataFim anterior a dataInicio(dataInicio: "2026-10-14", dataFim: null, valido: True) [< 1 ms]
2026-11-04T18:56:15.9252986Z   Passed I078 — HistoricoClinicoQuery: a validação do pedido aceita intervalos válidos e rejeita dataFim anterior a dataInicio(dataInicio: null, dataFim: "2026-10-14", valido: True) [< 1 ms]
2026-11-04T18:56:15.9255014Z   Passed I078 — HistoricoClinicoQuery: a validação do pedido aceita intervalos válidos e rejeita dataFim anterior a dataInicio(dataInicio: null, dataFim: null, valido: True) [< 1 ms]
2026-11-04T18:56:15.9257014Z   Passed I078 — HistoricoClinicoQuery: a validação do pedido aceita intervalos válidos e rejeita dataFim anterior a dataInicio(dataInicio: "2026-10-14", dataFim: "2026-10-14", valido: True) [< 1 ms]
2026-11-04T18:56:15.9259050Z   Passed I078 — HistoricoClinicoQuery: a validação do pedido aceita intervalos válidos e rejeita dataFim anterior a dataInicio(dataInicio: "2026-10-14", dataFim: "2026-09-01", valido: False) [< 1 ms]
2026-11-04T18:56:15.9260383Z   Passed UT-F004-011 — tratamento com descrição vazia devolve 400 [14 ms]
2026-11-04T18:56:15.9261374Z   Passed UT-F004-012 — diagnóstico com 1001 caracteres devolve 400 (máximo 1000) [5 ms]
2026-11-04T18:56:15.9262355Z   Passed I078 — o histórico não inclui registos de outros animais nem consultas sem registo [15 ms]
2026-11-04T18:56:15.9263232Z   Passed I078 — datas fora de 2000-01-01 a 9998-12-31 devolvem 400 [3 ms]
2026-11-04T18:56:15.9672928Z   Passed I088 (critério 3) — atualização com uma prescrição sem dosagem devolve 400 e a prescrição fica como estava [16 ms]
2026-11-04T18:56:15.9712492Z   Passed UT-F004-008 — prescrição sem medicamento, sem dosagem ou sem frequência devolve 400(campoEmFalta: "medicamento") [5 ms]
2026-11-04T18:56:15.9772470Z   Passed UT-F004-008 — prescrição sem medicamento, sem dosagem ou sem frequência devolve 400(campoEmFalta: "frequência") [3 ms]
2026-11-04T18:56:15.9802308Z   Passed UT-F004-008 — prescrição sem medicamento, sem dosagem ou sem frequência devolve 400(campoEmFalta: "dosagem") [17 ms]
2026-11-04T18:56:15.9820682Z   Passed I076 — Cliente sem marcações recebe listas vazias [61 ms]
2026-11-04T18:56:15.9862174Z   Passed UT-F004-002 — sem notas nem diagnóstico, só com uma prescrição completa, é aceite [13 ms]
2026-11-04T18:56:15.9891953Z   Passed I076 — cada marcação do Cliente aparece numa só das duas vistas [13 ms]
2026-11-04T18:56:15.9922183Z   Passed I076 — MinhasConsultasQuery aceita futuras, historico ou nada(tipo: "futuras") [< 1 ms]
2026-11-04T18:56:15.9952059Z   Passed I076 — MinhasConsultasQuery aceita futuras, historico ou nada(tipo: null) [< 1 ms]
2026-11-04T18:56:15.9953137Z   Passed I076 — MinhasConsultasQuery aceita futuras, historico ou nada(tipo: "historico") [< 1 ms]
2026-11-04T18:56:16.0012207Z   Passed I076 — o tipo não distingue maiúsculas nem espaços à volta(tipo: " Historico ") [5 ms]
2026-11-04T18:56:16.0052129Z   Passed I065 — Pode ser criada uma conta com cada um dos quatro perfis internos(perfil: "Veterinario") [106 ms]
2026-11-04T18:56:16.0081928Z   Passed UT-F004-017 — consulta de outro Veterinário devolve 403 [14 ms]
2026-11-04T18:56:16.0112085Z   Passed I076 — o tipo não distingue maiúsculas nem espaços à volta(tipo: "HISTORICO") [4 ms]
2026-11-04T18:56:16.0142027Z   Passed I078 — EP-033 não altera o registo (só leitura) [63 ms]
2026-11-04T18:56:16.0171976Z   Passed UT-F003-060 — tipo inválido devolve 400(tipo: "todas") [5 ms]
2026-11-04T18:56:16.0202052Z   Passed UT-F003-060 — tipo inválido devolve 400(tipo: "futura") [1 ms]
2026-11-04T18:56:16.0231921Z   Passed UT-F004-015 — consulta Cancelada devolve 422 [10 ms]
2026-11-04T18:56:16.0262102Z   Passed UT-F003-060 — tipo inválido devolve 400(tipo: "passadas") [4 ms]
2026-11-04T18:56:16.0291894Z   Passed UT-F004-027 — animal inexistente (id = 999) devolve 404 [12 ms]
2026-11-04T18:56:16.0322103Z   Passed UT-F003-059 — sem tipo é igual a tipo = futuras [4 ms]
2026-11-04T18:56:16.0369299Z   Passed I077 — os textos são aparados e as notas só com espaços ficam nulas [8 ms]
2026-11-04T18:56:16.0390373Z   Passed UT-F003-057 — fronteira: uma marcação que começa exatamente agora ainda é futura [3 ms]
2026-11-04T18:56:16.0391995Z   Passed UT-F004-023 — sem datas, devolve os dois registos do animal: primeiro o de 2026-10-14, depois o de 2026-09-02 [17 ms]
2026-11-04T18:56:16.0393290Z   Passed UT-F004-026 — dataInicio = 2026-10-14 e dataFim = 2026-09-01 devolve 400 [< 1 ms]
2026-11-04T18:56:16.0394612Z   Passed I088 (critério 3) — atualização com um tratamento sem descrição devolve 400 e o registo fica como estava [17 ms]
2026-11-04T18:56:16.0395659Z   Passed UT-F004-010 — prescrição sem duracaoDias é aceite [8 ms]
2026-11-04T18:56:16.0396420Z   Passed UT-F004-021 — registo inexistente devolve 404 [11 ms]
2026-11-04T18:56:16.0397300Z   Passed I077 — o mesmo id de tratamento repetido no pedido devolve 400 [5 ms]
2026-11-04T18:56:16.0752467Z   Passed UT-F004-005 — consultaId = 0, ou em falta, devolve 400(consultaId: null) [< 1 ms]
2026-11-04T18:56:16.0782110Z   Passed UT-F004-005 — consultaId = 0, ou em falta, devolve 400(consultaId: 0) [2 ms]
2026-11-04T18:56:16.0794363Z   Passed UT-F004-016 — consulta inexistente devolve 404 [5 ms]
2026-11-04T18:56:16.0812109Z   Passed I076 — animalId próprio devolve só as marcações desse animal [74 ms]
2026-11-04T18:56:16.0837272Z   Passed UT-F004-013 — segundo registo para a mesma consulta devolve 409 e nada é gravado [24 ms]
2026-11-04T18:56:16.0862270Z   Passed I076 — MinhasConsultasQuery recusa tipo desconhecido e animalId inferior a 1 no mesmo pedido [< 1 ms]
2026-11-04T18:56:16.0892311Z   Passed UT-F004-025 — dataInicio = dataFim = 2026-10-14 devolve só o registo de 2026-10-14 [64 ms]
2026-11-04T18:56:16.1292403Z   Passed I078 — animal sem registos devolve 200 com a lista vazia (IT-EP034-05) [11 ms]
2026-11-04T18:56:16.1315574Z   Passed UT-F003-058 — historico: marcações passadas e Canceladas, mesmo futuras, por data decrescente [14 ms]
2026-11-04T18:56:16.1357604Z   Passed I076 — uma marcação Realizada com data futura vai para o histórico [6 ms]
2026-11-04T18:56:16.1382485Z   Passed I076 — a resposta só tem dados administrativos: data em UTC, estado, animal e nome do Veterinário [8 ms]
2026-11-04T18:56:16.1391997Z   Passed UT-F003-061 — animalId de um animal de outro Cliente devolve 403 [5 ms]
2026-11-04T18:56:16.1392987Z   Passed I076 — animalId inexistente também devolve 403, sem revelar se o animal existe [2 ms]
2026-11-04T18:56:16.1394079Z   Passed I088 (critério 3) — a atualização grava as notas clínicas e o diagnóstico novos [57 ms]
2026-11-04T18:56:16.1662254Z   Passed UT-F004-007 — notas só com espaços e listas vazias devolvem 400 [8 ms]
2026-11-04T18:56:16.1689774Z   Passed UT-F001-103 — Um período Segunda 09:00–09:00 devolve 400 e o horário mantém-se [45 ms]
2026-11-04T18:56:16.1691414Z   Passed UT-F001-108 — Horas fora do formato HH:mm devolvem 400(abertura: "9h00", fecho: "13:00") [4 ms]
2026-11-04T18:56:16.1722186Z   Passed UT-F001-108 — Horas fora do formato HH:mm devolvem 400(abertura: "09:00", fecho: "25:00") [2 ms]
2026-11-04T18:56:16.1723821Z   Passed UT-F001-111 — Um NIF com o dígito de controlo errado é recusado na validação do pedido (400) [< 1 ms]
2026-11-04T18:56:16.1742384Z   Passed I065 — Pode ser criada uma conta com cada um dos quatro perfis internos(perfil: "Rececionista") [193 ms]
2026-11-04T18:56:16.1743593Z   Passed UT-F004-001 — cria o registo com tratamento e prescrição; o estado da marcação não muda [29 ms]
2026-11-04T18:56:16.2267773Z   Passed I077 — uma prescrição existente mantém-se indicando o id e é atualizada [8 ms]
2026-11-04T18:56:16.2305993Z   Passed I078 — só dataInicio ou só dataFim também filtram [95 ms]
2026-11-04T18:56:16.2307152Z   Passed UT-F004-004 — consulta Realizada sem registo é aceite (Confirmada e Realizada são admitidas) [11 ms]
2026-11-04T18:56:16.2309946Z   Passed I078 — a data é o dia da consulta em hora de Lisboa: 2026-10-13T23:30Z conta como 2026-10-14 (R-FUSO) [11 ms]
2026-11-04T18:56:16.2341971Z   Passed I078 — EP-033 com id inexistente devolve 404 [4 ms]
2026-11-04T18:56:16.2372392Z   Passed UT-F004-022 — devolve o RegistoClinicoDto completo (notas, diagnóstico, tratamentos e prescrições) [5 ms]
2026-11-04T18:56:16.2389469Z   Passed I078 — inclui os registos de todos os Veterinários do animal (RF035) [5 ms]
2026-11-04T18:56:16.2402230Z   Passed UT-F001-102 — Um pedido válido substitui os dados, a duração e o horário e devolve o ClinicaDto [64 ms]
2026-11-04T18:56:16.2607663Z   Passed UT-F001-106 — Um período que acaba às 13:00 e outro que começa às 13:00 não se sobrepõem [2 ms]
2026-11-04T18:56:16.2615948Z   Passed UT-F001-107 — As mesmas horas em dias diferentes (Segunda e Terca) são aceites [6 ms]
2026-11-04T18:56:16.2632351Z   Passed I088 (critério 4) — depois da atualização, o registo, os tratamentos e as prescrições continuam na mesma consulta e no mesmo paciente [52 ms]
2026-11-04T18:56:16.2642523Z   Passed UT-F001-110 — Horário vazio: aceite como clínica fechada (divergência registada; a I028 previa 400) [1 ms]
2026-11-04T18:56:16.2658004Z   Passed I065 — Pode ser criada uma conta com cada um dos quatro perfis internos(perfil: "AuxiliarVeterinario") [89 ms]
2026-11-04T18:56:16.2682342Z   Passed UT-F004-003 — sem notas, diagnóstico nem prescrições, só com um tratamento, é aceite [10 ms]
2026-11-04T18:56:16.2711934Z   Passed UT-F004-014 — consulta Pendente devolve 422 [8 ms]
2026-11-04T18:56:16.2872506Z   Passed UT-F004-020 — atualização com tudo vazio devolve 400 e o registo fica como estava [4 ms]
2026-11-04T18:56:16.2902038Z   Passed UT-F001-101 — Um período com DiaSemana = 7 é devolvido como Domingo [31 ms]
2026-11-04T18:56:16.2942233Z   Passed UT-F001-109 — duracaoConsultaMinutos = 0 é recusada na validação do pedido (400) [< 1 ms]
2026-11-04T18:56:16.2971965Z   Passed I067 — Sem a lista do horário (null), o pedido é recusado (400) [< 1 ms]
2026-11-04T18:56:16.2983742Z   Passed UT-F001-104 — Um período Segunda 14:00–13:00 devolve 400 [1 ms]
2026-11-04T18:56:16.3024763Z   Passed UT-F001-105 — Períodos sobrepostos no mesmo dia (09:00–13:00 e 12:00–15:00) devolvem 400 [1 ms]
2026-11-04T18:56:16.3051799Z   Passed UT-F001-100 — A clínica é devolvida com a duração de 30 minutos e os 11 períodos, com o dia 1 como Segunda [2 ms]
2026-11-04T18:56:16.3082378Z   Passed I072 — a Rececionista e o Auxiliar Veterinário não ficam restringidos a um Veterinário(perfil: "AuxiliarVeterinario") [< 1 ms]
2026-11-04T18:56:16.3122378Z   Passed I072 — a Rececionista e o Auxiliar Veterinário não ficam restringidos a um Veterinário(perfil: "Rececionista") [< 1 ms]
2026-11-04T18:56:16.3152291Z   Passed I072 — o Administrador e o Cliente não têm acesso à agenda (403)(perfil: "Cliente") [1 ms]
2026-11-04T18:56:16.3162276Z   Passed I072 — o Administrador e o Cliente não têm acesso à agenda (403)(perfil: "Administrador") [< 1 ms]
2026-11-04T18:56:16.3163288Z   Passed I072 — um token de Veterinário sem a claim veterinarioId dá 403 [< 1 ms]
2026-11-04T18:56:16.3164289Z   Passed I072 — o Veterinário fica restringido ao identificador da claim veterinarioId [< 1 ms]
2026-11-04T18:56:16.3165638Z   Passed I071 — As rotas são GET /veterinarios e GET /veterinarios/{id}/disponibilidade [< 1 ms]
2026-11-04T18:56:16.3166666Z   Passed I071 — EP-021 e EP-022 exigem a política Rececionista e nenhuma ação a dispensa [< 1 ms]
2026-11-04T18:56:16.3167788Z   Passed UT-F004-018 — mantém T1 atualizado, acrescenta um novo, remove T2 e P1; regista autor e data [29 ms]
2026-11-04T18:56:16.3293653Z   Passed I074 — EP-027 rejeita Pendente e estado inexistente com 400(estado: "Pendente") [18 ms]
2026-11-04T18:56:16.3302105Z   Passed I074 — EP-027 rejeita Pendente e estado inexistente com 400(estado: "Faltou") [5 ms]
2026-11-04T18:56:16.3343067Z   Passed UT-F004-006 — notas e diagnóstico vazios e listas vazias devolvem 400 (nenhum elemento clínico) [10 ms]
2026-11-04T18:56:16.3344001Z   Passed I074 — EP-027 altera Pendente para Confirmada [6 ms]
2026-11-04T18:56:16.3344732Z   Passed UT-F004-009 — prescrição com duracaoDias = 0 devolve 400 [7 ms]
2026-11-04T18:56:16.3345412Z   Passed I074 — EP-027 marcação inexistente devolve 404 [4 ms]
2026-11-04T18:56:16.3346210Z   Passed I074 — EP-027 não altera a marcação quando a transição é inválida [6 ms]
2026-11-04T18:56:16.3347071Z   Passed I077 — registo de outro Veterinário devolve 403 e não é alterado [10 ms]
2026-11-04T18:56:16.3577747Z   Passed UT-F003-044 — mudar estado com If-Match desatualizado devolve 412 [6 ms]
2026-11-04T18:56:16.3594674Z   Passed UT-F004-019 — id de tratamento que pertence a outro registo devolve 400 [7 ms]
2026-11-04T18:56:16.3612530Z   Passed I072 — a Rececionista filtra por Veterinário e por animal; sem estado vêm todos, incluindo as canceladas [132 ms]
2026-11-04T18:56:16.3614066Z   Passed I065 — Pode ser criada uma conta com cada um dos quatro perfis internos(perfil: "Administrador") [91 ms]
2026-11-04T18:56:16.3615211Z   Passed UT-F001-088 — O e-mail de outra conta dá 409 e nada muda [3 ms]
2026-11-04T18:56:16.3616141Z   Passed I064 — Uma falha da limpeza fica no log e não pára a API [568 ms]
2026-11-04T18:56:16.3617150Z   Passed I064 — A limpeza das revogações expiradas corre logo no arranque da API [3 ms]
2026-11-04T18:56:16.4712946Z   Passed I064 — Um intervalo de limpeza fora de 1 a 1440 minutos impede o arranque(intervalo: "1441") [< 1 ms]
2026-11-04T18:56:16.4735446Z   Passed I064 — Um intervalo de limpeza fora de 1 a 1440 minutos impede o arranque(intervalo: "-5") [< 1 ms]
2026-11-04T18:56:16.4738986Z   Passed I064 — Um intervalo de limpeza fora de 1 a 1440 minutos impede o arranque(intervalo: "0") [< 1 ms]
2026-11-04T18:56:16.4740165Z   Passed I064 — Ao parar a API, a rotina de limpeza termina sem erro [2 ms]
2026-11-04T18:56:16.4741544Z   Passed I072 — o estado não distingue maiúsculas nem espaços nos extremos(estado: " CONFIRMADA ") [39 ms]
2026-11-04T18:56:16.4742829Z   Passed I072 — o estado não distingue maiúsculas nem espaços nos extremos(estado: "confirmada") [3 ms]
2026-11-04T18:56:16.4744124Z   Passed UT-F003-051 — a Rececionista filtra por estado e recebe as marcações dos dois Veterinários [32 ms]
2026-11-04T18:56:16.4745460Z   Passed I072 — a agenda é por dia de Lisboa: no dia da mudança de hora conta a meia-noite local [59 ms]
2026-11-04T18:56:16.4746952Z   Passed UT-F003-052 — dataFim anterior a dataInicio devolve 400 [1 ms]
2026-11-04T18:56:16.5393219Z   Passed I072 — estado inválido ou datas fora de 2000-01-01 a 9998-12-31 devolvem 400(estado: null, dataInicio: "1999-12-31", dataFim: null) [1 ms]
2026-11-04T18:56:16.5400171Z   Passed I072 — estado inválido ou datas fora de 2000-01-01 a 9998-12-31 devolvem 400(estado: null, dataInicio: null, dataFim: "9999-12-31") [< 1 ms]
2026-11-04T18:56:16.5402221Z   Passed I072 — estado inválido ou datas fora de 2000-01-01 a 9998-12-31 devolvem 400(estado: "Faturada", dataInicio: null, dataFim: null) [< 1 ms]
2026-11-04T18:56:16.5403838Z   Passed I066 — Depois do bloqueio, o login dá 403 e o token já emitido deixa de ser aceite (critério 4) [182 ms]
2026-11-04T18:56:16.5405097Z   Passed I066 — O Administrador pode mudar o perfil de outro Administrador [2 ms]
2026-11-04T18:56:16.5406864Z   Passed UT-F001-085 — O Administrador não pode remover o próprio perfil de Administrador (422) [1 ms]
2026-11-04T18:56:16.5408341Z   Passed I066 — A autorização de apoio de um Auxiliar pode ser concedida ou retirada(antes: True, depois: False) [5 ms]
2026-11-04T18:56:16.5409818Z   Passed I066 — A autorização de apoio de um Auxiliar pode ser concedida ou retirada(antes: False, depois: True) [1 ms]
2026-11-04T18:56:16.6059134Z   Passed UT-F001-080 — Uma palavra-passe fraca dá 400 com o erro no campo passwordInicial [< 1 ms]
2026-11-04T18:56:16.6077597Z   Passed UT-F001-077 — Um e-mail já registado dá 409 e nada é gravado [1 ms]
2026-11-04T18:56:16.6092532Z   Passed UT-F001-081 — Um nome com 150 caracteres é aceite (limite de Funcionario.Nome) [53 ms]
2026-11-04T18:56:16.6099102Z   Passed I065 — Sem filtros, lista todas as contas de funcionário por nome e nunca as de Cliente [3 ms]
2026-11-04T18:56:16.6100531Z   Passed I072 — só dataInicio: dessa data em diante; só dataFim: até ao fim desse dia [127 ms]
2026-11-04T18:56:16.6102212Z   Passed UT-F001-095 — O Administrador não pode bloquear a própria conta (422) e mantém-se ativo [5 ms]
2026-11-04T18:56:16.6103691Z   Passed I066 — Um Auxiliar com apoio que muda de perfil sem retirar a autorização dá 422 (critério 3) [2 ms]
2026-11-04T18:56:16.6104991Z   Passed I072 — o Auxiliar Veterinário vê a agenda de todos os Veterinários [5 ms]
2026-11-04T18:56:16.6296268Z   Passed UT-F001-076 — Um Rececionista com podeRegistarApoio é recusado com 422 e nada é gravado [1 ms]
2026-11-04T18:56:16.6299910Z   Passed UT-F001-099 — Um perfil fora da enumeração ("Gestor") dá 400 [< 1 ms]
2026-11-04T18:56:16.6310611Z   Passed I066 — O e-mail de uma conta de Cliente também dá 409 [2 ms]
2026-11-04T18:56:16.6312183Z   Passed I072 — EP-024 devolve a versão da marcação como ETag (RowVersion em Base64, entre aspas) [4 ms]
2026-11-04T18:56:16.6313596Z   Passed I072 — cada item tem as datas em UTC e as referências ao animal, ao proprietário e ao Veterinário [5 ms]
2026-11-04T18:56:16.6314990Z   Passed I072 — só as notas da própria marcação, da mais antiga para a mais recente [7 ms]
2026-11-04T18:56:16.6325962Z   Passed UT-F003-053 — o Auxiliar sem autorização de apoio lê a marcação com a nota de preparação [5 ms]
2026-11-04T18:56:16.6327551Z   Passed UT-F001-098 — Filtrar por perfil Veterinario e estado Ativo devolve só os Veterinários ativos [19 ms]
2026-11-04T18:56:16.6804624Z   Passed UT-F001-090 — Um identificador inexistente dá 404 [1 ms]
2026-11-04T18:56:16.6806047Z   Passed I066 — Uma conta de Cliente dá 404 em EP-007 (só contas de funcionário) [2 ms]
2026-11-04T18:56:16.6807336Z   Passed I065 — A consulta devolve a conta de funcionário com a data de criação em UTC [1 ms]
2026-11-04T18:56:16.6808509Z   Passed UT-F001-086 — O Administrador altera os próprios dados mantendo o perfil [1 ms]
2026-11-04T18:56:16.6809598Z   Passed UT-F001-075 — O perfil Cliente é recusado com 422 e nada é gravado [1 ms]
2026-11-04T18:56:16.6810810Z   Passed I072 — paginação: ordem por data e hora e depois por id, com o total de todas as páginas [42 ms]
2026-11-04T18:56:16.6812152Z   Passed UT-F003-055 — o Veterinário lê a marcação que lhe está atribuída [5 ms]
2026-11-04T18:56:16.6813552Z   Passed I072 — marcação inexistente devolve 404, também para um Veterinário [5 ms]
2026-11-04T18:56:16.7546787Z [xUnit.net 00:00:04.28]   Finished:    ClinicaVeterinaria.UnitTests
2026-11-04T18:56:17.4977432Z Data collector 'Blame' message: All tests finished running, Sequence file will not be generated.
2026-11-04T18:56:17.5272375Z   Passed UT-F003-050 — o Veterinário que envia o veterinarioId de outro só recebe as suas marcações [47 ms]
2026-11-04T18:56:17.5273886Z   Passed I072 — uma página depois do fim, mesmo a maior possível, vem vazia e com o total [3 ms]
2026-11-04T18:56:17.5275115Z   Passed UT-F003-054 — o Veterinário que lê a marcação de outro Veterinário recebe 403 [4 ms]
2026-11-04T18:56:17.5276382Z   Passed UT-F001-073 — Criar um Auxiliar com podeRegistarApoio cria a conta ativa e o Funcionario [110 ms]
2026-11-04T18:56:17.5278237Z   Passed UT-F001-083 — Atualizar REC_A para Veterinario muda o perfil e os dados e devolve a conta [6 ms]
2026-11-04T18:56:17.6446582Z Results File: /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/resultados-ci/testes-unitarios/testes-unitarios.trx
2026-11-04T18:56:17.6451702Z 
2026-11-04T18:56:17.6485745Z Test Run Successful.
2026-11-04T18:56:17.6486135Z Total tests: 718
2026-11-04T18:56:17.6486425Z      Passed: 717
2026-11-04T18:56:17.6486740Z     Skipped: 1
2026-11-04T18:56:17.6487412Z  Total time: 6.2246 Seconds
2026-11-04T18:56:17.8040988Z 
2026-11-04T18:56:17.8045264Z Attachments:
2026-11-04T18:56:17.8047739Z   /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/resultados-ci/testes-unitarios/5f9dd718-fead-4a8c-a938-59642148190d/coverage.cobertura.xml
2026-11-04T18:56:17.8490876Z ##[group]Run python3 .github/scripts/resumo-testes.py "$RESULTADOS/testes-unitarios" "2 · Testes unitários (QG-02)"
2026-11-04T18:56:17.8492415Z [36;1mpython3 .github/scripts/resumo-testes.py "$RESULTADOS/testes-unitarios" "2 · Testes unitários (QG-02)"[0m
2026-11-04T18:56:17.8517576Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:56:17.8518129Z env:
2026-11-04T18:56:17.8518418Z   DOTNET_NOLOGO: true
2026-11-04T18:56:17.8518750Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:56:17.8519094Z   CONFIGURACAO: Release
2026-11-04T18:56:17.8519427Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:56:17.8520081Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:56:17.8521061Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:56:17.8521926Z   RESULTADOS: resultados-ci
2026-11-04T18:56:17.8522306Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:56:17.8522661Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:56:17.8523032Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:56:17.8523334Z ##[endgroup]
2026-11-04T18:56:17.8887142Z ### 2 · Testes unitários (QG-02)
2026-11-04T18:56:17.8887528Z 
2026-11-04T18:56:17.8888164Z **✅ Todos os testes passaram** (QG-02: 100% dos testes a passar)
2026-11-04T18:56:17.8888517Z 
2026-11-04T18:56:17.8888876Z | Total | Executados | Aprovados | Falhados | Não executados |
2026-11-04T18:56:17.8889365Z |---:|---:|---:|---:|---:|
2026-11-04T18:56:17.8889685Z | 718 | 717 | 717 | 0 | 0 |
2026-11-04T18:56:17.8889864Z 
2026-11-04T18:56:17.8890047Z Ficheiros TRX: `testes-unitarios.trx`
2026-11-04T18:56:17.8890334Z 
2026-11-04T18:56:17.8890339Z 
2026-11-04T18:56:17.8997139Z ##[group]Run actions/upload-artifact@cf430e030ddbb5b0abf93d22962f4752f3646cd9
2026-11-04T18:56:17.8997664Z with:
2026-11-04T18:56:17.8997948Z   name: resultados-testes-unitarios
2026-11-04T18:56:17.8998326Z   path: resultados-ci/testes-unitarios
2026-11-04T18:56:17.8998679Z   if-no-files-found: warn
2026-11-04T18:56:17.8998981Z   retention-days: 90
2026-11-04T18:56:17.8999263Z   compression-level: 6
2026-11-04T18:56:17.8999574Z   overwrite: false
2026-11-04T18:56:17.8999851Z   include-hidden-files: false
2026-11-04T18:56:17.9000167Z   archive: true
2026-11-04T18:56:17.9000413Z env:
2026-11-04T18:56:17.9000662Z   DOTNET_NOLOGO: true
2026-11-04T18:56:17.9000955Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:56:17.9001502Z   CONFIGURACAO: Release
2026-11-04T18:56:17.9001828Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:56:17.9002495Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:56:17.9003519Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:56:17.9004230Z   RESULTADOS: resultados-ci
2026-11-04T18:56:17.9004549Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:56:17.9004893Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:56:17.9005268Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:56:17.9005579Z ##[endgroup]
2026-11-04T18:56:18.0231305Z With the provided path, there will be 3 files uploaded
2026-11-04T18:56:18.0232803Z Artifact name is valid!
2026-11-04T18:56:18.0233463Z Root directory input is valid!
2026-11-04T18:56:18.4189184Z Uploading artifact: resultados-testes-unitarios.zip
2026-11-04T18:56:18.4219996Z Beginning upload of artifact content to blob storage
2026-11-04T18:56:19.0108516Z Uploaded bytes 305292
2026-11-04T18:56:19.0815807Z Finished uploading artifact content to blob storage!
2026-11-04T18:56:19.0816728Z SHA256 digest of uploaded artifact is 458672001636de4d383cbf613a3005522380646790b14b3ae96a76dd832fbb16
2026-11-04T18:56:19.0817389Z Finalizing artifact upload
2026-11-04T18:56:19.6928038Z Artifact resultados-testes-unitarios successfully finalized. Artifact ID 13354421798
2026-11-04T18:56:19.6929123Z Artifact resultados-testes-unitarios has been successfully uploaded! Final size is 305292 bytes. Artifact ID is 13354421798
2026-11-04T18:56:19.6932866Z Artifact download URL: https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/actions/runs/41891774877/artifacts/13354421798
2026-11-04T18:56:19.7092367Z Post job cleanup.
2026-11-04T18:56:19.7095744Z ##[start-action display=Reutilizar a cache dos pacotes NuGet;id=__65f9c296-155c-481f-8f52-c04960d9748c.__actions_cache]
2026-11-04T18:56:19.8582737Z Post job cleanup.
2026-11-04T18:56:19.9709784Z Cache hit occurred on the primary key nuget-Linux-c8252acdcad9b687c3131c725ee3c1b10105ceb23478247a43e8389c12c19830, not saving cache.
2026-11-04T18:56:19.9780629Z ##[end-action id=__65f9c296-155c-481f-8f52-c04960d9748c.__actions_cache;outcome=success;conclusion=success;duration_ms=268]
2026-11-04T18:56:19.9783334Z ##[start-action display=Instalar o SDK do .NET 10;id=__65f9c296-155c-481f-8f52-c04960d9748c.__actions_setup-dotnet]
2026-11-04T18:56:19.9816195Z Post job cleanup.
2026-11-04T18:56:20.0773451Z ##[end-action id=__65f9c296-155c-481f-8f52-c04960d9748c.__actions_setup-dotnet;outcome=success;conclusion=success;duration_ms=98]
2026-11-04T18:56:20.0922340Z Post job cleanup.
2026-11-04T18:56:20.1658164Z [command]/usr/bin/git version
2026-11-04T18:56:20.1727042Z git version 2.56.0
2026-11-04T18:56:20.1755606Z Temporarily overriding HOME='/home/runner/work/_temp/71b957b6-2fcd-4cc5-9695-a68986c2cd68' before making global git config changes
2026-11-04T18:56:20.1756907Z Adding repository directory to the temporary git global config as a safe directory
2026-11-04T18:56:20.1760442Z [command]/usr/bin/git config --global --add safe.directory /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:56:20.1796655Z Removing SSH command configuration
2026-11-04T18:56:20.1803096Z [command]/usr/bin/git config --local --name-only --get-regexp core\.sshCommand
2026-11-04T18:56:20.1839495Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'core\.sshCommand' && git config --local --unset-all 'core.sshCommand' || :"
2026-11-04T18:56:20.2057136Z Removing HTTP extra header
2026-11-04T18:56:20.2062872Z [command]/usr/bin/git config --local --name-only --get-regexp http\.https\:\/\/github\.com\/\.extraheader
2026-11-04T18:56:20.2096873Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'http\.https\:\/\/github\.com\/\.extraheader' && git config --local --unset-all 'http.https://github.com/.extraheader' || :"
2026-11-04T18:56:20.2312675Z Removing includeIf entries pointing to credentials config files
2026-11-04T18:56:20.2319646Z [command]/usr/bin/git config --local --name-only --get-regexp ^includeIf\.gitdir:
2026-11-04T18:56:20.2358916Z [command]/usr/bin/git submodule foreach --recursive git config --local --show-origin --name-only --get-regexp remote.origin.url
2026-11-04T18:56:20.2686269Z Cleaning up orphan processes
2026-11-04T18:56:20.2959312Z Terminate orphan process: pid (2441) (dotnet)
2026-11-04T18:56:20.2989159Z Terminate orphan process: pid (2442) (dotnet)
2026-11-04T18:56:20.3020800Z Terminate orphan process: pid (2443) (dotnet)
2026-11-04T18:56:20.3040411Z Terminate orphan process: pid (2553) (VBCSCompiler)
``````
