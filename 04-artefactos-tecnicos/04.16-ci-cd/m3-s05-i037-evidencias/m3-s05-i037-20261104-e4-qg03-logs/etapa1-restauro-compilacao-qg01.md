``````text
﻿2026-11-04T18:54:50.6395682Z Current runner version: '2.338.2'
2026-11-04T18:54:50.6417055Z ##[group]Runner Image Provisioner
2026-11-04T18:54:50.6417905Z Hosted Compute Agent
2026-11-04T18:54:50.6418406Z Version: 20261021.618
2026-11-04T18:54:50.6418927Z Commit: 21505ad2541d5be6135ed84fc33697146cf75b8e
2026-11-04T18:54:50.6419626Z Build Date: 2026-10-21T00:45:21Z
2026-11-04T18:54:50.6420199Z Worker ID: {2363f61b-15ed-4a26-b3fa-59e3f9be8779}
2026-11-04T18:54:50.6420859Z Region: westus3
2026-11-04T18:54:50.6421343Z Cloud: Azure
2026-11-04T18:54:50.6421800Z ##[endgroup]
2026-11-04T18:54:50.6423113Z ##[group]Operating System
2026-11-04T18:54:50.6423693Z Ubuntu
2026-11-04T18:54:50.6424193Z 24.04.5
2026-11-04T18:54:50.6424652Z LTS
2026-11-04T18:54:50.6425125Z ##[endgroup]
2026-11-04T18:54:50.6425642Z ##[group]Runner Image
2026-11-04T18:54:50.6426162Z Image: ubuntu-24.04
2026-11-04T18:54:50.6426631Z Version: 20261025.353.1
2026-11-04T18:54:50.6427746Z Included Software: https://github.com/actions/runner-images/blob/ubuntu24/20261025.353/images/ubuntu/Ubuntu2404-Readme.md
2026-11-04T18:54:50.6429048Z Image Release: https://github.com/actions/runner-images/releases/tag/ubuntu24%2F20261025.353
2026-11-04T18:54:50.6429830Z ##[endgroup]
2026-11-04T18:54:50.6430816Z ##[group]GITHUB_TOKEN Permissions
2026-11-04T18:54:50.6432399Z Contents: read
2026-11-04T18:54:50.6433098Z Metadata: read
2026-11-04T18:54:50.6433678Z ##[endgroup]
2026-11-04T18:54:50.6435469Z Secret source: Actions
2026-11-04T18:54:50.6436309Z Cache mode: write
2026-11-04T18:54:50.6436929Z Prepare workflow directory
2026-11-04T18:54:50.6767353Z Prepare all required actions
2026-11-04T18:54:50.6808258Z Getting action download info
2026-11-04T18:54:51.0047998Z Download action repository 'actions/checkout@3d3c42e5aac5ba805825da76410c181273ba90b1' (SHA:3d3c42e5aac5ba805825da76410c181273ba90b1)
2026-11-04T18:54:51.1874517Z Download action repository 'actions/upload-artifact@cf430e030ddbb5b0abf93d22962f4752f3646cd9' (SHA:cf430e030ddbb5b0abf93d22962f4752f3646cd9)
2026-11-04T18:54:51.3671176Z Complete job name: 1 · Restauro e compilação (QG-01)
2026-11-04T18:54:51.4382709Z ##[group]Run actions/checkout@3d3c42e5aac5ba805825da76410c181273ba90b1
2026-11-04T18:54:51.4383721Z with:
2026-11-04T18:54:51.4384084Z   persist-credentials: false
2026-11-04T18:54:51.4384598Z   repository: LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:54:51.4387874Z   token: ***
2026-11-04T18:54:51.4388231Z   ssh-strict: true
2026-11-04T18:54:51.4388590Z   ssh-user: git
2026-11-04T18:54:51.4388940Z   clean: true
2026-11-04T18:54:51.4389315Z   sparse-checkout-cone-mode: true
2026-11-04T18:54:51.4389746Z   fetch-depth: 1
2026-11-04T18:54:51.4390105Z   fetch-tags: false
2026-11-04T18:54:51.4390471Z   show-progress: true
2026-11-04T18:54:51.4390839Z   lfs: false
2026-11-04T18:54:51.4391185Z   submodules: false
2026-11-04T18:54:51.4391553Z   set-safe-directory: true
2026-11-04T18:54:51.4391963Z   allow-unsafe-pr-checkout: false
2026-11-04T18:54:51.4392572Z env:
2026-11-04T18:54:51.4393007Z   DOTNET_NOLOGO: true
2026-11-04T18:54:51.4393463Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:54:51.4393899Z   CONFIGURACAO: Release
2026-11-04T18:54:51.4394311Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:54:51.4395053Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:54:51.4396118Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:54:51.4396919Z   RESULTADOS: resultados-ci
2026-11-04T18:54:51.4397342Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:54:51.4397780Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:54:51.4398224Z ##[endgroup]
2026-11-04T18:54:51.7047430Z Syncing repository: LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:54:51.7049178Z ##[group]Getting Git version info
2026-11-04T18:54:51.7050064Z Working directory is '/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO'
2026-11-04T18:54:51.7051709Z [command]/usr/bin/git version
2026-11-04T18:54:51.7184657Z git version 2.56.0
2026-11-04T18:54:51.7207552Z ##[endgroup]
2026-11-04T18:54:51.7240547Z Temporarily overriding HOME='/home/runner/work/_temp/24b977e8-8cb6-4e7a-b7cb-8258b1862621' before making global git config changes
2026-11-04T18:54:51.7242968Z Adding repository directory to the temporary git global config as a safe directory
2026-11-04T18:54:51.7285246Z [command]/usr/bin/git config --global --add safe.directory /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:54:51.7329625Z Deleting the contents of '/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO'
2026-11-04T18:54:51.7342158Z ##[group]Determining repository object format
2026-11-04T18:54:51.7356479Z ##[endgroup]
2026-11-04T18:54:51.7357709Z ##[group]Initializing the repository
2026-11-04T18:54:51.7359170Z [command]/usr/bin/git init /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:54:51.7481045Z hint: Using 'master' as the name for the initial branch. This default branch name
2026-11-04T18:54:51.7483314Z hint: will change to "main" in Git 3.0. To configure the initial branch name
2026-11-04T18:54:51.7484919Z hint: to use in all of your new repositories, which will suppress this warning,
2026-11-04T18:54:51.7486133Z hint: call:
2026-11-04T18:54:51.7486852Z hint:
2026-11-04T18:54:51.7487726Z hint: 	git config --global init.defaultBranch <name>
2026-11-04T18:54:51.7488711Z hint:
2026-11-04T18:54:51.7489657Z hint: Names commonly chosen instead of 'master' are 'main', 'trunk' and
2026-11-04T18:54:51.7491103Z hint: 'development'. The just-created branch can be renamed via this command:
2026-11-04T18:54:51.7492351Z hint:
2026-11-04T18:54:51.7493284Z hint: 	git branch -m <name>
2026-11-04T18:54:51.7494111Z hint:
2026-11-04T18:54:51.7495120Z hint: Disable this message with "git config set advice.defaultBranchName false"
2026-11-04T18:54:51.7534313Z Initialized empty Git repository in /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/
2026-11-04T18:54:51.7565091Z [command]/usr/bin/git remote add origin https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:54:51.7915277Z ##[endgroup]
2026-11-04T18:54:51.7917562Z ##[group]Disabling automatic garbage collection
2026-11-04T18:54:51.7918903Z [command]/usr/bin/git config --local gc.auto 0
2026-11-04T18:54:51.7967410Z ##[endgroup]
2026-11-04T18:54:51.7968144Z ##[group]Setting up auth
2026-11-04T18:54:51.7968646Z Removing SSH command configuration
2026-11-04T18:54:51.7973454Z [command]/usr/bin/git config --local --name-only --get-regexp core\.sshCommand
2026-11-04T18:54:51.8017584Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'core\.sshCommand' && git config --local --unset-all 'core.sshCommand' || :"
2026-11-04T18:54:51.8361143Z Removing HTTP extra header
2026-11-04T18:54:51.8366279Z [command]/usr/bin/git config --local --name-only --get-regexp http\.https\:\/\/github\.com\/\.extraheader
2026-11-04T18:54:51.8406652Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'http\.https\:\/\/github\.com\/\.extraheader' && git config --local --unset-all 'http.https://github.com/.extraheader' || :"
2026-11-04T18:54:51.8619078Z Removing includeIf entries pointing to credentials config files
2026-11-04T18:54:51.8624829Z [command]/usr/bin/git config --local --name-only --get-regexp ^includeIf\.gitdir:
2026-11-04T18:54:51.8666945Z [command]/usr/bin/git submodule foreach --recursive git config --local --show-origin --name-only --get-regexp remote.origin.url
2026-11-04T18:54:51.8900848Z [command]/usr/bin/git config --file /home/runner/work/_temp/git-credentials-87e48186-2a9f-4671-a166-37562726307a.config http.https://github.com/.extraheader AUTHORIZATION: basic ***
2026-11-04T18:54:51.9021498Z [command]/usr/bin/git config --local includeIf.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path /home/runner/work/_temp/git-credentials-87e48186-2a9f-4671-a166-37562726307a.config
2026-11-04T18:54:51.9074492Z [command]/usr/bin/git config --local includeIf.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path /home/runner/work/_temp/git-credentials-87e48186-2a9f-4671-a166-37562726307a.config
2026-11-04T18:54:51.9115316Z [command]/usr/bin/git config --local includeIf.gitdir:/github/workspace/.git.path /github/runner_temp/git-credentials-87e48186-2a9f-4671-a166-37562726307a.config
2026-11-04T18:54:51.9194156Z [command]/usr/bin/git config --local includeIf.gitdir:/github/workspace/.git/worktrees/*.path /github/runner_temp/git-credentials-87e48186-2a9f-4671-a166-37562726307a.config
2026-11-04T18:54:51.9305630Z ##[endgroup]
2026-11-04T18:54:51.9306366Z ##[group]Fetching the repository
2026-11-04T18:54:51.9320896Z [command]/usr/bin/git -c protocol.version=2 fetch --no-tags --prune --no-recurse-submodules --depth=1 origin +904ebe1bb41fb9038081be81330a0758f3f6bae5:refs/remotes/pull/249/merge
2026-11-04T18:54:53.7374335Z From https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:54:53.7376069Z  * [new ref]         904ebe1bb41fb9038081be81330a0758f3f6bae5 -> pull/249/merge
2026-11-04T18:54:53.7378734Z ##[endgroup]
2026-11-04T18:54:53.7379500Z ##[group]Determining the checkout info
2026-11-04T18:54:53.7380277Z ##[endgroup]
2026-11-04T18:54:53.7384023Z [command]/usr/bin/git sparse-checkout disable
2026-11-04T18:54:53.7446204Z [command]/usr/bin/git config --local --unset-all extensions.worktreeConfig
2026-11-04T18:54:53.7485615Z ##[group]Checking out the ref
2026-11-04T18:54:53.7489078Z [command]/usr/bin/git checkout --progress --force refs/remotes/pull/249/merge
2026-11-04T18:54:53.9161904Z Note: switching to 'refs/remotes/pull/249/merge'.
2026-11-04T18:54:53.9162451Z 
2026-11-04T18:54:53.9162795Z You are in 'detached HEAD' state. You can look around, make experimental
2026-11-04T18:54:53.9163738Z changes and commit them, and you can discard any commits you make in this
2026-11-04T18:54:53.9164887Z state without impacting any branches by switching back to a branch.
2026-11-04T18:54:53.9165382Z 
2026-11-04T18:54:53.9165774Z If you want to create a new branch to retain commits you create, you may
2026-11-04T18:54:53.9166496Z do so (now or later) by using -c with the switch command. Example:
2026-11-04T18:54:53.9166888Z 
2026-11-04T18:54:53.9167083Z   git switch -c <new-branch-name>
2026-11-04T18:54:53.9167368Z 
2026-11-04T18:54:53.9167605Z Or undo this operation with:
2026-11-04T18:54:53.9167942Z 
2026-11-04T18:54:53.9168169Z   git switch -
2026-11-04T18:54:53.9168494Z 
2026-11-04T18:54:53.9168899Z Turn off this advice by setting config variable advice.detachedHead to false
2026-11-04T18:54:53.9169430Z 
2026-11-04T18:54:53.9169996Z HEAD is now at 904ebe1 Merge 6852c4461708be065c45ec293936ef6fea1e4a53 into 89d6b31c5e788b4e8e5cfc7178d991ef65856c06
2026-11-04T18:54:53.9191404Z ##[endgroup]
2026-11-04T18:54:53.9245109Z [command]/usr/bin/git log -1 --format=%H
2026-11-04T18:54:53.9279325Z 904ebe1bb41fb9038081be81330a0758f3f6bae5
2026-11-04T18:54:53.9287250Z ##[group]Removing auth
2026-11-04T18:54:53.9287779Z Removing SSH command configuration
2026-11-04T18:54:53.9290501Z [command]/usr/bin/git config --local --name-only --get-regexp core\.sshCommand
2026-11-04T18:54:53.9328778Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'core\.sshCommand' && git config --local --unset-all 'core.sshCommand' || :"
2026-11-04T18:54:53.9540803Z Removing HTTP extra header
2026-11-04T18:54:53.9544438Z [command]/usr/bin/git config --local --name-only --get-regexp http\.https\:\/\/github\.com\/\.extraheader
2026-11-04T18:54:53.9584012Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'http\.https\:\/\/github\.com\/\.extraheader' && git config --local --unset-all 'http.https://github.com/.extraheader' || :"
2026-11-04T18:54:53.9795610Z Removing includeIf entries pointing to credentials config files
2026-11-04T18:54:53.9798602Z [command]/usr/bin/git config --local --name-only --get-regexp ^includeIf\.gitdir:
2026-11-04T18:54:53.9834519Z includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path
2026-11-04T18:54:53.9835398Z includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path
2026-11-04T18:54:53.9836065Z includeif.gitdir:/github/workspace/.git.path
2026-11-04T18:54:53.9836680Z includeif.gitdir:/github/workspace/.git/worktrees/*.path
2026-11-04T18:54:53.9842317Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path
2026-11-04T18:54:53.9873629Z /home/runner/work/_temp/git-credentials-87e48186-2a9f-4671-a166-37562726307a.config
2026-11-04T18:54:53.9883879Z [command]/usr/bin/git config --local --unset includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path \/home\/runner\/work\/_temp\/git\-credentials\-87e48186\-2a9f\-4671\-a166\-37562726307a\.config
2026-11-04T18:54:53.9928705Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path
2026-11-04T18:54:53.9958645Z /home/runner/work/_temp/git-credentials-87e48186-2a9f-4671-a166-37562726307a.config
2026-11-04T18:54:53.9966622Z [command]/usr/bin/git config --local --unset includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path \/home\/runner\/work\/_temp\/git\-credentials\-87e48186\-2a9f\-4671\-a166\-37562726307a\.config
2026-11-04T18:54:54.0010778Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/github/workspace/.git.path
2026-11-04T18:54:54.0041335Z /github/runner_temp/git-credentials-87e48186-2a9f-4671-a166-37562726307a.config
2026-11-04T18:54:54.0048585Z [command]/usr/bin/git config --local --unset includeif.gitdir:/github/workspace/.git.path \/github\/runner_temp\/git\-credentials\-87e48186\-2a9f\-4671\-a166\-37562726307a\.config
2026-11-04T18:54:54.0087902Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/github/workspace/.git/worktrees/*.path
2026-11-04T18:54:54.0119326Z /github/runner_temp/git-credentials-87e48186-2a9f-4671-a166-37562726307a.config
2026-11-04T18:54:54.0126890Z [command]/usr/bin/git config --local --unset includeif.gitdir:/github/workspace/.git/worktrees/*.path \/github\/runner_temp\/git\-credentials\-87e48186\-2a9f\-4671\-a166\-37562726307a\.config
2026-11-04T18:54:54.0166632Z [command]/usr/bin/git submodule foreach --recursive git config --local --show-origin --name-only --get-regexp remote.origin.url
2026-11-04T18:54:54.0376789Z Removing credentials config '/home/runner/work/_temp/git-credentials-87e48186-2a9f-4671-a166-37562726307a.config'
2026-11-04T18:54:54.0385800Z ##[endgroup]
2026-11-04T18:54:54.0716676Z Prepare all required actions
2026-11-04T18:54:54.0717402Z Getting action download info
2026-11-04T18:54:54.2237360Z Download action repository 'actions/setup-dotnet@a98b56852c35b8e3190ac28c8c2271da59106c68' (SHA:a98b56852c35b8e3190ac28c8c2271da59106c68)
2026-11-04T18:54:54.8916654Z Download action repository 'actions/cache@55cc8345863c7cc4c66a329aec7e433d2d1c52a9' (SHA:55cc8345863c7cc4c66a329aec7e433d2d1c52a9)
2026-11-04T18:54:55.2872042Z ##[group]Run ./.github/actions/preparar-dotnet
2026-11-04T18:54:55.2872311Z env:
2026-11-04T18:54:55.2872478Z   DOTNET_NOLOGO: true
2026-11-04T18:54:55.2872673Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:54:55.2873089Z   CONFIGURACAO: Release
2026-11-04T18:54:55.2873372Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:54:55.2873774Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:54:55.2874417Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:54:55.2874845Z   RESULTADOS: resultados-ci
2026-11-04T18:54:55.2875051Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:54:55.2875276Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:54:55.2875672Z ##[endgroup]
2026-11-04T18:54:55.2921962Z ##[start-action display=Instalar o SDK do .NET 10;id=__self.__actions_setup-dotnet]
2026-11-04T18:54:55.2977691Z ##[group]Run actions/setup-dotnet@a98b56852c35b8e3190ac28c8c2271da59106c68
2026-11-04T18:54:55.2977994Z with:
2026-11-04T18:54:55.2978163Z   dotnet-version: 10.0.x
2026-11-04T18:54:55.2978401Z   cache: false
2026-11-04T18:54:55.2978561Z env:
2026-11-04T18:54:55.2978718Z   DOTNET_NOLOGO: true
2026-11-04T18:54:55.2978909Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:54:55.2979122Z   CONFIGURACAO: Release
2026-11-04T18:54:55.2979336Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:54:55.2979754Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:54:55.2980337Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:54:55.2980774Z   RESULTADOS: resultados-ci
2026-11-04T18:54:55.2981005Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:54:55.2981232Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:54:55.2981463Z ##[endgroup]
2026-11-04T18:54:55.4145027Z [command]/home/runner/work/_actions/actions/setup-dotnet/a98b56852c35b8e3190ac28c8c2271da59106c68/externals/install-dotnet.sh --skip-non-versioned-files --runtime dotnet --channel LTS
2026-11-04T18:54:55.6100115Z dotnet-install: .NET Core Runtime with version '10.0.14' is already installed.
2026-11-04T18:54:55.6121064Z [command]/home/runner/work/_actions/actions/setup-dotnet/a98b56852c35b8e3190ac28c8c2271da59106c68/externals/install-dotnet.sh --skip-non-versioned-files --channel 10.0
2026-11-04T18:54:55.7711177Z dotnet-install: .NET Core SDK with version '10.0.403' is already installed.
2026-11-04T18:54:55.7828269Z ##[end-action id=__self.__actions_setup-dotnet;outcome=success;conclusion=success;duration_ms=490]
2026-11-04T18:54:55.7830861Z ##[start-action display=Reutilizar a cache dos pacotes NuGet;id=__self.__actions_cache]
2026-11-04T18:54:55.9157478Z ##[group]Run actions/cache@55cc8345863c7cc4c66a329aec7e433d2d1c52a9
2026-11-04T18:54:55.9157764Z with:
2026-11-04T18:54:55.9157932Z   path: ~/.nuget/packages
2026-11-04T18:54:55.9158257Z   key: nuget-Linux-c8252acdcad9b687c3131c725ee3c1b10105ceb23478247a43e8389c12c19830
2026-11-04T18:54:55.9158605Z   restore-keys: nuget-Linux-

2026-11-04T18:54:55.9158814Z   enableCrossOsArchive: false
2026-11-04T18:54:55.9159010Z   fail-on-cache-miss: false
2026-11-04T18:54:55.9159200Z   lookup-only: false
2026-11-04T18:54:55.9159376Z   save-always: false
2026-11-04T18:54:55.9159551Z env:
2026-11-04T18:54:55.9159716Z   DOTNET_NOLOGO: true
2026-11-04T18:54:55.9159902Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:54:55.9160113Z   CONFIGURACAO: Release
2026-11-04T18:54:55.9160550Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:54:55.9160948Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:54:55.9161529Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:54:55.9161989Z   RESULTADOS: resultados-ci
2026-11-04T18:54:55.9162193Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:54:55.9162419Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:54:55.9162651Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:54:55.9163434Z ##[endgroup]
2026-11-04T18:54:56.2267004Z Cache hit for: nuget-Linux-c8252acdcad9b687c3131c725ee3c1b10105ceb23478247a43e8389c12c19830
2026-11-04T18:54:57.4677880Z Received 58720256 of 283633656 (20.7%), 55.9 MBs/sec
2026-11-04T18:54:58.4693448Z Received 239075328 of 283633656 (84.3%), 113.8 MBs/sec
2026-11-04T18:54:58.8292136Z Received 283633656 of 283633656 (100.0%), 114.5 MBs/sec
2026-11-04T18:54:58.8294036Z Cache Size: ~270 MB (283633656 B)
2026-11-04T18:54:58.8400803Z [command]/usr/bin/tar -xf /home/runner/work/_temp/e46b914b-0798-4f33-a827-0a83aaf1aa0e/cache.tzst -P -C /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO --use-compress-program unzstd
2026-11-04T18:54:59.7910303Z Cache restored successfully
2026-11-04T18:54:59.8170626Z Cache restored from key: nuget-Linux-c8252acdcad9b687c3131c725ee3c1b10105ceb23478247a43e8389c12c19830
2026-11-04T18:54:59.8282599Z ##[end-action id=__self.__actions_cache;outcome=success;conclusion=success;duration_ms=4045]
2026-11-04T18:54:59.8285345Z ##[start-action display=Registar o SDK utilizado;id=__self.__run]
2026-11-04T18:54:59.8314174Z ##[group]Run echo "SDK do .NET: $(dotnet --version)"
2026-11-04T18:54:59.8314541Z [36;1mecho "SDK do .NET: $(dotnet --version)"[0m
2026-11-04T18:54:59.8314794Z [36;1m{[0m
2026-11-04T18:54:59.8314974Z [36;1m  echo "### Ambiente .NET"[0m
2026-11-04T18:54:59.8315188Z [36;1m  echo ""[0m
2026-11-04T18:54:59.8315399Z [36;1m  echo "| Elemento | Valor |"[0m
2026-11-04T18:54:59.8315623Z [36;1m  echo "|---|---|"[0m
2026-11-04T18:54:59.8315869Z [36;1m  echo "| SDK do .NET | \`$(dotnet --version)\` |"[0m
2026-11-04T18:54:59.8316208Z [36;1m  echo "| global.json | \`$(tr -d '\n\r ' < src/backend/global.json)\` |"[0m
2026-11-04T18:54:59.8316584Z [36;1m  echo "| Runner | \`${RUNNER_OS} ${ImageOS:-} ${ImageVersion:-}\` |"[0m
2026-11-04T18:54:59.8316872Z [36;1m  echo ""[0m
2026-11-04T18:54:59.8317056Z [36;1m} >> "$GITHUB_STEP_SUMMARY"[0m
2026-11-04T18:54:59.8337707Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:54:59.8337990Z env:
2026-11-04T18:54:59.8338163Z   DOTNET_NOLOGO: true
2026-11-04T18:54:59.8338361Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:54:59.8338580Z   CONFIGURACAO: Release
2026-11-04T18:54:59.8338797Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:54:59.8339206Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:54:59.8339796Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:54:59.8340218Z   RESULTADOS: resultados-ci
2026-11-04T18:54:59.8340425Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:54:59.8340662Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:54:59.8340910Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:54:59.8341111Z ##[endgroup]
2026-11-04T18:55:00.7662254Z SDK do .NET: 10.0.403
2026-11-04T18:55:00.8428572Z ##[end-action id=__self.__run;outcome=success;conclusion=success;duration_ms=1014]
2026-11-04T18:55:00.8516706Z ##[group]Run dotnet restore "$SOLUCAO"
2026-11-04T18:55:00.8516994Z [36;1mdotnet restore "$SOLUCAO"[0m
2026-11-04T18:55:00.8534645Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:55:00.8534917Z env:
2026-11-04T18:55:00.8535085Z   DOTNET_NOLOGO: true
2026-11-04T18:55:00.8535278Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:00.8535503Z   CONFIGURACAO: Release
2026-11-04T18:55:00.8535720Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:00.8536123Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:00.8536723Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:00.8537148Z   RESULTADOS: resultados-ci
2026-11-04T18:55:00.8537359Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:00.8537585Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:00.8537825Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:55:00.8538059Z ##[endgroup]
2026-11-04T18:55:03.5111946Z   Determining projects to restore...
2026-11-04T18:55:04.4846779Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Domain/ClinicaVeterinaria.Domain.csproj (in 109 ms).
2026-11-04T18:55:04.7177106Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Application/ClinicaVeterinaria.Application.csproj (in 358 ms).
2026-11-04T18:55:04.7445325Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Infrastructure/ClinicaVeterinaria.Infrastructure.csproj (in 378 ms).
2026-11-04T18:55:04.7502527Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj (in 249 ms).
2026-11-04T18:55:04.7513791Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj (in 390 ms).
2026-11-04T18:55:04.7868889Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj (in 65 ms).
2026-11-04T18:55:04.8375107Z ##[group]Run mkdir -p "$GITHUB_WORKSPACE/$RESULTADOS/analise-estatica"
2026-11-04T18:55:04.8375557Z [36;1mmkdir -p "$GITHUB_WORKSPACE/$RESULTADOS/analise-estatica"[0m
2026-11-04T18:55:04.8375851Z [36;1mdotnet build "$SOLUCAO" \[0m
2026-11-04T18:55:04.8376104Z [36;1m  --configuration "$CONFIGURACAO" \[0m
2026-11-04T18:55:04.8376342Z [36;1m  --no-restore \[0m
2026-11-04T18:55:04.8376592Z [36;1m  -p:ContinuousIntegrationBuild=true \[0m
2026-11-04T18:55:04.8376948Z [36;1m  -p:PastaRelatoriosAnalise="$GITHUB_WORKSPACE/$RESULTADOS/analise-estatica"[0m
2026-11-04T18:55:04.8394564Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:55:04.8394835Z env:
2026-11-04T18:55:04.8395000Z   DOTNET_NOLOGO: true
2026-11-04T18:55:04.8395194Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:04.8395418Z   CONFIGURACAO: Release
2026-11-04T18:55:04.8395638Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:04.8396089Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:04.8396673Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:04.8397093Z   RESULTADOS: resultados-ci
2026-11-04T18:55:04.8397307Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:04.8397538Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:04.8397780Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:55:04.8397970Z ##[endgroup]
2026-11-04T18:55:11.5668342Z   ClinicaVeterinaria.Domain -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Domain/bin/Release/net10.0/ClinicaVeterinaria.Domain.dll
2026-11-04T18:55:13.7707240Z   ClinicaVeterinaria.Application -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Application/bin/Release/net10.0/ClinicaVeterinaria.Application.dll
2026-11-04T18:55:16.2515737Z   ClinicaVeterinaria.Infrastructure -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Infrastructure/bin/Release/net10.0/ClinicaVeterinaria.Infrastructure.dll
2026-11-04T18:55:20.0353219Z   ClinicaVeterinaria.Api -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Api/bin/Release/net10.0/ClinicaVeterinaria.Api.dll
2026-11-04T18:55:27.8941036Z   ClinicaVeterinaria.UnitTests -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.UnitTests/bin/Release/net10.0/ClinicaVeterinaria.UnitTests.dll
2026-11-04T18:55:28.3865961Z   ClinicaVeterinaria.IntegrationTests -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.IntegrationTests/bin/Release/net10.0/ClinicaVeterinaria.IntegrationTests.dll
2026-11-04T18:55:28.8539383Z 
2026-11-04T18:55:28.8559590Z Build succeeded.
2026-11-04T18:55:28.8560033Z     0 Warning(s)
2026-11-04T18:55:28.8560382Z     0 Error(s)
2026-11-04T18:55:28.8560619Z 
2026-11-04T18:55:28.8561605Z Time Elapsed 00:00:23.85
2026-11-04T18:55:28.8791186Z ##[group]Run {
2026-11-04T18:55:28.8791515Z [36;1m{[0m
2026-11-04T18:55:28.8791853Z [36;1m  echo "### 1 · Restauro e compilação (QG-01)"[0m
2026-11-04T18:55:28.8792259Z [36;1m  echo ""[0m
2026-11-04T18:55:28.8793127Z [36;1m  echo "✅ A solução \`$SOLUCAO\` foi restaurada e compilada em \`$CONFIGURACAO\` sem erros nem avisos (quality gate da I101)."[0m
2026-11-04T18:55:28.8794116Z [36;1m  echo ""[0m
2026-11-04T18:55:28.8794409Z [36;1m} >> "$GITHUB_STEP_SUMMARY"[0m
2026-11-04T18:55:28.8818793Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:55:28.8819197Z env:
2026-11-04T18:55:28.8819449Z   DOTNET_NOLOGO: true
2026-11-04T18:55:28.8819754Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:28.8820125Z   CONFIGURACAO: Release
2026-11-04T18:55:28.8820487Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:28.8821143Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:28.8822212Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:28.8823297Z   RESULTADOS: resultados-ci
2026-11-04T18:55:28.8823629Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:28.8824004Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:28.8824417Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:55:28.8824727Z ##[endgroup]
2026-11-04T18:55:28.8919024Z ##[group]Run actions/upload-artifact@cf430e030ddbb5b0abf93d22962f4752f3646cd9
2026-11-04T18:55:28.8919347Z with:
2026-11-04T18:55:28.8919536Z   name: relatorio-analise-estatica-sarif
2026-11-04T18:55:28.8919779Z   path: resultados-ci/analise-estatica
2026-11-04T18:55:28.8920008Z   if-no-files-found: warn
2026-11-04T18:55:28.8920206Z   retention-days: 90
2026-11-04T18:55:28.8920393Z   compression-level: 6
2026-11-04T18:55:28.8920584Z   overwrite: false
2026-11-04T18:55:28.8920762Z   include-hidden-files: false
2026-11-04T18:55:28.8920967Z   archive: true
2026-11-04T18:55:28.8921129Z env:
2026-11-04T18:55:28.8921284Z   DOTNET_NOLOGO: true
2026-11-04T18:55:28.8921468Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:28.8921683Z   CONFIGURACAO: Release
2026-11-04T18:55:28.8921890Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:28.8922287Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:28.8923090Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:28.8923533Z   RESULTADOS: resultados-ci
2026-11-04T18:55:28.8923742Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:28.8923975Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:28.8924209Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:55:28.8924409Z ##[endgroup]
2026-11-04T18:55:29.0311846Z With the provided path, there will be 6 files uploaded
2026-11-04T18:55:29.0313103Z Artifact name is valid!
2026-11-04T18:55:29.0314788Z Root directory input is valid!
2026-11-04T18:55:29.5780802Z Uploading artifact: relatorio-analise-estatica-sarif.zip
2026-11-04T18:55:29.5824146Z Beginning upload of artifact content to blob storage
2026-11-04T18:55:30.1129705Z Uploaded bytes 306536
2026-11-04T18:55:30.1748090Z Finished uploading artifact content to blob storage!
2026-11-04T18:55:30.1750894Z SHA256 digest of uploaded artifact is e365c5bd0329536e6dc2b49edeb0b1d7be4d3898114ec7a53903cf25544c23eb
2026-11-04T18:55:30.1751908Z Finalizing artifact upload
2026-11-04T18:55:30.7374072Z Artifact relatorio-analise-estatica-sarif successfully finalized. Artifact ID 13354841208
2026-11-04T18:55:30.7375242Z Artifact relatorio-analise-estatica-sarif has been successfully uploaded! Final size is 306536 bytes. Artifact ID is 13354841208
2026-11-04T18:55:30.7376524Z Artifact download URL: https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/actions/runs/41891774877/artifacts/13354841208
2026-11-04T18:55:30.7555527Z Post job cleanup.
2026-11-04T18:55:30.7559221Z ##[start-action display=Reutilizar a cache dos pacotes NuGet;id=__cbd5020c-8849-4bd8-b1de-e54412849e1d.__actions_cache]
2026-11-04T18:55:30.9641485Z Post job cleanup.
2026-11-04T18:55:31.0702051Z Cache hit occurred on the primary key nuget-Linux-c8252acdcad9b687c3131c725ee3c1b10105ceb23478247a43e8389c12c19830, not saving cache.
2026-11-04T18:55:31.0766892Z ##[end-action id=__cbd5020c-8849-4bd8-b1de-e54412849e1d.__actions_cache;outcome=success;conclusion=success;duration_ms=320]
2026-11-04T18:55:31.0770482Z ##[start-action display=Instalar o SDK do .NET 10;id=__cbd5020c-8849-4bd8-b1de-e54412849e1d.__actions_setup-dotnet]
2026-11-04T18:55:31.0823506Z Post job cleanup.
2026-11-04T18:55:31.2056775Z ##[end-action id=__cbd5020c-8849-4bd8-b1de-e54412849e1d.__actions_setup-dotnet;outcome=success;conclusion=success;duration_ms=128]
2026-11-04T18:55:31.2117163Z Post job cleanup.
2026-11-04T18:55:31.2918996Z [command]/usr/bin/git version
2026-11-04T18:55:31.2961117Z git version 2.56.0
2026-11-04T18:55:31.2993139Z Temporarily overriding HOME='/home/runner/work/_temp/d615ff09-cbef-46d7-9d51-05db9b07a698' before making global git config changes
2026-11-04T18:55:31.2994474Z Adding repository directory to the temporary git global config as a safe directory
2026-11-04T18:55:31.3004131Z [command]/usr/bin/git config --global --add safe.directory /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:55:31.3040682Z Removing SSH command configuration
2026-11-04T18:55:31.3042545Z [command]/usr/bin/git config --local --name-only --get-regexp core\.sshCommand
2026-11-04T18:55:31.3080321Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'core\.sshCommand' && git config --local --unset-all 'core.sshCommand' || :"
2026-11-04T18:55:31.3319530Z Removing HTTP extra header
2026-11-04T18:55:31.3325938Z [command]/usr/bin/git config --local --name-only --get-regexp http\.https\:\/\/github\.com\/\.extraheader
2026-11-04T18:55:31.3377387Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'http\.https\:\/\/github\.com\/\.extraheader' && git config --local --unset-all 'http.https://github.com/.extraheader' || :"
2026-11-04T18:55:31.3805282Z Removing includeIf entries pointing to credentials config files
2026-11-04T18:55:31.3815969Z [command]/usr/bin/git config --local --name-only --get-regexp ^includeIf\.gitdir:
2026-11-04T18:55:31.3892269Z [command]/usr/bin/git submodule foreach --recursive git config --local --show-origin --name-only --get-regexp remote.origin.url
2026-11-04T18:55:31.4517390Z Cleaning up orphan processes
2026-11-04T18:55:31.4837129Z Terminate orphan process: pid (2432) (dotnet)
2026-11-04T18:55:31.4874386Z Terminate orphan process: pid (2433) (dotnet)
2026-11-04T18:55:31.4897184Z Terminate orphan process: pid (2434) (dotnet)
2026-11-04T18:55:31.4916724Z Terminate orphan process: pid (2547) (VBCSCompiler)
``````
