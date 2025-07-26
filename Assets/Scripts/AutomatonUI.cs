using DotsKiller.Utility;
using TMPro;
using UnityEngine;

namespace DotsKiller
{
    public class AutomatonUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text tickspeedText;
        [SerializeField] private TMP_Text actionsPerTickText;
        [SerializeField] private Automaton automaton;

        private string _tickspeedFormat;
        private string _actionsFormat;


        private void Awake()
        {
            _tickspeedFormat = tickspeedText.text;
            _actionsFormat = actionsPerTickText.text;
        }


        private void Update()
        {
            nameText.text = automaton.Name;
            tickspeedText.text = string.Format(_tickspeedFormat, Formatting.DefaultFormat(automaton.Tickspeed));
            actionsPerTickText.text = string.Format(_actionsFormat, Formatting.DefaultFormat(automaton.ActionsPerTick));
        }


        public void SetStatus(bool value)
        {
            automaton.SetStatus(value);
        }
    }
}