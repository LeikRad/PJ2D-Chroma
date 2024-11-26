using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

    // Update is called once per frame
    void Update()
    {
        // Ajuste a posição da câmera para seguir o jogador
        transform.position = player.transform.position + new Vector3(0, 1, -10);
    }
}