using System.Collections;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    
    private float _gravityHolder;
    
    public PlayerDashState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory)
    {
    }
    
    public override void EnterState()
    {
        InitializeSubState();
        _gravityHolder = Ctx.rb.gravityScale;
        Ctx.canDash = false;
        Ctx.animator.SetBool("isDash", true);
        Ctx.StartCoroutine(HandleDash());
    }
    
    public override void UpdateState()
    {
        CheckSwitchState();
    }
    
    public override void ExitState()
    {
        Ctx.currentMovementX = 0;
        Ctx.currentMovementY = 0;
        Ctx.rb.gravityScale = _gravityHolder;
        Ctx.gravity = -_gravityHolder;

        Ctx.animator.SetBool("isDash", false);
    }
    
    public override void CheckSwitchState() {}
    
    public override void InitializeSubState() {}
    
    IEnumerator HandleDash()
    {
        Ctx.rb.gravityScale = 0;
        Ctx.gravity = 0;
        int dir = Ctx.transform.localScale.x > 0 ? 1 : -1;
        Vector2 movement = new Vector2(Ctx.dashSpeed * dir, 0);
        Ctx.appliedMovementX = movement.x;
        Ctx.currentMovementY = movement.y;

        // wait for dash time
        yield return new WaitForSeconds(Ctx.dashTime);  
        // reset gravity
        // start cooldown
        Ctx.StartCoroutine(dashCooldown());
        // exit to idl
        SwitchState(Factory.Idle());

    }
    
    IEnumerator dashCooldown()
    {
        yield return new WaitForSeconds(Ctx.dashCooldown);
        Ctx.canDash = true;
    }
}
