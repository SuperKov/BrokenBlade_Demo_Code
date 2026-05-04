using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class SceneLoader : Singleton<SceneLoader>
{
    public string CurrentScene => SceneManager.GetActiveScene().name;

    private const float _minLoadingDuration = 1f;

    private LoadingScreen _loading;

    private AsyncOperation _currentLoading;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void AutoInit()
    {
        // Просто обращаемся к Instance, чтобы сработала логика создания
        var i = Instance;
    }

    private LoadingScreen GetLoading()
    {
        if (_loading == null)
            return InitLoadingMenu();
        return _loading;
    }

    private LoadingScreen InitLoadingMenu()
    {
        _loading = Instantiate(Resources.Load<LoadingScreen>("Project/UI/LoadingCanvas"));
        DontDestroyOnLoad(_loading);
        return _loading;
    }

    private void Start()
    {
        if (_loading == null)
            InitLoadingMenu();
    }

    public void LoadScene(string name)
    {
        if (_currentLoading != null) return;

        _currentLoading = SceneManager.LoadSceneAsync(name);
        _currentLoading.allowSceneActivation = false;

        GetLoading().gameObject.SetActive(true);
        GetLoading().Switch(true, null);

        StartCoroutine(WaitSceneLoad());
    }

    private IEnumerator WaitSceneLoad()
    {

        float timer = 0;
        while (_currentLoading.progress < 0.9f || timer < _minLoadingDuration)
        {
            timer += Time.deltaTime;
            GetLoading().UpdateBar(Mathf.Clamp(_currentLoading.progress, 0, timer) + 0.1f);
            yield return null;
        }

        AudioController.Instance.DisableGlobalVolume(2);
        GetLoading().ShowText();

        yield return new WaitUntil(() => InputManager.Instance.Actions.Player.AnyKey.WasPerformedThisFrame());
        _currentLoading.allowSceneActivation = true;

        yield return new WaitUntil(() => _currentLoading.isDone);
        AudioController.Instance.EnableGlobalVolume(1);
        GetLoading().Switch(false, () => GetLoading().gameObject.SetActive(false));
        _currentLoading = null;
    }
}