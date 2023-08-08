using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    [SerializeField] Transform attackPoint;

    [SerializeField] float attackRadius = 2.0f;

    [SerializeField] LayerMask whatIsTarget;

    [SerializeField] float damage = 25.0f;

    Animator _animator;

    [SerializeField] int attackRate = 2;

    float _nextAttackTime = 0;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    public void OnAttack()
    {
        Collider[] colliders = Physics.OverlapSphere(attackPoint.position, attackRadius, whatIsTarget);

        foreach (Collider collider in colliders)
        {
            HealthController healthController = collider.GetComponent<HealthController>();

            if (healthController != null)
            {
                healthController.TakeDamage(damage);
            }
        }

        _animator.ResetTrigger("attack");
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > _nextAttackTime)
        {
            _nextAttackTime = Time.time + 1.0f / attackRate;

            _animator.SetTrigger("attack");
        }
    }
}