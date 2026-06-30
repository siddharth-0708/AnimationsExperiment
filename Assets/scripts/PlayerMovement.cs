using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Transform stickParent;

    private Animator animator;
    private Rigidbody rb;
    private Vector2 moveInput;
    private WeaponPickup nearbyWeapon;
    private WeaponPickup equippedWeapon;

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
        }
        ;
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
    public void DropWeapon(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        WeaponDrop();
    }
    public void RegisterNearbyWeapon(WeaponPickup weapon)
    {
        nearbyWeapon = weapon;
    }

    public void UnregisterNearbyWeapon(WeaponPickup weapon)
    {
        if (nearbyWeapon == weapon)
            nearbyWeapon = null;
    }

    public void Pickup(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (IsAttacking()) return;
        if (nearbyWeapon == null) return;
        animator.SetTrigger("pickup");
    }
    public void Attack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (IsAttacking()) return;
        animator.SetTrigger("attack");
    }
    // Called from pickup animation event
    public void WeaponPickup()
    {
        if (nearbyWeapon == null || stickParent == null)
            return;

        WeaponPickup weapon = nearbyWeapon;
        nearbyWeapon = null;
        weapon.AttachTo(stickParent);
        equippedWeapon = weapon;
    }

    public void WeaponDrop()
    {
        if (equippedWeapon == null && stickParent != null && stickParent.childCount > 0)
            equippedWeapon = stickParent.GetChild(0).GetComponent<WeaponPickup>();

        if (equippedWeapon == null)
            return;

        equippedWeapon.Detach(transform.position);
        equippedWeapon = null;
    }
}