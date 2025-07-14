using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using BreakInfinity;
using NorskaLib.Spreadsheets;
using UnityEngine;

namespace DotsKiller
{
    [CreateAssetMenu(fileName = "Regular_Upgrades", menuName = "Regular Upgrades SO", order = 0)]
    public class RegularUpgradesSO : SpreadsheetsContainerBase
    {
        [SpreadsheetContent, SerializeField] private RegularUpgradesSOContent content;
        [field: SerializeField] public List<RegularUpgradeEntry> Entries { get; private set; }
        public List<RegularUpgradeEntry> SortedByPrice => Entries.OrderBy(e => e.Price).ToList();


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
            Entries = new List<RegularUpgradeEntry>();
            for (int i = 0; i < content.ImportData.Count; i++)
            {
                Entries.Add(new RegularUpgradeEntry(content.ImportData[i]));
            }
            
            Debug.Log("Successfully converted imported data!");
        }
    }


    [Serializable]
    public class RegularUpgradesSOContent
    {
        [SpreadsheetPage("Regular Upgrades"), SerializeField] private List<RegularUpgradeImportData> entries;

        public List<RegularUpgradeImportData> ImportData => entries;
    }


    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")] // Field names have to be the same as the column headers in sheets table
    public struct RegularUpgradeImportData
    {
        public int ID;
        public string Title;
        public string Description;
        public string Price;
        public string PriceScaling;
        public int MaxLevel;
    }


    public struct RegularUpgradeEntry
    {
        public int ID { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public BigDouble Price { get; init; }
        public BigDouble PriceScaling { get; init; }
        public int MaxLevel { get; init; }


        public RegularUpgradeEntry(RegularUpgradeImportData importData)
        {
            ID = importData.ID;
            Title = importData.Title;
            Description = importData.Description;
            Price = BigDouble.Parse(PrepareForBigDoubleParse(importData.Price));
            PriceScaling = BigDouble.Parse(PrepareForBigDoubleParse(importData.PriceScaling));
            MaxLevel = importData.MaxLevel;
        }


        private static string PrepareForBigDoubleParse(string value)
        {
            string result = value;
                
            if (string.IsNullOrEmpty(result) || string.IsNullOrWhiteSpace(result))
            {
                result = "0";
                return result;
            }
            
            if (result == "None")
            {
                result = "1";
                return result;
            }
            
            if (result.Contains("+0"))
            {
                result = result.Replace("+0", "");
            }
            if (result.Contains("+"))
            {
                result = result.Replace("+", "");
            }

            result = result.ToLower();
            return result;
        }
    }
}