using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Patrol, Chase }

    [Header("References")]
    public Transform player;

    [Header("Speed Settings")]
    public float patrolSpeed = 1f;
    public float chaseSpeed = 1.25f;

    [Header("Detection")]
    public float chaseRange = 3f;
    public float patrolChangeTime = 1f;

    [Header("Rotation")]
    public float rotationSpeed = 250f;

    [Header("Wall Avoidance")]
    public float avoidDistance = 0.54321f;      
    public float avoidStrength = 5.12345f;   
    public LayerMask obstacleLayer;      

    private Rigidbody2D rb;
    public Vector2 moveDirection;
    private float patrolTimer;
    private EnemyState currentState;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ChooseRandomDirection();
        currentState = EnemyState.Patrol;
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case EnemyState.Patrol:
                PatrolState(distance);
                break;
            case EnemyState.Chase:
                ChaseState(distance);
                break;
        }

        RotateToMovement();
    }

    void ChooseRandomDirection()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);
        moveDirection = new Vector2(randomX, randomY).normalized;
    }

    void FixedUpdate()
    {
        float currentSpeed = currentState == EnemyState.Patrol ? patrolSpeed : chaseSpeed;
        rb.linearVelocity = moveDirection * currentSpeed;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }

    void PatrolState(float distance)
    {
        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolChangeTime)
        {
            ChooseRandomDirection();
            patrolTimer = 0f;
        }

        AvoidWalls();

        if (distance <= chaseRange)
            currentState = EnemyState.Chase;
    }

    void ChaseState(float distance)
    {
        moveDirection = (player.position - transform.position).normalized;

        AvoidWalls();

        if (distance >= chaseRange)
        {
            currentState = EnemyState.Patrol;
            ChooseRandomDirection();
        }
    }

    void AvoidWalls()
    {
        Vector2 pos = transform.position;

        Vector2 perpendicular = new Vector2(-moveDirection.y, moveDirection.x);

        Vector2[] rayDirs = {
            moveDirection,                                    // tengah
            (moveDirection + perpendicular * 0.5f).normalized, // kanan-depan
            (moveDirection - perpendicular * 0.5f).normalized  // kiri-depan
        };

        Vector2 steeringForce = Vector2.zero;

        foreach (Vector2 dir in rayDirs)
        {
            RaycastHit2D hit = Physics2D.Raycast(pos, dir, avoidDistance, obstacleLayer);

            if (hit.collider != null)
            {
                float proximity = 1f - (hit.distance / avoidDistance);
                steeringForce += hit.normal * proximity;
            }
        }

        if (steeringForce != Vector2.zero)
        {
            moveDirection = (moveDirection + steeringForce * avoidStrength * Time.deltaTime).normalized;
        }
    }

    void RotateToMovement()
    {
        if (moveDirection != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            targetAngle += 90f;

            Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState == EnemyState.Patrol)
            ChooseRandomDirection();
    }
}