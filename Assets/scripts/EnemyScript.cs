using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    private Animator animator;

    private Rigidbody myBody;
    public float speed = 5f;

    [SerializeField] private Transform playerTarget;

    public float attack_Distance = 1f;
    public float chase_Player_After_Attack = 1f;

    private float current_Attack_Time;
    private float default_Attack_Time = 2f;

    private bool followPlayer, attackPlayer;

    void Awake()
    {
        myBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        followPlayer = true;
        current_Attack_Time = default_Attack_Time;
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    void FixedUpdate()
    {
        FollowTarget();
    }

    void FollowTarget()
    {

        // if we are not supposed to follow the player
        if (!followPlayer)
            return;

        if (Vector3.Distance(transform.position, playerTarget.transform.position) > attack_Distance)
        {

            transform.LookAt(playerTarget.transform);
            myBody.linearVelocity = transform.forward * speed;

            if (myBody.linearVelocity.sqrMagnitude != 0)
            {
                animator.SetBool("movement", true);
            }


        }
        else if (Vector3.Distance(transform.position, playerTarget.transform.position) <= attack_Distance)
        {

            myBody.linearVelocity = Vector3.zero;
            animator.SetBool("movement", false);

            followPlayer = false;
            attackPlayer = true;

        }

    }

    void Attack()
    {

        if (!attackPlayer)
            return;

        current_Attack_Time += Time.deltaTime;

        if (current_Attack_Time > default_Attack_Time)
        {

            EnemyAttack(Random.Range(0, 4));

            current_Attack_Time = 0f;

        }

        if (Vector3.Distance(transform.position, playerTarget.transform.position) >
                attack_Distance + chase_Player_After_Attack)
        {

            attackPlayer = false;
            followPlayer = true;

        }

    }
    void EnemyAttack(int attack)
    {

        if (attack == 0)
        {
            animator.SetTrigger("punch1");
        }

        if (attack == 1)
        {
            animator.SetTrigger("punch2");
        }

        if (attack == 2)
        {
            animator.SetTrigger("kick1");
        }
        if (attack == 3) {
            animator.SetTrigger("kick2");
        }

    }
}
