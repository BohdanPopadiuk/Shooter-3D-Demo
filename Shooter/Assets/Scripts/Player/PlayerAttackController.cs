using Boosters;
using UnityEngine;
using Weapons;

namespace Player
{
    public class PlayerAttackController: MonoBehaviour, IBoosterClient
    {
        [SerializeField] private Weapon firstWeapon;
        [SerializeField] private Weapon secondWeapon;
    
        private PlayerInput _input;

        public BoosterType BoosterType { get; } = BoosterType.WeaponBooster;
        public float Booster { get; set; } = 1;


        private void Awake()
        {
            _input = new PlayerInput();
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
            {
                firstWeapon.Attack();
            }
        
            if (_input.Base.Shoot2.ReadValue<float>() > .1f)
            {
                secondWeapon.Attack();
            }
        }
    }
}