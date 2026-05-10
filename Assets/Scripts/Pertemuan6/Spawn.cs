using UnityEngine;

public class Spawn : MonoBehaviour
{
    // Komponen
    // 1. Spawn object
    // 2. Coordinate of spawn location
    // 3. Rotation of spawner

    [Header("Spawn Manager")]
    public GameObject spawnObject;
    public Transform coordinate;
    public Transform playerTransform;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        InvokeRepeating(nameof(Spawner), 1f, Random.Range(2f, 2.5f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Spawner()
    {
        GameObject enemy = Instantiate(spawnObject, coordinate.position, coordinate.rotation);
        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
        if (enemyAI != null)
            enemyAI.player = playerTransform;
    }
}
