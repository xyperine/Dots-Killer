using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace DotsKiller.Economy
{
    public class BulkBuyModeButton : MonoBehaviour
    {
        [SerializeField] private BulkBuyCategory category;
        [SerializeField] private List<SerializableBulkBuyAmount> bulkOptions;
        [SerializeField] private TMP_Text text;

        private BulkBuyProfile _bulkBuyProfile;
        private IEnumerator<SerializableBulkBuyAmount> _enumerator;


        [Inject]
        public void Initialize(BulkBuyProfile bulkBuyProfile)
        {
            _bulkBuyProfile = bulkBuyProfile;
        }


        private void Awake()
        {
            _enumerator = bulkOptions.GetEnumerator();
        }


        public void Change()
        {
            if (!_enumerator.MoveNext())
            {
                _enumerator.Reset();
                _enumerator.MoveNext();
            }

            BulkBuyAmount amount = _enumerator.Current.CreateNonSerializable();
            text.SetText(amount.Max ? "MAX" : amount.Value.Value.ToString("G0"));
            _bulkBuyProfile.Recreate(category, amount);
        }
    }
}