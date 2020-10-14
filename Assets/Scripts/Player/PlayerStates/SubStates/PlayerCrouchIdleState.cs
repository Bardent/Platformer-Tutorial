using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchIdleState : PlayerGroundedState
{
    public PlayerCrouchIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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
            player.CheckIfShouldFlip(xInput);

            player.SetVelocityX(0f);

            if (yInput >= 0 && !isTouchingCeiling)
            {
                Debug.Log("No ceiling detected, going to normal idle state");
                stateMachine.ChangeState(player.IdleState);
            }
            else if(xInput != 0)
            {
                stateMachine.ChangeState(player.CrouchMoveState);
            }
        }

    }
}
