using UnityEngine;

public class DeathState : BossBaseState
{
    public DeathState(BossStateMachine boss) : base(boss) { }

    public override void Enter()
    {
        boss.Die();
    }

    public override void Update() { }

    public override void Exit() { }
}