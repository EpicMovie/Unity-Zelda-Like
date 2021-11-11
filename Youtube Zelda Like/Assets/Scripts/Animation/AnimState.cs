using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAnimState
{
    Idle,
    Move,
    Attack,
}

public abstract class AnimState 
{
    public static AnimState CreateAnimState(EAnimState InAnimState)
    {
        switch(InAnimState)
        {
            case EAnimState.Idle:
                return new IdleState();
            case EAnimState.Move:
                return new MoveState();
            case EAnimState.Attack:
                return new AttackState();
            default:
                break;
        }

        return null;
    }

    public abstract EAnimState GetState();
    public abstract bool IsTransitionable();

    protected AnimState() {}
}

public class IdleState : AnimState
{
    public override EAnimState GetState()
    {
        return EAnimState.Idle;
    }

    public override bool IsTransitionable()
    {
        return true;
    }
}

public class MoveState : AnimState
{
    public override EAnimState GetState()
    {
        return EAnimState.Move;
    }

    public override bool IsTransitionable()
    {
        return true;
    }
}

public class AttackState : AnimState
{
    public override EAnimState GetState()
    {
        return EAnimState.Attack;
    }

    public override bool IsTransitionable()
    {
        return true;
    }
}

