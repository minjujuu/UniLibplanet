using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class Click : MonoBehaviour
    {
        public int Count { get; set; } = 0;

        public void Add()
        {
            Count++;
        }
        
        public void ResetCount()
        {
            Count = 0;
        }
    }

}
