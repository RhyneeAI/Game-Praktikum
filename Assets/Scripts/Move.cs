using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Move : MonoBehaviour
{
    [SerializeField] 
    private float speed;
    public Rigidbody2D body;
    public Vector2 currentInput;

    protected virtual void Awake() {
        body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        body.linearVelocity = speed * currentInput;
    }
}
