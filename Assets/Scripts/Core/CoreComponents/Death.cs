using UnityEngine;

public class Death : CoreComponent
{
  [SerializeField] private GameObject[] deathParticles;

  private Stats Stats { get => stats ?? core.GetCoreComponent(ref stats); }
  private Stats stats;

  private ParticleManager ParticleManager { get => particleManager ?? core.GetCoreComponent(ref particleManager); }
  private ParticleManager particleManager;

  public override void Init(Core core)
  {
    base.Init(core);

    Stats.HealthZero += Die;
  }

  public void Die()
  {
    foreach (var particle in deathParticles)
    {
      ParticleManager.StartParticles(particle);
    }

    core.transform.parent.gameObject.SetActive(false);
  }

  private void OnEnable()
  {
    if (Stats)
    {
      Stats.HealthZero += Die;
    }
  }

  private void OnDisable()
  {
    Stats.HealthZero -= Die;
  }
}
