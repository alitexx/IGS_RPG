using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : MonoBehaviour
{
    public GameObject Door;
    public GameObject Target;
    float maxDistance = 1.5f;
    float DistanceBetweenObjects;
    public SpriteRenderer spriteRenderer;
    public Sprite newSprite;
    public audioManager audioManager;
    private bool flipped;

    private void Update()
    {
        
        if (flipped == false)
        {
            DistanceBetweenObjects = Vector3.Distance(transform.position, Target.transform.position);

            if (DistanceBetweenObjects <= maxDistance && Input.GetKeyDown("space"))
            {
                Debug.Log(":D");
                Destroy(Door);
                spriteRenderer.sprite = newSprite;
                flipped = true;
                
            }
            
        }
    
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (flipped == false)
        {
            if (collision.gameObject.tag == "Player" && Input.GetKeyDown("space"))
            {
                Debug.Log(":D");
                Destroy(Door);
                spriteRenderer.sprite = newSprite;
                flipped = true;
                audioManager.playSFX(19);
            }
            
        }
    }
}
