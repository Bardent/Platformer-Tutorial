using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeRollState : PlayerAbilityState
{
    private bool dodgeRollRecovery;
    private int xInput;
    private int triggerNum;    

    public PlayerDodgeRollState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animationBoolName) : base(player, stateMachine, playerData, animationBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        isAbilityDone = true;
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        if(triggerNum == 0)
        {
            triggerNum++;
            player.SetVelocityX(playerData.DodgeRollSpeed * player.FacingDirection);
        }
        else
        {
            dodgeRollRecovery = true;
            player.Anim.SetBool("dodgeRollRecovery", true);
        }
    }

    public override void DoChecks()
    {
        base.DoChecks();
        
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocityX(playerData.DodgeRollInitialSpeed * player.FacingDirection);
        dodgeRollRecovery = false;
        triggerNum = 0;
        player.SetColliderHeight(playerData.crouchColliderHeight);
    }

    public override void Exit()
    {
        base.Exit();
        player.Anim.SetBool("dodgeRollRecovery", false);
        player.SetColliderHeight(playerData.standColliderHeight);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            xInput = player.InputHandler.NormalizedInputX;

            if (dodgeRollRecovery && (xInput == -player.FacingDirection || !isGrounded || isTouchingCeiling))
            {
                isAbilityDone = true;              
            }
        }

    }
}
