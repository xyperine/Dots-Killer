using System;
using System.Collections.Generic;
using BreakInfinity;
using DotsKiller.Dots;
using DotsKiller.MilestonesLogic;
using DotsKiller.SaveSystem;
using DotsKiller.StatsLogic;
using DotsKiller.Utility;
using UnityEngine;
using Zenject;

namespace DotsKiller.RegularUpgrading
{
    public class RegularUpgrades : MonoBehaviour
    {
        [SerializeField] private RegularUpgradesSO regularUpgradesSO;
        [SerializeField] private List<RegularUpgrade> upgrades; 
        
        private Stats _stats;
        private DotsTracker _dotsTracker;
        private Milestones _milestones;
        
        public BigDouble PointsOnKill { get; private set; } = BigDouble.Zero;
        public BigDouble KillsFactor { get; private set; } = BigDouble.One;
        public BigDouble CleanFactor { get; private set; } = BigDouble.One;
        public BigDouble TimeFactor { get; private set; } = BigDouble.One;
        public float BountyChancePercent { get; private set; } = 0f;
        public BigDouble AccumulationFactor { get; private set; } = BigDouble.One;
        public BigDouble GrowthExponent { get; private set; } = BigDouble.One;
        public BigDouble BoostMultiplier { get; private set; } = BigDouble.One;


        [Inject]
        public void Initialize(Stats stats, DotsTracker dotsTracker, Milestones milestones)
        {
            _stats = stats;
            _dotsTracker = dotsTracker;
            _milestones = milestones;
        }


        private void Start()
        {
            LoadUpgrades();
        }


        public void PurchaseAll()
        {
            for (int i = 0; i < upgrades.Count; i++)
            {
                if (upgrades[i].IsAffordable)
                {
                    upgrades[i].Purchase();
                }
            }
        }


        public void PurchaseAllInBulk()
        {
            for (int i = 0; i < upgrades.Count; i++)
            {
                upgrades[i].PurchaseInBulk();
            }
        }
        
        
        public void PurchaseAllInBulk(int amount)
        {
            for (int i = 0; i < upgrades.Count; i++)
            {
                upgrades[i].PurchaseInBulk(amount);
            }
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
            
            foreach ((int id, int level) in GameStateHandler.State.RegularUpgradeLevels)
            {
                SetAppropriateValue(id, level);
            }
        }


        private void SetAppropriateValue(RegularUpgrade regularUpgrade)
        {
            SetAppropriateValue(regularUpgrade.ID, regularUpgrade.Level);
        }


        private void SetAppropriateValue(int id, int level)
        {
            BigDouble bonus = GetBonus(id, level);
            switch (id)
            {
                case 0:
                    PointsOnKill = bonus;
                    return;
                case 1:
                    KillsFactor = bonus;
                    return;
                case 2:
                    CleanFactor = bonus;
                    return;
                case 3:
                    TimeFactor = bonus;
                    return;
                case 4:
                    BountyChancePercent = (float) bonus.ToDouble();
                    return;
                case 5:
                    AccumulationFactor = bonus;
                    return;
                case 6:
                    GrowthExponent = bonus;
                    return;
                case 7:
                    BoostMultiplier = bonus;
                    return;
                default:
                    return;
            }
        }


        public BigDouble GetBonus(int id, int level)
        {
            BigDouble bonus = id switch
            {
                0 => BigDouble.Pow(1f * level, _milestones.FirstUpgradeBoost),
                1 => BigDouble.Log10(_stats.Kills + BigDouble.One) * level + BigDouble.One,
                2 => Formulas.CalculateCleanFactor(_dotsTracker.AmountAlive, level),
                3 => level == 0 ? BigDouble.One : Formulas.CalculateTimeFactor(_stats.TotalPlaytime.TotalSeconds),
                4 => 10f * level,
                5 => level == 0 ? BigDouble.One :_stats.TotalPoints.PositiveSafeLog10() + BigDouble.One,
                6 => (0.05f * level) + BigDouble.One,
                7 => (0.01f * level) + BigDouble.One,
                _ => BigDouble.One,
            };

            if (id != 7)
            {
                bonus *= BoostMultiplier;
            }

            if (id == 4)
            {
                bonus = Math.Clamp(bonus.ToDouble(), 0d, 100d);
            }

            return bonus;
        }


        private void Update()
        {
            int upgradesBought = 0;
            for (int i = 0; i < upgrades.Count; i++)
            {
                RegularUpgrade regularUpgrade = upgrades[i];
                
                SetAppropriateValue(regularUpgrade);

                upgradesBought += regularUpgrade.Level;
            }

            _stats.RegularUpgradesBought = upgradesBought;
        }


        public RegularUpgradeEntry GetEntry(int id)
        {
            return regularUpgradesSO.Entries.Find(e => e.ID == id);
        }


        public RegularUpgradeEntry GetSorted(int i)
        {
            return regularUpgradesSO.SortedByPrice[i];
        }
        
        
        public string GetNameEntryName(int id)
        {
            string tableName = "RegularUpgrades";
            string entryName = id switch
            {
                0 => "PointsOnKill",
                1 => "KillsFactor",
                2 => "CleanFactor",
                3 => "TimeFactor",
                4 => "Bounty",
                5 => "AccumulationFactor",
                6 => "Growth",
                7 => "Boost",
                _ => "",
            };

            return string.Join('.', tableName, entryName, "Name");
        }
        
        
        public string GetDescriptionEntryName(int id)
        {
            string tableName = "RegularUpgrades";
            string entryName = id switch
            {
                0 => "PointsOnKill",
                1 => "KillsFactor",
                2 => "CleanFactor",
                3 => "TimeFactor",
                4 => "Bounty",
                5 => "AccumulationFactor",
                6 => "Growth",
                7 => "Boost",
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
                0 => "+",
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


        public void OnPurge()
        {
            Debug.Log("Regular upgrades reset");

            for (int i = 0; i < upgrades.Count; i++)
            {
                upgrades[i].OnPrestige();
            }
        }
    }
}