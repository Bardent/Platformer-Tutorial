using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    private int xInput;

    private bool isGrounded;
    private bool coyoteTime;
    private bool wallJumpCoyoteTime;
    private bool isTouchingLedge;
    private bool isTouchingWallBack;
    private bool oldIsTouchingWall;
    private bool oldIsTouchingWallBack;
    private bool isJumping;
    private bool isTouchingWall;

    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animationBoolName) : base(player, stateMachine, playerData, animationBoolName)
    {
    }

    public override void Enter()
    {
        oldIsTouchingWall = false;
        oldIsTouchingWallBack = false;

        base.Enter();

    }

    public override void Exit()
    {
        base.Exit();
        isJumping = false;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        oldIsTouchingWall = isTouchingWall;
        oldIsTouchingWallBack = isTouchingWallBack;

        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckIfTouchingWall();
        isTouchingWallBack = player.CheckIfTouchingWallBack();
        isTouchingLedge = player.CheckForLedge();


        if(!isTouchingLedge && isTouchingWall)
        {            
            player.LedgeClimbState.SetDetectionPosition(player.transform.position);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();


        if (player.InputHandler.DodgeRollInput)
        {
            player.InputHandler.UseDodgeRollInput();
            stateMachine.ChangeState(player.DodgeRollState);
        }
        else if (player.InputHandler.DashInput && player.DashState.canDash)
        {
            player.InputHandler.UseDashInput();
            stateMachine.ChangeState(player.DashState);
        }
        else if (isTouchingWall && !isTouchingLedge)
        {
            stateMachine.ChangeState(player.LedgeClimbState);
        }
        else if (player.InputHandler.JumpInput)
        {
            if (isTouchingWall || isTouchingWallBack || wallJumpCoyoteTime)
            {
                wallJumpCoyoteTime = false;
                player.InputHandler.UseJumpInput();
                stateMachine.ChangeState(player.WallJumpState);
            }
            else if (player.JumpState.AmountOfJumpsLeft > 0)
            {
                player.InputHandler.UseJumpInput();
                coyoteTime = false;
                stateMachine.ChangeState(player.JumpState);
            }
        }
        else if (player.InputHandler.GrabInput && isTouchingWall)
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
        else if (isTouchingWall && xInput == player.FacingDirection && player.CurrentVelocity.y < 0f && Mathf.Abs(player.InputHandler.RawMovementInput.x) > 0.75f)
        {
            stateMachine.ChangeState(player.WallSlideState);
        }        
        else
        {
            if (!isExitingState)
            {
                xInput = player.InputHandler.NormalizedInputX;

                player.SetVelocityX(xInput * playerData.movementSpeed);

                player.Anim.SetFloat("yVelocity", player.CurrentVelocity.y);
                player.Anim.SetFloat("xVelocity", Mathf.Abs(player.CurrentVelocity.x));

                player.CheckFlipPlayer(xInput);

                CheckCoyoteTime();
                CheckWallJumpCoyoteTime();
            }

            if (!wallJumpCoyoteTime && !isTouchingWall && !isTouchingWallBack && (oldIsTouchingWall || oldIsTouchingWallBack))
            {
                Debug.Log("1: " + isTouchingWall + " 2: " + isTouchingWallBack + " 3: " + oldIsTouchingWall + " 4: " + oldIsTouchingWallBack);

                Debug.Log("Starting Coyote Time");
                StartWallJumpCoyoteTime();
            }

            if (isJumping && player.CurrentVelocity.y <= 0)
            {
                isJumping = false;
            }

            if (isJumping && player.InputHandler.JumpInputStop)
            {
                isJumping = false;
                player.SetVelocityY(player.CurrentVelocity.y * playerData.varialbeJumpHeightMultuplier);
            }

            if (isGrounded && player.CurrentVelocity.y < 0.01f)
            {
                stateMachine.ChangeState(player.LandState);
            }
        }

    }

    private void CheckCoyoteTime()
    {
        if (coyoteTime && Time.time > startTime + playerData.coyoteTime)
        {
            coyoteTime = false;
            player.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    private void CheckWallJumpCoyoteTime()
    {
        if(wallJumpCoyoteTime && Time.time > startTime + playerData.coyoteTime)
        {
            wallJumpCoyoteTime = false;
        }
    }

    public void StartCoyoteTime() => coyoteTime = true;

    public void StartWallJumpCoyoteTime() => wallJumpCoyoteTime = true;

    public void StopWallJumpCoyoteTime() => wallJumpCoyoteTime = false;

    public void SetIsJumping() => isJumping = true;

   
}
