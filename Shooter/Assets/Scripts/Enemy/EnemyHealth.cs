using System;
using UnityEngine;
using Weapons;

namespace Enemy
{
    public class EnemyHealth : MonoBehaviour, IWeaponTarget
    {
        public Action HealthUpdated;
        public bool Dead { get; private set; }
        [field: SerializeField] public float Health { get; private set; } = 100;
        public float MaxHealth { get; private set; }

        private void Start()
        {
            MaxHealth = Health;
        }

        public void TakeDamage(float damage)
        {
            if(Dead) return;
        
            Health -= damage;
            Health = Math.Clamp(Health, 0, MaxHealth);

            if (Health <= 0)
                Death();
            
            HealthUpdated?.Invoke();
        }
    
        private void Death()
        {
            Dead = true;
        }
    }
}
