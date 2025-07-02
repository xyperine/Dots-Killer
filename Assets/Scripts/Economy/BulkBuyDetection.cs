using UnityEngine;

namespace DotsKiller.Economy
{
    public class BulkBuyDetection : MonoBehaviour, IBulkBuyStateProvider
    {
        [SerializeField] private KeyCode hotKey = KeyCode.LeftControl;

        public bool Active { get; private set; }


        private void Update()
        {
            Active = Input.GetKey(hotKey);
        }
    }
}