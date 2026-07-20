using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ScenesToolUtility
{
    [MenuItem("Scenes/InitialScene", false, 1)]
    public static void InitialScene() => OpenEditorScene("InitialScene");

    [MenuItem("Scenes/GameScene", false, 3)]
    public static void GameScene() => OpenEditorScene("GameScene");

    private static void OpenEditorScene(string sceneName)
    {
        if (Application.isPlaying)
            return;

        EditorSceneManager.OpenScene($"{Application.dataPath}/Scenes/{sceneName}.unity");
    }
}