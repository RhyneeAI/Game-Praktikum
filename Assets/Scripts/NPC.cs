using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NPC: MonoBehaviour
{
    public Transform target;
    public float scale = 1f;
    private float targetScale;
    public float smoothSpeed = 5f;

    public float moveSpeed = 2f;
    public float changeDirTime = 2f;
    public float detectRadius = 3f;

    private Vector2 moveDirection;
    private float timer;

    void Start()
    {
        
    }

    void Update() 
    {
        float distance = Vector2.Distance(transform.position, target.position);

        if (distance <= detectRadius)
        {
            Vector2 direction = (target.position - transform.position).normalized;

            // Hadap player
            transform.up = direction;

            // Cek ada tembok di depan
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f);

            if (hit.collider != null)
            {
                // Kalau nabrak, cari arah lain (random dikit)
                direction = Quaternion.Euler(0, 0, Random.Range(-90, 90)) * direction;
            }

            transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            // Random movement (seperti sebelumnya)
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                moveDirection = Random.insideUnitCircle.normalized;
                timer = changeDirTime;
            }

            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}
