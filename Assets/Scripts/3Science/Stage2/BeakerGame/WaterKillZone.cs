using UnityEngine;

public class WaterKillZone : MonoBehaviour
{
    public Transform respawnPoint;   //Round1/SpawnPoint

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.position = respawnPoint.position;
            }

        }
    }
}
