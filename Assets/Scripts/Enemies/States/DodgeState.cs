using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeState : State
{
    protected D_DodgeState stateData;

    protected bool performCloseRangeAction;
    protected bool isPlayerInMaxAgroRange;
    protected bool isGrounded;
    protected bool isDodgeOver;

    private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    private Movement movement;

    private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
    private CollisionSenses collisionSenses;

    public DodgeState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_DodgeState stateData) : base(etity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
        isGrounded = CollisionSenses.Ground;
    }

    public override void Enter()
    {
        base.Enter();

        isDodgeOver = false;

        Movement?.SetVelocity(stateData.dodgeSpeed, stateData.dodgeAngle, -Movement.FacingDirection);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= startTime + stateData.dodgeTime && isGrounded)
        {
            isDodgeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
