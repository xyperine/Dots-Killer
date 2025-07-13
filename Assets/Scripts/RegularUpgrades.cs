using System;
using System.Collections.Generic;
using BreakInfinity;
using DotsKiller.SaveSystem;
using DotsKiller.UI;
using DotsKiller.Utility;
using UnityEngine;
using Zenject;

namespace DotsKiller
{
    public class RegularUpgrades : MonoBehaviour
    {
        [SerializeField] private UpgradesSO upgradesSO;
        [SerializeField] private List<Upgrade> upgrades; 
        
        private Stats _stats;
        private DotsTracker _dotsTracker;
        private GameClock _gameClock;
        
        public BigDouble PointsOnKill { get; private set; } = BigDouble.Zero;
        public BigDouble KillsFactor { get; private set; } = BigDouble.One;
        public BigDouble CleanFactor { get; private set; } = BigDouble.One;
        public BigDouble TimeFactor { get; private set; } = BigDouble.One;


        [Inject]
        public void Initialize(Stats stats, DotsTracker dotsTracker, GameClock gameClock)
        {
            _stats = stats;
            _dotsTracker = dotsTracker;
            _gameClock = gameClock;
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

            foreach ((int id, int level) in GameStateHandler.State.RegularUpgradeLevels)
            {
                SetAppropriateValue(id, level);
            }
        }


        private void SetAppropriateValue(Upgrade upgrade)
        {
            SetAppropriateValue(upgrade.ID, upgrade.Level);
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
                default:
                    return;
            }
        }


        public BigDouble GetBonus(int id, int level)
        {
            return id switch
            {
                0 => 1f * level,
                1 => BigDouble.Log10(_stats.Kills + BigDouble.One) * level + BigDouble.One,
                2 => Formulas.CalculateCleanFactor(_dotsTracker.AmountAlive, level),
                3 => Formulas.CalculateTimeFactor(TimeSpan.FromMilliseconds(_gameClock.UnscaledTimeInMilliseconds).TotalSeconds),
                _ => BigDouble.One,
            };
        }


        private void Update()
        {
            for (int i = 0; i < upgrades.Count; i++)
            {
                Upgrade upgrade = upgrades[i];
                
                SetAppropriateValue(upgrade);
            }
        }


        public UpgradeEntry GetEntry(int id)
        {
            return upgradesSO.Entries.Find(e => e.ID == id);
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
                4 => "x",
                5 => "x",
                6 => "^",
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
                4 => string.Empty,
                5 => string.Empty,
                6 => string.Empty,
                _ => throw new ArgumentOutOfRangeException(nameof(id), id, null),
            };
        }
    }
}