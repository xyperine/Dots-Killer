using System.Collections.Generic;
using DotsKiller.Economy.BulkBuy;
using TMPro;
using UnityEngine;
using Zenject;

namespace DotsKiller.UI
{
    public class BulkBuyModeButton : MonoBehaviour
    {
        [SerializeField] private BulkBuyCategory category;
        [SerializeField] private List<SerializableBulkBuyMode> bulkOptions;
        [SerializeField] private TMP_Text text;

        private PlayerBulkBuy _playerBulkBuy;
        private IEnumerator<SerializableBulkBuyMode> _enumerator;


        [Inject]
        public void Initialize(PlayerBulkBuy playerBulkBuy)
        {
            _playerBulkBuy = playerBulkBuy;
        }


        private void Awake()
        {
            _enumerator = bulkOptions.GetEnumerator();
        }


        private void Start()
        {
            Change();
        }


        public void Change()
        {
            if (!_enumerator.MoveNext())
            {
                _enumerator.Reset();
                _enumerator.MoveNext();
            }

            BulkBuyMode mode = _enumerator.Current.CreateNonSerializable();
            text.SetText(mode.Max ? "MAX" : mode.Amount.Value.ToString("G0"));
            _playerBulkBuy.SetCategoryMode(category, mode);
        }
    }
}