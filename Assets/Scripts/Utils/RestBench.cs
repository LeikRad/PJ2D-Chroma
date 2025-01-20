using UnityEngine;

public class RestBench : MonoBehaviour
{
    public static Transform benchRespawnPoint;
    //private Animator player_animator = Player.Instance.GetComponent<Animator>();

    private void Start()
    {
        benchRespawnPoint = transform;
        RespawnManager.Instance.SetBenchRespawnPoint(benchRespawnPoint);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Vector2.Distance(transform.position, Player.Instance.transform.position) < 2f)
        {
            SitAndRest();
            //player_animator.SetBool("IsBench", true);
        }
        else
        {
            //player_animator.SetBool("IsBench", false);
        }
    }

    private void SitAndRest()
    {
        PlayerHealth playerHealth = Player.Instance.GetComponent<PlayerHealth>();
        playerHealth.RestoreHealth();
        Debug.Log("Player is resting at the bench.");
    }
}