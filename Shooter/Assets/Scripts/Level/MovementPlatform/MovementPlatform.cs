using UnityEngine;

namespace Level.MovementPlatform
{
    public class MovementPlatform : MonoBehaviour
    {
        [SerializeField] private Transform pathPointsGroup;

        [Space(10)]
        [SerializeField, Min(0)] private float moveSpeed = 5;
        [SerializeField] private bool drawPath = true;

        [Space(10)]
        [SerializeField] private MoveType moveType;

        private int _moveDirection = 1;
        private int _currentWaypointIndex;
        private Vector3[] _path;
        private Vector3 TargetPoint => _path[_currentWaypointIndex];
        
        
        private void Start()
        {
            _path = GetPath();
            drawPath = false;
            
            pathPointsGroup.gameObject.SetActive(false);
        }

        private void OnDrawGizmos()
        {
            if(!drawPath) return;

            int pathPointsCount = pathPointsGroup.childCount;
            
            if (pathPointsCount >= 2)
            {
                for (int i = 0; i < pathPointsCount - 1; i++)
                {
                    Vector3 linePointA = pathPointsGroup.GetChild(i).position;
                    Vector3 linePointB = pathPointsGroup.GetChild(i + 1).position;

                    Debug.DrawLine(linePointA, linePointB, Color.green);
                }
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                other.transform.parent = transform;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                other.transform.parent = null;
        }

        private void Update()
        {
            SetTargetPoint();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            Vector3 position = transform.position;
            float scaledSpeed = moveSpeed * Time.fixedDeltaTime;

            if (moveType == MoveType.Lerp)
            {
                transform.position = Vector3.Lerp(position, TargetPoint, scaledSpeed);
                return;
            }

            //linear
            transform.position = Vector3.MoveTowards(position, TargetPoint, scaledSpeed);
        }

        private void SetTargetPoint()
        {
            float distance = Vector3.Distance(transform.position, TargetPoint);
            
            if (distance < .1f)
            {
                bool isFirstPoint = _currentWaypointIndex <= 0;
                bool isLastPoint = _currentWaypointIndex >= (_path.Length - 1);
                
                if(isFirstPoint)
                    _moveDirection = 1;
                else if (isLastPoint)
                    _moveDirection = -1;

                _currentWaypointIndex += _moveDirection;
            }
        }

        private Vector3[] GetPath()
        {
            int count = pathPointsGroup.childCount;
            Vector3[] path = new Vector3[count];

            for (int i = 0; i < count; i++)
            {
                path[i] = pathPointsGroup.GetChild(i).position;
            }

            return path;
        }
    }
}
