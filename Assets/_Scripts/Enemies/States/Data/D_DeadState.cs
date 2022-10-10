using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDeadStateData", menuName = "Data/State Data/Dead State")]
public class D_DeadState : ScriptableObject
{
    public GameObject deathChunkParticle;
    public GameObject deathBloodParticle;
}
