using System;
using DotsKiller.Economy;
using DotsKiller.Utility;
using TMPro;
using UnityEngine;
using Zenject;

namespace DotsKiller.UI
{
    public class PointsText : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        private string _format;
        private Balance _balance;
        
        
        [Inject]
        public void Initialize(Balance balance)
        {
            _balance = balance;
        }


        private void Awake()
        {
            _format = text.text;
        }


        private void Update()
        {
            text.text = string.Format(_format, Formatting.DefaultFormat(_balance.Points));
        }
    }
}