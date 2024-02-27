using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerStateIndicator : MonoBehaviour
    {
        [SerializeField] private PlayerStateIcon[] playerStateIcons;
        [SerializeField] private Image playerStateImage;
        private void OnEnable()
        {
            PlayerStateManager.NewMoveState += ChangePlayerIcon;
        }

        private void OnDisable()
        {
            PlayerStateManager.NewMoveState -= ChangePlayerIcon;
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
    }
}
