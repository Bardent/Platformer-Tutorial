using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchMoveState : PlayerGroundedState
{
    private bool isTouchingCeiling;

    public PlayerCrouchMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animationBoolName) : base(player, stateMachine, playerData, animationBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isTouchingCeiling = player.CheckForCeiling();
    }

    public override void Enter()
    {
        base.Enter();
        player.SetColliderHeight(playerData.crouchColliderHeight);
    }

    public override void Exit()
    {
        base.Exit();
        player.SetColliderHeight(playerData.standColliderHeight);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            player.SetVelocityX(playerData.crouchMoveSpeed * player.FacingDirection);

            player.CheckFlipPlayer(xInput);

            if(yInput >= 0 && !isTouchingCeiling)
            {
                stateMachine.ChangeState(player.MoveState);
            }
            else if(xInput == 0)
            {
                stateMachine.ChangeState(player.CrouchIdleState);
            }
        }
    }
}
