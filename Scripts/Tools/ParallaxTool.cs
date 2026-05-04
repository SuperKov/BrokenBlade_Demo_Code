using NaughtyAttributes;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ParallaxTool : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private float defaultModifierY;
    [SerializeField] private float defaultParallax;

    [Header("Depth")]
    [SerializeField] private float yMultiplier;
    [SerializeField] private float parallaxMultiplier;

    [Header("Options")]
    [SerializeField] private bool clampMaxParallax1;

    [Header("Parallaxes")]
    [SerializeField] private Parallax[] parallaxes;

    [Button("Set Values")]
    public void SetValues()
    {
        float defPar = defaultParallax;
        float defY = defaultModifierY;

        foreach (var parallax in parallaxes)
        {
            parallax.SetValues(defPar, defY);

            defPar = (float)System.Math.Round(defPar * parallaxMultiplier, 2);
            if (clampMaxParallax1 && defPar > 1)
                defPar = 1;

            defY = (float)System.Math.Round(defY * yMultiplier, 2);
        }

        Debug.Log("PARRALAX IS DONE");
    }

    [Button]
    public void ResetList()
    {
        parallaxes = null;
    }
}