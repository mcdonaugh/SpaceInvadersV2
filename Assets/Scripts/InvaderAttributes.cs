
using UnityEngine;

namespace SpaceInvadersV2.Data
{
    [CreateAssetMenu(fileName = "New Invader", menuName = "Create Data/New Invader")]
    public class InvaderData : ScriptableObject
    {
        public Sprite[] Sprites => _sprites; 
        public int PointValue => _pointValue;  

        [Header("Sprites")]
        [SerializeField] private Sprite[] _sprites;

        [Header("Gameplay")]
        [SerializeField] private int _pointValue;
    }    
}