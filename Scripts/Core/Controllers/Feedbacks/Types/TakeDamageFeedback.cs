using MainProject.Feedbacks;
using NaughtyAttributes;
using UnityEngine;

public class TakeDamageFeedback : MonoBehaviour
{
    [SerializeField] private Entity _entity;

    [Header("Sound")]
    [Expandable][SerializeField] private SoundData TakeDamage;
    [SerializeField] private AudioSoundPlayer AudioPlayer;

    [Header("Effects")]
    [SerializeField] private bool UseEffects = false;
    [ShowIf("Effects")] [SerializeField] private ShakeFeedback Shake;
    [ShowIf("Effects")] [SerializeField] private TimeFeedback TimeStop;

    public bool Effects => UseEffects;

    private void Awake()
    {
        Transform root = transform.root;

        if (_entity == null) _entity = root.GetComponent<Entity>();
    }

    private void OnEnable()
    {
        _entity.HealthChanged += OnHealthChanged;
    }

    protected virtual void OnHealthChanged(float newHealth, float oldHealth)
    {
        TakeDamage.Play(AudioPlayer);

        if (!UseEffects) return;
        CameraManager.Instance.Shake(Shake);
        TimeManager.Instance.SetTimeForDuration(TimeStop);
    }
}