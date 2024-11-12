using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Rigidbody[] rigidbodies;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>(); //fetch all rigidbodies in the Character Roots
        animator = GetComponent<Animator>();
        DeactivateRagdoll();
    }

    // Update is called once per frame
    public void DeactivateRagdoll(){
        foreach(Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = false;
        }
    }
    public void ActivateRagdoll(){
        animator.enabled = false;
        foreach(Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = true;
        }
    }
}
