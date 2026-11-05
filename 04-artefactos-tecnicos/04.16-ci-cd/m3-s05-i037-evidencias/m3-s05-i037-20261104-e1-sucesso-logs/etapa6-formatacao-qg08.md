``````text
﻿2026-11-04T18:38:18.1927006Z Current runner version: '2.338.2'
2026-11-04T18:38:18.1961220Z ##[group]Runner Image Provisioner
2026-11-04T18:38:18.1962503Z Hosted Compute Agent
2026-11-04T18:38:18.1963566Z Version: 20261021.618
2026-11-04T18:38:18.1964635Z Commit: 21505ad2541d5be6135ed84fc33697146cf75b8e
2026-11-04T18:38:18.1966029Z Build Date: 2026-10-21T00:45:21Z
2026-11-04T18:38:18.1967645Z Worker ID: {71f4ebb2-c389-4110-a212-195dd9e7f148}
2026-11-04T18:38:18.1968967Z Region: eastus
2026-11-04T18:38:18.1969952Z Cloud: Azure
2026-11-04T18:38:18.1970857Z ##[endgroup]
2026-11-04T18:38:18.1973279Z ##[group]Operating System
2026-11-04T18:38:18.1974290Z Ubuntu
2026-11-04T18:38:18.1975261Z 24.04.5
2026-11-04T18:38:18.1976146Z LTS
2026-11-04T18:38:18.1977300Z ##[endgroup]
2026-11-04T18:38:18.1978315Z ##[group]Runner Image
2026-11-04T18:38:18.1979420Z Image: ubuntu-24.04
2026-11-04T18:38:18.1980400Z Version: 20261025.353.1
2026-11-04T18:38:18.1982543Z Included Software: https://github.com/actions/runner-images/blob/ubuntu24/20261025.353/images/ubuntu/Ubuntu2404-Readme.md
2026-11-04T18:38:18.1985344Z Image Release: https://github.com/actions/runner-images/releases/tag/ubuntu24%2F20261025.353
2026-11-04T18:38:18.1987330Z ##[endgroup]
2026-11-04T18:38:18.1989236Z ##[group]GITHUB_TOKEN Permissions
2026-11-04T18:38:18.1992210Z Contents: read
2026-11-04T18:38:18.1993237Z Metadata: read
2026-11-04T18:38:18.1994204Z ##[endgroup]
2026-11-04T18:38:18.1997672Z Secret source: Actions
2026-11-04T18:38:18.1999199Z Cache mode: write
2026-11-04T18:38:18.2000961Z Prepare workflow directory
2026-11-04T18:38:18.4205046Z Prepare all required actions
2026-11-04T18:38:18.4254166Z Getting action download info
2026-11-04T18:38:18.5790051Z Download action repository 'actions/checkout@3d3c42e5aac5ba805825da76410c181273ba90b1' (SHA:3d3c42e5aac5ba805825da76410c181273ba90b1)
2026-11-04T18:38:18.7456628Z Download action repository 'actions/upload-artifact@cf430e030ddbb5b0abf93d22962f4752f3646cd9' (SHA:cf430e030ddbb5b0abf93d22962f4752f3646cd9)
2026-11-04T18:38:18.9784499Z Complete job name: 6 · Formatação (QG-08)
2026-11-04T18:38:19.2244059Z ##[group]Run actions/checkout@3d3c42e5aac5ba805825da76410c181273ba90b1
2026-11-04T18:38:19.2245640Z with:
2026-11-04T18:38:19.2246130Z   persist-credentials: false
2026-11-04T18:38:19.2247057Z   repository: LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:38:19.2251160Z   token: ***
2026-11-04T18:38:19.2251603Z   ssh-strict: true
2026-11-04T18:38:19.2252061Z   ssh-user: git
2026-11-04T18:38:19.2252481Z   clean: true
2026-11-04T18:38:19.2252933Z   sparse-checkout-cone-mode: true
2026-11-04T18:38:19.2253458Z   fetch-depth: 1
2026-11-04T18:38:19.2253898Z   fetch-tags: false
2026-11-04T18:38:19.2254367Z   show-progress: true
2026-11-04T18:38:19.2254854Z   lfs: false
2026-11-04T18:38:19.2255274Z   submodules: false
2026-11-04T18:38:19.2255748Z   set-safe-directory: true
2026-11-04T18:38:19.2256268Z   allow-unsafe-pr-checkout: false
2026-11-04T18:38:19.2257565Z env:
2026-11-04T18:38:19.2258264Z   DOTNET_NOLOGO: true
2026-11-04T18:38:19.2258972Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:38:19.2259539Z   CONFIGURACAO: Release
2026-11-04T18:38:19.2260078Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:38:19.2261029Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:38:19.2262399Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:38:19.2263430Z   RESULTADOS: resultados-ci
2026-11-04T18:38:19.2263956Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:38:19.2264518Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:38:19.2265095Z ##[endgroup]
2026-11-04T18:38:19.9011790Z Syncing repository: LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:38:19.9014126Z ##[group]Getting Git version info
2026-11-04T18:38:19.9015469Z Working directory is '/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO'
2026-11-04T18:38:19.9017071Z [command]/usr/bin/git version
2026-11-04T18:38:19.9101340Z git version 2.56.0
2026-11-04T18:38:19.9124399Z ##[endgroup]
2026-11-04T18:38:19.9152005Z Temporarily overriding HOME='/home/runner/work/_temp/c2ebd6d4-f8aa-40e7-83f0-b3ad73a1d97f' before making global git config changes
2026-11-04T18:38:19.9154728Z Adding repository directory to the temporary git global config as a safe directory
2026-11-04T18:38:19.9159358Z [command]/usr/bin/git config --global --add safe.directory /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:38:19.9383868Z Deleting the contents of '/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO'
2026-11-04T18:38:19.9387519Z ##[group]Determining repository object format
2026-11-04T18:38:19.9389240Z ##[endgroup]
2026-11-04T18:38:19.9390723Z ##[group]Initializing the repository
2026-11-04T18:38:19.9403136Z [command]/usr/bin/git init /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:38:19.9510480Z hint: Using 'master' as the name for the initial branch. This default branch name
2026-11-04T18:38:19.9513075Z hint: will change to "main" in Git 3.0. To configure the initial branch name
2026-11-04T18:38:19.9515651Z hint: to use in all of your new repositories, which will suppress this warning,
2026-11-04T18:38:19.9517803Z hint: call:
2026-11-04T18:38:19.9518794Z hint:
2026-11-04T18:38:19.9519830Z hint: 	git config --global init.defaultBranch <name>
2026-11-04T18:38:19.9520697Z hint:
2026-11-04T18:38:19.9521553Z hint: Names commonly chosen instead of 'master' are 'main', 'trunk' and
2026-11-04T18:38:19.9522608Z hint: 'development'. The just-created branch can be renamed via this command:
2026-11-04T18:38:19.9523429Z hint:
2026-11-04T18:38:19.9523907Z hint: 	git branch -m <name>
2026-11-04T18:38:19.9524450Z hint:
2026-11-04T18:38:19.9525170Z hint: Disable this message with "git config set advice.defaultBranchName false"
2026-11-04T18:38:19.9538941Z Initialized empty Git repository in /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/
2026-11-04T18:38:19.9551448Z [command]/usr/bin/git remote add origin https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:38:19.9640122Z ##[endgroup]
2026-11-04T18:38:19.9641923Z ##[group]Disabling automatic garbage collection
2026-11-04T18:38:19.9643489Z [command]/usr/bin/git config --local gc.auto 0
2026-11-04T18:38:19.9672014Z ##[endgroup]
2026-11-04T18:38:19.9673705Z ##[group]Setting up auth
2026-11-04T18:38:19.9674904Z Removing SSH command configuration
2026-11-04T18:38:19.9690419Z [command]/usr/bin/git config --local --name-only --get-regexp core\.sshCommand
2026-11-04T18:38:19.9737888Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'core\.sshCommand' && git config --local --unset-all 'core.sshCommand' || :"
2026-11-04T18:38:20.0168559Z Removing HTTP extra header
2026-11-04T18:38:20.0174607Z [command]/usr/bin/git config --local --name-only --get-regexp http\.https\:\/\/github\.com\/\.extraheader
2026-11-04T18:38:20.0217229Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'http\.https\:\/\/github\.com\/\.extraheader' && git config --local --unset-all 'http.https://github.com/.extraheader' || :"
2026-11-04T18:38:20.0460728Z Removing includeIf entries pointing to credentials config files
2026-11-04T18:38:20.0465178Z [command]/usr/bin/git config --local --name-only --get-regexp ^includeIf\.gitdir:
2026-11-04T18:38:20.0507875Z [command]/usr/bin/git submodule foreach --recursive git config --local --show-origin --name-only --get-regexp remote.origin.url
2026-11-04T18:38:20.0751474Z [command]/usr/bin/git config --file /home/runner/work/_temp/git-credentials-b6a35259-78b7-42ba-98fd-1a47f7f3bcd4.config http.https://github.com/.extraheader AUTHORIZATION: basic ***
2026-11-04T18:38:20.0800168Z [command]/usr/bin/git config --local includeIf.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path /home/runner/work/_temp/git-credentials-b6a35259-78b7-42ba-98fd-1a47f7f3bcd4.config
2026-11-04T18:38:20.0842479Z [command]/usr/bin/git config --local includeIf.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path /home/runner/work/_temp/git-credentials-b6a35259-78b7-42ba-98fd-1a47f7f3bcd4.config
2026-11-04T18:38:20.0887407Z [command]/usr/bin/git config --local includeIf.gitdir:/github/workspace/.git.path /github/runner_temp/git-credentials-b6a35259-78b7-42ba-98fd-1a47f7f3bcd4.config
2026-11-04T18:38:20.0932053Z [command]/usr/bin/git config --local includeIf.gitdir:/github/workspace/.git/worktrees/*.path /github/runner_temp/git-credentials-b6a35259-78b7-42ba-98fd-1a47f7f3bcd4.config
2026-11-04T18:38:20.0974377Z ##[endgroup]
2026-11-04T18:38:20.0975997Z ##[group]Fetching the repository
2026-11-04T18:38:20.0983135Z [command]/usr/bin/git -c protocol.version=2 fetch --no-tags --prune --no-recurse-submodules --depth=1 origin +89d6b31c5e788b4e8e5cfc7178d991ef65856c06:refs/remotes/origin/develop
2026-11-04T18:38:21.5438646Z From https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:38:21.5441746Z  * [new ref]         89d6b31c5e788b4e8e5cfc7178d991ef65856c06 -> origin/develop
2026-11-04T18:38:21.5446634Z [command]/usr/bin/git branch --list --remote origin/develop
2026-11-04T18:38:21.5483903Z   origin/develop
2026-11-04T18:38:21.5493464Z [command]/usr/bin/git rev-parse refs/remotes/origin/develop
2026-11-04T18:38:21.5528374Z 89d6b31c5e788b4e8e5cfc7178d991ef65856c06
2026-11-04T18:38:21.5534359Z ##[endgroup]
2026-11-04T18:38:21.5536477Z ##[group]Determining the checkout info
2026-11-04T18:38:21.5538807Z ##[endgroup]
2026-11-04T18:38:21.5540500Z [command]/usr/bin/git sparse-checkout disable
2026-11-04T18:38:21.5603778Z [command]/usr/bin/git config --local --unset-all extensions.worktreeConfig
2026-11-04T18:38:21.5824812Z ##[group]Checking out the ref
2026-11-04T18:38:21.5827797Z [command]/usr/bin/git checkout --progress --force -B develop refs/remotes/origin/develop
2026-11-04T18:38:21.7840266Z Switched to a new branch 'develop'
2026-11-04T18:38:21.7845710Z branch 'develop' set up to track 'origin/develop'.
2026-11-04T18:38:21.7873653Z ##[endgroup]
2026-11-04T18:38:21.7932613Z [command]/usr/bin/git log -1 --format=%H
2026-11-04T18:38:21.7970458Z 89d6b31c5e788b4e8e5cfc7178d991ef65856c06
2026-11-04T18:38:21.7981632Z ##[group]Removing auth
2026-11-04T18:38:21.7982140Z Removing SSH command configuration
2026-11-04T18:38:21.7986499Z [command]/usr/bin/git config --local --name-only --get-regexp core\.sshCommand
2026-11-04T18:38:21.8030801Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'core\.sshCommand' && git config --local --unset-all 'core.sshCommand' || :"
2026-11-04T18:38:21.8271406Z Removing HTTP extra header
2026-11-04T18:38:21.8276055Z [command]/usr/bin/git config --local --name-only --get-regexp http\.https\:\/\/github\.com\/\.extraheader
2026-11-04T18:38:21.8320483Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'http\.https\:\/\/github\.com\/\.extraheader' && git config --local --unset-all 'http.https://github.com/.extraheader' || :"
2026-11-04T18:38:21.8559475Z Removing includeIf entries pointing to credentials config files
2026-11-04T18:38:21.8566303Z [command]/usr/bin/git config --local --name-only --get-regexp ^includeIf\.gitdir:
2026-11-04T18:38:21.8608588Z includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path
2026-11-04T18:38:21.8609439Z includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path
2026-11-04T18:38:21.8610045Z includeif.gitdir:/github/workspace/.git.path
2026-11-04T18:38:21.8610513Z includeif.gitdir:/github/workspace/.git/worktrees/*.path
2026-11-04T18:38:21.8617781Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path
2026-11-04T18:38:21.8651355Z /home/runner/work/_temp/git-credentials-b6a35259-78b7-42ba-98fd-1a47f7f3bcd4.config
2026-11-04T18:38:21.8661700Z [command]/usr/bin/git config --local --unset includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path \/home\/runner\/work\/_temp\/git\-credentials\-b6a35259\-78b7\-42ba\-98fd\-1a47f7f3bcd4\.config
2026-11-04T18:38:21.8708969Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path
2026-11-04T18:38:21.8742940Z /home/runner/work/_temp/git-credentials-b6a35259-78b7-42ba-98fd-1a47f7f3bcd4.config
2026-11-04T18:38:21.8751806Z [command]/usr/bin/git config --local --unset includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path \/home\/runner\/work\/_temp\/git\-credentials\-b6a35259\-78b7\-42ba\-98fd\-1a47f7f3bcd4\.config
2026-11-04T18:38:21.8798607Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/github/workspace/.git.path
2026-11-04T18:38:21.8833471Z /github/runner_temp/git-credentials-b6a35259-78b7-42ba-98fd-1a47f7f3bcd4.config
2026-11-04T18:38:21.8842684Z [command]/usr/bin/git config --local --unset includeif.gitdir:/github/workspace/.git.path \/github\/runner_temp\/git\-credentials\-b6a35259\-78b7\-42ba\-98fd\-1a47f7f3bcd4\.config
2026-11-04T18:38:21.8889160Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/github/workspace/.git/worktrees/*.path
2026-11-04T18:38:21.8924384Z /github/runner_temp/git-credentials-b6a35259-78b7-42ba-98fd-1a47f7f3bcd4.config
2026-11-04T18:38:21.8933372Z [command]/usr/bin/git config --local --unset includeif.gitdir:/github/workspace/.git/worktrees/*.path \/github\/runner_temp\/git\-credentials\-b6a35259\-78b7\-42ba\-98fd\-1a47f7f3bcd4\.config
2026-11-04T18:38:21.8979218Z [command]/usr/bin/git submodule foreach --recursive git config --local --show-origin --name-only --get-regexp remote.origin.url
2026-11-04T18:38:21.9220114Z Removing credentials config '/home/runner/work/_temp/git-credentials-b6a35259-78b7-42ba-98fd-1a47f7f3bcd4.config'
2026-11-04T18:38:21.9229190Z ##[endgroup]
2026-11-04T18:38:21.9751876Z ##[group]Run FINAL="$(jq -r '."QG-08".finalDeLinha' "$QUALITY_GATES")"
2026-11-04T18:38:21.9752470Z [36;1mFINAL="$(jq -r '."QG-08".finalDeLinha' "$QUALITY_GATES")"[0m
2026-11-04T18:38:21.9752861Z [36;1mif [ "$FINAL" = "crlf" ]; then[0m
2026-11-04T18:38:21.9753161Z [36;1m  git config core.autocrlf true[0m
2026-11-04T18:38:21.9753455Z [36;1m  git rm -r -q --cached .[0m
2026-11-04T18:38:21.9753720Z [36;1m  git reset -q --hard HEAD[0m
2026-11-04T18:38:21.9754158Z [36;1m  echo "Cópia de trabalho convertida para CRLF, como num checkout em Windows."[0m
2026-11-04T18:38:21.9754575Z [36;1melse[0m
2026-11-04T18:38:21.9754865Z [36;1m  echo "Final de linha configurado: ${FINAL}; sem conversão."[0m
2026-11-04T18:38:21.9755209Z [36;1mfi[0m
2026-11-04T18:38:21.9779207Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:38:21.9779581Z env:
2026-11-04T18:38:21.9779776Z   DOTNET_NOLOGO: true
2026-11-04T18:38:21.9780009Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:38:21.9780339Z   CONFIGURACAO: Release
2026-11-04T18:38:21.9780605Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:38:21.9781116Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:38:21.9781875Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:38:21.9782421Z   RESULTADOS: resultados-ci
2026-11-04T18:38:21.9782669Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:38:21.9782946Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:38:21.9783221Z ##[endgroup]
2026-11-04T18:38:23.1061979Z Cópia de trabalho convertida para CRLF, como num checkout em Windows.
2026-11-04T18:38:23.1732471Z Prepare all required actions
2026-11-04T18:38:23.1733011Z Getting action download info
2026-11-04T18:38:23.2515962Z Download action repository 'actions/setup-dotnet@a98b56852c35b8e3190ac28c8c2271da59106c68' (SHA:a98b56852c35b8e3190ac28c8c2271da59106c68)
2026-11-04T18:38:23.4220775Z Download action repository 'actions/cache@55cc8345863c7cc4c66a329aec7e433d2d1c52a9' (SHA:55cc8345863c7cc4c66a329aec7e433d2d1c52a9)
2026-11-04T18:38:23.6082237Z ##[group]Run ./.github/actions/preparar-dotnet
2026-11-04T18:38:23.6082559Z env:
2026-11-04T18:38:23.6082748Z   DOTNET_NOLOGO: true
2026-11-04T18:38:23.6082981Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:38:23.6083236Z   CONFIGURACAO: Release
2026-11-04T18:38:23.6083490Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:38:23.6083996Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:38:23.6084791Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:38:23.6085342Z   RESULTADOS: resultados-ci
2026-11-04T18:38:23.6085587Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:38:23.6085856Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:38:23.6086139Z ##[endgroup]
2026-11-04T18:38:23.6325642Z ##[start-action display=Instalar o SDK do .NET 10;id=__self.__actions_setup-dotnet]
2026-11-04T18:38:23.7412592Z ##[group]Run actions/setup-dotnet@a98b56852c35b8e3190ac28c8c2271da59106c68
2026-11-04T18:38:23.7412974Z with:
2026-11-04T18:38:23.7413166Z   dotnet-version: 10.0.x
2026-11-04T18:38:23.7413385Z   cache: false
2026-11-04T18:38:23.7413573Z env:
2026-11-04T18:38:23.7413746Z   DOTNET_NOLOGO: true
2026-11-04T18:38:23.7413963Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:38:23.7414209Z   CONFIGURACAO: Release
2026-11-04T18:38:23.7414442Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:38:23.7414952Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:38:23.7415698Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:38:23.7416252Z   RESULTADOS: resultados-ci
2026-11-04T18:38:23.7416487Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:38:23.7416941Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:38:23.7417207Z ##[endgroup]
2026-11-04T18:38:23.8833277Z [command]/home/runner/work/_actions/actions/setup-dotnet/a98b56852c35b8e3190ac28c8c2271da59106c68/externals/install-dotnet.sh --skip-non-versioned-files --runtime dotnet --channel LTS
2026-11-04T18:38:24.1129805Z dotnet-install: .NET Core Runtime with version '10.0.14' is already installed.
2026-11-04T18:38:24.1157583Z [command]/home/runner/work/_actions/actions/setup-dotnet/a98b56852c35b8e3190ac28c8c2271da59106c68/externals/install-dotnet.sh --skip-non-versioned-files --channel 10.0
2026-11-04T18:38:24.3083972Z dotnet-install: .NET Core SDK with version '10.0.403' is already installed.
2026-11-04T18:38:24.3216239Z ##[end-action id=__self.__actions_setup-dotnet;outcome=success;conclusion=success;duration_ms=688]
2026-11-04T18:38:24.3251082Z ##[start-action display=Reutilizar a cache dos pacotes NuGet;id=__self.__actions_cache]
2026-11-04T18:38:24.6660019Z ##[group]Run actions/cache@55cc8345863c7cc4c66a329aec7e433d2d1c52a9
2026-11-04T18:38:24.6660385Z with:
2026-11-04T18:38:24.6660591Z   path: ~/.nuget/packages
2026-11-04T18:38:24.6660989Z   key: nuget-Linux-4fa7929841d08bf5cf3cfe7c971093d27a71a5e6d01a1afb1fc9c22373e27b88
2026-11-04T18:38:24.6661436Z   restore-keys: nuget-Linux-

2026-11-04T18:38:24.6661690Z   enableCrossOsArchive: false
2026-11-04T18:38:24.6661937Z   fail-on-cache-miss: false
2026-11-04T18:38:24.6662171Z   lookup-only: false
2026-11-04T18:38:24.6662385Z   save-always: false
2026-11-04T18:38:24.6662589Z env:
2026-11-04T18:38:24.6662772Z   DOTNET_NOLOGO: true
2026-11-04T18:38:24.6663000Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:38:24.6663256Z   CONFIGURACAO: Release
2026-11-04T18:38:24.6663775Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:38:24.6664313Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:38:24.6665068Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:38:24.6665753Z   RESULTADOS: resultados-ci
2026-11-04T18:38:24.6666005Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:38:24.6666286Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:38:24.6666575Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:38:24.6667115Z ##[endgroup]
2026-11-04T18:38:24.8584271Z Cache hit for: nuget-Linux-4fa7929841d08bf5cf3cfe7c971093d27a71a5e6d01a1afb1fc9c22373e27b88
2026-11-04T18:38:25.9128445Z Received 167772160 of 283339035 (59.2%), 159.7 MBs/sec
2026-11-04T18:38:26.3576326Z Received 283339035 of 283339035 (100.0%), 186.7 MBs/sec
2026-11-04T18:38:26.3580010Z Cache Size: ~270 MB (283339035 B)
2026-11-04T18:38:26.3623019Z [command]/usr/bin/tar -xf /home/runner/work/_temp/9a6f8831-84fc-47e0-a31f-463de1b48e1f/cache.tzst -P -C /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO --use-compress-program unzstd
2026-11-04T18:38:27.7987884Z Cache restored successfully
2026-11-04T18:38:27.8219630Z Cache restored from key: nuget-Linux-4fa7929841d08bf5cf3cfe7c971093d27a71a5e6d01a1afb1fc9c22373e27b88
2026-11-04T18:38:27.8525635Z ##[end-action id=__self.__actions_cache;outcome=success;conclusion=success;duration_ms=3527]
2026-11-04T18:38:27.8557660Z ##[start-action display=Registar o SDK utilizado;id=__self.__run]
2026-11-04T18:38:27.8609913Z ##[group]Run echo "SDK do .NET: $(dotnet --version)"
2026-11-04T18:38:27.8610683Z [36;1mecho "SDK do .NET: $(dotnet --version)"[0m
2026-11-04T18:38:27.8611343Z [36;1m{[0m
2026-11-04T18:38:27.8611924Z [36;1m  echo "### Ambiente .NET"[0m
2026-11-04T18:38:27.8612463Z [36;1m  echo ""[0m
2026-11-04T18:38:27.8613038Z [36;1m  echo "| Elemento | Valor |"[0m
2026-11-04T18:38:27.8613620Z [36;1m  echo "|---|---|"[0m
2026-11-04T18:38:27.8614374Z [36;1m  echo "| SDK do .NET | \`$(dotnet --version)\` |"[0m
2026-11-04T18:38:27.8615224Z [36;1m  echo "| global.json | \`$(tr -d '\n\r ' < src/backend/global.json)\` |"[0m
2026-11-04T18:38:27.8616130Z [36;1m  echo "| Runner | \`${RUNNER_OS} ${ImageOS:-} ${ImageVersion:-}\` |"[0m
2026-11-04T18:38:27.8617155Z [36;1m  echo ""[0m
2026-11-04T18:38:27.8617745Z [36;1m} >> "$GITHUB_STEP_SUMMARY"[0m
2026-11-04T18:38:27.8644848Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:38:27.8645591Z env:
2026-11-04T18:38:27.8646048Z   DOTNET_NOLOGO: true
2026-11-04T18:38:27.8646634Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:38:27.8647571Z   CONFIGURACAO: Release
2026-11-04T18:38:27.8648128Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:38:27.8649226Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:38:27.8650694Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:38:27.8651811Z   RESULTADOS: resultados-ci
2026-11-04T18:38:27.8652435Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:38:27.8653075Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:38:27.8653754Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:38:27.8654283Z ##[endgroup]
2026-11-04T18:38:30.5745798Z SDK do .NET: 10.0.403
2026-11-04T18:38:30.6634930Z ##[end-action id=__self.__run;outcome=success;conclusion=success;duration_ms=2807]
2026-11-04T18:38:30.6760281Z ##[group]Run dotnet restore "$SOLUCAO"
2026-11-04T18:38:30.6760618Z [36;1mdotnet restore "$SOLUCAO"[0m
2026-11-04T18:38:30.6781402Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:38:30.6781753Z env:
2026-11-04T18:38:30.6781976Z   DOTNET_NOLOGO: true
2026-11-04T18:38:30.6782235Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:38:30.6782505Z   CONFIGURACAO: Release
2026-11-04T18:38:30.6782757Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:38:30.6783268Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:38:30.6784031Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:38:30.6784779Z   RESULTADOS: resultados-ci
2026-11-04T18:38:30.6785027Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:38:30.6785297Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:38:30.6785584Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:38:30.6785846Z ##[endgroup]
2026-11-04T18:38:34.4460307Z   Determining projects to restore...
2026-11-04T18:38:36.1389032Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Domain/ClinicaVeterinaria.Domain.csproj (in 207 ms).
2026-11-04T18:38:37.3909858Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Infrastructure/ClinicaVeterinaria.Infrastructure.csproj (in 1.36 sec).
2026-11-04T18:38:37.4288251Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj (in 1.5 sec).
2026-11-04T18:38:37.4903070Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj (in 1.21 sec).
2026-11-04T18:38:37.4906176Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj (in 1.52 sec).
2026-11-04T18:38:37.4909648Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Application/ClinicaVeterinaria.Application.csproj (in 2 ms).
2026-11-04T18:38:37.5897159Z ##[group]Run mkdir -p "$RESULTADOS/formatacao"
2026-11-04T18:38:37.5897614Z [36;1mmkdir -p "$RESULTADOS/formatacao"[0m
2026-11-04T18:38:37.5897908Z [36;1mset +e[0m
2026-11-04T18:38:37.5898291Z [36;1mdotnet format "$SOLUCAO" --verify-no-changes --no-restore --verbosity minimal \[0m
2026-11-04T18:38:37.5898768Z [36;1m  --report "$RESULTADOS/formatacao"[0m
2026-11-04T18:38:37.5899082Z [36;1mecho "codigo=$?" >> "$GITHUB_OUTPUT"[0m
2026-11-04T18:38:37.5899378Z [36;1mexit 0[0m
2026-11-04T18:38:37.5918829Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:38:37.5919167Z env:
2026-11-04T18:38:37.5919358Z   DOTNET_NOLOGO: true
2026-11-04T18:38:37.5919596Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:38:37.5919853Z   CONFIGURACAO: Release
2026-11-04T18:38:37.5920118Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:38:37.5920623Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:38:37.5921425Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:38:37.5921982Z   RESULTADOS: resultados-ci
2026-11-04T18:38:37.5922228Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:38:37.5922514Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:38:37.5922796Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:38:37.5923034Z ##[endgroup]
2026-11-04T18:39:07.0913319Z ##[group]Run python3 - <<'PYTHON'
2026-11-04T18:39:07.0913702Z [36;1mpython3 - <<'PYTHON'[0m
2026-11-04T18:39:07.0913953Z [36;1mimport json, os, sys[0m
2026-11-04T18:39:07.0914333Z [36;1mcfg = json.load(open(os.environ["QUALITY_GATES"], encoding="utf-8"))["QG-08"][0m
2026-11-04T18:39:07.0914814Z [36;1mmaximo = int(cfg["maximoFicheirosPorFormatar"])[0m
2026-11-04T18:39:07.0915197Z [36;1mcodigo = int(os.environ.get("CODIGO_FORMAT") or 1)[0m
2026-11-04T18:39:07.0915685Z [36;1mcaminho = os.path.join(os.environ["RESULTADOS"], "formatacao", "format-report.json")[0m
2026-11-04T18:39:07.0916349Z [36;1mrelatorio = json.load(open(caminho, encoding="utf-8-sig")) if os.path.exists(caminho) else None[0m
2026-11-04T18:39:07.0917193Z [36;1mlinhas = ["### 6 · Formatação (QG-08)", ""][0m
2026-11-04T18:39:07.0917539Z [36;1mif relatorio is None or codigo not in (0, 2):[0m
2026-11-04T18:39:07.0918299Z [36;1m    linhas.append(f"❌ **QG-08 não avaliado:** o dotnet format terminou com o código {codigo} e sem relatório utilizável (ver o registo do passo anterior).")[0m
2026-11-04T18:39:07.0919169Z [36;1m    falhou = True[0m
2026-11-04T18:39:07.0919398Z [36;1melse:[0m
2026-11-04T18:39:07.0919759Z [36;1m    ficheiros = sorted({f.get("FilePath") or f.get("FileName") for f in relatorio})[0m
2026-11-04T18:39:07.0920300Z [36;1m    alteracoes = sum(len(f.get("FileChanges") or []) for f in relatorio)[0m
2026-11-04T18:39:07.0920730Z [36;1m    linhas += ["| Medida | Valor |", "|---|---|",[0m
2026-11-04T18:39:07.0921124Z [36;1m               f"| Ficheiros por formatar | **{len(ficheiros)}** |",[0m
2026-11-04T18:39:07.0921538Z [36;1m               f"| Alterações necessárias | {alteracoes} |",[0m
2026-11-04T18:39:07.0921997Z [36;1m               f"| Máximo permitido (`.github/quality-gates.json`) | {maximo} |", ""][0m
2026-11-04T18:39:07.0922404Z [36;1m    if ficheiros:[0m
2026-11-04T18:39:07.0922791Z [36;1m        linhas += ["<details><summary>Ficheiros por formatar (até 30)</summary>", ""][0m
2026-11-04T18:39:07.0923230Z [36;1m        for f in relatorio[:30]:[0m
2026-11-04T18:39:07.0923563Z [36;1m            mudancas = f.get("FileChanges") or [][0m
2026-11-04T18:39:07.0923922Z [36;1m            primeira = mudancas[0] if mudancas else {}[0m
2026-11-04T18:39:07.0924422Z [36;1m            caminho_f = (f.get("FilePath") or f.get("FileName") or "?").split("/src/backend/")[-1][0m
2026-11-04T18:39:07.0925402Z [36;1m            linhas.append(f"- `{caminho_f}` — {len(mudancas)} alteração(ões), por exemplo linha {primeira.get('LineNumber', '?')}: {primeira.get('FormatDescription', '')} ({primeira.get('DiagnosticId', '')})")[0m
2026-11-04T18:39:07.0926216Z [36;1m        linhas += ["", "</details>", ""][0m
2026-11-04T18:39:07.0926533Z [36;1m    falhou = len(ficheiros) > maximo[0m
2026-11-04T18:39:07.0927515Z [36;1m    linhas.append(("❌ **QG-08 não cumprido:** há ficheiros por formatar. Corrigir com `dotnet format src/backend/ClinicaVeterinaria.sln`. O merge fica bloqueado."[0m
2026-11-04T18:39:07.0928365Z [36;1m                   if falhou else "✅ **QG-08 cumprido:** nenhum ficheiro por formatar."))[0m
2026-11-04T18:39:07.0928923Z [36;1mwith open(os.environ["GITHUB_STEP_SUMMARY"], "a", encoding="utf-8") as resumo:[0m
2026-11-04T18:39:07.0929373Z [36;1m    resumo.write("\n".join(linhas) + "\n")[0m
2026-11-04T18:39:07.0929670Z [36;1mprint("\n".join(linhas))[0m
2026-11-04T18:39:07.0929913Z [36;1mif falhou:[0m
2026-11-04T18:39:07.0930367Z [36;1m    print("::error title=QG-08 — formatação::Há ficheiros por formatar (dotnet format --verify-no-changes).")[0m
2026-11-04T18:39:07.0930865Z [36;1m    sys.exit(1)[0m
2026-11-04T18:39:07.0931079Z [36;1mPYTHON[0m
2026-11-04T18:39:07.0950822Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:39:07.0951154Z env:
2026-11-04T18:39:07.0951337Z   DOTNET_NOLOGO: true
2026-11-04T18:39:07.0951559Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:39:07.0951812Z   CONFIGURACAO: Release
2026-11-04T18:39:07.0952246Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:39:07.0952772Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:39:07.0953549Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:39:07.0954100Z   RESULTADOS: resultados-ci
2026-11-04T18:39:07.0954337Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:39:07.0954600Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:39:07.0954875Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:39:07.0955103Z   CODIGO_FORMAT: 0
2026-11-04T18:39:07.0955303Z ##[endgroup]
2026-11-04T18:39:07.1199189Z ### 6 · Formatação (QG-08)
2026-11-04T18:39:07.1199517Z 
2026-11-04T18:39:07.1199694Z | Medida | Valor |
2026-11-04T18:39:07.1200060Z |---|---|
2026-11-04T18:39:07.1200454Z | Ficheiros por formatar | **0** |
2026-11-04T18:39:07.1201126Z | Alterações necessárias | 0 |
2026-11-04T18:39:07.1201874Z | Máximo permitido (`.github/quality-gates.json`) | 0 |
2026-11-04T18:39:07.1202320Z 
2026-11-04T18:39:07.1202710Z ✅ **QG-08 cumprido:** nenhum ficheiro por formatar.
2026-11-04T18:39:07.1478477Z ##[group]Run actions/upload-artifact@cf430e030ddbb5b0abf93d22962f4752f3646cd9
2026-11-04T18:39:07.1478896Z with:
2026-11-04T18:39:07.1479093Z   name: relatorio-formatacao
2026-11-04T18:39:07.1479396Z   path: resultados-ci/formatacao
2026-11-04T18:39:07.1479641Z   if-no-files-found: warn
2026-11-04T18:39:07.1479858Z   retention-days: 90
2026-11-04T18:39:07.1480061Z   compression-level: 6
2026-11-04T18:39:07.1480326Z   overwrite: false
2026-11-04T18:39:07.1480560Z   include-hidden-files: false
2026-11-04T18:39:07.1480786Z   archive: true
2026-11-04T18:39:07.1480972Z env:
2026-11-04T18:39:07.1481144Z   DOTNET_NOLOGO: true
2026-11-04T18:39:07.1481359Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:39:07.1481609Z   CONFIGURACAO: Release
2026-11-04T18:39:07.1481847Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:39:07.1482348Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:39:07.1483134Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:39:07.1483688Z   RESULTADOS: resultados-ci
2026-11-04T18:39:07.1483928Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:39:07.1484196Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:39:07.1484473Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:39:07.1484702Z ##[endgroup]
2026-11-04T18:39:07.3067442Z With the provided path, there will be 1 file uploaded
2026-11-04T18:39:07.3074609Z Artifact name is valid!
2026-11-04T18:39:07.3075509Z Root directory input is valid!
2026-11-04T18:39:07.5107940Z Uploading artifact: relatorio-formatacao.zip
2026-11-04T18:39:07.5148174Z Beginning upload of artifact content to blob storage
2026-11-04T18:39:07.5913669Z Uploaded bytes 154
2026-11-04T18:39:07.6075247Z Finished uploading artifact content to blob storage!
2026-11-04T18:39:07.6075992Z SHA256 digest of uploaded artifact is 4c91f11beecf343f481c98d3cbbf1da4b3dc1e230f652683c804afe5bef42fbd
2026-11-04T18:39:07.6076961Z Finalizing artifact upload
2026-11-04T18:39:08.0874259Z Artifact relatorio-formatacao successfully finalized. Artifact ID 13352598271
2026-11-04T18:39:08.0875351Z Artifact relatorio-formatacao has been successfully uploaded! Final size is 154 bytes. Artifact ID is 13352598271
2026-11-04T18:39:08.0879306Z Artifact download URL: https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/actions/runs/41888968713/artifacts/13352598271
2026-11-04T18:39:08.1160654Z Post job cleanup.
2026-11-04T18:39:08.1192248Z ##[start-action display=Reutilizar a cache dos pacotes NuGet;id=__7c7a1c10-0ae4-4d8c-8f31-708bc0af0750.__actions_cache]
2026-11-04T18:39:08.4810232Z Post job cleanup.
2026-11-04T18:39:08.6053781Z Cache hit occurred on the primary key nuget-Linux-4fa7929841d08bf5cf3cfe7c971093d27a71a5e6d01a1afb1fc9c22373e27b88, not saving cache.
2026-11-04T18:39:08.6132715Z ##[end-action id=__7c7a1c10-0ae4-4d8c-8f31-708bc0af0750.__actions_cache;outcome=success;conclusion=success;duration_ms=493]
2026-11-04T18:39:08.6163639Z ##[start-action display=Instalar o SDK do .NET 10;id=__7c7a1c10-0ae4-4d8c-8f31-708bc0af0750.__actions_setup-dotnet]
2026-11-04T18:39:08.6380935Z Post job cleanup.
2026-11-04T18:39:08.7648735Z ##[end-action id=__7c7a1c10-0ae4-4d8c-8f31-708bc0af0750.__actions_setup-dotnet;outcome=success;conclusion=success;duration_ms=148]
2026-11-04T18:39:08.8155362Z Post job cleanup.
2026-11-04T18:39:08.9063942Z [command]/usr/bin/git version
2026-11-04T18:39:08.9118399Z git version 2.56.0
2026-11-04T18:39:08.9161363Z Temporarily overriding HOME='/home/runner/work/_temp/29e2f67e-a974-4578-8af4-0dfcb03a29f7' before making global git config changes
2026-11-04T18:39:08.9163148Z Adding repository directory to the temporary git global config as a safe directory
2026-11-04T18:39:08.9169002Z [command]/usr/bin/git config --global --add safe.directory /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:39:08.9210427Z Removing SSH command configuration
2026-11-04T18:39:08.9220033Z [command]/usr/bin/git config --local --name-only --get-regexp core\.sshCommand
2026-11-04T18:39:08.9267411Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'core\.sshCommand' && git config --local --unset-all 'core.sshCommand' || :"
2026-11-04T18:39:08.9559778Z Removing HTTP extra header
2026-11-04T18:39:08.9569375Z [command]/usr/bin/git config --local --name-only --get-regexp http\.https\:\/\/github\.com\/\.extraheader
2026-11-04T18:39:08.9617425Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'http\.https\:\/\/github\.com\/\.extraheader' && git config --local --unset-all 'http.https://github.com/.extraheader' || :"
2026-11-04T18:39:08.9879287Z Removing includeIf entries pointing to credentials config files
2026-11-04T18:39:08.9887865Z [command]/usr/bin/git config --local --name-only --get-regexp ^includeIf\.gitdir:
2026-11-04T18:39:08.9938327Z [command]/usr/bin/git submodule foreach --recursive git config --local --show-origin --name-only --get-regexp remote.origin.url
2026-11-04T18:39:09.0372552Z Cleaning up orphan processes
2026-11-04T18:39:09.0693961Z Terminate orphan process: pid (2676) (dotnet)
2026-11-04T18:39:09.0731881Z Terminate orphan process: pid (2677) (dotnet)
2026-11-04T18:39:09.0790409Z Terminate orphan process: pid (2678) (dotnet)
``````
