using Boosters;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerStateIndicator : MonoBehaviour
    {
        [SerializeField] private PlayerStateIcon[] playerStateIcons;
        [SerializeField] private Image playerStateImage;
        [SerializeField] private BoosterIndicator[] boosterIndicators;
        private void OnEnable()
        {
            PlayerStateManager.NewMoveState += ChangePlayerIcon;
            BoosterController.NewBoosterActivated += ShowActiveBooster;
        }

        private void OnDisable()
        {
            PlayerStateManager.NewMoveState -= ChangePlayerIcon;
            BoosterController.NewBoosterActivated += ShowActiveBooster;
        }
        
        private void ChangePlayerIcon(MoveState moveState)
        {
            foreach (var playerStateIcon in playerStateIcons)
            {
                if (moveState == playerStateIcon.moveState)
                {
                    playerStateImage.sprite = playerStateIcon.icon;
                    break;
                }
            }
        }

        private void ShowActiveBooster(BoosterController boosterController)
        {
            foreach (var indicator in boosterIndicators)
            {
                if (indicator.GetBoosterType == boosterController.GetBoosterType)
                {
                    indicator.Run(boosterController.BoosterDuration);
                    
                    return;
                }
            }
        }
    }
}
