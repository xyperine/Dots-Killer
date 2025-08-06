using DotsKiller.Economy;
using DotsKiller.StatsLogic;
using DotsKiller.Utility;
using TMPro;
using UnityEngine;
using Zenject;

namespace DotsKiller.UI
{
    public class ResourcesPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text pointsText;
        [SerializeField] private TMP_Text killsText;
        [SerializeField] private TMP_Text shardsText;
        [SerializeField] private TMP_Text purgesText;

        private Balance _balance;
        private Stats _stats;

        private string _pointsTextFormat;
        private string _killsTextFormat;
        private string _shardsTextFormat;
        private string _purgesTextFormat;


        [Inject]
        public void Initialize(Balance balance, Stats stats)
        {
            _balance = balance;
            _stats = stats;
        }


        private void Awake()
        {
            _pointsTextFormat = pointsText.text;
            _killsTextFormat = killsText.text;
            _shardsTextFormat = shardsText.text;
            _purgesTextFormat = purgesText.text;
        }


        private void Update()
        {
            pointsText.text = string.Format(_pointsTextFormat, Formatting.DefaultFormat(_balance.Points));
            killsText.text = string.Format(_killsTextFormat, Formatting.DefaultFormat(_stats.Kills));
            shardsText.text = string.Format(_shardsTextFormat, Formatting.DefaultFormat(_balance.Shards));
            purgesText.text = string.Format(_purgesTextFormat, Formatting.DefaultFormat(_stats.Purges));
        }
    }
}