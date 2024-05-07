using UnityEngine;

namespace SpaceInvadersV2.Controllers
{
    public class GameStateController : MonoBehaviour
    {
        [SerializeField] InvaderGridController _invaderGridController;
        [SerializeField] private float _gameBounds;
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