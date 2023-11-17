using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection), typeof (Damage))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    Vector2 moveInput;
    TouchingDirection touchingDirection;
    public float JumpImpulse = 10f;
    private bool isMoving = false;
    Damage Damage;
    public GameManagers gm;
    Collider2D col;
    
    public float CurrentMove
    {
        get
        {
            if(CanMove)
            {
                if(IsMoving && !touchingDirection.IsOnwall)
                {
                    return walkSpeed;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
            
        }
    }
    
    public bool IsMoving
    { get
        {
            return isMoving;
        } 
        
        private set
        {
            isMoving = value;
            anim.SetBool("IsMoving", value);
        }
    }

    public bool _isFacingRight = true;
    public bool IsFacingRight 
    { get 
        {
            return _isFacingRight;
        } 
        private set 
        {
            if(_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1,1);
            }
            _isFacingRight = value;
        } 
    }

    public bool CanMove 
    {
        get
        {
            return anim.GetBool("CanMove");
        }
    }

    Rigidbody2D rb;
    Animator anim;
    public string NextLevelScene;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        touchingDirection = GetComponent<TouchingDirection>();
        Damage = GetComponent<Damage>();
    }
    
    void Update()
    {
        if (!IsAlive && Damage.HP <= 0)
        {
            gm.gameOver();
        }
    }
    private void FixedUpdate() 
    {
        if(!Damage.LockVelocity)
            rb.velocity = new Vector2(moveInput.x * CurrentMove * Time.fixedDeltaTime, rb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if(IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;

            SetFacingDirection(moveInput);
        } 
        else
        {
            IsMoving = false;
        }
        
    }

    public bool IsAlive
    {
        get
        {
            return anim.GetBool("IsAlive");
        }
        
    }

    

    private void SetFacingDirection(Vector2 moveInput)
    {
        if(moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight) 
        {
            IsFacingRight = false;
        }
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.started && touchingDirection.IsGround && CanMove) 
        {
            anim.SetTrigger("Jump");
            rb.velocity = new Vector2(rb.velocity.x, JumpImpulse);
        }
    }

    public void  OnAttack(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            anim.SetTrigger("Attack");
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.CompareTag("Finish"))
        {
            SceneManager.LoadScene(NextLevelScene);
        }    
    }

}
