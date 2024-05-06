using UnityEngine;

namespace SpaceInvadersV2.Controllers
{
    public class GameStateController : MonoBehaviour
    {
        [SerializeField] InvaderGridController _invaderGridController;
        private void Start()
        {
            StartGame();
        }
        private void StartGame()
        {
            _invaderGridController.GenerateInvaders();
        }
    }    
}