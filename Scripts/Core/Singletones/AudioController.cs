using UnityEngine;
using DG.Tweening;

public class AudioController : Singleton<AudioController>
{
    private Tween _switchGlobalVolume;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void AutoInit()
    {
        // Просто обращаемся к Instance, чтобы сработала логика создания
        var i = Instance;
    }

    public void DisableGlobalVolume(float duration)
    {
        if (_switchGlobalVolume != null)
            _switchGlobalVolume.Kill();

        _switchGlobalVolume = DOTween.To(() => AudioListener.volume, x => AudioListener.volume = x, 0, duration)
           .SetUpdate(true);
    }

    public void EnableGlobalVolume(float duration)
    {
        if (_switchGlobalVolume != null)
            _switchGlobalVolume.Kill();

        _switchGlobalVolume = DOTween.To(() => AudioListener.volume, x => AudioListener.volume = x, 1, duration)
           .SetUpdate(true);
    }
}