using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class DummyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    private Rigidbody rb;
    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // --- Option 1: dynamic Rigidbody (Is Kinematic OFF) ---
        // Uses velocity each physics step. Body can be pushed by collisions.
        // float x = Mathf.Abs(moveInput.x) > 0.01f ? -moveInput.x * moveSpeed : 0f;
        // rb.linearVelocity = new Vector3(x, rb.linearVelocity.y, 0f);

        // --- Option 2: kinematic Rigidbody (Is Kinematic ON) ---
        // MovePosition steps the body through physics; no velocity on kinematic bodies.
        if (Mathf.Abs(moveInput.x) < 0.01f)
            return;

        Vector3 delta = new Vector3(-moveInput.x * moveSpeed * Time.fixedDeltaTime, 0f, 0f);
        rb.MovePosition(rb.position + delta);
    }

    public void PlayerActions(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}
