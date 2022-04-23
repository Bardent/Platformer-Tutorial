using UnityEngine;

public class ParticleManager : CoreComponent
{
  private Transform particleContainer;

  protected override void Awake()
  {
    base.Awake();

    particleContainer = GameObject.FindGameObjectWithTag("ParticleContainer").transform;
  }

  public GameObject StartParticlesWithRandomRotation(GameObject particlesPrefab)
  {
    var randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
    return StartParticles(particlesPrefab, transform.position, randomRotation);
  }

  public GameObject StartParticles(GameObject particlesPrefab)
  {
    return StartParticles(particlesPrefab, transform.position, Quaternion.identity);
  }

  public GameObject StartParticles(GameObject particlesPrefab, Vector2 position, Quaternion rotation)
  {
    return Instantiate(particlesPrefab, position, rotation, particleContainer);
  }
}
