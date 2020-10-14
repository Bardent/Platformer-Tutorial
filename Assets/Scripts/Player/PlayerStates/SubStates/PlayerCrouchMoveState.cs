using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchMoveState : PlayerGroundedState
{

    public PlayerCrouchMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
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

            player.CheckIfShouldFlip(xInput);

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
