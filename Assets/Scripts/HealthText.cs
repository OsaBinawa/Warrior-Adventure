using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthText : MonoBehaviour
{
    public Vector3 moveSpeed = new Vector3(0, 75, 0);
    public float fadeTime = 1f;
    RectTransform textTransform;
    TextMeshProUGUI textMeshPro;
    private Color startColor;
    private float timeElapsed;


    private void Awake() 
    {
        textTransform = GetComponent<RectTransform>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
        startColor = textMeshPro.color;
    }

    private void Update() 
    {
        textTransform.position += moveSpeed * Time.deltaTime;
        timeElapsed += Time.deltaTime;

        if(timeElapsed < fadeTime)
        {
            float fadeAlpha = startColor.a * (1 - (timeElapsed / fadeTime));
            textMeshPro.color = new Color(startColor.r, startColor.g, startColor.b, fadeAlpha);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
