using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damage : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageHit;
    public UnityEvent<int, int> healthChangge;
    Animator anim;
    [SerializeField]
    private int _MaxHP = 100;
    private bool _isAlive = true;
    [SerializeField]
    private bool IsInvincible = false;


    private float sinceHit = 0;
    public float invicibleTime = 0.25f;
    public int MaxHP
    {
        get
        {
            return _MaxHP;
        }
        set
        {
            _MaxHP = value;
        }
    }
    [SerializeField]
    private int _HP = 100;
    public int HP
    {
        get
        {
            return _HP;
        }
        set
        {
            _HP = value;
            healthChangge?.Invoke(_HP, MaxHP);

            if(_HP <= 0 )
            {
                IsAlive = false;
            }
        }
    }

   
    public bool IsAlive
    {
        get
        {
            return  _isAlive;
        }
        set
        {
            _isAlive = value;
            anim.SetBool("IsAlive", value);
            Debug.Log("Mati?" + value);
        }
    }

    public bool LockVelocity 
    {   get 
        {
            return anim.GetBool("LockVelocity");
        }
        set
        {
            anim.SetBool("LockVelocity", value);
        }
    }
 
    private void Awake() 
    {
      anim = GetComponent<Animator>();  
    }
    
    private void Update()
    {
        if(IsInvincible)
        {
            if(sinceHit > invicibleTime)
            {
                IsInvincible = false;
                sinceHit = 0;
            }

                sinceHit += Time.deltaTime;
        }
        
    }

    public bool Hit(int damage, Vector2 knockback)
    {
        if(IsAlive && !IsInvincible)
        {
            HP -= damage;
            IsInvincible = true;
            anim.SetTrigger("Hit");
            LockVelocity = true;
            damageHit?.Invoke(damage, knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);

            return true;
        }

        return false;
    }

    public bool Heal(int healthRestore)
    {
        if(IsAlive && HP < MaxHP)
        {
            int MaxHeal = Mathf.Max(MaxHP - HP, 0);
            int actualHeal = Mathf.Min(MaxHeal, healthRestore);
            HP += actualHeal;
            CharacterEvents.characterHealed(gameObject, actualHeal);
            return true;
        }
        return false;
    }

    

}
