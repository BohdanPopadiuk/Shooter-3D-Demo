using UnityEngine;

namespace Weapons
{
    public class BulletSplashZone: MonoBehaviour
    {
        [SerializeField] private Collider col;
        
        private float _damage;

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("BulletTarget"))
            {
                other.GetComponent<IWeaponTarget>()?.TakeDamage(_damage);
            }
        }
        
        public void Enable(float damage)
        {
            _damage = damage;
            col.enabled = true;
        }
        
        public void Disable()
        {
            col.enabled = false;
        }
    }
}