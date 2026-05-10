using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Komponen
    // 1. Spawn object
    // 2. Coordinate of spawn location
    // 3. Rotation of spawner

    [Header("Spawn Manager")]
    public Transform player;
    private float z = -10;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camera = player.position;
        camera.z = z;

        transform.position = camera;
    }
}

