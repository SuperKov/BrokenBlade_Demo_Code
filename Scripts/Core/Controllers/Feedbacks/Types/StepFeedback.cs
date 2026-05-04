using NaughtyAttributes;
using UnityEngine;

public class StepFeedback : MoveControllerFeedback
{
    [Header("Sound")]
    [Expandable]
    [SerializeField] private SoundData Step;
    [SerializeField] private AudioSoundPlayer AudioPlayer;

    private void Start()
    {
        if (AudioPlayer == null)
        {
            Transform root = transform.root;
            AudioPlayer = root.GetComponentInParent<AudioSoundPlayer>();
        }
    }

    private void OnEnable()
    {
        _moveController.StepTriggered += OnStep;
    }

    private void OnDisable()
    {
        _moveController.StepTriggered -= OnStep;
    }

    private void OnStep()
    {
        Step.Play(AudioPlayer);
    }
}