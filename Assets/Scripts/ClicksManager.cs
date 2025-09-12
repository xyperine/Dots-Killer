using System;
using System.Collections.Generic;
using DotsKiller.AudioSystem;
using DotsKiller.RegularUpgrading;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

namespace DotsKiller
{
    public class ClicksManager : MonoBehaviour
    {
        [SerializeField] private float upgradedRadius = 0.5f;
        [SerializeField] private LayerMask dotsMask = 0;
        [SerializeField] private ClickType defaultClickType = ClickType.AoeByDefault;
        [SerializeField, ShowIf(nameof(defaultClickType), ClickType.AoeByDefault)] 
        private float defaultClickRadius = 0.15f;
        [SerializeField, ShowIf(nameof(defaultClickType), ClickType.WithAssist),
         Tooltip("When missed will still count if clicked in a certain radius around clickable")] 
        private float assistRadius = 0.2f;
        [SerializeField] private bool debugClicks;
        [SerializeField, ShowIf(nameof(debugClicks))] private Transform debugCircle;

        private enum ClickType
        {
            Normal,
            AoeByDefault,
            WithAssist,
        }
        
        private readonly Collider2D[] _queryResults = new Collider2D[64]; // maybe need more than 64
        private ContactFilter2D _dotsContactFilter;
        
        private RegularUpgrades _regularUpgrades;
        private AudioManager _audioManager;

        public float DefaultRadius => defaultClickRadius;
        public float UpgradedRadius => upgradedRadius;


        [Inject]
        public void Initialize(RegularUpgrades regularUpgrades, AudioManager audioManager)
        {
            _regularUpgrades = regularUpgrades;
            _audioManager = audioManager;
        }


        private void Awake()
        {
            _dotsContactFilter = new ContactFilter2D
            {
                useLayerMask = true,
                layerMask = dotsMask,
                useTriggers = true,
            };
        }


        private void Start()
        {
            debugCircle.gameObject.SetActive(debugClicks);
        }


        private void Update()
        {
            if (!Pointer.current.press.wasPressedThisFrame)
            {
                return;
            }

            if (EventSystem.current.IsPointerOverGameObject())
            {
                foreach (RaycastResult result in GetAllElementsUnderPointer())
                {
                    if (!result.gameObject.CompareTag("Field"))
                    {
                        return;
                    }
                }
            }

            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Pointer.current.position.value);
                
            if (debugClicks)
            {
                debugCircle.gameObject.SetActive(true);

                debugCircle.position = clickPosition;
                debugCircle.localScale = Vector3.one * defaultClickRadius * 2 * 5; // sprite size specific
            }
            else
            {
                debugCircle.gameObject.SetActive(false);
            }
                
            Array.Clear(_queryResults, 0, 64);

            if (_regularUpgrades.AoeClicks)
            {
                PerformAoeClick(clickPosition);
            }
            else
            {
                PerformRegularClick(clickPosition);
            }
        }


        private void PerformAoeClick(Vector2 clickPosition)
        {
            int size = Physics2D.OverlapCircle(clickPosition, upgradedRadius, _dotsContactFilter, _queryResults);

            if (size >= 1)
            {
                PlayAudio();
            }
                    
            for (int i = 0; i < size; i++)
            {
                ClickOnTarget(_queryResults[i]);
            }
        }


        private List<RaycastResult> GetAllElementsUnderPointer()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                position = Pointer.current.position.value,
            };
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);
            return raycastResults;
        }


        private void PlayAudio()
        {
            _audioManager.PlaySound(AudioID.Clickable);
        }
        

        private void ClickOnTarget(Component component)
        {
            if (component.TryGetComponent(out Clickable clickable))
            {
                clickable.Click();
            }
        }


        private void PerformRegularClick(Vector2 clickPosition)
        {
            switch (defaultClickType)
            {
                case ClickType.AoeByDefault:
                {
                    int size = Physics2D.OverlapCircle(clickPosition, defaultClickRadius, _dotsContactFilter, _queryResults);
                    if (size < 1)
                    {
                        return;
                    }
                    
                    PlayAudio();

                    foreach (Collider2D result in _queryResults)
                    {
                        if (!result)
                        {
                            continue;
                        }
                        
                        ClickOnTarget(result);
                    }

                    break;
                }
                case ClickType.WithAssist:
                {
                    int size = Physics2D.OverlapCircle(clickPosition, assistRadius, _dotsContactFilter, _queryResults);
                    if (size < 1)
                    {
                        return;
                    }
                    
                    PlayAudio();

                    Collider2D closestResult = _queryResults[0];
                    float closesPosition = float.PositiveInfinity;
                    for (int i = 0; i < _queryResults.Length; i++)
                    {
                        Collider2D result = _queryResults[i];
                        if (!result)
                        {
                            continue;
                        }

                        float distance = Vector2.Distance(result.transform.position, clickPosition);
                        if (distance < closesPosition)
                        {
                            closestResult = result;
                            closesPosition = distance;
                        }
                    }

                    ClickOnTarget(closestResult);
                    break;
                }
                case ClickType.Normal:
                {
                    RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero, 100f, dotsMask.value);
                    if (!hit)
                    {
                        return;
                    }
                    
                    PlayAudio();

                    ClickOnTarget(hit.transform);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}