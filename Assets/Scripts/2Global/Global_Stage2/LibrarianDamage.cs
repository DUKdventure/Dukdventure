using UnityEngine;

public class LibrarianDamage : MonoBehaviour
{
    private bool canHit = true;
    public float cooldown = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canHit) return;

        if (collision.CompareTag("Player"))
        {
            HeartManager.Instance.RemoveHeart();
            collision.GetComponent<PlayerColorEffect>().HitEffect();

            canHit = false;
            Invoke("ResetHit", cooldown);
        }
    }

    void ResetHit()
    {
        canHit = true;
    }
}
