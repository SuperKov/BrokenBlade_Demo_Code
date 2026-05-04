using UnityEngine.Rendering;
using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] private Volume healthVolume;

    public Volume HealthVolume => healthVolume;

    public static VolumeManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}