using UnityEngine;
using System.Collections;

public class Dlog_01manager : MonoBehaviour
{
    [Header("Target Objects")]
    public GameObject dialogObject;      
    public GameObject[] objectsToDisable; 
    public GameObject[] objectsToEnable;  

    void OnEnable()
    {
        
        if (dialogObject != null)
            StartCoroutine(WatchDialog());
    }

    IEnumerator WatchDialog()
    {
        
        while (dialogObject.activeSelf)
            yield return null;

        
        foreach (var obj in objectsToDisable)
            if (obj != null) obj.SetActive(false);

        
        foreach (var obj in objectsToEnable)
            if (obj != null) obj.SetActive(true);
    }
}
