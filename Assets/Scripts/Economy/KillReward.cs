using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using BreakInfinity;
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
            foreach (KeyValuePair<Currency,BigDouble> kvp in value)
            {
                if (kvp.Key == Currency.Points)
                {
                    _balance.Add(kvp.Value + _regularUpgrades.PointsOnKill, kvp.Key);
                    continue;
                }
                
                _balance.Add(kvp.Value, kvp.Key);
            }
        }
    }
}