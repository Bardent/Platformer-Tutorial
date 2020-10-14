using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallGrabState : PlayerTouchingWallState
{
    private Vector2 holdPosition;
    private int yInput;

    public PlayerWallGrabState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animationBoolName) : base(player, stateMachine, playerData, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        holdPosition = player.transform.position;

        HoldPosition();

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            HoldPosition();

            yInput = player.InputHandler.NormalizedInputY;

            if (yInput > 0)
            {
                stateMachine.ChangeState(player.WallClimbState);
            }
            else if(yInput < 0 || !grabInput)
            {
                stateMachine.ChangeState(player.WallSlideState);
            }

        }
    }

    private void HoldPosition()
    {        
        player.SetVelocityX(0f);
        player.SetVelocityY(0f);
    }
}
