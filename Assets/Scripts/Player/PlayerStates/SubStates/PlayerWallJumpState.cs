using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState
{

    private bool isTouchingWallFront;
    private bool isTouchingWallBack;
    private int wallJumpDirection;

    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animationBoolName) : base(player, stateMachine, playerData, animationBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isTouchingWallFront = player.CheckIfTouchingWall();
        isTouchingWallBack = player.CheckIfTouchingWallBack();

    }

    public override void Enter()
    {
        base.Enter();        
        DetermineWallJumpDirection();
        player.CheckFlipPlayer(wallJumpDirection);
        player.JumpState.DecreaseAmountOfJumpsLeft();
        player.SetVelocity(playerData.wallJumpVelocity, playerData.wallJumpAngle, wallJumpDirection);
        player.DashState.SetCanDash(true);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        player.Anim.SetFloat("yVelocity", player.CurrentVelocity.y);
        player.Anim.SetFloat("xVelocity", Mathf.Abs(player.CurrentVelocity.x));

        if(Time.time >= startTime + playerData.wallJumpTime)
        {
            isAbilityDone = true;
        }

    }

    private void DetermineWallJumpDirection()
    {
        DoChecks();

        if (isTouchingWallBack)
        {
            wallJumpDirection = player.FacingDirection;
        }
        else if (isTouchingWallFront)
        {
            wallJumpDirection = -player.FacingDirection;
        }
        else
        {
            wallJumpDirection = player.FacingDirection;
        }
    }

    public void SetWallJumpDirection(int direction)
    {
        wallJumpDirection = direction;
    }
}
