using UnityEngine;
public class PlayerJumpState : PlayerBaseState, IRootState
{
    private bool _jumpReleased = false;

    public PlayerJumpState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        InitializeSubState();
        HandleJump();
    }

    public override void UpdateState()
    {
        if (!Ctx.isJumpPressed) _jumpReleased = true;
        HandleGravity();
        CheckSwitchState();
    }

    public override void ExitState()
    {
        Ctx.animator.SetBool("IsJumping", false);
    }

    public override void CheckSwitchState()
    {
        if (Ctx.isGrounded)
        {
            Ctx.jumpCount = 0;
            SwitchState(Factory.Grounded());
        }
        if (Ctx.isJumpPressed && _jumpReleased) HandleJump();
    }

    public override void InitializeSubState()
    {
        SetSubState(!Ctx.isMovementPressed ? Factory.Idle() : Factory.Walk());
    }

    void HandleJump()
    {
        if (Ctx.jumpCount >= Ctx.maxJumps)
        {
            return;
        }
        _jumpReleased = false;
        Ctx.jumpCount++;
        Ctx.currentMovementY = Ctx.jumpForce;
        Ctx.appliedMovementY = Ctx.jumpForce;
        Ctx.animator.SetBool("IsJumping", true);
    }

    public void HandleGravity()
    {
        float prevYVelocity = Ctx.currentMovementY;
        Ctx.currentMovementY += (Ctx.gravity * Time.deltaTime);
        Ctx.appliedMovementY = Mathf.Max((prevYVelocity + Ctx.currentMovementY), Ctx.terminalVelocity);
    }
}    
