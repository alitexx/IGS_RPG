using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaknessPopUp : MonoBehaviour
{
    private float disappearTimer;

    private Color spriteColor;
    [SerializeField] private SpriteRenderer sr;

    private void Awake()
    {
        disappearTimer = 0.7f;
    }

    private void Update()
    {
        float moveYSpeed = 2;
        transform.position += new Vector3(0, moveYSpeed, 0) * Time.deltaTime;

        disappearTimer -= Time.deltaTime;

        if (disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            spriteColor.a -= disappearSpeed * Time.deltaTime;
            sr.color = spriteColor;
            if (spriteColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
