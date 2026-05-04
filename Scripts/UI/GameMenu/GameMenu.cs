using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private Button Menu;

    private void OnEnable()
    {
        Menu.onClick.AddListener(() => SceneLoader.Instance.LoadScene("MainMenu"));
    }

    private void OnDisable()
    {
        Menu.onClick.RemoveListener(() => SceneLoader.Instance.LoadScene("MainMenu"));
    }
}