using UnityEngine;

namespace Enemy
{
    public class Enemy: MonoBehaviour
    {
        [SerializeField] private EnemyHealth enemyHealth;
        
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Rigidbody[] rigidbodies;
        [SerializeField] private Collider[] colliders;
        [SerializeField] private Collider mainCollider;

        [SerializeField] private Color maxHealthColor = Color.green;
        [SerializeField] private Color averageHealthColor = Color.yellow;
        [SerializeField] private Color minHealthColor = Color.red;
        
        private Material _material;

        private void Awake()
        {
            
            _material = meshRenderer.materials[0];
            _material.color = CalculateColor();
        }

        private void OnEnable()
        {
            enemyHealth.HealthUpdated += SetEnemy;
        }

        private void OnDisable()
        {
            enemyHealth.HealthUpdated += SetEnemy;
        }

        private void SetEnemy()
        {
            _material.color = CalculateColor();
            
            if (enemyHealth.Dead)
            {
                mainCollider.enabled = false;
                
                foreach (Rigidbody rb in rigidbodies)
                    rb.isKinematic = false;

                foreach (Collider col in colliders)
                    col.enabled = true;
            }
        }


        private Color CalculateColor()
        {
            float healthDelta = enemyHealth.Health / enemyHealth.MaxHealth;

            bool firstColorPair  = healthDelta > .5f;

            Color color1 = firstColorPair ? averageHealthColor : minHealthColor;
            Color color2 = firstColorPair ? maxHealthColor : averageHealthColor;

            float difference = firstColorPair ? 0.5f : 0;
            float t = (healthDelta - difference) * 2f;

            Color newColor = Color.Lerp(color1, color2, t);

            return newColor;
        }
    }
}