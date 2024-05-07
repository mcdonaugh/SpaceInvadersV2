using System.Collections;
using System.Collections.Generic;
using SpaceInvadersV2.Data;
using UnityEngine;

namespace SpaceInvadersV2.Controllers
{
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField] private ProjectileData[] _projectileData;
        [SerializeField] private int _projectileIndex;
        [SerializeField] private float _animationPlaySpeed = .05f;

        private void Awake()
        {
            StartCoroutine(AnimateSprite(_projectileIndex));
        }

        private void SetProjectileType(int projectileIndex)
        {
            _projectileIndex = projectileIndex;
        }

        public IEnumerator AnimateSprite(int projectileIndex)
        {
            while (this.isActiveAndEnabled)
            {
                for (int i = 0; i < _projectileData[_projectileIndex].Sprites.Length; i++)
                {
                    GetComponent<SpriteRenderer>().sprite = _projectileData[_projectileIndex].Sprites[i];
                    yield return new WaitForSeconds(_animationPlaySpeed);
                } 
            }
            
        }
    }    
}
