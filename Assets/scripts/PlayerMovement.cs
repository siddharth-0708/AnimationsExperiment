using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;

    private Animator animator;
    private Rigidbody rb;
    private Vector2 moveInput;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        UpdateFacing();
    }

    void FixedUpdate()
    {
        if (IsAttacking())
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        float x = -moveInput.x * moveSpeed;
        rb.linearVelocity = new Vector3(x, rb.linearVelocity.y, 0f);
    }

    void UpdateFacing()
    {
        float yRotation = moveInput.x < 0f ? 90f : -90f;
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
    bool IsAttacking()
    {
        // if (animator.IsInTransition(0)) return true;
        var state = animator.GetCurrentAnimatorStateInfo(0);
        return state.IsName("punch1") || state.IsName("punch2")
            || state.IsName("kick1") || state.IsName("kick2");
    }

    // Arrow keys → movement bool
    public void PlayerActions(InputAction.CallbackContext context)
    {
        if (!context.performed && !context.canceled)
        {
          return;  
        };
        moveInput = context.ReadValue<Vector2>();

        // true while any direction held, false when released
        bool isMoving = moveInput.x != 0f;
        animator.SetBool("movement", isMoving);
    }

    public void PunchOne(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (IsAttacking()) return;
        animator.SetTrigger("punch1");   // must match Animator param name
    }

    public void PunchTwo(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (IsAttacking()) return;
        animator.SetTrigger("punch2");
    }

    public void KickOne(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (IsAttacking()) return;
        animator.SetTrigger("kick1");
    }

    public void KickTwo(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (IsAttacking()) return;
        animator.SetTrigger("kick2");
    }
}