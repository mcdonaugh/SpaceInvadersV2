using System;
using UnityEngine;

namespace SpaceInvadersV2.Controllers
{
    public class InvaderController : MonoBehaviour
    {
        public int InvaderRow => _invaderRow;
        public int InvaderCol => _invaderCol;
        public bool ShootStatus => _canShoot; 

        [SerializeField] private Sprite[] _sprites;
        public event Action<InvaderController> OnInvaderDestroyed;
        private int _invaderRow;
        private int _invaderCol;
        private bool _canShoot = false;
        
        private void Awake()
        {
            Debug.Log(_sprites.Length);
        }
        
        private void OnMouseDown()
        {
            OnInvaderDestroyed?.Invoke(this);
        }
        
        public void SetShootStatus(bool shootStatus)
        {
            _canShoot = shootStatus;
        }

        public void SetCoordinates(int invaderRow, int invaderCol)
        {
            _invaderRow = invaderRow;
            _invaderCol = invaderCol;
        }

        public void DestroyInvader()
        {
            gameObject.SetActive(false);
        }

        public void SetSprite(int spriteIndex)
        {
            GetComponent<SpriteRenderer>().sprite = _sprites[spriteIndex]; 
        }

    }    
}
