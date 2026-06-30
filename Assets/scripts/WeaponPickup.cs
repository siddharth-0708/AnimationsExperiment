using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private Collider pickupTrigger;
    [SerializeField] private Collider weaponCollider;

    [Header("Local transform when parented to stickParent (set in Inspector)")]
    [SerializeField] private Vector3 attachLocalPosition;
    [SerializeField] private Vector3 attachLocalEulerAngles;

    private Quaternion initialWorldRotation;
    private Vector3 initialLocalScale;

    private PlayerMovement playerInRange;
    private bool isEquipped;

    public bool IsEquipped => isEquipped;

    void Awake()
    {
        initialWorldRotation = transform.rotation;
        initialLocalScale = transform.localScale;

        if (pickupTrigger == null)
        {
            foreach (Collider col in GetComponentsInChildren<Collider>())
            {
                if (!col.isTrigger)
                    continue;

                pickupTrigger = col;
                break;
            }
        }

        if (weaponCollider == null)
        {
            foreach (Collider col in GetComponentsInChildren<Collider>())
            {
                if (col.isTrigger)
                    continue;

                weaponCollider = col;
                break;
            }
        }

        if (pickupTrigger != null && pickupTrigger.gameObject != gameObject)
        {
            var relay = pickupTrigger.GetComponent<PickupTriggerRelay>();
            if (relay == null)
                relay = pickupTrigger.gameObject.AddComponent<PickupTriggerRelay>();
            relay.Initialize(this);
        }

        SetDroppedColliderState();
    }

    void SetDroppedColliderState()
    {
        if (pickupTrigger != null)
            pickupTrigger.enabled = true;

        if (weaponCollider != null)
            weaponCollider.enabled = false;
    }

    void SetEquippedColliderState()
    {
        if (pickupTrigger != null)
            pickupTrigger.enabled = false;

        if (weaponCollider != null)
            weaponCollider.enabled = true;
    }

    public void OnPlayerEnter(Collider other)
    {
        if (isEquipped || !other.CompareTag("randy"))
            return;

        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player == null)
            return;

        playerInRange = player;
        player.RegisterNearbyWeapon(this);
    }

    public void OnPlayerExit(Collider other)
    {
        if (!other.CompareTag("randy"))
            return;

        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player == null || playerInRange != player)
            return;

        player.UnregisterNearbyWeapon(this);
        playerInRange = null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (pickupTrigger != null && pickupTrigger.gameObject != gameObject)
            return;

        OnPlayerEnter(other);
    }

    void OnTriggerExit(Collider other)
    {
        if (pickupTrigger != null && pickupTrigger.gameObject != gameObject)
            return;

        OnPlayerExit(other);
    }

    public void AttachTo(Transform parent)
    {
        transform.SetParent(parent, false);
        transform.localPosition = attachLocalPosition;
        transform.localRotation = Quaternion.Euler(attachLocalEulerAngles);
        transform.localScale = initialLocalScale;

        SetEquippedColliderState();

        if (playerInRange != null)
        {
            playerInRange.UnregisterNearbyWeapon(this);
            playerInRange = null;
        }

        isEquipped = true;
    }

    public void Detach(Vector3 dropPosition)
    {
        transform.SetParent(null, false);
        transform.position = dropPosition;
        transform.rotation = initialWorldRotation;
        transform.localScale = initialLocalScale;

        SetDroppedColliderState();

        isEquipped = false;
    }
}

public class PickupTriggerRelay : MonoBehaviour
{
    WeaponPickup pickup;

    public void Initialize(WeaponPickup weaponPickup)
    {
        pickup = weaponPickup;
    }

    void OnTriggerEnter(Collider other)
    {
        pickup?.OnPlayerEnter(other);
    }

    void OnTriggerExit(Collider other)
    {
        pickup?.OnPlayerExit(other);
    }
}
