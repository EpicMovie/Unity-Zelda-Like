using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimStateMachine : MonoBehaviour
{
    private AnimState animState = null;
    private Animator animator;

    public AnimState GetState()
    {
        return animState;
    }

    public AnimStateMachine(EAnimState InState, Animator InAnimator)
    {
        this.TransitionTo(InState);

        animator = InAnimator;
    }

    public void TransitionTo(EAnimState InState)
    {
        if (animState == null)
        {
            animState = AnimState.CreateAnimState(InState);
        }
        else if (animState.GetState() == InState)
        {
            return;
        }
        else
        {
            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Change to Other State ...
        // Transit Animation compare with condition 
        // Is That animator flags needed?
        
    }
}
