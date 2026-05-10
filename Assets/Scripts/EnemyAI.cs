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
    public float avoidDistance = 0.8f;
    public float avoidStrength = 1.8f;
    public LayerMask obstacleLayer;

    [Header("Stuck Detection")]
    public float stuckCheckInterval = 0.5f;  // cek posisi tiap X detik
    public float stuckThreshold = 0.1f;      // dianggap stuck kalau gerak < nilai ini
    public float alternateSeekTime = 1.5f;   // berapa lama cari arah alternatif

    private Rigidbody2D rb;
    public Vector2 moveDirection;
    private float patrolTimer;
    private EnemyState currentState;

    // Stuck detection
    private Vector2 lastCheckedPosition;
    private float stuckCheckTimer;
    private bool isStuck;
    private float alternateSeekTimer;
    private Vector2 alternateDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ChooseRandomDirection();
        currentState = EnemyState.Patrol;
        lastCheckedPosition = transform.position;
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
        {
            currentState = EnemyState.Chase;
            isStuck = false;
        }
    }

    void ChaseState(float distance)
    {
        CheckIfStuck();

        if (isStuck)
        {
            alternateSeekTimer -= Time.deltaTime;

            if (alternateSeekTimer <= 0f)
            {
                isStuck = false;
                alternateDirection = FindBestAlternateDirection();
            }

            moveDirection = Vector2.Lerp(moveDirection, alternateDirection, Time.deltaTime * 5f).normalized;
        }
        else
        {
            Vector2 dirToPlayer = (player.position - transform.position).normalized;
            moveDirection = Vector2.Lerp(moveDirection, dirToPlayer, Time.deltaTime * 3f).normalized;
        }

        AvoidWalls();

        if (distance >= chaseRange)
        {
            currentState = EnemyState.Patrol;
            isStuck = false;
            ChooseRandomDirection();
        }
    }

    void CheckIfStuck()
    {
        stuckCheckTimer += Time.deltaTime;

        if (stuckCheckTimer >= stuckCheckInterval)
        {
            float moved = Vector2.Distance((Vector2)transform.position, lastCheckedPosition);

            if (moved < stuckThreshold)
            {
                isStuck = true;
                alternateSeekTimer = alternateSeekTime;
                alternateDirection = FindBestAlternateDirection();
            }
            else
            {
                isStuck = false;
            }

            lastCheckedPosition = transform.position;
            stuckCheckTimer = 0f;
        }
    }

    Vector2 FindBestAlternateDirection()
    {
        Vector2[] candidates = {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right,
            new Vector2(1, 1).normalized,
            new Vector2(1, -1).normalized,
            new Vector2(-1, 1).normalized,
            new Vector2(-1, -1).normalized
        };

        Vector2 bestDir = moveDirection;
        float bestScore = float.MinValue;
        Vector2 dirToPlayer = (player.position - transform.position).normalized;

        foreach (Vector2 dir in candidates)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, avoidDistance * 1.5f, obstacleLayer);

            if (hit.collider != null && hit.distance < 0.3f) continue;

            float clearance = hit.collider != null ? hit.distance : avoidDistance * 1.5f;
            float alignment = Vector2.Dot(dir, dirToPlayer); 

            float score = clearance + alignment * 1.5f;

            if (score > bestScore)
            {
                bestScore = score;
                bestDir = dir;
            }
        }

        return bestDir;
    }

    void AvoidWalls()
    {
        float originOffset = 0.2f;
        Vector2 pos = (Vector2)transform.position + moveDirection * originOffset;
        Vector2 perpendicular = new Vector2(-moveDirection.y, moveDirection.x);

        Vector2[] rayDirs = {
            moveDirection,
            (moveDirection + perpendicular * 0.5f).normalized,
            (moveDirection - perpendicular * 0.5f).normalized,
            perpendicular,
            -perpendicular
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
            Vector2 desiredDirection = (moveDirection + steeringForce * avoidStrength).normalized;
            moveDirection = Vector2.Lerp(moveDirection, desiredDirection, Time.deltaTime * 8f).normalized;
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

        if (currentState == EnemyState.Chase)
        {
            isStuck = true;
            alternateSeekTimer = alternateSeekTime;
            alternateDirection = FindBestAlternateDirection();
        }
    }
}