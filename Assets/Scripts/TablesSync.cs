using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DotsKiller
{
    public class TablesSync : MonoBehaviour
    {
        [SerializeField] private LocalizedStringTable localizedTable;
        [SerializeField] private RegularUpgradesSO regularUpgradesSO;
        [SerializeField] private bool useTableName;
        [SerializeField, HideIf(nameof(useTableName))] private string tableName;


        [Button]
        private void Sync()
        {
            AsyncOperationHandle<StringTable> operationHandle = localizedTable.GetTableAsync();
            operationHandle.Completed += TOnCompleted;
        }


        private void TOnCompleted(AsyncOperationHandle<StringTable> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                StringTable table = obj.Result;
                
                for (int i = 0; i < regularUpgradesSO.Entries.Count; i++)
                {
                    RegularUpgradeEntry entry = regularUpgradesSO.Entries[i];

                    string nameEntryName = $"{GetTableName(table)}.{RemoveWhitespaces(entry.Title)}.Name";
                    if (table.SharedData.Contains(nameEntryName))
                    {
                        Debug.LogWarning($"\"{nameEntryName}\" entry was updated");
                    }
                    table.AddEntry(nameEntryName, entry.Title);

                    string descriptionEntryName =  $"{GetTableName(table)}.{RemoveWhitespaces(entry.Title)}.Description";
                    if (table.SharedData.Contains(descriptionEntryName))
                    {
                        Debug.LogWarning($"\"{descriptionEntryName}\" entry was updated");
                    }
                    table.AddEntry(descriptionEntryName, entry.Description);
                }
                
                EditorUtility.SetDirty(table);
                EditorUtility.SetDirty(table.SharedData);
                
                AssetDatabase.SaveAssetIfDirty(table);
                AssetDatabase.SaveAssetIfDirty(table.SharedData);
                
                Debug.Log("Added entry to table");
            }
            else if (obj.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError(obj.OperationException);
            }
        }


        private string GetTableName(LocalizationTable table)
        {
            return useTableName ? RemoveWhitespaces(table.TableCollectionName) : tableName;
        }


        private static string RemoveWhitespaces(string s)
        {
            return s.Replace(" ", string.Empty);
        }
    }
}