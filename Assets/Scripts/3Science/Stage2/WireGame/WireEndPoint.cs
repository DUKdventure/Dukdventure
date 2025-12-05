using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class WireEndpoint : MonoBehaviour
{
    [Header("ID / Color")]
    public string wireId;                // "Red", "Blue" 같은 문자열로 색 구분

    [Header("References")]
    public SpriteRenderer wireSprite;    // 선 스프라이트
    public Transform wireHandle;         // 끝 동그라미 핸들
    public Transform startPoint;         // 시작점(왼쪽 고정점)
    public GameObject lightOn;           // 전등(왼쪽에 있는 불)

    Vector3 startPointWorld;
    Vector3 handleLocalStart;
    float initialLengthLocal;
    Vector3 initialRight;

    bool isConnected = false;
    WireSocket connectedSocket;
    Camera mainCam;
    WireGameController controller;

    void Awake()
    {
        mainCam = Camera.main;
        controller = FindFirstObjectByType<WireGameController>();

        if (startPoint != null)
            startPointWorld = startPoint.position;
        else
            startPointWorld = transform.position;

        if (wireHandle != null)
            handleLocalStart = wireHandle.localPosition;

        if (wireSprite != null)
            initialLengthLocal = wireSprite.size.x;

        initialRight = transform.right;
    }

    void Start()
    {
        ResetWireShape();
        SetLight(false);
    }

    public bool IsConnected => isConnected;

    public void ResetState()
    {
        isConnected = false;
        connectedSocket = null;

        ResetWireShape();
        SetLight(false);

        var col = GetComponent<Collider2D>();
        if (col) col.enabled = true;
    }

    void ResetWireShape()
    {
        transform.position = startPointWorld;
        transform.right = initialRight;

        if (wireSprite != null)
            wireSprite.size = new Vector2(initialLengthLocal, wireSprite.size.y);

        if (wireHandle != null)
            wireHandle.localPosition = handleLocalStart;
    }

    void SetLight(bool on)
    {
        if (lightOn != null)
            lightOn.SetActive(on);
    }

    void OnMouseDown()
    {
        if (!controller) return;
        if (!controller.CanDrag(this)) return;
    }

    void OnMouseDrag()
    {
        if (!controller || !controller.CanDrag(this)) return;
        if (isConnected) return;

        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        UpdateWireTo(mousePos);
    }

    void OnMouseUp()
    {
        if (!controller || isConnected) return;

        // 마우스 위치 근처의 소켓 찾기
        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        WireSocket target = controller.FindNearestSocket(mousePos, wireId, 0.3f);

        if (target != null)
        {
            controller.TryConnect(this, target);
        }
        else
        {
            // 못 꽂으면 원래대로
            ResetWireShape();
        }
    }

    public void AttachToSocket(WireSocket socket)
    {
        isConnected = true;
        connectedSocket = socket;

        // 선을 소켓 위치까지 땡겨서 고정
        Vector3 target = socket.snapPoint.position;
        UpdateWireTo(target);

        // collider 비활성화
        var col = GetComponent<Collider2D>();
        if (col) col.enabled = false;

        SetLight(true);
    }

    public void Detach()
    {
        isConnected = false;
        connectedSocket = null;
        ResetWireShape();
        SetLight(false);

        var col = GetComponent<Collider2D>();
        if (col) col.enabled = true;
    }

    void UpdateWireTo(Vector3 targetPos)
    {
        transform.position = startPointWorld;

        Vector3 dir = targetPos - startPointWorld;
        float distWorld = dir.magnitude;

        if (distWorld > 0.0001f)
            transform.right = dir.normalized;

        float scaleX = Mathf.Abs(transform.lossyScale.x);
        if (scaleX < 0.0001f) scaleX = 0.0001f;

        float distLocal = distWorld / scaleX;

        if (wireSprite != null)
            wireSprite.size = new Vector2(distLocal, wireSprite.size.y);

        if (wireHandle != null)
            wireHandle.localPosition = new Vector3(distLocal, handleLocalStart.y, handleLocalStart.z);
    }

    // 패턴 보여줄 때/정답일 때 불 켜거나 끌 수 있도록
    public void SetPatternLight(bool on)
    {
        SetLight(on);
    }
}