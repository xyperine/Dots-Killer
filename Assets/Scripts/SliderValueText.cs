using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DotsKiller
{
    public class SliderValueText : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_Text text;

        private string _format;


        private void Awake()
        {
            _format = text.text;
        }


        private void Update()
        {
            text.text = string.Format(_format, slider.value);
        }
    }
}