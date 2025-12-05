using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DraggableMirror : MonoBehaviour
{
    [Header("Drag OK Zone (Workld Vector2)")]
    public bool useClamp = false;
    public Vector2 minPos = new Vector2(-5f, -3f);
    public Vector2 maxPos = new Vector2(5f, 3f);

    [Header("Rotation Setting")]
    public bool allowRotate = true;
    public float rotateSpeed = 100f;

    Camera cam;
    Vector3 offset;
    bool isDragging = false;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (!allowRotate) return;

        //마우스 휠로 회전
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.0001f)
        {
            float rot = -scroll * rotateSpeed;
            transform.Rotate(0f, 0f, rot);
        }
    }

    void OnMouseDown()
    {
        if (cam == null) return;

        isDragging = true;

        //마우스 위치 -> 월드 좌표
        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = transform.position.z;

        //클릭한 지점과 오브젝트 위치 차이 저장
        offset = transform.position - mouseWorld;
    }

    void OnMouseDrag()
    {
        if (!isDragging || cam == null) return;

        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = transform.position.z;

        Vector3 targetPos = mouseWorld + offset;

        if (useClamp)
        {
            targetPos.x = Mathf.Clamp(targetPos.x, minPos.x, maxPos.x);
            targetPos.y = Mathf.Clamp(targetPos.y, minPos.y, maxPos.y);
        }

        transform.position = targetPos;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }
}
