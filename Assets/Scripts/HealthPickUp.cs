using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    public int healed = 20;
    public Vector3 spinRotatorSpeed = new Vector3(0, 180, 0);
    AudioSource audioSource;

    private void Awake() 
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        Damage damage = collision.GetComponent<Damage>();

        if(damage)
        {
            bool washealed = damage.Heal(healed);
            if(washealed)
                AudioSource.PlayClipAtPoint(audioSource.clip, gameObject.transform.position, audioSource.volume);
            
            Destroy(gameObject);
        }
    }

    private void Update() 
    {
        transform.eulerAngles += spinRotatorSpeed * Time.deltaTime;
    }
}
