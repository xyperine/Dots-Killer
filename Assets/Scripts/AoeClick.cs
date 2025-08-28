using DotsKiller.RegularUpgrading;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace DotsKiller
{
    public class AoeClick : MonoBehaviour
    {
        [SerializeField] private float radius = 1f;
        [SerializeField] private LayerMask dotsMask = 0;
        
        private RegularUpgrades _regularUpgrades;
        private Collider2D[] _queryResults = new Collider2D[64]; // maybe need more than 64


        [Inject]
        public void Initialize(RegularUpgrades regularUpgrades)
        {
            _regularUpgrades = regularUpgrades;
        }
        

        private void Update()
        {
            if (!_regularUpgrades.AoeClicks)
            {
                return;
            }
            
            if (Pointer.current.press.wasPressedThisFrame)
            {
                Vector2 center = Camera.main.ScreenToWorldPoint(Pointer.current.position.value);
                ContactFilter2D filter = new ContactFilter2D
                {
                    useLayerMask = true,
                    layerMask = dotsMask,
                    useTriggers = true,
                };
                int size = Physics2D.OverlapCircle(center, radius, filter, _queryResults);
                for (int i = 0; i < size; i++)
                {
                    if (_queryResults[i].TryGetComponent(out Clickable clickable))
                    {
                        clickable.Click();
                    }
                }
            }
        }
    }
}