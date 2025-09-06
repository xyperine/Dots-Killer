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
        private IEnumerator<SerializableBulkBuyAmount> e;


        [Inject]
        public void Initialize(BulkBuyProfile bulkBuyProfile)
        {
            _bulkBuyProfile = bulkBuyProfile;
        }


        private void Awake()
        {
            e = bulkOptions.GetEnumerator();
        }


        public void Change()
        {
            if (!e.MoveNext())
            {
                e.Reset();
                e.MoveNext();
            }

            BulkBuyAmount a = e.Current.CreateNonSerializable();
            text.SetText(a.Max ? "MAX" : a.Value.Value.ToString("G0"));
            _bulkBuyProfile.Recreate(category, a);
        }
    }
}