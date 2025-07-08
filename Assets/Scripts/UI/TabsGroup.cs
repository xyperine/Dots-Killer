using System.Collections.Generic;
using UnityEngine;

namespace DotsKiller.UI
{
    public class TabsGroup : MonoBehaviour
    {
        [SerializeField] private List<TabButton> tabs;
        [SerializeField] private List<GameObject> pages;
        [SerializeField] private AudioSource audioSource;

        [SerializeField] private Color defaultColor;
        [SerializeField] private Color selectColor;
        [SerializeField] private Color clickColor;

        private readonly List<TabButton> _clickedTabs = new List<TabButton>();

        private readonly Dictionary<int, int> _newItems = new Dictionary<int, int>();


        private void Start()
        {
            for (int i = 0; i < tabs.Count; i++)
            {
                _newItems.Add(i, 0);
            }
            
            Close();
        }


        private void Close()
        {
            ResetTabs();
            for (int i = 0; i < tabs.Count; i++)
            {
                pages[i].SetActive(_clickedTabs.Contains(tabs[i]));
            }
        }


        private void ResetTabs()
        {
            foreach (TabButton tabButton in tabs)
            {
                if (_clickedTabs.Contains(tabButton))
                {
                    continue;
                }

                tabButton.background.color = defaultColor;
            }
        }


        public void AddItem(int tabIndex)
        {
            if (_clickedTabs.Exists(t => t.transform.GetSiblingIndex() == tabIndex))
            {
                return;
            }
            
            if (!_newItems.TryAdd(tabIndex, 1))
            {
                _newItems[tabIndex]++;
            }
        }


        private void Update()
        {
            for (int i = 0; i < _newItems.Count; i++)
            {
                if (_newItems[i] < 1)
                {
                    tabs[i].HideNotification();
                    continue;
                }
                
                tabs[i].ShowNotification(_newItems[i]);
            }
        }


        public void Subscribe(TabButton tabButton)
        {
            if (tabs.Contains(tabButton))
            {
                return;
            }
            
            tabs.Add(tabButton);
        }


        public void OnTabSelected(TabButton tabButton)
        {
            ResetTabs();

            if (_clickedTabs.Contains(tabButton))
            {
                return;
            }
            
            tabButton.background.color = selectColor;
        }


        public void OnTabClick(TabButton tabButton)
        {
            audioSource.Play();
            
            if (_clickedTabs.Contains(tabButton))
            {
                ClosePage(tabButton);
                return;
            }

            int index = tabButton.transform.GetSiblingIndex();
            _newItems[index] = 0;
            tabButton.HideNotification();

            pages[index].transform.SetAsLastSibling();
            pages[index].SetActive(true);

            _clickedTabs.Add(tabButton);
            ResetTabs();
            tabButton.background.color = clickColor;
        }


        private void ClosePage(TabButton tabButton)
        {
            _clickedTabs.Remove(tabButton);
            Close();
        }
        

        public void ClosePage(GameObject pageObject)
        {
            TabButton matchingTabButton = tabs[pages.IndexOf(pageObject)];
            ClosePage(matchingTabButton);
        }


        public void OnTabExit(TabButton tabButton)
        {
            ResetTabs();
        }
    }
}