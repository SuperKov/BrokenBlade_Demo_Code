public class PlayerStateFactory
{
    public PlayerBaseState CurrentState;

    private PlayerController _context;

    // Кэшированные экземпляры состояний
    private PlayerBaseState _recovery;
    private PlayerBaseState _attack;
    private PlayerBaseState _focus;
    private PlayerBaseState _block;
    private PlayerBaseState _move;

    public PlayerBaseState Recovery => _recovery;
    public PlayerBaseState Attack => _attack;
    public PlayerBaseState Focus => _focus;
    public PlayerBaseState Block => _block;
    public PlayerBaseState Move => _move;

    public PlayerStateFactory(PlayerController currentContext)
    {
        _context = currentContext;

        // Создаем каждое состояние, передавая контекст и саму фабрику
        _move = new PlayerMoveState(_context, this);
        _block = new PlayerBlockState(_context, this);
        _focus = new PlayerFocusState(_context, this);
        _attack = new PlayerAttackState(_context, this);
        _recovery = new PlayerRecoveryState(_context, this);
    }
}