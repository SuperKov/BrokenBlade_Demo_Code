using UnityEngine.InputSystem;
using Unity.Cinemachine;
using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    public enum CameraType
    {
        Game,
        Cutscene
    }

    [SerializeField] private CinemachineCamera GameCamera;

    [Header("Focus")]
    [SerializeField] private float MaxOffset = 3f;
    [SerializeField] private float SmoothSpeed = 5f;
    [SerializeField] private float ZoomDuration = 0.4f;

    private CinemachineBasicMultiChannelPerlin _noise;
    private CinemachineCameraOffset _offsetExtension;

    private CinemachineCamera _currentCamera;
    private Transform _primalTarget;
    private Vector3 _defaultOffset;
    private Vector3 _targetOffset;

    private Tweener _currentZoom;

    private float _currentLensZoom;
    private float _defaultLensZoom;
    private float _timer;

    private bool _isFocusing;

    public static CameraManager Instance { get; private set; }

    private void Awake()
    {
        if (GameCamera == null)
        {
            Debug.LogError("Отсутствует компоненты камеры");
            gameObject.SetActive(false);
            return;
        }

        Instance = this;

        _currentCamera = GameCamera;

        _offsetExtension = GameCamera.GetComponent<CinemachineCameraOffset>();
        _noise = (CinemachineBasicMultiChannelPerlin)GameCamera.GetCinemachineComponent(CinemachineCore.Stage.Noise);

        _defaultOffset = _offsetExtension.Offset;

        _defaultLensZoom = GameCamera.Lens.OrthographicSize;
        _currentLensZoom = _defaultLensZoom;
    }

    private void Start()
    {
        if (PlayerStats.Instance != null)
            GameCamera.Target.TrackingTarget = PlayerStats.Instance.transform;
        else
            Debug.LogError("На сцене отсутствует игрок!");
    }

    private void Update()
    {
        CheckShakeTimer();
        Focus();
    }

    public Vector3 GetWorldMousePosition()
    {
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
        mouseScreenPos.z = Mathf.Abs(Camera.main.transform.position.z);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mousePos.z = 0;

        return mousePos;
    }

    public void SetPrimalTarget(Transform target)
    {
        _primalTarget = target; 
    }

    public void ChangeCamera(CinemachineCamera camera)
    {
        _currentCamera = camera;
    }

    public void ResetCamera()
    {
        _currentCamera = GameCamera;
    }

    public void SetZoom(float size)
    {
        if (_currentZoom != null)
            _currentZoom.Kill();

        _currentLensZoom = _defaultLensZoom / size;

        _currentZoom = DOVirtual.Float(GameCamera.Lens.OrthographicSize, _currentLensZoom, ZoomDuration, v => {
            GameCamera.Lens.OrthographicSize = v;
        }).SetEase(Ease.OutQuad);
    }

    public void ResetZoom()
    {
        if (_currentZoom != null)
            _currentZoom.Kill();

        _currentLensZoom = _defaultLensZoom;

        _currentZoom = DOVirtual.Float(GameCamera.Lens.OrthographicSize, _currentLensZoom, ZoomDuration, v => {
            GameCamera.Lens.OrthographicSize = v;
        }).SetEase(Ease.OutQuad);
    }

    public void Shake(MainProject.Feedbacks.ShakeFeedback shake)
    {
        if (_noise == null) return;

        _noise.AmplitudeGain = shake.Intensity;
        _timer = shake.Duration;
    }

    public void SwitchFocus(bool mode)
    {
        _isFocusing = mode;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void CheckShakeTimer()
    {
        if (_noise == null || _timer <= 0) return;

        _timer -= Time.unscaledDeltaTime;
        if (_timer <= 0)
        {
            _noise.AmplitudeGain = 0;
        }
    }

    private void Focus()
    {
        if (_currentCamera == null) return;

        if (_isFocusing)
            CalculateMouseOffset();
        else
            _targetOffset = Vector3.zero;

        _offsetExtension.Offset = Vector3.Lerp(_offsetExtension.Offset, _targetOffset, Time.deltaTime * SmoothSpeed);
        if (Mathf.Abs(_offsetExtension.Offset.x) < 0.01f + Mathf.Abs(_defaultOffset.x) &&
            Mathf.Abs(_offsetExtension.Offset.y) < 0.01f + Mathf.Abs(_defaultOffset.y))
        {
            _offsetExtension.Offset = Vector3.zero;
        }
    }

    private void CalculateMouseOffset()
    {
        Vector3 mousePos = GetWorldMousePosition();

        // Вычисляем направление от игрока к мыши
        Vector3 direction = mousePos - _primalTarget.position;

        // Ограничиваем смещение, чтобы камера не улетала слишком далеко
        _targetOffset = Vector3.ClampMagnitude(direction * 0.5f, MaxOffset);
    }
}
