using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public PlayerController playerController;
    private bool flipped;
    [SerializeField] private mapManager mapManager;
    [SerializeField] private GameObject associatedExclamation;
    [SerializeField] private bool inSameRoomAsDoor;

    private void Update()
    {
        
        if (flipped == false)
        {
            DistanceBetweenObjects = Vector3.Distance(transform.position, Target.transform.position);

            if (DistanceBetweenObjects <= maxDistance && Input.GetKeyDown(audioStatics.keycodeInterractButton))
            {
                Debug.Log(":D");
                Destroy(Door);
                spriteRenderer.sprite = newSprite;
                flipped = true;
                audioManager.playSFX(20);
                if (!inSameRoomAsDoor)
                {
                    mapManager.ForceOpenMap(associatedExclamation);
                }
            }
            
        }
    
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (flipped == false)
        {
            if (collision.gameObject.tag == "Player" && Input.GetKeyDown(audioStatics.keycodeInterractButton))
            {
                Debug.Log("I AM THE CORRECT ONE");
                Destroy(Door);
                spriteRenderer.sprite = newSprite;
                flipped = true;
                playerController.DoorsOpened += 1;
                audioManager.playSFX(20);
                if (!inSameRoomAsDoor)
                {
                    mapManager.ForceOpenMap(associatedExclamation);
                }
            }
            
        }
    }
}
