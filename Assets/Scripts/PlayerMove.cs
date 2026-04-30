using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMove : Move
{
    private Camera cam;

    protected override void Awake()
    {
        base.Awake(); 
        cam = Camera.main;
    }

    private void OnMove(InputValue value)
    {
        currentInput = value.Get<Vector2>();
    }

    private void Update()
    {
        RotateToCursor();
    }

    private void RotateToCursor()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);

        Vector2 direction = worldPos - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle + 75f);
    }
}