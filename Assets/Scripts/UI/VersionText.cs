using TMPro;
using UnityEngine;

namespace DotsKiller.UI
{
    public class VersionText : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private string format = "v{0}";


        private void OnValidate()
        {
            UpdateText();
        }


        private void Start()
        {
            UpdateText();
        }


        private void UpdateText()
        {
            text.text = string.Format(format, Application.version);
        }
    }
}