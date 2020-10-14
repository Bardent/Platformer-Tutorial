using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
    private Vector3 holdPosition;
    private Vector2 dashDirection;
    private Vector2 input;
    private Vector2 lastAfterImagePosition;

    private int xInput;

    private bool isHoldingPosition;
    public bool canDash { get; private set; }

    private float holdStartTime;

    public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animationBoolName) : base(player, stateMachine, playerData, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.Anim.SetBool("dashHold", true);

        isHoldingPosition = true;
        holdPosition = player.transform.position;

        //player.SetVelocityY(0f);
        //player.transform.position = holdPosition;

        Time.timeScale = 0.25f;
        player.DashDirectionIndicator.gameObject.SetActive(true);

        dashDirection.Set(player.FacingDirection, 1);
        xInput = player.FacingDirection;

        holdStartTime = Time.unscaledTime;        
        
    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocityY(player.CurrentVelocity.y * playerData.varialbeJumpHeightMultuplier);       

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (isHoldingPosition)
            {
                //player.SetVelocityY(0f);
                //player.transform.position = holdPosition;

                input = player.InputHandler.RawMovementInput;

                if(Mathf.Abs(input.x) > 0.5f || Mathf.Abs(input.y) > 0.5f)
                {
                    xInput = player.InputHandler.NormalizedInputX;
                    dashDirection.Set(Mathf.Round(input.x), Mathf.Round(input.y));
                    dashDirection.Normalize();
                }
                
                float angle = Vector2.SignedAngle(Vector2.right, dashDirection);

                player.DashDirectionIndicator.rotation = Quaternion.Euler(0f, 0f, angle - 45);                                

                if (player.InputHandler.DashInputStop || Time.unscaledTime >= holdStartTime + playerData.maxDashHoldTime)
                {
                    PlaceAfterImage();
                    isHoldingPosition = false;
                    startTime = Time.time;
                    Time.timeScale = 1f;
                    player.DashDirectionIndicator.gameObject.SetActive(false);
                    player.Anim.SetBool("dashHold", false);
                    //Debug.Log(Mathf.Round(dashDirection.x));
                    player.CheckFlipPlayer(xInput);
                    canDash = false;
                }

            }

            if (!isHoldingPosition)
            {
                player.SetVelocity(playerData.dashVelocity, dashDirection);
                CheckIfShouldPlaceAfterImage();

                if (Time.time > startTime + playerData.dashTime)
                {
                    isAbilityDone = true;
                }
            }

           
        }
    }

    private void CheckIfShouldPlaceAfterImage()
    {
        if(Vector2.Distance(player.transform.position, lastAfterImagePosition) > playerData.distnaceBetweenImages)
        {
            PlaceAfterImage();
        }
    }

    private void PlaceAfterImage()
    {
        PlayerAfterImagePool.Instance.GetFromPool();
        lastAfterImagePosition = player.transform.position;
    }

    public void SetCanDash(bool value) => canDash = value;

}
