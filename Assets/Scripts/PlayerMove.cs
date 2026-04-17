using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerMove: MonoBehaviour
{
    public float speed = 2.5f;
    void Start() 
    {
        transform.position = new Vector3(1, 1, 0);  
    }

    void Update() 
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(x, y).normalized;

        // Move
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // Rotate (kalau ada input)
        if (direction != Vector2.zero)
        {
            transform.up = -direction;
        }
    }
}
