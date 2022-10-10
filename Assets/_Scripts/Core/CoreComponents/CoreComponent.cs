using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreComponent : MonoBehaviour, ILogicUpdate
{
    protected Core core;

    protected virtual void Awake()
    {
        core = transform.parent.GetComponent<Core>();

        if(core == null) { Debug.LogError("There is no Core on the parent"); }
        core.AddComponent(this);
    }

    public virtual void LogicUpdate() { }

}
