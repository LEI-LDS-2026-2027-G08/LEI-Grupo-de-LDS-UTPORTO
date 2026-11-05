``````text
﻿2026-11-04T18:54:25.3484231Z Current runner version: '2.338.2'
2026-11-04T18:54:25.3513923Z ##[group]Runner Image Provisioner
2026-11-04T18:54:25.3514881Z Hosted Compute Agent
2026-11-04T18:54:25.3515642Z Version: 20261028.624
2026-11-04T18:54:25.3516528Z Commit: 54898b74c23fde5556c9b3c3a2290865feef0dfe
2026-11-04T18:54:25.3517452Z Build Date: 2026-10-28T21:51:42Z
2026-11-04T18:54:25.3518306Z Worker ID: {722dedc1-8eb8-4518-af8b-f57009ffa20a}
2026-11-04T18:54:25.3519151Z Region: eastus
2026-11-04T18:54:25.3519878Z Cloud: Azure
2026-11-04T18:54:25.3520523Z ##[endgroup]
2026-11-04T18:54:25.3522394Z ##[group]Operating System
2026-11-04T18:54:25.3523309Z Ubuntu
2026-11-04T18:54:25.3523947Z 24.04.5
2026-11-04T18:54:25.3524603Z LTS
2026-11-04T18:54:25.3525244Z ##[endgroup]
2026-11-04T18:54:25.3525942Z ##[group]Runner Image
2026-11-04T18:54:25.3526778Z Image: ubuntu-24.04
2026-11-04T18:54:25.3527470Z Version: 20261101.360.1
2026-11-04T18:54:25.3529113Z Included Software: https://github.com/actions/runner-images/blob/ubuntu24/20261101.360/images/ubuntu/Ubuntu2404-Readme.md
2026-11-04T18:54:25.3531191Z Image Release: https://github.com/actions/runner-images/releases/tag/ubuntu24%2F20261101.360
2026-11-04T18:54:25.3532557Z ##[endgroup]
2026-11-04T18:54:25.3533916Z ##[group]GITHUB_TOKEN Permissions
2026-11-04T18:54:25.3536440Z Contents: read
2026-11-04T18:54:25.3537185Z Metadata: read
2026-11-04T18:54:25.3537980Z ##[endgroup]
2026-11-04T18:54:25.3540311Z Secret source: Actions
2026-11-04T18:54:25.3541736Z Cache mode: write
2026-11-04T18:54:25.3542929Z Prepare workflow directory
2026-11-04T18:54:25.3999409Z Prepare all required actions
2026-11-04T18:54:25.4055771Z Getting action download info
2026-11-04T18:54:25.5956987Z Download action repository 'actions/checkout@3d3c42e5aac5ba805825da76410c181273ba90b1' (SHA:3d3c42e5aac5ba805825da76410c181273ba90b1)
2026-11-04T18:54:25.7559118Z Download action repository 'actions/upload-artifact@cf430e030ddbb5b0abf93d22962f4752f3646cd9' (SHA:cf430e030ddbb5b0abf93d22962f4752f3646cd9)
2026-11-04T18:54:25.9384384Z Complete job name: 1 · Restauro e compilação (QG-01)
2026-11-04T18:54:26.0072229Z ##[group]Run actions/checkout@3d3c42e5aac5ba805825da76410c181273ba90b1
2026-11-04T18:54:26.0073049Z with:
2026-11-04T18:54:26.0073417Z   persist-credentials: false
2026-11-04T18:54:26.0073931Z   repository: LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:54:26.0077338Z   token: ***
2026-11-04T18:54:26.0077712Z   ssh-strict: true
2026-11-04T18:54:26.0078087Z   ssh-user: git
2026-11-04T18:54:26.0078439Z   clean: true
2026-11-04T18:54:26.0078809Z   sparse-checkout-cone-mode: true
2026-11-04T18:54:26.0079234Z   fetch-depth: 1
2026-11-04T18:54:26.0079589Z   fetch-tags: false
2026-11-04T18:54:26.0079958Z   show-progress: true
2026-11-04T18:54:26.0080327Z   lfs: false
2026-11-04T18:54:26.0080682Z   submodules: false
2026-11-04T18:54:26.0081293Z   set-safe-directory: true
2026-11-04T18:54:26.0081768Z   allow-unsafe-pr-checkout: false
2026-11-04T18:54:26.0082419Z env:
2026-11-04T18:54:26.0082759Z   DOTNET_NOLOGO: true
2026-11-04T18:54:26.0083210Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:54:26.0083645Z   CONFIGURACAO: Release
2026-11-04T18:54:26.0084063Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:54:26.0084814Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:54:26.0085905Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:54:26.0086754Z   RESULTADOS: resultados-ci
2026-11-04T18:54:26.0087180Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:54:26.0087627Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:54:26.0088083Z ##[endgroup]
2026-11-04T18:54:26.3589238Z Syncing repository: LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:54:26.3592498Z ##[group]Getting Git version info
2026-11-04T18:54:26.3593812Z Working directory is '/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO'
2026-11-04T18:54:26.3595667Z [command]/usr/bin/git version
2026-11-04T18:54:26.3642944Z git version 2.56.0
2026-11-04T18:54:26.3651785Z ##[endgroup]
2026-11-04T18:54:26.3665841Z Temporarily overriding HOME='/home/runner/work/_temp/0b137f01-3f72-411c-8e47-350c51d1a15e' before making global git config changes
2026-11-04T18:54:26.3668435Z Adding repository directory to the temporary git global config as a safe directory
2026-11-04T18:54:26.3674644Z [command]/usr/bin/git config --global --add safe.directory /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:54:26.3740264Z Deleting the contents of '/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO'
2026-11-04T18:54:26.3810403Z ##[group]Determining repository object format
2026-11-04T18:54:26.3811921Z ##[endgroup]
2026-11-04T18:54:26.3812675Z ##[group]Initializing the repository
2026-11-04T18:54:26.3813810Z [command]/usr/bin/git init /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:54:26.3961804Z hint: Using 'master' as the name for the initial branch. This default branch name
2026-11-04T18:54:26.3964077Z hint: will change to "main" in Git 3.0. To configure the initial branch name
2026-11-04T18:54:26.3965004Z hint: to use in all of your new repositories, which will suppress this warning,
2026-11-04T18:54:26.3965968Z hint: call:
2026-11-04T18:54:26.3966539Z hint:
2026-11-04T18:54:26.3967150Z hint: 	git config --global init.defaultBranch <name>
2026-11-04T18:54:26.3967975Z hint:
2026-11-04T18:54:26.3968655Z hint: Names commonly chosen instead of 'master' are 'main', 'trunk' and
2026-11-04T18:54:26.3970098Z hint: 'development'. The just-created branch can be renamed via this command:
2026-11-04T18:54:26.3971424Z hint:
2026-11-04T18:54:26.3972144Z hint: 	git branch -m <name>
2026-11-04T18:54:26.3972948Z hint:
2026-11-04T18:54:26.3974020Z hint: Disable this message with "git config set advice.defaultBranchName false"
2026-11-04T18:54:26.3976201Z Initialized empty Git repository in /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/
2026-11-04T18:54:26.3986136Z [command]/usr/bin/git remote add origin https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:54:26.4131881Z ##[endgroup]
2026-11-04T18:54:26.4132702Z ##[group]Disabling automatic garbage collection
2026-11-04T18:54:26.4135655Z [command]/usr/bin/git config --local gc.auto 0
2026-11-04T18:54:26.4185959Z ##[endgroup]
2026-11-04T18:54:26.4187466Z ##[group]Setting up auth
2026-11-04T18:54:26.4188268Z Removing SSH command configuration
2026-11-04T18:54:26.4193198Z [command]/usr/bin/git config --local --name-only --get-regexp core\.sshCommand
2026-11-04T18:54:26.4237548Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'core\.sshCommand' && git config --local --unset-all 'core.sshCommand' || :"
2026-11-04T18:54:26.4633116Z Removing HTTP extra header
2026-11-04T18:54:26.4638633Z [command]/usr/bin/git config --local --name-only --get-regexp http\.https\:\/\/github\.com\/\.extraheader
2026-11-04T18:54:26.4683363Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'http\.https\:\/\/github\.com\/\.extraheader' && git config --local --unset-all 'http.https://github.com/.extraheader' || :"
2026-11-04T18:54:26.4892251Z Removing includeIf entries pointing to credentials config files
2026-11-04T18:54:26.4896965Z [command]/usr/bin/git config --local --name-only --get-regexp ^includeIf\.gitdir:
2026-11-04T18:54:26.4935820Z [command]/usr/bin/git submodule foreach --recursive git config --local --show-origin --name-only --get-regexp remote.origin.url
2026-11-04T18:54:26.5147444Z [command]/usr/bin/git config --file /home/runner/work/_temp/git-credentials-04956b8f-fc72-4fa7-9cb4-3d63beed7e6c.config http.https://github.com/.extraheader AUTHORIZATION: basic ***
2026-11-04T18:54:26.5191561Z [command]/usr/bin/git config --local includeIf.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path /home/runner/work/_temp/git-credentials-04956b8f-fc72-4fa7-9cb4-3d63beed7e6c.config
2026-11-04T18:54:26.5228352Z [command]/usr/bin/git config --local includeIf.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path /home/runner/work/_temp/git-credentials-04956b8f-fc72-4fa7-9cb4-3d63beed7e6c.config
2026-11-04T18:54:26.5268764Z [command]/usr/bin/git config --local includeIf.gitdir:/github/workspace/.git.path /github/runner_temp/git-credentials-04956b8f-fc72-4fa7-9cb4-3d63beed7e6c.config
2026-11-04T18:54:26.5307146Z [command]/usr/bin/git config --local includeIf.gitdir:/github/workspace/.git/worktrees/*.path /github/runner_temp/git-credentials-04956b8f-fc72-4fa7-9cb4-3d63beed7e6c.config
2026-11-04T18:54:26.5342211Z ##[endgroup]
2026-11-04T18:54:26.5342930Z ##[group]Fetching the repository
2026-11-04T18:54:26.5349406Z [command]/usr/bin/git -c protocol.version=2 fetch --no-tags --prune --no-recurse-submodules --depth=1 origin +1e0181f1ad52288323bc89cefd5d2641a3f2655a:refs/remotes/pull/247/merge
2026-11-04T18:54:27.9763523Z From https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:54:27.9765293Z  * [new ref]         1e0181f1ad52288323bc89cefd5d2641a3f2655a -> pull/247/merge
2026-11-04T18:54:27.9768964Z ##[endgroup]
2026-11-04T18:54:27.9769887Z ##[group]Determining the checkout info
2026-11-04T18:54:27.9771123Z ##[endgroup]
2026-11-04T18:54:27.9772454Z [command]/usr/bin/git sparse-checkout disable
2026-11-04T18:54:27.9839338Z [command]/usr/bin/git config --local --unset-all extensions.worktreeConfig
2026-11-04T18:54:27.9878779Z ##[group]Checking out the ref
2026-11-04T18:54:27.9881264Z [command]/usr/bin/git checkout --progress --force refs/remotes/pull/247/merge
2026-11-04T18:54:28.1560291Z Note: switching to 'refs/remotes/pull/247/merge'.
2026-11-04T18:54:28.1561538Z 
2026-11-04T18:54:28.1562389Z You are in 'detached HEAD' state. You can look around, make experimental
2026-11-04T18:54:28.1564306Z changes and commit them, and you can discard any commits you make in this
2026-11-04T18:54:28.1566756Z state without impacting any branches by switching back to a branch.
2026-11-04T18:54:28.1567930Z 
2026-11-04T18:54:28.1568731Z If you want to create a new branch to retain commits you create, you may
2026-11-04T18:54:28.1570484Z do so (now or later) by using -c with the switch command. Example:
2026-11-04T18:54:28.1571609Z 
2026-11-04T18:54:28.1572083Z   git switch -c <new-branch-name>
2026-11-04T18:54:28.1572933Z 
2026-11-04T18:54:28.1573557Z Or undo this operation with:
2026-11-04T18:54:28.1574442Z 
2026-11-04T18:54:28.1575030Z   git switch -
2026-11-04T18:54:28.1575792Z 
2026-11-04T18:54:28.1576843Z Turn off this advice by setting config variable advice.detachedHead to false
2026-11-04T18:54:28.1578349Z 
2026-11-04T18:54:28.1580053Z HEAD is now at 1e0181f Merge a94b7155edcdf8d0c0779b09a41573ebfadc26d4 into 89d6b31c5e788b4e8e5cfc7178d991ef65856c06
2026-11-04T18:54:28.1592667Z ##[endgroup]
2026-11-04T18:54:28.1643679Z [command]/usr/bin/git log -1 --format=%H
2026-11-04T18:54:28.1676789Z 1e0181f1ad52288323bc89cefd5d2641a3f2655a
2026-11-04T18:54:28.1685013Z ##[group]Removing auth
2026-11-04T18:54:28.1686178Z Removing SSH command configuration
2026-11-04T18:54:28.1688285Z [command]/usr/bin/git config --local --name-only --get-regexp core\.sshCommand
2026-11-04T18:54:28.1727540Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'core\.sshCommand' && git config --local --unset-all 'core.sshCommand' || :"
2026-11-04T18:54:28.1967648Z Removing HTTP extra header
2026-11-04T18:54:28.1970269Z [command]/usr/bin/git config --local --name-only --get-regexp http\.https\:\/\/github\.com\/\.extraheader
2026-11-04T18:54:28.2019464Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'http\.https\:\/\/github\.com\/\.extraheader' && git config --local --unset-all 'http.https://github.com/.extraheader' || :"
2026-11-04T18:54:28.2307893Z Removing includeIf entries pointing to credentials config files
2026-11-04T18:54:28.2310971Z [command]/usr/bin/git config --local --name-only --get-regexp ^includeIf\.gitdir:
2026-11-04T18:54:28.2345315Z includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path
2026-11-04T18:54:28.2348269Z includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path
2026-11-04T18:54:28.2350332Z includeif.gitdir:/github/workspace/.git.path
2026-11-04T18:54:28.2352140Z includeif.gitdir:/github/workspace/.git/worktrees/*.path
2026-11-04T18:54:28.2356082Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path
2026-11-04T18:54:28.2383809Z /home/runner/work/_temp/git-credentials-04956b8f-fc72-4fa7-9cb4-3d63beed7e6c.config
2026-11-04T18:54:28.2396346Z [command]/usr/bin/git config --local --unset includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path \/home\/runner\/work\/_temp\/git\-credentials\-04956b8f\-fc72\-4fa7\-9cb4\-3d63beed7e6c\.config
2026-11-04T18:54:28.2442402Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path
2026-11-04T18:54:28.2473074Z /home/runner/work/_temp/git-credentials-04956b8f-fc72-4fa7-9cb4-3d63beed7e6c.config
2026-11-04T18:54:28.2484036Z [command]/usr/bin/git config --local --unset includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path \/home\/runner\/work\/_temp\/git\-credentials\-04956b8f\-fc72\-4fa7\-9cb4\-3d63beed7e6c\.config
2026-11-04T18:54:28.2526621Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/github/workspace/.git.path
2026-11-04T18:54:28.2558408Z /github/runner_temp/git-credentials-04956b8f-fc72-4fa7-9cb4-3d63beed7e6c.config
2026-11-04T18:54:28.2566999Z [command]/usr/bin/git config --local --unset includeif.gitdir:/github/workspace/.git.path \/github\/runner_temp\/git\-credentials\-04956b8f\-fc72\-4fa7\-9cb4\-3d63beed7e6c\.config
2026-11-04T18:54:28.2609361Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/github/workspace/.git/worktrees/*.path
2026-11-04T18:54:28.2640736Z /github/runner_temp/git-credentials-04956b8f-fc72-4fa7-9cb4-3d63beed7e6c.config
2026-11-04T18:54:28.2663701Z [command]/usr/bin/git config --local --unset includeif.gitdir:/github/workspace/.git/worktrees/*.path \/github\/runner_temp\/git\-credentials\-04956b8f\-fc72\-4fa7\-9cb4\-3d63beed7e6c\.config
2026-11-04T18:54:28.2692232Z [command]/usr/bin/git submodule foreach --recursive git config --local --show-origin --name-only --get-regexp remote.origin.url
2026-11-04T18:54:28.2913939Z Removing credentials config '/home/runner/work/_temp/git-credentials-04956b8f-fc72-4fa7-9cb4-3d63beed7e6c.config'
2026-11-04T18:54:28.2924128Z ##[endgroup]
2026-11-04T18:54:28.3270044Z Prepare all required actions
2026-11-04T18:54:28.3270818Z Getting action download info
2026-11-04T18:54:28.4081653Z Download action repository 'actions/setup-dotnet@a98b56852c35b8e3190ac28c8c2271da59106c68' (SHA:a98b56852c35b8e3190ac28c8c2271da59106c68)
2026-11-04T18:54:28.6025240Z Download action repository 'actions/cache@55cc8345863c7cc4c66a329aec7e433d2d1c52a9' (SHA:55cc8345863c7cc4c66a329aec7e433d2d1c52a9)
2026-11-04T18:54:28.7783470Z ##[group]Run ./.github/actions/preparar-dotnet
2026-11-04T18:54:28.7783736Z env:
2026-11-04T18:54:28.7783908Z   DOTNET_NOLOGO: true
2026-11-04T18:54:28.7784108Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:54:28.7784326Z   CONFIGURACAO: Release
2026-11-04T18:54:28.7784538Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:54:28.7784954Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:54:28.7785604Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:54:28.7786045Z   RESULTADOS: resultados-ci
2026-11-04T18:54:28.7786258Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:54:28.7786497Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:54:28.7786894Z ##[endgroup]
2026-11-04T18:54:28.7832226Z ##[start-action display=Instalar o SDK do .NET 10;id=__self.__actions_setup-dotnet]
2026-11-04T18:54:28.7884135Z ##[group]Run actions/setup-dotnet@a98b56852c35b8e3190ac28c8c2271da59106c68
2026-11-04T18:54:28.7884435Z with:
2026-11-04T18:54:28.7884608Z   dotnet-version: 10.0.x
2026-11-04T18:54:28.7884802Z   cache: false
2026-11-04T18:54:28.7884964Z env:
2026-11-04T18:54:28.7885129Z   DOTNET_NOLOGO: true
2026-11-04T18:54:28.7885315Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:54:28.7885529Z   CONFIGURACAO: Release
2026-11-04T18:54:28.7885741Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:54:28.7886168Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:54:28.7886769Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:54:28.7887203Z   RESULTADOS: resultados-ci
2026-11-04T18:54:28.7887428Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:54:28.7887659Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:54:28.7887890Z ##[endgroup]
2026-11-04T18:54:28.9083449Z [command]/home/runner/work/_actions/actions/setup-dotnet/a98b56852c35b8e3190ac28c8c2271da59106c68/externals/install-dotnet.sh --skip-non-versioned-files --runtime dotnet --channel LTS
2026-11-04T18:54:29.2340356Z dotnet-install: .NET Core Runtime with version '10.0.14' is already installed.
2026-11-04T18:54:29.2362405Z [command]/home/runner/work/_actions/actions/setup-dotnet/a98b56852c35b8e3190ac28c8c2271da59106c68/externals/install-dotnet.sh --skip-non-versioned-files --channel 10.0
2026-11-04T18:54:29.3810490Z dotnet-install: .NET Core SDK with version '10.0.403' is already installed.
2026-11-04T18:54:29.3936956Z ##[end-action id=__self.__actions_setup-dotnet;outcome=success;conclusion=success;duration_ms=610]
2026-11-04T18:54:29.3939663Z ##[start-action display=Reutilizar a cache dos pacotes NuGet;id=__self.__actions_cache]
2026-11-04T18:54:29.6018501Z ##[group]Run actions/cache@55cc8345863c7cc4c66a329aec7e433d2d1c52a9
2026-11-04T18:54:29.6018789Z with:
2026-11-04T18:54:29.6018958Z   path: ~/.nuget/packages
2026-11-04T18:54:29.6019281Z   key: nuget-Linux-c8252acdcad9b687c3131c725ee3c1b10105ceb23478247a43e8389c12c19830
2026-11-04T18:54:29.6019627Z   restore-keys: nuget-Linux-

2026-11-04T18:54:29.6019838Z   enableCrossOsArchive: false
2026-11-04T18:54:29.6020034Z   fail-on-cache-miss: false
2026-11-04T18:54:29.6020226Z   lookup-only: false
2026-11-04T18:54:29.6020401Z   save-always: false
2026-11-04T18:54:29.6020572Z env:
2026-11-04T18:54:29.6020730Z   DOTNET_NOLOGO: true
2026-11-04T18:54:29.6020912Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:54:29.6021324Z   CONFIGURACAO: Release
2026-11-04T18:54:29.6021796Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:54:29.6022194Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:54:29.6022784Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:54:29.6023219Z   RESULTADOS: resultados-ci
2026-11-04T18:54:29.6023419Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:54:29.6023644Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:54:29.6023872Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:54:29.6024063Z ##[endgroup]
2026-11-04T18:54:29.8313379Z Cache hit for: nuget-Linux-c8252acdcad9b687c3131c725ee3c1b10105ceb23478247a43e8389c12c19830
2026-11-04T18:54:30.8751627Z Received 268435456 of 283633656 (94.6%), 256.3 MBs/sec
2026-11-04T18:54:30.9297121Z Received 283633656 of 283633656 (100.0%), 256.6 MBs/sec
2026-11-04T18:54:30.9299424Z Cache Size: ~270 MB (283633656 B)
2026-11-04T18:54:30.9388231Z [command]/usr/bin/tar -xf /home/runner/work/_temp/98763477-8190-48f8-bcc1-25a807ce75e4/cache.tzst -P -C /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO --use-compress-program unzstd
2026-11-04T18:54:31.8862383Z Cache restored successfully
2026-11-04T18:54:31.9147477Z Cache restored from key: nuget-Linux-c8252acdcad9b687c3131c725ee3c1b10105ceb23478247a43e8389c12c19830
2026-11-04T18:54:31.9257719Z ##[end-action id=__self.__actions_cache;outcome=success;conclusion=success;duration_ms=2531]
2026-11-04T18:54:31.9260300Z ##[start-action display=Registar o SDK utilizado;id=__self.__run]
2026-11-04T18:54:31.9294303Z ##[group]Run echo "SDK do .NET: $(dotnet --version)"
2026-11-04T18:54:31.9294673Z [36;1mecho "SDK do .NET: $(dotnet --version)"[0m
2026-11-04T18:54:31.9294925Z [36;1m{[0m
2026-11-04T18:54:31.9295281Z [36;1m  echo "### Ambiente .NET"[0m
2026-11-04T18:54:31.9295554Z [36;1m  echo ""[0m
2026-11-04T18:54:31.9295741Z [36;1m  echo "| Elemento | Valor |"[0m
2026-11-04T18:54:31.9295975Z [36;1m  echo "|---|---|"[0m
2026-11-04T18:54:31.9296228Z [36;1m  echo "| SDK do .NET | \`$(dotnet --version)\` |"[0m
2026-11-04T18:54:31.9296580Z [36;1m  echo "| global.json | \`$(tr -d '\n\r ' < src/backend/global.json)\` |"[0m
2026-11-04T18:54:31.9296955Z [36;1m  echo "| Runner | \`${RUNNER_OS} ${ImageOS:-} ${ImageVersion:-}\` |"[0m
2026-11-04T18:54:31.9297257Z [36;1m  echo ""[0m
2026-11-04T18:54:31.9297457Z [36;1m} >> "$GITHUB_STEP_SUMMARY"[0m
2026-11-04T18:54:31.9319415Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:54:31.9319706Z env:
2026-11-04T18:54:31.9319877Z   DOTNET_NOLOGO: true
2026-11-04T18:54:31.9320075Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:54:31.9320302Z   CONFIGURACAO: Release
2026-11-04T18:54:31.9320513Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:54:31.9320925Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:54:31.9321821Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:54:31.9322266Z   RESULTADOS: resultados-ci
2026-11-04T18:54:31.9322476Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:54:31.9322704Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:54:31.9322950Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:54:31.9323151Z ##[endgroup]
2026-11-04T18:54:34.4704257Z SDK do .NET: 10.0.403
2026-11-04T18:54:34.5422082Z ##[end-action id=__self.__run;outcome=success;conclusion=success;duration_ms=2616]
2026-11-04T18:54:34.5454616Z ##[group]Run dotnet restore "$SOLUCAO"
2026-11-04T18:54:34.5454886Z [36;1mdotnet restore "$SOLUCAO"[0m
2026-11-04T18:54:34.5472617Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:54:34.5472892Z env:
2026-11-04T18:54:34.5473060Z   DOTNET_NOLOGO: true
2026-11-04T18:54:34.5473257Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:54:34.5473489Z   CONFIGURACAO: Release
2026-11-04T18:54:34.5473704Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:54:34.5474103Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:54:34.5474691Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:54:34.5475138Z   RESULTADOS: resultados-ci
2026-11-04T18:54:34.5475349Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:54:34.5475576Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:54:34.5475815Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:54:34.5476045Z ##[endgroup]
2026-11-04T18:54:38.6026925Z   Determining projects to restore...
2026-11-04T18:54:40.4052939Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Domain/ClinicaVeterinaria.Domain.csproj (in 239 ms).
2026-11-04T18:54:41.0179921Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Application/ClinicaVeterinaria.Application.csproj (in 978 ms).
2026-11-04T18:54:41.0411681Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Infrastructure/ClinicaVeterinaria.Infrastructure.csproj (in 18 ms).
2026-11-04T18:54:41.0528469Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj (in 1.01 sec).
2026-11-04T18:54:41.0705558Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj (in 635 ms).
2026-11-04T18:54:41.0842961Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj (in 1.05 sec).
2026-11-04T18:54:41.1893819Z ##[group]Run mkdir -p "$GITHUB_WORKSPACE/$RESULTADOS/analise-estatica"
2026-11-04T18:54:41.1894281Z [36;1mmkdir -p "$GITHUB_WORKSPACE/$RESULTADOS/analise-estatica"[0m
2026-11-04T18:54:41.1894581Z [36;1mdotnet build "$SOLUCAO" \[0m
2026-11-04T18:54:41.1894836Z [36;1m  --configuration "$CONFIGURACAO" \[0m
2026-11-04T18:54:41.1895080Z [36;1m  --no-restore \[0m
2026-11-04T18:54:41.1895316Z [36;1m  -p:ContinuousIntegrationBuild=true \[0m
2026-11-04T18:54:41.1895697Z [36;1m  -p:PastaRelatoriosAnalise="$GITHUB_WORKSPACE/$RESULTADOS/analise-estatica"[0m
2026-11-04T18:54:41.1913593Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:54:41.1913875Z env:
2026-11-04T18:54:41.1914049Z   DOTNET_NOLOGO: true
2026-11-04T18:54:41.1914258Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:54:41.1914488Z   CONFIGURACAO: Release
2026-11-04T18:54:41.1914707Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:54:41.1915165Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:54:41.1915747Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:54:41.1916191Z   RESULTADOS: resultados-ci
2026-11-04T18:54:41.1916403Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:54:41.1916636Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:54:41.1916875Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:54:41.1917085Z ##[endgroup]
2026-11-04T18:54:49.2165523Z   ClinicaVeterinaria.Domain -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Domain/bin/Release/net10.0/ClinicaVeterinaria.Domain.dll
2026-11-04T18:54:51.0217179Z   ClinicaVeterinaria.Application -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Application/bin/Release/net10.0/ClinicaVeterinaria.Application.dll
2026-11-04T18:54:53.0449286Z   ClinicaVeterinaria.Infrastructure -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Infrastructure/bin/Release/net10.0/ClinicaVeterinaria.Infrastructure.dll
2026-11-04T18:54:56.1550192Z   ClinicaVeterinaria.Api -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Api/bin/Release/net10.0/ClinicaVeterinaria.Api.dll
2026-11-04T18:55:03.3255982Z   ClinicaVeterinaria.UnitTests -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.UnitTests/bin/Release/net10.0/ClinicaVeterinaria.UnitTests.dll
2026-11-04T18:55:03.7100593Z   ClinicaVeterinaria.IntegrationTests -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.IntegrationTests/bin/Release/net10.0/ClinicaVeterinaria.IntegrationTests.dll
2026-11-04T18:55:03.8674592Z 
2026-11-04T18:55:03.8680539Z Build succeeded.
2026-11-04T18:55:03.8700530Z     0 Warning(s)
2026-11-04T18:55:03.8701301Z     0 Error(s)
2026-11-04T18:55:03.8701521Z 
2026-11-04T18:55:03.8701725Z Time Elapsed 00:00:22.44
2026-11-04T18:55:03.8988448Z ##[group]Run {
2026-11-04T18:55:03.8988806Z [36;1m{[0m
2026-11-04T18:55:03.8989155Z [36;1m  echo "### 1 · Restauro e compilação (QG-01)"[0m
2026-11-04T18:55:03.8989568Z [36;1m  echo ""[0m
2026-11-04T18:55:03.8990272Z [36;1m  echo "✅ A solução \`$SOLUCAO\` foi restaurada e compilada em \`$CONFIGURACAO\` sem erros nem avisos (quality gate da I101)."[0m
2026-11-04T18:55:03.8991497Z [36;1m  echo ""[0m
2026-11-04T18:55:03.8991816Z [36;1m} >> "$GITHUB_STEP_SUMMARY"[0m
2026-11-04T18:55:03.9016819Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:55:03.9017297Z env:
2026-11-04T18:55:03.9017590Z   DOTNET_NOLOGO: true
2026-11-04T18:55:03.9017914Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:03.9018276Z   CONFIGURACAO: Release
2026-11-04T18:55:03.9018625Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:03.9019331Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:03.9020412Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:03.9021410Z   RESULTADOS: resultados-ci
2026-11-04T18:55:03.9021761Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:03.9022204Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:03.9022614Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:55:03.9022970Z ##[endgroup]
2026-11-04T18:55:03.9160641Z ##[group]Run actions/upload-artifact@cf430e030ddbb5b0abf93d22962f4752f3646cd9
2026-11-04T18:55:03.9161394Z with:
2026-11-04T18:55:03.9161683Z   name: relatorio-analise-estatica-sarif
2026-11-04T18:55:03.9162086Z   path: resultados-ci/analise-estatica
2026-11-04T18:55:03.9162439Z   if-no-files-found: warn
2026-11-04T18:55:03.9162745Z   retention-days: 90
2026-11-04T18:55:03.9163024Z   compression-level: 6
2026-11-04T18:55:03.9163318Z   overwrite: false
2026-11-04T18:55:03.9163601Z   include-hidden-files: false
2026-11-04T18:55:03.9163919Z   archive: true
2026-11-04T18:55:03.9164173Z env:
2026-11-04T18:55:03.9164417Z   DOTNET_NOLOGO: true
2026-11-04T18:55:03.9164715Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:03.9165053Z   CONFIGURACAO: Release
2026-11-04T18:55:03.9165390Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:03.9166034Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:03.9167079Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:03.9167819Z   RESULTADOS: resultados-ci
2026-11-04T18:55:03.9168139Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:03.9168486Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:03.9168846Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:55:03.9169163Z ##[endgroup]
2026-11-04T18:55:04.0515874Z With the provided path, there will be 6 files uploaded
2026-11-04T18:55:04.0516572Z Artifact name is valid!
2026-11-04T18:55:04.0516998Z Root directory input is valid!
2026-11-04T18:55:04.3648923Z Uploading artifact: relatorio-analise-estatica-sarif.zip
2026-11-04T18:55:04.3698011Z Beginning upload of artifact content to blob storage
2026-11-04T18:55:04.5579772Z Uploaded bytes 306800
2026-11-04T18:55:04.5699150Z Finished uploading artifact content to blob storage!
2026-11-04T18:55:04.5700033Z SHA256 digest of uploaded artifact is b9ba310cdb8d90f849ae2c7e66af4ae8e43dffa20b16e6bc59fb585b4bb11af7
2026-11-04T18:55:04.5700749Z Finalizing artifact upload
2026-11-04T18:55:05.0064777Z Artifact relatorio-analise-estatica-sarif successfully finalized. Artifact ID 13353587215
2026-11-04T18:55:05.0065933Z Artifact relatorio-analise-estatica-sarif has been successfully uploaded! Final size is 306800 bytes. Artifact ID is 13353587215
2026-11-04T18:55:05.0069285Z Artifact download URL: https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/actions/runs/41891706460/artifacts/13353587215
2026-11-04T18:55:05.0576032Z Post job cleanup.
2026-11-04T18:55:05.0582033Z ##[start-action display=Reutilizar a cache dos pacotes NuGet;id=__c954a7c8-c66e-4776-a88c-af31f4e6735a.__actions_cache]
2026-11-04T18:55:05.3644063Z Post job cleanup.
2026-11-04T18:55:05.4758429Z Cache hit occurred on the primary key nuget-Linux-c8252acdcad9b687c3131c725ee3c1b10105ceb23478247a43e8389c12c19830, not saving cache.
2026-11-04T18:55:05.4829578Z ##[end-action id=__c954a7c8-c66e-4776-a88c-af31f4e6735a.__actions_cache;outcome=success;conclusion=success;duration_ms=424]
2026-11-04T18:55:05.4833641Z ##[start-action display=Instalar o SDK do .NET 10;id=__c954a7c8-c66e-4776-a88c-af31f4e6735a.__actions_setup-dotnet]
2026-11-04T18:55:05.5428068Z Post job cleanup.
2026-11-04T18:55:05.6752406Z ##[end-action id=__c954a7c8-c66e-4776-a88c-af31f4e6735a.__actions_setup-dotnet;outcome=success;conclusion=success;duration_ms=191]
2026-11-04T18:55:05.7588152Z Post job cleanup.
2026-11-04T18:55:05.8448364Z [command]/usr/bin/git version
2026-11-04T18:55:05.8498361Z git version 2.56.0
2026-11-04T18:55:05.8541507Z Temporarily overriding HOME='/home/runner/work/_temp/127267f9-04b9-4d48-ade6-d3ac4beaa751' before making global git config changes
2026-11-04T18:55:05.8546181Z Adding repository directory to the temporary git global config as a safe directory
2026-11-04T18:55:05.8547477Z [command]/usr/bin/git config --global --add safe.directory /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:55:05.9465156Z Removing SSH command configuration
2026-11-04T18:55:05.9476206Z [command]/usr/bin/git config --local --name-only --get-regexp core\.sshCommand
2026-11-04T18:55:05.9532217Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'core\.sshCommand' && git config --local --unset-all 'core.sshCommand' || :"
2026-11-04T18:55:05.9793836Z Removing HTTP extra header
2026-11-04T18:55:05.9799526Z [command]/usr/bin/git config --local --name-only --get-regexp http\.https\:\/\/github\.com\/\.extraheader
2026-11-04T18:55:05.9844027Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'http\.https\:\/\/github\.com\/\.extraheader' && git config --local --unset-all 'http.https://github.com/.extraheader' || :"
2026-11-04T18:55:06.0062549Z Removing includeIf entries pointing to credentials config files
2026-11-04T18:55:06.0069425Z [command]/usr/bin/git config --local --name-only --get-regexp ^includeIf\.gitdir:
2026-11-04T18:55:06.0113485Z [command]/usr/bin/git submodule foreach --recursive git config --local --show-origin --name-only --get-regexp remote.origin.url
2026-11-04T18:55:06.0483259Z Cleaning up orphan processes
2026-11-04T18:55:06.0757085Z Terminate orphan process: pid (2487) (dotnet)
2026-11-04T18:55:06.0778845Z Terminate orphan process: pid (2488) (dotnet)
2026-11-04T18:55:06.0824816Z Terminate orphan process: pid (2489) (dotnet)
2026-11-04T18:55:06.0853744Z Terminate orphan process: pid (2596) (VBCSCompiler)
``````
