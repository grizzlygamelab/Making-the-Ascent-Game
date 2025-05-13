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
    private bool _isDead = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        _distanceToTarget = Vector3.Distance(_navMeshAgent.transform.position, target.position);

        if (_isDead == false)
        {
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
        else
        {
            _navMeshAgent.isStopped = true;
        }
       
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon-Player") && _canTakeDamage)
        {
            _canTakeDamage = false;
            int amount = other.gameObject.GetComponent<PlayerAttack>().GetDamageAmount();
            //int amount = 5;
            TakeDamage(amount);
            StartCoroutine(EnemyDamageCooldown());
            
        }
    }
    
    private void TakeDamage(int damage)
    {
        // enemy takes damage from the player's sword
        _health -= damage;
        if (_health <= 0)
        {
            // make enemy disappear
            //gameObject.SetActive(false);
            _animator.SetTrigger("DeathBlow");
            _isDead = true;
            UpdateEnemy();
        }
        bloodParticles.Play();
    }
    
    private IEnumerator EnemyDamageCooldown()
    {
        yield return new WaitForSeconds(1.5f);
        _canTakeDamage = true;
    }

    public bool IsDead()
    {
        return _isDead;
    }

    private void UpdateEnemy()
    {
        GameObject.FindObjectOfType<GameManager>().UpdateEnemyCount();
        StartCoroutine(RemoveEnemyFromScene());
    }

    private IEnumerator RemoveEnemyFromScene()
    {
        yield return new WaitForSeconds(1.8f);
        gameObject.SetActive(false);
    }
}
