using UnityEngine;
using UnityEngine.SceneManagement;

public class BossStateMachine : MonoBehaviour
{
    public BossBaseState CurrentState { get; private set; }
    public PhaseOneState PhaseOne { get; private set; }
    public PhaseTwoState PhaseTwo { get; private set; }
    public PhaseThreeState PhaseThree { get; private set; }
    public PhaseFourState PhaseFour { get; private set; }
    public DeathState Death { get; private set; }

    public float Health;
    public float MaxHealth;
    public bool IsInvulnerable = false;
    
    public GameObject PlatformPrefab; 
    public Transform Lava;
    public float LavaRiseSpeed = 1.5f;
    public Transform SwoopStartPoint; 
    public Transform SwoopEndPoint;   
    public GameObject SpiderLeg; 
    public GameObject GroundLegPrefab;
    public float SwoopSpeed = 5f;  
    public GameObject FinalPlatform;
    public float BossFinalHeightOffset = 2f; 
    public float PlatformSpacing = 3f;
    public float PlatformHorizontalVariation = 4f;
    public Transform BossStopPoint;
    public Vector3 PhaseFourStartPosition;
    public bool BossDefeated = false; 
    public bool PlatformsSpawned = false;
    private Scene thisScene;
    private string currentScene;

    private void Awake()
    {
        PhaseOne = new PhaseOneState(this);
        PhaseTwo = new PhaseTwoState(this);
        PhaseThree = new PhaseThreeState(this);
        PhaseFour = new PhaseFourState(this);
        Death = new DeathState(this);
    }

    private void Start()
    {
        if (SaveSystem.IsBossDefeated())
        {
            Debug.Log("Boss já foi derrotado, destruindo ao iniciar.");
            Destroy(gameObject);
        }

        if (SaveSystem.IsLavaDestroyed())
        {
            Debug.Log("Lava já foi removida, destruindo ao iniciar.");
            var lava = GameObject.FindWithTag("Lava");
            if (lava != null)
            {
                Destroy(lava);
            }
        }
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
    
    public void ResetPhaseFour()
    {
        thisScene = SceneManager.GetSceneAt(1);
        currentScene = SceneManager.GetSceneAt(1).name;
        SceneManager.UnloadSceneAsync(thisScene);
        SceneManager.LoadSceneAsync(currentScene, LoadSceneMode.Additive);
        CurrentState = PhaseFour;
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

    public void Die()
    {
        SaveSystem.MarkBossAsDefeated(); 

        var lava = GameObject.FindWithTag("BossLava");
        if (lava != null)
        {
            Destroy(lava);
        }

        Destroy(gameObject);
    }
}