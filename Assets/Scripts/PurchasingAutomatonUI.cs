using DotsKiller.Utility;
using TMPro;
using UnityEngine;

namespace DotsKiller
{
    public class PurchasingAutomatonUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text purchasesPerSecondText;
        [SerializeField] private PurchasingAutomaton automaton;

        private string _format;


        private void Awake()
        {
            _format = purchasesPerSecondText.text;
        }


        private void Update()
        {
            nameText.text = "Auto Purchase";
            purchasesPerSecondText.text = string.Format(_format,Formatting.DefaultFormat(automaton.Tickspeed));
        }


        public void SetStatus(bool value)
        {
            automaton.SetStatus(value);
        }
    }
}