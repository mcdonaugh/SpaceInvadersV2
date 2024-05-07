using UnityEngine;

namespace SpaceInvadersV2.Data
{
    [CreateAssetMenu(fileName = "New Projectile", menuName = "Create Data/New Projectile")]
    public class ProjectileData : ScriptableObject
    {
        public Sprite[] Sprites => _sprites;

        [Header("Sprites")]
        [SerializeField] private Sprite[] _sprites;
    }    
}
