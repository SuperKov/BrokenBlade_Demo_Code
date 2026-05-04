using MainProject.Feedbacks;
using NaughtyAttributes;
using UnityEngine;

public class AttackFeedback : MonoBehaviour
{
    [SerializeField] private CombatController _combatController;

    [Header("Sound")]
    [Expandable] [SerializeField] private SoundData Hit;
    [Expandable] [SerializeField] private SoundData NoHit;
    [SerializeField] private AudioSoundPlayer AudioPlayer;

    [Header("Effects")]
    [SerializeField] private bool UseEffects = false;
    [ShowIf(nameof(UseEffects))] [SerializeField] private ShakeFeedback Shake;
    [ShowIf(nameof(UseEffects))][SerializeField] private TimeFeedback TimeStop;

    private void OnValidate()
    {
        Transform root = transform.root;

        if (AudioPlayer == null) AudioPlayer = root.GetComponentInParent<AudioSoundPlayer>();
    }

    protected void Start()
    {
        Transform root = transform.root;
        if (_combatController == null)
            _combatController = root.GetComponentInChildren<CombatController>();
    }

    protected void OnEnable()
    {
        _combatController.Attacked += OnAttacked;
    }

    protected void OnDisable()
    {
        _combatController.Attacked -= OnAttacked;
    }

    protected virtual void OnAttacked(bool hit)
    {
        if (hit)
        {
            Hit.Play(AudioPlayer);

            if (!UseEffects) return;
            CameraManager.Instance.Shake(Shake);
            TimeManager.Instance.SetTimeForDuration(TimeStop);
        }
        else
            NoHit.Play(AudioPlayer);
    }
}