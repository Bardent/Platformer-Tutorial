using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDodgeStateData", menuName = "Data/State Data/Dodge State")]
public class D_DodgeState : ScriptableObject
{
    public float dodgeSpeed = 10f;
    public float dodgeTime = 0.2f;
    public float dodgeCooldown = 2f;
    public Vector2 dodgeAngle;
}
