using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DotsKiller.UI
{
    public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [SerializeField] private TabsGroup tabsGroup;
        [SerializeField] public Image background;
        [SerializeField] private GameObject notificationVisual;
        [SerializeField] private TMP_Text notificationAmountText;


        private void Start()
        {
            tabsGroup.Subscribe(this);
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            tabsGroup.OnTabSelected(this);
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            tabsGroup.OnTabClick(this);
        }


        public void OnPointerExit(PointerEventData eventData)
        {
            tabsGroup.OnTabExit(this);
        }


        // No notifications for now
        public void ShowNotification(int amount)
        {
            return;
            
            notificationAmountText.text = amount.ToString();
            notificationVisual.SetActive(true);
        }


        public void HideNotification()
        {
            return;
            
            notificationVisual.SetActive(false);
        }
    }
}