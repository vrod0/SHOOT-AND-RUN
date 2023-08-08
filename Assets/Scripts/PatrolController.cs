using UnityEngine;
using UnityEngine.AI;

public class PatrolController : MonoBehaviour
{
    enum PatrolStates { Patrol, Chase, Attack }

    [SerializeField] Transform target;

    LayerMask _whatIsTarget;

    [SerializeField] LayerMask _whatIsGround;

    NavMeshAgent _navAgent;

    PatrolStates _currentState;

    [SerializeField] float sightRange;

    [SerializeField] float attackRange;

    [SerializeField] float walkRange;

    Vector3 _walkPoint;

    bool _isTargetInSightRange;

    bool _isTargetInAttackRange;

    bool _hasWalkPoint;

    bool _isAttacking;

    [SerializeField] float attackTimeout;

    HealthController _healthController;

    private void Awake()
    {
        _whatIsTarget = transform.gameObject.layer;

        _navAgent = GetComponent<NavMeshAgent>();

        _healthController = GetComponent<HealthController>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, walkRange);

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void Update()
    {
        _isTargetInSightRange = Physics.CheckSphere(transform.position, sightRange, _whatIsTarget);

        _isTargetInAttackRange = Physics.CheckSphere(transform.position, attackRange, _whatIsTarget);

        if (_isTargetInAttackRange)
        {
            HandleAttack();
        }
        else if (_isTargetInSightRange)
        {
            HandleChase();
        }
        else
        {
            HandlePatrol();
        }
    }

    private void HandleAttack()
    {
        _navAgent.SetDestination(transform.position);

        transform.LookAt(target);

        if (!_isAttacking)
        {
            _isAttacking = true;

            Invoke(nameof(ResetAttack), attackTimeout);
        }
    }

    private void HandleChase()
    {
        _navAgent.SetDestination(target.position);
    }

    private void HandlePatrol()
    {
        if (!_hasWalkPoint)
        {
            float positionX = Random.Range(-walkRange, walkRange);

            float positionZ = Random.Range(-walkRange, walkRange);

            _walkPoint = transform.position;

            _walkPoint.x += positionX;

            _walkPoint.z += positionZ;

            _hasWalkPoint = Physics.Raycast(_walkPoint, -transform.up, _whatIsGround);

            if (_hasWalkPoint)
            {
                _navAgent.SetDestination(_walkPoint);
            }
        }

        Vector3 distanceToWalkPoint = transform.position - _walkPoint;

        if (distanceToWalkPoint.magnitude < 1.0f)
        {
            _hasWalkPoint = false;
        }
    }

    private void ResetAttack()
    {
        _isAttacking = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("Balas tocan al enemigo");
            _healthController.TakeDamage(35);
        }
    }
}