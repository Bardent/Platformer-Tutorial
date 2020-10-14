using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    public int AmountOfJumpsLeft { get; private set; }

    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animationBoolName) : base(player, stateMachine, playerData, animationBoolName)
    {
        AmountOfJumpsLeft = playerData.amountOfJumps;
    }

    public override void Enter()
    {
        base.Enter();
        player.InAirState.SetIsJumping();
        player.SetVelocityY(playerData.jumpVelocity);
        AmountOfJumpsLeft--;
        isAbilityDone = true;
    }

    public void ResetAmountOfJumpsLeft() => AmountOfJumpsLeft = playerData.amountOfJumps;

    public void DecreaseAmountOfJumpsLeft() => AmountOfJumpsLeft--;
}
