using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class WireSocket : MonoBehaviour
{
    [Header("ID / Color")]
    public string wireId;        // 왼쪽 WireEndpoint의 wireId와 같아야 함

    [Header("Snap Point")]
    public Transform snapPoint;  // 선 끝이 붙을 위치 (없으면 자기 transform)

    [Header("Light")]
    public GameObject lightOn;

    public bool IsConnected { get; private set; }
    public WireEndpoint ConnectedWire { get; private set; }

    void Awake()
    {
        if (snapPoint == null)
            snapPoint = transform;
    }

    public void Connect(WireEndpoint wire)
    {
        IsConnected = true;
        ConnectedWire = wire;
        SetLight(true);
    }

    public void ResetState()
    {
        IsConnected = false;
        ConnectedWire = null;
        SetLight(false);
    }
    public void SetLight(bool on)
    {
        if (lightOn != null)
            lightOn.SetActive(on);
    }
}
