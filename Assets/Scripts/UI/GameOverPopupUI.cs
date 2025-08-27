using UnityEngine;

namespace DotsKiller.UI
{
    public class GameOverPopupUI : MonoBehaviour
    {
        [SerializeField] private GameObject popupObject;
        

        public void Show()
        {
            popupObject.SetActive(true);
        }


        public void OnOK()
        {
            popupObject.SetActive(false);
        }
    }
}