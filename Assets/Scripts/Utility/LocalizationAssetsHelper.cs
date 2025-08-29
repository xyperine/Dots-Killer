using System;
using System.Collections;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DotsKiller.Utility
{
    public class LocalizationAssetsHelper : MonoBehaviour
    {
        public void GetLocalizedAsset<TResult>(AsyncOperationHandle<TResult> op, Action<TResult> onSuccess)
        {
            StartCoroutine(GetAsset());
            IEnumerator GetAsset()
            {
                yield return new WaitUntil(() => op.IsDone);
                
                switch (op.Status)
                {
                    case AsyncOperationStatus.None:
                        break;
                    case AsyncOperationStatus.Succeeded:
                        onSuccess.Invoke(op.Result);
                        break;
                    case AsyncOperationStatus.Failed:
                        Debug.Log($"Failed to get asset of type {typeof(TResult)}");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}