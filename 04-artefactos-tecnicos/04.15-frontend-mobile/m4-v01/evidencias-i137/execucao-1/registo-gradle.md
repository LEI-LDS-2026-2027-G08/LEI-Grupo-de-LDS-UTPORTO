# I137 — Execução 1 — correções de portabilidade

Data: 25/11/2026. Execução: `37a6c5d4af4a`. Ambiente macOS, API e SQL reais isolados. Sem `ANDROID_HOME` ou `ANDROID_SDK_ROOT` definidos no terminal.

```text
> Task :app:preBuild UP-TO-DATE
> Task :app:preDebugBuild UP-TO-DATE
> Task :app:generateDebugBuildConfig
> Task :app:generateDebugResources
> Task :app:packageDebugResources
> Task :app:processDebugNavigationResources
> Task :app:javaPreCompileDebug
> Task :app:mapDebugSourceSetPaths
> Task :app:compileDebugNavigationResources
> Task :app:checkDebugAarMetadata
> Task :app:parseDebugLocalResources
> Task :app:createDebugCompatibleScreenManifests
> Task :app:extractDeepLinksDebug
> Task :app:generateDebugRFile
> Task :app:mergeDebugResources
> Task :app:processDebugMainManifest
> Task :app:processDebugManifest
> Task :app:preDebugAndroidTestBuild SKIPPED
> Task :app:mergeDebugAndroidTestManifest
> Task :app:processDebugAndroidTestManifest
> Task :app:generateDebugAndroidTestBuildConfig
> Task :app:generateDebugAndroidTestResources
> Task :app:packageDebugAndroidTestResources
> Task :app:processDebugAndroidTestNavigationResources
> Task :app:parseDebugAndroidTestLocalResources
> Task :app:processDebugManifestForPackage
> Task :app:generateDebugAssets UP-TO-DATE
> Task :app:javaPreCompileDebugAndroidTest
> Task :app:generateDebugAndroidTestRFile
> Task :app:mergeDebugAssets
> Task :app:compressDebugAssets
> Task :app:processDebugResources
> Task :app:generateDebugGlobalSynthetics
> Task :app:desugarDebugFileDependencies
> Task :app:mergeDebugJniLibFolders
> Task :app:checkDebugDuplicateClasses
> Task :app:mergeDebugNativeLibs
> Task :app:mergeLibDexDebug

> Task :app:stripDebugDebugSymbols
Unable to strip the following libraries, packaging them as they are: libandroidx.graphics.path.so. Run with --info option to learn more.

> Task :app:validateSigningDebug
> Task :app:writeDebugAppMetadata
> Task :app:writeDebugSigningConfigVersions
> Task :app:generateDebugAndroidTestAssets UP-TO-DATE
> Task :app:mergeDebugAndroidTestAssets
> Task :app:compressDebugAndroidTestAssets
> Task :app:generateDebugAndroidTestGlobalSynthetics
> Task :app:desugarDebugAndroidTestFileDependencies
> Task :app:checkDebugAndroidTestDuplicateClasses
> Task :app:checkDebugAndroidTestAarMetadata
> Task :app:mapDebugAndroidTestSourceSetPaths
> Task :app:mergeLibDexDebugAndroidTest
> Task :app:compileDebugAndroidTestNavigationResources
> Task :app:mergeDebugAndroidTestJniLibFolders
> Task :app:mergeDebugAndroidTestNativeLibs NO-SOURCE
> Task :app:stripDebugAndroidTestDebugSymbols NO-SOURCE
> Task :app:validateSigningDebugAndroidTest
> Task :app:writeDebugAndroidTestSigningConfigVersions
> Task :app:mergeDebugAndroidTestResources
> Task :app:processDebugAndroidTestResources
> Task :app:mergeExtDexDebugAndroidTest
> Task :app:mergeExtDexDebug
> Task :app:compileDebugKotlin
> Task :app:compileDebugJavaWithJavac
> Task :app:bundleDebugClassesToCompileJar
> Task :app:processDebugJavaRes
> Task :app:mergeDebugJavaResource
> Task :app:dexBuilderDebug
> Task :app:mergeProjectDexDebug
> Task :app:compileDebugAndroidTestKotlin
> Task :app:compileDebugAndroidTestJavaWithJavac
> Task :app:processDebugAndroidTestJavaRes
> Task :app:mergeDebugAndroidTestJavaResource
> Task :app:dexBuilderDebugAndroidTest
> Task :app:mergeProjectDexDebugAndroidTest
> Task :app:packageDebug
> Task :app:createDebugApkListingFileRedirect
> Task :app:packageDebugAndroidTest
> Task :app:createDebugAndroidTestApkListingFileRedirect

> Task :app:connectedDebugAndroidTest
Running tests on devices: Medium_Phone_API_37.0(AVD) - 17

BUILD SUCCESSFUL in 3m 43s
72 actionable tasks: 72 executed
```
