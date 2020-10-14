using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animationBoolName) : base(player, stateMachine, playerData, animationBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            player.CheckFlipPlayer(xInput);

            player.SetVelocityX(playerData.movementSpeed * xInput);

            if (xInput == 0)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if(yInput == -1)
            {
                stateMachine.ChangeState(player.CrouchMoveState);
            }
        }
        

    }
}
