using System;
using Boosters;
using UnityEngine;
using Weapons;

namespace Player
{
    public class PlayerAttackController: MonoBehaviour, IBoosterClient
    {
        public static Action<WeaponType, bool> UseWeapon; //true == firstWeapon | false == secondWeapon
        
        [SerializeField] private Weapon firstWeapon;
        [SerializeField] private Weapon secondWeapon;

        [SerializeField] private LayerMask targetMask;
    
        private PlayerInput _input;
        private Camera _camera;


        public BoosterType BoosterType { get; } = BoosterType.WeaponBooster;
        public float Booster { get; set; } = 1;

        private void Awake()
        {
            _input = new PlayerInput();
            _camera = Camera.main;
        }

        private void OnEnable()
        {
            _input.Enable();
        }

        private void OnDisable()
        {
            _input.Disable();
        }

        private void Update()
        {
            if (_input.Base.Shoot1.ReadValue<float>() > .1f)
                Attack(firstWeapon, true);
        
            if (_input.Base.Shoot2.ReadValue<float>() > .1f)
                Attack(secondWeapon, false);
        }

        private void Attack(Weapon weapon, bool isFirstWeapon)
        {
            if (weapon.Attack(CalculateTargetPoint(), Booster))
            {
                UseWeapon?.Invoke(weapon.Type, isFirstWeapon);
            }
        }

        private Vector3 CalculateTargetPoint()
        {
            Transform rayPoint = _camera.transform;
            
            Ray ray = new Ray(rayPoint.position, rayPoint.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, targetMask))
                return hit.point;
            
            return ray.GetPoint(50);
        }
    }
}