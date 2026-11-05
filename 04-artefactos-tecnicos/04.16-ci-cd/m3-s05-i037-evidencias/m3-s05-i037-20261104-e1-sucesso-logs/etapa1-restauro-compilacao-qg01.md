``````text
﻿2026-11-04T18:37:26.7115123Z Current runner version: '2.338.2'
2026-11-04T18:37:26.7140006Z ##[group]Runner Image Provisioner
2026-11-04T18:37:26.7140891Z Hosted Compute Agent
2026-11-04T18:37:26.7141571Z Version: 20261021.618
2026-11-04T18:37:26.7142290Z Commit: 21505ad2541d5be6135ed84fc33697146cf75b8e
2026-11-04T18:37:26.7143071Z Build Date: 2026-10-21T00:45:21Z
2026-11-04T18:37:26.7143853Z Worker ID: {38e97155-710c-4cfc-8e4d-e7fdceab67f2}
2026-11-04T18:37:26.7144585Z Region: westcentralus
2026-11-04T18:37:26.7145555Z Cloud: Azure
2026-11-04T18:37:26.7146200Z ##[endgroup]
2026-11-04T18:37:26.7148074Z ##[group]Operating System
2026-11-04T18:37:26.7148837Z Ubuntu
2026-11-04T18:37:26.7149605Z 24.04.5
2026-11-04T18:37:26.7150140Z LTS
2026-11-04T18:37:26.7150787Z ##[endgroup]
2026-11-04T18:37:26.7151398Z ##[group]Runner Image
2026-11-04T18:37:26.7152067Z Image: ubuntu-24.04
2026-11-04T18:37:26.7152653Z Version: 20261025.353.1
2026-11-04T18:37:26.7154023Z Included Software: https://github.com/actions/runner-images/blob/ubuntu24/20261025.353/images/ubuntu/Ubuntu2404-Readme.md
2026-11-04T18:37:26.7155617Z Image Release: https://github.com/actions/runner-images/releases/tag/ubuntu24%2F20261025.353
2026-11-04T18:37:26.7156690Z ##[endgroup]
2026-11-04T18:37:26.7157829Z ##[group]GITHUB_TOKEN Permissions
2026-11-04T18:37:26.7160689Z Contents: read
2026-11-04T18:37:26.7161339Z Metadata: read
2026-11-04T18:37:26.7161907Z ##[endgroup]
2026-11-04T18:37:26.7164205Z Secret source: Actions
2026-11-04T18:37:26.7165223Z Cache mode: write
2026-11-04T18:37:26.7166036Z Prepare workflow directory
2026-11-04T18:37:26.9064613Z Prepare all required actions
2026-11-04T18:37:26.9112703Z Getting action download info
2026-11-04T18:37:27.2077068Z Download action repository 'actions/checkout@3d3c42e5aac5ba805825da76410c181273ba90b1' (SHA:3d3c42e5aac5ba805825da76410c181273ba90b1)
2026-11-04T18:37:27.3247823Z Download action repository 'actions/upload-artifact@cf430e030ddbb5b0abf93d22962f4752f3646cd9' (SHA:cf430e030ddbb5b0abf93d22962f4752f3646cd9)
2026-11-04T18:37:27.5603420Z Complete job name: 1 · Restauro e compilação (QG-01)
2026-11-04T18:37:27.7958234Z ##[group]Run actions/checkout@3d3c42e5aac5ba805825da76410c181273ba90b1
2026-11-04T18:37:27.7959699Z with:
2026-11-04T18:37:27.7960188Z   persist-credentials: false
2026-11-04T18:37:27.7960839Z   repository: LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:37:27.7964848Z   token: ***
2026-11-04T18:37:27.7965288Z   ssh-strict: true
2026-11-04T18:37:27.7965747Z   ssh-user: git
2026-11-04T18:37:27.7966168Z   clean: true
2026-11-04T18:37:27.7966635Z   sparse-checkout-cone-mode: true
2026-11-04T18:37:27.7967256Z   fetch-depth: 1
2026-11-04T18:37:27.7967999Z   fetch-tags: false
2026-11-04T18:37:27.7968710Z   show-progress: true
2026-11-04T18:37:27.7969687Z   lfs: false
2026-11-04T18:37:27.7970403Z   submodules: false
2026-11-04T18:37:27.7971136Z   set-safe-directory: true
2026-11-04T18:37:27.7971975Z   allow-unsafe-pr-checkout: false
2026-11-04T18:37:27.7973367Z env:
2026-11-04T18:37:27.7974108Z   DOTNET_NOLOGO: true
2026-11-04T18:37:27.7975010Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:37:27.7975931Z   CONFIGURACAO: Release
2026-11-04T18:37:27.7976498Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:37:27.7977444Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:37:27.7978784Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:37:27.7980146Z   RESULTADOS: resultados-ci
2026-11-04T18:37:27.7980670Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:37:27.7981229Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:37:27.7981784Z ##[endgroup]
2026-11-04T18:37:28.1122655Z Syncing repository: LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:37:28.1125163Z ##[group]Getting Git version info
2026-11-04T18:37:28.1126442Z Working directory is '/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO'
2026-11-04T18:37:28.1127718Z [command]/usr/bin/git version
2026-11-04T18:37:28.1228316Z git version 2.56.0
2026-11-04T18:37:28.1251963Z ##[endgroup]
2026-11-04T18:37:28.1263063Z Temporarily overriding HOME='/home/runner/work/_temp/70414d23-4d31-4183-aa30-0848df3e753b' before making global git config changes
2026-11-04T18:37:28.1265549Z Adding repository directory to the temporary git global config as a safe directory
2026-11-04T18:37:28.1270385Z [command]/usr/bin/git config --global --add safe.directory /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:37:28.1326371Z Deleting the contents of '/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO'
2026-11-04T18:37:28.1330386Z ##[group]Determining repository object format
2026-11-04T18:37:28.1332440Z ##[endgroup]
2026-11-04T18:37:28.1333744Z ##[group]Initializing the repository
2026-11-04T18:37:28.1341131Z [command]/usr/bin/git init /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:37:28.1479913Z hint: Using 'master' as the name for the initial branch. This default branch name
2026-11-04T18:37:28.1481116Z hint: will change to "main" in Git 3.0. To configure the initial branch name
2026-11-04T18:37:28.1482108Z hint: to use in all of your new repositories, which will suppress this warning,
2026-11-04T18:37:28.1482882Z hint: call:
2026-11-04T18:37:28.1483552Z hint:
2026-11-04T18:37:28.1484249Z hint: 	git config --global init.defaultBranch <name>
2026-11-04T18:37:28.1484985Z hint:
2026-11-04T18:37:28.1485817Z hint: Names commonly chosen instead of 'master' are 'main', 'trunk' and
2026-11-04T18:37:28.1486900Z hint: 'development'. The just-created branch can be renamed via this command:
2026-11-04T18:37:28.1487820Z hint:
2026-11-04T18:37:28.1488434Z hint: 	git branch -m <name>
2026-11-04T18:37:28.1489009Z hint:
2026-11-04T18:37:28.1490194Z hint: Disable this message with "git config set advice.defaultBranchName false"
2026-11-04T18:37:28.1505771Z Initialized empty Git repository in /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/
2026-11-04T18:37:28.1518862Z [command]/usr/bin/git remote add origin https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:37:28.1577931Z ##[endgroup]
2026-11-04T18:37:28.1580031Z ##[group]Disabling automatic garbage collection
2026-11-04T18:37:28.1583649Z [command]/usr/bin/git config --local gc.auto 0
2026-11-04T18:37:28.1621362Z ##[endgroup]
2026-11-04T18:37:28.1623176Z ##[group]Setting up auth
2026-11-04T18:37:28.1623935Z Removing SSH command configuration
2026-11-04T18:37:28.1628560Z [command]/usr/bin/git config --local --name-only --get-regexp core\.sshCommand
2026-11-04T18:37:28.1675264Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'core\.sshCommand' && git config --local --unset-all 'core.sshCommand' || :"
2026-11-04T18:37:28.2170032Z Removing HTTP extra header
2026-11-04T18:37:28.2244669Z [command]/usr/bin/git config --local --name-only --get-regexp http\.https\:\/\/github\.com\/\.extraheader
2026-11-04T18:37:28.2252169Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'http\.https\:\/\/github\.com\/\.extraheader' && git config --local --unset-all 'http.https://github.com/.extraheader' || :"
2026-11-04T18:37:28.2499440Z Removing includeIf entries pointing to credentials config files
2026-11-04T18:37:28.2505505Z [command]/usr/bin/git config --local --name-only --get-regexp ^includeIf\.gitdir:
2026-11-04T18:37:28.2550525Z [command]/usr/bin/git submodule foreach --recursive git config --local --show-origin --name-only --get-regexp remote.origin.url
2026-11-04T18:37:28.2795987Z [command]/usr/bin/git config --file /home/runner/work/_temp/git-credentials-019340a2-160e-4e64-afe8-a5ba7d2b601e.config http.https://github.com/.extraheader AUTHORIZATION: basic ***
2026-11-04T18:37:28.2847003Z [command]/usr/bin/git config --local includeIf.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path /home/runner/work/_temp/git-credentials-019340a2-160e-4e64-afe8-a5ba7d2b601e.config
2026-11-04T18:37:28.2892051Z [command]/usr/bin/git config --local includeIf.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path /home/runner/work/_temp/git-credentials-019340a2-160e-4e64-afe8-a5ba7d2b601e.config
2026-11-04T18:37:28.2955557Z [command]/usr/bin/git config --local includeIf.gitdir:/github/workspace/.git.path /github/runner_temp/git-credentials-019340a2-160e-4e64-afe8-a5ba7d2b601e.config
2026-11-04T18:37:28.3000464Z [command]/usr/bin/git config --local includeIf.gitdir:/github/workspace/.git/worktrees/*.path /github/runner_temp/git-credentials-019340a2-160e-4e64-afe8-a5ba7d2b601e.config
2026-11-04T18:37:28.3042383Z ##[endgroup]
2026-11-04T18:37:28.3043148Z ##[group]Fetching the repository
2026-11-04T18:37:28.3051370Z [command]/usr/bin/git -c protocol.version=2 fetch --no-tags --prune --no-recurse-submodules --depth=1 origin +89d6b31c5e788b4e8e5cfc7178d991ef65856c06:refs/remotes/origin/develop
2026-11-04T18:37:29.9628291Z From https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:37:29.9631521Z  * [new ref]         89d6b31c5e788b4e8e5cfc7178d991ef65856c06 -> origin/develop
2026-11-04T18:37:29.9639106Z [command]/usr/bin/git branch --list --remote origin/develop
2026-11-04T18:37:29.9678626Z   origin/develop
2026-11-04T18:37:29.9687935Z [command]/usr/bin/git rev-parse refs/remotes/origin/develop
2026-11-04T18:37:29.9725273Z 89d6b31c5e788b4e8e5cfc7178d991ef65856c06
2026-11-04T18:37:29.9730628Z ##[endgroup]
2026-11-04T18:37:29.9731618Z ##[group]Determining the checkout info
2026-11-04T18:37:29.9732592Z ##[endgroup]
2026-11-04T18:37:29.9737887Z [command]/usr/bin/git sparse-checkout disable
2026-11-04T18:37:29.9805768Z [command]/usr/bin/git config --local --unset-all extensions.worktreeConfig
2026-11-04T18:37:29.9851862Z ##[group]Checking out the ref
2026-11-04T18:37:29.9855785Z [command]/usr/bin/git checkout --progress --force -B develop refs/remotes/origin/develop
2026-11-04T18:37:30.1825094Z Switched to a new branch 'develop'
2026-11-04T18:37:30.1833988Z branch 'develop' set up to track 'origin/develop'.
2026-11-04T18:37:30.1862240Z ##[endgroup]
2026-11-04T18:37:30.1920960Z [command]/usr/bin/git log -1 --format=%H
2026-11-04T18:37:30.1962310Z 89d6b31c5e788b4e8e5cfc7178d991ef65856c06
2026-11-04T18:37:30.1973389Z ##[group]Removing auth
2026-11-04T18:37:30.1974084Z Removing SSH command configuration
2026-11-04T18:37:30.1977068Z [command]/usr/bin/git config --local --name-only --get-regexp core\.sshCommand
2026-11-04T18:37:30.2022261Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'core\.sshCommand' && git config --local --unset-all 'core.sshCommand' || :"
2026-11-04T18:37:30.2266899Z Removing HTTP extra header
2026-11-04T18:37:30.2270872Z [command]/usr/bin/git config --local --name-only --get-regexp http\.https\:\/\/github\.com\/\.extraheader
2026-11-04T18:37:30.2315477Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'http\.https\:\/\/github\.com\/\.extraheader' && git config --local --unset-all 'http.https://github.com/.extraheader' || :"
2026-11-04T18:37:30.2557997Z Removing includeIf entries pointing to credentials config files
2026-11-04T18:37:30.2572441Z [command]/usr/bin/git config --local --name-only --get-regexp ^includeIf\.gitdir:
2026-11-04T18:37:30.2614394Z includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path
2026-11-04T18:37:30.2618619Z includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path
2026-11-04T18:37:30.2621399Z includeif.gitdir:/github/workspace/.git.path
2026-11-04T18:37:30.2622811Z includeif.gitdir:/github/workspace/.git/worktrees/*.path
2026-11-04T18:37:30.2628270Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path
2026-11-04T18:37:30.2666491Z /home/runner/work/_temp/git-credentials-019340a2-160e-4e64-afe8-a5ba7d2b601e.config
2026-11-04T18:37:30.2678128Z [command]/usr/bin/git config --local --unset includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path \/home\/runner\/work\/_temp\/git\-credentials\-019340a2\-160e\-4e64\-afe8\-a5ba7d2b601e\.config
2026-11-04T18:37:30.2732599Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path
2026-11-04T18:37:30.2767362Z /home/runner/work/_temp/git-credentials-019340a2-160e-4e64-afe8-a5ba7d2b601e.config
2026-11-04T18:37:30.2777995Z [command]/usr/bin/git config --local --unset includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path \/home\/runner\/work\/_temp\/git\-credentials\-019340a2\-160e\-4e64\-afe8\-a5ba7d2b601e\.config
2026-11-04T18:37:30.2827299Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/github/workspace/.git.path
2026-11-04T18:37:30.2863430Z /github/runner_temp/git-credentials-019340a2-160e-4e64-afe8-a5ba7d2b601e.config
2026-11-04T18:37:30.2873585Z [command]/usr/bin/git config --local --unset includeif.gitdir:/github/workspace/.git.path \/github\/runner_temp\/git\-credentials\-019340a2\-160e\-4e64\-afe8\-a5ba7d2b601e\.config
2026-11-04T18:37:30.2919127Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/github/workspace/.git/worktrees/*.path
2026-11-04T18:37:30.2954491Z /github/runner_temp/git-credentials-019340a2-160e-4e64-afe8-a5ba7d2b601e.config
2026-11-04T18:37:30.2964368Z [command]/usr/bin/git config --local --unset includeif.gitdir:/github/workspace/.git/worktrees/*.path \/github\/runner_temp\/git\-credentials\-019340a2\-160e\-4e64\-afe8\-a5ba7d2b601e\.config
2026-11-04T18:37:30.3009593Z [command]/usr/bin/git submodule foreach --recursive git config --local --show-origin --name-only --get-regexp remote.origin.url
2026-11-04T18:37:30.3256087Z Removing credentials config '/home/runner/work/_temp/git-credentials-019340a2-160e-4e64-afe8-a5ba7d2b601e.config'
2026-11-04T18:37:30.3265797Z ##[endgroup]
2026-11-04T18:37:30.4083583Z Prepare all required actions
2026-11-04T18:37:30.4084856Z Getting action download info
2026-11-04T18:37:30.5458088Z Download action repository 'actions/setup-dotnet@a98b56852c35b8e3190ac28c8c2271da59106c68' (SHA:a98b56852c35b8e3190ac28c8c2271da59106c68)
2026-11-04T18:37:31.0643413Z Download action repository 'actions/cache@55cc8345863c7cc4c66a329aec7e433d2d1c52a9' (SHA:55cc8345863c7cc4c66a329aec7e433d2d1c52a9)
2026-11-04T18:37:31.2711703Z ##[group]Run ./.github/actions/preparar-dotnet
2026-11-04T18:37:31.2713000Z env:
2026-11-04T18:37:31.2713875Z   DOTNET_NOLOGO: true
2026-11-04T18:37:31.2714900Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:37:31.2716046Z   CONFIGURACAO: Release
2026-11-04T18:37:31.2717158Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:37:31.2719798Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:37:31.2722824Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:37:31.2725055Z   RESULTADOS: resultados-ci
2026-11-04T18:37:31.2726152Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:37:31.2727346Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:37:31.2728537Z ##[endgroup]
2026-11-04T18:37:31.2912522Z ##[start-action display=Instalar o SDK do .NET 10;id=__self.__actions_setup-dotnet]
2026-11-04T18:37:31.3767061Z ##[group]Run actions/setup-dotnet@a98b56852c35b8e3190ac28c8c2271da59106c68
2026-11-04T18:37:31.3768559Z with:
2026-11-04T18:37:31.3769558Z   dotnet-version: 10.0.x
2026-11-04T18:37:31.3770535Z   cache: false
2026-11-04T18:37:31.3771366Z env:
2026-11-04T18:37:31.3772193Z   DOTNET_NOLOGO: true
2026-11-04T18:37:31.3773179Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:37:31.3774276Z   CONFIGURACAO: Release
2026-11-04T18:37:31.3775335Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:37:31.3777306Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:37:31.3780455Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:37:31.3782593Z   RESULTADOS: resultados-ci
2026-11-04T18:37:31.3783651Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:37:31.3784791Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:37:31.3785945Z ##[endgroup]
2026-11-04T18:37:31.5284395Z [command]/home/runner/work/_actions/actions/setup-dotnet/a98b56852c35b8e3190ac28c8c2271da59106c68/externals/install-dotnet.sh --skip-non-versioned-files --runtime dotnet --channel LTS
2026-11-04T18:37:31.8748629Z dotnet-install: .NET Core Runtime with version '10.0.14' is already installed.
2026-11-04T18:37:31.8774562Z [command]/home/runner/work/_actions/actions/setup-dotnet/a98b56852c35b8e3190ac28c8c2271da59106c68/externals/install-dotnet.sh --skip-non-versioned-files --channel 10.0
2026-11-04T18:37:32.1819098Z dotnet-install: .NET Core SDK with version '10.0.403' is already installed.
2026-11-04T18:37:32.1969756Z ##[end-action id=__self.__actions_setup-dotnet;outcome=success;conclusion=success;duration_ms=905]
2026-11-04T18:37:32.2174394Z ##[start-action display=Reutilizar a cache dos pacotes NuGet;id=__self.__actions_cache]
2026-11-04T18:37:32.6535536Z ##[group]Run actions/cache@55cc8345863c7cc4c66a329aec7e433d2d1c52a9
2026-11-04T18:37:32.6536567Z with:
2026-11-04T18:37:32.6537377Z   path: ~/.nuget/packages
2026-11-04T18:37:32.6538478Z   key: nuget-Linux-c8252acdcad9b687c3131c725ee3c1b10105ceb23478247a43e8389c12c19830
2026-11-04T18:37:32.6539761Z   restore-keys: nuget-Linux-

2026-11-04T18:37:32.6540667Z   enableCrossOsArchive: false
2026-11-04T18:37:32.6541551Z   fail-on-cache-miss: false
2026-11-04T18:37:32.6542415Z   lookup-only: false
2026-11-04T18:37:32.6543241Z   save-always: false
2026-11-04T18:37:32.6544066Z env:
2026-11-04T18:37:32.6544868Z   DOTNET_NOLOGO: true
2026-11-04T18:37:32.6545729Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:37:32.6546652Z   CONFIGURACAO: Release
2026-11-04T18:37:32.6547895Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:37:32.6549291Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:37:32.6550933Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:37:32.6552213Z   RESULTADOS: resultados-ci
2026-11-04T18:37:32.6553093Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:37:32.6553999Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:37:32.6554980Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:37:32.6555848Z ##[endgroup]
2026-11-04T18:37:32.9632636Z Cache hit for: nuget-Linux-c8252acdcad9b687c3131c725ee3c1b10105ceb23478247a43e8389c12c19830
2026-11-04T18:37:34.1879870Z Received 58720256 of 283633656 (20.7%), 55.9 MBs/sec
2026-11-04T18:37:35.1880668Z Received 234881024 of 283633656 (82.8%), 111.9 MBs/sec
2026-11-04T18:37:35.5973149Z Received 283633656 of 283633656 (100.0%), 112.1 MBs/sec
2026-11-04T18:37:35.5975604Z Cache Size: ~270 MB (283633656 B)
2026-11-04T18:37:35.6110999Z [command]/usr/bin/tar -xf /home/runner/work/_temp/0e12d338-f57d-4c46-b8a9-9cef6dace488/cache.tzst -P -C /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO --use-compress-program unzstd
2026-11-04T18:37:36.8948353Z Cache restored successfully
2026-11-04T18:37:36.9310537Z Cache restored from key: nuget-Linux-c8252acdcad9b687c3131c725ee3c1b10105ceb23478247a43e8389c12c19830
2026-11-04T18:37:36.9437876Z ##[end-action id=__self.__actions_cache;outcome=success;conclusion=success;duration_ms=4726]
2026-11-04T18:37:36.9501478Z ##[start-action display=Registar o SDK utilizado;id=__self.__run]
2026-11-04T18:37:36.9641120Z ##[group]Run echo "SDK do .NET: $(dotnet --version)"
2026-11-04T18:37:36.9641612Z [36;1mecho "SDK do .NET: $(dotnet --version)"[0m
2026-11-04T18:37:36.9641941Z [36;1m{[0m
2026-11-04T18:37:36.9642174Z [36;1m  echo "### Ambiente .NET"[0m
2026-11-04T18:37:36.9642467Z [36;1m  echo ""[0m
2026-11-04T18:37:36.9642956Z [36;1m  echo "| Elemento | Valor |"[0m
2026-11-04T18:37:36.9643257Z [36;1m  echo "|---|---|"[0m
2026-11-04T18:37:36.9643583Z [36;1m  echo "| SDK do .NET | \`$(dotnet --version)\` |"[0m
2026-11-04T18:37:36.9644061Z [36;1m  echo "| global.json | \`$(tr -d '\n\r ' < src/backend/global.json)\` |"[0m
2026-11-04T18:37:36.9644567Z [36;1m  echo "| Runner | \`${RUNNER_OS} ${ImageOS:-} ${ImageVersion:-}\` |"[0m
2026-11-04T18:37:36.9644960Z [36;1m  echo ""[0m
2026-11-04T18:37:36.9645213Z [36;1m} >> "$GITHUB_STEP_SUMMARY"[0m
2026-11-04T18:37:36.9670634Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:37:36.9671027Z env:
2026-11-04T18:37:36.9671246Z   DOTNET_NOLOGO: true
2026-11-04T18:37:36.9671509Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:37:36.9671803Z   CONFIGURACAO: Release
2026-11-04T18:37:36.9672095Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:37:36.9672633Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:37:36.9673450Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:37:36.9674028Z   RESULTADOS: resultados-ci
2026-11-04T18:37:36.9674305Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:37:36.9674605Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:37:36.9674928Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:37:36.9675225Z ##[endgroup]
2026-11-04T18:37:38.6506537Z SDK do .NET: 10.0.403
2026-11-04T18:37:38.7413679Z ##[end-action id=__self.__run;outcome=success;conclusion=success;duration_ms=1791]
2026-11-04T18:37:38.7509684Z ##[group]Run dotnet restore "$SOLUCAO"
2026-11-04T18:37:38.7510064Z [36;1mdotnet restore "$SOLUCAO"[0m
2026-11-04T18:37:38.7530462Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:37:38.7530817Z env:
2026-11-04T18:37:38.7531016Z   DOTNET_NOLOGO: true
2026-11-04T18:37:38.7531265Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:37:38.7531550Z   CONFIGURACAO: Release
2026-11-04T18:37:38.7531814Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:37:38.7532334Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:37:38.7533106Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:37:38.7533665Z   RESULTADOS: resultados-ci
2026-11-04T18:37:38.7533914Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:37:38.7534199Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:37:38.7534489Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:37:38.7534765Z ##[endgroup]
2026-11-04T18:37:42.1072612Z   Determining projects to restore...
2026-11-04T18:37:44.3543172Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Application/ClinicaVeterinaria.Application.csproj (in 1.03 sec).
2026-11-04T18:37:44.3831896Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Infrastructure/ClinicaVeterinaria.Infrastructure.csproj (in 1.07 sec).
2026-11-04T18:37:44.3921189Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Domain/ClinicaVeterinaria.Domain.csproj (in 2 ms).
2026-11-04T18:37:44.4057236Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj (in 33 ms).
2026-11-04T18:37:44.4101694Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj (in 1.11 sec).
2026-11-04T18:37:44.4201453Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj (in 1.14 sec).
2026-11-04T18:37:44.4900719Z ##[group]Run mkdir -p "$GITHUB_WORKSPACE/$RESULTADOS/analise-estatica"
2026-11-04T18:37:44.4901467Z [36;1mmkdir -p "$GITHUB_WORKSPACE/$RESULTADOS/analise-estatica"[0m
2026-11-04T18:37:44.4901842Z [36;1mdotnet build "$SOLUCAO" \[0m
2026-11-04T18:37:44.4902157Z [36;1m  --configuration "$CONFIGURACAO" \[0m
2026-11-04T18:37:44.4902458Z [36;1m  --no-restore \[0m
2026-11-04T18:37:44.4902738Z [36;1m  -p:ContinuousIntegrationBuild=true \[0m
2026-11-04T18:37:44.4903206Z [36;1m  -p:PastaRelatoriosAnalise="$GITHUB_WORKSPACE/$RESULTADOS/analise-estatica"[0m
2026-11-04T18:37:44.4923715Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:37:44.4924070Z env:
2026-11-04T18:37:44.4924271Z   DOTNET_NOLOGO: true
2026-11-04T18:37:44.4924508Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:37:44.4924784Z   CONFIGURACAO: Release
2026-11-04T18:37:44.4925036Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:37:44.4925592Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:37:44.4926377Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:37:44.4926951Z   RESULTADOS: resultados-ci
2026-11-04T18:37:44.4927211Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:37:44.4927488Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:37:44.4927777Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:37:44.4928018Z ##[endgroup]
2026-11-04T18:37:50.9145848Z   ClinicaVeterinaria.Domain -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Domain/bin/Release/net10.0/ClinicaVeterinaria.Domain.dll
2026-11-04T18:37:53.2613681Z   ClinicaVeterinaria.Application -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Application/bin/Release/net10.0/ClinicaVeterinaria.Application.dll
2026-11-04T18:37:55.8922066Z   ClinicaVeterinaria.Infrastructure -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Infrastructure/bin/Release/net10.0/ClinicaVeterinaria.Infrastructure.dll
2026-11-04T18:37:59.1096470Z   ClinicaVeterinaria.Api -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Api/bin/Release/net10.0/ClinicaVeterinaria.Api.dll
2026-11-04T18:38:08.4110217Z   ClinicaVeterinaria.UnitTests -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.UnitTests/bin/Release/net10.0/ClinicaVeterinaria.UnitTests.dll
2026-11-04T18:38:08.6204981Z   ClinicaVeterinaria.IntegrationTests -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.IntegrationTests/bin/Release/net10.0/ClinicaVeterinaria.IntegrationTests.dll
2026-11-04T18:38:08.7139427Z 
2026-11-04T18:38:08.7149568Z Build succeeded.
2026-11-04T18:38:08.7150593Z     0 Warning(s)
2026-11-04T18:38:08.7151498Z     0 Error(s)
2026-11-04T18:38:08.7151783Z 
2026-11-04T18:38:08.7152011Z Time Elapsed 00:00:23.96
2026-11-04T18:38:08.7557434Z ##[group]Run {
2026-11-04T18:38:08.7557928Z [36;1m{[0m
2026-11-04T18:38:08.7558414Z [36;1m  echo "### 1 · Restauro e compilação (QG-01)"[0m
2026-11-04T18:38:08.7559008Z [36;1m  echo ""[0m
2026-11-04T18:38:08.7560312Z [36;1m  echo "✅ A solução \`$SOLUCAO\` foi restaurada e compilada em \`$CONFIGURACAO\` sem erros nem avisos (quality gate da I101)."[0m
2026-11-04T18:38:08.7561402Z [36;1m  echo ""[0m
2026-11-04T18:38:08.7561809Z [36;1m} >> "$GITHUB_STEP_SUMMARY"[0m
2026-11-04T18:38:08.7591145Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:38:08.7591803Z env:
2026-11-04T18:38:08.7592180Z   DOTNET_NOLOGO: true
2026-11-04T18:38:08.7592657Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:38:08.7593193Z   CONFIGURACAO: Release
2026-11-04T18:38:08.7593697Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:38:08.7594705Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:38:08.7596304Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:38:08.7597791Z   RESULTADOS: resultados-ci
2026-11-04T18:38:08.7598278Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:38:08.7598832Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:38:08.7599755Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:38:08.7600245Z ##[endgroup]
2026-11-04T18:38:08.7922823Z ##[group]Run actions/upload-artifact@cf430e030ddbb5b0abf93d22962f4752f3646cd9
2026-11-04T18:38:08.7923263Z with:
2026-11-04T18:38:08.7923493Z   name: relatorio-analise-estatica-sarif
2026-11-04T18:38:08.7923811Z   path: resultados-ci/analise-estatica
2026-11-04T18:38:08.7924090Z   if-no-files-found: warn
2026-11-04T18:38:08.7924327Z   retention-days: 90
2026-11-04T18:38:08.7924545Z   compression-level: 6
2026-11-04T18:38:08.7924772Z   overwrite: false
2026-11-04T18:38:08.7924990Z   include-hidden-files: false
2026-11-04T18:38:08.7925230Z   archive: true
2026-11-04T18:38:08.7925424Z env:
2026-11-04T18:38:08.7925628Z   DOTNET_NOLOGO: true
2026-11-04T18:38:08.7925858Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:38:08.7926119Z   CONFIGURACAO: Release
2026-11-04T18:38:08.7926374Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:38:08.7926880Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:38:08.7927692Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:38:08.7928284Z   RESULTADOS: resultados-ci
2026-11-04T18:38:08.7928695Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:38:08.7928970Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:38:08.7929258Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:38:08.7929662Z ##[endgroup]
2026-11-04T18:38:09.0660937Z With the provided path, there will be 6 files uploaded
2026-11-04T18:38:09.0691100Z Artifact name is valid!
2026-11-04T18:38:09.0714853Z Root directory input is valid!
2026-11-04T18:38:09.4560609Z Uploading artifact: relatorio-analise-estatica-sarif.zip
2026-11-04T18:38:09.4590025Z Beginning upload of artifact content to blob storage
2026-11-04T18:38:10.2303632Z Uploaded bytes 306190
2026-11-04T18:38:10.2891391Z Finished uploading artifact content to blob storage!
2026-11-04T18:38:10.2892705Z SHA256 digest of uploaded artifact is 11302906f96fa650fa4c09dc73db5498e8ddb7ec7025732b7e19aa9046541ec2
2026-11-04T18:38:10.2893430Z Finalizing artifact upload
2026-11-04T18:38:10.8690912Z Artifact relatorio-analise-estatica-sarif successfully finalized. Artifact ID 13351849014
2026-11-04T18:38:10.8692406Z Artifact relatorio-analise-estatica-sarif has been successfully uploaded! Final size is 306190 bytes. Artifact ID is 13351849014
2026-11-04T18:38:10.8697259Z Artifact download URL: https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/actions/runs/41888968713/artifacts/13351849014
2026-11-04T18:38:11.0201868Z Post job cleanup.
2026-11-04T18:38:11.0234234Z ##[start-action display=Reutilizar a cache dos pacotes NuGet;id=__044673c4-30c6-4e6a-806c-03b7dac783a3.__actions_cache]
2026-11-04T18:38:11.8203519Z Post job cleanup.
2026-11-04T18:38:11.9530014Z Cache hit occurred on the primary key nuget-Linux-c8252acdcad9b687c3131c725ee3c1b10105ceb23478247a43e8389c12c19830, not saving cache.
2026-11-04T18:38:11.9597637Z ##[end-action id=__044673c4-30c6-4e6a-806c-03b7dac783a3.__actions_cache;outcome=success;conclusion=success;duration_ms=936]
2026-11-04T18:38:11.9629122Z ##[start-action display=Instalar o SDK do .NET 10;id=__044673c4-30c6-4e6a-806c-03b7dac783a3.__actions_setup-dotnet]
2026-11-04T18:38:11.9854356Z Post job cleanup.
2026-11-04T18:38:12.1200845Z ##[end-action id=__044673c4-30c6-4e6a-806c-03b7dac783a3.__actions_setup-dotnet;outcome=success;conclusion=success;duration_ms=156]
2026-11-04T18:38:12.1867809Z Post job cleanup.
2026-11-04T18:38:12.2890221Z [command]/usr/bin/git version
2026-11-04T18:38:12.2952584Z git version 2.56.0
2026-11-04T18:38:12.3007755Z Temporarily overriding HOME='/home/runner/work/_temp/b8f030a3-d569-45e9-be0a-e9c9524294d0' before making global git config changes
2026-11-04T18:38:12.3010095Z Adding repository directory to the temporary git global config as a safe directory
2026-11-04T18:38:12.3017724Z [command]/usr/bin/git config --global --add safe.directory /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:38:12.3092870Z Removing SSH command configuration
2026-11-04T18:38:12.3103832Z [command]/usr/bin/git config --local --name-only --get-regexp core\.sshCommand
2026-11-04T18:38:12.3162319Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'core\.sshCommand' && git config --local --unset-all 'core.sshCommand' || :"
2026-11-04T18:38:12.3554456Z Removing HTTP extra header
2026-11-04T18:38:12.3562560Z [command]/usr/bin/git config --local --name-only --get-regexp http\.https\:\/\/github\.com\/\.extraheader
2026-11-04T18:38:12.3645784Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'http\.https\:\/\/github\.com\/\.extraheader' && git config --local --unset-all 'http.https://github.com/.extraheader' || :"
2026-11-04T18:38:12.3971452Z Removing includeIf entries pointing to credentials config files
2026-11-04T18:38:12.3972407Z [command]/usr/bin/git config --local --name-only --get-regexp ^includeIf\.gitdir:
2026-11-04T18:38:12.4057963Z [command]/usr/bin/git submodule foreach --recursive git config --local --show-origin --name-only --get-regexp remote.origin.url
2026-11-04T18:38:12.4615684Z Cleaning up orphan processes
2026-11-04T18:38:12.5028208Z Terminate orphan process: pid (2783) (dotnet)
2026-11-04T18:38:12.5081076Z Terminate orphan process: pid (2784) (dotnet)
2026-11-04T18:38:12.5122188Z Terminate orphan process: pid (2785) (dotnet)
2026-11-04T18:38:12.5167513Z Terminate orphan process: pid (2894) (VBCSCompiler)
``````
