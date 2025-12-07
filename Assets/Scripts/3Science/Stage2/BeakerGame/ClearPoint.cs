using UnityEngine;

public class ClearPoint : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            RoundManager.Instance.OnRoundClear();
        }
    }
}
