using System;
using UnityEngine;
using Weapons;

namespace Enemy
{
    public class EnemyHealth : MonoBehaviour, IWeaponTarget
    {
        public bool Dead { get; private set; }
        [field: SerializeField] public float Health { get; private set; } = 100;

        [SerializeField] private MeshRenderer meshRenderer;

        [SerializeField] private Color maxHealthColor = Color.green;
        [SerializeField] private Color averageHealthColor = Color.yellow;
        [SerializeField] private Color minHealthColor = Color.red;

        private float _maxHealth;
        private Material _material;

        private void Start()
        {
            _maxHealth = Health;
        
            _material = meshRenderer.materials[0];
            _material.color = CalculateColor();
        }

        public void TakeDamage(float damage)
        {
            if(Dead) return;
        
            Health -= damage;
            Health = Math.Clamp(Health, 0, _maxHealth);

            _material.color = CalculateColor();
        
            if (Health <= 0)
            {
                Death();
            }
        }
    
        private void Death()
        {
            Dead = true;
        }

        private Color CalculateColor()
        {
            float healthDelta = Health / _maxHealth;

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
