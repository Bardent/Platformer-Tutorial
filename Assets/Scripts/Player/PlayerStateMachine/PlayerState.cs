using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected PlayerData playerData;
    protected string animationBoolName;
    protected float startTime;
    protected bool isExitingState;
    protected bool isAnimationFinished;

    public PlayerState (Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animationBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animationBoolName = animationBoolName;
    }

    public virtual void Enter() 
    {
        isExitingState = false;
        isAnimationFinished = false;
        player.Anim.SetBool(animationBoolName, true);
        startTime = Time.time;
        DoChecks();
    }

    public virtual void Exit() 
    {
        isExitingState = true;
        player.Anim.SetBool(animationBoolName, false);
    }

    public virtual void LogicUpdate() { }

    public virtual void PhysicsUpdate() => DoChecks();

    public virtual void DoChecks() { }

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;

    public virtual void AnimationTrigger() { }
}
