using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.InputSystem.XInput;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 RawMovementInput { get; private set; }
    public int NormalizedInputX { get; private set; }
    public int NormalizedInputY { get; private set; }
    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public bool DodgeRollInput { get; private set; }
    public bool DashInput { get; private set; }
    public bool DashInputStop { get; private set; }
    public bool GrabInput { get; private set; }
    public bool GrabInputStop { get; private set; }

    [SerializeField] private float inputHoldTime = 0.2f;
    
    private float jumpInputStartTime;
    private float dodgeRollInputStartTime;
    private float dashInputStartTime;
    private float grabInputStartTime;

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDodgeRollInputHoldTime();
        CheckDashInputHoldTime();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();
        if(Mathf.Abs(RawMovementInput.x) > 0.5f)
        {
            NormalizedInputX = (int)(RawMovementInput * Vector2.right).normalized.x;
        }
        else
        {
            NormalizedInputX = 0;
        }
        if(Mathf.Abs(RawMovementInput.y) > 0.5f)
        {
            NormalizedInputY = (int)(RawMovementInput * Vector2.up).normalized.y;
        }
        else
        {
            NormalizedInputY = 0;
        }
        
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            JumpInputStop = true;
        }
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DashInput = true;
            DashInputStop = false;
            dashInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            DashInputStop = true;
        }
    }

    public void OnDodgeInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DodgeRollInput = true;
            dodgeRollInputStartTime = Time.time;
        }
    }

    public void OnGrabInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GrabInput = true;           
        }

        if (context.canceled)
        {
            GrabInput = false;
        }
    }

    public void CheckJumpInputHoldTime()
    {
        if (JumpInput && Time.time > jumpInputStartTime + inputHoldTime)
        {
            JumpInput = false;            
        }
    }

    public void CheckDodgeRollInputHoldTime()
    {
        if (DodgeRollInput && Time.time > dodgeRollInputStartTime + inputHoldTime)
        {
            DodgeRollInput = false;
        }
    }

    public void CheckDashInputHoldTime()
    {
        if (DashInput && Time.time > dashInputStartTime + inputHoldTime)
        {
            DashInput = false;
        }
    }  

    public void UseJumpInput() => JumpInput = false;

    public void UseDodgeRollInput() => DodgeRollInput = false;

    public void UseDashInput() => DashInput = false;

}
