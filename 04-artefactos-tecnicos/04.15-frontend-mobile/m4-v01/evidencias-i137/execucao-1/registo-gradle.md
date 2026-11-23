# I137 — Execução 1 — correção da localização do SDK

Execução: `d27fcc4bc41c`. Sem `ANDROID_HOME` ou `ANDROID_SDK_ROOT` definidos no terminal.

```text
> Task :app:preBuild UP-TO-DATE
> Task :app:preDebugBuild UP-TO-DATE
> Task :app:generateDebugBuildConfig
> Task :app:generateDebugResources UP-TO-DATE
> Task :app:packageDebugResources UP-TO-DATE
> Task :app:processDebugNavigationResources UP-TO-DATE
> Task :app:parseDebugLocalResources UP-TO-DATE
> Task :app:generateDebugRFile UP-TO-DATE
> Task :app:javaPreCompileDebug UP-TO-DATE
> Task :app:checkDebugAarMetadata UP-TO-DATE
> Task :app:mapDebugSourceSetPaths UP-TO-DATE
> Task :app:compileDebugNavigationResources UP-TO-DATE
> Task :app:mergeDebugResources UP-TO-DATE
> Task :app:createDebugCompatibleScreenManifests UP-TO-DATE
> Task :app:extractDeepLinksDebug UP-TO-DATE
> Task :app:processDebugMainManifest UP-TO-DATE
> Task :app:processDebugManifest UP-TO-DATE
> Task :app:processDebugManifestForPackage UP-TO-DATE
> Task :app:processDebugResources UP-TO-DATE
> Task :app:preDebugAndroidTestBuild SKIPPED
> Task :app:mergeDebugAndroidTestManifest
> Task :app:processDebugAndroidTestManifest
> Task :app:generateDebugAndroidTestBuildConfig
> Task :app:generateDebugAndroidTestResources
> Task :app:packageDebugAndroidTestResources
> Task :app:processDebugAndroidTestNavigationResources
> Task :app:generateDebugAssets UP-TO-DATE
> Task :app:javaPreCompileDebugAndroidTest
> Task :app:parseDebugAndroidTestLocalResources
> Task :app:mergeDebugAssets UP-TO-DATE
> Task :app:compressDebugAssets UP-TO-DATE
> Task :app:generateDebugGlobalSynthetics UP-TO-DATE
> Task :app:checkDebugDuplicateClasses UP-TO-DATE
> Task :app:desugarDebugFileDependencies UP-TO-DATE
> Task :app:generateDebugAndroidTestRFile
> Task :app:mergeExtDexDebug UP-TO-DATE
> Task :app:mergeLibDexDebug UP-TO-DATE
> Task :app:mergeDebugJniLibFolders UP-TO-DATE
> Task :app:mergeDebugNativeLibs UP-TO-DATE
> Task :app:stripDebugDebugSymbols UP-TO-DATE
> Task :app:validateSigningDebug UP-TO-DATE
> Task :app:writeDebugAppMetadata UP-TO-DATE
> Task :app:writeDebugSigningConfigVersions UP-TO-DATE
> Task :app:generateDebugAndroidTestAssets UP-TO-DATE
> Task :app:mergeDebugAndroidTestAssets
> Task :app:compressDebugAndroidTestAssets
> Task :app:generateDebugAndroidTestGlobalSynthetics
> Task :app:desugarDebugAndroidTestFileDependencies
> Task :app:checkDebugAndroidTestDuplicateClasses
> Task :app:checkDebugAndroidTestAarMetadata
> Task :app:mergeLibDexDebugAndroidTest
> Task :app:mapDebugAndroidTestSourceSetPaths
> Task :app:compileDebugAndroidTestNavigationResources
> Task :app:compileDebugKotlin
> Task :app:mergeExtDexDebugAndroidTest
> Task :app:mergeDebugAndroidTestResources
> Task :app:compileDebugJavaWithJavac
> Task :app:processDebugJavaRes UP-TO-DATE
> Task :app:mergeDebugJavaResource UP-TO-DATE
> Task :app:bundleDebugClassesToCompileJar
> Task :app:dexBuilderDebug
> Task :app:mergeDebugAndroidTestJniLibFolders
> Task :app:mergeProjectDexDebug
> Task :app:mergeDebugAndroidTestNativeLibs NO-SOURCE
> Task :app:stripDebugAndroidTestDebugSymbols NO-SOURCE
> Task :app:validateSigningDebugAndroidTest
> Task :app:writeDebugAndroidTestSigningConfigVersions
> Task :app:packageDebug
> Task :app:createDebugApkListingFileRedirect UP-TO-DATE
> Task :app:processDebugAndroidTestResources
> Task :app:compileDebugAndroidTestKotlin
> Task :app:compileDebugAndroidTestJavaWithJavac
> Task :app:processDebugAndroidTestJavaRes
> Task :app:dexBuilderDebugAndroidTest
> Task :app:mergeDebugAndroidTestJavaResource
> Task :app:mergeProjectDexDebugAndroidTest
> Task :app:packageDebugAndroidTest
> Task :app:createDebugAndroidTestApkListingFileRedirect

> Task :app:connectedDebugAndroidTest
Running tests on devices: Medium_Phone_API_37.0(AVD) - 17

BUILD SUCCESSFUL in 3m 7s
72 actionable tasks: 40 executed, 32 up-to-date
```
