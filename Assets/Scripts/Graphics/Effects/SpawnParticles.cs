using System.Collections.Generic;
using UnityEngine;

public class SpawnParticles : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> particles;
     
    public void Spawn(int id)
    {
        particles[id].Play(false);
    }
}
