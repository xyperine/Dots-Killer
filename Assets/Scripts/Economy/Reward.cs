using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using BreakInfinity;
using UnityEngine;
using Zenject;

namespace DotsKiller.Economy
{
    public class Reward : MonoBehaviour
    {
        [field: SerializeField, SerializedDictionary("Currency", "Amount")]
        public SerializedDictionary<Currency, BigDouble> Value { get; private set; }

        private Balance _balance;


        [Inject]
        public void Initialize(Balance balance)
        {
            _balance = balance;
        }


        public void Give()
        {
            foreach (KeyValuePair<Currency,BigDouble> kvp in Value)
            {
                _balance.Add(kvp.Value, kvp.Key);
            }
        }
    }
}