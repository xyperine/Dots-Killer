using AYellowpaper;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DotsKiller.UI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private Image fillImage;
        [SerializeField] private InterfaceReference<IProgressProvider> progressProviderReference;

        private IProgressProvider _progressProvider;

        private void Awake()
        {
            _progressProvider = progressProviderReference.Value;
        }


        private void Update()
        {
            fillImage.fillAmount = _progressProvider.Progress;
            text.text = _progressProvider.Progress.ToString("P2");
        }
    }
}