using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class LoadingScreen : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private CanvasGroup Screen;
    [SerializeField] private float FadeInDuration = 0.5f;
    [SerializeField] private float FadeOutDuration = 0.8f;
    [SerializeField] private float FadeTextDuration = 0.5f;

    [Header("Sounds")]
    [SerializeField] private AudioClip TextSound;

    [Header("UI")]
    [SerializeField] private Image Bar;
    [SerializeField] private TextMeshProUGUI PressKeyText;

    private Tweener _textShow;
    private AudioSource _audioPlayer;

    private void Awake()
    {
        _audioPlayer = GetComponent<AudioSource>();
    }

    public void UpdateBar(float value)
    {
        if (Bar == null) return;
        Bar.fillAmount = value;
    }

    public void ShowText()
    {
        if (PressKeyText == null) return;
        if (_textShow != null)
            _textShow.Kill();
        _textShow = PressKeyText.DOFade(1, FadeTextDuration);
        _audioPlayer.PlayOneShot(TextSound);
    }

    private void Reset()
    {
        if (_textShow != null)
            _textShow.Kill();
        Bar.fillAmount = 0;
        Color c = PressKeyText.color;
        c.a = 0;
        Screen.alpha = 0;
        PressKeyText.color = c;
    }

    public void Switch(bool mode, Action action)
    {
        if (mode)
        {
            Reset();
            Screen.DOFade(1, FadeInDuration).SetEase(Ease.InOutQuad).OnComplete(() => action?.Invoke());
        }
        else
        {
            Screen.DOFade(0, FadeOutDuration).SetEase(Ease.InOutQuad).OnComplete(() => action?.Invoke());
        }
    }
}