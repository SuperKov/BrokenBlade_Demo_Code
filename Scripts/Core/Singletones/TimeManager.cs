using UnityEngine.SceneManagement;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    private Coroutine _currentTimeDuration;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void AutoInit()
    {
        // Просто обращаемся к Instance, чтобы сработала логика создания
        var i = Instance;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SetTimeForDuration(float time, float duration)
    {
        if (_currentTimeDuration != null) return;
        _currentTimeDuration = StartCoroutine(TimeForDuration(time, duration));
    }

    public void SetTimeForDuration(MainProject.Feedbacks.TimeFeedback time)
    {
        if (_currentTimeDuration != null) return;
        _currentTimeDuration = StartCoroutine(TimeForDuration(time.TimeScale, time.Duration));
    }

    public void ResetTime()
    {
        if (_currentTimeDuration != null)
            StopCoroutine(_currentTimeDuration);
        Time.timeScale = 1f;
    }

    private IEnumerator TimeForDuration(float time, float duration)
    {
        Time.timeScale = time;
        yield return new WaitForSecondsRealtime(duration);
        _currentTimeDuration = null;
        Time.timeScale = 1;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        ResetTime();
    }

#if UNITY_EDITOR

    [Button]
    public void _ResetTime()
    {
        if (_currentTimeDuration != null)
            StopCoroutine(_currentTimeDuration);
        Time.timeScale = 1;
    }

    [Button]
    public void _ReloadScene()
    {
        if (_currentTimeDuration != null)
            StopCoroutine(_currentTimeDuration);
        Time.timeScale = 1;
        SceneLoader.Instance.LoadScene(SceneManager.GetActiveScene().name);
    }

    [Button]
    public void _ShowTimeInfo()
    {
        Debug.Log("Текущее время: " + Time.timeScale);
        Debug.Log("Есть ли куратина на остановку времени: " + (_currentTimeDuration != null));
    }
#endif
}