using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{    
    [Header("Move State")]
    public float movementSpeed = 10f;

    [Header("Jump State")]
    public int amountOfJumps = 1;
    public float jumpVelocity = 20f;

    [Header("Wall Jump State")]
    public float wallJumpVelocity = 20f;
    public float wallJumpTime = 0.2f;
    public Vector2 wallJumpAngle = new Vector2(1, 1);

    [Header("In Air State")]
    public float coyoteTime = 0.2f;
    public float varialbeJumpHeightMultuplier = 0.5f;

    [Header("Wall Slide Sate")]
    public float wallSlideVelocity = 2f;

    [Header("Ledge Climb State")]
    public Vector2 startPosOffset;
    public Vector2 endPosOffset;

    [Header("Dodge Roll State")]
    public float DodgeRollSpeed = 10f;
    public float DodgeRollInitialSpeed = 13f;

    [Header("Dash State")]
    public float dashVelocity = 30f;
    public float dashTime = 0.5f;
    public float maxDashHoldTime = 1f;
    public float distnaceBetweenImages = 0.5f;

    [Header("Wall Climb State")]
    public float wallClimbSpeed = 3f;

    [Header("Crouch State")]
    public float crouchMoveSpeed = 5f;
    public float crouchColliderHeight = 0.8f;
    public float standColliderHeight = 1.6f;

    [Header("Check Variables")]
    public float groundCheckRadius = 0.23f;
    public float wallCheckDistance = 0.2f;
    public LayerMask whatIsGround;
}
