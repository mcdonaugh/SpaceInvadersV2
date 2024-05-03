using System;
using UnityEngine;

namespace SpaceInvadersV2.Controllers
{
    public class InvaderController : MonoBehaviour
    {
        public event Action<InvaderController> OnInvaderDestroyed;
        public int _invaderRow;
        public int _invaderCol;
        public bool _canShoot = false;
        
        private void Start()
        {
            EnableShoot();
        }
        private void OnMouseDown()
        {
            OnInvaderDestroyed?.Invoke(this);
        }
        
        private void EnableShoot()
        {
            if (_canShoot)
            {
                Debug.Log($"Invader{_invaderRow},{_invaderCol} is Shooting"); 
            }
        }
    }    
}
