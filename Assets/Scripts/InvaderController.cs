using System;
using UnityEngine;

namespace SpaceInvadersV2.Controllers
{
    public class InvaderController : MonoBehaviour
    {
        public int InvaderRow => _invaderRow;
        public int InvaderCol => _invaderCol;
        public bool ShootStatus => _canShoot; 
        public event Action<InvaderController> OnInvaderDestroyed;
        private int _invaderRow;
        private int _invaderCol;
        private bool _canShoot = false;
        
        private void OnMouseDown()
        {
            OnInvaderDestroyed?.Invoke(this);
        }
        
        public void SetShootStatus(bool shootStatus)
        {
            _canShoot = shootStatus;
        }

        public void SetInvaderCoordinates(int invaderRow, int invaderCol)
        {
            _invaderRow = invaderRow;
            _invaderCol = invaderCol;
        }

        public void DestroyInvader()
        {
            gameObject.SetActive(false);
        }

        




        
    }    
}
