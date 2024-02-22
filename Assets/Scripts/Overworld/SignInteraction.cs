using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignInteraction : MonoBehaviour
{
    public GameObject SignUI;

    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && Input.GetKeyDown("space"))
        {
            Debug.Log("Sign signed correctly");
            StartCoroutine(SignMessage());
        }
    }

    private IEnumerator SignMessage()
    {
        SignUI.SetActive(true);
        yield return new WaitForSeconds(5);
        SignUI.SetActive(false);
    }

}
