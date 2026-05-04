using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform), typeof(Button))]
public class ButtonAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float EnterDuration = 0.2f;
    [SerializeField] private Ease EnterEase = Ease.OutBack;
    [SerializeField] private float ExitDuration = 0.3f;
    [SerializeField] private Ease ExitEase = Ease.OutBack;

    [Space]
    [SerializeField] private float ScaleMultiplier = 1.2f;

    [Space]
    [SerializeField] private RectTransform Line;
    [SerializeField] private float TargetLineScale = 1;


    private Button _button;
    private RectTransform _rectTransform;

    private Sequence _currentAnimation;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _button = GetComponent<Button>();

        Vector2 s = Line.localScale;
        s.x = 0;
        Line.localScale = s;
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_currentAnimation != null)
            _currentAnimation.Kill();

        _currentAnimation = DOTween.Sequence();
        _currentAnimation.Append(_rectTransform.DOScale(ScaleMultiplier, EnterDuration).SetEase(EnterEase)).
            Join(Line.DOScaleX(TargetLineScale, EnterDuration).SetEase(EnterEase)).
            SetUpdate(true);
        _currentAnimation.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_currentAnimation != null)
            _currentAnimation.Kill();

        _currentAnimation = DOTween.Sequence();
        _currentAnimation.Append(_rectTransform.DOScale(1, ExitDuration).SetEase(ExitEase)).
            Join(Line.DOScaleX(0, ExitDuration).SetEase(ExitEase)).
            SetUpdate(true);
        _currentAnimation.Play();
    }

    private void OnClick()
    {
        _rectTransform.DOPunchScale(new Vector3(-0.1f, -0.1f, 0), 0.2f, 10, 1f);
    }
}