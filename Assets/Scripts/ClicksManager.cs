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
        [SerializeField, Tooltip("When missed will still count if clicked in a certain radius around clickable")] 
        private bool clickAssist = true;
        [SerializeField, ShowIf(nameof(clickAssist))] private float assistRadius = 0.2f;
        
        private readonly Collider2D[] _queryResults = new Collider2D[64]; // maybe need more than 64
        
        private RegularUpgrades _regularUpgrades;


        [Inject]
        public void Initialize(RegularUpgrades regularUpgrades)
        {
            _regularUpgrades = regularUpgrades;
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
            ContactFilter2D filter = new ContactFilter2D
            {
                useLayerMask = true,
                layerMask = dotsMask,
                useTriggers = true,
            };
            int size = Physics2D.OverlapCircle(clickPosition, upgradedRadius, filter, _queryResults);
                    
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
            if (clickAssist)
            {
                ContactFilter2D filter = new ContactFilter2D
                {
                    useLayerMask = true,
                    layerMask = dotsMask,
                    useTriggers = true,
                };
                int size = Physics2D.OverlapCircle(clickPosition, assistRadius, filter, _queryResults);
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
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero, 100f, dotsMask.value);
                if (!hit)
                {
                    return;
                }

                ClickOnTarget(hit.transform);
            }
        }
    }
}