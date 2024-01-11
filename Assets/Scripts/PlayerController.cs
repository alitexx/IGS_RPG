using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5f;
    public Transform movePoint;

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, movementSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, movePoint.position) <= .05f) 
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f) //if left or right arrow is pressed
            {
                movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f); //move which ever direction is pressed
            }

            if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)  //If up or down arrow is pressed
            {
                movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f); //move which ever direction is pressed
            }
        }
    }
}
