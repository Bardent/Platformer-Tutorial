using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedState : State {
	protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
	private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }

	private Movement movement;
	private CollisionSenses collisionSenses;

	protected D_PlayerDetected stateData;

	protected bool isPlayerInMinAgroRange;
	protected bool isPlayerInMaxAgroRange;
	protected bool performLongRangeAction;
	protected bool performCloseRangeAction;
	protected bool isDetectingLedge;

	public PlayerDetectedState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetected stateData) : base(etity, stateMachine, animBoolName) {
		this.stateData = stateData;
	}

	public override void DoChecks() {
		base.DoChecks();

		isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
		isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
		isDetectingLedge = CollisionSenses.LedgeVertical;
		performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
	}

	public override void Enter() {
		base.Enter();

		performLongRangeAction = false;
		Movement?.SetVelocityX(0f);
	}

	public override void Exit() {
		base.Exit();
	}

	public override void LogicUpdate() {
		base.LogicUpdate();

		Movement?.SetVelocityX(0f);

		if (Time.time >= startTime + stateData.longRangeActionTime) {
			performLongRangeAction = true;
		}
	}

	public override void PhysicsUpdate() {
		base.PhysicsUpdate();
	}
}
