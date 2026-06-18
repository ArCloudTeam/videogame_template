using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.IO;
using System;

public static class AndroidBuilder
{
    [MenuItem("Build/Build Android App Bundle")]
    public static void BuildAAB()
    {
        PlayerSettings.Android.keystoreName = "Build/arcdteam.viva2026.keystore";
        PlayerSettings.Android.keystorePass = EnvOrDefault("ANDROID_KEYSTORE_PASS", "blacksoldierpendejo1");
        PlayerSettings.Android.keyaliasName = "arcdteam";
        PlayerSettings.Android.keyaliasPass = EnvOrDefault("ANDROID_KEYALIAS_PASS", "blacksoldierpendejo1");

        string[] scenes = { "Assets/Scenes/SampleScene.unity" };
        string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "Build/arcdteam.viva2026.aab");

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = outputPath,
            targetGroup = BuildTargetGroup.Android,
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);

        if (report.summary.result == BuildResult.Succeeded)
            UnityEngine.Debug.Log("Build exitoso: " + outputPath);
        else
            UnityEngine.Debug.LogError("Build fallido: " + report.summary.totalErrors + " errores");
    }

    private static string EnvOrDefault(string key, string fallback)
    {
        string val = Environment.GetEnvironmentVariable(key);
        return string.IsNullOrEmpty(val) ? fallback : val;
    }
}
