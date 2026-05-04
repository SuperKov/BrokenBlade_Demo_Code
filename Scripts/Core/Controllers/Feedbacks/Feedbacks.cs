using System;

namespace MainProject.Feedbacks
{
    [Serializable]
    public struct ShakeFeedback
    {
        public float Intensity;
        public float Duration;

        public ShakeFeedback(float intensity, float duration)
        {
            Intensity = intensity;
            Duration = duration;
        }
    }

    [Serializable]
    public struct TimeFeedback
    {
        public float TimeScale;
        public float Duration;

        public TimeFeedback(float timeScale, float duration)
        {
            TimeScale = timeScale;
            Duration = duration;
        }
    }
}