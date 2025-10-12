using UnityEngine;

public class bossSpawner : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] int spawnRate;
    [SerializeField] int tankDur;
    [SerializeField] Transform[] spawnPos;
    [SerializeField] bool tankMode;

    public bossAI boss;

    float spawnTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnRate && tankMode && !boss.defenseMode)
        {
            boss.defenseMode = true;
            spawn();
        }
        if (spawnTimer >= spawnRate + tankDur && tankMode)
        {
            boss.defenseMode = false;
            spawnTimer = 0;
            
        }

        else if (spawnTimer >= spawnRate && !tankMode)
        {
            spawn();
            spawnTimer = 0;
        }

        
    }
    
    void spawn()
    {
        int arrayPos = Random.Range(0, spawnPos.Length);
        Instantiate(objectToSpawn, spawnPos[arrayPos].position, spawnPos[arrayPos].rotation);
    }
}
