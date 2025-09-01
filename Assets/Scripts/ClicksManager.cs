using System;
using DotsKiller.RegularUpgrading;
using NaughtyAttributes;
using UnityEngine;
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
        
        private enum ClickType
        {
            Normal,
            AoeByDefault,
            WithAssist,
        }
        
        private readonly Collider2D[] _queryResults = new Collider2D[64]; // maybe need more than 64
        private ContactFilter2D _dotsContactFilter;
        
        private RegularUpgrades _regularUpgrades;

        public float DefaultRadius => defaultClickRadius;
        public float UpgradedRadius => upgradedRadius;


        [Inject]
        public void Initialize(RegularUpgrades regularUpgrades)
        {
            _regularUpgrades = regularUpgrades;
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


        private void Update()
        {
            if (Pointer.current.press.wasPressedThisFrame)
            {
                Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Pointer.current.position.value);

                if (_regularUpgrades.AoeClicks)
                {
                    PerformAoeClick(clickPosition);
                }
                else
                {
                    PerformRegularClick(clickPosition);
                }
            }
        }


        private void PerformAoeClick(Vector2 clickPosition)
        {
            int size = Physics2D.OverlapCircle(clickPosition, upgradedRadius, _dotsContactFilter, _queryResults);
                    
            for (int i = 0; i < size; i++)
            {
                ClickOnTarget(_queryResults[i]);
            }
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

                    ClickOnTarget(hit.transform);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}