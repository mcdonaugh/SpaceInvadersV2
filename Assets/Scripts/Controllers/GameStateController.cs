using UnityEngine;

namespace SpaceInvadersV2.Controllers
{
    public class GameStateController : MonoBehaviour
    {
        [SerializeField] private float _gameBounds = 2.125f;
        [SerializeField] private InvaderGridController _invaderGridController;
        private void Start()
        {
            StartGame();
        }
        private void StartGame()
        {
            _invaderGridController.GenerateInvaders();
            _invaderGridController.SetMoveBounds(_gameBounds);
        }
    }    
}