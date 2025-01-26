public class PlayerWalkState : PlayerBaseState
{
    
    public PlayerWalkState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory) {}

    public override void EnterState()
    {
        Ctx.animator.SetFloat("Speed", 1);
    }

    public override void UpdateState()
    {
        Ctx.appliedMovementX = Ctx.currentMovementX * Ctx.moveSpeed;
        CheckSwitchState();
    }
    
    public override void ExitState(){}

    public override void CheckSwitchState()
    {
        if (Ctx.dashEnabled && Ctx.isDashPressed && Ctx.canDash) SwitchState(Factory.Dash());
        if (!Ctx.isMovementPressed) SwitchState(Factory.Idle());
    }
    
    public override void InitializeSubState(){}
}
