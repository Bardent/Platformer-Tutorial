using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected int xInput;
    protected int yInput;
    protected bool isTouchingWall;
    protected bool isGrounded;

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animationBoolName) : base(player, stateMachine, playerData, animationBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckIfTouchingWall();

    }

    public override void Enter()
    {
        base.Enter();
        player.JumpState.ResetAmountOfJumpsLeft();
        player.DashState.SetCanDash(true);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        xInput = player.InputHandler.NormalizedInputX;
        yInput = player.InputHandler.NormalizedInputY;

        if (player.InputHandler.JumpInput)
        {
            if (player.JumpState.AmountOfJumpsLeft > 0)
            {
                player.InputHandler.UseJumpInput();                
                stateMachine.ChangeState(player.JumpState);
            }
        }
        else if (!isGrounded)
        {
            player.InAirState.StartCoyoteTime();
            stateMachine.ChangeState(player.InAirState);
        }
        else if (player.InputHandler.DodgeRollInput)
        {
            player.InputHandler.UseDodgeRollInput();
            stateMachine.ChangeState(player.DodgeRollState);
        }
        else if (player.InputHandler.DashInput && player.DashState.canDash && yInput != -1 && !player.CheckForCeiling())
        {
            player.InputHandler.UseDashInput();
            stateMachine.ChangeState(player.DashState);
        }
        else if (player.InputHandler.GrabInput && isTouchingWall)
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
    }
}
