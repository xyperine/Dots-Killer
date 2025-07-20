using System;
using System.Collections.Generic;
using BreakInfinity;
using DotsKiller.SaveSystem;
using DotsKiller.Utility;
using UnityEngine;

namespace DotsKiller
{
    public class AutomatonUpgrades : MonoBehaviour
    {
        [SerializeField] private AutomatonUpgradesSO automatonUpgradesSO;
        [SerializeField] private List<AutomatonUpgrade> upgrades;
        
        public float KillsPerSecondMultiplier { get; private set; }
        public float PurchasesPerSecondMultiplier { get; private set; }
        
        
        private void Start()
        {
            LoadUpgrades();
        }


        private void LoadUpgrades()
        {
            if (!GameStateHandler.Loaded)
            {
                return;
            }

            for (int i = 0; i < upgrades.Count; i++)
            {
                upgrades[i].Load();
            }
            
            foreach ((int id, int level) in GameStateHandler.State.AutomatonUpgradeLevels)
            {
                SetAppropriateValue(id, level);
            }
        }


        private void SetAppropriateValue(AutomatonUpgrade upgrade)
        {
            SetAppropriateValue(upgrade.ID, upgrade.Level);
        }


        private void SetAppropriateValue(int id, int level)
        {
            BigDouble bonus = GetBonus(id, level);
            switch (id)
            {
                case 0:
                    KillsPerSecondMultiplier = (float) bonus.ToDouble();
                    return;
                case 1:
                    PurchasesPerSecondMultiplier = (float) bonus.ToDouble();
                    return;
                default:
                    return;
            }
        }


        public BigDouble GetBonus(int id, int level)
        {
            BigDouble bonus = id switch
            {
                0 => BigDouble.Pow(2f, level),
                1 => BigDouble.Pow(2f, level),
                _ => BigDouble.One,
            };

            return bonus;
        }


        private void Update()
        {
            for (int i = 0; i < upgrades.Count; i++)
            {
                AutomatonUpgrade regularUpgrade = upgrades[i];
                
                SetAppropriateValue(regularUpgrade);
            }
        }


        public AutomatonUpgradeEntry GetEntry(int id)
        {
            return automatonUpgradesSO.Entries.Find(e => e.ID == id);
        }


        public AutomatonUpgradeEntry GetSorted(int i)
        {
            return automatonUpgradesSO.SortedByPrice[i];
        }
        
        
        public string GetNameEntryName(int id)
        {
            string tableName = "AutomatonUpgrades";
            string entryName = id switch
            {
                0 => "KillerSpeed",
                1 => "PurchaserSpeed",
                _ => "",
            };

            return string.Join('.', tableName, entryName, "Name");
        }
        
        
        public string GetDescriptionEntryName(int id)
        {
            string tableName = "AutomatonUpgrades";
            string entryName = id switch
            {
                0 => "KillerSpeed",
                1 => "PurchaserSpeed",
                _ => "",
            };

            return string.Join('.', tableName, entryName, "Description");
        }


        public string GetBonusText(int id, int level, bool maxedOut, Color bonusColor)
        {
            string bonusPrefix = GetBonusPrefix(id);
            string bonusSuffix = GetBonusSuffix(id);
            string currentBonus = Formatting.DefaultFormat(GetBonus(id, level));
            string bonuses = $"<color=#{ColorUtility.ToHtmlStringRGB(bonusColor)}>{bonusPrefix}{currentBonus}{bonusSuffix}</color>";
            if (!maxedOut)
            {
                string nextLevelBonus = Formatting.DefaultFormat(GetBonus(id, level + 1));
                bonuses += $" >> <color=#{ColorUtility.ToHtmlStringRGB(bonusColor)}>{bonusPrefix}{nextLevelBonus}{bonusSuffix}</color>";   
            }

            return bonuses;
        }
        

        private string GetBonusPrefix(int id)
        {
            return id switch
            {
                0 => "x",
                1 => "x",
                2 => "x",
                3 => "x",
                4 => string.Empty,
                5 => "x",
                6 => "^",
                7 => "x",
                _ => throw new ArgumentOutOfRangeException(nameof(id), id, null),
            };
        }
        
        
        private string GetBonusSuffix(int id)
        {
            return id switch
            {
                0 => string.Empty,
                1 => string.Empty,
                2 => string.Empty,
                3 => string.Empty,
                4 => "%",
                5 => string.Empty,
                6 => string.Empty,
                7 => string.Empty,
                _ => throw new ArgumentOutOfRangeException(nameof(id), id, null),
            };
        }
    }
}