using DotsKiller.Economy;
using TMPro;
using UnityEngine;
using Zenject;

namespace DotsKiller.UI
{
    public class PointsText : MonoBehaviour
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
            text.text = _balance.Points.ToString("F0");
        }
    }
}