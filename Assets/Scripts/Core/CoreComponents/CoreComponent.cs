using UnityEngine;

public class CoreComponent : MonoBehaviour, ILogicUpdate
{
  protected Core core;

  public virtual void Init(Core core){
    this.core = core;
  }

  protected virtual void Awake() { }

  public virtual void LogicUpdate() { }
}
