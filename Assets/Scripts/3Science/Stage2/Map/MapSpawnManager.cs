using UnityEngine;

public class MapSpawnManager : MonoBehaviour
{
    public Transform defaultSpawnPoint;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        var gsm = GameStateManager.Instance;

        if(gsm != null && gsm.hasPendingSpawn)
        {
            //미니게임 클리어 후 돌아온 상태, 저장된 위치로 텔레포트
            player.transform.position = gsm.nextSpawnPosition;
            gsm.hasPendingSpawn = false ;
        }
        else
        {
            //평소에는 기본 위치에서 시작
            if (defaultSpawnPoint != null)
                player.transform.position = defaultSpawnPoint.position;
        }
    }

    
}
