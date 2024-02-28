using System;
using System.Collections;
using UnityEngine;

namespace Weapons
{
    public abstract class Weapon: MonoBehaviour
    {
        protected Action AttackRealization;
        //[SerializeField] private Rigidbody rb;
        //[SerializeField] private Collider col;
        [SerializeField] private float damage;
        [SerializeField] protected float attackDuration;
        [field: SerializeField] public WeaponType Type { get; protected set; }

        protected Vector3 TargetPos;
        protected bool AttackLocked = false;
        private bool _canAttack = true;
        private float _damageMultiplier = 1;
        protected float CalculatedDamage => damage * _damageMultiplier;

        public bool Attack(Vector3 targetPos, float multiplier = 1)
        {
            if(!_canAttack || AttackLocked)
                return false;
            
            _damageMultiplier = multiplier;
            TargetPos = targetPos;
            
            StartCoroutine(AttackDuration());
            AttackRealization?.Invoke();
            
            return true;
        }
        
        /*public void Take(Transform parent)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            col.enabled = false;
            
            transform.parent = parent;
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
        }

        public void Put(Vector3 newPosition)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            col.enabled = true;

            transform.position = newPosition;
            transform.parent = null;
        }*/

        private IEnumerator AttackDuration()
        {
            _canAttack = false;
            yield return new WaitForSeconds(attackDuration);
            _canAttack = true;
        }
    }
}