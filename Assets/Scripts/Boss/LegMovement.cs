using UnityEngine;

public class LegMovement : MonoBehaviour
{
    private Vector3 endPosition;
    private float speed;
    private float attackDamage = 10f;
    //public SpriteRenderer spriteRenderer;

    public void Setup(Vector3 start, Vector3 end, float moveSpeed)
    {
        endPosition = end;
        speed = moveSpeed;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);
        
        if (transform.position == endPosition)
        {
            Destroy(gameObject);
        }

        // sprite reflection
        if (speed >= 0)
        {
            //spriteRenderer.flipX =true;
        }
        else
        {
            //spriteRenderer.flipX = false;
        }
    }
}