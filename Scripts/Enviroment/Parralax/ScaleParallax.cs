using UnityEngine;
using static Parallax;
using static Unity.Cinemachine.CinemachineFreeLookModifier;

public class ScaleParallax : MonoBehaviour
{
    [SerializeField] private float parallaxEffect;

    private Transform _cameraTransform;
    private Vector3 _lastFollowPosition;
    private float _cameraStartPos;
    private float _startScale;

    private void Start()
    {
        _cameraTransform = Camera.main.transform;

        _lastFollowPosition = _cameraTransform.position;
        _cameraStartPos = _cameraTransform.position.y;

        _startScale = transform.localScale.y;
    }

    private void LateUpdate()
    {
        FollowCamera();
    }

    private void FollowCamera()
    {
        float deltaY = _cameraStartPos - _lastFollowPosition.y;
        transform.localScale  = new Vector3(transform.localScale.x, _startScale + deltaY * parallaxEffect, transform.localScale.z);

        _lastFollowPosition = _cameraTransform.position;
    }
}