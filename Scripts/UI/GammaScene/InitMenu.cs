using UnityEngine.UI;
using UnityEngine;

public class InitMenu : MonoBehaviour
{
    [SerializeField] private string SceneName = "MainMenu";

    [Space]
    [SerializeField] private Button Ready;

    private void OnEnable()
    {
        Ready.onClick.AddListener(OnReady);
    }

    private void OnDisable()
    {
        Ready.onClick.RemoveListener(OnReady);
    }

    private void OnReady()
    {
        SceneLoader.Instance.LoadScene(SceneName);
    }
}