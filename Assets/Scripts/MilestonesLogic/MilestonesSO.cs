using System;
using System.Collections.Generic;
using System.Linq;
using NorskaLib.Spreadsheets;
using UnityEngine;

namespace DotsKiller.MilestonesLogic
{
    [CreateAssetMenu(fileName = "Milestones", menuName = "Milestones SO", order = 0)]
    public class MilestonesSO : SpreadsheetsContainerBase
    {
        [SpreadsheetContent, SerializeField] private MilestonesSOContent content;
        
        public List<MilestoneEntry> Entries { get; private set; }

        public List<MilestoneEntry> SortedByThreshold => Entries.OrderBy(e => e.KillsThreshold).ToList();
        
        
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
            Entries = new List<MilestoneEntry>();
            for (int i = 0; i < content.ImportData.Count; i++)
            {
                Entries.Add(new MilestoneEntry(content.ImportData[i]));
            }
            
            Debug.Log("Successfully converted milestones imported data!");
        }
    }


    [Serializable]
    public class MilestonesSOContent
    {
        [SpreadsheetPage("Milestones"), SerializeField]
        private List<MilestoneImportData> entries;

        public List<MilestoneImportData> ImportData => entries;
    }


    [Serializable]
    public struct MilestoneImportData
    {
        public int ID;
        public int KillsThreshold;
        public string Reward;
    }


    public struct MilestoneEntry
    {
        public int ID { get; init; }
        public int KillsThreshold { get; init; }
        public string Reward { get; init; }


        public MilestoneEntry(MilestoneImportData importData) : this(importData.ID, importData.KillsThreshold,
            importData.Reward)
        {
            
        }
        
        
        public MilestoneEntry(int id, int killsThreshold, string reward)
        {
            ID = id;
            KillsThreshold = killsThreshold;
            Reward = reward;
        }
    }
}