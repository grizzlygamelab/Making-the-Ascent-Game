using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    [Header("Enemy Target Settings")]
    public Transform target;
    public float attackDistance;
    
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private float _distanceToTarget;
    
    [Header("Enemy Health")]
    //public GameObject[] hearts;
    public ParticleSystem bloodParticles;
    [SerializeField] private int _health = 10;
    private bool _canTakeDamage = true;
    
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
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && _canTakeDamage)
        {
            //Debug.Log("I ran into the enemy.");
            _canTakeDamage = false;
            int amount = other.gameObject.GetComponent<EnemyAttack>().GetDamageAmount();
            TakeDamage(amount);
            //UpdatePlayerHealthUI();
            StartCoroutine(EnemyDamageCooldown());
        }
    }
    
    private void TakeDamage(int damage)
    {
        // player takes damage from the enemy
        _health -= damage;
        //_currentHealth--;
        bloodParticles.Play();
        // update the text in the UI
        //healthText.text = "HP: " + _currentHealth.ToString();
    }
    
    private IEnumerator EnemyDamageCooldown()
    {
        yield return new WaitForSeconds(1.5f);
        _canTakeDamage = true;
    }
}
