using System;
using UnityEngine;

namespace DotsKiller.Utility
{
    [Serializable]
    public class Range : ISerializationCallbackReceiver
    {
        public enum RangeFormat
        {
            MinMax,
            StartEnd,
        }
        
        [field: SerializeField] public float A { get; set; }
        [field: SerializeField] public float B { get; set; }
        [field: SerializeField] public RangeFormat Format { get; set; }


        public float Random()
        {
            return UnityEngine.Random.Range(A, B);
        }


        public float Lerp(float t)
        {
            return Mathf.Lerp(A, B, t);
        }


        public float InverseLerp(float value)
        {
            return Mathf.InverseLerp(A, B, value);
        }
        
        
        public void OnBeforeSerialize()
        {
            if (Format == RangeFormat.MinMax)
            {
                if (A > B)
                {
                    Debug.LogWarning($"{nameof(A)} = {A} cannot be bigger than {nameof(B)} = {B}");
                }
            }
        }


        public void OnAfterDeserialize() { }
    }
}