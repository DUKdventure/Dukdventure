using UnityEngine;

public class RunnerPlayerAbility : MonoBehaviour
{
    public float cooldown = 5f;
    public GameObject cratePrefab;
    public float crateSpawnHeight = 2f;
    public RatChaser rat;

    string result;

    float timer = 0f;

    private void Start()
    {
        result = TutorialResultManager.Instance.finalResult;
    }

    void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;

        if(result == "Science")
        {
            if (Input.GetKeyDown(KeyCode.E) && timer <= 0f)
            {
                UseAbility();
            }
        }
        
    }

    void UseAbility()
    {
        timer = cooldown;

        // 쥐 위에 상자 소환
        if (cratePrefab != null && rat != null)
        {
            Vector3 spawnPos = rat.transform.position + Vector3.up * crateSpawnHeight;
            Instantiate(cratePrefab, spawnPos, Quaternion.identity);
        }

        // 쥐 느려지게
        if (rat != null)
        {
            rat.OnPlayerAbility();
        }
    }
}
