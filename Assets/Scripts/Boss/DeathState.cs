using UnityEngine;

public class DeathState : BossBaseState
{
    public DeathState(BossStateMachine boss) : base(boss) { }

    public override void Enter()
    {
        Debug.Log("Boss Defeated");
    }

    public override void Update() { }

    public override void Exit() { }
}