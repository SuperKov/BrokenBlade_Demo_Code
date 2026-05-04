using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine;


public class GammaSettings : MonoBehaviour
{
    [SerializeField] private Slider _gammaSlider;
    [SerializeField] private Volume _postProcessVolume;

    private ColorAdjustments _colorAdjustments;

    private void Start()
    {
        if (_postProcessVolume.profile.TryGet(out _colorAdjustments))
        {
            float value = SavesManager.Instance.LoadFloat("Gamma");
            UpdateGamma(value);
        }

        _gammaSlider.onValueChanged.AddListener(UpdateGamma);
    }

    public void UpdateGamma(float value)
    {
        if (_colorAdjustments != null)
        {
            _colorAdjustments.postExposure.value = value;

            SavesManager.Instance.SaveFloat("Gamma", value);
            SavesManager.Instance.Save();
        }
    }
}