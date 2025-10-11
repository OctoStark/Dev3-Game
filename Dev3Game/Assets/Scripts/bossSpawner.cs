using UnityEngine;

public class bossSpawner : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;
    //[SerializeField] int numToSpawn;
    [SerializeField] int spawnRate;
    [SerializeField] Transform[] spawnPos;
    [SerializeField] bool tankMode;

    public bossAI boss;

    float spawnTimer;
    int spawnCount;
    int enemyCount;
    //bool startSpawning;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //gameManager.instance.updateGameGoal(numToSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer == spawnRate && tankMode && !boss.defenseMode)
        {
            boss.defenseMode = true;
            spawn();
            enemyCount += 4;
            if (enemyCount == 1)
            {
                boss.defenseMode = false;
                spawnTimer = 0;
            }
        }

        else if (spawnTimer >= spawnRate)
        {
            spawn();
            spawnTimer = 0;
        }

        
    }
    //public void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        startSpawning = true;
    //    }
    //}
    
    void spawn()
    {
        int arrayPos = Random.Range(0, spawnPos.Length);
        Instantiate(objectToSpawn, spawnPos[arrayPos].position, spawnPos[arrayPos].rotation);
        spawnCount++;
    }
}
