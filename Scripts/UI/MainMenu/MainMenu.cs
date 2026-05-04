using UnityEngine.UI;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button Polygon;
    [SerializeField] private string PolygonSceneName = "Gym";

    private void OnEnable()
    {
        Polygon.onClick.AddListener(OnPolygon);
    }

    private void OnDisable()
    {
        Polygon.onClick.RemoveListener(OnPolygon);
    }

    private void OnPolygon()
    {
        SceneLoader.Instance.LoadScene(PolygonSceneName);
    }
}