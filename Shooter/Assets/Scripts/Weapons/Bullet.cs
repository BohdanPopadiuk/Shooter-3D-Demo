using System.Collections;
using ObjectPool;
using UnityEngine;

namespace Weapons
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet: MonoBehaviour
    {
        [SerializeField] private Collider col;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float bulletSpeed = 5;
        [SerializeField] private ParticleSystem hitParticles;

        private ObjectPool<Bullet> _pool;
        private float _damage;
        private bool _bulletHit;
        private float HitParticlesDuration => hitParticles.main.duration;
        
        private void OnValidate()
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
                
                rb.interpolation = RigidbodyInterpolation.Extrapolate;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                
                rb.freezeRotation = true;
                rb.useGravity = false;
            }
        }
        
        public void Initialize(ObjectPool<Bullet> pool)
        {
            _pool = pool;
        }

        private void FixedUpdate()
        {
            if(!_bulletHit)
                rb.velocity = transform.forward * bulletSpeed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("BulletTarget"))
            {
                other.GetComponent<IWeaponTarget>()?.TakeDamage(_damage);
            }

            if(!_bulletHit)
                StartCoroutine(BulletHit());
        }

        private IEnumerator BulletHit()
        {
            _bulletHit = true;
            //ToDo playParticles && playSFX
            
            col.enabled = false;
            rb.velocity = Vector3.zero;
            
            hitParticles.Play();

            yield return new WaitForSeconds(HitParticlesDuration);
            _pool.Return(this);
        }

        public void SetBullet(Transform spawnPoint, float damage, ObjectPool<Bullet> pool)
        {
            _bulletHit = false;
            col.enabled = true;
            
            transform.position = spawnPoint.position;
            transform.rotation = Quaternion.LookRotation(spawnPoint.forward);

            _damage = damage;
            
            _pool = pool;
        }
    }
}