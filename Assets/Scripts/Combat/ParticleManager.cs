using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public Animator animator;

    private void Awake()
    {
        
    }

    public void Delete()
    {
        Destroy(this.gameObject);
    }
}
