using Unity.Mathematics;
using UnityEngine;

namespace SpaceInvadersV2.Controllers
{
    public class GameInstallerController : MonoBehaviour
    {
        [SerializeField] private float _gameBounds = 2.125f;
        [SerializeField] private InvaderGridController _invaderGridController;
        [SerializeField] private ProjectilePoolController _projectilePoolController;

        private void Start()
        {
            _invaderGridController.GenerateInvaders();
            _invaderGridController.SetProjectilePoolController(_projectilePoolController);
            _invaderGridController.SetMoveBounds(_gameBounds);
        }
    }
}