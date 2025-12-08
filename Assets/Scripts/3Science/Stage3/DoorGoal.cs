using UnityEngine;

public class DoorGoal : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        StageManager.Instance?.OnStageClear();
    }
}
