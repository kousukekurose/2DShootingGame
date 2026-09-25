using UnityEngine;

namespace Game.Bullet
{
    [CreateAssetMenu(fileName = "BulletData", menuName = "Game/Bullet Data")]
    public class BulletData : ScriptableObject
    {
        public Domain.Bullet.BulletType Type;
        public float Speed = 10f;
        public float Damage = 10f;
        public GameObject ViewPrefab;
        public float Lifetime = 5f;
    }
}
