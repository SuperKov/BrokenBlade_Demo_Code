using MainProject.Feedbacks;
using NaughtyAttributes;
using UnityEngine;

public class ParryFeedback : MonoBehaviour
{
    [SerializeField] private BlockController block;

    [Header("Sound")]
    [Expandable][SerializeField] private SoundData parrySound;
    [SerializeField] private AudioSoundPlayer audioPlayer;

    [Header("Effects")]
    [SerializeField] private bool useEffects = false;
    [ShowIf(nameof(useEffects))][SerializeField] private ShakeFeedback shake;
    [ShowIf(nameof(useEffects))][SerializeField] private TimeFeedback timeStop;

    private void OnEnable()
    {
        block.Parried += OnParried;
    }

    private void OnDisable()
    {
        block.Parried -= OnParried;
    }

    private void OnParried()
    {
        parrySound.Play(audioPlayer);
    }
}