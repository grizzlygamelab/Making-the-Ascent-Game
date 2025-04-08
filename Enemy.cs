using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    public Transform target;
    public float attackDistance;
    
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private float _distanceToTarget;
    
    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _distanceToTarget = Vector3.Distance(_navMeshAgent.transform.position, target.position);

        if (_distanceToTarget < attackDistance)
        {
            _animator.SetBool("IsMoving", false);
            _animator.SetBool("IsAttacking", true);
            _navMeshAgent.isStopped = true;
            
        }
        else
        {
            _animator.SetBool("IsMoving", true);
            _animator.SetBool("IsAttacking", false);
            _navMeshAgent.isStopped = false;
            _navMeshAgent.destination = target.position;
           
        }
    }
}
