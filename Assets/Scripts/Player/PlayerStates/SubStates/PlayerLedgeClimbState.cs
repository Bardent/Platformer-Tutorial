using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerState {

	protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
	private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }

	private Movement movement;
	private CollisionSenses collisionSenses;


	private Vector2 detectedPos;
	private Vector2 cornerPos;
	private Vector2 startPos;
	private Vector2 stopPos;
	private Vector2 workspace;

	private bool isHanging;
	private bool isClimbing;
	private bool jumpInput;
	private bool isTouchingCeiling;

	private int xInput;
	private int yInput;

	public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {
	}

	public override void AnimationFinishTrigger() {
		base.AnimationFinishTrigger();
		player.Anim.SetBool("climbLedge", false);
	}

	public override void AnimationTrigger() {
		base.AnimationTrigger();

		isHanging = true;
	}

	public override void Enter() {
		base.Enter();

		Movement?.SetVelocityZero();
		player.transform.position = detectedPos;
		cornerPos = DetermineCornerPosition();

		startPos.Set(cornerPos.x - (Movement.FacingDirection * playerData.startOffset.x), cornerPos.y - playerData.startOffset.y);
		stopPos.Set(cornerPos.x + (Movement.FacingDirection * playerData.stopOffset.x), cornerPos.y + playerData.stopOffset.y);

		player.transform.position = startPos;

	}

	public override void Exit() {
		base.Exit();

		isHanging = false;

		if (isClimbing) {
			player.transform.position = stopPos;
			isClimbing = false;
		}
	}

	public override void LogicUpdate() {
		base.LogicUpdate();

		if (isAnimationFinished) {
			if (isTouchingCeiling) {
				stateMachine.ChangeState(player.CrouchIdleState);
			} else {
				stateMachine.ChangeState(player.IdleState);
			}
		} else {
			xInput = player.InputHandler.NormInputX;
			yInput = player.InputHandler.NormInputY;
			jumpInput = player.InputHandler.JumpInput;

			Movement?.SetVelocityZero();
			player.transform.position = startPos;

			if (xInput == Movement.FacingDirection && isHanging && !isClimbing) {
				CheckForSpace();
				isClimbing = true;
				player.Anim.SetBool("climbLedge", true);
			} else if (yInput == -1 && isHanging && !isClimbing) {
				stateMachine.ChangeState(player.InAirState);
			} else if (jumpInput && !isClimbing) {
				player.WallJumpState.DetermineWallJumpDirection(true);
				stateMachine.ChangeState(player.WallJumpState);
			}
		}

	}

	public void SetDetectedPosition(Vector2 pos) => detectedPos = pos;

	private void CheckForSpace() {
		isTouchingCeiling = Physics2D.Raycast(cornerPos + (Vector2.up * 0.015f) + (Vector2.right * Movement.FacingDirection * 0.015f), Vector2.up, playerData.standColliderHeight, CollisionSenses.WhatIsGround);
		player.Anim.SetBool("isTouchingCeiling", isTouchingCeiling);
	}

	private Vector2 DetermineCornerPosition() {
		RaycastHit2D xHit = Physics2D.Raycast(CollisionSenses.WallCheck.position, Vector2.right * Movement.FacingDirection, CollisionSenses.WallCheckDistance, CollisionSenses.WhatIsGround);
		float xDist = xHit.distance;
		workspace.Set((xDist + 0.015f) * Movement.FacingDirection, 0f);
		RaycastHit2D yHit = Physics2D.Raycast(CollisionSenses.LedgeCheckHorizontal.position + (Vector3)(workspace), Vector2.down, CollisionSenses.LedgeCheckHorizontal.position.y - CollisionSenses.WallCheck.position.y + 0.015f, CollisionSenses.WhatIsGround);
		float yDist = yHit.distance;

		workspace.Set(CollisionSenses.WallCheck.position.x + (xDist * Movement.FacingDirection), CollisionSenses.LedgeCheckHorizontal.position.y - yDist);
		return workspace;
	}

}
