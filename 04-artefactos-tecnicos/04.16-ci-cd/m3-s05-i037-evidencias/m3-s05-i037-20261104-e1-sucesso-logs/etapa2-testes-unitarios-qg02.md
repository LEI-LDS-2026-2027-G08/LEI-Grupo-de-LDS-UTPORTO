``````text
﻿2026-11-04T18:38:18.4282937Z Current runner version: '2.338.2'
2026-11-04T18:38:18.4304563Z ##[group]Runner Image Provisioner
2026-11-04T18:38:18.4305422Z Hosted Compute Agent
2026-11-04T18:38:18.4305939Z Version: 20261028.624
2026-11-04T18:38:18.4306468Z Commit: 54898b74c23fde5556c9b3c3a2290865feef0dfe
2026-11-04T18:38:18.4307368Z Build Date: 2026-10-28T21:51:42Z
2026-11-04T18:38:18.4307974Z Worker ID: {b32e308c-518f-4d2f-88ad-d72093515e60}
2026-11-04T18:38:18.4308561Z Region: westus3
2026-11-04T18:38:18.4309123Z Cloud: Azure
2026-11-04T18:38:18.4309752Z ##[endgroup]
2026-11-04T18:38:18.4311182Z ##[group]Operating System
2026-11-04T18:38:18.4311756Z Ubuntu
2026-11-04T18:38:18.4312188Z 24.04.5
2026-11-04T18:38:18.4312685Z LTS
2026-11-04T18:38:18.4313138Z ##[endgroup]
2026-11-04T18:38:18.4313628Z ##[group]Runner Image
2026-11-04T18:38:18.4314190Z Image: ubuntu-24.04
2026-11-04T18:38:18.4314680Z Version: 20261101.360.1
2026-11-04T18:38:18.4315791Z Included Software: https://github.com/actions/runner-images/blob/ubuntu24/20261101.360/images/ubuntu/Ubuntu2404-Readme.md
2026-11-04T18:38:18.4317069Z Image Release: https://github.com/actions/runner-images/releases/tag/ubuntu24%2F20261101.360
2026-11-04T18:38:18.4317931Z ##[endgroup]
2026-11-04T18:38:18.4318904Z ##[group]GITHUB_TOKEN Permissions
2026-11-04T18:38:18.4320820Z Contents: read
2026-11-04T18:38:18.4321361Z Metadata: read
2026-11-04T18:38:18.4321970Z ##[endgroup]
2026-11-04T18:38:18.4323688Z Secret source: Actions
2026-11-04T18:38:18.4324512Z Cache mode: write
2026-11-04T18:38:18.4325366Z Prepare workflow directory
2026-11-04T18:38:18.5496853Z Prepare all required actions
2026-11-04T18:38:18.5536674Z Getting action download info
2026-11-04T18:38:18.8634732Z Download action repository 'actions/checkout@3d3c42e5aac5ba805825da76410c181273ba90b1' (SHA:3d3c42e5aac5ba805825da76410c181273ba90b1)
2026-11-04T18:38:18.9421489Z Download action repository 'actions/upload-artifact@cf430e030ddbb5b0abf93d22962f4752f3646cd9' (SHA:cf430e030ddbb5b0abf93d22962f4752f3646cd9)
2026-11-04T18:38:19.1621071Z Complete job name: 2 · Testes unitários (QG-02)
2026-11-04T18:38:19.3308508Z ##[group]Run actions/checkout@3d3c42e5aac5ba805825da76410c181273ba90b1
2026-11-04T18:38:19.3309471Z with:
2026-11-04T18:38:19.3309832Z   persist-credentials: false
2026-11-04T18:38:19.3310336Z   repository: LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:38:19.3313484Z   token: ***
2026-11-04T18:38:19.3313839Z   ssh-strict: true
2026-11-04T18:38:19.3314202Z   ssh-user: git
2026-11-04T18:38:19.3314538Z   clean: true
2026-11-04T18:38:19.3314897Z   sparse-checkout-cone-mode: true
2026-11-04T18:38:19.3315312Z   fetch-depth: 1
2026-11-04T18:38:19.3315658Z   fetch-tags: false
2026-11-04T18:38:19.3316016Z   show-progress: true
2026-11-04T18:38:19.3316367Z   lfs: false
2026-11-04T18:38:19.3316699Z   submodules: false
2026-11-04T18:38:19.3317052Z   set-safe-directory: true
2026-11-04T18:38:19.3317454Z   allow-unsafe-pr-checkout: false
2026-11-04T18:38:19.3318023Z env:
2026-11-04T18:38:19.3318341Z   DOTNET_NOLOGO: true
2026-11-04T18:38:19.3318786Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:38:19.3319324Z   CONFIGURACAO: Release
2026-11-04T18:38:19.3319737Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:38:19.3320467Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:38:19.3321509Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:38:19.3322307Z   RESULTADOS: resultados-ci
2026-11-04T18:38:19.3322715Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:38:19.3323145Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:38:19.3323583Z ##[endgroup]
2026-11-04T18:38:19.5376802Z Syncing repository: LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:38:19.5378794Z ##[group]Getting Git version info
2026-11-04T18:38:19.5379978Z Working directory is '/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO'
2026-11-04T18:38:19.5381420Z [command]/usr/bin/git version
2026-11-04T18:38:19.5423802Z git version 2.56.0
2026-11-04T18:38:19.5443569Z ##[endgroup]
2026-11-04T18:38:19.5453504Z Temporarily overriding HOME='/home/runner/work/_temp/6b4e1a27-b117-415b-ba33-86adcd685b26' before making global git config changes
2026-11-04T18:38:19.5455197Z Adding repository directory to the temporary git global config as a safe directory
2026-11-04T18:38:19.5459101Z [command]/usr/bin/git config --global --add safe.directory /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:38:19.5500142Z Deleting the contents of '/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO'
2026-11-04T18:38:19.5502632Z ##[group]Determining repository object format
2026-11-04T18:38:19.5503953Z ##[endgroup]
2026-11-04T18:38:19.5504543Z ##[group]Initializing the repository
2026-11-04T18:38:19.5507807Z [command]/usr/bin/git init /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:38:19.5585172Z hint: Using 'master' as the name for the initial branch. This default branch name
2026-11-04T18:38:19.5586538Z hint: will change to "main" in Git 3.0. To configure the initial branch name
2026-11-04T18:38:19.5587583Z hint: to use in all of your new repositories, which will suppress this warning,
2026-11-04T18:38:19.5588667Z hint: call:
2026-11-04T18:38:19.5589431Z hint:
2026-11-04T18:38:19.5590212Z hint: 	git config --global init.defaultBranch <name>
2026-11-04T18:38:19.5591070Z hint:
2026-11-04T18:38:19.5591760Z hint: Names commonly chosen instead of 'master' are 'main', 'trunk' and
2026-11-04T18:38:19.5592668Z hint: 'development'. The just-created branch can be renamed via this command:
2026-11-04T18:38:19.5593722Z hint:
2026-11-04T18:38:19.5594405Z hint: 	git branch -m <name>
2026-11-04T18:38:19.5594934Z hint:
2026-11-04T18:38:19.5595549Z hint: Disable this message with "git config set advice.defaultBranchName false"
2026-11-04T18:38:19.5596770Z Initialized empty Git repository in /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/
2026-11-04T18:38:19.5601177Z [command]/usr/bin/git remote add origin https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:38:19.5646093Z ##[endgroup]
2026-11-04T18:38:19.5646975Z ##[group]Disabling automatic garbage collection
2026-11-04T18:38:19.5648805Z [command]/usr/bin/git config --local gc.auto 0
2026-11-04T18:38:19.5680800Z ##[endgroup]
2026-11-04T18:38:19.5681957Z ##[group]Setting up auth
2026-11-04T18:38:19.5682595Z Removing SSH command configuration
2026-11-04T18:38:19.5686830Z [command]/usr/bin/git config --local --name-only --get-regexp core\.sshCommand
2026-11-04T18:38:19.5726317Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'core\.sshCommand' && git config --local --unset-all 'core.sshCommand' || :"
2026-11-04T18:38:19.6033391Z Removing HTTP extra header
2026-11-04T18:38:19.6038329Z [command]/usr/bin/git config --local --name-only --get-regexp http\.https\:\/\/github\.com\/\.extraheader
2026-11-04T18:38:19.6078850Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'http\.https\:\/\/github\.com\/\.extraheader' && git config --local --unset-all 'http.https://github.com/.extraheader' || :"
2026-11-04T18:38:19.6292901Z Removing includeIf entries pointing to credentials config files
2026-11-04T18:38:19.6297655Z [command]/usr/bin/git config --local --name-only --get-regexp ^includeIf\.gitdir:
2026-11-04T18:38:19.6334522Z [command]/usr/bin/git submodule foreach --recursive git config --local --show-origin --name-only --get-regexp remote.origin.url
2026-11-04T18:38:19.6539556Z [command]/usr/bin/git config --file /home/runner/work/_temp/git-credentials-5906ea70-61ae-4d8e-b332-28bae6444761.config http.https://github.com/.extraheader AUTHORIZATION: basic ***
2026-11-04T18:38:19.6579351Z [command]/usr/bin/git config --local includeIf.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path /home/runner/work/_temp/git-credentials-5906ea70-61ae-4d8e-b332-28bae6444761.config
2026-11-04T18:38:19.6615128Z [command]/usr/bin/git config --local includeIf.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path /home/runner/work/_temp/git-credentials-5906ea70-61ae-4d8e-b332-28bae6444761.config
2026-11-04T18:38:19.6653142Z [command]/usr/bin/git config --local includeIf.gitdir:/github/workspace/.git.path /github/runner_temp/git-credentials-5906ea70-61ae-4d8e-b332-28bae6444761.config
2026-11-04T18:38:19.6698971Z [command]/usr/bin/git config --local includeIf.gitdir:/github/workspace/.git/worktrees/*.path /github/runner_temp/git-credentials-5906ea70-61ae-4d8e-b332-28bae6444761.config
2026-11-04T18:38:19.6850406Z ##[endgroup]
2026-11-04T18:38:19.6851052Z ##[group]Fetching the repository
2026-11-04T18:38:19.6855354Z [command]/usr/bin/git -c protocol.version=2 fetch --no-tags --prune --no-recurse-submodules --depth=1 origin +89d6b31c5e788b4e8e5cfc7178d991ef65856c06:refs/remotes/origin/develop
2026-11-04T18:38:21.5093444Z From https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:38:21.5094803Z  * [new ref]         89d6b31c5e788b4e8e5cfc7178d991ef65856c06 -> origin/develop
2026-11-04T18:38:21.5100846Z [command]/usr/bin/git branch --list --remote origin/develop
2026-11-04T18:38:21.5132688Z   origin/develop
2026-11-04T18:38:21.5139312Z [command]/usr/bin/git rev-parse refs/remotes/origin/develop
2026-11-04T18:38:21.5169290Z 89d6b31c5e788b4e8e5cfc7178d991ef65856c06
2026-11-04T18:38:21.5177218Z ##[endgroup]
2026-11-04T18:38:21.5177921Z ##[group]Determining the checkout info
2026-11-04T18:38:21.5178642Z ##[endgroup]
2026-11-04T18:38:21.5180753Z [command]/usr/bin/git sparse-checkout disable
2026-11-04T18:38:21.5395207Z [command]/usr/bin/git config --local --unset-all extensions.worktreeConfig
2026-11-04T18:38:21.5433465Z ##[group]Checking out the ref
2026-11-04T18:38:21.5437083Z [command]/usr/bin/git checkout --progress --force -B develop refs/remotes/origin/develop
2026-11-04T18:38:21.7151221Z Switched to a new branch 'develop'
2026-11-04T18:38:21.7153622Z branch 'develop' set up to track 'origin/develop'.
2026-11-04T18:38:21.7176592Z ##[endgroup]
2026-11-04T18:38:21.7226617Z [command]/usr/bin/git log -1 --format=%H
2026-11-04T18:38:21.7258717Z 89d6b31c5e788b4e8e5cfc7178d991ef65856c06
2026-11-04T18:38:21.7270753Z ##[group]Removing auth
2026-11-04T18:38:21.7271536Z Removing SSH command configuration
2026-11-04T18:38:21.7272426Z [command]/usr/bin/git config --local --name-only --get-regexp core\.sshCommand
2026-11-04T18:38:21.7306960Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'core\.sshCommand' && git config --local --unset-all 'core.sshCommand' || :"
2026-11-04T18:38:21.7516457Z Removing HTTP extra header
2026-11-04T18:38:21.7518972Z [command]/usr/bin/git config --local --name-only --get-regexp http\.https\:\/\/github\.com\/\.extraheader
2026-11-04T18:38:21.7555357Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'http\.https\:\/\/github\.com\/\.extraheader' && git config --local --unset-all 'http.https://github.com/.extraheader' || :"
2026-11-04T18:38:21.7757987Z Removing includeIf entries pointing to credentials config files
2026-11-04T18:38:21.7760669Z [command]/usr/bin/git config --local --name-only --get-regexp ^includeIf\.gitdir:
2026-11-04T18:38:21.7796154Z includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path
2026-11-04T18:38:21.7799946Z includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path
2026-11-04T18:38:21.7802275Z includeif.gitdir:/github/workspace/.git.path
2026-11-04T18:38:21.7803896Z includeif.gitdir:/github/workspace/.git/worktrees/*.path
2026-11-04T18:38:21.7807841Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path
2026-11-04T18:38:21.7834835Z /home/runner/work/_temp/git-credentials-5906ea70-61ae-4d8e-b332-28bae6444761.config
2026-11-04T18:38:21.7844150Z [command]/usr/bin/git config --local --unset includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path \/home\/runner\/work\/_temp\/git\-credentials\-5906ea70\-61ae\-4d8e\-b332\-28bae6444761\.config
2026-11-04T18:38:21.7885184Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path
2026-11-04T18:38:21.7913518Z /home/runner/work/_temp/git-credentials-5906ea70-61ae-4d8e-b332-28bae6444761.config
2026-11-04T18:38:21.7921698Z [command]/usr/bin/git config --local --unset includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path \/home\/runner\/work\/_temp\/git\-credentials\-5906ea70\-61ae\-4d8e\-b332\-28bae6444761\.config
2026-11-04T18:38:21.7958151Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/github/workspace/.git.path
2026-11-04T18:38:21.7987756Z /github/runner_temp/git-credentials-5906ea70-61ae-4d8e-b332-28bae6444761.config
2026-11-04T18:38:21.7994510Z [command]/usr/bin/git config --local --unset includeif.gitdir:/github/workspace/.git.path \/github\/runner_temp\/git\-credentials\-5906ea70\-61ae\-4d8e\-b332\-28bae6444761\.config
2026-11-04T18:38:21.8031728Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/github/workspace/.git/worktrees/*.path
2026-11-04T18:38:21.8060202Z /github/runner_temp/git-credentials-5906ea70-61ae-4d8e-b332-28bae6444761.config
2026-11-04T18:38:21.8067751Z [command]/usr/bin/git config --local --unset includeif.gitdir:/github/workspace/.git/worktrees/*.path \/github\/runner_temp\/git\-credentials\-5906ea70\-61ae\-4d8e\-b332\-28bae6444761\.config
2026-11-04T18:38:21.8109438Z [command]/usr/bin/git submodule foreach --recursive git config --local --show-origin --name-only --get-regexp remote.origin.url
2026-11-04T18:38:21.8318280Z Removing credentials config '/home/runner/work/_temp/git-credentials-5906ea70-61ae-4d8e-b332-28bae6444761.config'
2026-11-04T18:38:21.8327016Z ##[endgroup]
2026-11-04T18:38:21.8989422Z Prepare all required actions
2026-11-04T18:38:21.8990995Z Getting action download info
2026-11-04T18:38:22.0529308Z Download action repository 'actions/setup-dotnet@a98b56852c35b8e3190ac28c8c2271da59106c68' (SHA:a98b56852c35b8e3190ac28c8c2271da59106c68)
2026-11-04T18:38:22.6945614Z Download action repository 'actions/cache@55cc8345863c7cc4c66a329aec7e433d2d1c52a9' (SHA:55cc8345863c7cc4c66a329aec7e433d2d1c52a9)
2026-11-04T18:38:22.9735140Z ##[group]Run ./.github/actions/preparar-dotnet
2026-11-04T18:38:22.9735419Z env:
2026-11-04T18:38:22.9735591Z   DOTNET_NOLOGO: true
2026-11-04T18:38:22.9735795Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:38:22.9736012Z   CONFIGURACAO: Release
2026-11-04T18:38:22.9736228Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:38:22.9736649Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:38:22.9737304Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:38:22.9737759Z   RESULTADOS: resultados-ci
2026-11-04T18:38:22.9737986Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:38:22.9738218Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:38:22.9738447Z ##[endgroup]
2026-11-04T18:38:22.9936061Z ##[start-action display=Instalar o SDK do .NET 10;id=__self.__actions_setup-dotnet]
2026-11-04T18:38:23.0840796Z ##[group]Run actions/setup-dotnet@a98b56852c35b8e3190ac28c8c2271da59106c68
2026-11-04T18:38:23.0841134Z with:
2026-11-04T18:38:23.0841318Z   dotnet-version: 10.0.x
2026-11-04T18:38:23.0841534Z   cache: false
2026-11-04T18:38:23.0841713Z env:
2026-11-04T18:38:23.0841885Z   DOTNET_NOLOGO: true
2026-11-04T18:38:23.0842087Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:38:23.0842309Z   CONFIGURACAO: Release
2026-11-04T18:38:23.0842525Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:38:23.0843007Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:38:23.0843848Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:38:23.0844295Z   RESULTADOS: resultados-ci
2026-11-04T18:38:23.0844508Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:38:23.0844756Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:38:23.0844992Z ##[endgroup]
2026-11-04T18:38:23.2019003Z [command]/home/runner/work/_actions/actions/setup-dotnet/a98b56852c35b8e3190ac28c8c2271da59106c68/externals/install-dotnet.sh --skip-non-versioned-files --runtime dotnet --channel LTS
2026-11-04T18:38:23.4442681Z dotnet-install: .NET Core Runtime with version '10.0.14' is already installed.
2026-11-04T18:38:23.4462045Z [command]/home/runner/work/_actions/actions/setup-dotnet/a98b56852c35b8e3190ac28c8c2271da59106c68/externals/install-dotnet.sh --skip-non-versioned-files --channel 10.0
2026-11-04T18:38:23.8103970Z dotnet-install: .NET Core SDK with version '10.0.403' is already installed.
2026-11-04T18:38:23.8216483Z ##[end-action id=__self.__actions_setup-dotnet;outcome=success;conclusion=success;duration_ms=827]
2026-11-04T18:38:23.8285631Z ##[start-action display=Reutilizar a cache dos pacotes NuGet;id=__self.__actions_cache]
2026-11-04T18:38:24.0822826Z ##[group]Run actions/cache@55cc8345863c7cc4c66a329aec7e433d2d1c52a9
2026-11-04T18:38:24.0823218Z with:
2026-11-04T18:38:24.0823550Z   path: ~/.nuget/packages
2026-11-04T18:38:24.0824012Z   key: nuget-Linux-c8252acdcad9b687c3131c725ee3c1b10105ceb23478247a43e8389c12c19830
2026-11-04T18:38:24.0824499Z   restore-keys: nuget-Linux-

2026-11-04T18:38:24.0824875Z   enableCrossOsArchive: false
2026-11-04T18:38:24.0825169Z   fail-on-cache-miss: false
2026-11-04T18:38:24.0825526Z   lookup-only: false
2026-11-04T18:38:24.0825857Z   save-always: false
2026-11-04T18:38:24.0826155Z env:
2026-11-04T18:38:24.0826437Z   DOTNET_NOLOGO: true
2026-11-04T18:38:24.0826793Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:38:24.0827147Z   CONFIGURACAO: Release
2026-11-04T18:38:24.0827787Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:38:24.0828363Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:38:24.0829605Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:38:24.0830326Z   RESULTADOS: resultados-ci
2026-11-04T18:38:24.0830688Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:38:24.0831047Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:38:24.0831368Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:38:24.0831723Z ##[endgroup]
2026-11-04T18:38:24.4004272Z Cache hit for: nuget-Linux-c8252acdcad9b687c3131c725ee3c1b10105ceb23478247a43e8389c12c19830
2026-11-04T18:38:25.6391666Z Received 33554432 of 283633656 (11.8%), 32.0 MBs/sec
2026-11-04T18:38:26.6376229Z Received 192937984 of 283633656 (68.0%), 92.0 MBs/sec
2026-11-04T18:38:27.3728123Z Received 283633656 of 283633656 (100.0%), 98.9 MBs/sec
2026-11-04T18:38:27.3750154Z Cache Size: ~270 MB (283633656 B)
2026-11-04T18:38:27.3775810Z [command]/usr/bin/tar -xf /home/runner/work/_temp/303e3483-1abf-4cb2-a034-a63cf270d968/cache.tzst -P -C /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO --use-compress-program unzstd
2026-11-04T18:38:28.3160187Z Cache restored successfully
2026-11-04T18:38:28.3395539Z Cache restored from key: nuget-Linux-c8252acdcad9b687c3131c725ee3c1b10105ceb23478247a43e8389c12c19830
2026-11-04T18:38:28.3597931Z ##[end-action id=__self.__actions_cache;outcome=success;conclusion=success;duration_ms=4531]
2026-11-04T18:38:28.3647953Z ##[start-action display=Registar o SDK utilizado;id=__self.__run]
2026-11-04T18:38:28.3699330Z ##[group]Run echo "SDK do .NET: $(dotnet --version)"
2026-11-04T18:38:28.3699729Z [36;1mecho "SDK do .NET: $(dotnet --version)"[0m
2026-11-04T18:38:28.3699976Z [36;1m{[0m
2026-11-04T18:38:28.3700144Z [36;1m  echo "### Ambiente .NET"[0m
2026-11-04T18:38:28.3700363Z [36;1m  echo ""[0m
2026-11-04T18:38:28.3700767Z [36;1m  echo "| Elemento | Valor |"[0m
2026-11-04T18:38:28.3700981Z [36;1m  echo "|---|---|"[0m
2026-11-04T18:38:28.3701218Z [36;1m  echo "| SDK do .NET | \`$(dotnet --version)\` |"[0m
2026-11-04T18:38:28.3701562Z [36;1m  echo "| global.json | \`$(tr -d '\n\r ' < src/backend/global.json)\` |"[0m
2026-11-04T18:38:28.3701927Z [36;1m  echo "| Runner | \`${RUNNER_OS} ${ImageOS:-} ${ImageVersion:-}\` |"[0m
2026-11-04T18:38:28.3702213Z [36;1m  echo ""[0m
2026-11-04T18:38:28.3702400Z [36;1m} >> "$GITHUB_STEP_SUMMARY"[0m
2026-11-04T18:38:28.3723862Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:38:28.3724148Z env:
2026-11-04T18:38:28.3724336Z   DOTNET_NOLOGO: true
2026-11-04T18:38:28.3724539Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:38:28.3724761Z   CONFIGURACAO: Release
2026-11-04T18:38:28.3724977Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:38:28.3725377Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:38:28.3725986Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:38:28.3726413Z   RESULTADOS: resultados-ci
2026-11-04T18:38:28.3726619Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:38:28.3726839Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:38:28.3727074Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:38:28.3727268Z ##[endgroup]
2026-11-04T18:38:30.0561682Z SDK do .NET: 10.0.403
2026-11-04T18:38:30.1323831Z ##[end-action id=__self.__run;outcome=success;conclusion=success;duration_ms=1767]
2026-11-04T18:38:30.1398080Z ##[group]Run dotnet restore "$SOLUCAO"
2026-11-04T18:38:30.1398368Z [36;1mdotnet restore "$SOLUCAO"[0m
2026-11-04T18:38:30.1416664Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:38:30.1416950Z env:
2026-11-04T18:38:30.1417120Z   DOTNET_NOLOGO: true
2026-11-04T18:38:30.1417311Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:38:30.1417541Z   CONFIGURACAO: Release
2026-11-04T18:38:30.1417755Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:38:30.1418157Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:38:30.1418733Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:38:30.1419284Z   RESULTADOS: resultados-ci
2026-11-04T18:38:30.1419519Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:38:30.1419749Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:38:30.1419978Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:38:30.1420202Z ##[endgroup]
2026-11-04T18:38:32.9423261Z   Determining projects to restore...
2026-11-04T18:38:34.0541374Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Domain/ClinicaVeterinaria.Domain.csproj (in 183 ms).
2026-11-04T18:38:34.5261896Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Application/ClinicaVeterinaria.Application.csproj (in 693 ms).
2026-11-04T18:38:34.5951380Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Infrastructure/ClinicaVeterinaria.Infrastructure.csproj (in 34 ms).
2026-11-04T18:38:34.6010574Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj (in 756 ms).
2026-11-04T18:38:34.6012494Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj (in 764 ms).
2026-11-04T18:38:34.6050688Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj (in 525 ms).
2026-11-04T18:38:34.6685811Z ##[group]Run dotnet build "$TESTES_UNITARIOS" --configuration "$CONFIGURACAO" --no-restore
2026-11-04T18:38:34.6686511Z [36;1mdotnet build "$TESTES_UNITARIOS" --configuration "$CONFIGURACAO" --no-restore[0m
2026-11-04T18:38:34.6704429Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:38:34.6704716Z env:
2026-11-04T18:38:34.6704879Z   DOTNET_NOLOGO: true
2026-11-04T18:38:34.6705077Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:38:34.6705290Z   CONFIGURACAO: Release
2026-11-04T18:38:34.6705509Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:38:34.6705911Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:38:34.6706525Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:38:34.6706974Z   RESULTADOS: resultados-ci
2026-11-04T18:38:34.6707193Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:38:34.6707462Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:38:34.6707698Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:38:34.6707899Z ##[endgroup]
2026-11-04T18:38:39.9311669Z   ClinicaVeterinaria.Domain -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Domain/bin/Release/net10.0/ClinicaVeterinaria.Domain.dll
2026-11-04T18:38:41.2766208Z   ClinicaVeterinaria.Application -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Application/bin/Release/net10.0/ClinicaVeterinaria.Application.dll
2026-11-04T18:38:43.0580779Z   ClinicaVeterinaria.Infrastructure -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Infrastructure/bin/Release/net10.0/ClinicaVeterinaria.Infrastructure.dll
2026-11-04T18:38:45.6655725Z   ClinicaVeterinaria.Api -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Api/bin/Release/net10.0/ClinicaVeterinaria.Api.dll
2026-11-04T18:38:49.4710476Z   ClinicaVeterinaria.UnitTests -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.UnitTests/bin/Release/net10.0/ClinicaVeterinaria.UnitTests.dll
2026-11-04T18:38:49.4961710Z 
2026-11-04T18:38:49.4968184Z Build succeeded.
2026-11-04T18:38:49.4968993Z     0 Warning(s)
2026-11-04T18:38:49.4970958Z     0 Error(s)
2026-11-04T18:38:49.4971740Z 
2026-11-04T18:38:49.4972151Z Time Elapsed 00:00:14.66
2026-11-04T18:38:49.5395222Z ##[group]Run dotnet test "$TESTES_UNITARIOS" \
2026-11-04T18:38:49.5395574Z [36;1mdotnet test "$TESTES_UNITARIOS" \[0m
2026-11-04T18:38:49.5397262Z [36;1m  --configuration "$CONFIGURACAO" \[0m
2026-11-04T18:38:49.5397625Z [36;1m  --no-build \[0m
2026-11-04T18:38:49.5398719Z [36;1m  --results-directory "$RESULTADOS/testes-unitarios" \[0m
2026-11-04T18:38:49.5399454Z [36;1m  --logger "trx;LogFileName=testes-unitarios.trx" \[0m
2026-11-04T18:38:49.5399743Z [36;1m  --logger "console;verbosity=normal" \[0m
2026-11-04T18:38:49.5401175Z [36;1m  --collect:"XPlat Code Coverage" \[0m
2026-11-04T18:38:49.5401513Z [36;1m  --blame-hang-timeout 5m[0m
2026-11-04T18:38:49.5440520Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:38:49.5440817Z env:
2026-11-04T18:38:49.5441008Z   DOTNET_NOLOGO: true
2026-11-04T18:38:49.5441223Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:38:49.5441462Z   CONFIGURACAO: Release
2026-11-04T18:38:49.5441747Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:38:49.5442183Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:38:49.5442807Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:38:49.5443266Z   RESULTADOS: resultados-ci
2026-11-04T18:38:49.5443487Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:38:49.5443730Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:38:49.5443979Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:38:49.5444189Z ##[endgroup]
2026-11-04T18:38:50.0887604Z Test run for /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.UnitTests/bin/Release/net10.0/ClinicaVeterinaria.UnitTests.dll (.NETCoreApp,Version=v10.0)
2026-11-04T18:38:50.9995809Z A total of 1 test files matched the specified pattern.
2026-11-04T18:38:52.2996461Z [xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v2.5.3.1+6b60a9e56a (64-bit .NET 10.0.14)
2026-11-04T18:38:52.7984797Z [xUnit.net 00:00:00.50]   Discovering: ClinicaVeterinaria.UnitTests
2026-11-04T18:38:52.9115546Z [xUnit.net 00:00:00.62]   Discovered:  ClinicaVeterinaria.UnitTests
2026-11-04T18:38:52.9137746Z [xUnit.net 00:00:00.62]   Starting:    ClinicaVeterinaria.UnitTests
2026-11-04T18:38:53.0780783Z   Passed I065 — Uma palavra-passe forte pode ter espaços [32 ms]
2026-11-04T18:38:53.0807578Z   Passed UT-F001-059 — Sem palavra-passe inicial, o pedido de EP-006 é inválido (obrigatória)(palavraPasse: null) [20 ms]
2026-11-04T18:38:53.0809019Z   Passed UT-F001-059 — Sem palavra-passe inicial, o pedido de EP-006 é inválido (obrigatória)(palavraPasse: "") [< 1 ms]
2026-11-04T18:38:53.0810988Z   Passed UT-F001-055 — Sem letra maiúscula, a palavra-passe é inválida [< 1 ms]
2026-11-04T18:38:53.0811809Z   Passed UT-F001-052 — Uma palavra-passe com 7 caracteres é inválida [< 1 ms]
2026-11-04T18:38:53.0812715Z   Passed UT-F001-051 — Uma palavra-passe com 8 caracteres e as quatro classes é válida [< 1 ms]
2026-11-04T18:38:53.0813717Z   Passed UT-F001-050 — "Temp#2026" (exemplo da API) é uma palavra-passe válida [< 1 ms]
2026-11-04T18:38:53.0814479Z   Passed I065 — O espaço não conta como símbolo [< 1 ms]
2026-11-04T18:38:53.0815189Z   Passed UT-F001-056 — Sem letra minúscula, a palavra-passe é inválida [< 1 ms]
2026-11-04T18:38:53.0815937Z   Passed UT-F001-057 — Sem algarismo, a palavra-passe é inválida [< 1 ms]
2026-11-04T18:38:53.0816690Z   Passed I065 — No atributo, um valor vazio fica para o [Required] [< 1 ms]
2026-11-04T18:38:53.0817569Z   Passed UT-F001-053 — Uma palavra-passe com 128 caracteres e as quatro classes é válida [< 1 ms]
2026-11-04T18:38:53.0818414Z   Passed UT-F001-058 — Sem símbolo, a palavra-passe é inválida [< 1 ms]
2026-11-04T18:38:53.0819120Z   Passed UT-F001-054 — Uma palavra-passe com 129 caracteres é inválida (máximo 128) [< 1 ms]
2026-11-04T18:38:53.0820763Z   Passed UT-F003-066 a 073 — As transições não permitidas devolvem 422(estadoAtual: "Realizada", novoEstado: "Confirmada") [< 1 ms]
2026-11-04T18:38:53.0821888Z   Passed UT-F003-066 a 073 — As transições não permitidas devolvem 422(estadoAtual: "Cancelada", novoEstado: "Realizada") [< 1 ms]
2026-11-04T18:38:53.0823021Z   Passed UT-F003-066 a 073 — As transições não permitidas devolvem 422(estadoAtual: "Pendente", novoEstado: "Realizada") [< 1 ms]
2026-11-04T18:38:53.0824330Z   Passed UT-F003-066 a 073 — As transições não permitidas devolvem 422(estadoAtual: "Cancelada", novoEstado: "Cancelada") [< 1 ms]
2026-11-04T18:38:53.0825354Z   Passed UT-F003-066 a 073 — As transições não permitidas devolvem 422(estadoAtual: "Realizada", novoEstado: "Cancelada") [< 1 ms]
2026-11-04T18:38:53.0826187Z   Passed UT-F003-066 a 073 — As transições não permitidas devolvem 422(estadoAtual: "Confirmada", novoEstado: "Confirmada") [< 1 ms]
2026-11-04T18:38:53.0826962Z   Passed UT-F003-066 a 073 — As transições não permitidas devolvem 422(estadoAtual: "Cancelada", novoEstado: "Confirmada") [< 1 ms]
2026-11-04T18:38:53.0827731Z   Passed UT-F003-066 a 073 — As transições não permitidas devolvem 422(estadoAtual: "Realizada", novoEstado: "Realizada") [< 1 ms]
2026-11-04T18:38:53.0828557Z   Passed UT-F003-062 a 065 — As quatro transições permitidas são aceites(estadoAtual: "Pendente", novoEstado: "Confirmada") [< 1 ms]
2026-11-04T18:38:53.0830373Z   Passed UT-F003-062 a 065 — As quatro transições permitidas são aceites(estadoAtual: "Confirmada", novoEstado: "Realizada") [< 1 ms]
2026-11-04T18:38:53.0881397Z   Passed UT-F003-062 a 065 — As quatro transições permitidas são aceites(estadoAtual: "Pendente", novoEstado: "Cancelada") [< 1 ms]
2026-11-04T18:38:53.0892942Z   Passed UT-F003-062 a 065 — As quatro transições permitidas são aceites(estadoAtual: "Confirmada", novoEstado: "Cancelada") [< 1 ms]
2026-11-04T18:38:53.0894672Z   Passed I074 — O estado é normalizado sem distinguir maiúsculas ou espaços(recebido: "confirmada", esperado: "Confirmada") [1 ms]
2026-11-04T18:38:53.0895954Z   Passed I074 — O estado é normalizado sem distinguir maiúsculas ou espaços(recebido: "cancelada", esperado: "Cancelada") [< 1 ms]
2026-11-04T18:38:53.0897266Z   Passed I074 — O estado é normalizado sem distinguir maiúsculas ou espaços(recebido: " REALIZADA ", esperado: "Realizada") [< 1 ms]
2026-11-04T18:38:53.0898418Z   Passed UT-F003-074 — Pendente nunca pode ser escolhido manualmente(estadoAtual: "Cancelada") [< 1 ms]
2026-11-04T18:38:53.0899591Z   Passed UT-F003-074 — Pendente nunca pode ser escolhido manualmente(estadoAtual: "Confirmada") [< 1 ms]
2026-11-04T18:38:53.0900596Z   Passed UT-F003-074 — Pendente nunca pode ser escolhido manualmente(estadoAtual: "Pendente") [< 1 ms]
2026-11-04T18:38:53.0901321Z   Passed UT-F003-074 — Pendente nunca pode ser escolhido manualmente(estadoAtual: "Realizada") [< 1 ms]
2026-11-04T18:38:53.1756859Z   Passed UT-F003-075 — Um estado inexistente devolve 400 [2 ms]
2026-11-04T18:38:53.1758297Z   Passed I072 — a Rececionista e o Auxiliar Veterinário não ficam restringidos a um Veterinário(perfil: "AuxiliarVeterinario") [16 ms]
2026-11-04T18:38:53.1760033Z   Passed I072 — a Rececionista e o Auxiliar Veterinário não ficam restringidos a um Veterinário(perfil: "Rececionista") [< 1 ms]
2026-11-04T18:38:53.1761269Z   Passed I072 — o Administrador e o Cliente não têm acesso à agenda (403)(perfil: "Cliente") [< 1 ms]
2026-11-04T18:38:53.1762333Z   Passed I072 — o Administrador e o Cliente não têm acesso à agenda (403)(perfil: "Administrador") [< 1 ms]
2026-11-04T18:38:53.1763291Z   Passed I072 — um token de Veterinário sem a claim veterinarioId dá 403 [< 1 ms]
2026-11-04T18:38:53.1764200Z   Passed I072 — o Veterinário fica restringido ao identificador da claim veterinarioId [3 ms]
2026-11-04T18:38:53.1765307Z   Passed I082 — EP-044 usa a política de propriedade do Cliente (ClienteAutenticadoControllerBase) [83 ms]
2026-11-04T18:38:53.1766746Z   Passed I082 — As rotas são /animais/{id}/vacinacoes (GET, POST), /animais/{id}/vacinacoes/{administracaoId} (PUT) e /me/animais/{id}/vacinacoes (GET) [46 ms]
2026-11-04T18:38:53.4214484Z   Passed I082 — EP-044 devolve 200 e usa o Cliente do token, nunca do pedido [202 ms]
2026-11-04T18:38:53.4250419Z   Passed I082 — EP-043 devolve 201 com Location /animais/{id}/vacinacoes/{administracaoId} e usa o Veterinário do token [15 ms]
2026-11-04T18:38:53.4251860Z   Passed I082 — RF056 devolve 200 com a administração e passa o animal, o registo e o Veterinário do token [1 ms]
2026-11-04T18:38:53.4283667Z   Passed I082 — um token de Veterinário sem a claim veterinarioId dá 403 sem chamar o serviço [5 ms]
2026-11-04T18:38:53.4310394Z   Passed I082 — EP-042, EP-043 e RF056 exigem a política Veterinário e nenhuma ação a dispensa [3 ms]
2026-11-04T18:38:53.4317098Z   Passed I082 — EP-044 com um token de Cliente sem a claim clienteId dá 403 sem chamar o serviço [2 ms]
2026-11-04T18:38:53.4351253Z   Passed I082 — EP-042 devolve 200 com o boletim e passa o animal e o filtro ao serviço [3 ms]
2026-11-04T18:38:53.4370200Z   Passed I071 — As rotas são GET /veterinarios e GET /veterinarios/{id}/disponibilidade [< 1 ms]
2026-11-04T18:38:53.4400630Z   Passed I071 — EP-021 e EP-022 exigem a política Rececionista e nenhuma ação a dispensa [1 ms]
2026-11-04T18:38:53.8869469Z   Passed I088 (critério 5) — o resumo do histórico (EP-034) só tem os campos do resumo [< 1 ms]
2026-11-04T18:38:53.8870728Z   Passed I088 (critério 5, PA-08) — a leitura não depende do perfil: o serviço não recebe o utilizador em EP-033 nem em EP-034 [14 ms]
2026-11-04T18:38:53.8871847Z   Passed I088 (critério 5, PA-08) — o registo de EP-033 tem os campos do contrato v02 [< 1 ms]
2026-11-04T18:38:53.8873025Z   Passed I088 (critério 5) — o resumo do histórico (EP-034) não expõe as notas clínicas nem o detalhe dos tratamentos e prescrições [< 1 ms]
2026-11-04T18:38:53.8874752Z   Passed UT-F001-024 — O token de um Veterinário tem veterinarioId e não tem clienteId [63 ms]
2026-11-04T18:38:53.8875870Z   Passed UT-F001-032 — Um token de Cliente com o payload de um Administrador e a assinatura original é rejeitado [335 ms]
2026-11-04T18:38:53.8876998Z   Passed UT-F001-029 — Um token emitido há 8 h 01 min é rejeitado por ter expirado [15 ms]
2026-11-04T18:38:53.8877989Z   Passed UT-F001-033 — Um token sem assinatura (alg = none) é rejeitado: só HS256 é aceite [1 ms]
2026-11-04T18:38:53.8879349Z   Passed UT-F001-028 — Um token emitido agora é aceite pela configuração da API e a claim role é lida com o nome curto [21 ms]
2026-11-04T18:38:53.8977534Z   Passed UT-F001-031 — Um token assinado com outra chave de 256 bits é rejeitado [1 ms]
2026-11-04T18:38:53.9006189Z   Passed UT-F001-025 — O token expira exatamente 8 horas depois, sem frações de segundo, e expiraEm é igual a exp [< 1 ms]
2026-11-04T18:38:53.9010072Z   Passed UT-F001-027 — O cabeçalho do token indica o algoritmo HS256 [< 1 ms]
2026-11-04T18:38:53.9030310Z   Passed UT-F001-023 — O token de um Cliente tem sub, email, role, clienteId e jti, sem veterinarioId [< 1 ms]
2026-11-04T18:38:53.9040187Z   Passed UT-F001-030 — Um token emitido há 7 h 59 min ainda é aceite (sem tolerância de relógio) [4 ms]
2026-11-04T18:38:53.9041309Z   Passed UT-F001-034 — Um token assinado com a chave certa mas com outro emissor é rejeitado [1 ms]
2026-11-04T18:38:53.9042243Z   Passed UT-F001-026 — Dois tokens do mesmo utilizador têm jti diferentes [5 ms]
2026-11-04T18:38:53.9043103Z   Passed UT-F003-016 — Sem data de início o pedido é recusado (obrigatória) [< 1 ms]
2026-11-04T18:38:53.9044142Z   Passed I071 — No pedido, sem dataInicio o [ApiController] responde 400 com a mensagem em português [1 ms]
2026-11-04T18:38:53.9111254Z   Passed I071 — Datas fora de 2000-01-01 a 9998-12-31 são recusadas(ano: 1, mes: 1, dia: 1) [< 1 ms]
2026-11-04T18:38:53.9112619Z   Passed I071 — Datas fora de 2000-01-01 a 9998-12-31 são recusadas(ano: 1999, mes: 12, dia: 31) [< 1 ms]
2026-11-04T18:38:53.9113972Z   Passed I071 — Datas fora de 2000-01-01 a 9998-12-31 são recusadas(ano: 9999, mes: 1, dia: 1) [< 1 ms]
2026-11-04T18:38:53.9115093Z   Passed I071 — Sem data de fim, o período é só o dia de início [1 ms]
2026-11-04T18:38:53.9116371Z   Passed UT-F003-014 — Um período de 32 dias é recusado [< 1 ms]
2026-11-04T18:38:53.9117389Z   Passed I071 — No pedido, um período válido não tem erros [1 ms]
2026-11-04T18:38:53.9118353Z   Passed UT-F003-015 — Uma data de fim anterior à de início é recusada [< 1 ms]
2026-11-04T18:38:53.9119567Z   Passed I071 — No pedido, um período de 32 dias tem erro no campo dataFim [1 ms]
2026-11-04T18:38:53.9120541Z   Passed UT-F003-013 — Um período de 31 dias é aceite [< 1 ms]
2026-11-04T18:38:54.3951151Z   Passed UT-F001-103 — Um período Segunda 09:00–09:00 devolve 400 e o horário mantém-se [1 s]
2026-11-04T18:38:54.3964357Z   Passed UT-F003-048 — segunda chegada devolve 422 e mantém a primeira hora [1 s]
2026-11-04T18:38:54.3965633Z   Passed UT-F001-108 — Horas fora do formato HH:mm devolvem 400(abertura: "9h00", fecho: "13:00") [31 ms]
2026-11-04T18:38:54.3966573Z   Passed UT-F003-049 — If-Match desatualizado devolve 412 e não regista a chegada [9 ms]
2026-11-04T18:38:54.3967616Z   Passed UT-F003-046 e 047 — chegada numa marcação não Confirmada devolve 422(estado: "Pendente") [1 ms]
2026-11-04T18:38:54.3968633Z   Passed UT-F001-108 — Horas fora do formato HH:mm devolvem 400(abertura: "09:00", fecho: "25:00") [4 ms]
2026-11-04T18:38:54.3969892Z   Passed UT-F003-046 e 047 — chegada numa marcação não Confirmada devolve 422(estado: "Cancelada") [1 ms]
2026-11-04T18:38:54.3970675Z   Passed I075 — EP-028 marcação inexistente devolve 404 [1 ms]
2026-11-04T18:38:54.4682641Z   Passed UT-F001-111 — Um NIF com o dígito de controlo errado é recusado na validação do pedido (400) [2 ms]
2026-11-04T18:38:54.4693600Z   Passed I082 — «hoje» é o dia em Lisboa: às 23:30 UTC de 13/10 (00:30 de 14/10 em Lisboa) aceita 2026-10-14 [490 ms]
2026-11-04T18:38:54.4695477Z   Passed UT-F003-045 — chegada numa marcação Confirmada grava a hora do servidor [5 ms]
2026-11-04T18:38:54.4696810Z   Passed UT-F001-102 — Um pedido válido substitui os dados, a duração e o horário e devolve o ClinicaDto [70 ms]
2026-11-04T18:38:54.4698140Z   Passed UT-F001-106 — Um período que acaba às 13:00 e outro que começa às 13:00 não se sobrepõem [3 ms]
2026-11-04T18:38:54.4699533Z   Passed UT-F001-107 — As mesmas horas em dias diferentes (Segunda e Terca) são aceites [6 ms]
2026-11-04T18:38:54.4700829Z   Passed UT-F001-110 — Horário vazio: aceite como clínica fechada (divergência registada; a I028 previa 400) [2 ms]
2026-11-04T18:38:54.5099953Z   Passed UT-F006-021 — próxima prevista para ontem (2026-10-13) não aparece em proximas [82 ms]
2026-11-04T18:38:54.5141027Z   Passed UT-F006-010 — sem tipo, produto, lote ou data de administração devolve 400 com o campo em falta(campo: "tipo", mensagem: "O tipo é obrigatório.") [11 ms]
2026-11-04T18:38:54.5201055Z   Passed UT-F006-010 — sem tipo, produto, lote ou data de administração devolve 400 com o campo em falta(campo: "dataAdministracao", mensagem: "A data de administração é obrigatória.") [2 ms]
2026-11-04T18:38:54.5230890Z   Passed UT-F006-010 — sem tipo, produto, lote ou data de administração devolve 400 com o campo em falta(campo: "produto", mensagem: "O produto é obrigatório.") [5 ms]
2026-11-04T18:38:54.5285824Z   Passed UT-F006-010 — sem tipo, produto, lote ou data de administração devolve 400 com o campo em falta(campo: "lote", mensagem: "O lote é obrigatório.") [2 ms]
2026-11-04T18:38:54.5287578Z   Passed UT-F006-017 — animal inexistente (id = 999) devolve 404 [2 ms]
2026-11-04T18:38:54.5288644Z   Passed UT-F001-101 — Um período com DiaSemana = 7 é devolvido como Domingo [31 ms]
2026-11-04T18:38:54.5290388Z   Passed UT-F001-109 — duracaoConsultaMinutos = 0 é recusada na validação do pedido (400) [1 ms]
2026-11-04T18:38:54.5350009Z   Passed I067 — Sem a lista do horário (null), o pedido é recusado (400) [< 1 ms]
2026-11-04T18:38:54.5410067Z   Passed UT-F001-104 — Um período Segunda 14:00–13:00 devolve 400 [1 ms]
2026-11-04T18:38:54.5467294Z   Passed I082 — próximas por ordem crescente, incluindo a de hoje [9 ms]
2026-11-04T18:38:54.5500202Z   Passed UT-F001-105 — Períodos sobrepostos no mesmo dia (09:00–13:00 e 12:00–15:00) devolvem 400 [3 ms]
2026-11-04T18:38:54.5551474Z   Passed UT-F003-034 — uma marcação Confirmada também pode ser reagendada [1 s]
2026-11-04T18:38:54.5574629Z   Passed UT-F001-100 — A clínica é devolvida com a duração de 30 minutos e os 11 períodos, com o dia 1 como Segunda [5 ms]
2026-11-04T18:38:54.5600033Z   Passed UT-F003-039 — reagendar para data passada devolve 400 [3 ms]
2026-11-04T18:38:54.5630175Z   Passed UT-F001-035 — Sem chave, a validação falha e indica Jwt:Key e os User Secrets [< 1 ms]
2026-11-04T18:38:54.5660132Z   Passed UT-F001-038 — Uma chave que não está em Base64 falha com mensagem sobre Base64 [< 1 ms]
2026-11-04T18:38:54.5692206Z   Passed UT-F001-036 — Uma chave com 31 bytes falha: o mínimo é 32 bytes (256 bits) [< 1 ms]
2026-11-04T18:38:54.5724978Z   Passed UT-F001-037 — Chave com 32 bytes, Issuer, Audience e validade de 8 horas: a configuração é aceite [< 1 ms]
2026-11-04T18:38:54.5790376Z   Passed UT-F001-039 — Uma validade de 0 horas falha [< 1 ms]
2026-11-04T18:38:54.5801166Z   Passed I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido(email: "joao@mail.") [< 1 ms]
2026-11-04T18:38:54.5870775Z   Passed I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido(email: "joao@@mail.pt") [< 1 ms]
2026-11-04T18:38:54.5872088Z   Passed I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido(email: "joao@mail..pt") [< 1 ms]
2026-11-04T18:38:54.5873188Z   Passed I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido(email: "") [< 1 ms]
2026-11-04T18:38:54.5874082Z   Passed I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido(email: "joao@") [3 ms]
2026-11-04T18:38:54.5875123Z   Passed I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido(email: " joao@mail.pt") [< 1 ms]
2026-11-04T18:38:54.5875841Z   Passed I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido(email: "@mail.pt") [< 1 ms]
2026-11-04T18:38:54.5876608Z   Passed I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido(email: "joao@.pt") [< 1 ms]
2026-11-04T18:38:54.5877381Z   Passed I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido(email: "joao@costa@mail.pt") [< 1 ms]
2026-11-04T18:38:54.5878084Z   Passed I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido(email: null) [< 1 ms]
2026-11-04T18:38:54.5878604Z   Passed UT-F001-068 — Um e-mail sem @ é inválido [< 1 ms]
2026-11-04T18:38:54.5879002Z   Passed UT-F001-070 — Um e-mail com espaços é inválido [< 1 ms]
2026-11-04T18:38:54.5879873Z   Passed I065 — No pedido, um valor vazio fica para o [Required](email: null) [< 1 ms]
2026-11-04T18:38:54.6140943Z   Passed I065 — No pedido, um valor vazio fica para o [Required](email: "") [< 1 ms]
2026-11-04T18:38:54.6180272Z   Passed UT-F003-043 — sem If-Match a alteração é aceite [5 ms]
2026-11-04T18:38:54.6269851Z   Passed UT-F001-067 — "joao.costa@mail.pt" é um e-mail válido [< 1 ms]
2026-11-04T18:38:54.6300202Z   Passed UT-F001-071 — Um e-mail válido com 256 caracteres é aceite [< 1 ms]
2026-11-04T18:38:54.6370197Z   Passed UT-F001-069 — Um e-mail sem ponto no domínio é inválido [< 1 ms]
2026-11-04T18:38:54.6400228Z   Passed UT-F001-072 — Um e-mail com 257 caracteres é inválido [< 1 ms]
2026-11-04T18:38:54.6441958Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.HorarioVazioSignificaClinicaFechada [< 1 ms]
2026-11-04T18:38:54.6500484Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.PeriodosSobrepostosNoMesmoDiaSaoRejeitados [< 1 ms]
2026-11-04T18:38:54.6540404Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.MesmasHorasEmDiasDiferentesNaoSeSobrepoem [< 1 ms]
2026-11-04T18:38:54.6547421Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.DiaOuHoraInvalidosSaoRejeitados(dia: "", abertura: "09:00", fecho: "13:00") [< 1 ms]
2026-11-04T18:38:54.6580829Z   Passed UT-F003-035/036 — Realizada ou Cancelada não podem ser alteradas(estado: "Cancelada") [2 ms]
2026-11-04T18:38:54.6598880Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.DiaOuHoraInvalidosSaoRejeitados(dia: "Segunda", abertura: "9h", fecho: "13:00") [3 ms]
2026-11-04T18:38:54.6640647Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.DiaOuHoraInvalidosSaoRejeitados(dia: "Segunda", abertura: "09:00", fecho: "25:00") [< 1 ms]
2026-11-04T18:38:54.6642799Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.DiaOuHoraInvalidosSaoRejeitados(dia: "Feriado", abertura: "09:00", fecho: "13:00") [< 1 ms]
2026-11-04T18:38:54.6670497Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.FechoIgualOuAnteriorAAberturaERejeitado(abertura: "13:00", fecho: "09:00") [< 1 ms]
2026-11-04T18:38:54.6673769Z   Passed UT-F003-035/036 — Realizada ou Cancelada não podem ser alteradas(estado: "Realizada") [2 ms]
2026-11-04T18:38:54.6700511Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.FechoIgualOuAnteriorAAberturaERejeitado(abertura: "13:00", fecho: "13:00") [< 1 ms]
2026-11-04T18:38:54.6728926Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.PeriodosContiguosSaoAceites [< 1 ms]
2026-11-04T18:38:54.6770670Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.VariosPeriodosNoMesmoDiaSaoOrdenadosPorDiaEHora [1 ms]
2026-11-04T18:38:54.6772243Z   Passed UT-F003-040 — marcação inexistente devolve 404 [3 ms]
2026-11-04T18:38:54.6799394Z   Passed UT-F001-115 — Um e-mail sem @ é recusado no pedido (400) [4 ms]
2026-11-04T18:38:54.6822791Z   Passed UT-F001-114 — Um NIF com o dígito de controlo errado é recusado no pedido (400) [< 1 ms]
2026-11-04T18:38:54.6824552Z   Passed UT-F001-116 — Sem telefone, o pedido é recusado (400) [< 1 ms]
2026-11-04T18:38:54.6825577Z   Passed UT-F003-037 — reagendar para horário ocupado devolve 422 [6 ms]
2026-11-04T18:38:54.6826744Z   Passed UT-F003-033 — altera Veterinário e horário de uma marcação Pendente [16 ms]
2026-11-04T18:38:54.6828555Z   Passed I089 — a atualização recalcula a próxima nos casos de calendário: fim de mês, fim de ano e 29 de fevereiro(dataAdministracao: "2025-12-31", intervaloDias: 1, esperada: "2026-01-01") [46 ms]
2026-11-04T18:38:54.6830928Z   Passed I089 — a atualização recalcula a próxima nos casos de calendário: fim de mês, fim de ano e 29 de fevereiro(dataAdministracao: "2026-01-31", intervaloDias: 1, esperada: "2026-02-01") [6 ms]
2026-11-04T18:38:54.6832378Z   Passed UT-F003-038 — a própria marcação não conta como ocupação [11 ms]
2026-11-04T18:38:54.6833996Z   Passed I089 — a atualização recalcula a próxima nos casos de calendário: fim de mês, fim de ano e 29 de fevereiro(dataAdministracao: "2024-02-28", intervaloDias: 1, esperada: "2024-02-29") [4 ms]
2026-11-04T18:38:54.6835651Z   Passed UT-F003-041 — If-Match desatualizado devolve 412 e não altera a marcação [4 ms]
2026-11-04T18:38:54.6837296Z   Passed I089 — a atualização recalcula a próxima nos casos de calendário: fim de mês, fim de ano e 29 de fevereiro(dataAdministracao: "2023-03-01", intervaloDias: 365, esperada: "2024-02-29") [5 ms]
2026-11-04T18:38:54.6838825Z   Passed UT-F003-042 — If-Match com a versão atual permite alterar a marcação [5 ms]
2026-11-04T18:38:54.6839957Z   Passed I082 — o boletim não inclui registos de outros animais [7 ms]
2026-11-04T18:38:54.6841165Z   Passed UT-F006-018 — administrações da mais recente para a mais antiga; próximas só com a vacina de 2027-10-14 [10 ms]
2026-11-04T18:38:54.6842498Z   Passed I082 — atualizar a data de administração recalcula a próxima (2026-09-01 + 365 = 2027-09-01) [16 ms]
2026-11-04T18:38:54.6843670Z   Passed I082 — atualizar um registo feito por outro Veterinário devolve 403 [3 ms]
2026-11-04T18:38:54.6844845Z   Passed UT-F003-032 — veterinarioId inexistente ou de uma conta que não é de Veterinário devolve 404 [34 ms]
2026-11-04T18:38:54.6846071Z   Passed UT-F006-011 — data de administração de amanhã (2026-10-15) devolve 400 [2 ms]
2026-11-04T18:38:54.6847455Z   Passed I065 — Uma conta de Cliente dá 404 (EP-005 só consulta contas de funcionário) [226 ms]
2026-11-04T18:38:54.6848598Z   Passed UT-F003-028 — marcação das 10:15 às 10:45 locais: nova às 10:30 devolve 422 [27 ms]
2026-11-04T18:38:54.6849836Z   Passed UT-F006-019 — tipo filtra as administrações e as próximas(tipo: "Desparasitacao") [35 ms]
2026-11-04T18:38:54.6850975Z   Passed UT-F006-019 — tipo filtra as administrações e as próximas(tipo: "Vacina") [3 ms]
2026-11-04T18:38:54.6852137Z   Passed I073 — a duração vem da configuração: com 45 minutos, 12:30 locais já não cabe (422) [18 ms]
2026-11-04T18:38:54.6853075Z   Passed I082 — EP-042 com animal inexistente devolve 404 [6 ms]
2026-11-04T18:38:54.6853941Z   Passed I082 — tipo fora de Vacina/Desparasitacao devolve 400 [4 ms]
2026-11-04T18:38:54.6854866Z   Passed I082 — animal sem registos: boletim com as duas listas vazias [3 ms]
2026-11-04T18:38:54.6855886Z   Passed I082 — atualizar o intervalo recalcula a próxima, e retirá-lo deixa-a nula [3 ms]
2026-11-04T18:38:54.6857238Z   Passed I073 — motivo com espaços é aparado e motivo vazio fica nulo(motivo: "  Vacinação  ", esperado: "Vacinação") [16 ms]
2026-11-04T18:38:54.7101052Z   Passed UT-F006-013 e UT-F006-015 — intervaloDias = 0 ou negativo devolve 400(intervaloDias: -30) [4 ms]
2026-11-04T18:38:54.7130438Z   Passed UT-F006-013 e UT-F006-015 — intervaloDias = 0 ou negativo devolve 400(intervaloDias: 0) [1 ms]
2026-11-04T18:38:54.7132075Z   Passed I073 — motivo com espaços é aparado e motivo vazio fica nulo(motivo: "   ", esperado: null) [10 ms]
2026-11-04T18:38:54.7160353Z   Passed I082 — a atualização com dados inválidos devolve 400 e o registo fica como estava [4 ms]
2026-11-04T18:38:54.7190568Z   Passed I089 — dar intervalo a uma desparasitação sem próxima (2026-08-01 + 90) cria a próxima de 2026-10-30, que entra nas próximas do boletim [5 ms]
2026-11-04T18:38:54.7220290Z   Passed I073 — motivo com espaços é aparado e motivo vazio fica nulo(motivo: null, esperado: null) [6 ms]
2026-11-04T18:38:54.7232010Z   Passed UT-F003-030 — marcação Cancelada às 10:00 locais não ocupa o horário [11 ms]
2026-11-04T18:38:54.7250338Z   Passed UT-F003-022 — animalId = 0 ou veterinarioId = 0 devolve 400(animalId: 0, veterinarioId: 1) [< 1 ms]
2026-11-04T18:38:54.7336165Z   Passed UT-F001-093 — Um estado diferente de Bloqueado dá 400 e a conta mantém-se ativa(estado: "Suspenso") [87 ms]
2026-11-04T18:38:54.7350289Z   Passed UT-F001-093 — Um estado diferente de Bloqueado dá 400 e a conta mantém-se ativa(estado: "Ativo") [1 ms]
2026-11-04T18:38:54.7379995Z   Passed UT-F001-082 — Um nome com 151 caracteres dá 400 [< 1 ms]
2026-11-04T18:38:54.7410227Z   Passed UT-F003-022 — animalId = 0 ou veterinarioId = 0 devolve 400(animalId: 1, veterinarioId: 0) [5 ms]
2026-11-04T18:38:54.7411596Z   Passed UT-F001-119 — Um novo e-mail atualiza o Cliente e o e-mail da conta Utilizador [177 ms]
2026-11-04T18:38:54.7440278Z   Passed UT-F003-025 — início às 13:00 locais (hora de fecho) devolve 422 [8 ms]
2026-11-04T18:38:54.7469977Z   Passed I082 — EP-044 com animal inexistente devolve 404 [29 ms]
2026-11-04T18:38:54.7590424Z   Passed UT-F001-084 — O perfil Cliente dá 400 (e não 422) e nada muda [18 ms]
2026-11-04T18:38:54.7620564Z   Passed UT-F001-112 — Criar um Cliente cria a conta Utilizador (Cliente, Ativo) e o Cliente ligado a ela; o e-mail fica só na conta [17 ms]
2026-11-04T18:38:54.7622099Z   Passed UT-F001-122 — Um NIF com 5 algarismos é recusado no pedido de atualização (400) [< 1 ms]
2026-11-04T18:38:54.7646932Z   Passed UT-F006-023 — o CLIENTE_A pede o boletim do ANIMAL_B1: 403 [8 ms]
2026-11-04T18:38:54.7665515Z   Passed UT-F001-120 — Atualizar um Cliente com o NIF de outro Cliente devolve 409 [2 ms]
2026-11-04T18:38:54.7666639Z   Passed UT-F001-121 — Manter o próprio NIF não conta como conflito [2 ms]
2026-11-04T18:38:54.7667759Z   Passed I073 — hora de inverno: 09:00Z de 2026-10-26 são 09:00 em Lisboa (aceite) e 08:30Z são 08:30 (422) [17 ms]
2026-11-04T18:38:54.7937943Z   Passed UT-F001-123 — Atualizar um Cliente inexistente devolve 404 [1 ms]
2026-11-04T18:38:54.7939358Z   Passed I065 — Um perfil fora da enumeração dá 400 (e não 422)(perfil: "Gestor") [12 ms]
2026-11-04T18:38:54.7950932Z   Passed I065 — Um perfil fora da enumeração dá 400 (e não 422)(perfil: "Auxiliar") [< 1 ms]
2026-11-04T18:38:54.8010098Z   Passed UT-F006-020 — tipo = "Vacinas" devolve 400 [8 ms]
2026-11-04T18:38:54.8060330Z   Passed I066 — O estado Bloqueado é aceite sem distinguir maiúsculas e sem motivo(estado: " BLOQUEADO ") [8 ms]
2026-11-04T18:38:54.8090597Z   Passed I066 — O estado Bloqueado é aceite sem distinguir maiúsculas e sem motivo(estado: "bloqueado") [1 ms]
2026-11-04T18:38:54.8170361Z   Passed I073 — marcação de outro Veterinário à mesma hora não ocupa o horário [14 ms]
2026-11-04T18:38:54.8200174Z   Passed UT-F006-024 — ANIMAL_A2 sem registos: historico = [] e proximas = [] [11 ms]
2026-11-04T18:38:54.8260151Z   Passed I065 — Um estado fora da enumeração dá 400 [3 ms]
2026-11-04T18:38:54.8262078Z   Passed UT-F006-016 — lote com 51 caracteres devolve 400 (máximo 50) [4 ms]
2026-11-04T18:38:54.8325532Z   Passed I073 — motivo com mais de 250 caracteres devolve 400 [7 ms]
2026-11-04T18:38:54.8370279Z   Passed UT-F003-021 — data igual a RELOGIO devolve 400: tem de ser estritamente futura [7 ms]
2026-11-04T18:38:54.8420284Z   Passed UT-F001-087 — podeRegistarApoio num Veterinário dá 422 e nada muda [16 ms]
2026-11-04T18:38:54.8450250Z   Passed UT-F001-089 — Manter o próprio e-mail (com outras maiúsculas) não é conflito [25 ms]
2026-11-04T18:38:54.8451549Z   Passed UT-F003-024 — início às 12:45 locais (acaba às 13:15) devolve 422 [33 ms]
2026-11-04T18:38:54.8521147Z   Passed UT-F006-022 — o Cliente vê o mesmo histórico e as mesmas próximas do boletim web [50 ms]
2026-11-04T18:38:54.8550205Z   Passed UT-F006-009 — desparasitação sem intervalo: dataProximaPrevista = null [4 ms]
2026-11-04T18:38:54.8610207Z   Passed UT-F001-124 — A pesquisa "Costa" devolve só o CLIENTE_A, com numeroAnimais = 2 [80 ms]
2026-11-04T18:38:54.8640228Z   Passed UT-F003-027 — marcação Confirmada do mesmo Veterinário às 10:00 locais devolve 422 [18 ms]
2026-11-04T18:38:54.8670711Z   Passed I089 — vacinação sem intervalo e desparasitação com intervalo: a próxima depende só do intervalo, não do tipo(tipo: "Vacina", intervaloDias: null, esperada: null) [7 ms]
2026-11-04T18:38:54.8698093Z   Passed UT-F001-092 — Bloquear REC_A ativo passa o estado a Bloqueado [26 ms]
2026-11-04T18:38:54.8810264Z   Passed UT-F001-079 — Um e-mail sem formato válido dá 400 com o erro no campo email [< 1 ms]
2026-11-04T18:38:54.8870112Z   Passed UT-F001-078 — Um nome vazio dá 400 com o erro no campo nome [< 1 ms]
2026-11-04T18:38:54.8959006Z   Passed I065 — Os DTOs de resposta não têm campos de palavra-passe nem de hash (critério 4) [< 1 ms]
2026-11-04T18:38:54.9019669Z   Passed I066 — Um pedido sem estado dá 400 na validação do DTO [< 1 ms]
2026-11-04T18:38:54.9080951Z   Passed I089 — vacinação sem intervalo e desparasitação com intervalo: a próxima depende só do intervalo, não do tipo(tipo: "Desparasitacao", intervaloDias: 90, esperada: "2027-01-12") [5 ms]
2026-11-04T18:38:54.9146431Z   Passed UT-F001-117 — O NIF de outro Cliente devolve 409 e nada é gravado [9 ms]
2026-11-04T18:38:54.9210742Z   Passed UT-F001-118 — Um e-mail já registado devolve 409 e nada é gravado [1 ms]
2026-11-04T18:38:54.9250456Z   Passed UT-F003-029 — marcação das 10:00 às 10:30 locais: nova às 10:30 é aceite (só se tocam) [16 ms]
2026-11-04T18:38:54.9310328Z   Passed UT-F001-113 — Sem NIF, o Cliente é criado (NIF opcional) [7 ms]
2026-11-04T18:38:54.9365141Z   Passed UT-F003-020 — data no passado (RELOGIO − 1 min) devolve 400 [8 ms]
2026-11-04T18:38:54.9420532Z   Passed UT-F006-008 — vacina com intervalo de 365 dias: registo do VET_A e próxima a 2027-10-14 [20 ms]
2026-11-04T18:38:54.9450120Z   Passed I082 — produto e lote são guardados sem espaços nos extremos [5 ms]
2026-11-04T18:38:54.9520595Z   Passed I082 — atualizar um registo de outro animal, ou inexistente, devolve 404 [8 ms]
2026-11-04T18:38:54.9580744Z   Passed I073 — Veterinário com a conta bloqueada devolve 404, como em EP-021 e EP-022 [23 ms]
2026-11-04T18:38:54.9623101Z   Passed I073 — animalId, veterinarioId e dataHora em falta devolvem 400 com as três mensagens [1 ms]
2026-11-04T18:38:54.9642824Z   Passed UT-TRV-005 — Com 57 Clientes, a página 3 de 20 tem 17 elementos e total = 57 [34 ms]
2026-11-04T18:38:54.9690199Z   Passed I073 — marcação mais longa que começou antes ocupa o horário (60 min às 09:45 locais) [6 ms]
2026-11-04T18:38:54.9691449Z   Passed I089 — o Veterinário que não fez o registo recebe 403 e o registo fica como estava [16 ms]
2026-11-04T18:38:54.9740309Z   Passed I082 — a atualização substitui produto e lote e mantém o animal e o Veterinário [6 ms]
2026-11-04T18:38:54.9750641Z   Passed I073 — dataHora sem indicação de fuso é lida como UTC e os segundos fracionários são retirados [11 ms]
2026-11-04T18:38:54.9780196Z   Passed UT-F001-125 — O ClienteDto tem o e-mail da conta, o estado da conta e os dois animais [18 ms]
2026-11-04T18:38:54.9781257Z   Passed UT-F006-014 — intervaloDias = 1 é aceite e a próxima é 2026-10-15 [3 ms]
2026-11-04T18:38:54.9810223Z   Passed I064 — Uma claim exp em falta ou inválida não dá data de expiração(valor: "1791993600.5") [< 1 ms]
2026-11-04T18:38:54.9840149Z   Passed I064 — Uma claim exp em falta ou inválida não dá data de expiração(valor: null) [< 1 ms]
2026-11-04T18:38:54.9841216Z   Passed I064 — Uma claim exp em falta ou inválida não dá data de expiração(valor: "-1") [< 1 ms]
2026-11-04T18:38:54.9842205Z   Passed I064 — Uma claim exp em falta ou inválida não dá data de expiração(valor: "abc") [< 1 ms]
2026-11-04T18:38:54.9843241Z   Passed I064 — Uma claim exp em falta ou inválida não dá data de expiração(valor: "253402300800") [< 1 ms]
2026-11-04T18:38:54.9844603Z   Passed I064 — Uma claim exp em falta ou inválida não dá data de expiração(valor: "") [< 1 ms]
2026-11-04T18:38:54.9845525Z   Passed I064 — A expiração lida do token validado pela API é a do token emitido [1 ms]
2026-11-04T18:38:54.9846435Z   Passed UT-F006-012 — data de administração de hoje (2026-10-14) é aceite [6 ms]
2026-11-04T18:38:54.9847396Z   Passed I072 — NotaPreparacaoDto e as referências ao animal, ao proprietário e ao Veterinário [< 1 ms]
2026-11-04T18:38:54.9848763Z   Passed I072 — nenhum DTO da agenda tem um campo clínico(dto: typeof(ClinicaVeterinaria.Application.DTOs.Comum.AnimalRefDto)) [< 1 ms]
2026-11-04T18:38:54.9850089Z   Passed I064 — A claim exp, em segundos desde 1970, é convertida para UTC [5 ms]
2026-11-04T18:38:54.9851302Z   Passed I072 — nenhum DTO da agenda tem um campo clínico(dto: typeof(ClinicaVeterinaria.Application.DTOs.Comum.ClienteRefDto)) [< 1 ms]
2026-11-04T18:38:54.9852878Z   Passed I072 — nenhum DTO da agenda tem um campo clínico(dto: typeof(ClinicaVeterinaria.Application.DTOs.Comum.VeterinarioRefDto)) [< 1 ms]
2026-11-04T18:38:54.9854498Z   Passed I072 — nenhum DTO da agenda tem um campo clínico(dto: typeof(ClinicaVeterinaria.Application.DTOs.Consultas.ConsultaListItemDto)) [< 1 ms]
2026-11-04T18:38:54.9856409Z   Passed I072 — nenhum DTO da agenda tem um campo clínico(dto: typeof(ClinicaVeterinaria.Application.DTOs.Consultas.ConsultaDto)) [< 1 ms]
2026-11-04T18:38:54.9910506Z   Passed I072 — nenhum DTO da agenda tem um campo clínico(dto: typeof(ClinicaVeterinaria.Application.DTOs.Consultas.NotaPreparacaoDto)) [< 1 ms]
2026-11-04T18:38:54.9940081Z   Passed I072 — ConsultaListItemDto (EP-023): só dados administrativos [1 ms]
2026-11-04T18:38:54.9961888Z   Passed I072 — ConsultaDto (EP-024): só dados administrativos e as notas de preparação [< 1 ms]
2026-11-04T18:38:55.0000378Z   Passed UT-TRV-004 — tamanho = 0 ou pagina = 0 são recusados (400)(pagina: 0, tamanho: 20, campo: "Pagina") [2 ms]
2026-11-04T18:38:55.0030287Z   Passed UT-TRV-004 — tamanho = 0 ou pagina = 0 são recusados (400)(pagina: 1, tamanho: 0, campo: "Tamanho") [< 1 ms]
2026-11-04T18:38:55.0031258Z   Passed UT-TRV-003 — tamanho = 101 é recusado (400) [< 1 ms]
2026-11-04T18:38:55.0050521Z   Passed UT-F003-019 — cria a marcação Pendente, com a duração configurada e a Rececionista como autora [40 ms]
2026-11-04T18:38:55.0084159Z   Passed I071 — EP-021 sem Veterinários devolve uma lista vazia [28 ms]
2026-11-04T18:38:55.0110087Z   Passed UT-F003-023 — domingo (clínica fechada) devolve 422 [8 ms]
2026-11-04T18:38:55.0170096Z   Passed UT-F003-031 — animal inexistente devolve 404 [8 ms]
2026-11-04T18:38:55.0208503Z   Passed UT-F003-026 — início às 12:30 locais (acaba às 13:00) é aceite [10 ms]
2026-11-04T18:38:55.0254039Z   Passed I071 — Um intervalo fora do horário não está disponível(inicio: "2026-10-15T11:45:00Z", duracao: 30) [< 1 ms]
2026-11-04T18:38:55.0290519Z   Passed I071 — Um intervalo fora do horário não está disponível(inicio: "2026-10-18T09:00:00Z", duracao: 30) [< 1 ms]
2026-11-04T18:38:55.0322148Z   Passed I071 — Um intervalo fora do horário não está disponível(inicio: "2026-10-15T07:30:00Z", duracao: 60) [< 1 ms]
2026-11-04T18:38:55.0350456Z   Passed I071 — Um intervalo fora do horário não está disponível(inicio: "2026-10-26T08:30:00Z", duracao: 30) [< 1 ms]
2026-11-04T18:38:55.0581161Z   Passed UT-F003-008 — Com duração de 45 minutos a grelha é de 45 em 45 minutos e só se a consulta couber [6 ms]
2026-11-04T18:38:55.0584600Z   Passed I071 — Uma consulta mais longa do que o período não tem horários [< 1 ms]
2026-11-04T18:38:55.0610925Z   Passed I071 — A hora local de Lisboa é convertida para UTC conforme a hora de verão(data: "2026-10-26", hora: "09:00", esperado: "2026-10-26T09:00:00Z") [< 1 ms]
2026-11-04T18:38:55.0640232Z   Passed UT-F003-013 — Um período de 31 dias é aceite e devolve 31 dias [40 ms]
2026-11-04T18:38:55.0670655Z   Passed I071 — A hora local de Lisboa é convertida para UTC conforme a hora de verão(data: "2026-10-25", hora: "01:30", esperado: "2026-10-25T00:30:00Z") [9 ms]
2026-11-04T18:38:55.0692179Z   Passed I071 — A hora local de Lisboa é convertida para UTC conforme a hora de verão(data: "2026-10-15", hora: "10:00", esperado: "2026-10-15T09:00:00Z") [< 1 ms]
2026-11-04T18:38:55.0700647Z   Passed I071 — O dia da semana segue a base de dados: 1 = segunda … 7 = domingo(data: "2026-10-15", esperado: 4) [< 1 ms]
2026-11-04T18:38:55.0730440Z   Passed I071 — O dia da semana segue a base de dados: 1 = segunda … 7 = domingo(data: "2026-10-12", esperado: 1) [< 1 ms]
2026-11-04T18:38:55.0738131Z   Passed I071 — O dia da semana segue a base de dados: 1 = segunda … 7 = domingo(data: "2026-10-18", esperado: 7) [< 1 ms]
2026-11-04T18:38:55.0770514Z   Passed I071 — O dia da semana segue a base de dados: 1 = segunda … 7 = domingo(data: "2026-10-17", esperado: 6) [< 1 ms]
2026-11-04T18:38:55.0784044Z   Passed UT-F003-012 — Os horários que já começaram não são devolvidos [< 1 ms]
2026-11-04T18:38:55.0790482Z   Passed I071 — Um dia inteiramente no passado não tem horários livres [< 1 ms]
2026-11-04T18:38:55.0791519Z   Passed UT-F003-005 — Uma marcação Cancelada não ocupa horário [< 1 ms]
2026-11-04T18:38:55.0792683Z   Passed I071 — EP-022 devolve os horários livres em HH:mm, com a duração e o Veterinário [12 ms]
2026-11-04T18:38:55.0794453Z   Passed UT-F003-003 — Duas marcações seguidas ocupam os dois horários e libertam os vizinhos [8 ms]
2026-11-04T18:38:55.0795702Z   Passed UT-F003-004 — O último horário de cada período acaba no fecho [< 1 ms]
2026-11-04T18:38:55.0930366Z   Passed UT-F003-011 — Na hora de inverno, 09:00Z é 09:00 em Lisboa [< 1 ms]
2026-11-04T18:38:55.0972361Z   Passed I071 — Um intervalo que se sobrepõe a uma marcação não cancelada não está disponível [< 1 ms]
2026-11-04T18:38:55.0984305Z   Passed UT-F003-009 — Uma marcação de 60 minutos às 10:15 ocupa todos os horários que se sobrepõem [< 1 ms]
2026-11-04T18:38:55.1078182Z   Passed UT-F003-006 — Um domingo sem períodos é devolvido com a lista vazia [< 1 ms]
2026-11-04T18:38:55.1130198Z   Passed I071 — Uma marcação cancelada não impede um intervalo [< 1 ms]
2026-11-04T18:38:55.1190267Z   Passed I071 — Vários dias: um elemento por dia, por ordem, incluindo sábado e domingo [< 1 ms]
2026-11-04T18:38:55.1220612Z   Passed UT-F003-002 — Uma marcação Confirmada às 10:00 locais ocupa só esse horário [< 1 ms]
2026-11-04T18:38:55.1257643Z   Passed I071 — Um intervalo livre e dentro de um período está disponível, mesmo fora da grelha(inicio: "2026-10-15T09:15:00Z", duracao: 30) [9 ms]
2026-11-04T18:38:55.1300425Z   Passed I071 — Um intervalo livre e dentro de um período está disponível, mesmo fora da grelha(inicio: "2026-10-15T11:30:00Z", duracao: 30) [< 1 ms]
2026-11-04T18:38:55.1360476Z   Passed I071 — Um intervalo livre e dentro de um período está disponível, mesmo fora da grelha(inicio: "2026-10-15T08:00:00Z", duracao: 30) [< 1 ms]
2026-11-04T18:38:55.1390375Z   Passed I071 — Um intervalo livre e dentro de um período está disponível, mesmo fora da grelha(inicio: "2026-10-26T09:00:00Z", duracao: 30) [< 1 ms]
2026-11-04T18:38:55.1420262Z   Passed I071 — EP-022 retira os horários das marcações guardadas, mas não os das canceladas [20 ms]
2026-11-04T18:38:55.1456572Z   Passed UT-F003-018 — O id de quem não é Veterinário, ou inexistente, dá 404(conta: "REC_A") [10 ms]
2026-11-04T18:38:55.1497888Z   Passed UT-F003-007 — Entre os dois períodos do dia (13:00–14:00) não há horários [15 ms]
2026-11-04T18:38:55.1500109Z   Passed I071 — Uma duração não positiva ou um período invertido são recusados [< 1 ms]
2026-11-04T18:38:55.1530212Z   Passed UT-F003-001 — Sem marcações, um dia útil tem 18 horários de 30 em 30 minutos [< 1 ms]
2026-11-04T18:38:55.1540561Z   Passed I071 — Um horário que começa no próprio instante atual ainda é devolvido [< 1 ms]
2026-11-04T18:38:55.1570240Z   Passed I071 — Uma hora que não existe (mudança para a hora de verão) não tem UTC [< 1 ms]
2026-11-04T18:38:55.1600281Z   Passed I071 — No dia da mudança para a hora de verão, a hora que não existe não é devolvida [< 1 ms]
2026-11-04T18:38:55.1601675Z   Passed I071 — Uma marcação que acaba quando o horário começa não o ocupa (R-HORARIO) [< 1 ms]
2026-11-04T18:38:55.1611284Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClinicaTests.TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(telefone: "22900000") [< 1 ms]
2026-11-04T18:38:55.1613333Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClinicaTests.TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(telefone: "22900000a") [< 1 ms]
2026-11-04T18:38:55.1615268Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClinicaTests.TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(telefone: "22+9000000") [3 ms]
2026-11-04T18:38:55.1617233Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClinicaTests.TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(telefone: "+12345678901234567890") [< 1 ms]
2026-11-04T18:38:55.1619449Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClinicaTests.TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(telefone: "123456789012345678901") [< 1 ms]
2026-11-04T18:38:55.1621296Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClinicaTests.TelefoneAte20CaracteresEAceite(telefone: "+351 229 000 000") [< 1 ms]
2026-11-04T18:38:55.1623236Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClinicaTests.TelefoneAte20CaracteresEAceite(telefone: "229000000") [< 1 ms]
2026-11-04T18:38:55.1624811Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClinicaTests.TelefoneAte20CaracteresEAceite(telefone: "+1234567890123456789") [< 1 ms]
2026-11-04T18:38:55.1626425Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClinicaTests.TelefoneAte20CaracteresEAceite(telefone: "12345678901234567890") [< 1 ms]
2026-11-04T18:38:55.1627871Z   Passed UT-F001-002 — Login de um Cliente devolve o clienteId e passa-o ao IJwtTokenService [128 ms]
2026-11-04T18:38:55.1628964Z   Passed UT-F003-018 — O id de quem não é Veterinário, ou inexistente, dá 404(conta: "999") [18 ms]
2026-11-04T18:38:55.1630115Z   Passed UT-F001-006 — O e-mail é comparado sem os espaços iniciais e finais [11 ms]
2026-11-04T18:38:55.1631121Z   Passed I071 — EP-022 usa a duração configurada em EP-010 e ignora os períodos inativos [20 ms]
2026-11-04T18:38:55.1632045Z   Passed UT-F001-020 — Conta bloqueada em /auth/me devolve 401 [19 ms]
2026-11-04T18:38:55.1633590Z   Passed UT-F003-014, UT-F003-015 e UT-F003-016 — Período de 32 dias, invertido ou sem data de início dá 400(dataInicio: "2026-10-01", dataFim: "2026-11-01", mensagem: "O período não pode ter mais de 31 dias.") [11 ms]
2026-11-04T18:38:55.1634970Z   Passed UT-F001-010 — Conta bloqueada com a palavra-passe errada devolve 401 e não 403 [11 ms]
2026-11-04T18:38:55.1636564Z   Passed UT-F003-014, UT-F003-015 e UT-F003-016 — Período de 32 dias, invertido ou sem data de início dá 400(dataInicio: "2026-10-15", dataFim: "2026-10-14", mensagem: "A data de fim (dataFim) não pode ser anterior à da"···) [9 ms]
2026-11-04T18:38:55.1638589Z   Passed UT-F003-014, UT-F003-015 e UT-F003-016 — Período de 32 dias, invertido ou sem data de início dá 400(dataInicio: null, dataFim: "2026-10-15", mensagem: "A data de início (dataInicio) é obrigatória, no fo"···) [7 ms]
2026-11-04T18:38:55.1666174Z   Passed I071 — Um Veterinário com a conta bloqueada dá 404 (não está disponível para marcação) [9 ms]
2026-11-04T18:38:55.1667379Z   Passed UT-F001-001 — Login do Administrador devolve o token do IJwtTokenService e regista o último acesso [27 ms]
2026-11-04T18:38:55.1668495Z   Passed I065 — O e-mail é guardado em minúsculas e um repetido com outras maiúsculas dá 409 [282 ms]
2026-11-04T18:38:55.1670198Z   Passed I065 — podeRegistarApoio em qualquer perfil que não seja AuxiliarVeterinario dá 422(perfil: "Veterinario") [1 ms]
2026-11-04T18:38:55.1690181Z   Passed I071 — EP-021 lista só os Veterinários com conta ativa, por nome [15 ms]
2026-11-04T18:38:55.1710618Z   Passed I065 — podeRegistarApoio em qualquer perfil que não seja AuxiliarVeterinario dá 422(perfil: "Administrador") [3 ms]
2026-11-04T18:38:55.1712163Z   Passed UT-F003-010 — Uma marcação de outro Veterinário não ocupa o horário [5 ms]
2026-11-04T18:38:55.1730363Z   Passed UT-F003-017 — Sem dataFim, a disponibilidade tem um único dia [4 ms]
2026-11-04T18:38:55.1731591Z   Passed I071 — EP-021 com Veterinários com o mesmo nome ordena pelo identificador [1 ms]
2026-11-04T18:38:55.1732891Z   Passed UT-F001-013 — Conta ativa, com o perfil do token e jti não revogado: a sessão é válida [24 ms]
2026-11-04T18:38:55.1734228Z   Passed I071 — Um período inválido dá 400 antes de se procurar o Veterinário [< 1 ms]
2026-11-04T18:38:55.1850352Z   Passed UT-F001-014 — Conta bloqueada depois da emissão do token: a sessão deixa de ser válida [11 ms]
2026-11-04T18:38:55.1870248Z   Passed UT-F001-011 — Com SuccessRehashNeeded, o login aceita e grava o novo hash [1 ms]
2026-11-04T18:38:55.1900479Z   Passed UT-F001-012 — O registo da tentativa falhada tem o e-mail e o motivo, mas nunca a palavra-passe [5 ms]
2026-11-04T18:38:55.1930108Z   Passed I065 — Com perfil=Cliente a lista fica vazia (EP-004 só lista funcionários) [34 ms]
2026-11-04T18:38:55.1931416Z   Passed UT-F001-009 — Conta bloqueada com a palavra-passe certa devolve 403 sem emitir token [5 ms]
2026-11-04T18:38:55.1938557Z   Passed UT-F001-008 — Palavra-passe errada devolve 401 com a mesma mensagem, sem token e sem alterar o último acesso [1 ms]
2026-11-04T18:38:55.1946848Z   Passed UT-F001-019 — Cliente autenticado: clienteId e nome do registo Cliente [1 ms]
2026-11-04T18:38:55.2053483Z   Passed UT-F001-003 — E-mail vazio devolve 400 «O e-mail é obrigatório.» sem consultar o hasher nem a base de dados [5 ms]
2026-11-04T18:38:55.2068308Z   Passed UT-F001-018 — Veterinário autenticado: nome do Funcionario, perfil Veterinario e sem clienteId [1 ms]
2026-11-04T18:38:55.2101861Z   Passed UT-F001-017 — Utilizador inexistente: a sessão não é válida [1 ms]
2026-11-04T18:38:55.2110193Z   Passed UT-F001-005 — Um e-mail só com espaços conta como em falta (400) [1 ms]
2026-11-04T18:38:55.2122680Z   Passed UT-F001-004 — Palavra-passe vazia devolve 400 «A palavra-passe é obrigatória.» [1 ms]
2026-11-04T18:38:55.2130321Z   Passed UT-F001-015 — Perfil alterado depois da emissão do token: a sessão deixa de ser válida [1 ms]
2026-11-04T18:38:55.2160442Z   Passed UT-F001-007 — E-mail inexistente devolve 401 «Credenciais inválidas.», verifica um hash de referência e regista o evento 3001 [2 ms]
2026-11-04T18:38:55.2180388Z   Passed I065 — A pesquisa procura no nome e no e-mail e nunca devolve Clientes(pesquisa: "Vet", esperados: "Vasco Vet|Vera Vet|Vitor Vet") [59 ms]
2026-11-04T18:38:55.2471584Z   Passed I065 — A pesquisa procura no nome e no e-mail e nunca devolve Clientes(pesquisa: "carla.aux@", esperados: "Carla Aux") [5 ms]
2026-11-04T18:38:55.2480785Z   Passed I065 — A pesquisa procura no nome e no e-mail e nunca devolve Clientes(pesquisa: "costa", esperados: "") [3 ms]
2026-11-04T18:38:55.2510475Z   Passed UT-F001-091 — Um e-mail sem formato válido dá 400 com o erro no campo email [< 1 ms]
2026-11-04T18:38:55.2512556Z   Passed I066 — Um perfil fora da enumeração dá 400 na atualização(perfil: "Auxiliar") [3 ms]
2026-11-04T18:38:55.2560209Z   Passed I066 — Um perfil fora da enumeração dá 400 na atualização(perfil: "Gestor") [1 ms]
2026-11-04T18:38:55.2590390Z   Passed I088 — diagnóstico com exatamente 1000 caracteres é aceite (fronteira de UT-F004-012) [202 ms]
2026-11-04T18:38:55.2610407Z   Passed I072 — a Rececionista filtra por Veterinário e por animal; sem estado vêm todos, incluindo as canceladas [129 ms]
2026-11-04T18:38:55.2670355Z   Passed UT-F004-011 — tratamento com descrição vazia devolve 400 [27 ms]
2026-11-04T18:38:55.2680169Z   Passed UT-F004-012 — diagnóstico com 1001 caracteres devolve 400 (máximo 1000) [5 ms]
2026-11-04T18:38:55.2710498Z   Passed I065 — Contas com o mesmo nome ficam ordenadas pelo identificador (ordem estável) [41 ms]
2026-11-04T18:38:55.2740007Z   Passed I065 — Um identificador inexistente dá 404 [1 ms]
2026-11-04T18:38:55.2770528Z   Passed I065 — A paginação devolve a página pedida; depois do fim, ou numa página muito alta, a lista fica vazia(pagina: 2147483647, tamanho: 100, esperados: "") [3 ms]
2026-11-04T18:38:55.2772117Z   Passed I074 — PreconditionFailedException é convertida em HTTP 412 [102 ms]
2026-11-04T18:38:55.2800676Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClienteTests.TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(telefone: "123456789012345678901") [< 1 ms]
2026-11-04T18:38:55.2830780Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClienteTests.TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(telefone: "+12345678901234567890") [< 1 ms]
2026-11-04T18:38:55.2840677Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClienteTests.TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(telefone: "12345678") [< 1 ms]
2026-11-04T18:38:55.2860681Z   Passed I065 — A paginação devolve a página pedida; depois do fim, ou numa página muito alta, a lista fica vazia(pagina: 3, tamanho: 3, esperados: "Vera Vet|Vitor Vet") [3 ms]
2026-11-04T18:38:55.2890537Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClienteTests.TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(telefone: "91234567a") [< 1 ms]
2026-11-04T18:38:55.2920537Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClienteTests.TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(telefone: "91+2345678") [< 1 ms]
2026-11-04T18:38:55.2950879Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClienteTests.TelefoneAte20CaracteresEAceite(telefone: "+1234567890123456789") [< 1 ms]
2026-11-04T18:38:55.2952767Z   Passed I065 — A paginação devolve a página pedida; depois do fim, ou numa página muito alta, a lista fica vazia(pagina: 4, tamanho: 3, esperados: "") [2 ms]
2026-11-04T18:38:55.2980495Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClienteTests.TelefoneAte20CaracteresEAceite(telefone: "912345678") [< 1 ms]
2026-11-04T18:38:55.3010410Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClienteTests.TelefoneAte20CaracteresEAceite(telefone: "+351 912 345 678") [< 1 ms]
2026-11-04T18:38:55.3040538Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClienteTests.TelefoneAte20CaracteresEAceite(telefone: "12345678901234567890") [< 1 ms]
2026-11-04T18:38:55.3042957Z   Passed I065 — A paginação devolve a página pedida; depois do fim, ou numa página muito alta, a lista fica vazia(pagina: 2, tamanho: 3, esperados: "Rita Rececao|Rui Bloqueado|Vasco Vet") [3 ms]
2026-11-04T18:38:55.3070565Z   Passed UT-F001-041 — A política Rececionista só aceita o perfil Rececionista(perfil: "Veterinario", aceite: False) [14 ms]
2026-11-04T18:38:55.3100400Z   Passed I088 (critério 3) — atualização com uma prescrição sem dosagem devolve 400 e a prescrição fica como estava [34 ms]
2026-11-04T18:38:55.3130340Z   Passed UT-F001-041 — A política Rececionista só aceita o perfil Rececionista(perfil: "Rececionista", aceite: True) [7 ms]
2026-11-04T18:38:55.3131762Z   Passed UT-F001-041 — A política Rececionista só aceita o perfil Rececionista(perfil: "AuxiliarVeterinario", aceite: False) [< 1 ms]
2026-11-04T18:38:55.3160553Z   Passed UT-F001-041 — A política Rececionista só aceita o perfil Rececionista(perfil: "Cliente", aceite: False) [< 1 ms]
2026-11-04T18:38:55.3190431Z   Passed UT-F001-041 — A política Rececionista só aceita o perfil Rececionista(perfil: "Administrador", aceite: False) [3 ms]
2026-11-04T18:38:55.3220610Z   Passed UT-F001-040 — A política Administrador só aceita o perfil Administrador(perfil: "AuxiliarVeterinario", aceite: False) [< 1 ms]
2026-11-04T18:38:55.3229869Z   Passed UT-F001-040 — A política Administrador só aceita o perfil Administrador(perfil: "Cliente", aceite: False) [< 1 ms]
2026-11-04T18:38:55.3260475Z   Passed UT-F001-040 — A política Administrador só aceita o perfil Administrador(perfil: "Administrador", aceite: True) [< 1 ms]
2026-11-04T18:38:55.3290368Z   Passed UT-F001-040 — A política Administrador só aceita o perfil Administrador(perfil: "Veterinario", aceite: False) [< 1 ms]
2026-11-04T18:38:55.3307627Z   Passed UT-F001-040 — A política Administrador só aceita o perfil Administrador(perfil: "Rececionista", aceite: False) [< 1 ms]
2026-11-04T18:38:55.3320531Z   Passed UT-F004-008 — prescrição sem medicamento, sem dosagem ou sem frequência devolve 400(campoEmFalta: "medicamento") [7 ms]
2026-11-04T18:38:55.3351077Z   Passed UT-F001-045 — A política Funcionario aceita os quatro perfis internos e recusa o Cliente(perfil: "AuxiliarVeterinario", aceite: True) [< 1 ms]
2026-11-04T18:38:55.3380801Z   Passed UT-F001-045 — A política Funcionario aceita os quatro perfis internos e recusa o Cliente(perfil: "Administrador", aceite: True) [3 ms]
2026-11-04T18:38:55.3400576Z   Passed UT-F001-045 — A política Funcionario aceita os quatro perfis internos e recusa o Cliente(perfil: "Veterinario", aceite: True) [< 1 ms]
2026-11-04T18:38:55.3420561Z   Passed UT-F001-045 — A política Funcionario aceita os quatro perfis internos e recusa o Cliente(perfil: "Rececionista", aceite: True) [< 1 ms]
2026-11-04T18:38:55.3440438Z   Passed UT-F004-008 — prescrição sem medicamento, sem dosagem ou sem frequência devolve 400(campoEmFalta: "frequência") [4 ms]
2026-11-04T18:38:55.3470765Z   Passed UT-F001-045 — A política Funcionario aceita os quatro perfis internos e recusa o Cliente(perfil: "Cliente", aceite: False) [< 1 ms]
2026-11-04T18:38:55.3500979Z   Passed UT-F001-044 — A política Cliente só aceita o perfil Cliente(perfil: "AuxiliarVeterinario", aceite: False) [< 1 ms]
2026-11-04T18:38:55.3510282Z   Passed UT-F001-044 — A política Cliente só aceita o perfil Cliente(perfil: "Rececionista", aceite: False) [< 1 ms]
2026-11-04T18:38:55.3531069Z   Passed UT-F001-044 — A política Cliente só aceita o perfil Cliente(perfil: "Cliente", aceite: True) [3 ms]
2026-11-04T18:38:55.3560575Z   Passed UT-F001-044 — A política Cliente só aceita o perfil Cliente(perfil: "Veterinario", aceite: False) [< 1 ms]
2026-11-04T18:38:55.3590505Z   Passed UT-F001-044 — A política Cliente só aceita o perfil Cliente(perfil: "Administrador", aceite: False) [< 1 ms]
2026-11-04T18:38:55.3596028Z   Passed UT-F001-048 — A política de propriedade recusa os perfis internos mesmo com clienteId(perfil: "Rececionista") [1 ms]
2026-11-04T18:38:55.3620576Z   Passed UT-F004-008 — prescrição sem medicamento, sem dosagem ou sem frequência devolve 400(campoEmFalta: "dosagem") [8 ms]
2026-11-04T18:38:55.3650602Z   Passed UT-F001-048 — A política de propriedade recusa os perfis internos mesmo com clienteId(perfil: "AuxiliarVeterinario") [3 ms]
2026-11-04T18:38:55.3670615Z   Passed UT-F001-048 — A política de propriedade recusa os perfis internos mesmo com clienteId(perfil: "Veterinario") [< 1 ms]
2026-11-04T18:38:55.3690372Z   Passed UT-F001-048 — A política de propriedade recusa os perfis internos mesmo com clienteId(perfil: "Administrador") [< 1 ms]
2026-11-04T18:38:55.3710487Z   Passed UT-F001-046 — A política de propriedade aceita um Cliente com a claim clienteId [< 1 ms]
2026-11-04T18:38:55.3740409Z   Passed UT-F001-042 — A política Veterinario só aceita o perfil Veterinario(perfil: "Cliente", aceite: False) [< 1 ms]
2026-11-04T18:38:55.3762899Z   Passed UT-F001-042 — A política Veterinario só aceita o perfil Veterinario(perfil: "Administrador", aceite: False) [4 ms]
2026-11-04T18:38:55.3771006Z   Passed UT-F001-042 — A política Veterinario só aceita o perfil Veterinario(perfil: "Rececionista", aceite: False) [< 1 ms]
2026-11-04T18:38:55.3781112Z   Passed UT-F001-042 — A política Veterinario só aceita o perfil Veterinario(perfil: "AuxiliarVeterinario", aceite: False) [< 1 ms]
2026-11-04T18:38:55.3782771Z   Passed UT-F001-042 — A política Veterinario só aceita o perfil Veterinario(perfil: "Veterinario", aceite: True) [< 1 ms]
2026-11-04T18:38:55.3784417Z   Passed UT-F001-043 — A política AuxiliarVeterinario só aceita o perfil AuxiliarVeterinario(perfil: "Cliente", aceite: False) [< 1 ms]
2026-11-04T18:38:55.3785841Z   Passed UT-F004-002 — sem notas nem diagnóstico, só com uma prescrição completa, é aceite [11 ms]
2026-11-04T18:38:55.3787549Z   Passed UT-F001-043 — A política AuxiliarVeterinario só aceita o perfil AuxiliarVeterinario(perfil: "Rececionista", aceite: False) [3 ms]
2026-11-04T18:38:55.3789483Z   Passed UT-F001-043 — A política AuxiliarVeterinario só aceita o perfil AuxiliarVeterinario(perfil: "Veterinario", aceite: False) [< 1 ms]
2026-11-04T18:38:55.3791222Z   Passed UT-F001-043 — A política AuxiliarVeterinario só aceita o perfil AuxiliarVeterinario(perfil: "Administrador", aceite: False) [< 1 ms]
2026-11-04T18:38:55.3793075Z   Passed UT-F001-043 — A política AuxiliarVeterinario só aceita o perfil AuxiliarVeterinario(perfil: "AuxiliarVeterinario", aceite: True) [< 1 ms]
2026-11-04T18:38:55.3794696Z   Passed UT-F001-047 — A política de propriedade recusa um Cliente sem a claim clienteId [< 1 ms]
2026-11-04T18:38:55.3795937Z   Passed UT-F001-049 — Sem autenticação, a política por omissão recusa o acesso (401) [5 ms]
2026-11-04T18:38:55.3797019Z   Passed UT-F004-017 — consulta de outro Veterinário devolve 403 [14 ms]
2026-11-04T18:38:55.3798901Z   Passed UT-F006-013, UT-F006-014, UT-F006-015 e UT-F006-016 — intervaloDias de 1 a 32767 quando indicado; lote até 50 e produto até 150 caracteres(intervaloDias: 32767, tamanhoLote: 50, tamanhoProduto: 150, valido: True) [3 ms]
2026-11-04T18:38:55.3902111Z   Passed UT-F006-013, UT-F006-014, UT-F006-015 e UT-F006-016 — intervaloDias de 1 a 32767 quando indicado; lote até 50 e produto até 150 caracteres(intervaloDias: 32768, tamanhoLote: 50, tamanhoProduto: 150, valido: False) [2 ms]
2026-11-04T18:38:55.3914217Z   Passed UT-F006-013, UT-F006-014, UT-F006-015 e UT-F006-016 — intervaloDias de 1 a 32767 quando indicado; lote até 50 e produto até 150 caracteres(intervaloDias: 1, tamanhoLote: 50, tamanhoProduto: 150, valido: True) [< 1 ms]
2026-11-04T18:38:55.3916933Z   Passed UT-F006-013, UT-F006-014, UT-F006-015 e UT-F006-016 — intervaloDias de 1 a 32767 quando indicado; lote até 50 e produto até 150 caracteres(intervaloDias: 90, tamanhoLote: 51, tamanhoProduto: 150, valido: False) [< 1 ms]
2026-11-04T18:38:55.3941105Z   Passed UT-F006-013, UT-F006-014, UT-F006-015 e UT-F006-016 — intervaloDias de 1 a 32767 quando indicado; lote até 50 e produto até 150 caracteres(intervaloDias: -30, tamanhoLote: 50, tamanhoProduto: 150, valido: False) [< 1 ms]
2026-11-04T18:38:55.3970394Z   Passed UT-F006-013, UT-F006-014, UT-F006-015 e UT-F006-016 — intervaloDias de 1 a 32767 quando indicado; lote até 50 e produto até 150 caracteres(intervaloDias: null, tamanhoLote: 50, tamanhoProduto: 150, valido: True) [< 1 ms]
2026-11-04T18:38:55.4000900Z   Passed UT-F006-013, UT-F006-014, UT-F006-015 e UT-F006-016 — intervaloDias de 1 a 32767 quando indicado; lote até 50 e produto até 150 caracteres(intervaloDias: 0, tamanhoLote: 50, tamanhoProduto: 150, valido: False) [< 1 ms]
2026-11-04T18:38:55.4010784Z   Passed UT-F006-013, UT-F006-014, UT-F006-015 e UT-F006-016 — intervaloDias de 1 a 32767 quando indicado; lote até 50 e produto até 150 caracteres(intervaloDias: 90, tamanhoLote: 50, tamanhoProduto: 151, valido: False) [< 1 ms]
2026-11-04T18:38:55.4030635Z   Passed I082 — tipo só aceita Vacina ou Desparasitacao(tipo: "Desparasitação", valido: False) [< 1 ms]
2026-11-04T18:38:55.4060481Z   Passed I082 — tipo só aceita Vacina ou Desparasitacao(tipo: "Vacinas", valido: False) [< 1 ms]
2026-11-04T18:38:55.4262207Z   Passed I082 — tipo só aceita Vacina ou Desparasitacao(tipo: "Vacina", valido: True) [< 1 ms]
2026-11-04T18:38:55.4270284Z   Passed I082 — tipo só aceita Vacina ou Desparasitacao(tipo: "vacina", valido: False) [< 1 ms]
2026-11-04T18:38:55.4290445Z   Passed I082 — tipo só aceita Vacina ou Desparasitacao(tipo: "Desparasitacao", valido: True) [< 1 ms]
2026-11-04T18:38:55.4320350Z   Passed UT-F006-020 — BoletimQuery: tipo opcional, só Vacina ou Desparasitacao(tipo: "Vacinas", valido: False) [< 1 ms]
2026-11-04T18:38:55.4321998Z   Passed UT-F006-020 — BoletimQuery: tipo opcional, só Vacina ou Desparasitacao(tipo: null, valido: True) [< 1 ms]
2026-11-04T18:38:55.4350688Z   Passed UT-F006-020 — BoletimQuery: tipo opcional, só Vacina ou Desparasitacao(tipo: "Desparasitacao", valido: True) [< 1 ms]
2026-11-04T18:38:55.4359838Z   Passed UT-F006-020 — BoletimQuery: tipo opcional, só Vacina ou Desparasitacao(tipo: "Vacina", valido: True) [< 1 ms]
2026-11-04T18:38:55.4390615Z   Passed UT-F006-010 — tipo, produto, lote e data de administração são obrigatórios(campo: "dataAdministracao") [< 1 ms]
2026-11-04T18:38:55.4403720Z   Passed UT-F006-010 — tipo, produto, lote e data de administração são obrigatórios(campo: "tipo") [< 1 ms]
2026-11-04T18:38:55.4467927Z   Passed UT-F006-010 — tipo, produto, lote e data de administração são obrigatórios(campo: "lote") [< 1 ms]
2026-11-04T18:38:55.4490515Z   Passed UT-F006-010 — tipo, produto, lote e data de administração são obrigatórios(campo: "produto") [< 1 ms]
2026-11-04T18:38:55.4520224Z   Passed I082 — o pedido de exemplo da API (EP-043) é válido [< 1 ms]
2026-11-04T18:38:55.4540072Z   Passed UT-F004-015 — consulta Cancelada devolve 422 [8 ms]
2026-11-04T18:38:55.4580361Z   Passed I072 — o estado não distingue maiúsculas nem espaços nos extremos(estado: " CONFIRMADA ") [104 ms]
2026-11-04T18:38:55.4581786Z   Passed I077 — os textos são aparados e as notas só com espaços ficam nulas [10 ms]
2026-11-04T18:38:55.4610333Z   Passed I072 — o estado não distingue maiúsculas nem espaços nos extremos(estado: "confirmada") [10 ms]
2026-11-04T18:38:55.4644347Z   Passed UT-F001-074 — Criar um Veterinário sem podeRegistarApoio guarda a autorização a false [92 ms]
2026-11-04T18:38:55.4670588Z   Passed UT-F001-094 — Bloquear uma conta já bloqueada dá 422 [1 ms]
2026-11-04T18:38:55.4690061Z   Passed UT-TRV-001 — Sem pagina nem tamanho, a lista usa a página 1 com 20 elementos [4 ms]
2026-11-04T18:38:55.4730428Z   Passed I066 — Depois da mudança de perfil, o token antigo deixa de ser aceite (EP-003) [9 ms]
2026-11-04T18:38:55.4731719Z   Passed UT-F001-097 — Bloquear um identificador inexistente dá 404 [6 ms]
2026-11-04T18:38:55.4735072Z   Passed I088 (critério 3) — atualização com um tratamento sem descrição devolve 400 e o registo fica como estava [33 ms]
2026-11-04T18:38:55.4760194Z   Passed UT-F004-010 — prescrição sem duracaoDias é aceite [23 ms]
2026-11-04T18:38:55.4790246Z   Passed UT-F001-096 — Bloquear uma conta de Cliente dá 422 e a conta mantém-se ativa [34 ms]
2026-11-04T18:38:55.4888676Z   Passed UT-F004-021 — registo inexistente devolve 404 [30 ms]
2026-11-04T18:38:55.4895001Z   Passed I077 — o mesmo id de tratamento repetido no pedido devolve 400 [15 ms]
2026-11-04T18:38:55.4920234Z   Passed UT-F004-005 — consultaId = 0, ou em falta, devolve 400(consultaId: null) [< 1 ms]
2026-11-04T18:38:55.4930030Z   Passed UT-F004-005 — consultaId = 0, ou em falta, devolve 400(consultaId: 0) [2 ms]
2026-11-04T18:38:55.4950399Z   Passed UT-F003-051 — a Rececionista filtra por estado e recebe as marcações dos dois Veterinários [107 ms]
2026-11-04T18:38:55.4980108Z   Passed UT-F004-016 — consulta inexistente devolve 404 [11 ms]
2026-11-04T18:38:55.5010478Z   Passed UT-F003-056 — futuras: só marcações Pendentes ou Confirmadas futuras dos animais do Cliente, por data crescente [145 ms]
2026-11-04T18:38:55.5730357Z   Passed UT-F004-013 — segundo registo para a mesma consulta devolve 409 e nada é gravado [29 ms]
2026-11-04T18:38:55.5760387Z   Passed I076 — Cliente sem marcações recebe listas vazias [54 ms]
2026-11-04T18:38:55.5761712Z   Passed I072 — a agenda é por dia de Lisboa: no dia da mudança de hora conta a meia-noite local [86 ms]
2026-11-04T18:38:55.5790282Z   Passed UT-F003-052 — dataFim anterior a dataInicio devolve 400 [6 ms]
2026-11-04T18:38:55.5829052Z   Passed I088 (critério 3) — a atualização grava as notas clínicas e o diagnóstico novos [58 ms]
2026-11-04T18:38:55.5862403Z   Passed I076 — cada marcação do Cliente aparece numa só das duas vistas [25 ms]
2026-11-04T18:38:55.5897766Z   Passed I076 — MinhasConsultasQuery aceita futuras, historico ou nada(tipo: "futuras") [< 1 ms]
2026-11-04T18:38:55.5949701Z   Passed I076 — MinhasConsultasQuery aceita futuras, historico ou nada(tipo: null) [< 1 ms]
2026-11-04T18:38:55.5980501Z   Passed I076 — MinhasConsultasQuery aceita futuras, historico ou nada(tipo: "historico") [< 1 ms]
2026-11-04T18:38:55.6010772Z   Passed I072 — estado inválido ou datas fora de 2000-01-01 a 9998-12-31 devolvem 400(estado: null, dataInicio: "1999-12-31", dataFim: null) [7 ms]
2026-11-04T18:38:55.6050778Z   Passed I072 — estado inválido ou datas fora de 2000-01-01 a 9998-12-31 devolvem 400(estado: null, dataInicio: null, dataFim: "9999-12-31") [< 1 ms]
2026-11-04T18:38:55.6080918Z   Passed I072 — estado inválido ou datas fora de 2000-01-01 a 9998-12-31 devolvem 400(estado: "Faturada", dataInicio: null, dataFim: null) [< 1 ms]
2026-11-04T18:38:55.6094430Z   Passed UT-F004-007 — notas só com espaços e listas vazias devolvem 400 [12 ms]
2026-11-04T18:38:55.6150698Z   Passed I076 — o tipo não distingue maiúsculas nem espaços à volta(tipo: " Historico ") [13 ms]
2026-11-04T18:38:55.6201036Z   Passed I076 — o tipo não distingue maiúsculas nem espaços à volta(tipo: "HISTORICO") [8 ms]
2026-11-04T18:38:55.6240499Z   Passed UT-F003-060 — tipo inválido devolve 400(tipo: "todas") [4 ms]
2026-11-04T18:38:55.6277119Z   Passed UT-F004-001 — cria o registo com tratamento e prescrição; o estado da marcação não muda [22 ms]
2026-11-04T18:38:55.6320305Z   Passed UT-F003-060 — tipo inválido devolve 400(tipo: "futura") [6 ms]
2026-11-04T18:38:55.6361238Z   Passed I077 — uma prescrição existente mantém-se indicando o id e é atualizada [6 ms]
2026-11-04T18:38:55.6420570Z   Passed UT-F003-060 — tipo inválido devolve 400(tipo: "passadas") [1 ms]
2026-11-04T18:38:55.6490472Z   Passed UT-F004-004 — consulta Realizada sem registo é aceite (Confirmada e Realizada são admitidas) [4 ms]
2026-11-04T18:38:55.6520064Z   Passed UT-F003-059 — sem tipo é igual a tipo = futuras [9 ms]
2026-11-04T18:38:55.6951041Z   Passed UT-F003-057 — fronteira: uma marcação que começa exatamente agora ainda é futura [4 ms]
2026-11-04T18:38:55.6980763Z   Passed I088 (critério 4) — depois da atualização, o registo, os tratamentos e as prescrições continuam na mesma consulta e no mesmo paciente [31 ms]
2026-11-04T18:38:55.7010266Z   Passed UT-F004-003 — sem notas, diagnóstico nem prescrições, só com um tratamento, é aceite [5 ms]
2026-11-04T18:38:55.7030048Z   Passed UT-F004-014 — consulta Pendente devolve 422 [4 ms]
2026-11-04T18:38:55.7060310Z   Passed UT-F004-020 — atualização com tudo vazio devolve 400 e o registo fica como estava [4 ms]
2026-11-04T18:38:55.7090227Z   Passed UT-F004-018 — mantém T1 atualizado, acrescenta um novo, remove T2 e P1; regista autor e data [14 ms]
2026-11-04T18:38:55.7120387Z   Passed UT-F004-006 — notas e diagnóstico vazios e listas vazias devolvem 400 (nenhum elemento clínico) [5 ms]
2026-11-04T18:38:55.7170176Z   Passed UT-F004-009 — prescrição com duracaoDias = 0 devolve 400 [13 ms]
2026-11-04T18:38:55.7250183Z   Passed I077 — registo de outro Veterinário devolve 403 e não é alterado [16 ms]
2026-11-04T18:38:55.7287826Z   Passed UT-F004-019 — id de tratamento que pertence a outro registo devolve 400 [4 ms]
2026-11-04T18:38:55.7300666Z   Passed I077 e I078 — EP-031 e EP-032 exigem também a política Veterinário (o Auxiliar recebe 403)(nomeAcao: "Criar") [< 1 ms]
2026-11-04T18:38:55.7320307Z   Passed I077 e I078 — EP-031 e EP-032 exigem também a política Veterinário (o Auxiliar recebe 403)(nomeAcao: "Atualizar") [< 1 ms]
2026-11-04T18:38:55.7350169Z   Passed I077 — EP-032 devolve 200 com o registo e usa o Veterinário do token [2 ms]
2026-11-04T18:38:55.7380325Z   Passed I078 — o Auxiliar Veterinário só tem operações de leitura (GET) neste controlador [< 1 ms]
2026-11-04T18:38:55.7381694Z   Passed I077 — As rotas são POST /registos-clinicos e PUT /registos-clinicos/{id} [< 1 ms]
2026-11-04T18:38:55.7382816Z   Passed I077 — um token de Veterinário sem a claim veterinarioId dá 403 sem chamar o serviço [1 ms]
2026-11-04T18:38:55.7384075Z   Passed I078 — o controlador aceita só o Veterinário e o Auxiliar Veterinário e nenhuma ação dispensa a autenticação [< 1 ms]
2026-11-04T18:38:55.7385452Z   Passed I077 — EP-031 devolve 201 com Location /registos-clinicos/{id} e usa o Veterinário do token [< 1 ms]
2026-11-04T18:38:55.7386409Z   Passed I078 — EP-033 devolve 200 com o registo do serviço [1 ms]
2026-11-04T18:38:55.7387577Z   Passed I078 — EP-033 e EP-034 não acrescentam restrições: Veterinário e Auxiliar Veterinário leem(nomeAcao: "Obter") [< 1 ms]
2026-11-04T18:38:55.7388990Z   Passed I078 — EP-033 e EP-034 não acrescentam restrições: Veterinário e Auxiliar Veterinário leem(nomeAcao: "ObterHistorico") [< 1 ms]
2026-11-04T18:38:55.7390664Z   Passed I078 — EP-034 devolve 200 com o histórico e passa o animal e as datas ao serviço [1 ms]
2026-11-04T18:38:55.7391757Z   Passed I078 — As rotas são GET /registos-clinicos/{id} e GET /animais/{id}/historico-clinico [< 1 ms]
2026-11-04T18:38:55.7393063Z   Passed I064 — Um intervalo de limpeza entre 1 e 1440 minutos é aceite (60 por omissão)(intervalo: "1440", esperado: 1440) [1 ms]
2026-11-04T18:38:55.7394420Z   Passed I064 — Um intervalo de limpeza entre 1 e 1440 minutos é aceite (60 por omissão)(intervalo: "1", esperado: 1) [< 1 ms]
2026-11-04T18:38:55.7395788Z   Passed I064 — Um intervalo de limpeza entre 1 e 1440 minutos é aceite (60 por omissão)(intervalo: null, esperado: 60) [< 1 ms]
2026-11-04T18:38:55.8071546Z   Passed I065 — A conta criada inicia sessão em EP-001 com a palavra-passe inicial (critério 6) [305 ms]
2026-11-04T18:38:55.8080257Z   Passed I072 — EP-024 devolve a marcação no corpo e a versão no cabeçalho ETag [2 ms]
2026-11-04T18:38:55.8110405Z   Passed I073 — EP-025 devolve 201 com Location, ETag e o utilizador do token como autor [7 ms]
2026-11-04T18:38:55.8111596Z   Passed I074 — EP-026 passa If-Match ao serviço e devolve 200 com ETag [4 ms]
2026-11-04T18:38:55.8140239Z   Passed I073 — EP-025 é POST /consultas e exige também a política Rececionista [< 1 ms]
2026-11-04T18:38:55.8170524Z   Passed I073 — EP-025 com um token sem identificador de utilizador dá 403 sem chamar o serviço [< 1 ms]
2026-11-04T18:38:55.8200458Z   Passed I072 — EP-023 e EP-024 só aceitam Rececionista, Veterinário e Auxiliar Veterinário [< 1 ms]
2026-11-04T18:38:55.8201734Z   Passed I076 — animalId próprio devolve só as marcações desse animal [143 ms]
2026-11-04T18:38:55.8230377Z   Passed I076 — MinhasConsultasQuery recusa tipo desconhecido e animalId inferior a 1 no mesmo pedido [< 1 ms]
2026-11-04T18:38:55.8264698Z   Passed I074 — EP-027 passa If-Match ao serviço e devolve 204 [7 ms]
2026-11-04T18:38:55.8290094Z   Passed I074 — EP-026 é PUT /consultas/{id} e exige Rececionista [< 1 ms]
2026-11-04T18:38:55.8320139Z   Passed I074 — EP-027 é PATCH /consultas/{id}/estado e exige Rececionista [< 1 ms]
2026-11-04T18:38:55.8321377Z   Passed I072 — EP-023 passa ao serviço o Veterinário do token, e não o do pedido [1 ms]
2026-11-04T18:38:55.8350169Z   Passed I072 — As rotas são GET /consultas e GET /consultas/{id} [< 1 ms]
2026-11-04T18:38:55.8380286Z   Passed I072 — perfil sem acesso à agenda dá 403 antes de chamar o serviço [< 1 ms]
2026-11-04T18:38:55.8410281Z   Passed UT-F003-058 — historico: marcações passadas e Canceladas, mesmo futuras, por data decrescente [17 ms]
2026-11-04T18:38:55.8411828Z   Passed I074 — EP-027 rejeita Pendente e estado inexistente com 400(estado: "Pendente") [20 ms]
2026-11-04T18:38:55.8440313Z   Passed I076 — uma marcação Realizada com data futura vai para o histórico [9 ms]
2026-11-04T18:38:55.8470504Z   Passed I076 — a resposta só tem dados administrativos: data em UTC, estado, animal e nome do Veterinário [6 ms]
2026-11-04T18:38:55.8500263Z   Passed I072 — só dataInicio: dessa data em diante; só dataFim: até ao fim desse dia [234 ms]
2026-11-04T18:38:55.8501558Z   Passed I074 — EP-027 rejeita Pendente e estado inexistente com 400(estado: "Faltou") [10 ms]
2026-11-04T18:38:55.8530203Z   Passed I072 — o Auxiliar Veterinário vê a agenda de todos os Veterinários [4 ms]
2026-11-04T18:38:55.8560151Z   Passed UT-F003-061 — animalId de um animal de outro Cliente devolve 403 [8 ms]
2026-11-04T18:38:55.8590263Z   Passed I072 — EP-024 devolve a versão da marcação como ETag (RowVersion em Base64, entre aspas) [3 ms]
2026-11-04T18:38:55.8591524Z   Passed I074 — EP-027 altera Pendente para Confirmada [8 ms]
2026-11-04T18:38:55.8620327Z   Passed I076 — animalId inexistente também devolve 403, sem revelar se o animal existe [5 ms]
2026-11-04T18:38:55.8650389Z   Passed I065 — Pode ser criada uma conta com cada um dos quatro perfis internos(perfil: "Veterinario") [90 ms]
2026-11-04T18:38:55.8680768Z   Passed UT-TRV-002/003 (EP-023) — tamanho entre 1 e 100 e página a partir de 1(pagina: 0, tamanho: 20, valido: False) [< 1 ms]
2026-11-04T18:38:55.8682394Z   Passed I072 — cada item tem as datas em UTC e as referências ao animal, ao proprietário e ao Veterinário [4 ms]
2026-11-04T18:38:55.8710286Z   Passed I074 — EP-027 marcação inexistente devolve 404 [4 ms]
2026-11-04T18:38:55.8740528Z   Passed UT-TRV-002/003 (EP-023) — tamanho entre 1 e 100 e página a partir de 1(pagina: 1, tamanho: 0, valido: False) [3 ms]
2026-11-04T18:38:55.8770406Z   Passed UT-TRV-002/003 (EP-023) — tamanho entre 1 e 100 e página a partir de 1(pagina: 1, tamanho: 101, valido: False) [< 1 ms]
2026-11-04T18:38:55.8772008Z   Passed UT-TRV-002/003 (EP-023) — tamanho entre 1 e 100 e página a partir de 1(pagina: 1, tamanho: 1, valido: True) [< 1 ms]
2026-11-04T18:38:55.8800589Z   Passed UT-TRV-002/003 (EP-023) — tamanho entre 1 e 100 e página a partir de 1(pagina: 2147483647, tamanho: 20, valido: True) [< 1 ms]
2026-11-04T18:38:55.8830562Z   Passed UT-TRV-002/003 (EP-023) — tamanho entre 1 e 100 e página a partir de 1(pagina: 1, tamanho: 100, valido: True) [< 1 ms]
2026-11-04T18:38:55.8850440Z   Passed I072 — o estado tem de ser um valor de EstadoConsulta, sem distinguir maiúsculas(estado: "Confirmadas", valido: False) [< 1 ms]
2026-11-04T18:38:55.8860606Z   Passed I072 — o estado tem de ser um valor de EstadoConsulta, sem distinguir maiúsculas(estado: "CANCELADA", valido: True) [< 1 ms]
2026-11-04T18:38:55.8870792Z   Passed I072 — o estado tem de ser um valor de EstadoConsulta, sem distinguir maiúsculas(estado: "", valido: True) [< 1 ms]
2026-11-04T18:38:55.8872465Z   Passed I072 — o estado tem de ser um valor de EstadoConsulta, sem distinguir maiúsculas(estado: "Pendente", valido: True) [< 1 ms]
2026-11-04T18:38:55.8874590Z   Passed I072 — o estado tem de ser um valor de EstadoConsulta, sem distinguir maiúsculas(estado: "confirmada", valido: True) [< 1 ms]
2026-11-04T18:38:55.8876245Z   Passed I072 — o estado tem de ser um valor de EstadoConsulta, sem distinguir maiúsculas(estado: " Realizada ", valido: True) [< 1 ms]
2026-11-04T18:38:55.8877899Z   Passed I072 — o estado tem de ser um valor de EstadoConsulta, sem distinguir maiúsculas(estado: "Faturada", valido: False) [< 1 ms]
2026-11-04T18:38:55.8879488Z   Passed UT-F003-052 (validação do pedido) — dataFim anterior a dataInicio é inválida [< 1 ms]
2026-11-04T18:38:55.8881627Z   Passed I072 — datas fora de 2000-01-01 a 9998-12-31 são inválidas; os limites são aceites(ano: 9999, mes: 1, dia: 1, valida: False) [< 1 ms]
2026-11-04T18:38:55.8883275Z   Passed I072 — datas fora de 2000-01-01 a 9998-12-31 são inválidas; os limites são aceites(ano: 9998, mes: 12, dia: 31, valida: True) [< 1 ms]
2026-11-04T18:38:55.8884867Z   Passed I072 — datas fora de 2000-01-01 a 9998-12-31 são inválidas; os limites são aceites(ano: 1999, mes: 12, dia: 31, valida: False) [< 1 ms]
2026-11-04T18:38:55.8886525Z   Passed I072 — datas fora de 2000-01-01 a 9998-12-31 são inválidas; os limites são aceites(ano: 2000, mes: 1, dia: 1, valida: True) [< 1 ms]
2026-11-04T18:38:55.8887874Z   Passed UT-TRV-001 — sem pagina nem tamanho, pagina = 1 e tamanho = 20 [< 1 ms]
2026-11-04T18:38:55.8888973Z   Passed I074 — EP-027 não altera a marcação quando a transição é inválida [6 ms]
2026-11-04T18:38:55.8920477Z   Passed I072 — veterinarioId e animalId, quando indicados, têm de ser superiores a 0(veterinarioId: null, animalId: 0) [< 1 ms]
2026-11-04T18:38:55.8950473Z   Passed I072 — veterinarioId e animalId, quando indicados, têm de ser superiores a 0(veterinarioId: 0, animalId: null) [< 1 ms]
2026-11-04T18:38:55.8952228Z   Passed I072 — veterinarioId e animalId, quando indicados, têm de ser superiores a 0(veterinarioId: -1, animalId: null) [< 1 ms]
2026-11-04T18:38:55.8986566Z   Passed I072 — todos os filtros são opcionais e um só dia (dataInicio = dataFim) é válido [< 1 ms]
2026-11-04T18:38:55.9010295Z   Passed I076 — EP-030 usa a política de propriedade do Cliente e nenhuma ação a dispensa [< 1 ms]
2026-11-04T18:38:55.9040693Z [xUnit.net 00:00:03.58]     UT-TRV-007 — Uma exceção inesperada dá 500 com mensagem genérica, sem a mensagem interna [SKIP]
2026-11-04T18:38:55.9070841Z [xUnit.net 00:00:03.58]       Defeito D-01 (I028, PA-07): o ExceptionHandlingMiddleware copia para detail a mensagem de qualquer exceção, também no 500. Reativar quando o defeito for corrigido.
2026-11-04T18:38:55.9100015Z   Passed UT-F003-044 — mudar estado com If-Match desatualizado devolve 412 [6 ms]
2026-11-04T18:38:55.9101371Z   Passed I076 — EP-030 devolve 200 com a lista do serviço e usa o Cliente do token [4 ms]
2026-11-04T18:38:55.9130178Z   Passed I072 — só as notas da própria marcação, da mais antiga para a mais recente [14 ms]
2026-11-04T18:38:55.9135775Z   Passed I076 — a única operação é GET /me/consultas (sem criar, alterar, confirmar ou cancelar) [< 1 ms]
2026-11-04T18:38:55.9169726Z   Passed UT-F003-053 — o Auxiliar sem autorização de apoio lê a marcação com a nota de preparação [4 ms]
2026-11-04T18:38:55.9200344Z   Passed UT-TRV-010 — O registo do pedido tem método, caminho, código e duração, mas nunca o token nem o corpo [22 ms]
2026-11-04T18:38:55.9221935Z   Skipped UT-TRV-007 — Uma exceção inesperada dá 500 com mensagem genérica, sem a mensagem interna [1 ms]
2026-11-04T18:38:55.9223807Z   Passed UT-TRV-006 — Cada exceção prevista dá o seu código HTTP, com detail igual à mensagem e o formato único de erro(tipo: "KeyNotFoundException", esperado: 404) [11 ms]
2026-11-04T18:38:55.9225882Z   Passed UT-TRV-006 — Cada exceção prevista dá o seu código HTTP, com detail igual à mensagem e o formato único de erro(tipo: "UnprocessableEntityException", esperado: 422) [< 1 ms]
2026-11-04T18:38:55.9227992Z   Passed UT-TRV-006 — Cada exceção prevista dá o seu código HTTP, com detail igual à mensagem e o formato único de erro(tipo: "InvalidOperationException", esperado: 409) [< 1 ms]
2026-11-04T18:38:55.9230607Z   Passed UT-TRV-006 — Cada exceção prevista dá o seu código HTTP, com detail igual à mensagem e o formato único de erro(tipo: "UnauthorizedAccessException", esperado: 401) [< 1 ms]
2026-11-04T18:38:55.9232583Z   Passed UT-TRV-006 — Cada exceção prevista dá o seu código HTTP, com detail igual à mensagem e o formato único de erro(tipo: "ForbiddenException", esperado: 403) [< 1 ms]
2026-11-04T18:38:55.9234479Z   Passed UT-TRV-006 — Cada exceção prevista dá o seu código HTTP, com detail igual à mensagem e o formato único de erro(tipo: "ArgumentException", esperado: 400) [< 1 ms]
2026-11-04T18:38:55.9236093Z   Passed I072 — paginação: ordem por data e hora e depois por id, com o total de todas as páginas [38 ms]
2026-11-04T18:38:55.9237233Z   Passed UT-F003-055 — o Veterinário lê a marcação que lhe está atribuída [4 ms]
2026-11-04T18:38:55.9238304Z   Passed I072 — marcação inexistente devolve 404, também para um Veterinário [12 ms]
2026-11-04T18:38:55.9240313Z   Passed UT-TRV-009 — Corpo em falta ou JSON mal formado dão 400 com a mensagem genérica, sem mensagens internas(chave: "") [29 ms]
2026-11-04T18:38:55.9241930Z   Passed UT-TRV-009 — Corpo em falta ou JSON mal formado dão 400 com a mensagem genérica, sem mensagens internas(chave: "$.email") [< 1 ms]
2026-11-04T18:38:55.9243444Z   Passed UT-TRV-008 — Erros de validação em email e password dão 400 com as duas mensagens em português no detail [< 1 ms]
2026-11-04T18:38:55.9244812Z   Passed I065 — Pode ser criada uma conta com cada um dos quatro perfis internos(perfil: "Rececionista") [93 ms]
2026-11-04T18:38:55.9245930Z   Passed UT-F002-025 — Nascido a 2022-05-10, em 2026-10-14 tem 4 anos [85 ms]
2026-11-04T18:38:55.9506843Z   Passed UT-F002-029 — Nascido a 2024-02-29, a 2025-03-01 tem 1 ano [3 ms]
2026-11-04T18:38:55.9508167Z   Passed UT-F001-021 — O fim de sessão regista o jti como revogado até à expiração do token [30 ms]
2026-11-04T18:38:55.9530168Z   Passed I064 — Um jti vazio é recusado (400) e nada é gravado(jti: "   ") [4 ms]
2026-11-04T18:38:55.9560039Z   Passed I064 — Um jti vazio é recusado (400) e nada é gravado(jti: "") [< 1 ms]
2026-11-04T18:38:55.9590134Z   Passed I064 — O fim de sessão não afeta os outros tokens do mesmo utilizador [1 ms]
2026-11-04T18:38:55.9591737Z   Passed I064 — A revogação fica na base de dados e continua a valer num novo arranque da API [4 ms]
2026-11-04T18:38:55.9620246Z   Passed UT-F001-022 — Terminar a sessão duas vezes com o mesmo jti não falha nem repete a revogação [1 ms]
2026-11-04T18:38:55.9650039Z   Passed UT-F002-011 — Data de nascimento de hoje é aceite e a idade é 0 [29 ms]
2026-11-04T18:38:55.9680146Z   Passed UT-F002-010 — Data de nascimento de amanhã (2026-10-15) devolve 400 [1 ms]
2026-11-04T18:38:55.9710145Z   Passed UT-F002-028 — Nascido a 2024-02-29, a 2025-02-28 ainda tem 0 anos [2 ms]
2026-11-04T18:38:55.9711459Z   Passed I064 — A limpeza apaga só as revogações de tokens já expirados [16 ms]
2026-11-04T18:38:55.9740109Z   Passed UT-F002-003 — clienteId = 0 (em falta) é recusado no pedido (400) [2 ms]
2026-11-04T18:38:55.9770112Z   Passed UT-F001-016 — Um token com o jti revogado deixa de ser uma sessão válida [1 ms]
2026-11-04T18:38:55.9800086Z   Passed I064 — Depois do fim de sessão, o mesmo token deixa de ser uma sessão válida [1 ms]
2026-11-04T18:38:55.9801354Z   Passed I064 — A limpeza mantém a revogação de um token que expira no próprio instante [5 ms]
2026-11-04T18:38:55.9830087Z   Passed I064 — A data de revogação é gravada sem frações de segundo (DATETIME2(0)) [1 ms]
2026-11-04T18:38:55.9950288Z   Passed I064 — Sem revogações expiradas, a limpeza não apaga nada [< 1 ms]
2026-11-04T18:38:55.9951434Z   Passed UT-F002-002 — Sem raça, data de nascimento e peso, o paciente é registado com esses campos vazios [9 ms]
2026-11-04T18:38:55.9980226Z   Passed UT-F003-050 — o Veterinário que envia o veterinarioId de outro só recebe as suas marcações [91 ms]
2026-11-04T18:38:56.0010105Z   Passed UT-F002-023 — O CLIENTE_A pede o ANIMAL_B1 e recebe 403 sem dados do animal [19 ms]
2026-11-04T18:38:56.0040158Z   Passed I072 — uma página depois do fim, mesmo a maior possível, vem vazia e com o total [3 ms]
2026-11-04T18:38:56.0060545Z   Passed UT-F002-013 — Uma raça de gato (Siamês) num cão devolve 400 com erro na raça [2 ms]
2026-11-04T18:38:56.0100146Z   Passed UT-F002-024 — O CLIENTE_A pede um animal inexistente e recebe 404 [1 ms]
2026-11-04T18:38:56.0370766Z   Passed UT-F003-054 — o Veterinário que lê a marcação de outro Veterinário recebe 403 [7 ms]
2026-11-04T18:38:56.0371995Z   Passed I065 — Pode ser criada uma conta com cada um dos quatro perfis internos(perfil: "AuxiliarVeterinario") [88 ms]
2026-11-04T18:38:56.0405632Z   Passed UT-F002-020 — Na área do Cliente, o CLIENTE_A vê só os seus animais [20 ms]
2026-11-04T18:38:56.0475720Z   Passed UT-F002-014 — Atualizar o peso do ANIMAL_A1 grava o novo peso e DataUltimaAlteracao = RELOGIO [20 ms]
2026-11-04T18:38:56.0476998Z   Passed UT-F004-024 — o resumo indica numeroTratamentos = 1 e numeroPrescricoes = 2 [67 ms]
2026-11-04T18:38:56.0520787Z   Passed UT-F002-001 — Registar o Bobi cria o paciente do CLIENTE_A com DataRegisto = RELOGIO e idadeAnos = 4 [3 ms]
2026-11-04T18:38:56.0560827Z   Passed I078 — HistoricoClinicoQuery: a validação do pedido aceita intervalos válidos e rejeita dataFim anterior a dataInicio(dataInicio: "1999-12-31", dataFim: null, valido: False) [< 1 ms]
2026-11-04T18:38:56.0600687Z   Passed I078 — HistoricoClinicoQuery: a validação do pedido aceita intervalos válidos e rejeita dataFim anterior a dataInicio(dataInicio: "2026-10-14", dataFim: null, valido: True) [< 1 ms]
2026-11-04T18:38:56.0612912Z   Passed I078 — HistoricoClinicoQuery: a validação do pedido aceita intervalos válidos e rejeita dataFim anterior a dataInicio(dataInicio: null, dataFim: "2026-10-14", valido: True) [< 1 ms]
2026-11-04T18:38:56.0616151Z   Passed I078 — HistoricoClinicoQuery: a validação do pedido aceita intervalos válidos e rejeita dataFim anterior a dataInicio(dataInicio: null, dataFim: null, valido: True) [< 1 ms]
2026-11-04T18:38:56.0618326Z   Passed I078 — HistoricoClinicoQuery: a validação do pedido aceita intervalos válidos e rejeita dataFim anterior a dataInicio(dataInicio: "2026-10-14", dataFim: "2026-10-14", valido: True) [< 1 ms]
2026-11-04T18:38:56.0621047Z   Passed I078 — HistoricoClinicoQuery: a validação do pedido aceita intervalos válidos e rejeita dataFim anterior a dataInicio(dataInicio: "2026-10-14", dataFim: "2026-09-01", valido: False) [< 1 ms]
2026-11-04T18:38:56.0622532Z   Passed UT-F002-017 — Atualizar para um Cliente inexistente devolve 404 [1 ms]
2026-11-04T18:38:56.0623396Z   Passed UT-F002-022 — O CLIENTE_A consulta o ANIMAL_A1 e recebe o MeuAnimalDto [2 ms]
2026-11-04T18:38:56.0624303Z   Passed UT-F002-006 — Um nome com 81 caracteres é recusado (máximo 80) [< 1 ms]
2026-11-04T18:38:56.0649802Z   Passed UT-F002-021 — Um Cliente sem animais recebe uma lista vazia, sem erro [1 ms]
2026-11-04T18:38:56.0660043Z   Passed UT-F003-076 — Auxiliar com apoio cria a nota com o autor e a hora do servidor [47 ms]
2026-11-04T18:38:56.0712220Z   Passed UT-F003-084 — marcação inexistente devolve 404 [1 ms]
2026-11-04T18:38:56.0760207Z   Passed I078 — o histórico não inclui registos de outros animais nem consultas sem registo [11 ms]
2026-11-04T18:38:56.0795221Z   Passed I078 — datas fora de 2000-01-01 a 9998-12-31 devolvem 400 [< 1 ms]
2026-11-04T18:38:56.0846359Z   Passed UT-F003-078, 079 e 081 — texto vazio, só espaços ou com 1001 caracteres devolve 400(texto: "") [12 ms]
2026-11-04T18:38:56.0880306Z   Passed UT-F003-078, 079 e 081 — texto vazio, só espaços ou com 1001 caracteres devolve 400(texto: "   ") [1 ms]
2026-11-04T18:38:56.0910288Z   Passed UT-F003-078, 079 e 081 — texto vazio, só espaços ou com 1001 caracteres devolve 400(texto: null) [4 ms]
2026-11-04T18:38:56.0940151Z   Passed UT-F003-083 — marcação Realizada é aceite: só Cancelada impede o registo [3 ms]
2026-11-04T18:38:56.0960224Z   Passed UT-F003-077 — Auxiliar sem autorização de apoio recebe 403 e nada é gravado [4 ms]
2026-11-04T18:38:56.0970717Z   Passed UT-F003-080 e 081 — 1000 caracteres é aceite e 1001 devolve 400(comprimento: 1001, aceite: False) [1 ms]
2026-11-04T18:38:56.0972116Z   Passed UT-F003-080 e 081 — 1000 caracteres é aceite e 1001 devolve 400(comprimento: 1000, aceite: True) [4 ms]
2026-11-04T18:38:56.0973147Z   Passed I075 — EP-029 não altera a marcação [6 ms]
2026-11-04T18:38:56.0973936Z   Passed UT-F003-082 — marcação Cancelada devolve 422 [1 ms]
2026-11-04T18:38:56.0975745Z   Passed UT-F006-001, UT-F006-003, UT-F006-004, UT-F006-005, UT-F006-006 e UT-F006-007 — data de administração + intervalo em dias de calendário(dataAdministracao: "2026-10-14", intervaloDias: 90, esperada: "2027-01-12") [< 1 ms]
2026-11-04T18:38:56.1120419Z   Passed UT-F006-001, UT-F006-003, UT-F006-004, UT-F006-005, UT-F006-006 e UT-F006-007 — data de administração + intervalo em dias de calendário(dataAdministracao: "2026-01-31", intervaloDias: 1, esperada: "2026-02-01") [< 1 ms]
2026-11-04T18:38:56.1131111Z   Passed UT-F006-001, UT-F006-003, UT-F006-004, UT-F006-005, UT-F006-006 e UT-F006-007 — data de administração + intervalo em dias de calendário(dataAdministracao: "2027-03-01", intervaloDias: 365, esperada: "2028-02-29") [< 1 ms]
2026-11-04T18:38:56.1161191Z   Passed UT-F006-001, UT-F006-003, UT-F006-004, UT-F006-005, UT-F006-006 e UT-F006-007 — data de administração + intervalo em dias de calendário(dataAdministracao: "2026-12-31", intervaloDias: 1, esperada: "2027-01-01") [< 1 ms]
2026-11-04T18:38:56.1191104Z   Passed UT-F006-001, UT-F006-003, UT-F006-004, UT-F006-005, UT-F006-006 e UT-F006-007 — data de administração + intervalo em dias de calendário(dataAdministracao: "2028-02-28", intervaloDias: 1, esperada: "2028-02-29") [< 1 ms]
2026-11-04T18:38:56.1197843Z   Passed UT-F006-001, UT-F006-003, UT-F006-004, UT-F006-005, UT-F006-006 e UT-F006-007 — data de administração + intervalo em dias de calendário(dataAdministracao: "2027-02-28", intervaloDias: 1, esperada: "2027-03-01") [< 1 ms]
2026-11-04T18:38:56.1199849Z   Passed I082 — intervalo indicado e não superior a 0 é recusado(intervaloDias: -30) [< 1 ms]
2026-11-04T18:38:56.1200983Z   Passed I082 — intervalo indicado e não superior a 0 é recusado(intervaloDias: 0) [< 1 ms]
2026-11-04T18:38:56.1202074Z   Passed UT-F006-002 — sem intervalo não há próxima administração (null) [< 1 ms]
2026-11-04T18:38:56.1203367Z   Passed I072 — sem versão (null) é um erro de programação [< 1 ms]
2026-11-04T18:38:56.1228687Z   Passed I078 — EP-033 não altera o registo (só leitura) [39 ms]
2026-11-04T18:38:56.1240166Z   Passed UT-F003-087 — If-Match com versão diferente não corresponde [3 ms]
2026-11-04T18:38:56.1270438Z   Passed I074 — Sem If-Match a operação não fica condicionada à versão [< 1 ms]
2026-11-04T18:38:56.1303628Z   Passed I072 — versões diferentes dão ETags diferentes [< 1 ms]
2026-11-04T18:38:56.1330115Z   Passed UT-F003-089 — If-Match asterisco corresponde a recurso existente [< 1 ms]
2026-11-04T18:38:56.1345000Z   Passed UT-F003-086 — If-Match com a versão atual corresponde [< 1 ms]
2026-11-04T18:38:56.1364622Z   Passed UT-F003-088 — If-Match fraco não corresponde [< 1 ms]
2026-11-04T18:38:56.1389089Z   Passed UT-F003-085 — RowVersion 00 00 00 00 00 00 07 D1 dá o ETag "AAAAAAAAB9E=" [< 1 ms]
2026-11-04T18:38:56.1417724Z   Passed UT-F003-090 — If-Match sem aspas não corresponde [< 1 ms]
2026-11-04T18:38:56.1444116Z   Passed UT-F001-062 — "501234567" tem o dígito de controlo errado e é inválido [< 1 ms]
2026-11-04T18:38:56.1480251Z   Passed I067 — Sem valor, o atributo aceita: a obrigatoriedade fica para o [Required](valor: "") [< 1 ms]
2026-11-04T18:38:56.1481566Z   Passed I067 — Sem valor, o atributo aceita: a obrigatoriedade fica para o [Required](valor: "   ") [< 1 ms]
2026-11-04T18:38:56.1510184Z   Passed I067 — Sem valor, o atributo aceita: a obrigatoriedade fica para o [Required](valor: null) [< 1 ms]
2026-11-04T18:38:56.1540109Z   Passed UT-F001-063 — Um NIF com 8 algarismos é inválido [< 1 ms]
2026-11-04T18:38:56.1545160Z   Passed UT-F001-064 — Um NIF com 10 algarismos é inválido [< 1 ms]
2026-11-04T18:38:56.1560613Z   Passed I067 — Outros NIF com o dígito de controlo correto são válidos(nif: "999999990") [< 1 ms]
2026-11-04T18:38:56.1561879Z   Passed I067 — Outros NIF com o dígito de controlo correto são válidos(nif: "123456789") [< 1 ms]
2026-11-04T18:38:56.1562981Z   Passed I067 — No pedido, os espaços à volta do NIF são ignorados [< 1 ms]
2026-11-04T18:38:56.1564022Z   Passed UT-F001-061 — "234567899" (resto 2, dígito de controlo 9) é um NIF válido [< 1 ms]
2026-11-04T18:38:56.1565119Z   Passed UT-F001-066 — Um NIF com espaços no meio é inválido: não há normalização [< 1 ms]
2026-11-04T18:38:56.1566164Z   Passed UT-F001-065 — Um NIF com caracteres não numéricos é inválido [< 1 ms]
2026-11-04T18:38:56.1567207Z   Passed UT-F001-060 — "501234560" (resto 1, dígito de controlo 0) é um NIF válido [< 1 ms]
2026-11-04T18:38:56.1568440Z   Passed UT-F002-019 — Filtrar por clienteId do CLIENTE_A devolve só o ANIMAL_A1 e o ANIMAL_A2 [50 ms]
2026-11-04T18:38:56.1569712Z   Passed UT-F004-027 — animal inexistente (id = 999) devolve 404 [6 ms]
2026-11-04T18:38:56.1570721Z   Passed UT-F002-012 — Uma espécie que não existe devolve 400 com erro na espécie [2 ms]
2026-11-04T18:38:56.1572147Z   Passed UT-F002-030 — Sem data de nascimento, idadeAnos fica vazio (divergência registada; a I028 propunha 0) [1 ms]
2026-11-04T18:38:56.1573635Z   Passed I065 — Pode ser criada uma conta com cada um dos quatro perfis internos(perfil: "Administrador") [109 ms]
2026-11-04T18:38:56.1574722Z   Passed UT-F002-008 — Peso 0,01 é aceite [13 ms]
2026-11-04T18:38:56.1575615Z   Passed UT-F002-015 — Atualizar com peso 0 devolve 400 e nada é alterado [2 ms]
2026-11-04T18:38:56.1576594Z   Passed UT-F001-088 — O e-mail de outra conta dá 409 e nada muda [3 ms]
2026-11-04T18:38:56.1577797Z   Passed UT-F004-023 — sem datas, devolve os dois registos do animal: primeiro o de 2026-10-14, depois o de 2026-09-02 [21 ms]
2026-11-04T18:38:56.1578983Z   Passed UT-F002-026 — Nascido a 2022-10-14, faz 4 anos no próprio dia [2 ms]
2026-11-04T18:38:56.1580286Z   Passed UT-F004-026 — dataInicio = 2026-10-14 e dataFim = 2026-09-01 devolve 400 [< 1 ms]
2026-11-04T18:38:56.1581340Z   Passed UT-F002-016 — Atualizar um animal inexistente devolve 404 [2 ms]
2026-11-04T18:38:56.1582503Z   Passed UT-F002-018 — A ficha do ANIMAL_A1 tem só dados administrativos e o telefone do Cliente [1 ms]
2026-11-04T18:38:56.1583955Z   Passed UT-F002-007 — Peso 0 devolve 400 «O peso deve ser superior a 0.» [1 ms]
2026-11-04T18:38:56.1584927Z   Passed UT-F002-009 — Peso negativo devolve 400 [1 ms]
2026-11-04T18:38:56.1585907Z   Passed UT-F002-027 — Nascido a 2022-10-15, ainda tem 3 anos na véspera do aniversário [1 ms]
2026-11-04T18:38:56.1586912Z   Passed UT-F002-005 — Sem nome, o pedido é recusado (400) [< 1 ms]
2026-11-04T18:38:56.1587788Z   Passed UT-F002-004 — Um Cliente inexistente devolve 404 e nada é gravado [6 ms]
2026-11-04T18:38:56.1588705Z   Passed I064 — Uma falha da limpeza fica no log e não pára a API [402 ms]
2026-11-04T18:38:56.2185702Z   Passed I064 — A limpeza das revogações expiradas corre logo no arranque da API [5 ms]
2026-11-04T18:38:56.2188798Z   Passed I064 — Um intervalo de limpeza fora de 1 a 1440 minutos impede o arranque(intervalo: "1441") [< 1 ms]
2026-11-04T18:38:56.2190395Z   Passed I064 — Um intervalo de limpeza fora de 1 a 1440 minutos impede o arranque(intervalo: "-5") [< 1 ms]
2026-11-04T18:38:56.2191681Z   Passed I064 — Um intervalo de limpeza fora de 1 a 1440 minutos impede o arranque(intervalo: "0") [< 1 ms]
2026-11-04T18:38:56.2192765Z   Passed I064 — Ao parar a API, a rotina de limpeza termina sem erro [3 ms]
2026-11-04T18:38:56.2193843Z   Passed UT-F004-025 — dataInicio = dataFim = 2026-10-14 devolve só o registo de 2026-10-14 [52 ms]
2026-11-04T18:38:56.2194928Z   Passed I078 — animal sem registos devolve 200 com a lista vazia (IT-EP034-05) [4 ms]
2026-11-04T18:38:56.2195889Z   Passed I078 — só dataInicio ou só dataFim também filtram [41 ms]
2026-11-04T18:38:56.2196990Z   Passed I078 — a data é o dia da consulta em hora de Lisboa: 2026-10-13T23:30Z conta como 2026-10-14 (R-FUSO) [5 ms]
2026-11-04T18:38:56.2949644Z   Passed I078 — EP-033 com id inexistente devolve 404 [3 ms]
2026-11-04T18:38:56.2957940Z   Passed UT-F004-022 — devolve o RegistoClinicoDto completo (notas, diagnóstico, tratamentos e prescrições) [4 ms]
2026-11-04T18:38:56.2959674Z   Passed I078 — inclui os registos de todos os Veterinários do animal (RF035) [4 ms]
2026-11-04T18:38:56.2960949Z   Passed I066 — Depois do bloqueio, o login dá 403 e o token já emitido deixa de ser aceite (critério 4) [172 ms]
2026-11-04T18:38:56.2962167Z   Passed I066 — O Administrador pode mudar o perfil de outro Administrador [1 ms]
2026-11-04T18:38:56.2963363Z   Passed UT-F001-085 — O Administrador não pode remover o próprio perfil de Administrador (422) [1 ms]
2026-11-04T18:38:56.2964729Z   Passed I066 — A autorização de apoio de um Auxiliar pode ser concedida ou retirada(antes: True, depois: False) [5 ms]
2026-11-04T18:38:56.2966194Z   Passed I066 — A autorização de apoio de um Auxiliar pode ser concedida ou retirada(antes: False, depois: True) [< 1 ms]
2026-11-04T18:38:56.2967568Z   Passed UT-F001-080 — Uma palavra-passe fraca dá 400 com o erro no campo passwordInicial [< 1 ms]
2026-11-04T18:38:56.3785778Z   Passed UT-F001-077 — Um e-mail já registado dá 409 e nada é gravado [1 ms]
2026-11-04T18:38:56.3786871Z   Passed UT-F001-081 — Um nome com 150 caracteres é aceite (limite de Funcionario.Nome) [52 ms]
2026-11-04T18:38:56.3787935Z   Passed I065 — Sem filtros, lista todas as contas de funcionário por nome e nunca as de Cliente [3 ms]
2026-11-04T18:38:56.3789094Z   Passed UT-F001-095 — O Administrador não pode bloquear a própria conta (422) e mantém-se ativo [4 ms]
2026-11-04T18:38:56.3790371Z   Passed I066 — Um Auxiliar com apoio que muda de perfil sem retirar a autorização dá 422 (critério 3) [1 ms]
2026-11-04T18:38:56.3791460Z   Passed UT-F001-076 — Um Rececionista com podeRegistarApoio é recusado com 422 e nada é gravado [< 1 ms]
2026-11-04T18:38:56.3792417Z   Passed UT-F001-099 — Um perfil fora da enumeração ("Gestor") dá 400 [< 1 ms]
2026-11-04T18:38:56.3793229Z   Passed I066 — O e-mail de uma conta de Cliente também dá 409 [1 ms]
2026-11-04T18:38:56.3794184Z   Passed UT-F001-098 — Filtrar por perfil Veterinario e estado Ativo devolve só os Veterinários ativos [16 ms]
2026-11-04T18:38:56.4986742Z [xUnit.net 00:00:04.20]   Finished:    ClinicaVeterinaria.UnitTests
2026-11-04T18:38:57.1833939Z Data collector 'Blame' message: All tests finished running, Sequence file will not be generated.
2026-11-04T18:38:57.2139050Z   Passed UT-F001-090 — Um identificador inexistente dá 404 [1 ms]
2026-11-04T18:38:57.2140368Z   Passed I066 — Uma conta de Cliente dá 404 em EP-007 (só contas de funcionário) [1 ms]
2026-11-04T18:38:57.2141635Z   Passed I065 — A consulta devolve a conta de funcionário com a data de criação em UTC [1 ms]
2026-11-04T18:38:57.2142727Z   Passed UT-F001-086 — O Administrador altera os próprios dados mantendo o perfil [1 ms]
2026-11-04T18:38:57.2143715Z   Passed UT-F001-075 — O perfil Cliente é recusado com 422 e nada é gravado [< 1 ms]
2026-11-04T18:38:57.2144885Z   Passed UT-F001-073 — Criar um Auxiliar com podeRegistarApoio cria a conta ativa e o Funcionario [104 ms]
2026-11-04T18:38:57.2146181Z   Passed UT-F001-083 — Atualizar REC_A para Veterinario muda o perfil e os dados e devolve a conta [6 ms]
2026-11-04T18:38:57.3278643Z Results File: /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/resultados-ci/testes-unitarios/testes-unitarios.trx
2026-11-04T18:38:57.3281176Z 
2026-11-04T18:38:57.3317759Z Test Run Successful.
2026-11-04T18:38:57.3318106Z Total tests: 718
2026-11-04T18:38:57.3318375Z      Passed: 717
2026-11-04T18:38:57.3318621Z     Skipped: 1
2026-11-04T18:38:57.3319758Z  Total time: 6.2237 Seconds
2026-11-04T18:38:57.5064647Z 
2026-11-04T18:38:57.5068898Z Attachments:
2026-11-04T18:38:57.5071516Z   /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/resultados-ci/testes-unitarios/883d3af9-6ddd-40af-8d3e-7a33f18f28c1/coverage.cobertura.xml
2026-11-04T18:38:57.5662674Z ##[group]Run python3 .github/scripts/resumo-testes.py "$RESULTADOS/testes-unitarios" "2 · Testes unitários (QG-02)"
2026-11-04T18:38:57.5663726Z [36;1mpython3 .github/scripts/resumo-testes.py "$RESULTADOS/testes-unitarios" "2 · Testes unitários (QG-02)"[0m
2026-11-04T18:38:57.5688596Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:38:57.5689036Z env:
2026-11-04T18:38:57.5689458Z   DOTNET_NOLOGO: true
2026-11-04T18:38:57.5689747Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:38:57.5690073Z   CONFIGURACAO: Release
2026-11-04T18:38:57.5690403Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:38:57.5691029Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:38:57.5691980Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:38:57.5692695Z   RESULTADOS: resultados-ci
2026-11-04T18:38:57.5693102Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:38:57.5693467Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:38:57.5693846Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:38:57.5694154Z ##[endgroup]
2026-11-04T18:38:57.6062462Z ### 2 · Testes unitários (QG-02)
2026-11-04T18:38:57.6062857Z 
2026-11-04T18:38:57.6063530Z **✅ Todos os testes passaram** (QG-02: 100% dos testes a passar)
2026-11-04T18:38:57.6063885Z 
2026-11-04T18:38:57.6064229Z | Total | Executados | Aprovados | Falhados | Não executados |
2026-11-04T18:38:57.6064690Z |---:|---:|---:|---:|---:|
2026-11-04T18:38:57.6064993Z | 718 | 717 | 717 | 0 | 0 |
2026-11-04T18:38:57.6065181Z 
2026-11-04T18:38:57.6065349Z Ficheiros TRX: `testes-unitarios.trx`
2026-11-04T18:38:57.6065588Z 
2026-11-04T18:38:57.6065594Z 
2026-11-04T18:38:57.6265860Z ##[group]Run actions/upload-artifact@cf430e030ddbb5b0abf93d22962f4752f3646cd9
2026-11-04T18:38:57.6266223Z with:
2026-11-04T18:38:57.6266409Z   name: resultados-testes-unitarios
2026-11-04T18:38:57.6266671Z   path: resultados-ci/testes-unitarios
2026-11-04T18:38:57.6266910Z   if-no-files-found: warn
2026-11-04T18:38:57.6267100Z   retention-days: 90
2026-11-04T18:38:57.6267282Z   compression-level: 6
2026-11-04T18:38:57.6267466Z   overwrite: false
2026-11-04T18:38:57.6267653Z   include-hidden-files: false
2026-11-04T18:38:57.6267852Z   archive: true
2026-11-04T18:38:57.6268014Z env:
2026-11-04T18:38:57.6268168Z   DOTNET_NOLOGO: true
2026-11-04T18:38:57.6268366Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:38:57.6268577Z   CONFIGURACAO: Release
2026-11-04T18:38:57.6268784Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:38:57.6269368Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:38:57.6270024Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:38:57.6270461Z   RESULTADOS: resultados-ci
2026-11-04T18:38:57.6270667Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:38:57.6270906Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:38:57.6271139Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:38:57.6271337Z ##[endgroup]
2026-11-04T18:38:57.7492592Z With the provided path, there will be 3 files uploaded
2026-11-04T18:38:57.7499461Z Artifact name is valid!
2026-11-04T18:38:57.7500077Z Root directory input is valid!
2026-11-04T18:38:58.0895189Z Uploading artifact: resultados-testes-unitarios.zip
2026-11-04T18:38:58.0925269Z Beginning upload of artifact content to blob storage
2026-11-04T18:38:58.5530274Z Uploaded bytes 305281
2026-11-04T18:38:58.6168373Z Finished uploading artifact content to blob storage!
2026-11-04T18:38:58.6169059Z SHA256 digest of uploaded artifact is afbbd06bd3fbaa2d7d33a1635306abfc84eddb11988ee5f453994bd34881c311
2026-11-04T18:38:58.6169847Z Finalizing artifact upload
2026-11-04T18:38:59.2263400Z Artifact resultados-testes-unitarios successfully finalized. Artifact ID 13352638229
2026-11-04T18:38:59.2264437Z Artifact resultados-testes-unitarios has been successfully uploaded! Final size is 305281 bytes. Artifact ID is 13352638229
2026-11-04T18:38:59.2267108Z Artifact download URL: https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/actions/runs/41888968713/artifacts/13352638229
2026-11-04T18:38:59.2975056Z Post job cleanup.
2026-11-04T18:38:59.3000161Z ##[start-action display=Reutilizar a cache dos pacotes NuGet;id=__cce0ee8f-caa2-4f8a-a73e-c33b680e62a6.__actions_cache]
2026-11-04T18:38:59.6368631Z Post job cleanup.
2026-11-04T18:38:59.7378398Z Cache hit occurred on the primary key nuget-Linux-c8252acdcad9b687c3131c725ee3c1b10105ceb23478247a43e8389c12c19830, not saving cache.
2026-11-04T18:38:59.7446049Z ##[end-action id=__cce0ee8f-caa2-4f8a-a73e-c33b680e62a6.__actions_cache;outcome=success;conclusion=success;duration_ms=444]
2026-11-04T18:38:59.7470954Z ##[start-action display=Instalar o SDK do .NET 10;id=__cce0ee8f-caa2-4f8a-a73e-c33b680e62a6.__actions_setup-dotnet]
2026-11-04T18:38:59.8770827Z Post job cleanup.
2026-11-04T18:38:59.9822445Z ##[end-action id=__cce0ee8f-caa2-4f8a-a73e-c33b680e62a6.__actions_setup-dotnet;outcome=success;conclusion=success;duration_ms=233]
2026-11-04T18:39:00.1210758Z Post job cleanup.
2026-11-04T18:39:00.2017667Z [command]/usr/bin/git version
2026-11-04T18:39:00.2065657Z git version 2.56.0
2026-11-04T18:39:00.2104659Z Temporarily overriding HOME='/home/runner/work/_temp/3fdba9f5-2030-4bc6-b3bc-87ca5b80973c' before making global git config changes
2026-11-04T18:39:00.2110452Z Adding repository directory to the temporary git global config as a safe directory
2026-11-04T18:39:00.2111672Z [command]/usr/bin/git config --global --add safe.directory /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:39:00.2278254Z Removing SSH command configuration
2026-11-04T18:39:00.2286462Z [command]/usr/bin/git config --local --name-only --get-regexp core\.sshCommand
2026-11-04T18:39:00.2325977Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'core\.sshCommand' && git config --local --unset-all 'core.sshCommand' || :"
2026-11-04T18:39:00.2557549Z Removing HTTP extra header
2026-11-04T18:39:00.2571474Z [command]/usr/bin/git config --local --name-only --get-regexp http\.https\:\/\/github\.com\/\.extraheader
2026-11-04T18:39:00.2609614Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'http\.https\:\/\/github\.com\/\.extraheader' && git config --local --unset-all 'http.https://github.com/.extraheader' || :"
2026-11-04T18:39:00.2830247Z Removing includeIf entries pointing to credentials config files
2026-11-04T18:39:00.2839406Z [command]/usr/bin/git config --local --name-only --get-regexp ^includeIf\.gitdir:
2026-11-04T18:39:00.2880793Z [command]/usr/bin/git submodule foreach --recursive git config --local --show-origin --name-only --get-regexp remote.origin.url
2026-11-04T18:39:00.3348718Z Cleaning up orphan processes
2026-11-04T18:39:00.3616924Z Terminate orphan process: pid (2489) (dotnet)
2026-11-04T18:39:00.3660394Z Terminate orphan process: pid (2490) (dotnet)
2026-11-04T18:39:00.3688601Z Terminate orphan process: pid (2491) (dotnet)
2026-11-04T18:39:00.3717268Z Terminate orphan process: pid (2604) (VBCSCompiler)
``````
