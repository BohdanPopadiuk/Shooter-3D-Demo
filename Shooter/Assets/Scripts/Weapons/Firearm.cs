using System.Collections;
using ObjectPool;
using UnityEngine;

namespace Weapons
{
    public class Firearm: Weapon
    {
        [SerializeField] private int maxMagazineCapacity = 30;
        [SerializeField] private float reloadTime = 2;
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform bulletSpawnPoint;
        [SerializeField] private ParticleSystem flashParticles;
        
        private ObjectPool<Bullet> _bulletPool;
        
        private int _magazineCapacity;

        private void Awake()
        {
            _bulletPool = new ObjectPool<Bullet>(bulletPrefab, 5, 
                (bullet) => { bullet.Initialize(_bulletPool); });
            
            _magazineCapacity = maxMagazineCapacity;
            AttackRealization = Shoot;
        }

        protected virtual void Shoot()
        {
            bulletSpawnPoint.LookAt(TargetPos);
            
            Bullet bullet = _bulletPool.Get();
            bullet.SetBullet(bulletSpawnPoint, CalculatedDamage, _bulletPool);

            if (flashParticles != null)
                flashParticles.Play();
            
            _magazineCapacity--;
            
            if (_magazineCapacity <= 0)
            {
                StartCoroutine(Reload());
            }
        }

        private IEnumerator Reload()
        {
            //ToDo (RELOAD ANIMATION) still has no animation, I set (reloadTime 0)
            AttackLocked = true;
            
            yield return new WaitForSeconds(reloadTime);
            
            _magazineCapacity = maxMagazineCapacity;
            AttackLocked = false;
        }
    }
}