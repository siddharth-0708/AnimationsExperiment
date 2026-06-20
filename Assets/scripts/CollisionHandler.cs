using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision detected with: " + other.gameObject.name);
        if (!other.gameObject.CompareTag("tripleh"))
            return;

        Animator enemyAnimator = other.gameObject.GetComponent<Animator>();
        if (enemyAnimator == null)
            return;

        enemyAnimator.SetTrigger("dance");
    }
}
