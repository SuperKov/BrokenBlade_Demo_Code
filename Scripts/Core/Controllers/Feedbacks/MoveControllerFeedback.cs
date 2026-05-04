using UnityEngine;

public abstract class MoveControllerFeedback : MonoBehaviour
{
    [SerializeField] protected MoveController _moveController;

    protected void Awake()
    {
        if (_moveController == null)
        {
            Transform root = transform.root;
            _moveController = root.GetComponentInChildren<MoveController>();
        }
    }
}