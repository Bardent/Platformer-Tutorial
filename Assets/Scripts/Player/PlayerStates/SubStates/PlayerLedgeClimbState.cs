using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    private Vector2 detectingPos;
    private Vector2 ledgeCornerPos;
    private Vector2 ledgeClimbStartPos;
    private Vector2 ledgeClimbEndPos;
    private bool isHanging;
    private bool isClimbingLedge;
    private RaycastHit2D isTouchingWall;

    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animationBoolName) : base(player, stateMachine, playerData, animationBoolName)
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

        player.SetVelocityX(0f);
        player.SetVelocityY(0f);

        player.transform.position = detectingPos;

        isTouchingWall = player.CheckIfTouchingWall();

        ledgeCornerPos = player.DetermineCornerPosition(isTouchingWall);

        Debug.Log(ledgeCornerPos);
        ledgeClimbStartPos.Set(ledgeCornerPos.x - (player.FacingDirection * playerData.startPosOffset.x), ledgeCornerPos.y - playerData.startPosOffset.y);
        ledgeClimbEndPos.Set(ledgeCornerPos.x + (player.FacingDirection * playerData.endPosOffset.x), ledgeCornerPos.y + playerData.endPosOffset.y);        
        player.transform.position = ledgeClimbStartPos;
        
    }

    public override void Exit()
    {
        base.Exit();

        isHanging = false;

        if (isClimbingLedge)
        {
            player.transform.position = ledgeClimbEndPos;
            isClimbingLedge = false;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else
        {

            player.SetVelocityY(0f);
            player.transform.position = ledgeClimbStartPos;

            if (player.InputHandler.RawMovementInput.y < -0.75f && isHanging && !isClimbingLedge)
            {
                stateMachine.ChangeState(player.InAirState);
            }

            if (Mathf.Abs(player.InputHandler.RawMovementInput.x) > 0.75f && isHanging && player.InputHandler.NormalizedInputX == player.FacingDirection)
            {
                isClimbingLedge = true;
                player.Anim.SetBool("climbLedge", true);
            }
        }

    }

    public void SetCornerPosition(Vector2 pos) => ledgeCornerPos = pos;

    public void SetDetectionPosition(Vector2 pos) => detectingPos = pos;
}
