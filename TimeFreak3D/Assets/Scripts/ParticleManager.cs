using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public GameObject[] bulletParticle;
    public static ParticleManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }
}
