using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using BreakInfinity;
using DotsKiller.RegularUpgrading;
using UnityEngine;
using Zenject;

namespace DotsKiller.Economy
{
    public class KillReward : MonoBehaviour
    {
        [field: SerializeField, SerializedDictionary("Currency", "Amount")]
        public SerializedDictionary<Currency, BigDouble> Value { get; private set; }

        private Balance _balance;
        private RegularUpgrades _regularUpgrades;


        [Inject]
        public void Initialize(Balance balance, RegularUpgrades regularUpgrades)
        {
            _balance = balance;
            _regularUpgrades = regularUpgrades;
        }


        public void Give()
        {
            Give(Value);
        }


        public void Give(Dictionary<Currency, BigDouble> value)
        {
            foreach ((Currency currency, BigDouble reward) in value)
            {
                if (currency == Currency.Points)
                {
                    BigDouble pointsReward = reward;
                    
                    pointsReward += _regularUpgrades.PointsOnKill;
                    
                    if (_regularUpgrades.BountyChancePercent >= Random.Range(0f, 100f))
                    {
                        pointsReward *= 2f;
                    }
                    
                    _balance.Add(pointsReward, currency);
                    continue;
                }

                _balance.Add(reward, currency);
            }
        }
    }
}