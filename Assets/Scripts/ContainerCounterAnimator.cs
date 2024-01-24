using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterAnimator : MonoBehaviour
{
    [SerializeField] private ContainerCounter containerCounter;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        containerCounter.OnInteract += ContainerCounter_OnInteract;
    }

    private void ContainerCounter_OnInteract(object sender, System.EventArgs e)
    {
        animator.SetTrigger("OpenClose");
    }
}
