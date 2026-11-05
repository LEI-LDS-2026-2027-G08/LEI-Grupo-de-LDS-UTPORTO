``````text
﻿2026-11-04T18:55:11.3910420Z Current runner version: '2.338.2'
2026-11-04T18:55:11.3936765Z ##[group]Runner Image Provisioner
2026-11-04T18:55:11.3937786Z Hosted Compute Agent
2026-11-04T18:55:11.3938432Z Version: 20261028.624
2026-11-04T18:55:11.3939566Z Commit: 54898b74c23fde5556c9b3c3a2290865feef0dfe
2026-11-04T18:55:11.3940462Z Build Date: 2026-10-28T21:51:42Z
2026-11-04T18:55:11.3941224Z Worker ID: {14bc159b-d368-44be-be2d-be9132b2bffe}
2026-11-04T18:55:11.3941969Z Region: westus
2026-11-04T18:55:11.3942622Z Cloud: Azure
2026-11-04T18:55:11.3943192Z ##[endgroup]
2026-11-04T18:55:11.3944901Z ##[group]Operating System
2026-11-04T18:55:11.3945629Z Ubuntu
2026-11-04T18:55:11.3946164Z 24.04.5
2026-11-04T18:55:11.3946775Z LTS
2026-11-04T18:55:11.3947319Z ##[endgroup]
2026-11-04T18:55:11.3947938Z ##[group]Runner Image
2026-11-04T18:55:11.3948632Z Image: ubuntu-24.04
2026-11-04T18:55:11.3949581Z Version: 20261101.360.1
2026-11-04T18:55:11.3950991Z Included Software: https://github.com/actions/runner-images/blob/ubuntu24/20261101.360/images/ubuntu/Ubuntu2404-Readme.md
2026-11-04T18:55:11.3952605Z Image Release: https://github.com/actions/runner-images/releases/tag/ubuntu24%2F20261101.360
2026-11-04T18:55:11.3953667Z ##[endgroup]
2026-11-04T18:55:11.3954869Z ##[group]GITHUB_TOKEN Permissions
2026-11-04T18:55:11.3957048Z Contents: read
2026-11-04T18:55:11.3957689Z Metadata: read
2026-11-04T18:55:11.3958432Z ##[endgroup]
2026-11-04T18:55:11.3960829Z Secret source: Actions
2026-11-04T18:55:11.3961893Z Cache mode: write
2026-11-04T18:55:11.3962951Z Prepare workflow directory
2026-11-04T18:55:11.4435447Z Prepare all required actions
2026-11-04T18:55:11.4483635Z Getting action download info
2026-11-04T18:55:11.8067663Z Download action repository 'actions/checkout@3d3c42e5aac5ba805825da76410c181273ba90b1' (SHA:3d3c42e5aac5ba805825da76410c181273ba90b1)
2026-11-04T18:55:11.9143597Z Download action repository 'actions/upload-artifact@cf430e030ddbb5b0abf93d22962f4752f3646cd9' (SHA:cf430e030ddbb5b0abf93d22962f4752f3646cd9)
2026-11-04T18:55:12.0882011Z Complete job name: 2 · Testes unitários (QG-02)
2026-11-04T18:55:12.1837756Z ##[group]Run actions/checkout@3d3c42e5aac5ba805825da76410c181273ba90b1
2026-11-04T18:55:12.1839248Z with:
2026-11-04T18:55:12.1839770Z   persist-credentials: false
2026-11-04T18:55:12.1840450Z   repository: LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:55:12.1844381Z   token: ***
2026-11-04T18:55:12.1844840Z   ssh-strict: true
2026-11-04T18:55:12.1845289Z   ssh-user: git
2026-11-04T18:55:12.1845720Z   clean: true
2026-11-04T18:55:12.1846177Z   sparse-checkout-cone-mode: true
2026-11-04T18:55:12.1846709Z   fetch-depth: 1
2026-11-04T18:55:12.1847156Z   fetch-tags: false
2026-11-04T18:55:12.1847617Z   show-progress: true
2026-11-04T18:55:12.1848090Z   lfs: false
2026-11-04T18:55:12.1848515Z   submodules: false
2026-11-04T18:55:12.1849137Z   set-safe-directory: true
2026-11-04T18:55:12.1849664Z   allow-unsafe-pr-checkout: false
2026-11-04T18:55:12.1850474Z env:
2026-11-04T18:55:12.1850896Z   DOTNET_NOLOGO: true
2026-11-04T18:55:12.1851466Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:12.1852004Z   CONFIGURACAO: Release
2026-11-04T18:55:12.1852527Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:12.1853464Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:12.1854803Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:12.1855825Z   RESULTADOS: resultados-ci
2026-11-04T18:55:12.1856355Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:12.1856903Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:12.1857475Z ##[endgroup]
2026-11-04T18:55:12.5182985Z Syncing repository: LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:55:12.5186165Z ##[group]Getting Git version info
2026-11-04T18:55:12.5188367Z Working directory is '/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO'
2026-11-04T18:55:12.5191083Z [command]/usr/bin/git version
2026-11-04T18:55:12.5320255Z git version 2.56.0
2026-11-04T18:55:12.5365094Z ##[endgroup]
2026-11-04T18:55:12.5387836Z Temporarily overriding HOME='/home/runner/work/_temp/2ae1901c-39f2-4c72-91d3-6f182faaee4f' before making global git config changes
2026-11-04T18:55:12.5396744Z Adding repository directory to the temporary git global config as a safe directory
2026-11-04T18:55:12.5403723Z [command]/usr/bin/git config --global --add safe.directory /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:55:12.5480334Z Deleting the contents of '/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO'
2026-11-04T18:55:12.5485165Z ##[group]Determining repository object format
2026-11-04T18:55:12.5487098Z ##[endgroup]
2026-11-04T18:55:12.5488192Z ##[group]Initializing the repository
2026-11-04T18:55:12.5494455Z [command]/usr/bin/git init /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:55:12.5637395Z hint: Using 'master' as the name for the initial branch. This default branch name
2026-11-04T18:55:12.5639454Z hint: will change to "main" in Git 3.0. To configure the initial branch name
2026-11-04T18:55:12.5640583Z hint: to use in all of your new repositories, which will suppress this warning,
2026-11-04T18:55:12.5641403Z hint: call:
2026-11-04T18:55:12.5641852Z hint:
2026-11-04T18:55:12.5642458Z hint: 	git config --global init.defaultBranch <name>
2026-11-04T18:55:12.5643165Z hint:
2026-11-04T18:55:12.5643814Z hint: Names commonly chosen instead of 'master' are 'main', 'trunk' and
2026-11-04T18:55:12.5644833Z hint: 'development'. The just-created branch can be renamed via this command:
2026-11-04T18:55:12.5645688Z hint:
2026-11-04T18:55:12.5646153Z hint: 	git branch -m <name>
2026-11-04T18:55:12.5646697Z hint:
2026-11-04T18:55:12.5647382Z hint: Disable this message with "git config set advice.defaultBranchName false"
2026-11-04T18:55:12.5662956Z Initialized empty Git repository in /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/
2026-11-04T18:55:12.5676355Z [command]/usr/bin/git remote add origin https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:55:12.5737550Z ##[endgroup]
2026-11-04T18:55:12.5738547Z ##[group]Disabling automatic garbage collection
2026-11-04T18:55:12.5742696Z [command]/usr/bin/git config --local gc.auto 0
2026-11-04T18:55:12.5785161Z ##[endgroup]
2026-11-04T18:55:12.5786586Z ##[group]Setting up auth
2026-11-04T18:55:12.5787247Z Removing SSH command configuration
2026-11-04T18:55:12.5793285Z [command]/usr/bin/git config --local --name-only --get-regexp core\.sshCommand
2026-11-04T18:55:12.5837814Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'core\.sshCommand' && git config --local --unset-all 'core.sshCommand' || :"
2026-11-04T18:55:12.6271997Z Removing HTTP extra header
2026-11-04T18:55:12.6278215Z [command]/usr/bin/git config --local --name-only --get-regexp http\.https\:\/\/github\.com\/\.extraheader
2026-11-04T18:55:12.6331063Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'http\.https\:\/\/github\.com\/\.extraheader' && git config --local --unset-all 'http.https://github.com/.extraheader' || :"
2026-11-04T18:55:12.6572163Z Removing includeIf entries pointing to credentials config files
2026-11-04T18:55:12.6578596Z [command]/usr/bin/git config --local --name-only --get-regexp ^includeIf\.gitdir:
2026-11-04T18:55:12.6626825Z [command]/usr/bin/git submodule foreach --recursive git config --local --show-origin --name-only --get-regexp remote.origin.url
2026-11-04T18:55:12.6876301Z [command]/usr/bin/git config --file /home/runner/work/_temp/git-credentials-e928b59d-7da5-4275-800b-0b419fc95f3b.config http.https://github.com/.extraheader AUTHORIZATION: basic ***
2026-11-04T18:55:12.6928647Z [command]/usr/bin/git config --local includeIf.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path /home/runner/work/_temp/git-credentials-e928b59d-7da5-4275-800b-0b419fc95f3b.config
2026-11-04T18:55:12.6974163Z [command]/usr/bin/git config --local includeIf.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path /home/runner/work/_temp/git-credentials-e928b59d-7da5-4275-800b-0b419fc95f3b.config
2026-11-04T18:55:12.7018644Z [command]/usr/bin/git config --local includeIf.gitdir:/github/workspace/.git.path /github/runner_temp/git-credentials-e928b59d-7da5-4275-800b-0b419fc95f3b.config
2026-11-04T18:55:12.7064164Z [command]/usr/bin/git config --local includeIf.gitdir:/github/workspace/.git/worktrees/*.path /github/runner_temp/git-credentials-e928b59d-7da5-4275-800b-0b419fc95f3b.config
2026-11-04T18:55:12.7108131Z ##[endgroup]
2026-11-04T18:55:12.7109393Z ##[group]Fetching the repository
2026-11-04T18:55:12.7116063Z [command]/usr/bin/git -c protocol.version=2 fetch --no-tags --prune --no-recurse-submodules --depth=1 origin +1e0181f1ad52288323bc89cefd5d2641a3f2655a:refs/remotes/pull/247/merge
2026-11-04T18:55:14.2365400Z From https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:55:14.2367710Z  * [new ref]         1e0181f1ad52288323bc89cefd5d2641a3f2655a -> pull/247/merge
2026-11-04T18:55:14.2372042Z ##[endgroup]
2026-11-04T18:55:14.2373416Z ##[group]Determining the checkout info
2026-11-04T18:55:14.2374877Z ##[endgroup]
2026-11-04T18:55:14.2380058Z [command]/usr/bin/git sparse-checkout disable
2026-11-04T18:55:14.2450254Z [command]/usr/bin/git config --local --unset-all extensions.worktreeConfig
2026-11-04T18:55:14.2494624Z ##[group]Checking out the ref
2026-11-04T18:55:14.2499988Z [command]/usr/bin/git checkout --progress --force refs/remotes/pull/247/merge
2026-11-04T18:55:14.4487502Z Note: switching to 'refs/remotes/pull/247/merge'.
2026-11-04T18:55:14.4488157Z 
2026-11-04T18:55:14.4488680Z You are in 'detached HEAD' state. You can look around, make experimental
2026-11-04T18:55:14.4490003Z changes and commit them, and you can discard any commits you make in this
2026-11-04T18:55:14.4491913Z state without impacting any branches by switching back to a branch.
2026-11-04T18:55:14.4492599Z 
2026-11-04T18:55:14.4493028Z If you want to create a new branch to retain commits you create, you may
2026-11-04T18:55:14.4493885Z do so (now or later) by using -c with the switch command. Example:
2026-11-04T18:55:14.4494382Z 
2026-11-04T18:55:14.4494612Z   git switch -c <new-branch-name>
2026-11-04T18:55:14.4494959Z 
2026-11-04T18:55:14.4495178Z Or undo this operation with:
2026-11-04T18:55:14.4495500Z 
2026-11-04T18:55:14.4495683Z   git switch -
2026-11-04T18:55:14.4495969Z 
2026-11-04T18:55:14.4496510Z Turn off this advice by setting config variable advice.detachedHead to false
2026-11-04T18:55:14.4497569Z 
2026-11-04T18:55:14.4498486Z HEAD is now at 1e0181f Merge a94b7155edcdf8d0c0779b09a41573ebfadc26d4 into 89d6b31c5e788b4e8e5cfc7178d991ef65856c06
2026-11-04T18:55:14.4522964Z ##[endgroup]
2026-11-04T18:55:14.4582453Z [command]/usr/bin/git log -1 --format=%H
2026-11-04T18:55:14.4622199Z 1e0181f1ad52288323bc89cefd5d2641a3f2655a
2026-11-04T18:55:14.4632155Z ##[group]Removing auth
2026-11-04T18:55:14.4632749Z Removing SSH command configuration
2026-11-04T18:55:14.4636991Z [command]/usr/bin/git config --local --name-only --get-regexp core\.sshCommand
2026-11-04T18:55:14.4683741Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'core\.sshCommand' && git config --local --unset-all 'core.sshCommand' || :"
2026-11-04T18:55:14.4934640Z Removing HTTP extra header
2026-11-04T18:55:14.4939853Z [command]/usr/bin/git config --local --name-only --get-regexp http\.https\:\/\/github\.com\/\.extraheader
2026-11-04T18:55:14.4986320Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'http\.https\:\/\/github\.com\/\.extraheader' && git config --local --unset-all 'http.https://github.com/.extraheader' || :"
2026-11-04T18:55:14.5235459Z Removing includeIf entries pointing to credentials config files
2026-11-04T18:55:14.5240073Z [command]/usr/bin/git config --local --name-only --get-regexp ^includeIf\.gitdir:
2026-11-04T18:55:14.5281316Z includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path
2026-11-04T18:55:14.5282867Z includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path
2026-11-04T18:55:14.5284343Z includeif.gitdir:/github/workspace/.git.path
2026-11-04T18:55:14.5285137Z includeif.gitdir:/github/workspace/.git/worktrees/*.path
2026-11-04T18:55:14.5292735Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path
2026-11-04T18:55:14.5329748Z /home/runner/work/_temp/git-credentials-e928b59d-7da5-4275-800b-0b419fc95f3b.config
2026-11-04T18:55:14.5344219Z [command]/usr/bin/git config --local --unset includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path \/home\/runner\/work\/_temp\/git\-credentials\-e928b59d\-7da5\-4275\-800b\-0b419fc95f3b\.config
2026-11-04T18:55:14.5395934Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path
2026-11-04T18:55:14.5431788Z /home/runner/work/_temp/git-credentials-e928b59d-7da5-4275-800b-0b419fc95f3b.config
2026-11-04T18:55:14.5440873Z [command]/usr/bin/git config --local --unset includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path \/home\/runner\/work\/_temp\/git\-credentials\-e928b59d\-7da5\-4275\-800b\-0b419fc95f3b\.config
2026-11-04T18:55:14.5489514Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/github/workspace/.git.path
2026-11-04T18:55:14.5524922Z /github/runner_temp/git-credentials-e928b59d-7da5-4275-800b-0b419fc95f3b.config
2026-11-04T18:55:14.5532956Z [command]/usr/bin/git config --local --unset includeif.gitdir:/github/workspace/.git.path \/github\/runner_temp\/git\-credentials\-e928b59d\-7da5\-4275\-800b\-0b419fc95f3b\.config
2026-11-04T18:55:14.5578670Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/github/workspace/.git/worktrees/*.path
2026-11-04T18:55:14.5614119Z /github/runner_temp/git-credentials-e928b59d-7da5-4275-800b-0b419fc95f3b.config
2026-11-04T18:55:14.5625406Z [command]/usr/bin/git config --local --unset includeif.gitdir:/github/workspace/.git/worktrees/*.path \/github\/runner_temp\/git\-credentials\-e928b59d\-7da5\-4275\-800b\-0b419fc95f3b\.config
2026-11-04T18:55:14.5672201Z [command]/usr/bin/git submodule foreach --recursive git config --local --show-origin --name-only --get-regexp remote.origin.url
2026-11-04T18:55:14.5919661Z Removing credentials config '/home/runner/work/_temp/git-credentials-e928b59d-7da5-4275-800b-0b419fc95f3b.config'
2026-11-04T18:55:14.5929457Z ##[endgroup]
2026-11-04T18:55:14.6316135Z Prepare all required actions
2026-11-04T18:55:14.6316824Z Getting action download info
2026-11-04T18:55:14.8047196Z Download action repository 'actions/setup-dotnet@a98b56852c35b8e3190ac28c8c2271da59106c68' (SHA:a98b56852c35b8e3190ac28c8c2271da59106c68)
2026-11-04T18:55:15.1921352Z Download action repository 'actions/cache@55cc8345863c7cc4c66a329aec7e433d2d1c52a9' (SHA:55cc8345863c7cc4c66a329aec7e433d2d1c52a9)
2026-11-04T18:55:15.3245630Z ##[group]Run ./.github/actions/preparar-dotnet
2026-11-04T18:55:15.3245956Z env:
2026-11-04T18:55:15.3246170Z   DOTNET_NOLOGO: true
2026-11-04T18:55:15.3246423Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:15.3246698Z   CONFIGURACAO: Release
2026-11-04T18:55:15.3246973Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:15.3247508Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:15.3248329Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:15.3249109Z   RESULTADOS: resultados-ci
2026-11-04T18:55:15.3249385Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:15.3249678Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:15.3250193Z ##[endgroup]
2026-11-04T18:55:15.3307171Z ##[start-action display=Instalar o SDK do .NET 10;id=__self.__actions_setup-dotnet]
2026-11-04T18:55:15.3376219Z ##[group]Run actions/setup-dotnet@a98b56852c35b8e3190ac28c8c2271da59106c68
2026-11-04T18:55:15.3376623Z with:
2026-11-04T18:55:15.3376840Z   dotnet-version: 10.0.x
2026-11-04T18:55:15.3377089Z   cache: false
2026-11-04T18:55:15.3377307Z env:
2026-11-04T18:55:15.3377507Z   DOTNET_NOLOGO: true
2026-11-04T18:55:15.3377753Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:15.3378028Z   CONFIGURACAO: Release
2026-11-04T18:55:15.3378296Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:15.3378831Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:15.3379912Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:15.3380478Z   RESULTADOS: resultados-ci
2026-11-04T18:55:15.3380770Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:15.3381078Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:15.3381377Z ##[endgroup]
2026-11-04T18:55:15.4853921Z [command]/home/runner/work/_actions/actions/setup-dotnet/a98b56852c35b8e3190ac28c8c2271da59106c68/externals/install-dotnet.sh --skip-non-versioned-files --runtime dotnet --channel LTS
2026-11-04T18:55:15.6869585Z dotnet-install: .NET Core Runtime with version '10.0.14' is already installed.
2026-11-04T18:55:15.6898230Z [command]/home/runner/work/_actions/actions/setup-dotnet/a98b56852c35b8e3190ac28c8c2271da59106c68/externals/install-dotnet.sh --skip-non-versioned-files --channel 10.0
2026-11-04T18:55:15.8609489Z dotnet-install: .NET Core SDK with version '10.0.403' is already installed.
2026-11-04T18:55:15.8755534Z ##[end-action id=__self.__actions_setup-dotnet;outcome=success;conclusion=success;duration_ms=544]
2026-11-04T18:55:15.8758710Z ##[start-action display=Reutilizar a cache dos pacotes NuGet;id=__self.__actions_cache]
2026-11-04T18:55:16.0610923Z ##[group]Run actions/cache@55cc8345863c7cc4c66a329aec7e433d2d1c52a9
2026-11-04T18:55:16.0611294Z with:
2026-11-04T18:55:16.0611497Z   path: ~/.nuget/packages
2026-11-04T18:55:16.0611916Z   key: nuget-Linux-c8252acdcad9b687c3131c725ee3c1b10105ceb23478247a43e8389c12c19830
2026-11-04T18:55:16.0612392Z   restore-keys: nuget-Linux-

2026-11-04T18:55:16.0612662Z   enableCrossOsArchive: false
2026-11-04T18:55:16.0612921Z   fail-on-cache-miss: false
2026-11-04T18:55:16.0613168Z   lookup-only: false
2026-11-04T18:55:16.0613390Z   save-always: false
2026-11-04T18:55:16.0613607Z env:
2026-11-04T18:55:16.0613798Z   DOTNET_NOLOGO: true
2026-11-04T18:55:16.0614031Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:16.0614300Z   CONFIGURACAO: Release
2026-11-04T18:55:16.0614878Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:16.0615424Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:16.0616236Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:16.0616833Z   RESULTADOS: resultados-ci
2026-11-04T18:55:16.0617084Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:16.0617366Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:16.0617662Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:55:16.0617905Z ##[endgroup]
2026-11-04T18:55:16.4638436Z Cache hit for: nuget-Linux-c8252acdcad9b687c3131c725ee3c1b10105ceb23478247a43e8389c12c19830
2026-11-04T18:55:17.7907445Z Received 12582912 of 283633656 (4.4%), 12.0 MBs/sec
2026-11-04T18:55:18.7915826Z Received 130023424 of 283633656 (45.8%), 62.0 MBs/sec
2026-11-04T18:55:19.7912224Z Received 130023424 of 283633656 (45.8%), 41.3 MBs/sec
2026-11-04T18:55:20.7913552Z Received 130023424 of 283633656 (45.8%), 31.0 MBs/sec
2026-11-04T18:55:21.7922649Z Received 171966464 of 283633656 (60.6%), 32.8 MBs/sec
2026-11-04T18:55:22.7956628Z Received 264241152 of 283633656 (93.2%), 42.0 MBs/sec
2026-11-04T18:55:23.7936371Z Received 264241152 of 283633656 (93.2%), 36.0 MBs/sec
2026-11-04T18:55:24.7932068Z Received 279439352 of 283633656 (98.5%), 33.3 MBs/sec
2026-11-04T18:55:25.7935373Z Received 279439352 of 283633656 (98.5%), 29.6 MBs/sec
2026-11-04T18:55:26.3640772Z Received 283633656 of 283633656 (100.0%), 28.3 MBs/sec
2026-11-04T18:55:26.3650228Z Cache Size: ~270 MB (283633656 B)
2026-11-04T18:55:26.3721246Z [command]/usr/bin/tar -xf /home/runner/work/_temp/ef9c7cf3-e6ab-46ce-84e5-b645fc040fa9/cache.tzst -P -C /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO --use-compress-program unzstd
2026-11-04T18:55:27.5427094Z Cache restored successfully
2026-11-04T18:55:27.5811397Z Cache restored from key: nuget-Linux-c8252acdcad9b687c3131c725ee3c1b10105ceb23478247a43e8389c12c19830
2026-11-04T18:55:27.5934239Z ##[end-action id=__self.__actions_cache;outcome=success;conclusion=success;duration_ms=11717]
2026-11-04T18:55:27.5937316Z ##[start-action display=Registar o SDK utilizado;id=__self.__run]
2026-11-04T18:55:27.5976088Z ##[group]Run echo "SDK do .NET: $(dotnet --version)"
2026-11-04T18:55:27.5976534Z [36;1mecho "SDK do .NET: $(dotnet --version)"[0m
2026-11-04T18:55:27.5976861Z [36;1m{[0m
2026-11-04T18:55:27.5977097Z [36;1m  echo "### Ambiente .NET"[0m
2026-11-04T18:55:27.5977375Z [36;1m  echo ""[0m
2026-11-04T18:55:27.5977620Z [36;1m  echo "| Elemento | Valor |"[0m
2026-11-04T18:55:27.5977912Z [36;1m  echo "|---|---|"[0m
2026-11-04T18:55:27.5978228Z [36;1m  echo "| SDK do .NET | \`$(dotnet --version)\` |"[0m
2026-11-04T18:55:27.5978681Z [36;1m  echo "| global.json | \`$(tr -d '\n\r ' < src/backend/global.json)\` |"[0m
2026-11-04T18:55:27.5979692Z [36;1m  echo "| Runner | \`${RUNNER_OS} ${ImageOS:-} ${ImageVersion:-}\` |"[0m
2026-11-04T18:55:27.5980342Z [36;1m  echo ""[0m
2026-11-04T18:55:27.5980732Z [36;1m} >> "$GITHUB_STEP_SUMMARY"[0m
2026-11-04T18:55:27.6005906Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:55:27.6006278Z env:
2026-11-04T18:55:27.6006518Z   DOTNET_NOLOGO: true
2026-11-04T18:55:27.6006779Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:27.6007063Z   CONFIGURACAO: Release
2026-11-04T18:55:27.6007347Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:27.6007892Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:27.6008683Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:27.6009592Z   RESULTADOS: resultados-ci
2026-11-04T18:55:27.6009882Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:27.6010191Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:27.6010505Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:55:27.6010767Z ##[endgroup]
2026-11-04T18:55:34.9241916Z SDK do .NET: 10.0.403
2026-11-04T18:55:35.0273502Z ##[end-action id=__self.__run;outcome=success;conclusion=success;duration_ms=7433]
2026-11-04T18:55:35.0335884Z ##[group]Run dotnet restore "$SOLUCAO"
2026-11-04T18:55:35.0336243Z [36;1mdotnet restore "$SOLUCAO"[0m
2026-11-04T18:55:35.0356685Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:55:35.0357031Z env:
2026-11-04T18:55:35.0357229Z   DOTNET_NOLOGO: true
2026-11-04T18:55:35.0357469Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:35.0357738Z   CONFIGURACAO: Release
2026-11-04T18:55:35.0358012Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:35.0358535Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:35.0359649Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:35.0360225Z   RESULTADOS: resultados-ci
2026-11-04T18:55:35.0360483Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:35.0360762Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:35.0361054Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:55:35.0361336Z ##[endgroup]
2026-11-04T18:55:40.0453244Z   Determining projects to restore...
2026-11-04T18:55:43.1095978Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Application/ClinicaVeterinaria.Application.csproj (in 1.11 sec).
2026-11-04T18:55:43.1111416Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Infrastructure/ClinicaVeterinaria.Infrastructure.csproj (in 1.13 sec).
2026-11-04T18:55:43.1114038Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj (in 1.13 sec).
2026-11-04T18:55:43.1116386Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj (in 1.16 sec).
2026-11-04T18:55:43.1161102Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Domain/ClinicaVeterinaria.Domain.csproj (in 2 ms).
2026-11-04T18:55:43.1627111Z   Restored /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj (in 42 ms).
2026-11-04T18:55:43.2185661Z ##[group]Run dotnet build "$TESTES_UNITARIOS" --configuration "$CONFIGURACAO" --no-restore
2026-11-04T18:55:43.2186334Z [36;1mdotnet build "$TESTES_UNITARIOS" --configuration "$CONFIGURACAO" --no-restore[0m
2026-11-04T18:55:43.2206273Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:55:43.2206620Z env:
2026-11-04T18:55:43.2206826Z   DOTNET_NOLOGO: true
2026-11-04T18:55:43.2207082Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:43.2207347Z   CONFIGURACAO: Release
2026-11-04T18:55:43.2207608Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:43.2208134Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:43.2209173Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:43.2209792Z   RESULTADOS: resultados-ci
2026-11-04T18:55:43.2210053Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:43.2210361Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:43.2210662Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:55:43.2210905Z ##[endgroup]
2026-11-04T18:55:51.6833888Z   ClinicaVeterinaria.Domain -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Domain/bin/Release/net10.0/ClinicaVeterinaria.Domain.dll
2026-11-04T18:55:53.4336566Z   ClinicaVeterinaria.Application -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Application/bin/Release/net10.0/ClinicaVeterinaria.Application.dll
2026-11-04T18:55:55.6762557Z   ClinicaVeterinaria.Infrastructure -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Infrastructure/bin/Release/net10.0/ClinicaVeterinaria.Infrastructure.dll
2026-11-04T18:55:59.3054803Z   ClinicaVeterinaria.Api -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.Api/bin/Release/net10.0/ClinicaVeterinaria.Api.dll
2026-11-04T18:56:03.9664976Z   ClinicaVeterinaria.UnitTests -> /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.UnitTests/bin/Release/net10.0/ClinicaVeterinaria.UnitTests.dll
2026-11-04T18:56:03.9932844Z 
2026-11-04T18:56:03.9942697Z Build succeeded.
2026-11-04T18:56:03.9943360Z     0 Warning(s)
2026-11-04T18:56:03.9943826Z     0 Error(s)
2026-11-04T18:56:03.9944033Z 
2026-11-04T18:56:03.9944208Z Time Elapsed 00:00:20.50
2026-11-04T18:56:04.0248099Z ##[group]Run dotnet test "$TESTES_UNITARIOS" \
2026-11-04T18:56:04.0248785Z [36;1mdotnet test "$TESTES_UNITARIOS" \[0m
2026-11-04T18:56:04.0249633Z [36;1m  --configuration "$CONFIGURACAO" \[0m
2026-11-04T18:56:04.0250156Z [36;1m  --no-build \[0m
2026-11-04T18:56:04.0250699Z [36;1m  --results-directory "$RESULTADOS/testes-unitarios" \[0m
2026-11-04T18:56:04.0251752Z [36;1m  --logger "trx;LogFileName=testes-unitarios.trx" \[0m
2026-11-04T18:56:04.0252396Z [36;1m  --logger "console;verbosity=normal" \[0m
2026-11-04T18:56:04.0252970Z [36;1m  --collect:"XPlat Code Coverage" \[0m
2026-11-04T18:56:04.0253517Z [36;1m  --blame-hang-timeout 5m[0m
2026-11-04T18:56:04.0282897Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:56:04.0283492Z env:
2026-11-04T18:56:04.0283811Z   DOTNET_NOLOGO: true
2026-11-04T18:56:04.0284216Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:56:04.0284708Z   CONFIGURACAO: Release
2026-11-04T18:56:04.0285220Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:56:04.0286140Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:56:04.0287552Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:56:04.0288596Z   RESULTADOS: resultados-ci
2026-11-04T18:56:04.0289336Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:56:04.0289829Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:56:04.0290344Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:56:04.0290778Z ##[endgroup]
2026-11-04T18:56:04.6677205Z Test run for /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.UnitTests/bin/Release/net10.0/ClinicaVeterinaria.UnitTests.dll (.NETCoreApp,Version=v10.0)
2026-11-04T18:56:05.4523238Z A total of 1 test files matched the specified pattern.
2026-11-04T18:56:07.1347061Z [xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v2.5.3.1+6b60a9e56a (64-bit .NET 10.0.14)
2026-11-04T18:56:07.4923950Z [xUnit.net 00:00:00.37]   Discovering: ClinicaVeterinaria.UnitTests
2026-11-04T18:56:07.6644344Z [xUnit.net 00:00:00.54]   Discovered:  ClinicaVeterinaria.UnitTests
2026-11-04T18:56:07.6690796Z [xUnit.net 00:00:00.55]   Starting:    ClinicaVeterinaria.UnitTests
2026-11-04T18:56:07.9024899Z   Passed I064 — Uma claim exp em falta ou inválida não dá data de expiração(valor: "1791993600.5") [12 ms]
2026-11-04T18:56:07.9026871Z [xUnit.net 00:00:00.74]     I037 - falha provocada (nao fazer merge) [FAIL]
2026-11-04T18:56:07.9051229Z   Passed I064 — Uma claim exp em falta ou inválida não dá data de expiração(valor: null) [< 1 ms]
2026-11-04T18:56:07.9052689Z   Passed I064 — Uma claim exp em falta ou inválida não dá data de expiração(valor: "-1") [< 1 ms]
2026-11-04T18:56:07.9054130Z   Passed I064 — Uma claim exp em falta ou inválida não dá data de expiração(valor: "abc") [< 1 ms]
2026-11-04T18:56:07.9055662Z   Passed I064 — Uma claim exp em falta ou inválida não dá data de expiração(valor: "253402300800") [< 1 ms]
2026-11-04T18:56:07.9057115Z   Passed I064 — Uma claim exp em falta ou inválida não dá data de expiração(valor: "") [< 1 ms]
2026-11-04T18:56:07.9058127Z [xUnit.net 00:00:00.74]       Falha provocada para a I037.
2026-11-04T18:56:07.9058783Z [xUnit.net 00:00:00.75]       Stack Trace:
2026-11-04T18:56:07.9061302Z [xUnit.net 00:00:00.75]         /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.UnitTests/FalhaProvocadaI037Tests.cs(6,0): at ClinicaVeterinaria.UnitTests.FalhaProvocadaI037Tests.FalhaProvocada()
2026-11-04T18:56:07.9063902Z [xUnit.net 00:00:00.75]            at System.Reflection.MethodBaseInvoker.InterpretedInvoke_Method(Object obj, IntPtr* args)
2026-11-04T18:56:07.9065573Z [xUnit.net 00:00:00.75]            at System.Reflection.MethodBaseInvoker.InvokeWithNoArgs(Object obj, BindingFlags invokeAttr)
2026-11-04T18:56:07.9066912Z   Failed I037 - falha provocada (nao fazer merge) [5 ms]
2026-11-04T18:56:07.9067985Z   Error Message:
2026-11-04T18:56:07.9068431Z    Falha provocada para a I037.
2026-11-04T18:56:07.9069158Z   Stack Trace:
2026-11-04T18:56:07.9071148Z      at ClinicaVeterinaria.UnitTests.FalhaProvocadaI037Tests.FalhaProvocada() in /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/src/backend/ClinicaVeterinaria.UnitTests/FalhaProvocadaI037Tests.cs:line 6
2026-11-04T18:56:07.9073698Z    at System.Reflection.MethodBaseInvoker.InterpretedInvoke_Method(Object obj, IntPtr* args)
2026-11-04T18:56:07.9075571Z    at System.Reflection.MethodBaseInvoker.InvokeWithNoArgs(Object obj, BindingFlags invokeAttr)
2026-11-04T18:56:07.9077439Z   Passed UT-F003-066 a 073 — As transições não permitidas devolvem 422(estadoAtual: "Realizada", novoEstado: "Confirmada") [1 ms]
2026-11-04T18:56:07.9079938Z   Passed UT-F003-066 a 073 — As transições não permitidas devolvem 422(estadoAtual: "Cancelada", novoEstado: "Realizada") [< 1 ms]
2026-11-04T18:56:07.9081939Z   Passed UT-F003-066 a 073 — As transições não permitidas devolvem 422(estadoAtual: "Pendente", novoEstado: "Realizada") [< 1 ms]
2026-11-04T18:56:07.9083989Z   Passed UT-F003-066 a 073 — As transições não permitidas devolvem 422(estadoAtual: "Cancelada", novoEstado: "Cancelada") [< 1 ms]
2026-11-04T18:56:07.9085982Z   Passed UT-F003-066 a 073 — As transições não permitidas devolvem 422(estadoAtual: "Realizada", novoEstado: "Cancelada") [< 1 ms]
2026-11-04T18:56:07.9088095Z   Passed UT-F003-066 a 073 — As transições não permitidas devolvem 422(estadoAtual: "Confirmada", novoEstado: "Confirmada") [< 1 ms]
2026-11-04T18:56:07.9090317Z   Passed UT-F003-066 a 073 — As transições não permitidas devolvem 422(estadoAtual: "Cancelada", novoEstado: "Confirmada") [< 1 ms]
2026-11-04T18:56:07.9092283Z   Passed UT-F003-066 a 073 — As transições não permitidas devolvem 422(estadoAtual: "Realizada", novoEstado: "Realizada") [< 1 ms]
2026-11-04T18:56:07.9192582Z   Passed UT-F003-062 a 065 — As quatro transições permitidas são aceites(estadoAtual: "Pendente", novoEstado: "Confirmada") [< 1 ms]
2026-11-04T18:56:07.9212316Z   Passed UT-F003-062 a 065 — As quatro transições permitidas são aceites(estadoAtual: "Confirmada", novoEstado: "Realizada") [< 1 ms]
2026-11-04T18:56:07.9215044Z   Passed UT-F003-062 a 065 — As quatro transições permitidas são aceites(estadoAtual: "Pendente", novoEstado: "Cancelada") [< 1 ms]
2026-11-04T18:56:07.9217859Z   Passed UT-F003-062 a 065 — As quatro transições permitidas são aceites(estadoAtual: "Confirmada", novoEstado: "Cancelada") [< 1 ms]
2026-11-04T18:56:07.9220590Z   Passed I074 — O estado é normalizado sem distinguir maiúsculas ou espaços(recebido: "confirmada", esperado: "Confirmada") [4 ms]
2026-11-04T18:56:07.9222816Z   Passed I074 — O estado é normalizado sem distinguir maiúsculas ou espaços(recebido: "cancelada", esperado: "Cancelada") [< 1 ms]
2026-11-04T18:56:07.9225146Z   Passed I074 — O estado é normalizado sem distinguir maiúsculas ou espaços(recebido: " REALIZADA ", esperado: "Realizada") [< 1 ms]
2026-11-04T18:56:07.9227231Z   Passed UT-F003-074 — Pendente nunca pode ser escolhido manualmente(estadoAtual: "Cancelada") [< 1 ms]
2026-11-04T18:56:07.9229344Z   Passed UT-F003-074 — Pendente nunca pode ser escolhido manualmente(estadoAtual: "Confirmada") [< 1 ms]
2026-11-04T18:56:07.9395055Z   Passed UT-F003-074 — Pendente nunca pode ser escolhido manualmente(estadoAtual: "Pendente") [< 1 ms]
2026-11-04T18:56:07.9420310Z   Passed UT-F003-074 — Pendente nunca pode ser escolhido manualmente(estadoAtual: "Realizada") [< 1 ms]
2026-11-04T18:56:07.9422371Z   Passed UT-F003-075 — Um estado inexistente devolve 400 [< 1 ms]
2026-11-04T18:56:07.9442114Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClienteTests.TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(telefone: "123456789012345678901") [50 ms]
2026-11-04T18:56:07.9455163Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClienteTests.TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(telefone: "+12345678901234567890") [< 1 ms]
2026-11-04T18:56:07.9459236Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClienteTests.TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(telefone: "12345678") [< 1 ms]
2026-11-04T18:56:07.9462369Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClienteTests.TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(telefone: "91234567a") [< 1 ms]
2026-11-04T18:56:07.9465783Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClienteTests.TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(telefone: "91+2345678") [< 1 ms]
2026-11-04T18:56:07.9468737Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClienteTests.TelefoneAte20CaracteresEAceite(telefone: "+1234567890123456789") [< 1 ms]
2026-11-04T18:56:07.9579947Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClienteTests.TelefoneAte20CaracteresEAceite(telefone: "912345678") [< 1 ms]
2026-11-04T18:56:07.9589415Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClienteTests.TelefoneAte20CaracteresEAceite(telefone: "+351 912 345 678") [< 1 ms]
2026-11-04T18:56:07.9592163Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClienteTests.TelefoneAte20CaracteresEAceite(telefone: "12345678901234567890") [< 1 ms]
2026-11-04T18:56:07.9595104Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClinicaTests.TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(telefone: "22900000") [5 ms]
2026-11-04T18:56:07.9598188Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClinicaTests.TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(telefone: "22900000a") [1 ms]
2026-11-04T18:56:07.9601550Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClinicaTests.TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(telefone: "22+9000000") [< 1 ms]
2026-11-04T18:56:07.9604686Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClinicaTests.TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(telefone: "+12345678901234567890") [< 1 ms]
2026-11-04T18:56:07.9607918Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClinicaTests.TelefoneForaDoFormatoOuComMaisDe20CaracteresERejeitado(telefone: "123456789012345678901") [< 1 ms]
2026-11-04T18:56:07.9611091Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClinicaTests.TelefoneAte20CaracteresEAceite(telefone: "+351 229 000 000") [< 1 ms]
2026-11-04T18:56:08.1806782Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClinicaTests.TelefoneAte20CaracteresEAceite(telefone: "229000000") [< 1 ms]
2026-11-04T18:56:08.1823239Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClinicaTests.TelefoneAte20CaracteresEAceite(telefone: "+1234567890123456789") [< 1 ms]
2026-11-04T18:56:08.1850624Z   Passed ClinicaVeterinaria.UnitTests.Validacao.TelefoneClinicaTests.TelefoneAte20CaracteresEAceite(telefone: "12345678901234567890") [< 1 ms]
2026-11-04T18:56:08.1879132Z   Passed I077 e I078 — EP-031 e EP-032 exigem também a política Veterinário (o Auxiliar recebe 403)(nomeAcao: "Criar") [51 ms]
2026-11-04T18:56:08.1910585Z   Passed I077 e I078 — EP-031 e EP-032 exigem também a política Veterinário (o Auxiliar recebe 403)(nomeAcao: "Atualizar") [< 1 ms]
2026-11-04T18:56:08.1922148Z   Passed I077 — EP-032 devolve 200 com o registo e usa o Veterinário do token [214 ms]
2026-11-04T18:56:08.1940388Z   Passed I078 — o Auxiliar Veterinário só tem operações de leitura (GET) neste controlador [13 ms]
2026-11-04T18:56:08.1970352Z   Passed I077 — As rotas são POST /registos-clinicos e PUT /registos-clinicos/{id} [6 ms]
2026-11-04T18:56:08.1972126Z   Passed I077 — um token de Veterinário sem a claim veterinarioId dá 403 sem chamar o serviço [6 ms]
2026-11-04T18:56:08.3071179Z   Passed I078 — o controlador aceita só o Veterinário e o Auxiliar Veterinário e nenhuma ação dispensa a autenticação [< 1 ms]
2026-11-04T18:56:08.3073164Z   Passed I077 — EP-031 devolve 201 com Location /registos-clinicos/{id} e usa o Veterinário do token [1 ms]
2026-11-04T18:56:08.3100139Z   Passed I078 — EP-033 devolve 200 com o registo do serviço [1 ms]
2026-11-04T18:56:08.3130839Z   Passed I078 — EP-033 e EP-034 não acrescentam restrições: Veterinário e Auxiliar Veterinário leem(nomeAcao: "Obter") [< 1 ms]
2026-11-04T18:56:08.3160700Z   Passed I078 — EP-033 e EP-034 não acrescentam restrições: Veterinário e Auxiliar Veterinário leem(nomeAcao: "ObterHistorico") [< 1 ms]
2026-11-04T18:56:08.3190150Z   Passed I078 — EP-034 devolve 200 com o histórico e passa o animal e as datas ao serviço [3 ms]
2026-11-04T18:56:08.3192254Z   Passed I078 — As rotas são GET /registos-clinicos/{id} e GET /animais/{id}/historico-clinico [< 1 ms]
2026-11-04T18:56:08.3240812Z   Passed I064 — A expiração lida do token validado pela API é a do token emitido [544 ms]
2026-11-04T18:56:08.3430273Z   Passed I064 — A claim exp, em segundos desde 1970, é convertida para UTC [7 ms]
2026-11-04T18:56:08.3459471Z   Passed UT-F006-013, UT-F006-014, UT-F006-015 e UT-F006-016 — intervaloDias de 1 a 32767 quando indicado; lote até 50 e produto até 150 caracteres(intervaloDias: 32767, tamanhoLote: 50, tamanhoProduto: 150, valido: True) [4 ms]
2026-11-04T18:56:08.3501379Z   Passed UT-F006-013, UT-F006-014, UT-F006-015 e UT-F006-016 — intervaloDias de 1 a 32767 quando indicado; lote até 50 e produto até 150 caracteres(intervaloDias: 32768, tamanhoLote: 50, tamanhoProduto: 150, valido: False) [1 ms]
2026-11-04T18:56:08.3504926Z   Passed UT-F006-013, UT-F006-014, UT-F006-015 e UT-F006-016 — intervaloDias de 1 a 32767 quando indicado; lote até 50 e produto até 150 caracteres(intervaloDias: 1, tamanhoLote: 50, tamanhoProduto: 150, valido: True) [< 1 ms]
2026-11-04T18:56:08.3541815Z   Passed UT-F006-013, UT-F006-014, UT-F006-015 e UT-F006-016 — intervaloDias de 1 a 32767 quando indicado; lote até 50 e produto até 150 caracteres(intervaloDias: 90, tamanhoLote: 51, tamanhoProduto: 150, valido: False) [< 1 ms]
2026-11-04T18:56:08.3571376Z   Passed UT-F006-013, UT-F006-014, UT-F006-015 e UT-F006-016 — intervaloDias de 1 a 32767 quando indicado; lote até 50 e produto até 150 caracteres(intervaloDias: -30, tamanhoLote: 50, tamanhoProduto: 150, valido: False) [< 1 ms]
2026-11-04T18:56:08.3601193Z   Passed UT-F006-013, UT-F006-014, UT-F006-015 e UT-F006-016 — intervaloDias de 1 a 32767 quando indicado; lote até 50 e produto até 150 caracteres(intervaloDias: null, tamanhoLote: 50, tamanhoProduto: 150, valido: True) [< 1 ms]
2026-11-04T18:56:08.3641275Z   Passed UT-F006-013, UT-F006-014, UT-F006-015 e UT-F006-016 — intervaloDias de 1 a 32767 quando indicado; lote até 50 e produto até 150 caracteres(intervaloDias: 0, tamanhoLote: 50, tamanhoProduto: 150, valido: False) [< 1 ms]
2026-11-04T18:56:08.3671241Z   Passed UT-F006-013, UT-F006-014, UT-F006-015 e UT-F006-016 — intervaloDias de 1 a 32767 quando indicado; lote até 50 e produto até 150 caracteres(intervaloDias: 90, tamanhoLote: 50, tamanhoProduto: 151, valido: False) [< 1 ms]
2026-11-04T18:56:08.3700234Z   Passed I082 — tipo só aceita Vacina ou Desparasitacao(tipo: "Desparasitação", valido: False) [< 1 ms]
2026-11-04T18:56:08.3701852Z   Passed I082 — tipo só aceita Vacina ou Desparasitacao(tipo: "Vacinas", valido: False) [2 ms]
2026-11-04T18:56:08.3730423Z   Passed I082 — tipo só aceita Vacina ou Desparasitacao(tipo: "Vacina", valido: True) [< 1 ms]
2026-11-04T18:56:08.3760211Z   Passed I082 — tipo só aceita Vacina ou Desparasitacao(tipo: "vacina", valido: False) [< 1 ms]
2026-11-04T18:56:08.3761905Z   Passed I082 — tipo só aceita Vacina ou Desparasitacao(tipo: "Desparasitacao", valido: True) [< 1 ms]
2026-11-04T18:56:08.3790528Z   Passed UT-F006-020 — BoletimQuery: tipo opcional, só Vacina ou Desparasitacao(tipo: "Vacinas", valido: False) [< 1 ms]
2026-11-04T18:56:08.3810942Z   Passed UT-F006-020 — BoletimQuery: tipo opcional, só Vacina ou Desparasitacao(tipo: null, valido: True) [< 1 ms]
2026-11-04T18:56:08.3841188Z   Passed UT-F006-020 — BoletimQuery: tipo opcional, só Vacina ou Desparasitacao(tipo: "Desparasitacao", valido: True) [< 1 ms]
2026-11-04T18:56:08.3880435Z   Passed UT-F006-020 — BoletimQuery: tipo opcional, só Vacina ou Desparasitacao(tipo: "Vacina", valido: True) [< 1 ms]
2026-11-04T18:56:09.3621493Z   Passed UT-F006-010 — tipo, produto, lote e data de administração são obrigatórios(campo: "dataAdministracao") [< 1 ms]
2026-11-04T18:56:09.3650541Z   Passed UT-F006-010 — tipo, produto, lote e data de administração são obrigatórios(campo: "tipo") [< 1 ms]
2026-11-04T18:56:09.3670484Z   Passed UT-F006-010 — tipo, produto, lote e data de administração são obrigatórios(campo: "lote") [< 1 ms]
2026-11-04T18:56:09.3672629Z   Passed UT-F006-010 — tipo, produto, lote e data de administração são obrigatórios(campo: "produto") [< 1 ms]
2026-11-04T18:56:09.3674013Z   Passed I082 — o pedido de exemplo da API (EP-043) é válido [< 1 ms]
2026-11-04T18:56:09.3675259Z   Passed I065 — Uma conta de Cliente dá 404 (EP-005 só consulta contas de funcionário) [1 s]
2026-11-04T18:56:09.3676723Z   Passed I088 — diagnóstico com exatamente 1000 caracteres é aceite (fronteira de UT-F004-012) [1 s]
2026-11-04T18:56:09.4476679Z   Passed UT-F001-093 — Um estado diferente de Bloqueado dá 400 e a conta mantém-se ativa(estado: "Suspenso") [79 ms]
2026-11-04T18:56:09.4478467Z   Passed UT-F004-011 — tratamento com descrição vazia devolve 400 [49 ms]
2026-11-04T18:56:09.4536140Z   Passed UT-F001-093 — Um estado diferente de Bloqueado dá 400 e a conta mantém-se ativa(estado: "Ativo") [26 ms]
2026-11-04T18:56:09.4539305Z   Passed UT-F004-012 — diagnóstico com 1001 caracteres devolve 400 (máximo 1000) [4 ms]
2026-11-04T18:56:09.4540560Z   Passed UT-F001-082 — Um nome com 151 caracteres dá 400 [9 ms]
2026-11-04T18:56:09.4542062Z   Passed I088 (critério 3) — atualização com uma prescrição sem dosagem devolve 400 e a prescrição fica como estava [19 ms]
2026-11-04T18:56:09.4544082Z   Passed UT-F004-008 — prescrição sem medicamento, sem dosagem ou sem frequência devolve 400(campoEmFalta: "medicamento") [8 ms]
2026-11-04T18:56:09.4545764Z   Passed UT-F001-021 — O fim de sessão regista o jti como revogado até à expiração do token [1 s]
2026-11-04T18:56:09.4590135Z   Passed UT-F001-084 — O perfil Cliente dá 400 (e não 422) e nada muda [25 ms]
2026-11-04T18:56:09.4620588Z   Passed UT-F004-008 — prescrição sem medicamento, sem dosagem ou sem frequência devolve 400(campoEmFalta: "frequência") [6 ms]
2026-11-04T18:56:09.4622550Z   Passed I064 — Um jti vazio é recusado (400) e nada é gravado(jti: "   ") [4 ms]
2026-11-04T18:56:09.4650267Z   Passed I064 — Um jti vazio é recusado (400) e nada é gravado(jti: "") [< 1 ms]
2026-11-04T18:56:09.4671594Z   Passed UT-F001-002 — Login de um Cliente devolve o clienteId e passa-o ao IJwtTokenService [1 s]
2026-11-04T18:56:09.4710367Z   Passed UT-F004-008 — prescrição sem medicamento, sem dosagem ou sem frequência devolve 400(campoEmFalta: "dosagem") [7 ms]
2026-11-04T18:56:09.4751743Z   Passed I065 — Um perfil fora da enumeração dá 400 (e não 422)(perfil: "Gestor") [15 ms]
2026-11-04T18:56:09.4753383Z   Passed I064 — O fim de sessão não afeta os outros tokens do mesmo utilizador [13 ms]
2026-11-04T18:56:09.4755048Z   Passed I065 — Um perfil fora da enumeração dá 400 (e não 422)(perfil: "Auxiliar") [3 ms]
2026-11-04T18:56:09.4766685Z   Passed UT-F001-006 — O e-mail é comparado sem os espaços iniciais e finais [14 ms]
2026-11-04T18:56:09.4810315Z   Passed I064 — A revogação fica na base de dados e continua a valer num novo arranque da API [3 ms]
2026-11-04T18:56:09.4840455Z   Passed UT-F001-022 — Terminar a sessão duas vezes com o mesmo jti não falha nem repete a revogação [2 ms]
2026-11-04T18:56:09.4870303Z   Passed UT-F004-002 — sem notas nem diagnóstico, só com uma prescrição completa, é aceite [15 ms]
2026-11-04T18:56:09.5001101Z   Passed I066 — O estado Bloqueado é aceite sem distinguir maiúsculas e sem motivo(estado: " BLOQUEADO ") [10 ms]
2026-11-04T18:56:09.5031259Z   Passed I066 — O estado Bloqueado é aceite sem distinguir maiúsculas e sem motivo(estado: "bloqueado") [1 ms]
2026-11-04T18:56:09.5057879Z   Passed I065 — Um estado fora da enumeração dá 400 [5 ms]
2026-11-04T18:56:09.5110148Z   Passed UT-F004-017 — consulta de outro Veterinário devolve 403 [15 ms]
2026-11-04T18:56:09.5139197Z   Passed I064 — A limpeza apaga só as revogações de tokens já expirados [18 ms]
2026-11-04T18:56:09.5182453Z   Passed UT-F001-016 — Um token com o jti revogado deixa de ser uma sessão válida [1 ms]
2026-11-04T18:56:09.5207572Z   Passed I064 — Depois do fim de sessão, o mesmo token deixa de ser uma sessão válida [1 ms]
2026-11-04T18:56:09.5280329Z   Passed UT-F001-020 — Conta bloqueada em /auth/me devolve 401 [24 ms]
2026-11-04T18:56:09.5300239Z   Passed I064 — A limpeza mantém a revogação de um token que expira no próprio instante [1 ms]
2026-11-04T18:56:09.5323734Z   Passed I064 — A data de revogação é gravada sem frações de segundo (DATETIME2(0)) [1 ms]
2026-11-04T18:56:09.5380431Z   Passed I064 — Sem revogações expiradas, a limpeza não apaga nada [1 ms]
2026-11-04T18:56:09.5389185Z   Passed UT-F004-015 — consulta Cancelada devolve 422 [9 ms]
2026-11-04T18:56:09.5450393Z   Passed UT-F001-010 — Conta bloqueada com a palavra-passe errada devolve 401 e não 403 [9 ms]
2026-11-04T18:56:09.5482501Z   Passed UT-F001-087 — podeRegistarApoio num Veterinário dá 422 e nada muda [20 ms]
2026-11-04T18:56:09.5527859Z   Passed I077 — os textos são aparados e as notas só com espaços ficam nulas [7 ms]
2026-11-04T18:56:09.5574205Z   Passed I071 — Um intervalo fora do horário não está disponível(inicio: "2026-10-15T11:45:00Z", duracao: 30) [20 ms]
2026-11-04T18:56:09.5576824Z   Passed I071 — Um intervalo fora do horário não está disponível(inicio: "2026-10-18T09:00:00Z", duracao: 30) [< 1 ms]
2026-11-04T18:56:09.5640243Z   Passed I071 — Um intervalo fora do horário não está disponível(inicio: "2026-10-15T07:30:00Z", duracao: 60) [< 1 ms]
2026-11-04T18:56:09.5670422Z   Passed I071 — Um intervalo fora do horário não está disponível(inicio: "2026-10-26T08:30:00Z", duracao: 30) [< 1 ms]
2026-11-04T18:56:09.5700168Z   Passed UT-F001-089 — Manter o próprio e-mail (com outras maiúsculas) não é conflito [17 ms]
2026-11-04T18:56:09.5755052Z   Passed UT-F001-001 — Login do Administrador devolve o token do IJwtTokenService e regista o último acesso [20 ms]
2026-11-04T18:56:09.5816954Z   Passed UT-F003-008 — Com duração de 45 minutos a grelha é de 45 em 45 minutos e só se a consulta couber [9 ms]
2026-11-04T18:56:09.5842507Z   Passed I071 — Uma consulta mais longa do que o período não tem horários [< 1 ms]
2026-11-04T18:56:09.5897726Z   Passed I071 — A hora local de Lisboa é convertida para UTC conforme a hora de verão(data: "2026-10-26", hora: "09:00", esperado: "2026-10-26T09:00:00Z") [< 1 ms]
2026-11-04T18:56:09.5920379Z   Passed UT-F001-013 — Conta ativa, com o perfil do token e jti não revogado: a sessão é válida [6 ms]
2026-11-04T18:56:09.5942166Z   Passed I071 — A hora local de Lisboa é convertida para UTC conforme a hora de verão(data: "2026-10-25", hora: "01:30", esperado: "2026-10-25T00:30:00Z") [3 ms]
2026-11-04T18:56:09.5990764Z   Passed I071 — A hora local de Lisboa é convertida para UTC conforme a hora de verão(data: "2026-10-15", hora: "10:00", esperado: "2026-10-15T09:00:00Z") [< 1 ms]
2026-11-04T18:56:09.6027585Z   Passed I071 — O dia da semana segue a base de dados: 1 = segunda … 7 = domingo(data: "2026-10-15", esperado: 4) [< 1 ms]
2026-11-04T18:56:09.6060272Z   Passed I071 — O dia da semana segue a base de dados: 1 = segunda … 7 = domingo(data: "2026-10-12", esperado: 1) [< 1 ms]
2026-11-04T18:56:09.6080138Z   Passed I071 — O dia da semana segue a base de dados: 1 = segunda … 7 = domingo(data: "2026-10-18", esperado: 7) [< 1 ms]
2026-11-04T18:56:09.6090395Z   Passed I071 — O dia da semana segue a base de dados: 1 = segunda … 7 = domingo(data: "2026-10-17", esperado: 6) [< 1 ms]
2026-11-04T18:56:09.6109910Z   Passed UT-F003-012 — Os horários que já começaram não são devolvidos [< 1 ms]
2026-11-04T18:56:09.6149928Z   Passed I071 — Um dia inteiramente no passado não tem horários livres [< 1 ms]
2026-11-04T18:56:09.6169760Z   Passed UT-F003-005 — Uma marcação Cancelada não ocupa horário [2 ms]
2026-11-04T18:56:09.6170902Z   Passed UT-F001-092 — Bloquear REC_A ativo passa o estado a Bloqueado [17 ms]
2026-11-04T18:56:09.6180535Z   Passed UT-F003-003 — Duas marcações seguidas ocupam os dois horários e libertam os vizinhos [4 ms]
2026-11-04T18:56:09.6181927Z   Passed UT-F001-079 — Um e-mail sem formato válido dá 400 com o erro no campo email [1 ms]
2026-11-04T18:56:09.6183114Z   Passed UT-F003-004 — O último horário de cada período acaba no fecho [< 1 ms]
2026-11-04T18:56:09.6240064Z   Passed UT-F001-078 — Um nome vazio dá 400 com o erro no campo nome [< 1 ms]
2026-11-04T18:56:09.6241772Z   Passed UT-F003-011 — Na hora de inverno, 09:00Z é 09:00 em Lisboa [< 1 ms]
2026-11-04T18:56:09.6267098Z   Passed I088 (critério 3) — atualização com um tratamento sem descrição devolve 400 e o registo fica como estava [36 ms]
2026-11-04T18:56:09.6300013Z   Passed UT-F001-014 — Conta bloqueada depois da emissão do token: a sessão deixa de ser válida [13 ms]
2026-11-04T18:56:09.6301431Z   Passed I065 — Os DTOs de resposta não têm campos de palavra-passe nem de hash (critério 4) [3 ms]
2026-11-04T18:56:09.6633445Z   Passed I071 — Um intervalo que se sobrepõe a uma marcação não cancelada não está disponível [4 ms]
2026-11-04T18:56:09.6640422Z   Passed I066 — Um pedido sem estado dá 400 na validação do DTO [< 1 ms]
2026-11-04T18:56:09.6652903Z   Passed UT-F003-009 — Uma marcação de 60 minutos às 10:15 ocupa todos os horários que se sobrepõem [< 1 ms]
2026-11-04T18:56:09.6674140Z   Passed UT-F001-011 — Com SuccessRehashNeeded, o login aceita e grava o novo hash [4 ms]
2026-11-04T18:56:09.6704437Z   Passed UT-F003-006 — Um domingo sem períodos é devolvido com a lista vazia [5 ms]
2026-11-04T18:56:09.6730185Z   Passed I071 — Uma marcação cancelada não impede um intervalo [< 1 ms]
2026-11-04T18:56:09.6744428Z   Passed I071 — Vários dias: um elemento por dia, por ordem, incluindo sábado e domingo [1 ms]
2026-11-04T18:56:09.6767321Z   Passed UT-F003-002 — Uma marcação Confirmada às 10:00 locais ocupa só esse horário [< 1 ms]
2026-11-04T18:56:09.6790833Z   Passed I071 — Um intervalo livre e dentro de um período está disponível, mesmo fora da grelha(inicio: "2026-10-15T09:15:00Z", duracao: 30) [1 ms]
2026-11-04T18:56:09.6820594Z   Passed UT-F001-012 — O registo da tentativa falhada tem o e-mail e o motivo, mas nunca a palavra-passe [6 ms]
2026-11-04T18:56:09.6832949Z   Passed I071 — Um intervalo livre e dentro de um período está disponível, mesmo fora da grelha(inicio: "2026-10-15T11:30:00Z", duracao: 30) [< 1 ms]
2026-11-04T18:56:09.6860617Z   Passed I071 — Um intervalo livre e dentro de um período está disponível, mesmo fora da grelha(inicio: "2026-10-15T08:00:00Z", duracao: 30) [< 1 ms]
2026-11-04T18:56:09.6880051Z   Passed I071 — Um intervalo livre e dentro de um período está disponível, mesmo fora da grelha(inicio: "2026-10-26T09:00:00Z", duracao: 30) [< 1 ms]
2026-11-04T18:56:09.6904359Z   Passed UT-F004-010 — prescrição sem duracaoDias é aceite [13 ms]
2026-11-04T18:56:09.6930533Z   Passed UT-F001-009 — Conta bloqueada com a palavra-passe certa devolve 403 sem emitir token [2 ms]
2026-11-04T18:56:09.6940496Z   Passed UT-F001-008 — Palavra-passe errada devolve 401 com a mesma mensagem, sem token e sem alterar o último acesso [2 ms]
2026-11-04T18:56:09.6972089Z   Passed UT-F001-019 — Cliente autenticado: clienteId e nome do registo Cliente [2 ms]
2026-11-04T18:56:09.6990704Z   Passed UT-F001-003 — E-mail vazio devolve 400 «O e-mail é obrigatório.» sem consultar o hasher nem a base de dados [3 ms]
2026-11-04T18:56:09.6993279Z   Passed UT-F003-007 — Entre os dois períodos do dia (13:00–14:00) não há horários [10 ms]
2026-11-04T18:56:09.7020034Z   Passed I071 — Uma duração não positiva ou um período invertido são recusados [< 1 ms]
2026-11-04T18:56:09.7041077Z   Passed UT-F003-001 — Sem marcações, um dia útil tem 18 horários de 30 em 30 minutos [< 1 ms]
2026-11-04T18:56:09.7065665Z   Passed I071 — Um horário que começa no próprio instante atual ainda é devolvido [< 1 ms]
2026-11-04T18:56:09.7093006Z   Passed I071 — Uma hora que não existe (mudança para a hora de verão) não tem UTC [< 1 ms]
2026-11-04T18:56:09.7120558Z   Passed I071 — No dia da mudança para a hora de verão, a hora que não existe não é devolvida [< 1 ms]
2026-11-04T18:56:09.7131238Z   Passed UT-F001-018 — Veterinário autenticado: nome do Funcionario, perfil Veterinario e sem clienteId [4 ms]
2026-11-04T18:56:09.7152230Z   Passed I071 — Uma marcação que acaba quando o horário começa não o ocupa (R-HORARIO) [3 ms]
2026-11-04T18:56:09.7183025Z   Passed UT-F003-016 — Sem data de início o pedido é recusado (obrigatória) [< 1 ms]
2026-11-04T18:56:09.7210353Z   Passed UT-F001-017 — Utilizador inexistente: a sessão não é válida [1 ms]
2026-11-04T18:56:09.7220816Z   Passed I071 — No pedido, sem dataInicio o [ApiController] responde 400 com a mensagem em português [1 ms]
2026-11-04T18:56:09.7253190Z   Passed I071 — Datas fora de 2000-01-01 a 9998-12-31 são recusadas(ano: 1, mes: 1, dia: 1) [< 1 ms]
2026-11-04T18:56:09.7270132Z   Passed I071 — Datas fora de 2000-01-01 a 9998-12-31 são recusadas(ano: 1999, mes: 12, dia: 31) [< 1 ms]
2026-11-04T18:56:09.7278061Z   Passed I071 — Datas fora de 2000-01-01 a 9998-12-31 são recusadas(ano: 9999, mes: 1, dia: 1) [< 1 ms]
2026-11-04T18:56:09.7310375Z   Passed UT-F001-005 — Um e-mail só com espaços conta como em falta (400) [1 ms]
2026-11-04T18:56:09.7320438Z   Passed UT-F001-004 — Palavra-passe vazia devolve 400 «A palavra-passe é obrigatória.» [2 ms]
2026-11-04T18:56:09.7330148Z   Passed I071 — Sem data de fim, o período é só o dia de início [3 ms]
2026-11-04T18:56:09.7340145Z   Passed UT-F003-014 — Um período de 32 dias é recusado [< 1 ms]
2026-11-04T18:56:09.7341641Z   Passed I071 — No pedido, um período válido não tem erros [1 ms]
2026-11-04T18:56:09.7358114Z   Passed UT-F003-015 — Uma data de fim anterior à de início é recusada [< 1 ms]
2026-11-04T18:56:09.7360354Z   Passed UT-F001-015 — Perfil alterado depois da emissão do token: a sessão deixa de ser válida [2 ms]
2026-11-04T18:56:09.7362133Z   Passed UT-F004-021 — registo inexistente devolve 404 [24 ms]
2026-11-04T18:56:09.7364315Z   Passed UT-F001-007 — E-mail inexistente devolve 401 «Credenciais inválidas.», verifica um hash de referência e regista o evento 3001 [3 ms]
2026-11-04T18:56:09.7366497Z   Passed I071 — No pedido, um período de 32 dias tem erro no campo dataFim [3 ms]
2026-11-04T18:56:09.7368050Z   Passed UT-F003-013 — Um período de 31 dias é aceite [< 1 ms]
2026-11-04T18:56:09.7369765Z   Passed I077 — o mesmo id de tratamento repetido no pedido devolve 400 [18 ms]
2026-11-04T18:56:09.7371503Z   Passed UT-F004-005 — consultaId = 0, ou em falta, devolve 400(consultaId: null) [4 ms]
2026-11-04T18:56:09.7373168Z   Passed UT-F004-005 — consultaId = 0, ou em falta, devolve 400(consultaId: 0) [< 1 ms]
2026-11-04T18:56:09.7374608Z   Passed UT-F004-016 — consulta inexistente devolve 404 [60 ms]
2026-11-04T18:56:09.7376130Z   Passed I071 — EP-021 sem Veterinários devolve uma lista vazia [91 ms]
2026-11-04T18:56:09.7377798Z   Passed UT-F004-013 — segundo registo para a mesma consulta devolve 409 e nada é gravado [47 ms]
2026-11-04T18:56:09.7379877Z   Passed I065 — O e-mail é guardado em minúsculas e um repetido com outras maiúsculas dá 409 [166 ms]
2026-11-04T18:56:09.7382055Z   Passed I065 — podeRegistarApoio em qualquer perfil que não seja AuxiliarVeterinario dá 422(perfil: "Veterinario") [1 ms]
2026-11-04T18:56:09.7384569Z   Passed I065 — podeRegistarApoio em qualquer perfil que não seja AuxiliarVeterinario dá 422(perfil: "Administrador") [< 1 ms]
2026-11-04T18:56:09.7810919Z   Passed I065 — Com perfil=Cliente a lista fica vazia (EP-004 só lista funcionários) [19 ms]
2026-11-04T18:56:09.7812810Z   Passed UT-F003-032 — veterinarioId inexistente ou de uma conta que não é de Veterinário devolve 404 [161 ms]
2026-11-04T18:56:09.7870354Z   Passed UT-F003-028 — marcação das 10:15 às 10:45 locais: nova às 10:30 devolve 422 [21 ms]
2026-11-04T18:56:09.7880379Z   Passed UT-F003-013 — Um período de 31 dias é aceite e devolve 31 dias [93 ms]
2026-11-04T18:56:09.7910308Z   Passed I071 — EP-022 devolve os horários livres em HH:mm, com a duração e o Veterinário [12 ms]
2026-11-04T18:56:09.7911842Z   Passed I088 (critério 3) — a atualização grava as notas clínicas e o diagnóstico novos [73 ms]
2026-11-04T18:56:09.8131401Z   Passed I071 — EP-022 retira os horários das marcações guardadas, mas não os das canceladas [9 ms]
2026-11-04T18:56:09.8138019Z   Passed UT-F003-018 — O id de quem não é Veterinário, ou inexistente, dá 404(conta: "REC_A") [6 ms]
2026-11-04T18:56:09.8190629Z   Passed I065 — A pesquisa procura no nome e no e-mail e nunca devolve Clientes(pesquisa: "Vet", esperados: "Vasco Vet|Vera Vet|Vitor Vet") [62 ms]
2026-11-04T18:56:09.8193084Z   Passed UT-F004-007 — notas só com espaços e listas vazias devolvem 400 [16 ms]
2026-11-04T18:56:09.8194986Z   Passed UT-F003-018 — O id de quem não é Veterinário, ou inexistente, dá 404(conta: "999") [6 ms]
2026-11-04T18:56:09.8280642Z   Passed I065 — A pesquisa procura no nome e no e-mail e nunca devolve Clientes(pesquisa: "carla.aux@", esperados: "Carla Aux") [6 ms]
2026-11-04T18:56:09.8300264Z   Passed I071 — EP-022 usa a duração configurada em EP-010 e ignora os períodos inativos [11 ms]
2026-11-04T18:56:09.8420611Z   Passed I065 — A pesquisa procura no nome e no e-mail e nunca devolve Clientes(pesquisa: "costa", esperados: "") [10 ms]
2026-11-04T18:56:09.8430268Z   Passed UT-F001-091 — Um e-mail sem formato válido dá 400 com o erro no campo email [< 1 ms]
2026-11-04T18:56:09.8450266Z   Passed I066 — Um perfil fora da enumeração dá 400 na atualização(perfil: "Auxiliar") [2 ms]
2026-11-04T18:56:09.8480169Z   Passed I066 — Um perfil fora da enumeração dá 400 na atualização(perfil: "Gestor") [1 ms]
2026-11-04T18:56:09.8511242Z   Passed UT-F003-014, UT-F003-015 e UT-F003-016 — Período de 32 dias, invertido ou sem data de início dá 400(dataInicio: "2026-10-01", dataFim: "2026-11-01", mensagem: "O período não pode ter mais de 31 dias.") [9 ms]
2026-11-04T18:56:09.8514592Z   Passed UT-F003-014, UT-F003-015 e UT-F003-016 — Período de 32 dias, invertido ou sem data de início dá 400(dataInicio: "2026-10-15", dataFim: "2026-10-14", mensagem: "A data de fim (dataFim) não pode ser anterior à da"···) [7 ms]
2026-11-04T18:56:09.8548642Z   Passed UT-F003-014, UT-F003-015 e UT-F003-016 — Período de 32 dias, invertido ou sem data de início dá 400(dataInicio: null, dataFim: "2026-10-15", mensagem: "A data de início (dataInicio) é obrigatória, no fo"···) [4 ms]
2026-11-04T18:56:09.8567033Z   Passed I071 — Um Veterinário com a conta bloqueada dá 404 (não está disponível para marcação) [6 ms]
2026-11-04T18:56:09.8900809Z   Passed I071 — EP-021 lista só os Veterinários com conta ativa, por nome [7 ms]
2026-11-04T18:56:09.8926253Z   Passed UT-F004-001 — cria o registo com tratamento e prescrição; o estado da marcação não muda [49 ms]
2026-11-04T18:56:09.8940290Z   Passed UT-F003-010 — Uma marcação de outro Veterinário não ocupa o horário [9 ms]
2026-11-04T18:56:09.8941629Z   Passed UT-F003-017 — Sem dataFim, a disponibilidade tem um único dia [6 ms]
2026-11-04T18:56:09.8970136Z   Passed I071 — EP-021 com Veterinários com o mesmo nome ordena pelo identificador [2 ms]
2026-11-04T18:56:09.8971619Z   Passed I077 — uma prescrição existente mantém-se indicando o id e é atualizada [17 ms]
2026-11-04T18:56:09.9000275Z   Passed I071 — Um período inválido dá 400 antes de se procurar o Veterinário [12 ms]
2026-11-04T18:56:09.9001853Z   Passed UT-F004-004 — consulta Realizada sem registo é aceite (Confirmada e Realizada são admitidas) [22 ms]
2026-11-04T18:56:09.9330812Z [xUnit.net 00:00:02.81]     UT-TRV-007 — Uma exceção inesperada dá 500 com mensagem genérica, sem a mensagem interna [SKIP]
2026-11-04T18:56:09.9361107Z [xUnit.net 00:00:02.81]       Defeito D-01 (I028, PA-07): o ExceptionHandlingMiddleware copia para detail a mensagem de qualquer exceção, também no 500. Reativar quando o defeito for corrigido.
2026-11-04T18:56:09.9390394Z   Passed I065 — Contas com o mesmo nome ficam ordenadas pelo identificador (ordem estável) [75 ms]
2026-11-04T18:56:09.9420015Z   Passed I065 — Um identificador inexistente dá 404 [1 ms]
2026-11-04T18:56:09.9450215Z   Passed I065 — A paginação devolve a página pedida; depois do fim, ou numa página muito alta, a lista fica vazia(pagina: 2147483647, tamanho: 100, esperados: "") [5 ms]
2026-11-04T18:56:09.9494130Z   Passed I065 — A paginação devolve a página pedida; depois do fim, ou numa página muito alta, a lista fica vazia(pagina: 3, tamanho: 3, esperados: "Vera Vet|Vitor Vet") [5 ms]
2026-11-04T18:56:09.9496719Z   Passed I065 — A paginação devolve a página pedida; depois do fim, ou numa página muito alta, a lista fica vazia(pagina: 4, tamanho: 3, esperados: "") [4 ms]
2026-11-04T18:56:09.9530997Z   Passed I065 — A paginação devolve a página pedida; depois do fim, ou numa página muito alta, a lista fica vazia(pagina: 2, tamanho: 3, esperados: "Rita Rececao|Rui Bloqueado|Vasco Vet") [4 ms]
2026-11-04T18:56:09.9560259Z   Passed UT-TRV-010 — O registo do pedido tem método, caminho, código e duração, mas nunca o token nem o corpo [53 ms]
2026-11-04T18:56:09.9573570Z   Skipped UT-TRV-007 — Uma exceção inesperada dá 500 com mensagem genérica, sem a mensagem interna [1 ms]
2026-11-04T18:56:09.9891053Z   Passed I088 (critério 4) — depois da atualização, o registo, os tratamentos e as prescrições continuam na mesma consulta e no mesmo paciente [66 ms]
2026-11-04T18:56:09.9920265Z   Passed I073 — a duração vem da configuração: com 45 minutos, 12:30 locais já não cabe (422) [195 ms]
2026-11-04T18:56:09.9950240Z   Passed UT-F004-003 — sem notas, diagnóstico nem prescrições, só com um tratamento, é aceite [7 ms]
2026-11-04T18:56:09.9980387Z   Passed I073 — motivo com espaços é aparado e motivo vazio fica nulo(motivo: "  Vacinação  ", esperado: "Vacinação") [10 ms]
2026-11-04T18:56:09.9990400Z   Passed UT-F004-014 — consulta Pendente devolve 422 [5 ms]
2026-11-04T18:56:10.0020149Z   Passed UT-F004-020 — atualização com tudo vazio devolve 400 e o registo fica como estava [5 ms]
2026-11-04T18:56:10.0044610Z   Passed I073 — motivo com espaços é aparado e motivo vazio fica nulo(motivo: "   ", esperado: null) [8 ms]
2026-11-04T18:56:10.0060459Z   Passed I073 — motivo com espaços é aparado e motivo vazio fica nulo(motivo: null, esperado: null) [12 ms]
2026-11-04T18:56:10.0152871Z   Passed UT-F004-018 — mantém T1 atualizado, acrescenta um novo, remove T2 e P1; regista autor e data [17 ms]
2026-11-04T18:56:10.0154210Z   Passed UT-F004-006 — notas e diagnóstico vazios e listas vazias devolvem 400 (nenhum elemento clínico) [6 ms]
2026-11-04T18:56:10.0155312Z   Passed UT-F003-030 — marcação Cancelada às 10:00 locais não ocupa o horário [11 ms]
2026-11-04T18:56:10.0156265Z   Passed UT-F004-009 — prescrição com duracaoDias = 0 devolve 400 [6 ms]
2026-11-04T18:56:10.0157291Z   Passed UT-F003-022 — animalId = 0 ou veterinarioId = 0 devolve 400(animalId: 0, veterinarioId: 1) [4 ms]
2026-11-04T18:56:10.0158403Z   Passed UT-F003-022 — animalId = 0 ou veterinarioId = 0 devolve 400(animalId: 1, veterinarioId: 0) [< 1 ms]
2026-11-04T18:56:10.0159659Z   Passed I077 — registo de outro Veterinário devolve 403 e não é alterado [8 ms]
2026-11-04T18:56:10.0160945Z   Passed UT-TRV-006 — Cada exceção prevista dá o seu código HTTP, com detail igual à mensagem e o formato único de erro(tipo: "KeyNotFoundException", esperado: 404) [82 ms]
2026-11-04T18:56:10.0311356Z   Passed UT-TRV-006 — Cada exceção prevista dá o seu código HTTP, com detail igual à mensagem e o formato único de erro(tipo: "UnprocessableEntityException", esperado: 422) [< 1 ms]
2026-11-04T18:56:10.0352573Z   Passed UT-TRV-006 — Cada exceção prevista dá o seu código HTTP, com detail igual à mensagem e o formato único de erro(tipo: "InvalidOperationException", esperado: 409) [< 1 ms]
2026-11-04T18:56:10.0365290Z   Passed UT-TRV-006 — Cada exceção prevista dá o seu código HTTP, com detail igual à mensagem e o formato único de erro(tipo: "UnauthorizedAccessException", esperado: 401) [< 1 ms]
2026-11-04T18:56:10.0420845Z   Passed UT-TRV-006 — Cada exceção prevista dá o seu código HTTP, com detail igual à mensagem e o formato único de erro(tipo: "ForbiddenException", esperado: 403) [< 1 ms]
2026-11-04T18:56:10.0450762Z   Passed UT-F003-025 — início às 13:00 locais (hora de fecho) devolve 422 [10 ms]
2026-11-04T18:56:10.0470068Z   Passed UT-F004-019 — id de tratamento que pertence a outro registo devolve 400 [9 ms]
2026-11-04T18:56:10.0480962Z   Passed UT-TRV-006 — Cada exceção prevista dá o seu código HTTP, com detail igual à mensagem e o formato único de erro(tipo: "ArgumentException", esperado: 400) [8 ms]
2026-11-04T18:56:10.0666275Z   Passed I082 — EP-044 usa a política de propriedade do Cliente (ClienteAutenticadoControllerBase) [< 1 ms]
2026-11-04T18:56:10.0691001Z   Passed I082 — As rotas são /animais/{id}/vacinacoes (GET, POST), /animais/{id}/vacinacoes/{administracaoId} (PUT) e /me/animais/{id}/vacinacoes (GET) [< 1 ms]
2026-11-04T18:56:10.0720410Z   Passed I082 — EP-044 devolve 200 e usa o Cliente do token, nunca do pedido [7 ms]
2026-11-04T18:56:10.0722446Z   Passed I073 — hora de inverno: 09:00Z de 2026-10-26 são 09:00 em Lisboa (aceite) e 08:30Z são 08:30 (422) [18 ms]
2026-11-04T18:56:10.0758629Z   Passed I082 — EP-043 devolve 201 com Location /animais/{id}/vacinacoes/{administracaoId} e usa o Veterinário do token [4 ms]
2026-11-04T18:56:10.0790493Z   Passed I082 — RF056 devolve 200 com a administração e passa o animal, o registo e o Veterinário do token [3 ms]
2026-11-04T18:56:10.0820336Z   Passed I082 — um token de Veterinário sem a claim veterinarioId dá 403 sem chamar o serviço [< 1 ms]
2026-11-04T18:56:10.0865683Z   Passed I082 — EP-042, EP-043 e RF056 exigem a política Veterinário e nenhuma ação a dispensa [< 1 ms]
2026-11-04T18:56:10.0867996Z   Passed I082 — EP-044 com um token de Cliente sem a claim clienteId dá 403 sem chamar o serviço [< 1 ms]
2026-11-04T18:56:10.0900942Z   Passed UT-TRV-009 — Corpo em falta ou JSON mal formado dão 400 com a mensagem genérica, sem mensagens internas(chave: "") [21 ms]
2026-11-04T18:56:10.0950737Z   Passed UT-TRV-009 — Corpo em falta ou JSON mal formado dão 400 com a mensagem genérica, sem mensagens internas(chave: "$.email") [< 1 ms]
2026-11-04T18:56:10.0990475Z   Passed UT-TRV-008 — Erros de validação em email e password dão 400 com as duas mensagens em português no detail [< 1 ms]
2026-11-04T18:56:10.1020661Z   Passed UT-F001-035 — Sem chave, a validação falha e indica Jwt:Key e os User Secrets [< 1 ms]
2026-11-04T18:56:10.1038082Z   Passed UT-F001-038 — Uma chave que não está em Base64 falha com mensagem sobre Base64 [< 1 ms]
2026-11-04T18:56:10.1062027Z   Passed UT-F001-036 — Uma chave com 31 bytes falha: o mínimo é 32 bytes (256 bits) [< 1 ms]
2026-11-04T18:56:10.1070510Z   Passed UT-F001-037 — Chave com 32 bytes, Issuer, Audience e validade de 8 horas: a configuração é aceite [< 1 ms]
2026-11-04T18:56:10.1090089Z   Passed UT-F001-039 — Uma validade de 0 horas falha [< 1 ms]
2026-11-04T18:56:10.1091812Z   Passed I082 — EP-042 devolve 200 com o boletim e passa o animal e o filtro ao serviço [5 ms]
2026-11-04T18:56:10.1093636Z   Passed I071 — As rotas são GET /veterinarios e GET /veterinarios/{id}/disponibilidade [< 1 ms]
2026-11-04T18:56:10.1095314Z   Passed I071 — EP-021 e EP-022 exigem a política Rececionista e nenhuma ação a dispensa [< 1 ms]
2026-11-04T18:56:10.1097555Z   Passed I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido(email: "joao@mail.") [< 1 ms]
2026-11-04T18:56:10.1099997Z   Passed I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido(email: "joao@@mail.pt") [< 1 ms]
2026-11-04T18:56:10.1102161Z   Passed I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido(email: "joao@mail..pt") [< 1 ms]
2026-11-04T18:56:10.1104596Z   Passed I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido(email: "") [< 1 ms]
2026-11-04T18:56:10.1106538Z   Passed I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido(email: "joao@") [< 1 ms]
2026-11-04T18:56:10.1108715Z   Passed I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido(email: " joao@mail.pt") [< 1 ms]
2026-11-04T18:56:10.1111201Z   Passed I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido(email: "@mail.pt") [< 1 ms]
2026-11-04T18:56:10.1113381Z   Passed I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido(email: "joao@.pt") [< 1 ms]
2026-11-04T18:56:10.1115596Z   Passed I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido(email: "joao@costa@mail.pt") [< 1 ms]
2026-11-04T18:56:10.1117783Z   Passed I065 — Parte local vazia, dois @ ou domínio com partes vazias tornam o e-mail inválido(email: null) [< 1 ms]
2026-11-04T18:56:10.1119712Z   Passed UT-F001-068 — Um e-mail sem @ é inválido [< 1 ms]
2026-11-04T18:56:10.1121008Z   Passed UT-F001-070 — Um e-mail com espaços é inválido [< 1 ms]
2026-11-04T18:56:10.1122472Z   Passed I065 — No pedido, um valor vazio fica para o [Required](email: null) [< 1 ms]
2026-11-04T18:56:10.1124003Z   Passed I065 — No pedido, um valor vazio fica para o [Required](email: "") [< 1 ms]
2026-11-04T18:56:10.1125601Z   Passed I073 — marcação de outro Veterinário à mesma hora não ocupa o horário [15 ms]
2026-11-04T18:56:10.1127139Z   Passed UT-F001-067 — "joao.costa@mail.pt" é um e-mail válido [< 1 ms]
2026-11-04T18:56:10.1128578Z   Passed UT-F001-071 — Um e-mail válido com 256 caracteres é aceite [3 ms]
2026-11-04T18:56:10.1130329Z   Passed UT-F001-069 — Um e-mail sem ponto no domínio é inválido [< 1 ms]
2026-11-04T18:56:10.1131773Z   Passed UT-F001-072 — Um e-mail com 257 caracteres é inválido [< 1 ms]
2026-11-04T18:56:10.1133148Z   Passed I073 — motivo com mais de 250 caracteres devolve 400 [5 ms]
2026-11-04T18:56:10.1135809Z   Passed UT-F006-001, UT-F006-003, UT-F006-004, UT-F006-005, UT-F006-006 e UT-F006-007 — data de administração + intervalo em dias de calendário(dataAdministracao: "2026-10-14", intervaloDias: 90, esperada: "2027-01-12") [5 ms]
2026-11-04T18:56:10.1170027Z   Passed UT-F006-001, UT-F006-003, UT-F006-004, UT-F006-005, UT-F006-006 e UT-F006-007 — data de administração + intervalo em dias de calendário(dataAdministracao: "2026-01-31", intervaloDias: 1, esperada: "2026-02-01") [< 1 ms]
2026-11-04T18:56:10.1173732Z   Passed UT-F006-001, UT-F006-003, UT-F006-004, UT-F006-005, UT-F006-006 e UT-F006-007 — data de administração + intervalo em dias de calendário(dataAdministracao: "2027-03-01", intervaloDias: 365, esperada: "2028-02-29") [< 1 ms]
2026-11-04T18:56:10.1177449Z   Passed UT-F006-001, UT-F006-003, UT-F006-004, UT-F006-005, UT-F006-006 e UT-F006-007 — data de administração + intervalo em dias de calendário(dataAdministracao: "2026-12-31", intervaloDias: 1, esperada: "2027-01-01") [< 1 ms]
2026-11-04T18:56:10.1181397Z   Passed UT-F006-001, UT-F006-003, UT-F006-004, UT-F006-005, UT-F006-006 e UT-F006-007 — data de administração + intervalo em dias de calendário(dataAdministracao: "2028-02-28", intervaloDias: 1, esperada: "2028-02-29") [< 1 ms]
2026-11-04T18:56:10.1184910Z   Passed UT-F006-001, UT-F006-003, UT-F006-004, UT-F006-005, UT-F006-006 e UT-F006-007 — data de administração + intervalo em dias de calendário(dataAdministracao: "2027-02-28", intervaloDias: 1, esperada: "2027-03-01") [< 1 ms]
2026-11-04T18:56:10.1187753Z   Passed I082 — intervalo indicado e não superior a 0 é recusado(intervaloDias: -30) [< 1 ms]
2026-11-04T18:56:10.1189776Z   Passed I082 — intervalo indicado e não superior a 0 é recusado(intervaloDias: 0) [< 1 ms]
2026-11-04T18:56:10.1191419Z   Passed UT-F006-002 — sem intervalo não há próxima administração (null) [< 1 ms]
2026-11-04T18:56:10.1193051Z   Passed UT-F003-021 — data igual a RELOGIO devolve 400: tem de ser estritamente futura [9 ms]
2026-11-04T18:56:10.1460656Z   Passed UT-F003-024 — início às 12:45 locais (acaba às 13:15) devolve 422 [10 ms]
2026-11-04T18:56:10.1519269Z   Passed UT-F003-048 — segunda chegada devolve 422 e mantém a primeira hora [53 ms]
2026-11-04T18:56:10.1520938Z   Passed UT-F003-027 — marcação Confirmada do mesmo Veterinário às 10:00 locais devolve 422 [26 ms]
2026-11-04T18:56:10.1570875Z   Passed UT-F003-049 — If-Match desatualizado devolve 412 e não regista a chegada [8 ms]
2026-11-04T18:56:10.1610542Z   Passed UT-F003-046 e 047 — chegada numa marcação não Confirmada devolve 422(estado: "Pendente") [2 ms]
2026-11-04T18:56:10.1670531Z   Passed UT-F003-046 e 047 — chegada numa marcação não Confirmada devolve 422(estado: "Cancelada") [< 1 ms]
2026-11-04T18:56:10.1680021Z   Passed I075 — EP-028 marcação inexistente devolve 404 [9 ms]
2026-11-04T18:56:10.1710125Z   Passed UT-F003-045 — chegada numa marcação Confirmada grava a hora do servidor [8 ms]
2026-11-04T18:56:10.1770129Z   Passed I074 — PreconditionFailedException é convertida em HTTP 412 [4 ms]
2026-11-04T18:56:10.1820354Z   Passed UT-F001-062 — "501234567" tem o dígito de controlo errado e é inválido [< 1 ms]
2026-11-04T18:56:10.1824215Z   Passed I067 — Sem valor, o atributo aceita: a obrigatoriedade fica para o [Required](valor: "") [< 1 ms]
2026-11-04T18:56:10.1903469Z   Passed I067 — Sem valor, o atributo aceita: a obrigatoriedade fica para o [Required](valor: "   ") [< 1 ms]
2026-11-04T18:56:10.1913899Z   Passed I067 — Sem valor, o atributo aceita: a obrigatoriedade fica para o [Required](valor: null) [< 1 ms]
2026-11-04T18:56:10.1970425Z   Passed UT-F001-063 — Um NIF com 8 algarismos é inválido [< 1 ms]
2026-11-04T18:56:10.2002586Z   Passed UT-F001-064 — Um NIF com 10 algarismos é inválido [< 1 ms]
2026-11-04T18:56:10.2040403Z   Passed I067 — Outros NIF com o dígito de controlo correto são válidos(nif: "999999990") [< 1 ms]
2026-11-04T18:56:10.2067773Z   Passed I067 — Outros NIF com o dígito de controlo correto são válidos(nif: "123456789") [< 1 ms]
2026-11-04T18:56:10.2096675Z   Passed I067 — No pedido, os espaços à volta do NIF são ignorados [< 1 ms]
2026-11-04T18:56:10.2128747Z   Passed UT-F001-061 — "234567899" (resto 2, dígito de controlo 9) é um NIF válido [< 1 ms]
2026-11-04T18:56:10.2144698Z   Passed UT-F001-066 — Um NIF com espaços no meio é inválido: não há normalização [< 1 ms]
2026-11-04T18:56:10.2146125Z   Passed UT-F001-065 — Um NIF com caracteres não numéricos é inválido [< 1 ms]
2026-11-04T18:56:10.2147541Z   Passed UT-F001-060 — "501234560" (resto 1, dígito de controlo 0) é um NIF válido [< 1 ms]
2026-11-04T18:56:10.2149440Z   Passed UT-F003-029 — marcação das 10:00 às 10:30 locais: nova às 10:30 é aceite (só se tocam) [33 ms]
2026-11-04T18:56:10.2151054Z   Passed I076 — EP-030 usa a política de propriedade do Cliente e nenhuma ação a dispensa [3 ms]
2026-11-04T18:56:10.2152791Z   Passed UT-F003-076 — Auxiliar com apoio cria a nota com o autor e a hora do servidor [78 ms]
2026-11-04T18:56:10.2154281Z   Passed UT-F003-084 — marcação inexistente devolve 404 [2 ms]
2026-11-04T18:56:10.2155586Z   Passed UT-F003-020 — data no passado (RELOGIO − 1 min) devolve 400 [8 ms]
2026-11-04T18:56:10.2156946Z   Passed I076 — EP-030 devolve 200 com a lista do serviço e usa o Cliente do token [5 ms]
2026-11-04T18:56:10.2158727Z   Passed UT-F001-074 — Criar um Veterinário sem podeRegistarApoio guarda a autorização a false [235 ms]
2026-11-04T18:56:10.2160362Z   Passed UT-F001-094 — Bloquear uma conta já bloqueada dá 422 [2 ms]
2026-11-04T18:56:10.2161754Z   Passed UT-F003-078, 079 e 081 — texto vazio, só espaços ou com 1001 caracteres devolve 400(texto: "") [10 ms]
2026-11-04T18:56:10.2163924Z   Passed UT-F003-078, 079 e 081 — texto vazio, só espaços ou com 1001 caracteres devolve 400(texto: "   ") [1 ms]
2026-11-04T18:56:10.2165629Z   Passed UT-F003-078, 079 e 081 — texto vazio, só espaços ou com 1001 caracteres devolve 400(texto: null) [< 1 ms]
2026-11-04T18:56:10.2167091Z   Passed UT-F003-083 — marcação Realizada é aceite: só Cancelada impede o registo [5 ms]
2026-11-04T18:56:10.2168863Z   Passed I076 — a única operação é GET /me/consultas (sem criar, alterar, confirmar ou cancelar) [16 ms]
2026-11-04T18:56:10.2171424Z   Passed UT-TRV-001 — Sem pagina nem tamanho, a lista usa a página 1 com 20 elementos [11 ms]
2026-11-04T18:56:10.2172966Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.HorarioVazioSignificaClinicaFechada [5 ms]
2026-11-04T18:56:10.2174753Z   Passed UT-F003-077 — Auxiliar sem autorização de apoio recebe 403 e nada é gravado [9 ms]
2026-11-04T18:56:10.2176148Z   Passed I073 — Veterinário com a conta bloqueada devolve 404, como em EP-021 e EP-022 [26 ms]
2026-11-04T18:56:10.2177569Z   Passed I073 — animalId, veterinarioId e dataHora em falta devolvem 400 com as três mensagens [1 ms]
2026-11-04T18:56:10.2179559Z   Passed UT-F003-080 e 081 — 1000 caracteres é aceite e 1001 devolve 400(comprimento: 1001, aceite: False) [2 ms]
2026-11-04T18:56:10.2181395Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.PeriodosSobrepostosNoMesmoDiaSaoRejeitados [7 ms]
2026-11-04T18:56:10.2183431Z   Passed UT-F003-080 e 081 — 1000 caracteres é aceite e 1001 devolve 400(comprimento: 1000, aceite: True) [4 ms]
2026-11-04T18:56:10.2185181Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.MesmasHorasEmDiasDiferentesNaoSeSobrepoem [4 ms]
2026-11-04T18:56:10.2187528Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.DiaOuHoraInvalidosSaoRejeitados(dia: "", abertura: "09:00", fecho: "13:00") [< 1 ms]
2026-11-04T18:56:10.2314018Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.DiaOuHoraInvalidosSaoRejeitados(dia: "Segunda", abertura: "9h", fecho: "13:00") [< 1 ms]
2026-11-04T18:56:10.2340729Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.DiaOuHoraInvalidosSaoRejeitados(dia: "Segunda", abertura: "09:00", fecho: "25:00") [< 1 ms]
2026-11-04T18:56:10.2343686Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.DiaOuHoraInvalidosSaoRejeitados(dia: "Feriado", abertura: "09:00", fecho: "13:00") [< 1 ms]
2026-11-04T18:56:10.2395014Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.FechoIgualOuAnteriorAAberturaERejeitado(abertura: "13:00", fecho: "09:00") [< 1 ms]
2026-11-04T18:56:10.2417717Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.FechoIgualOuAnteriorAAberturaERejeitado(abertura: "13:00", fecho: "13:00") [< 1 ms]
2026-11-04T18:56:10.2420316Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.PeriodosContiguosSaoAceites [< 1 ms]
2026-11-04T18:56:10.2422362Z   Passed I073 — marcação mais longa que começou antes ocupa o horário (60 min às 09:45 locais) [9 ms]
2026-11-04T18:56:10.2423756Z   Passed I075 — EP-029 não altera a marcação [7 ms]
2026-11-04T18:56:10.2425318Z   Passed ClinicaVeterinaria.UnitTests.Validacao.ValidadorHorarioTests.VariosPeriodosNoMesmoDiaSaoOrdenadosPorDiaEHora [4 ms]
2026-11-04T18:56:10.2427054Z   Passed UT-F003-082 — marcação Cancelada devolve 422 [5 ms]
2026-11-04T18:56:10.2428337Z   Passed I066 — Depois da mudança de perfil, o token antigo deixa de ser aceite (EP-003) [28 ms]
2026-11-04T18:56:10.2429988Z   Passed I072 — EP-024 devolve a marcação no corpo e a versão no cabeçalho ETag [3 ms]
2026-11-04T18:56:10.2431283Z   Passed UT-F001-097 — Bloquear um identificador inexistente dá 404 [4 ms]
2026-11-04T18:56:10.2432733Z   Passed I073 — dataHora sem indicação de fuso é lida como UTC e os segundos fracionários são retirados [15 ms]
2026-11-04T18:56:10.2540410Z   Passed I073 — EP-025 devolve 201 com Location, ETag e o utilizador do token como autor [10 ms]
2026-11-04T18:56:10.2559158Z   Passed I074 — EP-026 passa If-Match ao serviço e devolve 200 com ETag [5 ms]
2026-11-04T18:56:10.2570092Z   Passed I073 — EP-025 é POST /consultas e exige também a política Rececionista [< 1 ms]
2026-11-04T18:56:10.2600262Z   Passed I073 — EP-025 com um token sem identificador de utilizador dá 403 sem chamar o serviço [< 1 ms]
2026-11-04T18:56:10.2630189Z   Passed I072 — EP-023 e EP-024 só aceitam Rececionista, Veterinário e Auxiliar Veterinário [< 1 ms]
2026-11-04T18:56:10.2660369Z   Passed UT-F001-041 — A política Rececionista só aceita o perfil Rececionista(perfil: "Veterinario", aceite: False) [27 ms]
2026-11-04T18:56:10.2674985Z   Passed I074 — EP-027 passa If-Match ao serviço e devolve 204 [7 ms]
2026-11-04T18:56:10.2720214Z   Passed I074 — EP-026 é PUT /consultas/{id} e exige Rececionista [< 1 ms]
2026-11-04T18:56:10.2750256Z   Passed UT-F001-096 — Bloquear uma conta de Cliente dá 422 e a conta mantém-se ativa [27 ms]
2026-11-04T18:56:10.2763440Z   Passed I074 — EP-027 é PATCH /consultas/{id}/estado e exige Rececionista [3 ms]
2026-11-04T18:56:10.2764761Z   Passed I072 — EP-023 passa ao serviço o Veterinário do token, e não o do pedido [1 ms]
2026-11-04T18:56:10.2766331Z   Passed UT-F003-019 — cria a marcação Pendente, com a duração configurada e a Rececionista como autora [29 ms]
2026-11-04T18:56:10.2768158Z   Passed UT-F001-041 — A política Rececionista só aceita o perfil Rececionista(perfil: "Rececionista", aceite: True) [15 ms]
2026-11-04T18:56:10.2770492Z   Passed UT-F001-041 — A política Rececionista só aceita o perfil Rececionista(perfil: "AuxiliarVeterinario", aceite: False) [< 1 ms]
2026-11-04T18:56:10.2772013Z   Passed UT-F003-023 — domingo (clínica fechada) devolve 422 [4 ms]
2026-11-04T18:56:10.2773525Z   Passed UT-F001-041 — A política Rececionista só aceita o perfil Rececionista(perfil: "Cliente", aceite: False) [< 1 ms]
2026-11-04T18:56:10.2775381Z   Passed UT-F001-041 — A política Rececionista só aceita o perfil Rececionista(perfil: "Administrador", aceite: False) [< 1 ms]
2026-11-04T18:56:10.2776868Z   Passed I072 — As rotas são GET /consultas e GET /consultas/{id} [6 ms]
2026-11-04T18:56:10.2778083Z   Passed I072 — perfil sem acesso à agenda dá 403 antes de chamar o serviço [< 1 ms]
2026-11-04T18:56:10.2780259Z   Passed I072 — a Rececionista e o Auxiliar Veterinário não ficam restringidos a um Veterinário(perfil: "AuxiliarVeterinario") [< 1 ms]
2026-11-04T18:56:10.2782218Z   Passed I072 — a Rececionista e o Auxiliar Veterinário não ficam restringidos a um Veterinário(perfil: "Rececionista") [< 1 ms]
2026-11-04T18:56:10.2870783Z   Passed I072 — o Administrador e o Cliente não têm acesso à agenda (403)(perfil: "Cliente") [< 1 ms]
2026-11-04T18:56:10.2872801Z   Passed I072 — o Administrador e o Cliente não têm acesso à agenda (403)(perfil: "Administrador") [< 1 ms]
2026-11-04T18:56:10.2910152Z   Passed I072 — um token de Veterinário sem a claim veterinarioId dá 403 [< 1 ms]
2026-11-04T18:56:10.2930691Z   Passed I072 — o Veterinário fica restringido ao identificador da claim veterinarioId [< 1 ms]
2026-11-04T18:56:10.2939875Z   Passed UT-F003-031 — animal inexistente devolve 404 [4 ms]
2026-11-04T18:56:10.2978192Z   Passed UT-F001-040 — A política Administrador só aceita o perfil Administrador(perfil: "AuxiliarVeterinario", aceite: False) [8 ms]
2026-11-04T18:56:10.3000446Z   Passed UT-F001-040 — A política Administrador só aceita o perfil Administrador(perfil: "Cliente", aceite: False) [< 1 ms]
2026-11-04T18:56:10.3077967Z   Passed UT-F001-040 — A política Administrador só aceita o perfil Administrador(perfil: "Administrador", aceite: True) [< 1 ms]
2026-11-04T18:56:10.3090266Z   Passed UT-F003-026 — início às 12:30 locais (acaba às 13:00) é aceite [6 ms]
2026-11-04T18:56:10.3127754Z   Passed UT-F001-040 — A política Administrador só aceita o perfil Administrador(perfil: "Veterinario", aceite: False) [< 1 ms]
2026-11-04T18:56:10.3160309Z   Passed I072 — NotaPreparacaoDto e as referências ao animal, ao proprietário e ao Veterinário [< 1 ms]
2026-11-04T18:56:10.3191129Z   Passed UT-F001-040 — A política Administrador só aceita o perfil Administrador(perfil: "Rececionista", aceite: False) [< 1 ms]
2026-11-04T18:56:10.3220674Z   Passed I072 — nenhum DTO da agenda tem um campo clínico(dto: typeof(ClinicaVeterinaria.Application.DTOs.Comum.AnimalRefDto)) [< 1 ms]
2026-11-04T18:56:10.3223281Z   Passed UT-F001-045 — A política Funcionario aceita os quatro perfis internos e recusa o Cliente(perfil: "AuxiliarVeterinario", aceite: True) [< 1 ms]
2026-11-04T18:56:10.3270540Z   Passed I072 — nenhum DTO da agenda tem um campo clínico(dto: typeof(ClinicaVeterinaria.Application.DTOs.Comum.ClienteRefDto)) [< 1 ms]
2026-11-04T18:56:10.3272835Z   Passed I072 — nenhum DTO da agenda tem um campo clínico(dto: typeof(ClinicaVeterinaria.Application.DTOs.Comum.VeterinarioRefDto)) [< 1 ms]
2026-11-04T18:56:10.3330867Z   Passed I072 — nenhum DTO da agenda tem um campo clínico(dto: typeof(ClinicaVeterinaria.Application.DTOs.Consultas.ConsultaListItemDto)) [< 1 ms]
2026-11-04T18:56:10.3360609Z   Passed I072 — nenhum DTO da agenda tem um campo clínico(dto: typeof(ClinicaVeterinaria.Application.DTOs.Consultas.ConsultaDto)) [< 1 ms]
2026-11-04T18:56:10.3382398Z   Passed I072 — nenhum DTO da agenda tem um campo clínico(dto: typeof(ClinicaVeterinaria.Application.DTOs.Consultas.NotaPreparacaoDto)) [< 1 ms]
2026-11-04T18:56:10.3420087Z   Passed I072 — ConsultaListItemDto (EP-023): só dados administrativos [< 1 ms]
2026-11-04T18:56:10.3436420Z   Passed I072 — ConsultaDto (EP-024): só dados administrativos e as notas de preparação [< 1 ms]
2026-11-04T18:56:10.3479791Z   Passed UT-F001-045 — A política Funcionario aceita os quatro perfis internos e recusa o Cliente(perfil: "Administrador", aceite: True) [6 ms]
2026-11-04T18:56:10.3510592Z   Passed UT-F001-045 — A política Funcionario aceita os quatro perfis internos e recusa o Cliente(perfil: "Veterinario", aceite: True) [< 1 ms]
2026-11-04T18:56:10.3531357Z   Passed UT-F001-045 — A política Funcionario aceita os quatro perfis internos e recusa o Cliente(perfil: "Rececionista", aceite: True) [< 1 ms]
2026-11-04T18:56:10.3570679Z   Passed UT-F001-045 — A política Funcionario aceita os quatro perfis internos e recusa o Cliente(perfil: "Cliente", aceite: False) [< 1 ms]
2026-11-04T18:56:10.3609188Z   Passed UT-F001-044 — A política Cliente só aceita o perfil Cliente(perfil: "AuxiliarVeterinario", aceite: False) [< 1 ms]
2026-11-04T18:56:10.3633219Z   Passed UT-F001-044 — A política Cliente só aceita o perfil Cliente(perfil: "Rececionista", aceite: False) [4 ms]
2026-11-04T18:56:10.3660431Z   Passed UT-F001-044 — A política Cliente só aceita o perfil Cliente(perfil: "Cliente", aceite: True) [< 1 ms]
2026-11-04T18:56:10.3680899Z   Passed UT-F001-044 — A política Cliente só aceita o perfil Cliente(perfil: "Veterinario", aceite: False) [< 1 ms]
2026-11-04T18:56:10.3720318Z   Passed UT-F001-044 — A política Cliente só aceita o perfil Cliente(perfil: "Administrador", aceite: False) [< 1 ms]
2026-11-04T18:56:10.3750487Z   Passed UT-F001-048 — A política de propriedade recusa os perfis internos mesmo com clienteId(perfil: "Rececionista") [3 ms]
2026-11-04T18:56:10.3800493Z   Passed UT-F001-048 — A política de propriedade recusa os perfis internos mesmo com clienteId(perfil: "AuxiliarVeterinario") [< 1 ms]
2026-11-04T18:56:10.3823176Z   Passed UT-F001-048 — A política de propriedade recusa os perfis internos mesmo com clienteId(perfil: "Veterinario") [5 ms]
2026-11-04T18:56:10.3850653Z   Passed UT-F001-048 — A política de propriedade recusa os perfis internos mesmo com clienteId(perfil: "Administrador") [< 1 ms]
2026-11-04T18:56:10.3890159Z   Passed UT-F001-046 — A política de propriedade aceita um Cliente com a claim clienteId [1 ms]
2026-11-04T18:56:10.3920356Z   Passed UT-F001-042 — A política Veterinario só aceita o perfil Veterinario(perfil: "Cliente", aceite: False) [< 1 ms]
2026-11-04T18:56:10.3950616Z   Passed UT-F001-042 — A política Veterinario só aceita o perfil Veterinario(perfil: "Administrador", aceite: False) [8 ms]
2026-11-04T18:56:10.3981012Z   Passed UT-F001-042 — A política Veterinario só aceita o perfil Veterinario(perfil: "Rececionista", aceite: False) [< 1 ms]
2026-11-04T18:56:10.3992195Z   Passed UT-F001-042 — A política Veterinario só aceita o perfil Veterinario(perfil: "AuxiliarVeterinario", aceite: False) [< 1 ms]
2026-11-04T18:56:10.4031858Z   Passed UT-F001-042 — A política Veterinario só aceita o perfil Veterinario(perfil: "Veterinario", aceite: True) [< 1 ms]
2026-11-04T18:56:10.4040678Z   Passed UT-F001-043 — A política AuxiliarVeterinario só aceita o perfil AuxiliarVeterinario(perfil: "Cliente", aceite: False) [< 1 ms]
2026-11-04T18:56:10.4042276Z   Passed UT-F001-043 — A política AuxiliarVeterinario só aceita o perfil AuxiliarVeterinario(perfil: "Rececionista", aceite: False) [6 ms]
2026-11-04T18:56:10.4043744Z   Passed UT-F001-043 — A política AuxiliarVeterinario só aceita o perfil AuxiliarVeterinario(perfil: "Veterinario", aceite: False) [< 1 ms]
2026-11-04T18:56:10.4045253Z   Passed UT-F001-043 — A política AuxiliarVeterinario só aceita o perfil AuxiliarVeterinario(perfil: "Administrador", aceite: False) [< 1 ms]
2026-11-04T18:56:10.4046732Z   Passed UT-F001-043 — A política AuxiliarVeterinario só aceita o perfil AuxiliarVeterinario(perfil: "AuxiliarVeterinario", aceite: True) [< 1 ms]
2026-11-04T18:56:10.4047957Z   Passed UT-F003-034 — uma marcação Confirmada também pode ser reagendada [55 ms]
2026-11-04T18:56:10.4049308Z   Passed UT-F001-047 — A política de propriedade recusa um Cliente sem a claim clienteId [7 ms]
2026-11-04T18:56:10.4050419Z   Passed UT-F003-039 — reagendar para data passada devolve 400 [18 ms]
2026-11-04T18:56:10.4051447Z   Passed UT-F001-049 — Sem autenticação, a política por omissão recusa o acesso (401) [16 ms]
2026-11-04T18:56:10.4052503Z   Passed I088 (critério 5) — o resumo do histórico (EP-034) só tem os campos do resumo [< 1 ms]
2026-11-04T18:56:10.4053732Z   Passed I088 (critério 5, PA-08) — a leitura não depende do perfil: o serviço não recebe o utilizador em EP-033 nem em EP-034 [< 1 ms]
2026-11-04T18:56:10.4054874Z   Passed I088 (critério 5, PA-08) — o registo de EP-033 tem os campos do contrato v02 [< 1 ms]
2026-11-04T18:56:10.4056064Z   Passed I088 (critério 5) — o resumo do histórico (EP-034) não expõe as notas clínicas nem o detalhe dos tratamentos e prescrições [< 1 ms]
2026-11-04T18:56:10.4057109Z   Passed I065 — Uma palavra-passe forte pode ter espaços [< 1 ms]
2026-11-04T18:56:10.4057946Z   Passed UT-F003-043 — sem If-Match a alteração é aceite [11 ms]
2026-11-04T18:56:10.4059288Z   Passed UT-F001-059 — Sem palavra-passe inicial, o pedido de EP-006 é inválido (obrigatória)(palavraPasse: null) [8 ms]
2026-11-04T18:56:10.4061149Z   Passed UT-F001-059 — Sem palavra-passe inicial, o pedido de EP-006 é inválido (obrigatória)(palavraPasse: "") [< 1 ms]
2026-11-04T18:56:10.4122384Z   Passed UT-F001-055 — Sem letra maiúscula, a palavra-passe é inválida [< 1 ms]
2026-11-04T18:56:10.4150070Z   Passed UT-F001-052 — Uma palavra-passe com 7 caracteres é inválida [< 1 ms]
2026-11-04T18:56:10.4182336Z   Passed UT-F001-051 — Uma palavra-passe com 8 caracteres e as quatro classes é válida [< 1 ms]
2026-11-04T18:56:10.4230589Z   Passed UT-F001-050 — "Temp#2026" (exemplo da API) é uma palavra-passe válida [< 1 ms]
2026-11-04T18:56:10.4266111Z   Passed I065 — O espaço não conta como símbolo [< 1 ms]
2026-11-04T18:56:10.4320224Z   Passed UT-F001-056 — Sem letra minúscula, a palavra-passe é inválida [< 1 ms]
2026-11-04T18:56:10.4321585Z   Passed UT-F001-057 — Sem algarismo, a palavra-passe é inválida [< 1 ms]
2026-11-04T18:56:10.4380166Z   Passed I065 — No atributo, um valor vazio fica para o [Required] [< 1 ms]
2026-11-04T18:56:10.4400557Z   Passed UT-F001-053 — Uma palavra-passe com 128 caracteres e as quatro classes é válida [< 1 ms]
2026-11-04T18:56:10.4410441Z   Passed UT-F001-058 — Sem símbolo, a palavra-passe é inválida [< 1 ms]
2026-11-04T18:56:10.4440423Z   Passed UT-F001-054 — Uma palavra-passe com 129 caracteres é inválida (máximo 128) [< 1 ms]
2026-11-04T18:56:10.4443135Z   Passed UT-TRV-004 — tamanho = 0 ou pagina = 0 são recusados (400)(pagina: 0, tamanho: 20, campo: "Pagina") [5 ms]
2026-11-04T18:56:10.4470047Z   Passed UT-TRV-004 — tamanho = 0 ou pagina = 0 são recusados (400)(pagina: 1, tamanho: 0, campo: "Tamanho") [< 1 ms]
2026-11-04T18:56:10.4500224Z   Passed UT-TRV-003 — tamanho = 101 é recusado (400) [< 1 ms]
2026-11-04T18:56:10.4530211Z   Passed UT-F003-035/036 — Realizada ou Cancelada não podem ser alteradas(estado: "Cancelada") [15 ms]
2026-11-04T18:56:10.4550181Z   Passed UT-F003-035/036 — Realizada ou Cancelada não podem ser alteradas(estado: "Realizada") [5 ms]
2026-11-04T18:56:10.4560141Z   Passed UT-F003-040 — marcação inexistente devolve 404 [7 ms]
2026-11-04T18:56:10.4582888Z   Passed UT-F003-037 — reagendar para horário ocupado devolve 422 [9 ms]
2026-11-04T18:56:10.4610402Z   Passed I074 — EP-027 rejeita Pendente e estado inexistente com 400(estado: "Pendente") [22 ms]
2026-11-04T18:56:10.4640320Z   Passed UT-F003-033 — altera Veterinário e horário de uma marcação Pendente [10 ms]
2026-11-04T18:56:10.4670324Z   Passed I074 — EP-027 rejeita Pendente e estado inexistente com 400(estado: "Faltou") [12 ms]
2026-11-04T18:56:10.4672286Z   Passed UT-F003-038 — a própria marcação não conta como ocupação [15 ms]
2026-11-04T18:56:10.4700070Z   Passed I074 — EP-027 altera Pendente para Confirmada [15 ms]
2026-11-04T18:56:10.4705206Z   Passed UT-F003-041 — If-Match desatualizado devolve 412 e não altera a marcação [4 ms]
2026-11-04T18:56:10.4730110Z   Passed I074 — EP-027 marcação inexistente devolve 404 [1 ms]
2026-11-04T18:56:10.4760410Z   Passed UT-F003-042 — If-Match com a versão atual permite alterar a marcação [14 ms]
2026-11-04T18:56:10.4769342Z   Passed I074 — EP-027 não altera a marcação quando a transição é inválida [15 ms]
2026-11-04T18:56:10.4800187Z   Passed I082 — «hoje» é o dia em Lisboa: às 23:30 UTC de 13/10 (00:30 de 14/10 em Lisboa) aceita 2026-10-14 [163 ms]
2026-11-04T18:56:10.4801509Z   Passed UT-F003-044 — mudar estado com If-Match desatualizado devolve 412 [16 ms]
2026-11-04T18:56:10.4812382Z   Passed UT-F001-024 — O token de um Veterinário tem veterinarioId e não tem clienteId [8 ms]
2026-11-04T18:56:10.4840553Z   Passed UT-F001-032 — Um token de Cliente com o payload de um Administrador e a assinatura original é rejeitado [20 ms]
2026-11-04T18:56:10.4859987Z   Passed UT-F001-029 — Um token emitido há 8 h 01 min é rejeitado por ter expirado [13 ms]
2026-11-04T18:56:10.4950357Z   Passed UT-F001-103 — Um período Segunda 09:00–09:00 devolve 400 e o horário mantém-se [73 ms]
2026-11-04T18:56:10.4952566Z   Passed UT-F001-033 — Um token sem assinatura (alg = none) é rejeitado: só HS256 é aceite [10 ms]
2026-11-04T18:56:10.4954424Z   Passed UT-F001-108 — Horas fora do formato HH:mm devolvem 400(abertura: "9h00", fecho: "13:00") [2 ms]
2026-11-04T18:56:10.4956811Z   Passed UT-F001-028 — Um token emitido agora é aceite pela configuração da API e a claim role é lida com o nome curto [3 ms]
2026-11-04T18:56:10.5040346Z   Passed UT-F001-031 — Um token assinado com outra chave de 256 bits é rejeitado [1 ms]
2026-11-04T18:56:10.5094358Z   Passed UT-F001-025 — O token expira exatamente 8 horas depois, sem frações de segundo, e expiraEm é igual a exp [< 1 ms]
2026-11-04T18:56:10.5150219Z   Passed UT-F001-027 — O cabeçalho do token indica o algoritmo HS256 [< 1 ms]
2026-11-04T18:56:10.5180160Z   Passed UT-F001-023 — O token de um Cliente tem sub, email, role, clienteId e jti, sem veterinarioId [3 ms]
2026-11-04T18:56:10.5247452Z   Passed UT-F001-108 — Horas fora do formato HH:mm devolvem 400(abertura: "09:00", fecho: "25:00") [7 ms]
2026-11-04T18:56:10.5278114Z   Passed UT-F001-030 — Um token emitido há 7 h 59 min ainda é aceite (sem tolerância de relógio) [1 ms]
2026-11-04T18:56:10.5280189Z   Passed UT-F001-111 — Um NIF com o dígito de controlo errado é recusado na validação do pedido (400) [< 1 ms]
2026-11-04T18:56:10.5281748Z   Passed UT-F001-034 — Um token assinado com a chave certa mas com outro emissor é rejeitado [4 ms]
2026-11-04T18:56:10.5283045Z   Passed UT-F001-026 — Dois tokens do mesmo utilizador têm jti diferentes [1 ms]
2026-11-04T18:56:10.5284483Z   Passed I072 — sem versão (null) é um erro de programação [< 1 ms]
2026-11-04T18:56:10.5285589Z   Passed UT-F003-087 — If-Match com versão diferente não corresponde [3 ms]
2026-11-04T18:56:10.5286721Z   Passed I074 — Sem If-Match a operação não fica condicionada à versão [< 1 ms]
2026-11-04T18:56:10.5879142Z   Passed I072 — versões diferentes dão ETags diferentes [< 1 ms]
2026-11-04T18:56:10.5880586Z   Passed UT-F003-089 — If-Match asterisco corresponde a recurso existente [< 1 ms]
2026-11-04T18:56:10.5881794Z   Passed UT-F003-086 — If-Match com a versão atual corresponde [< 1 ms]
2026-11-04T18:56:10.5882834Z   Passed UT-F003-088 — If-Match fraco não corresponde [< 1 ms]
2026-11-04T18:56:10.5883999Z   Passed UT-F003-085 — RowVersion 00 00 00 00 00 00 07 D1 dá o ETag "AAAAAAAAB9E=" [< 1 ms]
2026-11-04T18:56:10.5885169Z   Passed UT-F003-090 — If-Match sem aspas não corresponde [< 1 ms]
2026-11-04T18:56:10.5886487Z   Passed UT-F006-021 — próxima prevista para ontem (2026-10-13) não aparece em proximas [137 ms]
2026-11-04T18:56:10.5888549Z   Passed UT-F006-010 — sem tipo, produto, lote ou data de administração devolve 400 com o campo em falta(campo: "tipo", mensagem: "O tipo é obrigatório.") [17 ms]
2026-11-04T18:56:10.6081684Z   Passed UT-F006-010 — sem tipo, produto, lote ou data de administração devolve 400 com o campo em falta(campo: "dataAdministracao", mensagem: "A data de administração é obrigatória.") [5 ms]
2026-11-04T18:56:10.6116283Z   Passed UT-F001-102 — Um pedido válido substitui os dados, a duração e o horário e devolve o ClinicaDto [88 ms]
2026-11-04T18:56:10.6141854Z   Passed UT-F006-010 — sem tipo, produto, lote ou data de administração devolve 400 com o campo em falta(campo: "produto", mensagem: "O produto é obrigatório.") [7 ms]
2026-11-04T18:56:10.6187743Z   Passed UT-F001-106 — Um período que acaba às 13:00 e outro que começa às 13:00 não se sobrepõem [7 ms]
2026-11-04T18:56:10.6214249Z   Passed UT-F006-010 — sem tipo, produto, lote ou data de administração devolve 400 com o campo em falta(campo: "lote", mensagem: "O lote é obrigatório.") [20 ms]
2026-11-04T18:56:10.6240238Z   Passed UT-F001-107 — As mesmas horas em dias diferentes (Segunda e Terca) são aceites [16 ms]
2026-11-04T18:56:10.6250374Z   Passed UT-F001-110 — Horário vazio: aceite como clínica fechada (divergência registada; a I028 previa 400) [2 ms]
2026-11-04T18:56:10.6270080Z   Passed UT-F006-017 — animal inexistente (id = 999) devolve 404 [6 ms]
2026-11-04T18:56:10.6534646Z   Passed UT-F003-056 — futuras: só marcações Pendentes ou Confirmadas futuras dos animais do Cliente, por data crescente [109 ms]
2026-11-04T18:56:10.6560538Z   Passed I082 — próximas por ordem crescente, incluindo a de hoje [17 ms]
2026-11-04T18:56:10.6590250Z   Passed UT-F001-101 — Um período com DiaSemana = 7 é devolvido como Domingo [39 ms]
2026-11-04T18:56:10.6617841Z   Passed UT-F001-109 — duracaoConsultaMinutos = 0 é recusada na validação do pedido (400) [< 1 ms]
2026-11-04T18:56:10.6620455Z   Passed I067 — Sem a lista do horário (null), o pedido é recusado (400) [< 1 ms]
2026-11-04T18:56:10.6653783Z   Passed UT-F001-104 — Um período Segunda 14:00–13:00 devolve 400 [2 ms]
2026-11-04T18:56:10.6680328Z   Passed UT-F001-105 — Períodos sobrepostos no mesmo dia (09:00–13:00 e 12:00–15:00) devolvem 400 [4 ms]
2026-11-04T18:56:10.6860021Z   Passed UT-F001-100 — A clínica é devolvida com a duração de 30 minutos e os 11 períodos, com o dia 1 como Segunda [5 ms]
2026-11-04T18:56:10.6864789Z   Passed UT-F001-115 — Um e-mail sem @ é recusado no pedido (400) [4 ms]
2026-11-04T18:56:10.6890198Z   Passed UT-F001-114 — Um NIF com o dígito de controlo errado é recusado no pedido (400) [< 1 ms]
2026-11-04T18:56:10.6912638Z   Passed UT-F001-116 — Sem telefone, o pedido é recusado (400) [< 1 ms]
2026-11-04T18:56:10.6920954Z   Passed I089 — a atualização recalcula a próxima nos casos de calendário: fim de mês, fim de ano e 29 de fevereiro(dataAdministracao: "2025-12-31", intervaloDias: 1, esperada: "2026-01-01") [51 ms]
2026-11-04T18:56:10.6949990Z   Passed I076 — Cliente sem marcações recebe listas vazias [73 ms]
2026-11-04T18:56:10.6971688Z   Passed I089 — a atualização recalcula a próxima nos casos de calendário: fim de mês, fim de ano e 29 de fevereiro(dataAdministracao: "2026-01-31", intervaloDias: 1, esperada: "2026-02-01") [10 ms]
2026-11-04T18:56:10.7301466Z   Passed I089 — a atualização recalcula a próxima nos casos de calendário: fim de mês, fim de ano e 29 de fevereiro(dataAdministracao: "2024-02-28", intervaloDias: 1, esperada: "2024-02-29") [6 ms]
2026-11-04T18:56:10.7311306Z   Passed I089 — a atualização recalcula a próxima nos casos de calendário: fim de mês, fim de ano e 29 de fevereiro(dataAdministracao: "2023-03-01", intervaloDias: 365, esperada: "2024-02-29") [6 ms]
2026-11-04T18:56:10.7330166Z   Passed I082 — o boletim não inclui registos de outros animais [7 ms]
2026-11-04T18:56:10.7360355Z   Passed I076 — cada marcação do Cliente aparece numa só das duas vistas [35 ms]
2026-11-04T18:56:10.7371159Z   Passed I076 — MinhasConsultasQuery aceita futuras, historico ou nada(tipo: "futuras") [< 1 ms]
2026-11-04T18:56:10.7400441Z   Passed I076 — MinhasConsultasQuery aceita futuras, historico ou nada(tipo: null) [< 1 ms]
2026-11-04T18:56:10.7402071Z   Passed I076 — MinhasConsultasQuery aceita futuras, historico ou nada(tipo: "historico") [< 1 ms]
2026-11-04T18:56:10.7420649Z   Passed UT-F006-018 — administrações da mais recente para a mais antiga; próximas só com a vacina de 2027-10-14 [9 ms]
2026-11-04T18:56:10.7870420Z   Passed I076 — o tipo não distingue maiúsculas nem espaços à volta(tipo: " Historico ") [10 ms]
2026-11-04T18:56:10.7882775Z   Passed I076 — o tipo não distingue maiúsculas nem espaços à volta(tipo: "HISTORICO") [8 ms]
2026-11-04T18:56:10.7910403Z   Passed I082 — atualizar a data de administração recalcula a próxima (2026-09-01 + 365 = 2027-09-01) [15 ms]
2026-11-04T18:56:10.7930127Z   Passed I082 — atualizar um registo feito por outro Veterinário devolve 403 [8 ms]
2026-11-04T18:56:10.7931325Z   Passed UT-F003-060 — tipo inválido devolve 400(tipo: "todas") [13 ms]
2026-11-04T18:56:10.7960295Z   Passed UT-F003-060 — tipo inválido devolve 400(tipo: "futura") [2 ms]
2026-11-04T18:56:10.7989411Z   Passed UT-F006-011 — data de administração de amanhã (2026-10-15) devolve 400 [11 ms]
2026-11-04T18:56:10.8020075Z   Passed UT-F003-060 — tipo inválido devolve 400(tipo: "passadas") [8 ms]
2026-11-04T18:56:10.8335312Z   Passed UT-F003-059 — sem tipo é igual a tipo = futuras [18 ms]
2026-11-04T18:56:10.8343185Z   Passed UT-F003-057 — fronteira: uma marcação que começa exatamente agora ainda é futura [13 ms]
2026-11-04T18:56:10.8370333Z   Passed UT-F006-019 — tipo filtra as administrações e as próximas(tipo: "Desparasitacao") [45 ms]
2026-11-04T18:56:10.8397989Z   Passed UT-F006-019 — tipo filtra as administrações e as próximas(tipo: "Vacina") [6 ms]
2026-11-04T18:56:10.8420119Z   Passed I082 — EP-042 com animal inexistente devolve 404 [8 ms]
2026-11-04T18:56:10.8430346Z   Passed I082 — tipo fora de Vacina/Desparasitacao devolve 400 [9 ms]
2026-11-04T18:56:10.8437657Z   Passed I065 — A conta criada inicia sessão em EP-001 com a palavra-passe inicial (critério 6) [613 ms]
2026-11-04T18:56:10.9139819Z   Passed I082 — animal sem registos: boletim com as duas listas vazias [12 ms]
2026-11-04T18:56:10.9170400Z   Passed I082 — atualizar o intervalo recalcula a próxima, e retirá-lo deixa-a nula [5 ms]
2026-11-04T18:56:10.9200322Z   Passed UT-F006-013 e UT-F006-015 — intervaloDias = 0 ou negativo devolve 400(intervaloDias: -30) [7 ms]
2026-11-04T18:56:10.9206131Z   Passed UT-F006-013 e UT-F006-015 — intervaloDias = 0 ou negativo devolve 400(intervaloDias: 0) [5 ms]
2026-11-04T18:56:10.9220186Z   Passed I082 — a atualização com dados inválidos devolve 400 e o registo fica como estava [6 ms]
2026-11-04T18:56:10.9250575Z   Passed I089 — dar intervalo a uma desparasitação sem próxima (2026-08-01 + 90) cria a próxima de 2026-10-30, que entra nas próximas do boletim [8 ms]
2026-11-04T18:56:10.9260453Z   Passed UT-F001-119 — Um novo e-mail atualiza o Cliente e o e-mail da conta Utilizador [229 ms]
2026-11-04T18:56:10.9283814Z   Passed I082 — EP-044 com animal inexistente devolve 404 [27 ms]
2026-11-04T18:56:10.9568782Z   Passed UT-F006-023 — o CLIENTE_A pede o boletim do ANIMAL_B1: 403 [8 ms]
2026-11-04T18:56:10.9599991Z   Passed UT-F006-020 — tipo = "Vacinas" devolve 400 [3 ms]
2026-11-04T18:56:10.9601849Z   Passed UT-F001-112 — Criar um Cliente cria a conta Utilizador (Cliente, Ativo) e o Cliente ligado a ela; o e-mail fica só na conta [41 ms]
2026-11-04T18:56:10.9629469Z   Passed UT-F006-024 — ANIMAL_A2 sem registos: historico = [] e proximas = [] [16 ms]
2026-11-04T18:56:10.9660331Z   Passed UT-F001-122 — Um NIF com 5 algarismos é recusado no pedido de atualização (400) [5 ms]
2026-11-04T18:56:10.9690191Z   Passed UT-F001-120 — Atualizar um Cliente com o NIF de outro Cliente devolve 409 [2 ms]
2026-11-04T18:56:10.9691844Z   Passed I076 — animalId próprio devolve só as marcações desse animal [156 ms]
2026-11-04T18:56:10.9905029Z   Passed UT-F006-016 — lote com 51 caracteres devolve 400 (máximo 50) [9 ms]
2026-11-04T18:56:10.9930563Z   Passed I076 — MinhasConsultasQuery recusa tipo desconhecido e animalId inferior a 1 no mesmo pedido [3 ms]
2026-11-04T18:56:10.9960091Z   Passed UT-F001-121 — Manter o próprio NIF não conta como conflito [6 ms]
2026-11-04T18:56:10.9990348Z   Passed I065 — Pode ser criada uma conta com cada um dos quatro perfis internos(perfil: "Veterinario") [141 ms]
2026-11-04T18:56:11.0010089Z   Passed UT-F001-123 — Atualizar um Cliente inexistente devolve 404 [26 ms]
2026-11-04T18:56:11.0020521Z   Passed UT-F003-058 — historico: marcações passadas e Canceladas, mesmo futuras, por data decrescente [28 ms]
2026-11-04T18:56:11.0177741Z   Passed I076 — uma marcação Realizada com data futura vai para o histórico [7 ms]
2026-11-04T18:56:11.0202427Z   Passed UT-F006-022 — o Cliente vê o mesmo histórico e as mesmas próximas do boletim web [40 ms]
2026-11-04T18:56:11.0210389Z   Passed I076 — a resposta só tem dados administrativos: data em UTC, estado, animal e nome do Veterinário [7 ms]
2026-11-04T18:56:11.0230730Z   Passed UT-F006-009 — desparasitação sem intervalo: dataProximaPrevista = null [12 ms]
2026-11-04T18:56:11.0260253Z   Passed UT-F003-061 — animalId de um animal de outro Cliente devolve 403 [9 ms]
2026-11-04T18:56:11.0290807Z   Passed I089 — vacinação sem intervalo e desparasitação com intervalo: a próxima depende só do intervalo, não do tipo(tipo: "Vacina", intervaloDias: null, esperada: null) [9 ms]
2026-11-04T18:56:11.0320178Z   Passed I076 — animalId inexistente também devolve 403, sem revelar se o animal existe [6 ms]
2026-11-04T18:56:11.0322124Z   Passed UT-TRV-002/003 (EP-023) — tamanho entre 1 e 100 e página a partir de 1(pagina: 0, tamanho: 20, valido: False) [1 ms]
2026-11-04T18:56:11.0418454Z   Passed I089 — vacinação sem intervalo e desparasitação com intervalo: a próxima depende só do intervalo, não do tipo(tipo: "Desparasitacao", intervaloDias: 90, esperada: "2027-01-12") [8 ms]
2026-11-04T18:56:11.0430879Z   Passed UT-TRV-002/003 (EP-023) — tamanho entre 1 e 100 e página a partir de 1(pagina: 1, tamanho: 0, valido: False) [8 ms]
2026-11-04T18:56:11.0444602Z   Passed UT-TRV-002/003 (EP-023) — tamanho entre 1 e 100 e página a partir de 1(pagina: 1, tamanho: 101, valido: False) [< 1 ms]
2026-11-04T18:56:11.0470204Z   Passed UT-TRV-002/003 (EP-023) — tamanho entre 1 e 100 e página a partir de 1(pagina: 1, tamanho: 1, valido: True) [1 ms]
2026-11-04T18:56:11.0487560Z   Passed UT-TRV-002/003 (EP-023) — tamanho entre 1 e 100 e página a partir de 1(pagina: 2147483647, tamanho: 20, valido: True) [< 1 ms]
2026-11-04T18:56:11.0520450Z   Passed UT-TRV-002/003 (EP-023) — tamanho entre 1 e 100 e página a partir de 1(pagina: 1, tamanho: 100, valido: True) [< 1 ms]
2026-11-04T18:56:11.0540681Z   Passed I072 — o estado tem de ser um valor de EstadoConsulta, sem distinguir maiúsculas(estado: "Confirmadas", valido: False) [< 1 ms]
2026-11-04T18:56:11.0580763Z   Passed I072 — o estado tem de ser um valor de EstadoConsulta, sem distinguir maiúsculas(estado: "CANCELADA", valido: True) [< 1 ms]
2026-11-04T18:56:11.0640660Z   Passed I072 — o estado tem de ser um valor de EstadoConsulta, sem distinguir maiúsculas(estado: "", valido: True) [< 1 ms]
2026-11-04T18:56:11.0661020Z   Passed I072 — o estado tem de ser um valor de EstadoConsulta, sem distinguir maiúsculas(estado: "Pendente", valido: True) [< 1 ms]
2026-11-04T18:56:11.0663600Z   Passed I072 — o estado tem de ser um valor de EstadoConsulta, sem distinguir maiúsculas(estado: "confirmada", valido: True) [< 1 ms]
2026-11-04T18:56:11.0665925Z   Passed I072 — o estado tem de ser um valor de EstadoConsulta, sem distinguir maiúsculas(estado: " Realizada ", valido: True) [< 1 ms]
2026-11-04T18:56:11.0668159Z   Passed I072 — o estado tem de ser um valor de EstadoConsulta, sem distinguir maiúsculas(estado: "Faturada", valido: False) [< 1 ms]
2026-11-04T18:56:11.0670404Z   Passed UT-F003-052 (validação do pedido) — dataFim anterior a dataInicio é inválida [6 ms]
2026-11-04T18:56:11.0672719Z   Passed I072 — datas fora de 2000-01-01 a 9998-12-31 são inválidas; os limites são aceites(ano: 9999, mes: 1, dia: 1, valida: False) [< 1 ms]
2026-11-04T18:56:11.0674995Z   Passed I072 — datas fora de 2000-01-01 a 9998-12-31 são inválidas; os limites são aceites(ano: 9998, mes: 12, dia: 31, valida: True) [< 1 ms]
2026-11-04T18:56:11.0677295Z   Passed I072 — datas fora de 2000-01-01 a 9998-12-31 são inválidas; os limites são aceites(ano: 1999, mes: 12, dia: 31, valida: False) [< 1 ms]
2026-11-04T18:56:11.0771254Z   Passed I072 — datas fora de 2000-01-01 a 9998-12-31 são inválidas; os limites são aceites(ano: 2000, mes: 1, dia: 1, valida: True) [< 1 ms]
2026-11-04T18:56:11.0780609Z   Passed UT-TRV-001 — sem pagina nem tamanho, pagina = 1 e tamanho = 20 [< 1 ms]
2026-11-04T18:56:11.0828626Z   Passed I072 — veterinarioId e animalId, quando indicados, têm de ser superiores a 0(veterinarioId: null, animalId: 0) [< 1 ms]
2026-11-04T18:56:11.0874098Z   Passed I072 — veterinarioId e animalId, quando indicados, têm de ser superiores a 0(veterinarioId: 0, animalId: null) [< 1 ms]
2026-11-04T18:56:11.0900556Z   Passed I072 — veterinarioId e animalId, quando indicados, têm de ser superiores a 0(veterinarioId: -1, animalId: null) [< 1 ms]
2026-11-04T18:56:11.0930218Z   Passed I072 — todos os filtros são opcionais e um só dia (dataInicio = dataFim) é válido [< 1 ms]
2026-11-04T18:56:11.0951929Z   Passed UT-F001-124 — A pesquisa "Costa" devolve só o CLIENTE_A, com numeroAnimais = 2 [84 ms]
2026-11-04T18:56:11.0980429Z   Passed UT-F006-008 — vacina com intervalo de 365 dias: registo do VET_A e próxima a 2027-10-14 [47 ms]
2026-11-04T18:56:11.1354415Z   Passed UT-F001-117 — O NIF de outro Cliente devolve 409 e nada é gravado [16 ms]
2026-11-04T18:56:11.1380163Z   Passed I082 — produto e lote são guardados sem espaços nos extremos [13 ms]
2026-11-04T18:56:11.1387300Z   Passed UT-F001-118 — Um e-mail já registado devolve 409 e nada é gravado [3 ms]
2026-11-04T18:56:11.1410279Z   Passed UT-F001-113 — Sem NIF, o Cliente é criado (NIF opcional) [7 ms]
2026-11-04T18:56:11.1434378Z   Passed I082 — atualizar um registo de outro animal, ou inexistente, devolve 404 [11 ms]
2026-11-04T18:56:11.1460319Z   Passed I089 — o Veterinário que não fez o registo recebe 403 e o registo fica como estava [24 ms]
2026-11-04T18:56:11.1490233Z   Passed UT-TRV-005 — Com 57 Clientes, a página 3 de 20 tem 17 elementos e total = 57 [35 ms]
2026-11-04T18:56:11.1510182Z   Passed I082 — a atualização substitui produto e lote e mantém o animal e o Veterinário [11 ms]
2026-11-04T18:56:11.1609994Z   Passed UT-F002-025 — Nascido a 2022-05-10, em 2026-10-14 tem 4 anos [97 ms]
2026-11-04T18:56:11.1636472Z   Passed UT-F006-014 — intervaloDias = 1 é aceite e a próxima é 2026-10-15 [7 ms]
2026-11-04T18:56:11.1655256Z   Passed UT-F002-029 — Nascido a 2024-02-29, a 2025-03-01 tem 1 ano [2 ms]
2026-11-04T18:56:11.1680319Z   Passed UT-F006-012 — data de administração de hoje (2026-10-14) é aceite [13 ms]
2026-11-04T18:56:11.1710687Z   Passed I064 — Um intervalo de limpeza entre 1 e 1440 minutos é aceite (60 por omissão)(intervalo: "1440", esperado: 1440) [2 ms]
2026-11-04T18:56:11.1715985Z   Passed I064 — Um intervalo de limpeza entre 1 e 1440 minutos é aceite (60 por omissão)(intervalo: "1", esperado: 1) [< 1 ms]
2026-11-04T18:56:11.1740477Z   Passed I064 — Um intervalo de limpeza entre 1 e 1440 minutos é aceite (60 por omissão)(intervalo: null, esperado: 60) [< 1 ms]
2026-11-04T18:56:11.1761506Z   Passed UT-F001-125 — O ClienteDto tem o e-mail da conta, o estado da conta e os dois animais [33 ms]
2026-11-04T18:56:11.2444688Z   Passed I065 — Pode ser criada uma conta com cada um dos quatro perfis internos(perfil: "Rececionista") [226 ms]
2026-11-04T18:56:11.2470478Z   Passed UT-F002-011 — Data de nascimento de hoje é aceite e a idade é 0 [66 ms]
2026-11-04T18:56:11.2500461Z   Passed UT-F002-010 — Data de nascimento de amanhã (2026-10-15) devolve 400 [4 ms]
2026-11-04T18:56:11.2504081Z   Passed UT-F002-028 — Nascido a 2024-02-29, a 2025-02-28 ainda tem 0 anos [5 ms]
2026-11-04T18:56:11.2510005Z   Passed UT-F002-003 — clienteId = 0 (em falta) é recusado no pedido (400) [4 ms]
2026-11-04T18:56:11.2511746Z   Passed UT-F004-024 — o resumo indica numeroTratamentos = 1 e numeroPrescricoes = 2 [57 ms]
2026-11-04T18:56:11.2803871Z   Passed UT-F002-002 — Sem raça, data de nascimento e peso, o paciente é registado com esses campos vazios [22 ms]
2026-11-04T18:56:11.2811121Z   Passed I078 — HistoricoClinicoQuery: a validação do pedido aceita intervalos válidos e rejeita dataFim anterior a dataInicio(dataInicio: "1999-12-31", dataFim: null, valido: False) [1 ms]
2026-11-04T18:56:11.2841094Z   Passed I078 — HistoricoClinicoQuery: a validação do pedido aceita intervalos válidos e rejeita dataFim anterior a dataInicio(dataInicio: "2026-10-14", dataFim: null, valido: True) [< 1 ms]
2026-11-04T18:56:11.2865255Z   Passed I078 — HistoricoClinicoQuery: a validação do pedido aceita intervalos válidos e rejeita dataFim anterior a dataInicio(dataInicio: null, dataFim: "2026-10-14", valido: True) [< 1 ms]
2026-11-04T18:56:11.2891141Z   Passed I078 — HistoricoClinicoQuery: a validação do pedido aceita intervalos válidos e rejeita dataFim anterior a dataInicio(dataInicio: null, dataFim: null, valido: True) [< 1 ms]
2026-11-04T18:56:11.2964473Z   Passed I078 — HistoricoClinicoQuery: a validação do pedido aceita intervalos válidos e rejeita dataFim anterior a dataInicio(dataInicio: "2026-10-14", dataFim: "2026-10-14", valido: True) [< 1 ms]
2026-11-04T18:56:11.2981219Z   Passed I078 — HistoricoClinicoQuery: a validação do pedido aceita intervalos válidos e rejeita dataFim anterior a dataInicio(dataInicio: "2026-10-14", dataFim: "2026-09-01", valido: False) [< 1 ms]
2026-11-04T18:56:11.2983972Z   Passed I078 — o histórico não inclui registos de outros animais nem consultas sem registo [8 ms]
2026-11-04T18:56:11.3446382Z   Passed I078 — datas fora de 2000-01-01 a 9998-12-31 devolvem 400 [1 ms]
2026-11-04T18:56:11.3470347Z   Passed UT-F002-023 — O CLIENTE_A pede o ANIMAL_B1 e recebe 403 sem dados do animal [56 ms]
2026-11-04T18:56:11.3500882Z   Passed UT-F002-013 — Uma raça de gato (Siamês) num cão devolve 400 com erro na raça [5 ms]
2026-11-04T18:56:11.3560201Z   Passed I078 — EP-033 não altera o registo (só leitura) [52 ms]
2026-11-04T18:56:11.3590160Z   Passed UT-F002-024 — O CLIENTE_A pede um animal inexistente e recebe 404 [5 ms]
2026-11-04T18:56:11.3641674Z   Passed UT-F004-027 — animal inexistente (id = 999) devolve 404 [14 ms]
2026-11-04T18:56:11.3690435Z   Passed UT-F004-023 — sem datas, devolve os dois registos do animal: primeiro o de 2026-10-14, depois o de 2026-09-02 [13 ms]
2026-11-04T18:56:11.3730227Z   Passed UT-F004-026 — dataInicio = 2026-10-14 e dataFim = 2026-09-01 devolve 400 [4 ms]
2026-11-04T18:56:11.4233396Z   Passed UT-F002-020 — Na área do Cliente, o CLIENTE_A vê só os seus animais [44 ms]
2026-11-04T18:56:11.4285650Z   Passed I065 — Pode ser criada uma conta com cada um dos quatro perfis internos(perfil: "AuxiliarVeterinario") [183 ms]
2026-11-04T18:56:11.4350711Z   Passed I072 — a Rececionista filtra por Veterinário e por animal; sem estado vêm todos, incluindo as canceladas [229 ms]
2026-11-04T18:56:11.4380328Z   Passed UT-F002-014 — Atualizar o peso do ANIMAL_A1 grava o novo peso e DataUltimaAlteracao = RELOGIO [50 ms]
2026-11-04T18:56:11.4410221Z   Passed UT-F004-025 — dataInicio = dataFim = 2026-10-14 devolve só o registo de 2026-10-14 [68 ms]
2026-11-04T18:56:11.4470306Z   Passed UT-F002-001 — Registar o Bobi cria o paciente do CLIENTE_A com DataRegisto = RELOGIO e idadeAnos = 4 [6 ms]
2026-11-04T18:56:11.5260741Z   Passed UT-F002-017 — Atualizar para um Cliente inexistente devolve 404 [3 ms]
2026-11-04T18:56:11.5320355Z   Passed I078 — animal sem registos devolve 200 com a lista vazia (IT-EP034-05) [9 ms]
2026-11-04T18:56:11.5370315Z   Passed UT-F002-022 — O CLIENTE_A consulta o ANIMAL_A1 e recebe o MeuAnimalDto [6 ms]
2026-11-04T18:56:11.5391088Z   Passed UT-F002-006 — Um nome com 81 caracteres é recusado (máximo 80) [< 1 ms]
2026-11-04T18:56:11.5430144Z   Passed UT-F002-021 — Um Cliente sem animais recebe uma lista vazia, sem erro [4 ms]
2026-11-04T18:56:11.5460362Z   Passed I072 — o estado não distingue maiúsculas nem espaços nos extremos(estado: " CONFIRMADA ") [112 ms]
2026-11-04T18:56:11.5520357Z   Passed I072 — o estado não distingue maiúsculas nem espaços nos extremos(estado: "confirmada") [7 ms]
2026-11-04T18:56:11.5550287Z   Passed I065 — Pode ser criada uma conta com cada um dos quatro perfis internos(perfil: "Administrador") [135 ms]
2026-11-04T18:56:11.5580138Z   Passed UT-F001-088 — O e-mail de outra conta dá 409 e nada muda [5 ms]
2026-11-04T18:56:11.5610019Z   Passed I078 — só dataInicio ou só dataFim também filtram [114 ms]
2026-11-04T18:56:11.5611367Z   Passed UT-F002-019 — Filtrar por clienteId do CLIENTE_A devolve só o ANIMAL_A1 e o ANIMAL_A2 [104 ms]
2026-11-04T18:56:11.5612832Z   Passed UT-F002-012 — Uma espécie que não existe devolve 400 com erro na espécie [4 ms]
2026-11-04T18:56:11.5667469Z   Passed UT-F002-030 — Sem data de nascimento, idadeAnos fica vazio (divergência registada; a I028 propunha 0) [3 ms]
2026-11-04T18:56:11.5669113Z   Passed UT-F002-008 — Peso 0,01 é aceite [4 ms]
2026-11-04T18:56:11.5670528Z   Passed I078 — a data é o dia da consulta em hora de Lisboa: 2026-10-13T23:30Z conta como 2026-10-14 (R-FUSO) [14 ms]
2026-11-04T18:56:11.5672319Z   Passed UT-F002-015 — Atualizar com peso 0 devolve 400 e nada é alterado [5 ms]
2026-11-04T18:56:11.5710166Z   Passed UT-F002-026 — Nascido a 2022-10-14, faz 4 anos no próprio dia [2 ms]
2026-11-04T18:56:11.5740175Z   Passed UT-F002-016 — Atualizar um animal inexistente devolve 404 [2 ms]
2026-11-04T18:56:11.5765341Z   Passed I078 — EP-033 com id inexistente devolve 404 [11 ms]
2026-11-04T18:56:11.5790265Z   Passed UT-F002-018 — A ficha do ANIMAL_A1 tem só dados administrativos e o telefone do Cliente [5 ms]
2026-11-04T18:56:11.5820086Z   Passed UT-F002-007 — Peso 0 devolve 400 «O peso deve ser superior a 0.» [2 ms]
2026-11-04T18:56:11.5821505Z   Passed UT-F002-009 — Peso negativo devolve 400 [1 ms]
2026-11-04T18:56:11.6339544Z   Passed UT-F004-022 — devolve o RegistoClinicoDto completo (notas, diagnóstico, tratamentos e prescrições) [10 ms]
2026-11-04T18:56:11.6370307Z   Passed UT-F002-027 — Nascido a 2022-10-15, ainda tem 3 anos na véspera do aniversário [5 ms]
2026-11-04T18:56:11.6399944Z   Passed UT-F002-005 — Sem nome, o pedido é recusado (400) [< 1 ms]
2026-11-04T18:56:11.6401906Z   Passed UT-F002-004 — Um Cliente inexistente devolve 404 e nada é gravado [6 ms]
2026-11-04T18:56:11.6430062Z   Passed I078 — inclui os registos de todos os Veterinários do animal (RF035) [9 ms]
2026-11-04T18:56:11.6459948Z   Passed I064 — Uma falha da limpeza fica no log e não pára a API [460 ms]
2026-11-04T18:56:11.6461811Z   Passed UT-F003-051 — a Rececionista filtra por estado e recebe as marcações dos dois Veterinários [106 ms]
2026-11-04T18:56:11.6490066Z   Passed I064 — A limpeza das revogações expiradas corre logo no arranque da API [3 ms]
2026-11-04T18:56:11.7191270Z   Passed I064 — Um intervalo de limpeza fora de 1 a 1440 minutos impede o arranque(intervalo: "1441") [< 1 ms]
2026-11-04T18:56:11.7210385Z   Passed I064 — Um intervalo de limpeza fora de 1 a 1440 minutos impede o arranque(intervalo: "-5") [< 1 ms]
2026-11-04T18:56:11.7240185Z   Passed I064 — Um intervalo de limpeza fora de 1 a 1440 minutos impede o arranque(intervalo: "0") [< 1 ms]
2026-11-04T18:56:11.7269970Z   Passed I064 — Ao parar a API, a rotina de limpeza termina sem erro [4 ms]
2026-11-04T18:56:11.7301660Z   Passed I072 — a agenda é por dia de Lisboa: no dia da mudança de hora conta a meia-noite local [65 ms]
2026-11-04T18:56:11.7303008Z   Passed UT-F003-052 — dataFim anterior a dataInicio devolve 400 [1 ms]
2026-11-04T18:56:11.7360545Z   Passed I072 — estado inválido ou datas fora de 2000-01-01 a 9998-12-31 devolvem 400(estado: null, dataInicio: "1999-12-31", dataFim: null) [1 ms]
2026-11-04T18:56:11.7390491Z   Passed I072 — estado inválido ou datas fora de 2000-01-01 a 9998-12-31 devolvem 400(estado: null, dataInicio: null, dataFim: "9999-12-31") [< 1 ms]
2026-11-04T18:56:11.7392708Z   Passed I072 — estado inválido ou datas fora de 2000-01-01 a 9998-12-31 devolvem 400(estado: "Faturada", dataInicio: null, dataFim: null) [< 1 ms]
2026-11-04T18:56:11.8759405Z   Passed I066 — Depois do bloqueio, o login dá 403 e o token já emitido deixa de ser aceite (critério 4) [326 ms]
2026-11-04T18:56:11.8770186Z   Passed I072 — só dataInicio: dessa data em diante; só dataFim: até ao fim desse dia [166 ms]
2026-11-04T18:56:11.8791662Z   Passed I066 — O Administrador pode mudar o perfil de outro Administrador [2 ms]
2026-11-04T18:56:11.8802888Z   Passed UT-F001-085 — O Administrador não pode remover o próprio perfil de Administrador (422) [2 ms]
2026-11-04T18:56:11.8809360Z   Passed I072 — o Auxiliar Veterinário vê a agenda de todos os Veterinários [7 ms]
2026-11-04T18:56:11.8811217Z   Passed I066 — A autorização de apoio de um Auxiliar pode ser concedida ou retirada(antes: True, depois: False) [9 ms]
2026-11-04T18:56:11.8813125Z   Passed I072 — EP-024 devolve a versão da marcação como ETag (RowVersion em Base64, entre aspas) [5 ms]
2026-11-04T18:56:11.8815254Z   Passed I066 — A autorização de apoio de um Auxiliar pode ser concedida ou retirada(antes: False, depois: True) [1 ms]
2026-11-04T18:56:11.9516246Z   Passed UT-F001-080 — Uma palavra-passe fraca dá 400 com o erro no campo passwordInicial [< 1 ms]
2026-11-04T18:56:11.9518122Z   Passed UT-F001-077 — Um e-mail já registado dá 409 e nada é gravado [1 ms]
2026-11-04T18:56:11.9520279Z   Passed I072 — cada item tem as datas em UTC e as referências ao animal, ao proprietário e ao Veterinário [6 ms]
2026-11-04T18:56:11.9522257Z   Passed I072 — só as notas da própria marcação, da mais antiga para a mais recente [7 ms]
2026-11-04T18:56:11.9524220Z   Passed UT-F003-053 — o Auxiliar sem autorização de apoio lê a marcação com a nota de preparação [5 ms]
2026-11-04T18:56:11.9526121Z   Passed I072 — paginação: ordem por data e hora e depois por id, com o total de todas as páginas [48 ms]
2026-11-04T18:56:11.9527847Z   Passed UT-F003-055 — o Veterinário lê a marcação que lhe está atribuída [5 ms]
2026-11-04T18:56:11.9530058Z   Passed I072 — marcação inexistente devolve 404, também para um Veterinário [5 ms]
2026-11-04T18:56:12.0244191Z   Passed UT-F003-050 — o Veterinário que envia o veterinarioId de outro só recebe as suas marcações [54 ms]
2026-11-04T18:56:12.0246544Z   Passed UT-F001-081 — Um nome com 150 caracteres é aceite (limite de Funcionario.Nome) [131 ms]
2026-11-04T18:56:12.0249321Z   Passed I072 — uma página depois do fim, mesmo a maior possível, vem vazia e com o total [4 ms]
2026-11-04T18:56:12.0251306Z   Passed I065 — Sem filtros, lista todas as contas de funcionário por nome e nunca as de Cliente [5 ms]
2026-11-04T18:56:12.0253237Z   Passed UT-F003-054 — o Veterinário que lê a marcação de outro Veterinário recebe 403 [5 ms]
2026-11-04T18:56:12.0255120Z   Passed UT-F001-095 — O Administrador não pode bloquear a própria conta (422) e mantém-se ativo [6 ms]
2026-11-04T18:56:12.0260921Z   Passed I066 — Um Auxiliar com apoio que muda de perfil sem retirar a autorização dá 422 (critério 3) [2 ms]
2026-11-04T18:56:12.0263078Z   Passed UT-F001-076 — Um Rececionista com podeRegistarApoio é recusado com 422 e nada é gravado [< 1 ms]
2026-11-04T18:56:12.0264924Z   Passed UT-F001-099 — Um perfil fora da enumeração ("Gestor") dá 400 [< 1 ms]
2026-11-04T18:56:12.2037824Z [xUnit.net 00:00:05.08]   Finished:    ClinicaVeterinaria.UnitTests
2026-11-04T18:56:12.9305232Z Data collector 'Blame' message: All tests finished running, Sequence file will not be generated.
2026-11-04T18:56:12.9750143Z   Passed I066 — O e-mail de uma conta de Cliente também dá 409 [2 ms]
2026-11-04T18:56:12.9751979Z   Passed UT-F001-098 — Filtrar por perfil Veterinario e estado Ativo devolve só os Veterinários ativos [20 ms]
2026-11-04T18:56:12.9753943Z   Passed UT-F001-090 — Um identificador inexistente dá 404 [1 ms]
2026-11-04T18:56:12.9755477Z   Passed I066 — Uma conta de Cliente dá 404 em EP-007 (só contas de funcionário) [2 ms]
2026-11-04T18:56:12.9757216Z   Passed I065 — A consulta devolve a conta de funcionário com a data de criação em UTC [1 ms]
2026-11-04T18:56:12.9759208Z   Passed UT-F001-086 — O Administrador altera os próprios dados mantendo o perfil [1 ms]
2026-11-04T18:56:12.9760854Z   Passed UT-F001-075 — O perfil Cliente é recusado com 422 e nada é gravado [1 ms]
2026-11-04T18:56:12.9762925Z   Passed UT-F001-073 — Criar um Auxiliar com podeRegistarApoio cria a conta ativa e o Funcionario [139 ms]
2026-11-04T18:56:12.9765091Z   Passed UT-F001-083 — Atualizar REC_A para Veterinario muda o perfil e os dados e devolve a conta [7 ms]
2026-11-04T18:56:13.1325539Z Results File: /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/resultados-ci/testes-unitarios/testes-unitarios.trx
2026-11-04T18:56:13.1328084Z 
2026-11-04T18:56:13.1374752Z Test Run Failed.
2026-11-04T18:56:13.1375224Z Total tests: 719
2026-11-04T18:56:13.1375602Z      Passed: 717
2026-11-04T18:56:13.1375951Z      Failed: 1
2026-11-04T18:56:13.1376282Z     Skipped: 1
2026-11-04T18:56:13.1377084Z  Total time: 7.5354 Seconds
2026-11-04T18:56:13.3517317Z 
2026-11-04T18:56:13.3523506Z Attachments:
2026-11-04T18:56:13.3526578Z   /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/resultados-ci/testes-unitarios/e5bf2869-0de6-41fc-a383-d43de795dae4/coverage.cobertura.xml
2026-11-04T18:56:13.4048266Z ##[error]Process completed with exit code 1.
2026-11-04T18:56:13.4111983Z ##[group]Run python3 .github/scripts/resumo-testes.py "$RESULTADOS/testes-unitarios" "2 · Testes unitários (QG-02)"
2026-11-04T18:56:13.4112863Z [36;1mpython3 .github/scripts/resumo-testes.py "$RESULTADOS/testes-unitarios" "2 · Testes unitários (QG-02)"[0m
2026-11-04T18:56:13.4134057Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:56:13.4134417Z env:
2026-11-04T18:56:13.4134614Z   DOTNET_NOLOGO: true
2026-11-04T18:56:13.4134860Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:56:13.4135127Z   CONFIGURACAO: Release
2026-11-04T18:56:13.4135393Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:56:13.4135915Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:56:13.4136678Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:56:13.4137245Z   RESULTADOS: resultados-ci
2026-11-04T18:56:13.4137551Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:56:13.4138016Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:56:13.4138308Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:56:13.4138548Z ##[endgroup]
2026-11-04T18:56:13.4603906Z ### 2 · Testes unitários (QG-02)
2026-11-04T18:56:13.4604295Z 
2026-11-04T18:56:13.4604736Z **❌ Há testes falhados** (QG-02: 100% dos testes a passar)
2026-11-04T18:56:13.4605138Z 
2026-11-04T18:56:13.4605598Z | Total | Executados | Aprovados | Falhados | Não executados |
2026-11-04T18:56:13.4606184Z |---:|---:|---:|---:|---:|
2026-11-04T18:56:13.4606559Z | 719 | 718 | 717 | 1 | 0 |
2026-11-04T18:56:13.4606788Z 
2026-11-04T18:56:13.4607010Z Ficheiros TRX: `testes-unitarios.trx`
2026-11-04T18:56:13.4607332Z 
2026-11-04T18:56:13.4607522Z | Teste falhado | Mensagem |
2026-11-04T18:56:13.4607927Z |---|---|
2026-11-04T18:56:13.4608536Z | `I037 - falha provocada (nao fazer merge)` | Falha provocada para a I037. |
2026-11-04T18:56:13.4609326Z 
2026-11-04T18:56:13.4609340Z 
2026-11-04T18:56:13.4731086Z ##[group]Run actions/upload-artifact@cf430e030ddbb5b0abf93d22962f4752f3646cd9
2026-11-04T18:56:13.4731554Z with:
2026-11-04T18:56:13.4731777Z   name: resultados-testes-unitarios
2026-11-04T18:56:13.4732076Z   path: resultados-ci/testes-unitarios
2026-11-04T18:56:13.4732360Z   if-no-files-found: warn
2026-11-04T18:56:13.4732592Z   retention-days: 90
2026-11-04T18:56:13.4732809Z   compression-level: 6
2026-11-04T18:56:13.4733023Z   overwrite: false
2026-11-04T18:56:13.4733243Z   include-hidden-files: false
2026-11-04T18:56:13.4733482Z   archive: true
2026-11-04T18:56:13.4733677Z env:
2026-11-04T18:56:13.4733857Z   DOTNET_NOLOGO: true
2026-11-04T18:56:13.4734090Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:56:13.4734348Z   CONFIGURACAO: Release
2026-11-04T18:56:13.4734600Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:56:13.4735101Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:56:13.4735902Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:56:13.4736461Z   RESULTADOS: resultados-ci
2026-11-04T18:56:13.4736710Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:56:13.4736989Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:56:13.4737279Z   DOTNET_ROOT: /usr/share/dotnet
2026-11-04T18:56:13.4737519Z ##[endgroup]
2026-11-04T18:56:13.6601285Z With the provided path, there will be 3 files uploaded
2026-11-04T18:56:13.6602514Z Artifact name is valid!
2026-11-04T18:56:13.6603129Z Root directory input is valid!
2026-11-04T18:56:14.0726961Z Uploading artifact: resultados-testes-unitarios.zip
2026-11-04T18:56:14.0783908Z Beginning upload of artifact content to blob storage
2026-11-04T18:56:14.7998633Z Uploaded bytes 306856
2026-11-04T18:56:14.8787406Z Finished uploading artifact content to blob storage!
2026-11-04T18:56:14.8789566Z SHA256 digest of uploaded artifact is ad91f0578d9668bffbd1c3d7d8823ff8615570eed8564d30b595f00d383093ba
2026-11-04T18:56:14.8790989Z Finalizing artifact upload
2026-11-04T18:56:15.5690844Z Artifact resultados-testes-unitarios successfully finalized. Artifact ID 13353108465
2026-11-04T18:56:15.5693786Z Artifact resultados-testes-unitarios has been successfully uploaded! Final size is 306856 bytes. Artifact ID is 13353108465
2026-11-04T18:56:15.5697924Z Artifact download URL: https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/actions/runs/41891706460/artifacts/13353108465
2026-11-04T18:56:15.5931980Z Post job cleanup.
2026-11-04T18:56:15.5936082Z ##[start-action display=Reutilizar a cache dos pacotes NuGet;id=__24f879d2-deee-4a41-99b7-bade25a53d46.__actions_cache]
2026-11-04T18:56:15.5939598Z ##[end-action id=__24f879d2-deee-4a41-99b7-bade25a53d46.__actions_cache;outcome=skipped;conclusion=skipped;duration_ms=0]
2026-11-04T18:56:15.5941865Z ##[start-action display=Instalar o SDK do .NET 10;id=__24f879d2-deee-4a41-99b7-bade25a53d46.__actions_setup-dotnet]
2026-11-04T18:56:15.5944128Z ##[end-action id=__24f879d2-deee-4a41-99b7-bade25a53d46.__actions_setup-dotnet;outcome=skipped;conclusion=skipped;duration_ms=0]
2026-11-04T18:56:15.6010435Z Post job cleanup.
2026-11-04T18:56:15.6940250Z [command]/usr/bin/git version
2026-11-04T18:56:15.6980608Z git version 2.56.0
2026-11-04T18:56:15.7056924Z Temporarily overriding HOME='/home/runner/work/_temp/d68609df-3229-43b6-8e86-e0ad811ff17d' before making global git config changes
2026-11-04T18:56:15.7061349Z Adding repository directory to the temporary git global config as a safe directory
2026-11-04T18:56:15.7064516Z [command]/usr/bin/git config --global --add safe.directory /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:56:15.7112408Z Removing SSH command configuration
2026-11-04T18:56:15.7131272Z [command]/usr/bin/git config --local --name-only --get-regexp core\.sshCommand
2026-11-04T18:56:15.7201201Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'core\.sshCommand' && git config --local --unset-all 'core.sshCommand' || :"
2026-11-04T18:56:15.7553981Z Removing HTTP extra header
2026-11-04T18:56:15.7563375Z [command]/usr/bin/git config --local --name-only --get-regexp http\.https\:\/\/github\.com\/\.extraheader
2026-11-04T18:56:15.7641540Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'http\.https\:\/\/github\.com\/\.extraheader' && git config --local --unset-all 'http.https://github.com/.extraheader' || :"
2026-11-04T18:56:15.7954323Z Removing includeIf entries pointing to credentials config files
2026-11-04T18:56:15.7963744Z [command]/usr/bin/git config --local --name-only --get-regexp ^includeIf\.gitdir:
2026-11-04T18:56:15.8039597Z [command]/usr/bin/git submodule foreach --recursive git config --local --show-origin --name-only --get-regexp remote.origin.url
2026-11-04T18:56:15.8556847Z Cleaning up orphan processes
2026-11-04T18:56:15.8887913Z Terminate orphan process: pid (2696) (dotnet)
2026-11-04T18:56:15.8914269Z Terminate orphan process: pid (2697) (dotnet)
2026-11-04T18:56:15.8939162Z Terminate orphan process: pid (2698) (dotnet)
2026-11-04T18:56:15.8969470Z Terminate orphan process: pid (2810) (VBCSCompiler)
``````
