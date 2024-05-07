using System;
using System.Collections;
using SpaceInvadersV2.Data;
using UnityEngine;

namespace SpaceInvadersV2.Controllers
{
    public class InvaderController : MonoBehaviour
    {
        public int InvaderRow => _invaderRow;
        public int InvaderCol => _invaderCol;
        public bool ShootStatus => _canShoot; 
        public event Action<InvaderController> OnInvaderDestroyed;
        [SerializeField] private InvaderData[] _invaderData;
        [SerializeField] private AudioClip _invaderDeathAudio;
        private ProjectilePoolController _projectilePoolController;
        private AudioSource _audio;
        private int _invaderIndex;
        private int _invaderRow;
        private int _invaderCol;
        private int _spriteIndex;
        private bool _canShoot = false;
        
        private void Awake()
        {
            _audio = GetComponent<AudioSource>();
        }

        private void Update()
        {
            Shoot();
        }

        private void OnMouseDown()
        {
            DestroyInvader();
            OnInvaderDestroyed?.Invoke(this);
        }
        
        public void SetShootStatus(bool shootStatus)
        {
            _canShoot = shootStatus;
        }

        public void Shoot()
        {
            if (_canShoot)
            {
                _projectilePoolController.GetProjectile(transform.position, -1);
            }
        }

        public void SetProjectilePoolController(ProjectilePoolController projectilePoolController)
        {
            _projectilePoolController = projectilePoolController;
        }

        public void SetCoordinates(int invaderRow, int invaderCol)
        {
            _invaderRow = invaderRow;
            _invaderCol = invaderCol;
        }

        public void DestroyInvader()
        {
            StartCoroutine(PlayInvaderDeathAnimation());
        }

        private IEnumerator PlayInvaderDeathAnimation()
        {
            _spriteIndex = 2;
            ChangeSprite(_spriteIndex);
            _audio.clip = _invaderDeathAudio;
            _audio.Play();
            yield return new WaitForSeconds(.4f);
            gameObject.SetActive(false);
        }

        public void SetInvader(int invaderIndex)
        {
            _invaderIndex = invaderIndex;
            _spriteIndex = 0;
            ChangeSprite(_spriteIndex);
        }

        public void ChangeSprite(int spriteIndex)
        {
            GetComponent<SpriteRenderer>().sprite = _invaderData[_invaderIndex].Sprites[spriteIndex];  
        }

        public void PlayMoveAnimation()
        {
            if (_spriteIndex == 1)
            {
                _spriteIndex = 0;
            }
            else if (_spriteIndex == 0)
            {
                _spriteIndex = 1;
            }

            ChangeSprite(_spriteIndex);
        }
    }    
}
