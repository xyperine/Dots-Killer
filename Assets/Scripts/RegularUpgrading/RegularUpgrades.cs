using System;
using System.Collections.Generic;
using BreakInfinity;
using DotsKiller.Economy;
using DotsKiller.SaveSystem;
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
        private GameClock _gameClock;
        private Balance _balance;
        
        public BigDouble PointsOnKill { get; private set; } = BigDouble.Zero;
        public BigDouble KillsFactor { get; private set; } = BigDouble.One;
        public BigDouble CleanFactor { get; private set; } = BigDouble.One;
        public BigDouble TimeFactor { get; private set; } = BigDouble.One;
        public float BountyChancePercent { get; private set; } = 0f;
        public BigDouble AccumulationFactor { get; private set; } = BigDouble.One;
        public BigDouble GrowthExponent { get; private set; } = BigDouble.One;
        public BigDouble BoostMultiplier { get; private set; } = BigDouble.One;


        [Inject]
        public void Initialize(Stats stats, DotsTracker dotsTracker, GameClock gameClock, Balance balance)
        {
            _stats = stats;
            _dotsTracker = dotsTracker;
            _gameClock = gameClock;
            _balance = balance;
        }


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
                0 => 1f * level,
                1 => BigDouble.Log10(_stats.Kills + BigDouble.One) * level + BigDouble.One,
                2 => Formulas.CalculateCleanFactor(_dotsTracker.AmountAlive, level),
                3 => Formulas.CalculateTimeFactor(TimeSpan.FromMilliseconds(_gameClock.UnscaledTimeInMilliseconds).TotalSeconds),
                4 => 10f * level,
                5 => BigDouble.Log10(_balance.TotalPoints + BigDouble.One),
                6 => (0.05f * level) + BigDouble.One,
                7 => (0.01f * level) + BigDouble.One,
                _ => BigDouble.One,
            };

            if (id != 7)
            {
                bonus *= BoostMultiplier;
            }

            return bonus;
        }


        private void Update()
        {
            for (int i = 0; i < upgrades.Count; i++)
            {
                RegularUpgrade regularUpgrade = upgrades[i];
                
                SetAppropriateValue(regularUpgrade);
            }
        }


        public RegularUpgradeEntry GetEntry(int id)
        {
            return regularUpgradesSO.Entries.Find(e => e.ID == id);
        }


        public RegularUpgradeEntry GetSorted(int i)
        {
            return regularUpgradesSO.SortedByPrice[i];
        }
        
        
        public string GetName(int id)
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
        
        
        public string GetDescription(int id)
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


        public string GetBonusText(int id, int level, bool maxedOut)
        {
            string bonusPrefix = GetBonusPrefix(id);
            string bonusSuffix = GetBonusSuffix(id);
            string currentBonus = Formatting.DefaultFormat(GetBonus(id, level));
            string bonuses = $"{bonusPrefix}{currentBonus}{bonusSuffix}";
            if (!maxedOut)
            {
                string nextLevelBonus = Formatting.DefaultFormat(GetBonus(id, level + 1));
                bonuses += $" >> {bonusPrefix}{nextLevelBonus}{bonusSuffix}";   
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
    }
}