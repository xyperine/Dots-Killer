using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace DotsKiller
{
    public class StatUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text valueText;
        [SerializeField] private LocalizeStringEvent labelLse;


        public void SetText(string labelTableEntryName, string value)
        {
            labelLse.SetEntry($"Stats.{labelTableEntryName}.Label");
            valueText.text = value;
        }
    }
}