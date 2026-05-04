using UnityEngine;
using TMPro;

public class StateDisplay : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private StateMachine state;
    [SerializeField] private float offset_Y = 1f;

    private TextMeshProUGUI _text;
    private GameObject _canvas;

    private void Start()
    {
        _canvas = Instantiate(canvas, state.transform);
        _text = _canvas.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        Vector2 pos = state.transform.position;
        pos.y += offset_Y;
        _canvas.transform.position = pos;
        _text.text = state.CurrentState.GetType().Name;
    }
}