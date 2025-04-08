using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float runSpeed = 5.0f;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private bool groundedPlayer;
    private CharacterController _controller;
    private Animator _animator;
    private Vector3 _playerVelocity;
    
    [Header("Player Health")]
    public GameObject[] hearts;
    public TextMeshProUGUI healthText;
    public ParticleSystem bloodParticles;
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _currentHealth;
    private bool _canTakeDamage = true;
    
    
    [Header("Player Items")]
    public GameObject itemPromptText;
    public GameObject playerSword;
    public GameObject playerKey;
    public bool hasSword = false;
    public bool hasKey = false;
    
    private void Start()
    {
        _controller = gameObject.GetComponent<CharacterController>();
        _animator = gameObject.GetComponentInChildren<Animator>();
        _items = gameObject.GetComponent<PlayerItems>();
        currentSpeed = walkSpeed;
        
        playerSword.SetActive(false);
        itemPromptText.SetActive(false);
        
        _currentHealth = _maxHealth;
        healthText.text = "HP: " + _currentHealth.ToString();
        UpdatePlayerHealthUI();
    }

    void Update()
    {
        // Player movement
        Movement();
        
        // Makes the player pickup an item
        if (Input.GetKeyDown(KeyCode.E) && !PlayerHasKey())
        {
            _animator.SetTrigger("CanPickup");
        }
        
        // Makes the player attack with weapon
        if (Input.GetMouseButtonDown(0) && PlayerHasSword())
        {
            _animator.SetTrigger("Attack");
        }

        // Makes the player jump
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            _playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
            _animator.SetBool("IsGrounded", false);
        }
        else
        {
            _animator.SetBool("IsGrounded", true);
        }
    }

    private void Movement()
    {
        groundedPlayer = _controller.isGrounded;
        if (groundedPlayer && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _controller.Move(move * Time.deltaTime * currentSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
            _animator.SetBool("IsMoving", true);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                _animator.SetBool("IsStrafing", true);
                currentSpeed = runSpeed;
            }
            else
            {
                _animator.SetBool("IsStrafing", false);
                currentSpeed = walkSpeed;
            }
        }
        else
        {
            _animator.SetBool("IsMoving", false);
            currentSpeed = walkSpeed;
        }
        
        _playerVelocity.y += gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && _canTakeDamage)
        {
            //Debug.Log("I ran into the enemy.");
            _canTakeDamage = false;
            int amount = other.gameObject.GetComponent<EnemyAttack>().GetDamageAmount();
            TakeDamage(amount);
            UpdatePlayerHealthUI();
            StartCoroutine(PlayerDamageCooldown());
        }

        if (other.gameObject.CompareTag("Sword-Pickup"))
        {
            other.gameObject.SetActive(false);
            playerSword.SetActive(true);
            hasSword = true;
        }

        if (other.gameObject.CompareTag("Key-Pickup"))
        {
            playerKey.SetActive(false);
            hasKey = true;
        }
    }

    private bool PlayerHasSword()
    {
        return hasSword;
    }

    private bool PlayerHasKey()
    {
        return hasKey;
    }
    
    private IEnumerator PlayerDamageCooldown()
    {
        yield return new WaitForSeconds(1.5f);
        _canTakeDamage = true;
    }
    
    private void TakeDamage(int damage)
    {
        // player takes damage from the enemy
        _currentHealth -= damage;
        //_currentHealth--;
        bloodParticles.Play();
        // update the text in the UI
        healthText.text = "HP: " + _currentHealth.ToString();
    }

    private void UpdatePlayerHealthUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < _currentHealth);
        }
    }
    
}
