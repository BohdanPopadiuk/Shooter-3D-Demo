using System.Collections;
using ObjectPool;
using UnityEngine;

namespace Weapons
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet: MonoBehaviour
    {
        [SerializeField] private SphereCollider col;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float bulletSpeed = 5;
        [SerializeField] private float impactForce = 5;
        [SerializeField] private ParticleSystem hitParticles;
        [SerializeField] private TrailRenderer trailRenderer;
        
        [SerializeField] private GameObject splashZone;

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

        private void OnDisable()
        {
            StopAllCoroutines();
            if (trailRenderer != null)
                trailRenderer.Clear();
        }

        public void Initialize(ObjectPool<Bullet> pool)
        {
            _pool = pool;
            
            if (splashZone != null)
                splashZone.SetActive(false);
        }

        private void FixedUpdate()
        {
            if(!_bulletHit)
                rb.velocity = transform.forward * bulletSpeed;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(!_bulletHit)
                StartCoroutine(Hit());
            
            GameObject colObject = collision.gameObject;
            
            if(colObject.CompareTag("BulletTarget"))
            {
                colObject.GetComponent<IWeaponTarget>()?.TakeDamage(_damage);
            }

            if (!colObject.isStatic)
            {
                Rigidbody otherRb = colObject.GetComponent<Rigidbody>();
                if (otherRb != null)
                {
                    Vector3 direction = -collision.contacts[0].normal;
                    otherRb.AddForce(direction * impactForce);
                }
            }

        }

        private IEnumerator Hit()
        {
            if (splashZone != null)
                splashZone.SetActive(true);
            
            _bulletHit = true;
            
            col.enabled = false;
            rb.velocity = Vector3.zero;
            
            hitParticles.Play();

            float duration = 0.2f;
            
            if (splashZone != null)
            {
                yield return new WaitForSeconds(duration);
                splashZone.SetActive(false);
            }
            
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