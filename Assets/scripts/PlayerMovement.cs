using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;

    private Animator animator;
    private Vector2 moveInput;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (IsAttacking()){
            return;
        }
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);
        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);
    }
    bool IsAttacking()
{
    if (animator.IsInTransition(0)) return true;
        var state = animator.GetCurrentAnimatorStateInfo(0);
        return state.IsName("punch1") || state.IsName("punch2")
            || state.IsName("kick1") || state.IsName("kick2");
    }

    // Arrow keys → movement bool
    public void PlayerActions(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        // true while any direction held, false when released
        bool isMoving = moveInput.sqrMagnitude > 0.01f;
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