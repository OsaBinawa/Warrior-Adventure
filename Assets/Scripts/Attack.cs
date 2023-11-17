using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    Collider2D attackCol;
    public int attackDamage = 10;
    public Vector2 knockback = Vector2.zero;


    private void Awake() 
    {
        attackCol = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        Damage damage = collision.GetComponent<Damage>();

        if(damage != null)
        {
            Vector2 deliveredKnockback = transform.parent.localScale.x > 0? knockback : new Vector2(-knockback.x, knockback.y);
            bool gotHit = damage.Hit(attackDamage, deliveredKnockback);
        }
    }
}


 
