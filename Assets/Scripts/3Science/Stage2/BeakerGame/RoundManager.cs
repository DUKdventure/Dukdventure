using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance { get; private set; }

    [Header("Rounds")]
    public GameObject[] rounds;

    public Transform player;

    int currentIndex = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        ActivateRound(0);    
    }

    public void ActivateRound(int index)
    {
        for (int i = 0; i < rounds.Length; i++)
        {
            if (rounds[i] != null)
                rounds[i].SetActive(i == index);
        }

        currentIndex = index;

        //해당 라운드의 SpawnPoint 위치로 플레이어 이동
        Transform spawn = rounds[index].transform.Find("SpawnPoint");
        if (spawn != null && player != null)
        {
            var rb = player.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;

            player.position = spawn.position;
        }
    }

    public void OnRoundClear()
    {
        //마지막 라운드가 아니면 다음 라운드로
        if (currentIndex < rounds.Length - 1)
        {
            ActivateRound(currentIndex + 1);
        }
        else
        {
            Debug.Log("모든 라운드 클리어!");
            // TODO: 여기서 클리어 패널 띄우기 등
        }
    }
}
