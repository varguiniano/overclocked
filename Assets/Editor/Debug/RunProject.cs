using UnityEditor;
using UnityEditor.SceneManagement;

public static class RunProject
{
    [MenuItem("Whatever/Run")]
    public static void RunProjectFromLaunchScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/LaunchScene.unity");
        EditorApplication.ExecuteMenuItem("Edit/Play");
    }
}
