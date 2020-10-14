using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchingWallState : PlayerState
{
    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool isTouchingLedge;
    protected bool grabInput;
    protected int xInput;

    public PlayerTouchingWallState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animationBoolName) : base(player, stateMachine, playerData, animationBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckIfTouchingWall();
        isTouchingLedge = player.CheckForLedge();

        if (!isTouchingLedge && isTouchingWall)
        {
            player.LedgeClimbState.SetDetectionPosition(player.transform.position);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormalizedInputX;
        grabInput = player.InputHandler.GrabInput;

        if (player.InputHandler.JumpInput)
        {
            player.InAirState.StopWallJumpCoyoteTime();
            player.InputHandler.UseJumpInput();
            stateMachine.ChangeState(player.WallJumpState);
        }
        else if((!isTouchingWall && !isGrounded) || (xInput != player.FacingDirection && !grabInput))
        {            
            stateMachine.ChangeState(player.InAirState);
        }
        else if (isGrounded && !grabInput)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else if (isTouchingWall && !isTouchingLedge)
        {
            stateMachine.ChangeState(player.LedgeClimbState);
        }
    }
}
