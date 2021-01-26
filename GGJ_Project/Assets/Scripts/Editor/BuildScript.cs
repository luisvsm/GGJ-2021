using UnityEditor;
using System.Linq;

public class BuildScript
{
    static string[] getScenes()
    {
        return EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
    }

    [MenuItem("NullyRef/Build/Linux64")]
    static void PerformBuildLinux64()
    {
        string buildName = PlayerSettings.productName.Replace(" ", "-");
        string buildFolder = "linux";
        string extension = "x86_64";
        string buildArtifact = $"./builds/{buildFolder}/{buildName}.{extension}";

        BuildPipeline.BuildPlayer(getScenes(), buildArtifact,
            BuildTarget.StandaloneLinux64, BuildOptions.None);

        System.IO.File.WriteAllText(@"./Assets/Game/Scripts/Build/lastbuild.txt", buildArtifact);
    }


    [MenuItem("NullyRef/Build/OSX")]
    static void PerformBuildOSX()
    {
        string buildName = PlayerSettings.productName.Replace(" ", "-");
        string buildFolder = "osx";
        string extension = "app";
        string buildArtifact = $"./builds/{buildFolder}/{buildName}.{extension}";

        BuildPipeline.BuildPlayer(getScenes(), buildArtifact,
            BuildTarget.StandaloneOSX, BuildOptions.None);

        System.IO.File.WriteAllText(@"./Assets/Game/Scripts/Build/lastbuild.txt", $"./builds/{buildFolder}");
    }

    [MenuItem("NullyRef/Build/Windows")]
    static void PerformBuildWindows()
    {
        string buildName = PlayerSettings.productName.Replace(" ", "-");
        string buildFolder = "windows";
        string extension = "exe";
        string buildArtifact = $"./builds/{buildFolder}/{buildName}.{extension}";

        BuildPipeline.BuildPlayer(getScenes(), buildArtifact,
            BuildTarget.StandaloneWindows64, BuildOptions.None);

        System.IO.File.WriteAllText(@"./Assets/Game/Scripts/Build/lastbuild.txt", $"./builds/{buildFolder}");
    }

    [MenuItem("NullyRef/Build/Android")]
    static void PerformBuildAndroid()
    {
        string buildName = PlayerSettings.productName.Replace(" ", "-");
        string buildFolder = "android";
        string extension = "apk";
        string buildArtifact = $"./builds/{buildFolder}/{buildName}.{extension}";

        BuildPipeline.BuildPlayer(getScenes(), buildArtifact,
            BuildTarget.Android, BuildOptions.None);

        System.IO.File.WriteAllText(@"./Assets/Game/Scripts/Build/lastbuild.txt", buildArtifact);
    }

    [MenuItem("NullyRef/Build/iOS")]
    static void PerformBuildIOS()
    {
        string buildName = "XcodeProject";
        string buildFolder = "ios";
        string buildArtifact = $"./builds/{buildFolder}/{buildName}";

        BuildPipeline.BuildPlayer(getScenes(), buildArtifact,
            BuildTarget.iOS, BuildOptions.None);

        System.IO.File.WriteAllText(@"./Assets/Game/Scripts/Build/lastbuild.txt", buildArtifact);
    }
}