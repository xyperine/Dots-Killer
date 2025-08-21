using System.Collections.Generic;
using UnityEngine;

namespace DotsKiller
{
    public class PurchaseLocks : MonoBehaviour
    {
        private readonly List<PurchaseLock> _purchaseLocks = new List<PurchaseLock>();


        public void Register(PurchaseLock purchaseLock)
        {
            if (_purchaseLocks.Contains(purchaseLock))
            {
                return;
            }

            _purchaseLocks.Add(purchaseLock);
        }


        private void Awake()
        {
            foreach (PurchaseLock purchaseLock in FindObjectsByType<PurchaseLock>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                Register(purchaseLock);
                purchaseLock.Setup();
            }
        }


        private void Start()
        {
            foreach (PurchaseLock purchaseLock in _purchaseLocks)
            {
                purchaseLock.Load();
            }
        }
    }
}