using Player;
using UnityEngine;

namespace Level
{
    public class SafetyWall : MonoBehaviour
    {
        [SerializeField] private Collider col;
        private bool _playerStayInTrigger;

        private void OnValidate()
        {
            if (col == null)
                col = GetComponent<Collider>();
        }

        private void OnEnable()
        {
            PlayerStateManager.NewMoveState += DisableWall;
        }

        private void OnDisable()
        {
            PlayerStateManager.NewMoveState -= DisableWall;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                _playerStayInTrigger = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _playerStayInTrigger = false;
                col.isTrigger = false;
            }
        }

        private void DisableWall(MoveState moveState)
        {
            if(_playerStayInTrigger) return;
            col.isTrigger = (moveState == MoveState.InAir);
        }
    }
}
