using UnityEngine.Rendering;
using UnityEngine.Audio;
using UnityEngine;

public class PlayerHealthView : MonoBehaviour
{
    [SerializeField] private PlayerStats _stats;

    [Header("Options")]
    [SerializeField] private float speed = 5f;

    [Header("Sound")]
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private string parameterName = "HealthLowPass";
    [SerializeField] private float normalFreq = 22000f; // Полностью открытый звук
    [SerializeField] private float muffledFreq = 500f;   // Глухой звук (под водой)
    [SerializeField] private float criticalHealthThreshold = 0.3f; // 30% HP

    private float _healthPercent = 1;
    private Volume _lowHPVolume;

    private void Start()
    {
        _lowHPVolume = VolumeManager.Instance.HealthVolume;
    }

    private void OnEnable()
    {
        _stats.HealthChanged += UpdateVisuals;
    }

    private void OnDisable()
    {
        _stats.HealthChanged -= UpdateVisuals;
    }

    private void Update()
    {
        if (_lowHPVolume == null) return;

        _lowHPVolume.weight = Mathf.Lerp(_lowHPVolume.weight,
            1 - _healthPercent,
            Time.deltaTime * speed);

        // Если здоровье ниже порога, начинаем "глушить" звук
        float targetFreq = normalFreq;

        if (_healthPercent < criticalHealthThreshold)
        {
            // Рассчитываем степень глухоты внутри критического порога
            float t = _healthPercent / criticalHealthThreshold;
            targetFreq = Mathf.Lerp(muffledFreq, normalFreq, t);
        }

        mainMixer.SetFloat(parameterName, targetFreq);
    }

    private void UpdateVisuals(float currentHealth, float maxHealth)
    {
        _healthPercent = currentHealth / maxHealth;
    }
}