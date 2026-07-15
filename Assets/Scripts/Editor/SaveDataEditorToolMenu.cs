using System.IO;
using UnityEditor;
using UnityEngine;

public static class SaveDataEditorToolMenu
{
    private const string OpenSaveFileMenu = "Tools/Open Save File Directory";
    private const string DeleteSaveFileMenu = "Tools/Delete Save Files Directory";

    [MenuItem(OpenSaveFileMenu, false, 1)]
    private static void OpenSaveFileLocation()
    {
        var path = Application.persistentDataPath;
        Debug.Log($"[SaveTool] persistentDataPath = {path}");
        EditorUtility.RevealInFinder(path);
    }

    [MenuItem(OpenSaveFileMenu, true, 2)]
    private static bool ValidateOpenSaveFileLocation()
    {
        return Directory.Exists(Application.persistentDataPath);
    }

    [MenuItem(DeleteSaveFileMenu, false, 3)]
    private static void DeleteSaveFileLocation()
    {
        var path = Application.persistentDataPath;
        Debug.Log($"[SaveTool] Deleting contents of: {path}");

        try
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                Directory.CreateDirectory(path);
                Debug.Log("[SaveTool] Deleted folder and contents.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("[SaveTool] Failed to delete: " + ex);
        }
    }

    [MenuItem(DeleteSaveFileMenu, true, 4)]
    private static bool ValidateDeleteSaveFileLocation()
    {
        return Directory.Exists(Application.persistentDataPath);
    }
}