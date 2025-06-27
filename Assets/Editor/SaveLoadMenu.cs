using System.Diagnostics;
using System.IO;
using DotsKiller.SaveSystem;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace DotsKiller.Editor
{
    public static class SaveLoadMenu
    {
        private static readonly string FolderPath = Application.persistentDataPath;
        private static readonly string FullPath = FolderPath + "/" + "save.json";
        
        
        [MenuItem("Saving/Select Manager")]
        private static void SelectManager()
        {
            SaveLoadManager saveLoadManager = Object.FindFirstObjectByType<SaveLoadManager>();

            if (!saveLoadManager)
            {
                Debug.LogWarning("Save Load Manager wasn't found!");
                return;
            }
            
            EditorGUIUtility.PingObject(saveLoadManager);
            Selection.SetActiveObjectWithContext(saveLoadManager, null);
        }
        
        
        [MenuItem("Saving/Save File/Open", priority = 0)]
        private static void Open()
        {
            if (!File.Exists(FullPath))
            {
                Debug.LogWarning($"{FullPath} was not found!");
                return;
            }
            
            Process.Start(FullPath);
        }
        
        
        [MenuItem("Saving/Save File/Open Folder", priority = 1)]
        private static void OpenFolder()
        {
            if (!Directory.Exists(FolderPath))
            {
                Debug.LogWarning($"{FolderPath} was not found!");
                return;
            }
            
            Process.Start(FolderPath);
        }


        [MenuItem("Saving/Save File/Clear", priority = 2)]
        private static void Clear()
        {
            File.Delete(FullPath);
        }
    }
}
