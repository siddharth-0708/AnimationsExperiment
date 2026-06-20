using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision detected with: " + other.gameObject.name);
        if (!other.gameObject.CompareTag("tripleh"))
        {
            return;
        }
        Collider ourHitbox = other.GetContact(0).thisCollider;
        GameObject hitter = ourHitbox.gameObject;
        
        if (!hitter.CompareTag("hitBox"))
        {
            return;
        }
        Animator enemyAnimator = other.gameObject.GetComponent<Animator>();
        if (enemyAnimator == null)
            return;

        enemyAnimator.SetTrigger("dance");
    }
}
