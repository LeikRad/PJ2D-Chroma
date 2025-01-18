using UnityEngine;

public class BossStateMachine : MonoBehaviour
{
    public BossBaseState CurrentState { get; private set; }
    public PhaseOneState PhaseOne { get; private set; }
    public PhaseTwoState PhaseTwo { get; private set; }
    public PhaseThreeState PhaseThree { get; private set; }
    public DeathState Death { get; private set; }

    public float Health;
    public float MaxHealth;

    public Transform[] SafeSpots;
    public GameObject PlatformPrefab; 
    public Transform Lava; 
    public Transform SwoopStartPoint; 
    public Transform SwoopEndPoint;   
    public GameObject SpiderLeg; 
    public GameObject GroundLegPrefab;
    public float SwoopSpeed = 5f;     

    private void Awake()
    {
        PhaseOne = new PhaseOneState(this);
        PhaseTwo = new PhaseTwoState(this);
        PhaseThree = new PhaseThreeState(this);
        Death = new DeathState(this);
    }

    private void Start()
    {
        MaxHealth = Health;
        TransitionToState(PhaseOne);
    }

    private void Update()
    {
        CurrentState?.Update();
    }

    public void TransitionToState(BossBaseState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
    
    public void TakeDamage(float damageAmount)
    {
        Health -= damageAmount;
        if (Health <= 0)
        {
            Health = 0;
            TransitionToState(Death);
        }
    }
}