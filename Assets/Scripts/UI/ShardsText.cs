using DotsKiller.Economy;
using DotsKiller.Utility;
using TMPro;
using UnityEngine;
using Zenject;

namespace DotsKiller.UI
{
    public class ShardsText : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        
        private Balance _balance;
        
        
        [Inject]
        public void Initialize(Balance balance)
        {
            _balance = balance;
        }


        private void Update()
        {
            text.text = Formatting.DefaultFormat(_balance.Shards);
        }
    }
}