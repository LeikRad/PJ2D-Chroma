using UnityEngine;

public class PlayerGroundedState : PlayerBaseState, IRootState
{
    public PlayerGroundedState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory)
    {
        IsRootState = true;
    }
    
    public void HandleGravity()
    {
        float prevYVelocity = Ctx.currentMovementY;
        Ctx.currentMovementY += (Ctx.gravity * Time.deltaTime);
        Ctx.appliedMovementY = Mathf.Max((prevYVelocity + Ctx.currentMovementY) * .5f, Ctx.terminalVelocity); 
    }
    
    public override void EnterState()
    {
        InitializeSubState();
        Ctx.jumpCount = 0;
        HandleGravity();
    }

    public override void UpdateState()
    {
        HandleGravity();
        CheckSwitchState();
    }
    
    public override void ExitState(){}

    public override void CheckSwitchState()
    {
        if(Ctx.isJumpPressed) SwitchState(Factory.Jump());
    }

    public override void InitializeSubState()
    {
        SetSubState(!Ctx.isMovementPressed ? Factory.Idle() : Factory.Walk());
    }

}
