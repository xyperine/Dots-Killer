using TMPro;
using UnityEngine;

namespace DotsKiller
{
    public class KillerAutomatonUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text killsPerSecondText;
        [SerializeField] private KillerAutomaton automaton;

        private string _format;


        private void Awake()
        {
            _format = killsPerSecondText.text;
        }


        private void Update()
        {
            nameText.text = "Killer";
            killsPerSecondText.text = string.Format(_format, automaton.KillsPerSecond);
        }


        public void SetStatus(bool value)
        {
            automaton.SetStatus(value);
        }
    }
}