using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Wire : MonoBehaviour
{
    [Header("References")]
    public SpriteRenderer wireSprite;         //Wire_end의 SpriteRenderer
    public Transform wireHandle;             //Wire_in
    public GameObject lightOn;               //Light
    public Transform startPointTransform;    //Wire_start

    Vector3 startPointWorld;    //Wire_start의 월드 위치
    Vector3 startHandleLocal;   //Wire_in의 초기 로컬 위치
    float initialLengthLocal;   //초기 선 길이 (로컬 기준)
    Vector3 initialRight;       //초기 방향(회전 값)

    void Start()
    {
        //시작점 고정
        startPointWorld = startPointTransform.position;

        //Wire_end의 기준 위치는 항상 시작점
        transform.position = startPointWorld;

        //초기 값 저장 (리셋용)
        if (wireHandle != null)
        {
            startHandleLocal = wireHandle.localPosition;
            initialLengthLocal = Mathf.Abs(startHandleLocal.x);
        }
        else
        {
            startHandleLocal = Vector3.zero;
            initialLengthLocal = 0f;
        }

        initialRight = transform.right;

        ResetWire();   //처음 모양으로 세팅
    }

    void ResetWire()
    {
        //위치 & 방향 리셋
        transform.position = startPointWorld;
        transform.right = initialRight;

        //길이 리셋
        if (wireSprite != null)
            wireSprite.size = new Vector2(initialLengthLocal, wireSprite.size.y);

        //핸들 위치 리셋
        if (wireHandle != null)
            wireHandle.localPosition = startHandleLocal;
    }

    void OnMouseDrag()
    {
        //mouse position to world point
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        //일단 마우스를 따라가게 선 업데이트
        UpdateWire(mousePos);

        //check for nearby connection points
        Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePos, 0.1f);
        foreach (Collider2D collider in colliders)
        {
            var otherWire = collider.GetComponent<Wire>();

            if (otherWire == null || otherWire == this)
                continue;

            Vector3 snapPos = otherWire.wireHandle.position;
            UpdateWire(snapPos);

            //같은 색인지 확인
            if (transform.parent.name.Equals(otherWire.transform.parent.name))
            {
                SuccessGame.Instance.SwitchChange(1);

                otherWire.Done();
                Done();
            }
            return;
        }
    }

    void Done()
    {
        if (lightOn != null)
            lightOn.SetActive(true);

        var col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Destroy(this);
    }

    private void OnMouseUp()
    {
        ResetWire();
    }

    void UpdateWire(Vector3 targetPos)
    {
        //시작점은 그대로
        transform.position = startPointWorld;

        //방향 & 거리
        Vector3 dir = targetPos - startPointWorld;
        float distWorld = dir.magnitude;  //월드 기준 거리

        if (dir.sqrMagnitude > 0.0001f)
            transform.right = dir.normalized;

        //부모까지 포함한 전체 스케일
        float scaleX = transform.lossyScale.x;
        if (Mathf.Approximately(scaleX, 0f))
            scaleX = 0.0001f;

        //월드 거리 → 로컬 거리로 변환
        float distLocal = distWorld / scaleX;

        //선 길이
        if (wireSprite != null)
            wireSprite.size = new Vector2(distLocal, wireSprite.size.y);

        //핸들(동그라미) 위치
        if (wireHandle != null)
            wireHandle.localPosition = new Vector3(distLocal, startHandleLocal.y, startHandleLocal.z);
    }
}
