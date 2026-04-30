using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotate : MonoBehaviour
{
    private void OnLook(InputValue value)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(value.Get<Vector2>());
        LookAt(mousePosition);
    }

    void Update()
    {
        
    }

    protected void LookAt(Vector3 target)
    {
        float lookAngle = AngleBetweenTwoPoints(transform.position, target) + 90;
        transform.eulerAngles = new Vector3(0, 0, lookAngle);
    }

    private float AngleBetweenTwoPoints(Vector3 position, Vector3 target)
    {
        return Mathf.Atan2(position.y - target.y, position.x - target.x) * Mathf.Rad2Deg;
    }
}
