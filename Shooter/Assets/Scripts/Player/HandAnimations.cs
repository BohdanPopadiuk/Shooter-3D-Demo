using DG.Tweening;
using UnityEngine;
using Weapons;

namespace Player
{
    public class HandAnimations : MonoBehaviour
    {
        [SerializeField] private bool isLeftHand;
        [Space(10)]
        [SerializeField] private Transform wrist;
        [SerializeField] private Transform elbow;
        
        [Header("Pistol Shoot Anim")]
        
        [SerializeField] private float pistolShootWristAngle = .2f;
        [SerializeField] private float pistolShootElbowOffset = .2f;
        [SerializeField] private float pistolShootDuration = .2f;
        [SerializeField] private Ease pistolShootEase = Ease.Linear;

        private Vector3 _elbowDefaultPos;
        private Vector3 _wristDefaultAngle;

        void Start()
        {
            _elbowDefaultPos = elbow.localPosition;
            _wristDefaultAngle = wrist.localEulerAngles;
        }

        private void OnEnable()
        {
            PlayerAttackController.UseWeapon += AttackAnimation;
        }

        private void OnDisable()
        {
            PlayerAttackController.UseWeapon -= AttackAnimation;
        }

        private void AttackAnimation(WeaponType weaponType, bool firstWeapon)
        {
            if(isLeftHand != firstWeapon) return;
            
            if (weaponType == WeaponType.Firearm)
                PistolShootAnimation();
        }

        private void PistolShootAnimation()
        {
            float targetX = _elbowDefaultPos.x + pistolShootElbowOffset;
            
            elbow.DOLocalMoveX(targetX, pistolShootDuration)
                .SetEase(pistolShootEase)
                .SetLoops(2, LoopType.Yoyo);
            
            Vector3 targetRotation = _wristDefaultAngle + Vector3.up * pistolShootWristAngle;
            
            wrist.DOLocalRotate(targetRotation, pistolShootDuration)
                .SetEase(pistolShootEase)
                .SetLoops(2, LoopType.Yoyo);
        }
    }
}
