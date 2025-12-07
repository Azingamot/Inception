using UnityEngine;

public class ReturnParticlesToPool : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        ObjectPoolController.ReturnObjectToPool(gameObject, ObjectPoolController.PoolType.ParticleSystem);
    }
}
