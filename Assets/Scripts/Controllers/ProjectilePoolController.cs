using Unity.Mathematics;
using UnityEngine;

namespace SpaceInvadersV2.Controllers
{
    public class ProjectilePoolController : MonoBehaviour
    {
        public ProjectileController[] ProjectilePool => _projectilePool;
        [SerializeField] private ProjectileController _projectileController;
        private ProjectileController[] _projectilePool;

        private void Awake()
        {
            _projectilePool = new ProjectileController[5];
            CreatePool();
        }
        private void CreatePool()
        {
            for (int i = 0; i < _projectilePool.Length; i++)
            {
                ProjectileController newProjectile = Instantiate(_projectileController, transform.position, quaternion.identity);
                newProjectile.gameObject.SetActive(false);
                _projectilePool[i] = newProjectile;
            }
        }

        public ProjectileController GetProjectile(Vector3 projectileOrigin, int direction)
        {
            foreach (var projectile in _projectilePool)
            {
                if(!projectile.gameObject.activeInHierarchy)
                {
                    projectile.transform.position = projectileOrigin;
                    projectile.gameObject.SetActive(true);
                    projectile.MoveProjectile(direction);
                    return projectile;
                }
            }
            return null; 
        }
    }    
}
