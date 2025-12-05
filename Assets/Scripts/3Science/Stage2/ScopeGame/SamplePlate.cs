using UnityEngine;

public class SamplePlate : MonoBehaviour
{
    public GameObject sampleBlur;
    public GameObject sampleSharp;

    void Awake()
    {
        sampleBlur.SetActive(false);
        sampleSharp.SetActive(false);
    }

    public void ShowBlur()
    {
        sampleBlur.SetActive(true);
        sampleSharp.SetActive(false);
    }

    public void ShowSharp()
    {
        sampleBlur.SetActive(false);
        sampleSharp.SetActive(true);
    }
}
