using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection), typeof(Damage))]
public class Bandit : MonoBehaviour
{
    public float walkspeed = 3f ;
    public float walkStopRate = 0.3f;
    public DetectionZone attackZone;
    public DetectionZone cliffZone;
    Rigidbody2D rb;
    TouchingDirection touchingDirection;
    Animator anim;
    Damage Damage;

    public enum walkDirection {Right, Left}
    private walkDirection _walkDirection;
    private Vector2 WalkDirectionVector = Vector2.right;

    public walkDirection WalkDirection
    {
        get
        {
            return _walkDirection;
        }
        set
        {
            if(_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if(value == walkDirection.Right)
                {
                    WalkDirectionVector = Vector2.right;
                }
                else if (value == walkDirection.Left)
                {
                    WalkDirectionVector = Vector2.left;
                }

            }
            _walkDirection = value;
        }
    }

    public bool _hasTarget = false;
    

    public bool HasTarget 
    { get
        {
            return _hasTarget;

        }

     private set
        {
            _hasTarget = value;
            anim.SetBool("HasTarget", value);
        }
     
    }

    public bool CanMove
    {
        get
        {
            return anim.GetBool("CanMove");
        }
    }

    public float attackCooldown 
    {   get 
        {
            return anim.GetFloat("AttackCooldown");
        }
        private set
        {
            anim.SetFloat("AttackCooldown", MathF.Max(value, 0));
        }
    }

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirection = GetComponent<TouchingDirection>();
        anim = GetComponent<Animator>();
        Damage = GetComponent<Damage>();
    }


    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
        
        
        if(attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
        
    }

    private void FixedUpdate() 
    {
        if(touchingDirection.IsGround && touchingDirection.IsOnwall)
        {
            FlipDirection();
        }
        if(!Damage.LockVelocity)
        {
            if(CanMove)
            rb.velocity = new Vector2(walkspeed * WalkDirectionVector.x, rb.velocity.y);
            else 
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
        }
        
    }

    private void FlipDirection()
    {
        if (WalkDirection == walkDirection.Right)
        {
            WalkDirection = walkDirection.Left;
        }
        else if (WalkDirection == walkDirection.Left)
        {
            WalkDirection = walkDirection.Right;
        }

    }

    public void OnHit(int damage, Vector2 knockback)
    {
        
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnCliffDetected()
    {
        if(touchingDirection.IsGround)
        {
            FlipDirection();
        }
    }


}
