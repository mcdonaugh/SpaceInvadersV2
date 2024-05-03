using System;
using UnityEngine;

namespace SpaceInvadersV2.Controllers
{
    public class InvaderController : MonoBehaviour
    {
        public event Action<InvaderController> OnInvaderDestroyed;
        public int invaderRow;
        public int invaderCol;
        private void Update()
        {
            OnMouseEnter();
        }
        private void OnMouseEnter()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnInvaderDestroyed?.Invoke(this);
            }
        }
    }

    
}