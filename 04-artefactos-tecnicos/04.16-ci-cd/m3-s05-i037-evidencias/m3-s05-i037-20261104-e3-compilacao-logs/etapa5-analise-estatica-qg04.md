``````text
﻿2026-11-04T18:55:21.9722116Z Current runner version: '2.338.2'
2026-11-04T18:55:21.9747907Z ##[group]Runner Image Provisioner
2026-11-04T18:55:21.9748946Z Hosted Compute Agent
2026-11-04T18:55:21.9749606Z Version: 20261021.618
2026-11-04T18:55:21.9750502Z Commit: 21505ad2541d5be6135ed84fc33697146cf75b8e
2026-11-04T18:55:21.9751433Z Build Date: 2026-10-21T00:45:21Z
2026-11-04T18:55:21.9752194Z Worker ID: {5ddef077-2f76-44ba-a38c-530be3f79976}
2026-11-04T18:55:21.9753020Z Region: westus
2026-11-04T18:55:21.9753626Z Cloud: Azure
2026-11-04T18:55:21.9754206Z ##[endgroup]
2026-11-04T18:55:21.9755866Z ##[group]Operating System
2026-11-04T18:55:21.9756558Z Ubuntu
2026-11-04T18:55:21.9757177Z 24.04.5
2026-11-04T18:55:21.9757792Z LTS
2026-11-04T18:55:21.9758388Z ##[endgroup]
2026-11-04T18:55:21.9759047Z ##[group]Runner Image
2026-11-04T18:55:21.9759701Z Image: ubuntu-24.04
2026-11-04T18:55:21.9760689Z Version: 20261025.353.1
2026-11-04T18:55:21.9762147Z Included Software: https://github.com/actions/runner-images/blob/ubuntu24/20261025.353/images/ubuntu/Ubuntu2404-Readme.md
2026-11-04T18:55:21.9763945Z Image Release: https://github.com/actions/runner-images/releases/tag/ubuntu24%2F20261025.353
2026-11-04T18:55:21.9764983Z ##[endgroup]
2026-11-04T18:55:21.9766198Z ##[group]GITHUB_TOKEN Permissions
2026-11-04T18:55:21.9768106Z Contents: read
2026-11-04T18:55:21.9768769Z Metadata: read
2026-11-04T18:55:21.9769543Z ##[endgroup]
2026-11-04T18:55:21.9772121Z Secret source: Actions
2026-11-04T18:55:21.9773183Z Cache mode: write
2026-11-04T18:55:21.9773949Z Prepare workflow directory
2026-11-04T18:55:22.0253720Z Prepare all required actions
2026-11-04T18:55:22.0303362Z Getting action download info
2026-11-04T18:55:22.3826122Z Download action repository 'actions/checkout@3d3c42e5aac5ba805825da76410c181273ba90b1' (SHA:3d3c42e5aac5ba805825da76410c181273ba90b1)
2026-11-04T18:55:22.5240863Z Download action repository 'actions/download-artifact@9000827ccba6bdab643e8b6fd33ac0654aef8333' (SHA:9000827ccba6bdab643e8b6fd33ac0654aef8333)
2026-11-04T18:55:22.9088545Z Download action repository 'actions/upload-artifact@cf430e030ddbb5b0abf93d22962f4752f3646cd9' (SHA:cf430e030ddbb5b0abf93d22962f4752f3646cd9)
2026-11-04T18:55:23.0833475Z Complete job name: 5 · Análise estática (QG-04)
2026-11-04T18:55:23.1755767Z ##[group]Run actions/checkout@3d3c42e5aac5ba805825da76410c181273ba90b1
2026-11-04T18:55:23.1756933Z with:
2026-11-04T18:55:23.1757430Z   persist-credentials: false
2026-11-04T18:55:23.1758114Z   repository: LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:55:23.1762864Z   token: ***
2026-11-04T18:55:23.1763370Z   ssh-strict: true
2026-11-04T18:55:23.1763874Z   ssh-user: git
2026-11-04T18:55:23.1764359Z   clean: true
2026-11-04T18:55:23.1764872Z   sparse-checkout-cone-mode: true
2026-11-04T18:55:23.1765474Z   fetch-depth: 1
2026-11-04T18:55:23.1765968Z   fetch-tags: false
2026-11-04T18:55:23.1766473Z   show-progress: true
2026-11-04T18:55:23.1767026Z   lfs: false
2026-11-04T18:55:23.1767511Z   submodules: false
2026-11-04T18:55:23.1768023Z   set-safe-directory: true
2026-11-04T18:55:23.1768630Z   allow-unsafe-pr-checkout: false
2026-11-04T18:55:23.1769505Z env:
2026-11-04T18:55:23.1770158Z   DOTNET_NOLOGO: true
2026-11-04T18:55:23.1770764Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:23.1771383Z   CONFIGURACAO: Release
2026-11-04T18:55:23.1771958Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:23.1773015Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:23.1774563Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:23.1775723Z   RESULTADOS: resultados-ci
2026-11-04T18:55:23.1776304Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:23.1776932Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:23.1777562Z ##[endgroup]
2026-11-04T18:55:23.4982705Z Syncing repository: LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:55:23.4984894Z ##[group]Getting Git version info
2026-11-04T18:55:23.4986340Z Working directory is '/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO'
2026-11-04T18:55:23.4987735Z [command]/usr/bin/git version
2026-11-04T18:55:23.5090482Z git version 2.56.0
2026-11-04T18:55:23.5120475Z ##[endgroup]
2026-11-04T18:55:23.5128086Z Temporarily overriding HOME='/home/runner/work/_temp/645c1373-3060-411e-b7c0-66f83eb403c9' before making global git config changes
2026-11-04T18:55:23.5131607Z Adding repository directory to the temporary git global config as a safe directory
2026-11-04T18:55:23.5134986Z [command]/usr/bin/git config --global --add safe.directory /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:55:23.5200296Z Deleting the contents of '/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO'
2026-11-04T18:55:23.5204997Z ##[group]Determining repository object format
2026-11-04T18:55:23.5207185Z ##[endgroup]
2026-11-04T18:55:23.5208882Z ##[group]Initializing the repository
2026-11-04T18:55:23.5213218Z [command]/usr/bin/git init /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:55:23.5341453Z hint: Using 'master' as the name for the initial branch. This default branch name
2026-11-04T18:55:23.5343529Z hint: will change to "main" in Git 3.0. To configure the initial branch name
2026-11-04T18:55:23.5345059Z hint: to use in all of your new repositories, which will suppress this warning,
2026-11-04T18:55:23.5346136Z hint: call:
2026-11-04T18:55:23.5346781Z hint:
2026-11-04T18:55:23.5347541Z hint: 	git config --global init.defaultBranch <name>
2026-11-04T18:55:23.5348421Z hint:
2026-11-04T18:55:23.5349255Z hint: Names commonly chosen instead of 'master' are 'main', 'trunk' and
2026-11-04T18:55:23.5350898Z hint: 'development'. The just-created branch can be renamed via this command:
2026-11-04T18:55:23.5351816Z hint:
2026-11-04T18:55:23.5352341Z hint: 	git branch -m <name>
2026-11-04T18:55:23.5352937Z hint:
2026-11-04T18:55:23.5353704Z hint: Disable this message with "git config set advice.defaultBranchName false"
2026-11-04T18:55:23.5371012Z Initialized empty Git repository in /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/
2026-11-04T18:55:23.5395034Z [command]/usr/bin/git remote add origin https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:55:23.5445111Z ##[endgroup]
2026-11-04T18:55:23.5447108Z ##[group]Disabling automatic garbage collection
2026-11-04T18:55:23.5464558Z [command]/usr/bin/git config --local gc.auto 0
2026-11-04T18:55:23.5504708Z ##[endgroup]
2026-11-04T18:55:23.5506095Z ##[group]Setting up auth
2026-11-04T18:55:23.5507067Z Removing SSH command configuration
2026-11-04T18:55:23.5507958Z [command]/usr/bin/git config --local --name-only --get-regexp core\.sshCommand
2026-11-04T18:55:23.5551708Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'core\.sshCommand' && git config --local --unset-all 'core.sshCommand' || :"
2026-11-04T18:55:23.5976758Z Removing HTTP extra header
2026-11-04T18:55:23.5983219Z [command]/usr/bin/git config --local --name-only --get-regexp http\.https\:\/\/github\.com\/\.extraheader
2026-11-04T18:55:23.6029724Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'http\.https\:\/\/github\.com\/\.extraheader' && git config --local --unset-all 'http.https://github.com/.extraheader' || :"
2026-11-04T18:55:23.6275446Z Removing includeIf entries pointing to credentials config files
2026-11-04T18:55:23.6281780Z [command]/usr/bin/git config --local --name-only --get-regexp ^includeIf\.gitdir:
2026-11-04T18:55:23.6327700Z [command]/usr/bin/git submodule foreach --recursive git config --local --show-origin --name-only --get-regexp remote.origin.url
2026-11-04T18:55:23.6580839Z [command]/usr/bin/git config --file /home/runner/work/_temp/git-credentials-6448d772-71a9-4805-9a5e-67fbc02cca29.config http.https://github.com/.extraheader AUTHORIZATION: basic ***
2026-11-04T18:55:23.6631975Z [command]/usr/bin/git config --local includeIf.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path /home/runner/work/_temp/git-credentials-6448d772-71a9-4805-9a5e-67fbc02cca29.config
2026-11-04T18:55:23.6679048Z [command]/usr/bin/git config --local includeIf.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path /home/runner/work/_temp/git-credentials-6448d772-71a9-4805-9a5e-67fbc02cca29.config
2026-11-04T18:55:23.6726225Z [command]/usr/bin/git config --local includeIf.gitdir:/github/workspace/.git.path /github/runner_temp/git-credentials-6448d772-71a9-4805-9a5e-67fbc02cca29.config
2026-11-04T18:55:23.6769356Z [command]/usr/bin/git config --local includeIf.gitdir:/github/workspace/.git/worktrees/*.path /github/runner_temp/git-credentials-6448d772-71a9-4805-9a5e-67fbc02cca29.config
2026-11-04T18:55:23.6810847Z ##[endgroup]
2026-11-04T18:55:23.6812286Z ##[group]Fetching the repository
2026-11-04T18:55:23.6817589Z [command]/usr/bin/git -c protocol.version=2 fetch --no-tags --prune --no-recurse-submodules --depth=1 origin +4ca137d61a701c8b2dabecaf16d47e4f997de66a:refs/remotes/pull/248/merge
2026-11-04T18:55:25.2656732Z From https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:55:25.2662658Z  * [new ref]         4ca137d61a701c8b2dabecaf16d47e4f997de66a -> pull/248/merge
2026-11-04T18:55:25.2667690Z ##[endgroup]
2026-11-04T18:55:25.2669671Z ##[group]Determining the checkout info
2026-11-04T18:55:25.2672075Z ##[endgroup]
2026-11-04T18:55:25.2674990Z [command]/usr/bin/git sparse-checkout disable
2026-11-04T18:55:25.2744865Z [command]/usr/bin/git config --local --unset-all extensions.worktreeConfig
2026-11-04T18:55:25.2787649Z ##[group]Checking out the ref
2026-11-04T18:55:25.2791826Z [command]/usr/bin/git checkout --progress --force refs/remotes/pull/248/merge
2026-11-04T18:55:25.4851201Z Note: switching to 'refs/remotes/pull/248/merge'.
2026-11-04T18:55:25.4851829Z 
2026-11-04T18:55:25.4852607Z You are in 'detached HEAD' state. You can look around, make experimental
2026-11-04T18:55:25.4853810Z changes and commit them, and you can discard any commits you make in this
2026-11-04T18:55:25.4855164Z state without impacting any branches by switching back to a branch.
2026-11-04T18:55:25.4855787Z 
2026-11-04T18:55:25.4856282Z If you want to create a new branch to retain commits you create, you may
2026-11-04T18:55:25.4861201Z do so (now or later) by using -c with the switch command. Example:
2026-11-04T18:55:25.4861792Z 
2026-11-04T18:55:25.4862041Z   git switch -c <new-branch-name>
2026-11-04T18:55:25.4862422Z 
2026-11-04T18:55:25.4862669Z Or undo this operation with:
2026-11-04T18:55:25.4863030Z 
2026-11-04T18:55:25.4863240Z   git switch -
2026-11-04T18:55:25.4863590Z 
2026-11-04T18:55:25.4864068Z Turn off this advice by setting config variable advice.detachedHead to false
2026-11-04T18:55:25.4864775Z 
2026-11-04T18:55:25.4865491Z HEAD is now at 4ca137d Merge 79f1c47a6bcb41647a4c04ad978f43f0f6899865 into 89d6b31c5e788b4e8e5cfc7178d991ef65856c06
2026-11-04T18:55:25.4885612Z ##[endgroup]
2026-11-04T18:55:25.4945619Z [command]/usr/bin/git log -1 --format=%H
2026-11-04T18:55:25.4982764Z 4ca137d61a701c8b2dabecaf16d47e4f997de66a
2026-11-04T18:55:25.4992681Z ##[group]Removing auth
2026-11-04T18:55:25.4996577Z Removing SSH command configuration
2026-11-04T18:55:25.4997705Z [command]/usr/bin/git config --local --name-only --get-regexp core\.sshCommand
2026-11-04T18:55:25.5043173Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'core\.sshCommand' && git config --local --unset-all 'core.sshCommand' || :"
2026-11-04T18:55:25.5383640Z Removing HTTP extra header
2026-11-04T18:55:25.5385048Z [command]/usr/bin/git config --local --name-only --get-regexp http\.https\:\/\/github\.com\/\.extraheader
2026-11-04T18:55:25.5388041Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'http\.https\:\/\/github\.com\/\.extraheader' && git config --local --unset-all 'http.https://github.com/.extraheader' || :"
2026-11-04T18:55:25.5605536Z Removing includeIf entries pointing to credentials config files
2026-11-04T18:55:25.5611164Z [command]/usr/bin/git config --local --name-only --get-regexp ^includeIf\.gitdir:
2026-11-04T18:55:25.5654984Z includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path
2026-11-04T18:55:25.5656318Z includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path
2026-11-04T18:55:25.5657331Z includeif.gitdir:/github/workspace/.git.path
2026-11-04T18:55:25.5658021Z includeif.gitdir:/github/workspace/.git/worktrees/*.path
2026-11-04T18:55:25.5665530Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path
2026-11-04T18:55:25.5701351Z /home/runner/work/_temp/git-credentials-6448d772-71a9-4805-9a5e-67fbc02cca29.config
2026-11-04T18:55:25.5718930Z [command]/usr/bin/git config --local --unset includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git.path \/home\/runner\/work\/_temp\/git\-credentials\-6448d772\-71a9\-4805\-9a5e\-67fbc02cca29\.config
2026-11-04T18:55:25.5775199Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path
2026-11-04T18:55:25.5810639Z /home/runner/work/_temp/git-credentials-6448d772-71a9-4805-9a5e-67fbc02cca29.config
2026-11-04T18:55:25.5819510Z [command]/usr/bin/git config --local --unset includeif.gitdir:/home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/.git/worktrees/*.path \/home\/runner\/work\/_temp\/git\-credentials\-6448d772\-71a9\-4805\-9a5e\-67fbc02cca29\.config
2026-11-04T18:55:25.5867044Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/github/workspace/.git.path
2026-11-04T18:55:25.5901907Z /github/runner_temp/git-credentials-6448d772-71a9-4805-9a5e-67fbc02cca29.config
2026-11-04T18:55:25.5910491Z [command]/usr/bin/git config --local --unset includeif.gitdir:/github/workspace/.git.path \/github\/runner_temp\/git\-credentials\-6448d772\-71a9\-4805\-9a5e\-67fbc02cca29\.config
2026-11-04T18:55:25.5956847Z [command]/usr/bin/git config --local --get-all includeif.gitdir:/github/workspace/.git/worktrees/*.path
2026-11-04T18:55:25.5991720Z /github/runner_temp/git-credentials-6448d772-71a9-4805-9a5e-67fbc02cca29.config
2026-11-04T18:55:25.6002263Z [command]/usr/bin/git config --local --unset includeif.gitdir:/github/workspace/.git/worktrees/*.path \/github\/runner_temp\/git\-credentials\-6448d772\-71a9\-4805\-9a5e\-67fbc02cca29\.config
2026-11-04T18:55:25.6047597Z [command]/usr/bin/git submodule foreach --recursive git config --local --show-origin --name-only --get-regexp remote.origin.url
2026-11-04T18:55:25.6292518Z Removing credentials config '/home/runner/work/_temp/git-credentials-6448d772-71a9-4805-9a5e-67fbc02cca29.config'
2026-11-04T18:55:25.6303908Z ##[endgroup]
2026-11-04T18:55:25.6591724Z ##[group]Run actions/download-artifact@9000827ccba6bdab643e8b6fd33ac0654aef8333
2026-11-04T18:55:25.6592323Z with:
2026-11-04T18:55:25.6592647Z   name: relatorio-analise-estatica-sarif
2026-11-04T18:55:25.6593055Z   path: resultados-ci/analise-estatica
2026-11-04T18:55:25.6593450Z   merge-multiple: false
2026-11-04T18:55:25.6593855Z   repository: LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:55:25.6594296Z   run-id: 41891749860
2026-11-04T18:55:25.6594605Z   skip-decompress: false
2026-11-04T18:55:25.6594928Z   digest-mismatch: error
2026-11-04T18:55:25.6595233Z env:
2026-11-04T18:55:25.6595509Z   DOTNET_NOLOGO: true
2026-11-04T18:55:25.6595828Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:25.6596257Z   CONFIGURACAO: Release
2026-11-04T18:55:25.6596638Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:25.6597269Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:25.6598301Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:25.6599198Z   RESULTADOS: resultados-ci
2026-11-04T18:55:25.6599551Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:25.6600258Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:25.6600690Z ##[endgroup]
2026-11-04T18:55:25.8094694Z Downloading single artifact
2026-11-04T18:55:26.0860251Z Preparing to download the following artifacts:
2026-11-04T18:55:26.0862118Z - relatorio-analise-estatica-sarif (ID: 13354041682, Size: 39159, Expected Digest: sha256:51b05167e361b68bd02ef380f30e03e4cfa7646b689b3b152255201d9c7538fd)
2026-11-04T18:55:26.6585829Z Redirecting to blob download url: https://productionresultssa0.blob.core.windows.net/actions-results/66a418ae-154a-81cb-8dc3-674d08989881/workflow-job-run-1bc3da31-4f36-572e-a428-99c9520d1653/artifacts/a2fef74b5eea335cb0d8e66dd7870574cfd12de5697ed4a0fe226e531632d9c0.zip
2026-11-04T18:55:26.6588332Z Starting download of artifact to: /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO/resultados-ci/analise-estatica
2026-11-04T18:55:26.9693886Z (node:2416) [DEP0005] DeprecationWarning: Buffer() is deprecated due to security and usability issues. Please use the Buffer.alloc(), Buffer.allocUnsafe(), or Buffer.from() methods instead.
2026-11-04T18:55:26.9696156Z (Use `node --trace-deprecation ...` to show where the warning was created)
2026-11-04T18:55:27.0414990Z SHA256 digest of downloaded artifact is 51b05167e361b68bd02ef380f30e03e4cfa7646b689b3b152255201d9c7538fd
2026-11-04T18:55:27.0415976Z Artifact download completed successfully.
2026-11-04T18:55:27.0416461Z Total of 1 artifact(s) downloaded
2026-11-04T18:55:27.0419314Z Download artifact has finished successfully
2026-11-04T18:55:27.0575148Z ##[group]Run python3 .github/scripts/gate-analise-estatica.py \
2026-11-04T18:55:27.0575800Z [36;1mpython3 .github/scripts/gate-analise-estatica.py \[0m
2026-11-04T18:55:27.0576221Z [36;1m  "$QUALITY_GATES" \[0m
2026-11-04T18:55:27.0576532Z [36;1m  "$RESULTADOS/analise-estatica" \[0m
2026-11-04T18:55:27.0577002Z [36;1m  "$RESULTADOS/analise-estatica/relatorio-analise-estatica.md"[0m
2026-11-04T18:55:27.0602634Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:55:27.0603051Z env:
2026-11-04T18:55:27.0603280Z   DOTNET_NOLOGO: true
2026-11-04T18:55:27.0603561Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:27.0603879Z   CONFIGURACAO: Release
2026-11-04T18:55:27.0604198Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:27.0604831Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:27.0605849Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:27.0606547Z   RESULTADOS: resultados-ci
2026-11-04T18:55:27.0606850Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:27.0607189Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:27.0607524Z ##[endgroup]
2026-11-04T18:55:27.0945720Z ### QG-04 · Análise estática (Roslyn)
2026-11-04T18:55:27.0946506Z 
2026-11-04T18:55:27.0946768Z | Medida | Valor |
2026-11-04T18:55:27.0947383Z |---|---|
2026-11-04T18:55:27.0948136Z | Relatórios SARIF (um por projeto) | 1 |
2026-11-04T18:55:27.0949304Z | Ocorrências por nível (não suprimidas) | error: 1 |
2026-11-04T18:55:27.0950717Z | Ocorrências suprimidas no código | 0 |
2026-11-04T18:55:27.0951445Z | **Ocorrências que bloqueiam** (error, warning) | **1** |
2026-11-04T18:55:27.0952264Z | Máximo permitido (`.github/quality-gates.json`) | 0 |
2026-11-04T18:55:27.0952708Z 
2026-11-04T18:55:27.0952937Z | Regra | Nível | Ocorrências |
2026-11-04T18:55:27.0953411Z |---|---|---|
2026-11-04T18:55:27.0953796Z | `CS0029` | error | 1 |
2026-11-04T18:55:27.0954057Z 
2026-11-04T18:55:27.0954476Z <details><summary>Ocorrências que bloqueiam (até 50)</summary>
2026-11-04T18:55:27.0954979Z 
2026-11-04T18:55:27.0955171Z | Projeto | Regra | Local | Mensagem |
2026-11-04T18:55:27.0955650Z |---|---|---|---|
2026-11-04T18:55:27.0956865Z | ClinicaVeterinaria.Domain | `CS0029` | `src/backend/ClinicaVeterinaria.Domain/FalhaProvocadaI037.cs:5` | Cannot implicitly convert type 'string' to 'int' |
2026-11-04T18:55:27.0958174Z 
2026-11-04T18:55:27.0958300Z </details>
2026-11-04T18:55:27.0958474Z 
2026-11-04T18:55:27.0959063Z ❌ **QG-04 não cumprido:** 1 ocorrência(s) com nível que bloqueia (máximo 0). O merge fica bloqueado.
2026-11-04T18:55:27.0959690Z 
2026-11-04T18:55:27.0980712Z ##[error]src/backend/ClinicaVeterinaria.Domain/FalhaProvocadaI037.cs:5 — Cannot implicitly convert type 'string' to 'int'
2026-11-04T18:55:27.0994498Z ##[error]Process completed with exit code 1.
2026-11-04T18:55:27.1093985Z ##[group]Run actions/upload-artifact@cf430e030ddbb5b0abf93d22962f4752f3646cd9
2026-11-04T18:55:27.1094427Z with:
2026-11-04T18:55:27.1094647Z   name: relatorio-analise-estatica
2026-11-04T18:55:27.1094943Z   path: resultados-ci/analise-estatica
2026-11-04T18:55:27.1095237Z   if-no-files-found: warn
2026-11-04T18:55:27.1095490Z   retention-days: 90
2026-11-04T18:55:27.1095736Z   compression-level: 6
2026-11-04T18:55:27.1095969Z   overwrite: false
2026-11-04T18:55:27.1096194Z   include-hidden-files: false
2026-11-04T18:55:27.1096448Z   archive: true
2026-11-04T18:55:27.1096659Z env:
2026-11-04T18:55:27.1096854Z   DOTNET_NOLOGO: true
2026-11-04T18:55:27.1097097Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:55:27.1097375Z   CONFIGURACAO: Release
2026-11-04T18:55:27.1097648Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:55:27.1098193Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:55:27.1099060Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:55:27.1099666Z   RESULTADOS: resultados-ci
2026-11-04T18:55:27.1100228Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:55:27.1100564Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:55:27.1100861Z ##[endgroup]
2026-11-04T18:55:27.2682172Z With the provided path, there will be 2 files uploaded
2026-11-04T18:55:27.2683744Z Artifact name is valid!
2026-11-04T18:55:27.2684624Z Root directory input is valid!
2026-11-04T18:55:27.6824215Z Uploading artifact: relatorio-analise-estatica.zip
2026-11-04T18:55:27.6875787Z Beginning upload of artifact content to blob storage
2026-11-04T18:55:28.1173572Z Uploaded bytes 39801
2026-11-04T18:55:28.1987038Z Finished uploading artifact content to blob storage!
2026-11-04T18:55:28.1988526Z SHA256 digest of uploaded artifact is 3c726d09e00b6cd6975a42bb0393b836e559e43b07c7ecf4f341fe41b29a67b2
2026-11-04T18:55:28.1990281Z Finalizing artifact upload
2026-11-04T18:55:28.9572908Z Artifact relatorio-analise-estatica successfully finalized. Artifact ID 13355150114
2026-11-04T18:55:28.9574701Z Artifact relatorio-analise-estatica has been successfully uploaded! Final size is 39801 bytes. Artifact ID is 13355150114
2026-11-04T18:55:28.9580384Z Artifact download URL: https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/actions/runs/41891749860/artifacts/13355150114
2026-11-04T18:55:28.9795416Z Post job cleanup.
2026-11-04T18:55:29.0723901Z [command]/usr/bin/git version
2026-11-04T18:55:29.0774079Z git version 2.56.0
2026-11-04T18:55:29.0847399Z Temporarily overriding HOME='/home/runner/work/_temp/1a68424f-9564-4c5c-89cd-7ef11d5eee18' before making global git config changes
2026-11-04T18:55:29.0849022Z Adding repository directory to the temporary git global config as a safe directory
2026-11-04T18:55:29.0854542Z [command]/usr/bin/git config --global --add safe.directory /home/runner/work/LEI-Grupo-de-LDS-UTPORTO/LEI-Grupo-de-LDS-UTPORTO
2026-11-04T18:55:29.0897382Z Removing SSH command configuration
2026-11-04T18:55:29.0906973Z [command]/usr/bin/git config --local --name-only --get-regexp core\.sshCommand
2026-11-04T18:55:29.0952590Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'core\.sshCommand' && git config --local --unset-all 'core.sshCommand' || :"
2026-11-04T18:55:29.1330960Z Removing HTTP extra header
2026-11-04T18:55:29.1332311Z [command]/usr/bin/git config --local --name-only --get-regexp http\.https\:\/\/github\.com\/\.extraheader
2026-11-04T18:55:29.1399612Z [command]/usr/bin/git submodule foreach --recursive sh -c "git config --local --name-only --get-regexp 'http\.https\:\/\/github\.com\/\.extraheader' && git config --local --unset-all 'http.https://github.com/.extraheader' || :"
2026-11-04T18:55:29.1740665Z Removing includeIf entries pointing to credentials config files
2026-11-04T18:55:29.1750196Z [command]/usr/bin/git config --local --name-only --get-regexp ^includeIf\.gitdir:
2026-11-04T18:55:29.1798341Z [command]/usr/bin/git submodule foreach --recursive git config --local --show-origin --name-only --get-regexp remote.origin.url
2026-11-04T18:55:29.2206289Z Cleaning up orphan processes
``````
