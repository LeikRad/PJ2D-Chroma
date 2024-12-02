using Unity.Cinemachine;
using UnityEngine;

public class CameraWakeUp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // get player object
        GameObject player = GameObject.Find("Player");
        // follow player with cinemachine
        CinemachineCamera virtualCamera = GetComponent<CinemachineCamera>();
        virtualCamera.Follow = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
