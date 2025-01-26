using UnityEngine;
public class PlayerIdleState : PlayerBaseState
{

    public PlayerIdleState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory) {}

    public override void EnterState()
    {
        Ctx.animator.SetFloat("Speed", 0);
        Ctx.appliedMovementX = 0;
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }
    
    public override void ExitState(){}

    public override void CheckSwitchState()
    {
        if (Ctx.dashEnabled && Ctx.isDashPressed && Ctx.canDash)
        {
            SwitchState(Factory.Dash());
        } else if (Ctx.isMovementPressed)
        {
            SwitchState(Factory.Walk());
        }
    }
    
    public override void InitializeSubState(){}   
}
