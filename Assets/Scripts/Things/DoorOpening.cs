using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpening : MonoBehaviour
{
    private Animator animator;
    private bool isStay;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(!isStay)
           animator.SetBool("isOpen", false);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
            animator.Play("DoorOpen");
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        isStay = true;
        animator.SetBool("isOpen", true);
    }
    
    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Player")
            animator.Play("DoorClose");
        isStay = false;
    }
}
