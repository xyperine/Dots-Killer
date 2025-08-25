using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;

namespace DotsKiller.UI
{
    public class LanguageButtons : MonoBehaviour
    {
        [SerializeField, SerializedDictionary("Code", "Button")]
        private SerializedDictionary<string, Button> buttons;
        [SerializeField] private Transform activeLanguageButtonFrame;


        public void SetActive(string code)
        {
            if (buttons.TryGetValue(code, out Button button))
            {
                activeLanguageButtonFrame.position = button.transform.position;
            }
            else
            {
                throw new KeyNotFoundException($"Key not found: {code}");
            }
        }
    }
}