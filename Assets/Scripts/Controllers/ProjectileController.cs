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
        [SerializeField] private float _projectileSpeed = 1f;

        private void Awake()
        {
            StartCoroutine(AnimateSprite(_projectileIndex));
        }

        public void SetProjectileType(int projectileIndex)
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

        public IEnumerator MoveProjectile(int direction)
        {
            transform.position += new Vector3(0,direction,0) * _projectileSpeed * Time.deltaTime;
            yield return new WaitForSeconds(2);
            gameObject.SetActive(false);               
        }
    }    
}
