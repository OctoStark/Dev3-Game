using UnityEngine;

public class Traps : MonoBehaviour
{
    enum trapType {Spike, Dart}
    [SerializeField] trapType type;
    [SerializeField] Transform[] holePos;
    [SerializeField] GameObject dmgObj;

    [SerializeField] float shootRate;
    [SerializeField] int waitTime;
    [SerializeField] int trapDuration;

    float dartTimer;
    float spikeTimer;
    float shootTimer;
    bool playerInTrigger;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spikeTimer += Time.deltaTime;
        shootTimer += Time.deltaTime;

        if (type == trapType.Spike)
        {
            if (spikeTimer >= waitTime)
            {
                dmgObj.SetActive(true);
                if (spikeTimer >= trapDuration)
                {
                dmgObj.SetActive(false);
                spikeTimer = 0;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (type == trapType.Dart)
            {
                dartTimer = 0;
                dartTimer += Time.deltaTime;
                if (dartTimer <= trapDuration)
                {
                    if (shootTimer >= shootRate)
                    {
                        shoot();
                    }
                }
            }
        }
    }
    void shoot()
    {
        shootTimer = 0;
        for (int i = 0; i < holePos.Length; i++)
        {
            Instantiate(dmgObj, holePos[i].position, transform.rotation);
        }
    }
}
