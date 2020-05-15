using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OccupiedZone : MonoBehaviour
{
    private Animator animator;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        animator.SetBool("HasOccupant", true);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        animator.SetBool("HasOccupant", false);
    }
}