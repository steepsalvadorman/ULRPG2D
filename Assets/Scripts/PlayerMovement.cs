using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 4f;

    private Rigidbody2D mRb;
    private Vector3 mDirection = Vector3.zero;
    private Animator mAnimator;
    private PlayerInput mPlayerInput;
    private Transform hitBox;

    //Para hacer danio a bos
    [SerializeField] private float hitBoxSize;
    [SerializeField] private Image healthBar;
    [SerializeField] private float maxHp;
    [SerializeField] private float hp;
    [SerializeField] private int damage;

    //Barra de energia para nuevo poder
    [SerializeField] private GameObject fire;
    [SerializeField] private Image energyBar;
    [SerializeField] private float maxEnergy;
    [SerializeField] private float energy;
    [SerializeField] private int fireDamage;
    [SerializeField] private float fireArea;

    private void Start()
    {
        //Igualamos actual al maximo de vida
        hp = maxHp;

        mRb = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
        mPlayerInput = GetComponent<PlayerInput>();

        hitBox = transform.Find("HitBox");

        ConversationManager.Instance.OnConversationStop += OnConversationStopDelegate;
        
    }

    private void OnConversationStopDelegate()
    {
        mPlayerInput.SwitchCurrentActionMap("Player");
    }

    private void Update()
    {
        //Para actualizar energia
        energyBar.fillAmount = energy / maxEnergy;
        //Para actualizar vida
        healthBar.fillAmount = hp / maxHp;

        //Ir aumentando la energia por segundo hasta 10.
        if (energy < 10)
        {
            energy += 1 * Time.deltaTime;
        }
        OnFire();


        if (mDirection != Vector3.zero)
        {
            mAnimator.SetBool("IsMoving", true);
            mAnimator.SetFloat("Horizontal", mDirection.x);
            mAnimator.SetFloat("Vertical", mDirection.y);
        }else
        {
            // Quieto
            mAnimator.SetBool("IsMoving", false);
        }
    }

    private void FixedUpdate()
    {
        mRb.MovePosition(
            transform.position + (mDirection * speed * Time.fixedDeltaTime)
        );
    }

    public void OnMove(InputValue value)
    {
        mDirection = value.Get<Vector2>().normalized;
    }

    public void OnNext(InputValue value)
    {
        if (value.isPressed)
        {
            ConversationManager.Instance.NextConversation();
        }
    }

    public void OnCancel(InputValue value)
    {
        if (value.isPressed)
        {
            ConversationManager.Instance.StopConversation();
        }
    }

    public void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            mAnimator.SetTrigger("Attack");
            hitBox.gameObject.SetActive(true);
            InflictDamage();
        }
    }

    private void OnFire()
    {
        if (Input.GetKey(KeyCode.LeftControl) && energy >= 10)
        {
            Debug.Log("FIRE FIRE");
            fire.SetActive(true);
            Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, fireArea);
            foreach (Collider2D col in objects)
            {
                if (col.CompareTag("Boss"))
                {
                    col.gameObject.GetComponent<Boss>().TakeDamageBoss(fireDamage);
                }
            }
            energy = 0;
        }
    }

    private void InflictDamage()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(hitBox.position, hitBoxSize);

        foreach (Collider2D colisionador in objects)
        {
            if (colisionador.CompareTag("Boss"))
            {
                Debug.Log("Hit");
                //Hacer danio al boss
                colisionador.gameObject.GetComponent<Boss>().TakeDamageBoss(damage);
            }
        }
    }

    public void TakeDamagePlayer(int damage)
    {
        hp -= damage;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Conversation conversation;
        if (other.transform.TryGetComponent<Conversation>(out conversation))
        {
            mPlayerInput.SwitchCurrentActionMap("Conversation");
            ConversationManager.Instance.StartConversation(conversation);
        }
    }

    public void DisableHitBox()
    {
        hitBox.gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, fireArea);
    }
}
