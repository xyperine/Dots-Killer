using System;
using System.Collections.Generic;
using BreakInfinity;
using NorskaLib.Spreadsheets;
using UnityEngine;

namespace DotsKiller
{
    [CreateAssetMenu(fileName = "Upgrades", menuName = "Upgrades SO", order = 0)]
    public class UpgradesSO : SpreadsheetsContainerBase
    {
        [SpreadsheetContent, SerializeField] private UpgradeSoContent content;
        [field: SerializeField] public List<UpgradeEntry> Entries { get; private set; }
        

#if UNITY_EDITOR
        private void OnValidate()
        {
            Convert();
        }
#endif


        private void OnEnable()
        {
            Convert();
        }


        private void Convert()
        {
            Entries = new List<UpgradeEntry>();
            for (int i = 0; i < content.ImportData.Count; i++)
            {
                Entries.Add(new UpgradeEntry(content.ImportData[i]));
            }
            
            Debug.Log("Successfully converted imported data!");
        }
    }


    [Serializable]
    public class UpgradeSoContent
    {
        [SpreadsheetPage("Upgrades"), SerializeField] private List<UpgradeImportData> entries;

        public List<UpgradeImportData> ImportData => entries;
    }


    [Serializable]
    public struct UpgradeImportData
    {
        public int ID;
        public string Title;
        public string Description;
        public string Price;
        public string PriceScaling;
        public int MaxLevel;
    }


    public struct UpgradeEntry
    {
        public int ID { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public BigDouble Price { get; init; }
        public BigDouble PriceScaling { get; init; }
        public int MaxLevel { get; init; }


        public UpgradeEntry(UpgradeImportData importData)
        {
            ID = importData.ID;
            Title = importData.Title;
            Description = importData.Description;
            Price = BigDouble.Parse(ValidateBigDouble(importData.Price));
            PriceScaling = BigDouble.Parse(ValidateBigDouble(importData.PriceScaling));
            MaxLevel = importData.MaxLevel;
        }


        private static string ValidateBigDouble(string v)
        {
            string result = v;
            
            if (result == "None")
            {
                result = "1";
                return result;
            }
            
            if (result.Contains("+0"))
            {
                result = result.Replace("+0", "");
            }

            result = result.ToLower();
            return result;
        }
    }
}