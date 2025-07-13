using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    public enum MovementType
    {
        Horizontal,
        Vertical,
        Circular,
        BackAndForth
    }

    [Header("Movement Settings")]
    public MovementType movementType = MovementType.BackAndForth;
    public float speed = 2f;
    public float movementRange = 3f;

    [Header("Circular Movement")]
    public float radius = 2f;

    private Vector3 startPosition;
    private float timeCounter = 0f;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        switch (movementType)
        {
            case MovementType.Horizontal:
                HorizontalMovement();
                break;
            case MovementType.Vertical:
                VerticalMovement();
                break;
            case MovementType.Circular:
                CircularMovement();
                break;
            case MovementType.BackAndForth:
                BackAndForthMovement();
                break;
        }
    }

    void HorizontalMovement()
    {
        float newX = startPosition.x + Mathf.PingPong(Time.time * speed, movementRange);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    void VerticalMovement()
    {
        float newY = startPosition.y + Mathf.PingPong(Time.time * speed, movementRange);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void CircularMovement()
    {
        timeCounter += Time.deltaTime * speed;
        float x = startPosition.x + Mathf.Cos(timeCounter) * radius;
        float z = startPosition.z + Mathf.Sin(timeCounter) * radius;
        transform.position = new Vector3(x, transform.position.y, z);
    }

    void BackAndForthMovement()
    {
        float newZ = startPosition.z + Mathf.PingPong(Time.time * speed, movementRange);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
    }

    // Optional: Visualization in Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(startPosition, 0.5f);

        if (movementType == MovementType.Circular)
        {
            Gizmos.DrawWireSphere(startPosition, radius);
        }
    }
}