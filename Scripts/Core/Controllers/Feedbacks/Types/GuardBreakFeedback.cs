using MainProject.Feedbacks;
using NaughtyAttributes;
using UnityEngine;

public class GuardBreakFeedback : MonoBehaviour
{
    [SerializeField] private BlockController block;

    [Header("Sound")]
    [Expandable][SerializeField] private SoundData sound;
    [SerializeField] private AudioSoundPlayer audioPlayer;

    [Header("Effects")]
    [SerializeField] private bool useEffects = false;
    [ShowIf(nameof(useEffects))][SerializeField] private ShakeFeedback shake;
    [ShowIf(nameof(useEffects))][SerializeField] private TimeFeedback timeStop;

    private void OnEnable()
    {
        block.GuardBreak += OnGuardBreak;
    }

    private void OnDisable()
    {
        block.GuardBreak -= OnGuardBreak;
    }

    private void OnGuardBreak()
    {
        sound.Play(audioPlayer);
    }
}