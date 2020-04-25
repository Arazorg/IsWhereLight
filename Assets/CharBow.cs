using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharBow : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ArrowString()
    {
        animator.SetBool("IsButtonDown", true);
        Debug.Log("Натягивание");
    }

    public void Shoot()
    {
        animator.SetBool("IsButtonDown", false);
        Debug.Log("Стрельба");
    }
}
