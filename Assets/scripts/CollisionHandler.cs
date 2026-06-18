using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("tripleh"))
            return;

        Animator enemyAnimator = other.gameObject.GetComponent<Animator>();
        if (enemyAnimator == null)
            return;

        enemyAnimator.SetTrigger("dance");
    }
}
