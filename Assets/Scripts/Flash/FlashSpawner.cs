using UnityEngine;

public class FlashSpawner : MonoBehaviour
{
    public static FlashSpawner Instance;

    public GameObject flashPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnFlashAt(GameObject target, System.Action onFinished = null)
    {
        GameObject flash = Instantiate(flashPrefab, target.transform.position, Quaternion.identity);
        flash.GetComponent<FlashObject>().PlayFlash(onFinished);
    }
}
