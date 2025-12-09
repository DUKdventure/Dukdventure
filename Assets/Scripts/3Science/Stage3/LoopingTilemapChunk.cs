using UnityEngine;
using UnityEngine.Tilemaps;

public class LoopingTilemapChunk : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;   // 따라갈 카메라 (보통 Main Camera)

    [Header("TileMap Settings")]
    public float chunkWidth = 0f;       // 이 Chunk의 가로 길이 (0이면 자동 계산)
    public int totalChunks = 3;         // 씬에 있는 Chunk 개수 (2~3 권장)

    void Start()
    {
        // 카메라 자동 할당
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        // Chunk 가로 길이 자동 계산 (TilemapRenderer bounds 사용)
        if (chunkWidth <= 0f)
        {
            TilemapRenderer tr = GetComponentInChildren<TilemapRenderer>();
            if (tr != null)
            {
                chunkWidth = tr.bounds.size.x;
            }
            else
            {
                Debug.LogWarning($"{name} : TilemapRenderer를 찾지 못해서 chunkWidth를 계산 못했어.");
            }
        }
    }

    void Update()
    {
        if (cameraTransform == null || chunkWidth <= 0f) return;

        // 카메라가 이 Chunk의 오른쪽을 충분히 지나쳤는지 체크
        float camX = cameraTransform.position.x;
        float chunkRight = transform.position.x + chunkWidth;

        if (camX > chunkRight)
        {
            // 이 Chunk를 앞으로 totalChunks * chunkWidth 만큼 이동
            Vector3 pos = transform.position;
            pos.x += chunkWidth * totalChunks;
            transform.position = pos;
        }
    }
}
