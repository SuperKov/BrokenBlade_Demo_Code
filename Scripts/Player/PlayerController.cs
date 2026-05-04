using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(PlayerStats)), SelectionBase]
public class PlayerController : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private PlayerAnimator _view;

    private PlayerStateFactory _states;

    public MoveController Movement => Stats.Movement;
    public CombatController Combat => Stats.Combat;
    public BlockController Block => Stats.Block;
    public PlayerAnimator View => _view;

    public bool IsFocusing { get; private set; }
    public bool IsRunning { get; private set; }

    public PlayerStats Stats { get; private set; }

    private void Awake()
    {
        Stats = GetComponent<PlayerStats>();
        if (_view == null) _view = GetComponent<PlayerAnimator>();
        _states = new PlayerStateFactory(this);
    }

    private void Start()
    {
        CameraManager.Instance.SetPrimalTarget(transform);

        Stats.Movement.SetData(Stats.Stats.Movement);
        Stats.Combat.SetData(Stats.Stats.Combat);
        Stats.Block.SetData(Stats.Stats.Block);

        _states.CurrentState = _states.Move;
        _states.CurrentState.EnterState();
    }

    private void OnEnable()
    {
        var playerActions = InputManager.Instance.Actions.Player;

        playerActions.Focus.performed += OnFocusAction;
        playerActions.Sprint.performed += OnSprintAction;
    }

    private void OnDisable()
    {
        if (InputManager.Instance == null) return;

        var playerActions = InputManager.Instance.Actions.Player;

        playerActions.Focus.performed -= OnFocusAction;
        playerActions.Sprint.performed -= OnSprintAction;
    }

    private void Update()
    {
        if (Stats.InCutscene)
        {
            Debug.Log("IN CUTSCENE");
            return;
        }
        _states.CurrentState.UpdateState();
    }

    public void StartBlocking()
    {
        Block.StartBlocking();
        Movement.BlockMoving = true;
    }

    public void StopBlocking()
    {
        Block.StopBlocking();
        Movement.BlockMoving = false;
    }

    private void OnFocusAction(InputAction.CallbackContext context) => IsFocusing = context.ReadValueAsButton();
    private void OnSprintAction(InputAction.CallbackContext context) => IsRunning = context.ReadValueAsButton();
}