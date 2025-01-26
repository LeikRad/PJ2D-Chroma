using System.Collections.Generic;

enum PlayerStates
{
    grounded,
    jump,
    idle,
    walk,
    dash
}
public class PlayerStateFactory
{
    PlayerStateMachine _ctx;
    Dictionary<PlayerStates, PlayerBaseState> _states = new Dictionary<PlayerStates, PlayerBaseState>();
    public PlayerStateFactory(PlayerStateMachine ctx)
    {
        _ctx = ctx;
        _states.Add(PlayerStates.grounded, new PlayerGroundedState(_ctx, this));
        _states.Add(PlayerStates.jump, new PlayerJumpState(_ctx, this));
        _states.Add(PlayerStates.idle, new PlayerIdleState(_ctx, this));
        _states.Add(PlayerStates.walk, new PlayerWalkState(_ctx, this));
        _states.Add(PlayerStates.dash, new PlayerDashState(_ctx, this));
    }
    
    public PlayerBaseState Idle()
    {
        return _states[PlayerStates.idle];
    }
    public PlayerBaseState Grounded()
    {
        return _states[PlayerStates.grounded];
    }
    public PlayerBaseState Jump()
    {
        return _states[PlayerStates.jump];
    }
    public PlayerBaseState Walk()
    {
        return _states[PlayerStates.walk];
    }
    
    public PlayerBaseState Dash()
    {
        return _states[PlayerStates.dash];
    }
    
}
