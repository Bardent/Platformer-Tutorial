using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    private Vector2 detectedPos;
    private Vector2 cornerPos;
    private Vector2 startPos;
    private Vector2 stopPos;

    private bool isHanging;
    private bool isClimbing;
    private bool jumpInput;
    private bool isTouchingCeiling;

    private int xInput;
    private int yInput;

    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        player.Anim.SetBool("climbLedge", false);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        isHanging = true;
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocityZero();
        player.transform.position = detectedPos;
        cornerPos = player.DetermineCornerPosition();

        Debug.Log("Corner Position: " + cornerPos);

        startPos.Set(cornerPos.x - (player.FacingDirection * playerData.startOffset.x), cornerPos.y - playerData.startOffset.y);
        stopPos.Set(cornerPos.x + (player.FacingDirection * playerData.stopOffset.x), cornerPos.y + playerData.stopOffset.y);

        player.transform.position = startPos;

    }

    public override void Exit()
    {
        base.Exit();

        isHanging = false;
        
        if (isClimbing)
        {
            player.transform.position = stopPos;
            isClimbing = false;
            Debug.Log("Final pos: " + stopPos + " Actual Pos: " + player.transform.position);
        }

       
       
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {          

            if (!isTouchingCeiling)
            {                
                stateMachine.ChangeState(player.IdleState);
            }
            else
            {                
                stateMachine.ChangeState(player.CrouchIdleState);
            }
        }
        else
        {
            xInput = player.InputHandler.NormInputX;
            yInput = player.InputHandler.NormInputY;
            jumpInput = player.InputHandler.JumpInput;

            player.SetVelocityZero();
            player.transform.position = startPos;

            if (xInput == player.FacingDirection && isHanging && !isClimbing)
            {
                isClimbing = true;
                player.Anim.SetBool("climbLedge", true);
                CheckForSpace();
            }
            else if (yInput == -1 && isHanging && !isClimbing)
            {
                stateMachine.ChangeState(player.InAirState);
            }
            else if(jumpInput && !isClimbing)
            {
                player.WallJumpState.DetermineWallJumpDirection(true);
                stateMachine.ChangeState(player.WallJumpState);
            }
        }
      
    }

    public void SetDetectedPosition(Vector2 pos) => detectedPos = pos;

    private void CheckForSpace()
    {
        RaycastHit2D hit = Physics2D.Raycast(cornerPos + (Vector2.up * 0.15f), Vector2.up, 1.15f, playerData.whatIsGround);

        if(hit && hit.distance <= 2f)
        {
            isTouchingCeiling = true;             
        }
        else
        {
            isTouchingCeiling = false;
        }

        player.Anim.SetBool("ledgeClimbCeiling", isTouchingCeiling);
    }
}
