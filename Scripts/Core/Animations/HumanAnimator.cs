using UnityEngine;

public class HumanAnimator : MonoBehaviour
{
    [SerializeField] private FrameAnimator bodyAnimator;
    [SerializeField] private FrameAnimator armsAnimator;

    public FrameAnimator BodyAnimator => bodyAnimator;
    public FrameAnimator ArmsAnimator => armsAnimator;

    public void Play(string id, bool ignoreRepeat = false)
    {
        bodyAnimator.Play(id, ignoreRepeat);
        armsAnimator.Play(id, ignoreRepeat);
    }

    public void Play(string id, float speed, bool ignoreRepeat = false)
    {
        bodyAnimator.Play(id, speed, ignoreRepeat);
        armsAnimator.Play(id, speed, ignoreRepeat);
    }
}