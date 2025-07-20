using TMPro;
using UnityEngine;

namespace DotsKiller
{
    public class PurchasingAutomatonUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text purchasesAutomaton;
        [SerializeField] private PurchasingAutomaton automaton;

        private string _format;


        private void Awake()
        {
            _format = purchasesAutomaton.text;
        }


        private void Update()
        {
            nameText.text = "Purchasing Automaton";
            purchasesAutomaton.text = string.Format(_format, automaton.PurchasesPerSecond);
        }


        public void SetStatus(bool value)
        {
            automaton.SetStatus(value);
        }
    }
}