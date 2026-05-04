using UnityEngine.InputSystem;
using NaughtyAttributes;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public enum ParallaxTarget
    {
        Camera,
        Cursor
    }

    [SerializeField] private ParallaxTarget Target = ParallaxTarget.Camera;

    [SerializeField] private float _modifierY = 1f;

    [ShowIf("IsCameraTarget")]
    [Tooltip("0 = движется вместе с камерой, 1 = стоит на месте, 0.5 = параллакс")]
    [BoxGroup("Camera")] [SerializeField] private float _parallaxEffect;

    [ShowIf("IsCursorTarget")]
    [BoxGroup("Cursor")] [SerializeField] private float CursorMultiplier = 1f;
    [ShowIf("IsCursorTarget")]
    [BoxGroup("Cursor")] [SerializeField] private float Speed = 1f;

    private Transform _cameraTransform;
    private Vector3 _lastFollowPosition;
    private Vector3 _startPosition;

    public bool IsCameraTarget => Target == ParallaxTarget.Camera;
    public bool IsCursorTarget => Target == ParallaxTarget.Cursor;

    private void Start()
    {
        if (Target == ParallaxTarget.Camera)
        {

            _cameraTransform = Camera.main.transform;
            _lastFollowPosition = _cameraTransform.position;
        }
        else if (Target == ParallaxTarget.Cursor)
            _startPosition = transform.position;
    }

    private void LateUpdate()
    {
        FollowCamera();
        FollowCursor();
    }

    public void SetValues(float parallax, float modifierY)
    {
        _modifierY = modifierY;
        _parallaxEffect = parallax;
    }

    private void FollowCamera()
    {
        if (Target != ParallaxTarget.Camera) return;
        Vector3 deltaMovement = _cameraTransform.position - _lastFollowPosition;
        transform.position += new Vector3(deltaMovement.x * _parallaxEffect, deltaMovement.y * _parallaxEffect * _modifierY, 0);

        _lastFollowPosition = _cameraTransform.position;
    }

    private void FollowCursor()
    {
        if (Target != ParallaxTarget.Cursor) return;

        Vector3 mousePos = Mouse.current.position.ReadValue();
        float mouseNormalizedX = (mousePos.x / Screen.width) - 0.5f;
        float mouseNormalizedY = (mousePos.y / Screen.height) - 0.5f;

        // Вычисляем целевое смещение
        // Мы используем Lerp для плавности, чтобы фон не дергался при резких движениях мыши
       // Vector3 targetMouseOffset = new Vector3(mouseNormalizedX, mouseNormalizedY, 0) * _parallaxEffect;
       Vector3 targetOffset = new(
           mouseNormalizedX * CursorMultiplier,
           mouseNormalizedY * CursorMultiplier * _modifierY,
           0);

        // Применяем смещение (локально или через добавление к позиции)
        // Здесь мы немного хитрим: используем локальное смещение относительно "базовой" позиции параллакса
        Vector3 targetPosition = _startPosition + targetOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * Speed);
        _lastFollowPosition = mousePos;
    }
}