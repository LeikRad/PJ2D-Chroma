using UnityEngine;

public class TrailControl : MonoBehaviour
{
    private TrailRenderer trailRenderer;
    private Player player;

    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        player = GetComponent<Player>();

        trailRenderer.enabled = false;
    }

    void Update()
    {
        if (player != null && player.isDashing)
        {
            trailRenderer.enabled = true;
           
        }
        else
        {
            trailRenderer.enabled = false;
        }
    }
}
