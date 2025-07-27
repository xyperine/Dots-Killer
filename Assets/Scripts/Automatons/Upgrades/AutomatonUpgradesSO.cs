using System;
using System.Collections.Generic;
using BreakInfinity;
using NorskaLib.Spreadsheets;
using UnityEngine;

namespace DotsKiller.Automatons.Upgrades
{
    [CreateAssetMenu(fileName = "Automaton_Upgrades", menuName = "Automaton Upgrades SO", order = 0)]
    public class AutomatonUpgradesSO : SpreadsheetsContainerBase
    {
        [SpreadsheetContent, SerializeField] private AutomatonUpgradesSOContent content;
        [field: SerializeField] public List<AutomatonUpgradeEntry> Entries { get; private set; }


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


        public void Convert()
        {
            Entries = new List<AutomatonUpgradeEntry>();
            for (int i = 0; i < content.ImportData.Count; i++)
            {
                Entries.Add(new AutomatonUpgradeEntry(content.ImportData[i]));
            }
            
            Debug.Log("Successfully converted automatons upgrades imported data!");
        }
    }


    [Serializable]
    public class AutomatonUpgradesSOContent
    {
        [SpreadsheetPage("Automaton Upgrades"), SerializeField] private List<AutomatonUpgradeImportData> entries;

        public List<AutomatonUpgradeImportData> ImportData => entries;
    }


    [Serializable]
    public class AutomatonUpgradeImportData
    {
        public int ID;
        public int AutomatonID;
        public string Name;
        public string Description;
        public string Price;
        public string PriceScaling;
        public int MaxLevel;
    }


    public struct AutomatonUpgradeEntry
    {
        public int ID { get; init; }
        public AutomatonID AutomatonID { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public BigDouble Price { get; init; }
        public BigDouble PriceScaling { get; init; }
        public int MaxLevel { get; init; }


        public AutomatonUpgradeEntry(AutomatonUpgradeImportData importData)
        {
            ID = importData.ID;
            AutomatonID = (AutomatonID) importData.AutomatonID;
            Name = importData.Name;
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