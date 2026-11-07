``````text
﻿2026-11-04T18:43:39.6806406Z Current runner version: '2.338.2'
2026-11-04T18:43:39.6828288Z ##[group]Runner Image Provisioner
2026-11-04T18:43:39.6828887Z Hosted Compute Agent
2026-11-04T18:43:39.6829384Z Version: 20261021.618
2026-11-04T18:43:39.6829788Z Commit: 21505ad2541d5be6135ed84fc33697146cf75b8e
2026-11-04T18:43:39.6830286Z Build Date: 2026-10-21T00:45:21Z
2026-11-04T18:43:39.6830731Z Worker ID: {57f19812-5d57-41f7-b6a3-cc47bf6025c7}
2026-11-04T18:43:39.6831175Z Region: southcentralus
2026-11-04T18:43:39.6831608Z Cloud: Azure
2026-11-04T18:43:39.6832058Z ##[endgroup]
2026-11-04T18:43:39.6833351Z ##[group]Operating System
2026-11-04T18:43:39.6833766Z Ubuntu
2026-11-04T18:43:39.6834107Z 24.04.5
2026-11-04T18:43:39.6834511Z LTS
2026-11-04T18:43:39.6834847Z ##[endgroup]
2026-11-04T18:43:39.6835190Z ##[group]Runner Image
2026-11-04T18:43:39.6835632Z Image: ubuntu-24.04
2026-11-04T18:43:39.6835992Z Version: 20261025.353.1
2026-11-04T18:43:39.6836913Z Included Software: https://github.com/actions/runner-images/blob/ubuntu24/20261025.353/images/ubuntu/Ubuntu2404-Readme.md
2026-11-04T18:43:39.6837962Z Image Release: https://github.com/actions/runner-images/releases/tag/ubuntu24%2F20261025.353
2026-11-04T18:43:39.6838603Z ##[endgroup]
2026-11-04T18:43:39.6839334Z ##[group]GITHUB_TOKEN Permissions
2026-11-04T18:43:39.6841026Z Contents: read
2026-11-04T18:43:39.6841428Z Metadata: read
2026-11-04T18:43:39.6841839Z ##[endgroup]
2026-11-04T18:43:39.6843667Z Secret source: Actions
2026-11-04T18:43:39.6844335Z Cache mode: write
2026-11-04T18:43:39.6845051Z Prepare workflow directory
2026-11-04T18:43:39.7966624Z Prepare all required actions
2026-11-04T18:43:39.8002316Z Getting action download info
2026-11-04T18:43:40.0769670Z Download action repository 'actions/checkout@3d3c42e5aac5ba805825da76410c181273ba90b1' (SHA:3d3c42e5aac5ba805825da76410c181273ba90b1)
2026-11-04T18:43:40.1551467Z Download action repository 'actions/setup-dotnet@a98b56852c35b8e3190ac28c8c2271da59106c68' (SHA:a98b56852c35b8e3190ac28c8c2271da59106c68)
2026-11-04T18:43:40.8715993Z Download action repository 'actions/download-artifact@9000827ccba6bdab643e8b6fd33ac0654aef8333' (SHA:9000827ccba6bdab643e8b6fd33ac0654aef8333)
2026-11-04T18:43:41.3438153Z Download action repository 'actions/upload-artifact@cf430e030ddbb5b0abf93d22962f4752f3646cd9' (SHA:cf430e030ddbb5b0abf93d22962f4752f3646cd9)
2026-11-04T18:43:41.6278023Z Complete job name: 4 · Cobertura (QG-03)
2026-11-04T18:43:41.8101699Z ##[group]Run actions/checkout@3d3c42e5aac5ba805825da76410c181273ba90b1
2026-11-04T18:43:41.8102919Z with:
2026-11-04T18:43:41.8103479Z   persist-credentials: false
2026-11-04T18:43:41.8104307Z   repository: LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:43:41.8109582Z   token: ***
2026-11-04T18:43:41.8110063Z   ssh-strict: true
2026-11-04T18:43:41.8110553Z   ssh-user: git
2026-11-04T18:43:41.8111033Z   clean: true
2026-11-04T18:43:41.8111540Z   sparse-checkout-cone-mode: true
2026-11-04T18:43:41.8112137Z   fetch-depth: 1
2026-11-04T18:43:41.8112783Z   fetch-tags: false
2026-11-04T18:43:41.8113335Z   show-progress: true
2026-11-04T18:43:41.8113822Z   lfs: false
2026-11-04T18:43:41.8114286Z   submodules: false
2026-11-04T18:43:41.8114798Z   set-safe-directory: true
2026-11-04T18:43:41.8115371Z   allow-unsafe-pr-checkout: false
2026-11-04T18:43:41.8116068Z env:
2026-11-04T18:43:41.8116511Z   DOTNET_NOLOGO: true
2026-11-04T18:43:41.8117080Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:43:41.8117668Z   CONFIGURACAO: Release
2026-11-04T18:43:41.8118244Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:43:41.8119287Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:43:41.8120797Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:43:41.8121935Z   RESULTADOS: resultados-ci
2026-11-04T18:43:41.8122604Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:43:41.8123246Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:43:41.8123872Z ##[endgroup]
2026-11-04T18:43:42.0141661Z Syncing repository: LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:43:42.0144459Z ##[group]Getting Git version info
2026-11-04T18:43:42.0145457Z Working directory is '/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO'
2026-11-04T18:43:42.0147156Z [command]/usr/bin/git version
2026-11-04T18:43:42.0196403Z git version 2.56.0
2026-11-04T18:43:42.0211085Z ##[endgroup]
2026-11-04T18:43:42.0219202Z Temporarily overriding HOME='/home/runner/work/_temp/1ae8a770-aec0-4301-9195-cdc5399335cd' before making global git config changes
2026-11-04T18:43:42.0221728Z Adding repository directory to the temporary git global config as a safe directory
2026-11-04T18:43:42.0224280Z [command]/usr/bin/git config --global --add safe.directory /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:43:42.0269134Z Deleting the contents of '/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO'
2026-11-04T18:43:42.0271654Z ##[group]Determining repository object format
2026-11-04T18:43:42.0273501Z ##[endgroup]
2026-11-04T18:43:42.0274957Z ##[group]Initializing the repository
2026-11-04T18:43:42.0276597Z [command]/usr/bin/git init /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:43:42.0361807Z hint: Using 'master' as the name for the initial branch. This default branch name
2026-11-04T18:43:42.0363964Z hint: will change to "main" in Git 3.0. To configure the initial branch name
2026-11-04T18:43:42.0365632Z hint: to use in all of your new repositories, which will suppress this warning,
2026-11-04T18:43:42.0366970Z hint: call:
2026-11-04T18:43:42.0367652Z hint:
2026-11-04T18:43:42.0368527Z hint: 	git config --global init.defaultBranch <name>
2026-11-04T18:43:42.0369640Z hint:
2026-11-04T18:43:42.0370647Z hint: Names commonly chosen instead of 'master' are 'main', 'trunk' and
2026-11-04T18:43:42.0372440Z hint: 'development'. The just-created branch can be renamed via this command:
2026-11-04T18:43:42.0373740Z hint:
2026-11-04T18:43:42.0374517Z hint: 	git branch -m <name>
2026-11-04T18:43:42.0375352Z hint:
2026-11-04T18:43:42.0376477Z hint: Disable this message with "git config set advice.defaultBranchName false"
2026-11-04T18:43:42.0378713Z Initialized empty Git repository in /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/
2026-11-04T18:43:42.0382699Z [command]/usr/bin/git remote add origin https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:43:42.0426759Z ##[endgroup]
2026-11-04T18:43:42.0428229Z ##[group]Disabling automatic garbage collection
2026-11-04T18:43:42.0429540Z [command]/usr/bin/git config --local gc.auto 0
2026-11-04T18:43:42.0465928Z ##[endgroup]
2026-11-04T18:43:42.0467407Z ##[group]Setting up auth
2026-11-04T18:43:42.0468191Z Removing SSH command configuration
2026-11-04T18:43:42.0470167Z [command]/usr/bin/git config --local --name-only --get-regexp core\.sshCommand
2026-11-04T18:43:42.0508578Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'core\.sshCommand' && git config --local --unset-all 'core.sshCommand' || :"
2026-11-04T18:43:42.0846578Z Removing HTTP extra header
2026-11-04T18:43:42.0852820Z [command]/usr/bin/git config --local --name-only --get-regexp http\.https\:\/\/github\.com\/\.extraheader
2026-11-04T18:43:42.0897902Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'http\.https\:\/\/github\.com\/\.extraheader' && git config --local --unset-all 'http.https://github.com/.extraheader' || :"
2026-11-04T18:43:42.1107653Z Removing includeIf entries pointing to credentials config files
2026-11-04T18:43:42.1112685Z [command]/usr/bin/git config --local --name-only --get-regexp ^includeIf\.gitdir:
2026-11-04T18:43:42.1159437Z [command]/usr/bin/git submodule foreach --recursive git config --local --show-origin --name-only --get-regexp remote.origin.url
2026-11-04T18:43:42.1379143Z [command]/usr/bin/git config --file /home/runner/work/_temp/git-credentials-4ed5b1aa-b93a-4db0-a7cb-41670ede7cb3.config http.https://github.com/.extraheader AUTHORIZATION: basic ***
2026-11-04T18:43:42.1426328Z [command]/usr/bin/git config --local includeIf.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path /home/runner/work/_temp/git-credentials-4ed5b1aa-b93a-4db0-a7cb-41670ede7cb3.config
2026-11-04T18:43:42.1467906Z [command]/usr/bin/git config --local includeIf.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path /home/runner/work/_temp/git-credentials-4ed5b1aa-b93a-4db0-a7cb-41670ede7cb3.config
2026-11-04T18:43:42.1519583Z [command]/usr/bin/git config --local includeIf.gitdir:/github/workspace/.git.path /github/runner_temp/git-credentials-4ed5b1aa-b93a-4db0-a7cb-41670ede7cb3.config
2026-11-04T18:43:42.1562587Z [command]/usr/bin/git config --local includeIf.gitdir:/github/workspace/.git/worktrees/*.path /github/runner_temp/git-credentials-4ed5b1aa-b93a-4db0-a7cb-41670ede7cb3.config
2026-11-04T18:43:42.1604395Z ##[endgroup]
2026-11-04T18:43:42.1605523Z ##[group]Fetching the repository
2026-11-04T18:43:42.1609528Z [command]/usr/bin/git -c protocol.version=2 fetch --no-tags --prune --no-recurse-submodules --depth=1 origin +89d6b31c5e788b4e8e5cfc7178d991ef65856c06:refs/remotes/origin/develop
2026-11-04T18:43:43.7386043Z From https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:43:43.7387128Z  * [new ref]         89d6b31c5e788b4e8e5cfc7178d991ef65856c06 -> origin/develop
2026-11-04T18:43:43.7436743Z [command]/usr/bin/git branch --list --remote origin/develop
2026-11-04T18:43:43.7440543Z   origin/develop
2026-11-04T18:43:43.7452288Z [command]/usr/bin/git rev-parse refs/remotes/origin/develop
2026-11-04T18:43:43.7493533Z 89d6b31c5e788b4e8e5cfc7178d991ef65856c06
2026-11-04T18:43:43.7497248Z ##[endgroup]
2026-11-04T18:43:43.7497497Z ##[group]Determining the checkout info
2026-11-04T18:43:43.7499299Z ##[endgroup]
2026-11-04T18:43:43.7504768Z [command]/usr/bin/git sparse-checkout disable
2026-11-04T18:43:43.7569211Z [command]/usr/bin/git config --local --unset-all extensions.worktreeConfig
2026-11-04T18:43:43.7616891Z ##[group]Checking out the ref
2026-11-04T18:43:43.7621392Z [command]/usr/bin/git checkout --progress --force -B develop refs/remotes/origin/develop
2026-11-04T18:43:43.9191051Z Switched to a new branch 'develop'
2026-11-04T18:43:43.9196647Z branch 'develop' set up to track 'origin/develop'.
2026-11-04T18:43:43.9225805Z ##[endgroup]
2026-11-04T18:43:43.9291026Z [command]/usr/bin/git log -1 --format=%H
2026-11-04T18:43:43.9335249Z 89d6b31c5e788b4e8e5cfc7178d991ef65856c06
2026-11-04T18:43:43.9344974Z ##[group]Removing auth
2026-11-04T18:43:43.9345347Z Removing SSH command configuration
2026-11-04T18:43:43.9348922Z [command]/usr/bin/git config --local --name-only --get-regexp core\.sshCommand
2026-11-04T18:43:43.9397815Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'core\.sshCommand' && git config --local --unset-all 'core.sshCommand' || :"
2026-11-04T18:43:43.9709401Z Removing HTTP extra header
2026-11-04T18:43:43.9713423Z [command]/usr/bin/git config --local --name-only --get-regexp http\.https\:\/\/github\.com\/\.extraheader
2026-11-04T18:43:43.9777517Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'http\.https\:\/\/github\.com\/\.extraheader' && git config --local --unset-all 'http.https://github.com/.extraheader' || :"
2026-11-04T18:43:44.0051206Z Removing includeIf entries pointing to credentials config files
2026-11-04T18:43:44.0085733Z [command]/usr/bin/git config --local --name-only --get-regexp ^includeIf\.gitdir:
2026-11-04T18:43:44.0113939Z includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path
2026-11-04T18:43:44.0117126Z includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path
2026-11-04T18:43:44.0117747Z includeif.gitdir:/github/workspace/.git.path
2026-11-04T18:43:44.0118608Z includeif.gitdir:/github/workspace/.git/worktrees/*.path
2026-11-04T18:43:44.0128372Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path
2026-11-04T18:43:44.0178346Z /home/runner/work/_temp/git-credentials-4ed5b1aa-b93a-4db0-a7cb-41670ede7cb3.config
2026-11-04T18:43:44.0192021Z [command]/usr/bin/git config --local --unset includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path \/home\/runner\/work\/_temp\/git\-credentials\-4ed5b1aa\-b93a\-4db0\-a7cb\-41670ede7cb3\.config
2026-11-04T18:43:44.0249895Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path
2026-11-04T18:43:44.0301235Z /home/runner/work/_temp/git-credentials-4ed5b1aa-b93a-4db0-a7cb-41670ede7cb3.config
2026-11-04T18:43:44.0312049Z [command]/usr/bin/git config --local --unset includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path \/home\/runner\/work\/_temp\/git\-credentials\-4ed5b1aa\-b93a\-4db0\-a7cb\-41670ede7cb3\.config
2026-11-04T18:43:44.0369135Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/github/workspace/.git.path
2026-11-04T18:43:44.0417817Z /github/runner_temp/git-credentials-4ed5b1aa-b93a-4db0-a7cb-41670ede7cb3.config
2026-11-04T18:43:44.0427975Z [command]/usr/bin/git config --local --unset includeif.gitdir:/github/workspace/.git.path \/github\/runner_temp\/git\-credentials\-4ed5b1aa\-b93a\-4db0\-a7cb\-41670ede7cb3\.config
2026-11-04T18:43:44.0482978Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/github/workspace/.git/worktrees/*.path
2026-11-04T18:43:44.0529113Z /github/runner_temp/git-credentials-4ed5b1aa-b93a-4db0-a7cb-41670ede7cb3.config
2026-11-04T18:43:44.0530819Z [command]/usr/bin/git config --local --unset includeif.gitdir:/github/workspace/.git/worktrees/*.path \/github\/runner_temp\/git\-credentials\-4ed5b1aa\-b93a\-4db0\-a7cb\-41670ede7cb3\.config
2026-11-04T18:43:44.0596218Z [command]/usr/bin/git submodule foreach --recursive git config --local --show-origin --name-only --get-regexp remote.origin.url
2026-11-04T18:43:44.0831555Z Removing credentials config '/home/runner/work/_temp/git-credentials-4ed5b1aa-b93a-4db0-a7cb-41670ede7cb3.config'
2026-11-04T18:43:44.0842170Z ##[endgroup]
2026-11-04T18:43:44.1797893Z ##[group]Run actions/setup-dotnet@a98b56852c35b8e3190ac28c8c2271da59106c68
2026-11-04T18:43:44.1798135Z with:
2026-11-04T18:43:44.1798260Z   dotnet-version: 10.0.x
2026-11-04T18:43:44.1798396Z   cache: false
2026-11-04T18:43:44.1798509Z env:
2026-11-04T18:43:44.1798620Z   DOTNET_NOLOGO: true
2026-11-04T18:43:44.1798751Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:43:44.1798904Z   CONFIGURACAO: Release
2026-11-04T18:43:44.1799047Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:43:44.1799329Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:43:44.1799758Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:43:44.1800054Z   RESULTADOS: resultados-ci
2026-11-04T18:43:44.1800197Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:43:44.1800351Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:43:44.1800510Z ##[endgroup]
2026-11-04T18:43:44.2884595Z [command]/home/runner/work/_actions/actions/setup-dotnet/a98b56852c35b8e3190ac28c8c2271da59106c68/externals/install-dotnet.sh --skip-non-versioned-files --runtime dotnet --channel LTS
2026-11-04T18:43:44.6049161Z dotnet-install: .NET Core Runtime with version '10.0.14' is already installed.
2026-11-04T18:43:44.6075825Z [command]/home/runner/work/_actions/actions/setup-dotnet/a98b56852c35b8e3190ac28c8c2271da59106c68/externals/install-dotnet.sh --skip-non-versioned-files --channel 10.0
2026-11-04T18:43:44.8711651Z dotnet-install: .NET Core SDK with version '10.0.403' is already installed.
2026-11-04T18:43:44.9053595Z ##[group]Run actions/download-artifact@9000827ccba6bdab643e8b6fd33ac0654aef8333
2026-11-04T18:43:44.9053839Z with:
2026-11-04T18:43:44.9053969Z   pattern: resultados-testes-*
2026-11-04T18:43:44.9054121Z   path: resultados-ci/entrada
2026-11-04T18:43:44.9054266Z   merge-multiple: false
2026-11-04T18:43:44.9054441Z   repository: LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:43:44.9054632Z   run-id: 41888968713
2026-11-04T18:43:44.9054760Z   skip-decompress: false
2026-11-04T18:43:44.9054890Z   digest-mismatch: error
2026-11-04T18:43:44.9055013Z env:
2026-11-04T18:43:44.9055121Z   DOTNET_NOLOGO: true
2026-11-04T18:43:44.9055255Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:43:44.9055409Z   CONFIGURACAO: Release
2026-11-04T18:43:44.9055555Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:43:44.9055846Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:43:44.9056301Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:43:44.9056616Z   RESULTADOS: resultados-ci
2026-11-04T18:43:44.9056757Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:43:44.9056912Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:43:44.9057075Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:43:44.9057215Z ##[endgroup]
2026-11-04T18:43:45.2392844Z Found 6 artifact(s)
2026-11-04T18:43:45.2393440Z Filtering artifacts by pattern 'resultados-testes-*'
2026-11-04T18:43:45.2406592Z Preparing to download the following artifacts:
2026-11-04T18:43:45.2407113Z - resultados-testes-integracao (ID: 13353680528, Size: 452553, Expected Digest: sha256:f22fc050cc400702952b686e36b144dee72b44fa0c3c9733f00d308c5fe212b6)
2026-11-04T18:43:45.2407755Z - resultados-testes-unitarios (ID: 13352638229, Size: 305281, Expected Digest: sha256:afbbd06bd3fbaa2d7d33a1635306abfc84eddb11988ee5f453994bd34881c311)
2026-11-04T18:43:45.6086118Z Redirecting to blob download url: https://productionresultssa32.blob.core.windows.net/actions-results/49d4b900-19fc-897b-9a6d-62b8e2de22d3/workflow-job-run-94fcb3d9-3065-57a0-816a-2a7fb4600873/artifacts/de5b3bd62a15c0e3e864335d199c3fd28602c8e81547a1b58329f33915a666bf.zip
2026-11-04T18:43:45.6087626Z Starting download of artifact to: /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/resultados-ci/entrada/resultados-testes-unitarios
2026-11-04T18:43:45.6107931Z Redirecting to blob download url: https://productionresultssa32.blob.core.windows.net/actions-results/49d4b900-19fc-897b-9a6d-62b8e2de22d3/workflow-job-run-b3dbeb40-9e79-5263-aeea-ca1cb27d79aa/artifacts/9a81cda04cbad22d75fc0e5f8a5b3e2f7b3c8fc0e83fe38e297c266cb6ddc35e.zip
2026-11-04T18:43:45.6109417Z Starting download of artifact to: /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/resultados-ci/entrada/resultados-testes-integracao
2026-11-04T18:43:45.7604584Z (node:2354) [DEP0005] DeprecationWarning: Buffer() is deprecated due to security and usability issues. Please use the Buffer.alloc(), Buffer.allocUnsafe(), or Buffer.from() methods instead.
2026-11-04T18:43:45.7606497Z (Use `node --trace-deprecation ...` to show where the warning was created)
2026-11-04T18:43:45.8686888Z SHA256 digest of downloaded artifact is afbbd06bd3fbaa2d7d33a1635306abfc84eddb11988ee5f453994bd34881c311
2026-11-04T18:43:45.8687303Z Artifact download completed successfully.
2026-11-04T18:43:45.9196816Z SHA256 digest of downloaded artifact is f22fc050cc400702952b686e36b144dee72b44fa0c3c9733f00d308c5fe212b6
2026-11-04T18:43:45.9197459Z Artifact download completed successfully.
2026-11-04T18:43:45.9197814Z Total of 2 artifact(s) downloaded
2026-11-04T18:43:45.9202221Z Download artifact has finished successfully
2026-11-04T18:43:45.9891860Z ##[group]Run dotnet tool install dotnet-reportgenerator-globaltool --version "$REPORTGENERATOR_VERSAO" --tool-path .ferramentas
2026-11-04T18:43:45.9892790Z [36;1mdotnet tool install dotnet-reportgenerator-globaltool --version "$REPORTGENERATOR_VERSAO" --tool-path .ferramentas[0m
2026-11-04T18:43:45.9914680Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:43:45.9914909Z env:
2026-11-04T18:43:45.9915048Z   DOTNET_NOLOGO: true
2026-11-04T18:43:45.9915202Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:43:45.9915377Z   CONFIGURACAO: Release
2026-11-04T18:43:45.9915544Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:43:45.9915854Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:43:45.9916304Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:43:45.9916627Z   RESULTADOS: resultados-ci
2026-11-04T18:43:45.9916844Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:43:45.9917026Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:43:45.9917208Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:43:45.9917370Z ##[endgroup]
2026-11-04T18:43:47.8648908Z You can invoke the tool using the following command: reportgenerator
2026-11-04T18:43:47.8649360Z Tool 'dotnet-reportgenerator-globaltool' (version '5.5.11') was successfully installed.
2026-11-04T18:43:47.8869576Z ##[group]Run shopt -s globstar nullglob
2026-11-04T18:43:47.8869834Z [36;1mshopt -s globstar nullglob[0m
2026-11-04T18:43:47.8870046Z [36;1mFICHEIROS=("$RESULTADOS"/entrada/**/coverage.cobertura.xml)[0m
2026-11-04T18:43:47.8870268Z [36;1mif [ "${#FICHEIROS[@]}" -eq 0 ]; then[0m
2026-11-04T18:43:47.8870461Z [36;1m  echo "gerado=false" >> "$GITHUB_OUTPUT"[0m
2026-11-04T18:43:47.8870644Z [36;1m  if [ "$TESTES_OK" = "true" ]; then[0m
2026-11-04T18:43:47.8870987Z [36;1m    echo "::error title=Cobertura::Os testes passaram, mas não foi produzido nenhum coverage.cobertura.xml."[0m
2026-11-04T18:43:47.8871290Z [36;1m    exit 1[0m
2026-11-04T18:43:47.8871415Z [36;1m  fi[0m
2026-11-04T18:43:47.8871670Z [36;1m  echo "::notice title=Cobertura::Sem ficheiros de cobertura, porque os testes não terminaram com sucesso."[0m
2026-11-04T18:43:47.8872151Z [36;1m  echo "ℹ️ Relatório de cobertura e QG-03 não avaliados: os testes não terminaram com sucesso." >> "$GITHUB_STEP_SUMMARY"[0m
2026-11-04T18:43:47.8872647Z [36;1m  exit 0[0m
2026-11-04T18:43:47.8872807Z [36;1mfi[0m
2026-11-04T18:43:47.8872958Z [36;1mecho "Ficheiros de cobertura: ${#FICHEIROS[@]}"[0m
2026-11-04T18:43:47.8873156Z [36;1m./.ferramentas/reportgenerator \[0m
2026-11-04T18:43:47.8873380Z [36;1m  "-reports:$RESULTADOS/entrada/**/coverage.cobertura.xml" \[0m
2026-11-04T18:43:47.8873602Z [36;1m  "-targetdir:$RESULTADOS/cobertura" \[0m
2026-11-04T18:43:47.8873839Z [36;1m  "-reporttypes:Html;Cobertura;MarkdownSummaryGithub;TextSummary" \[0m
2026-11-04T18:43:47.8874097Z [36;1m  "-assemblyfilters:-*.UnitTests;-*.IntegrationTests" \[0m
2026-11-04T18:43:47.8874335Z [36;1m  "-title:Backend — Gestão de Clínica Veterinária (G08)"[0m
2026-11-04T18:43:47.8874547Z [36;1mecho "gerado=true" >> "$GITHUB_OUTPUT"[0m
2026-11-04T18:43:47.8874709Z [36;1m{[0m
2026-11-04T18:43:47.8874874Z [36;1m  echo "### 4 · Cobertura — relatório completo (informativo)"[0m
2026-11-04T18:43:47.8875075Z [36;1m  echo ""[0m
2026-11-04T18:43:47.8875482Z [36;1m  echo "Medida com Coverlet sobre os testes unitários e de integração em conjunto e gerada com ReportGenerator ${REPORTGENERATOR_VERSAO}, para todas as camadas exceto os projetos de testes."[0m
2026-11-04T18:43:47.8875898Z [36;1m  echo ""[0m
2026-11-04T18:43:47.8876080Z [36;1m  echo "<details><summary>Resumo por assembly e classe</summary>"[0m
2026-11-04T18:43:47.8876291Z [36;1m  echo ""[0m
2026-11-04T18:43:47.8876445Z [36;1m  cat "$RESULTADOS/cobertura/SummaryGithub.md"[0m
2026-11-04T18:43:47.8876617Z [36;1m  echo ""[0m
2026-11-04T18:43:47.8876748Z [36;1m  echo "</details>"[0m
2026-11-04T18:43:47.8876888Z [36;1m  echo ""[0m
2026-11-04T18:43:47.8877021Z [36;1m} >> "$GITHUB_STEP_SUMMARY"[0m
2026-11-04T18:43:47.8893743Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:43:47.8893948Z env:
2026-11-04T18:43:47.8894075Z   DOTNET_NOLOGO: true
2026-11-04T18:43:47.8894388Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:43:47.8894568Z   CONFIGURACAO: Release
2026-11-04T18:43:47.8894725Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:43:47.8895022Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:43:47.8895451Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:43:47.8895764Z   RESULTADOS: resultados-ci
2026-11-04T18:43:47.8895919Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:43:47.8896086Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:43:47.8896286Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:43:47.8896435Z   TESTES_OK: true
2026-11-04T18:43:47.8896563Z ##[endgroup]
2026-11-04T18:43:47.8938844Z Ficheiros de cobertura: 4
2026-11-04T18:43:47.9297915Z 2026-11-04T18:43:47: Arguments
2026-11-04T18:43:47.9298440Z 2026-11-04T18:43:47:  -reports:resultados-ci/entrada/**/coverage.cobertura.xml
2026-11-04T18:43:47.9298865Z 2026-11-04T18:43:47:  -targetdir:resultados-ci/cobertura
2026-11-04T18:43:47.9299620Z 2026-11-04T18:43:47:  -reporttypes:Html;Cobertura;MarkdownSummaryGithub;TextSummary
2026-11-04T18:43:47.9299934Z 2026-11-04T18:43:47:  -assemblyfilters:-*.UnitTests;-*.IntegrationTests
2026-11-04T18:43:47.9300533Z 2026-11-04T18:43:47:  -title:Backend — Gestão de Clínica Veterinária (G08)
2026-11-04T18:43:48.3745708Z 2026-11-04T18:43:48: File '/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Api/obj/Release/net10.0/Microsoft.Extensions.Logging.Generators/Microsoft.Extensions.Logging.Generators.LoggerMessageGenerator/LoggerMessage.g.cs' does not exist (any more).
2026-11-04T18:43:48.3760873Z 2026-11-04T18:43:48: File '/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Api/obj/Release/net10.0/Microsoft.Extensions.Logging.Generators/Microsoft.Extensions.Logging.Generators.LoggerMessageGenerator/LoggerMessage.g.cs' does not exist (any more).
2026-11-04T18:43:48.3796734Z 2026-11-04T18:43:48: File '/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Api/obj/Release/net10.0/Microsoft.Extensions.Logging.Generators/Microsoft.Extensions.Logging.Generators.LoggerMessageGenerator/LoggerMessage.g.cs' does not exist (any more).
2026-11-04T18:43:48.4477061Z 2026-11-04T18:43:48: Writing report file 'resultados-ci/cobertura/Cobertura.xml'
2026-11-04T18:43:48.4581068Z 2026-11-04T18:43:48: Writing report file 'resultados-ci/cobertura/index.html'
2026-11-04T18:43:48.4893397Z 2026-11-04T18:43:48: Writing report file 'resultados-ci/cobertura/SummaryGithub.md'
2026-11-04T18:43:48.4941478Z 2026-11-04T18:43:48: Writing report file 'resultados-ci/cobertura/Summary.txt'
2026-11-04T18:43:48.5024901Z 2026-11-04T18:43:48: Report generation took 0.5 seconds
2026-11-04T18:43:48.5251695Z ##[group]Run python3 .github/scripts/gate-cobertura.py filtros "$QUALITY_GATES"
2026-11-04T18:43:48.5252056Z [36;1mpython3 .github/scripts/gate-cobertura.py filtros "$QUALITY_GATES"[0m
2026-11-04T18:43:48.5269340Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:43:48.5269540Z env:
2026-11-04T18:43:48.5269660Z   DOTNET_NOLOGO: true
2026-11-04T18:43:48.5269805Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:43:48.5269962Z   CONFIGURACAO: Release
2026-11-04T18:43:48.5270115Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:43:48.5270403Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:43:48.5270814Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:43:48.5271121Z   RESULTADOS: resultados-ci
2026-11-04T18:43:48.5271269Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:43:48.5271472Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:43:48.5271640Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:43:48.5271791Z ##[endgroup]
2026-11-04T18:43:48.5517321Z assemblyfilters=+ClinicaVeterinaria.Domain;+ClinicaVeterinaria.Application;+ClinicaVeterinaria.Infrastructure
2026-11-04T18:43:48.5518104Z classfilters=-ClinicaVeterinaria.Application.DTOs.*;-ClinicaVeterinaria.Infrastructure.Migrations.*;-ClinicaVeterinaria.Infrastructure.ClinicaDbContext;-ClinicaVeterinaria.*.DependencyInjection
2026-11-04T18:43:48.5518610Z filefilters=-**/obj/**
2026-11-04T18:43:48.5518750Z limiar=70
2026-11-04T18:43:48.5629439Z ##[group]Run ./.ferramentas/reportgenerator \
2026-11-04T18:43:48.5629704Z [36;1m./.ferramentas/reportgenerator \[0m
2026-11-04T18:43:48.5629928Z [36;1m  "-reports:$RESULTADOS/entrada/**/coverage.cobertura.xml" \[0m
2026-11-04T18:43:48.5630163Z [36;1m  "-targetdir:$RESULTADOS/cobertura/qg-03" \[0m
2026-11-04T18:43:48.5630408Z [36;1m  "-reporttypes:Html;Cobertura;MarkdownSummaryGithub;TextSummary" \[0m
2026-11-04T18:43:48.5630653Z [36;1m  "-assemblyfilters:$FILTRO_ASSEMBLIES" \[0m
2026-11-04T18:43:48.5630843Z [36;1m  "-classfilters:$FILTRO_CLASSES" \[0m
2026-11-04T18:43:48.5631042Z [36;1m  "-filefilters:$FILTRO_FICHEIROS" \[0m
2026-11-04T18:43:48.5631265Z [36;1m  "-title:Backend — âmbito do QG-03 (Domínio e Serviços)"[0m
2026-11-04T18:43:48.5648108Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:43:48.5648304Z env:
2026-11-04T18:43:48.5648427Z   DOTNET_NOLOGO: true
2026-11-04T18:43:48.5648571Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:43:48.5648818Z   CONFIGURACAO: Release
2026-11-04T18:43:48.5648974Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:43:48.5649267Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:43:48.5649702Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:43:48.5650056Z   RESULTADOS: resultados-ci
2026-11-04T18:43:48.5650210Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:43:48.5650380Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:43:48.5650563Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:43:48.5650914Z   FILTRO_ASSEMBLIES: +ClinicaVeterinaria.Domain;+ClinicaVeterinaria.Application;+ClinicaVeterinaria.Infrastructure
2026-11-04T18:43:48.5651614Z   FILTRO_CLASSES: -ClinicaVeterinaria.Application.DTOs.*;-ClinicaVeterinaria.Infrastructure.Migrations.*;-ClinicaVeterinaria.Infrastructure.ClinicaDbContext;-ClinicaVeterinaria.*.DependencyInjection
2026-11-04T18:43:48.5652100Z   FILTRO_FICHEIROS: -**/obj/**
2026-11-04T18:43:48.5652284Z ##[endgroup]
2026-11-04T18:43:48.6058964Z 2026-11-04T18:43:48: Arguments
2026-11-04T18:43:48.6059677Z 2026-11-04T18:43:48:  -reports:resultados-ci/entrada/**/coverage.cobertura.xml
2026-11-04T18:43:48.6060225Z 2026-11-04T18:43:48:  -targetdir:resultados-ci/cobertura/qg-03
2026-11-04T18:43:48.6060789Z 2026-11-04T18:43:48:  -reporttypes:Html;Cobertura;MarkdownSummaryGithub;TextSummary
2026-11-04T18:43:48.6061571Z 2026-11-04T18:43:48:  -assemblyfilters:+ClinicaVeterinaria.Domain;+ClinicaVeterinaria.Application;+ClinicaVeterinaria.Infrastructure
2026-11-04T18:43:48.6063129Z 2026-11-04T18:43:48:  -classfilters:-ClinicaVeterinaria.Application.DTOs.*;-ClinicaVeterinaria.Infrastructure.Migrations.*;-ClinicaVeterinaria.Infrastructure.ClinicaDbContext;-ClinicaVeterinaria.*.DependencyInjection
2026-11-04T18:43:48.6064244Z 2026-11-04T18:43:48:  -filefilters:-**/obj/**
2026-11-04T18:43:48.6065023Z 2026-11-04T18:43:48:  -title:Backend — âmbito do QG-03 (Domínio e Serviços)
2026-11-04T18:43:49.0524990Z 2026-11-04T18:43:49: Writing report file 'resultados-ci/cobertura/qg-03/Cobertura.xml'
2026-11-04T18:43:49.0589023Z 2026-11-04T18:43:49: Writing report file 'resultados-ci/cobertura/qg-03/index.html'
2026-11-04T18:43:49.0844972Z 2026-11-04T18:43:49: Writing report file 'resultados-ci/cobertura/qg-03/SummaryGithub.md'
2026-11-04T18:43:49.0874207Z 2026-11-04T18:43:49: Writing report file 'resultados-ci/cobertura/qg-03/Summary.txt'
2026-11-04T18:43:49.0998683Z 2026-11-04T18:43:49: Report generation took 0.4 seconds
2026-11-04T18:43:49.1186283Z ##[group]Run python3 .github/scripts/gate-cobertura.py avaliar "$QUALITY_GATES" "$RESULTADOS/cobertura/qg-03/Cobertura.xml"
2026-11-04T18:43:49.1187158Z [36;1mpython3 .github/scripts/gate-cobertura.py avaliar "$QUALITY_GATES" "$RESULTADOS/cobertura/qg-03/Cobertura.xml"[0m
2026-11-04T18:43:49.1211309Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:43:49.1211659Z env:
2026-11-04T18:43:49.1211860Z   DOTNET_NOLOGO: true
2026-11-04T18:43:49.1212092Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:43:49.1212545Z   CONFIGURACAO: Release
2026-11-04T18:43:49.1212800Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:43:49.1213288Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:43:49.1213976Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:43:49.1214466Z   RESULTADOS: resultados-ci
2026-11-04T18:43:49.1214755Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:43:49.1215035Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:43:49.1215309Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:43:49.1215549Z ##[endgroup]
2026-11-04T18:43:49.1542776Z ### QG-03 · Cobertura de linhas (Domínio e Serviços)
2026-11-04T18:43:49.1543063Z 
2026-11-04T18:43:49.1543183Z | Medida | Valor |
2026-11-04T18:43:49.1543381Z |---|---|
2026-11-04T18:43:49.1543892Z | Assemblies medidas | `ClinicaVeterinaria.Application`, `ClinicaVeterinaria.Domain`, `ClinicaVeterinaria.Infrastructure` |
2026-11-04T18:43:49.1544512Z | Linhas cobertas / mensuráveis | 2118 / 2150 |
2026-11-04T18:43:49.1544795Z | **Cobertura de linhas** | **98.51%** |
2026-11-04T18:43:49.1545126Z | Limiar (`.github/quality-gates.json`) | ≥ 70% |
2026-11-04T18:43:49.1545548Z | Cobertura de ramos (LT-02, informativa) | 92.83% (referência 60%) |
2026-11-04T18:43:49.1545778Z 
2026-11-04T18:43:49.1546713Z Exclusões aplicadas: `ClinicaVeterinaria.Application.DTOs.*`, `ClinicaVeterinaria.Infrastructure.Migrations.*`, `ClinicaVeterinaria.Infrastructure.ClinicaDbContext`, `ClinicaVeterinaria.*.DependencyInjection`, `**/obj/**`.
2026-11-04T18:43:49.1547451Z 
2026-11-04T18:43:49.1547703Z <details><summary>As 10 classes com menor cobertura no âmbito</summary>
2026-11-04T18:43:49.1547939Z 
2026-11-04T18:43:49.1548051Z | Classe | Linhas cobertas | Cobertura |
2026-11-04T18:43:49.1548279Z |---|---|---|
2026-11-04T18:43:49.1548644Z | `ClinicaVeterinaria.Domain.Entities.HorarioFuncionamento` | 4 / 5 | 80.0% |
2026-11-04T18:43:49.1549173Z | `ClinicaVeterinaria.Infrastructure.BloqueioAgendaVeterinario` | 16 / 19 | 84.2% |
2026-11-04T18:43:49.1549741Z | `ClinicaVeterinaria.Application.Validacao.ValidadorRegistoClinico` | 33 / 38 | 86.8% |
2026-11-04T18:43:49.1550300Z | `ClinicaVeterinaria.Application.Agenda.FusoHorarioClinica` | 14 / 16 | 87.5% |
2026-11-04T18:43:49.1550810Z | `ClinicaVeterinaria.Domain.Entities.RegistoProfilaxia` | 10 / 11 | 90.9% |
2026-11-04T18:43:49.1551303Z | `ClinicaVeterinaria.Infrastructure.ClinicaService` | 66 / 70 | 94.3% |
2026-11-04T18:43:49.1551813Z | `ClinicaVeterinaria.Application.Validacao.ValidadorHorario` | 46 / 48 | 95.8% |
2026-11-04T18:43:49.1552512Z | `ClinicaVeterinaria.Application.Agenda.MaquinaEstadosMarcacao` | 25 / 26 | 96.2% |
2026-11-04T18:43:49.1553062Z | `ClinicaVeterinaria.Infrastructure.UtilizadoresService` | 181 / 186 | 97.3% |
2026-11-04T18:43:49.1553553Z | `ClinicaVeterinaria.Infrastructure.AuthService` | 151 / 155 | 97.4% |
2026-11-04T18:43:49.1553810Z 
2026-11-04T18:43:49.1553887Z </details>
2026-11-04T18:43:49.1553989Z 
2026-11-04T18:43:49.1554161Z ✅ **QG-03 cumprido:** 98.51% ≥ 70%.
2026-11-04T18:43:49.1554315Z 
2026-11-04T18:43:49.1748173Z ##[group]Run actions/upload-artifact@cf430e030ddbb5b0abf93d22962f4752f3646cd9
2026-11-04T18:43:49.1748423Z with:
2026-11-04T18:43:49.1748551Z   name: relatorio-cobertura
2026-11-04T18:43:49.1748709Z   path: resultados-ci/cobertura
2026-11-04T18:43:49.1748865Z   if-no-files-found: ignore
2026-11-04T18:43:49.1749141Z   retention-days: 90
2026-11-04T18:43:49.1749277Z   compression-level: 6
2026-11-04T18:43:49.1749406Z   overwrite: false
2026-11-04T18:43:49.1749540Z   include-hidden-files: false
2026-11-04T18:43:49.1749688Z   archive: true
2026-11-04T18:43:49.1749805Z env:
2026-11-04T18:43:49.1749920Z   DOTNET_NOLOGO: true
2026-11-04T18:43:49.1750061Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:43:49.1750215Z   CONFIGURACAO: Release
2026-11-04T18:43:49.1750367Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:43:49.1750651Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:43:49.1751103Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:43:49.1751408Z   RESULTADOS: resultados-ci
2026-11-04T18:43:49.1751559Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:43:49.1751720Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:43:49.1751888Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:43:49.1752127Z ##[endgroup]
2026-11-04T18:43:49.3240675Z With the provided path, there will be 263 files uploaded
2026-11-04T18:43:49.3244431Z Artifact name is valid!
2026-11-04T18:43:49.3244847Z Root directory input is valid!
2026-11-04T18:43:49.5948522Z Uploading artifact: relatorio-cobertura.zip
2026-11-04T18:43:49.5976303Z Beginning upload of artifact content to blob storage
2026-11-04T18:43:50.1865833Z Uploaded bytes 1326871
2026-11-04T18:43:50.2257455Z Finished uploading artifact content to blob storage!
2026-11-04T18:43:50.2258115Z SHA256 digest of uploaded artifact is 6663d1a84006672d1e9b12b471293af0fcbd9deb123c761535ce1239640a3cae
2026-11-04T18:43:50.2258600Z Finalizing artifact upload
2026-11-04T18:43:50.7755823Z Artifact relatorio-cobertura successfully finalized. Artifact ID 13352634236
2026-11-04T18:43:50.7756460Z Artifact relatorio-cobertura has been successfully uploaded! Final size is 1326871 bytes. Artifact ID is 13352634236
2026-11-04T18:43:50.7758806Z Artifact download URL: https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/actions/runs/41888968713/artifacts/13352634236
2026-11-04T18:43:50.8103203Z Post job cleanup.
2026-11-04T18:43:50.9492253Z Post job cleanup.
2026-11-04T18:43:51.0176007Z [command]/usr/bin/git version
2026-11-04T18:43:51.0218650Z git version 2.56.0
2026-11-04T18:43:51.0245503Z Temporarily overriding HOME='/home/runner/work/_temp/950983aa-3d3a-40d1-97d4-fe1bb0e04ec5' before making global git config changes
2026-11-04T18:43:51.0246345Z Adding repository directory to the temporary git global config as a safe directory
2026-11-04T18:43:51.0251178Z [command]/usr/bin/git config --global --add safe.directory /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:43:51.0291385Z Removing SSH command configuration
2026-11-04T18:43:51.0298184Z [command]/usr/bin/git config --local --name-only --get-regexp core\.sshCommand
2026-11-04T18:43:51.0343167Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'core\.sshCommand' && git config --local --unset-all 'core.sshCommand' || :"
2026-11-04T18:43:51.0589230Z Removing HTTP extra header
2026-11-04T18:43:51.0596313Z [command]/usr/bin/git config --local --name-only --get-regexp http\.https\:\/\/github\.com\/\.extraheader
2026-11-04T18:43:51.0646305Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'http\.https\:\/\/github\.com\/\.extraheader' && git config --local --unset-all 'http.https://github.com/.extraheader' || :"
2026-11-04T18:43:51.0941883Z Removing includeIf entries pointing to credentials config files
2026-11-04T18:43:51.0948833Z [command]/usr/bin/git config --local --name-only --get-regexp ^includeIf\.gitdir:
2026-11-04T18:43:51.1035619Z [command]/usr/bin/git submodule foreach --recursive git config --local --show-origin --name-only --get-regexp remote.origin.url
2026-11-04T18:43:51.1436242Z Cleaning up orphan processes
``````
