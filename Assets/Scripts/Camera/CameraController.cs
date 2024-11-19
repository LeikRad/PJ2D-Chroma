using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

    public Vector2 bottom_left_corner;
    
    public Vector2 top_right_corner;
    
    public float speed = 0.2f;

    void Start()
    {
        // Calculate the camera's half height
        Camera localcamera = GetComponent<Camera>();

        Vector3 targetPosition = new Vector3(0, 1, -10) + player.position;
        targetPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, bottom_left_corner.x, top_right_corner.x),
            Mathf.Clamp(targetPosition.y, bottom_left_corner.y, top_right_corner.y),
            targetPosition.z
        );
        transform.position = targetPosition;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Calculate the camera's half height

        // Follow player, but keep z position
        Vector3 targetPosition = new Vector3(0, 1, -10) + player.position;
        
        // Only move the camera up if the player reaches half of the camera height
        

        // Lock camera to the map
        targetPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, bottom_left_corner.x, top_right_corner.x),
            Mathf.Clamp(targetPosition.y, bottom_left_corner.y, top_right_corner.y),
            targetPosition.z
        );

        transform.position = Vector3.Lerp(transform.position, targetPosition, speed);
        
    }
}