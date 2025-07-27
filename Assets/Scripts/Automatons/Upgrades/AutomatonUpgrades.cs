using System;
using System.Collections.Generic;
using System.Linq;
using BreakInfinity;
using DotsKiller.SaveSystem;
using DotsKiller.Utility;
using UnityEngine;

namespace DotsKiller.Automatons.Upgrades
{
    public class AutomatonUpgrades : MonoBehaviour
    {
        [SerializeField] private AutomatonUpgradesSO automatonUpgradesSO;
        [SerializeField] private List<AutomatonUpgrade> upgrades;

        public float KillsTickspeedMultiplier { get; private set; } = 1f;
        public float PurchasesTickspeedMultiplier { get; private set; } = 1f;
        public float KillsActionsPerTickMultiplier { get; private set; } = 1f;
        public float PurchasesActionsPerTickMultiplier { get; private set; } = 1f;
        public bool AutoPurchaseAptMaxedOut { get; private set; } = false;
        
        
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
        }


        private void SetAppropriateValue(AutomatonUpgrade upgrade)
        {
            SetAppropriateValue(upgrade.ID, upgrade.Level, upgrade.MaxedOut);
        }


        private void SetAppropriateValue(int id, int level, bool maxedOut)
        {
            BigDouble bonus = GetBonus(id, level);
            switch (id)
            {
                case 0:
                    KillsTickspeedMultiplier = (float) bonus.ToDouble();
                    return;
                case 1:
                    PurchasesTickspeedMultiplier = (float) bonus.ToDouble();
                    return;
                case 2:
                    KillsActionsPerTickMultiplier = (float) bonus.ToDouble();
                    return;
                case 3:
                    PurchasesActionsPerTickMultiplier = (float) bonus.ToDouble();
                    if (maxedOut)
                    {
                        AutoPurchaseAptMaxedOut = true;
                    }
                    return;
                default:
                    return;
            }
        }


        public BigDouble GetBonus(int id, int level)
        {
            BigDouble bonus = id switch
            {
                0 => BigDouble.Pow(1.25f, level),
                1 => BigDouble.Pow(1.25f, level),
                2 => BigDouble.Pow(2f, level),
                3 => BigDouble.Pow(2f, level),
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


        public AutomatonUpgradeEntry GetSorted(int i, AutomatonID automatonID)
        {
            AutomatonUpgradeEntry[] a = automatonUpgradesSO.Entries.Where(e => e.AutomatonID == automatonID)
                .OrderBy(e => e.Price)
                .ToArray();
            return a[i];
        }
        
        
        public string GetNameTableEntryName(int id)
        {
            string tableName = "AutomatonUpgrades";
            string entryName = GetEntryTableName(id);

            return string.Join('.', tableName, entryName, "Name");
        }


        private string GetEntryTableName(int id)
        {
            return id switch
            {
                0 => "AutoKill.Tickspeed",
                1 => "AutoPurchase.Tickspeed",
                2 => "AutoKill.APT",
                3 => "AutoPurchase.APT",
                _ => "",
            };
        }
        

        public string GetBonusText(int id, int level, int maxLevel, Color bonusColor)
        {
            bool maxedOut = level >= maxLevel;
            bool maxedOutNextLevel = level + 1 >= maxLevel;

            string bonusPrefix = GetBonusPrefix(id, maxedOut);
            string bonusSuffix = GetBonusSuffix(id, maxedOut);
            string currentBonus =
                maxedOut ? GetMaxedOutBonusText(id, maxLevel) : Formatting.DefaultFormat(GetBonus(id, level));
            string bonuses = $"<color=#{ColorUtility.ToHtmlStringRGB(bonusColor)}>{bonusPrefix}{currentBonus}{bonusSuffix}</color>";
            if (!maxedOut)
            {
                string nextLevelBonus = maxedOutNextLevel
                    ? GetMaxedOutBonusText(id, maxLevel)
                    : Formatting.DefaultFormat(GetBonus(id, level + 1));
                bonuses +=
                    $" >> <color=#{ColorUtility.ToHtmlStringRGB(bonusColor)}>{GetBonusPrefix(id, maxedOutNextLevel)}{nextLevelBonus}{GetBonusSuffix(id, maxedOutNextLevel)}</color>";
            }
            
            return bonuses;
        }


        private string GetMaxedOutBonusText(int id, int maxLevel)
        {
            return id switch
            {
                0 => Formatting.DefaultFormat(GetBonus(id, maxLevel)),
                1 => Formatting.DefaultFormat(GetBonus(id, maxLevel)),
                2 => Formatting.DefaultFormat(GetBonus(id, maxLevel)),
                3 => "BULK",
                _ => throw new ArgumentOutOfRangeException(nameof(id)),
            };
        }
        

        private string GetBonusPrefix(int id, bool maxedOut)
        {
            return id switch
            {
                0 => "x",
                1 => "x",
                2 => "x",
                3 => maxedOut? string.Empty : "x",
                4 => string.Empty,
                5 => "x",
                6 => "^",
                7 => "x",
                _ => throw new ArgumentOutOfRangeException(nameof(id), id, null),
            };
        }
        
        
        private string GetBonusSuffix(int id, bool maxedOut)
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