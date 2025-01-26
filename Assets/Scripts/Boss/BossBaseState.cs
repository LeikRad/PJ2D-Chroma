using UnityEngine;
using System.Collections;

public abstract class BossBaseState
{
    protected BossStateMachine boss;

    public BossBaseState(BossStateMachine boss)
    {
        this.boss = boss;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
    
    protected Coroutine StartCoroutine(IEnumerator routine)
    {
        return boss.StartCoroutine(routine);
    }
}