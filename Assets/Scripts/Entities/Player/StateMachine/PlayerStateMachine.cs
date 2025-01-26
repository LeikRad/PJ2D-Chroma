using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    // Basic Movemnt
    public float _moveSpeed = 6f;
    public float _jumpForce = 7f;
    public float _gravity = -9.81f;
    public float _terminalVelocity = -10f;
    private float _horizontal;
    private bool _isFacingRight = true;
    
    // Jumps
    int _maxJumps = 1;
    private int _jumpCount = 0;
    
    // Dash
    float _dashingForce = 24f;
    float _dashingTime = 0.2f;
    float _dashingCooldown = 1f;
    bool _canDash = true;
    bool _isDashing;
    bool _dashEnabled = false;
    
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _groundCheck;

    private bool _canMove = true;
    private Vector3 _velocity;
    bool _isGrounded;
    public Animator _animator;
    
    PlayerInputMap _playerInput;

    PlayerBaseState _currentState;
    
    
    private PlayerStateFactory _states;
    
    Vector2 _currentMovementInput;
    Vector2 _appliedMovement;
    bool _isMovementPressed;
    bool _isJumpPressed;
    bool _isDashPressed;
    
    
    // getters and setters
    public int maxJumps { get => _maxJumps; set => _maxJumps = value; }
    public bool dashEnabled { get => _dashEnabled; set => _dashEnabled = value; }
    public bool isDashPressed { get => _isDashPressed; }
    public bool isFacingRight { get => _isFacingRight; }
    public float dashSpeed { get => _dashingForce; }
    public float dashTime { get => _dashingTime; }
    public float dashCooldown { get => _dashingCooldown; }
    
    public bool canDash { get => _canDash; set => _canDash = value; }
    public float terminalVelocity { get => _terminalVelocity; }
    public float gravity { get => _gravity; set => _gravity = value; }
    public float moveSpeed { get => _moveSpeed; }
    public float currentMovementX { get => _currentMovementInput.x; set => _currentMovementInput.x = value; }
    public float currentMovementY { get => _currentMovementInput.y; set => _currentMovementInput.y = value; }
    public float appliedMovementX { get => _appliedMovement.x; set => _appliedMovement.x = value; }
    public float appliedMovementY { get => _appliedMovement.y; set => _appliedMovement.y = value; }
    
    public bool isMovementPressed { get => _isMovementPressed; set => _isMovementPressed = value; }
    public int jumpCount { get => _jumpCount; set => _jumpCount = value; }
    public Animator animator { get => _animator; }
    public bool isGrounded { get => _isGrounded; }
    public bool isJumpPressed { get => _isJumpPressed; }
    
    public Rigidbody2D rb { get => _rb; }
    public float jumpForce { get => _jumpForce; }
    public PlayerBaseState CurrentState { 
        get => _currentState;
        set => _currentState = value;
    }
    
    public void Awake()
    {
        _playerInput = new PlayerInputMap();
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();

        _playerInput.CharacterControls.Move.started += onMovementInput;
        _playerInput.CharacterControls.Move.performed += onMovementInput;
        _playerInput.CharacterControls.Move.canceled += onMovementInput;
        
        _playerInput.CharacterControls.Jump.started += onJump;
        _playerInput.CharacterControls.Jump.canceled += onJump;
        
        _playerInput.CharacterControls.Dash.started += onDash;
        _playerInput.CharacterControls.Dash.canceled += onDash;
    }
    
    void Start()
    {   
        _rb.linearVelocity = _appliedMovement * Time.deltaTime;
    }
    public void Update()
    {
        // handle movement/rotation etc
        IsGroundedCheck();
        _currentState.UpdateStates();
        Flip();
        rb.linearVelocity = _appliedMovement;
    }
    
    // callback handle functions for movement

    private void IsGroundedCheck()
    { 
        if (_rb.linearVelocityY > 0)
        {
            _isGrounded = false;
            return;
        }
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, 0.1f, _groundLayer);
    }

    void onJump(InputAction.CallbackContext ctx)
    {
        _isJumpPressed = ctx.ReadValueAsButton();
    }
    
    void onMovementInput(InputAction.CallbackContext ctx)
    {
        _currentMovementInput.x = ctx.ReadValue<float>();
        _isMovementPressed = _currentMovementInput.x != 0;
    }
    
    void onDash(InputAction.CallbackContext ctx)
    {
        _isDashPressed = ctx.ReadValueAsButton();
    }
    
    void OnEnable()
    {
        _playerInput.CharacterControls.Enable();
    }
    
    void OnDisable()
    {
        _playerInput.CharacterControls.Disable();
    }
    
    private void Flip()
    {
        if (_isFacingRight && _appliedMovement.x < 0 || !_isFacingRight && _appliedMovement.x  > 0)
        {
            _isFacingRight = !_isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
            // rotate weaponholder
        }
    }   

}