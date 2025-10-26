using UnityEngine;

public class Traps : MonoBehaviour
{
    enum trapType {Spike, Dart}
    [SerializeField] trapType type;
    [SerializeField] Transform[] holePos;
    [SerializeField] GameObject dmgObj;

    [SerializeField] float shootRate;
    [SerializeField] float waitTime;
    [SerializeField] float trapDuration;
    [SerializeField] float trapDelay;

    float dartTimer;
    float spikeTimer;
    float shootTimer;
    float delayTimer;
    bool playerInTrigger;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        delayTimer += Time.deltaTime;
        shootTimer += Time.deltaTime;
        if (delayTimer >= trapDelay)
        {
            spikeTimer += Time.deltaTime;
        }

        if (type == trapType.Spike)
        {

            if (spikeTimer >= waitTime)
            {
                dmgObj.SetActive(true);
                //AudioSource.PlayClipAtPoint(AudioManager.Instance.audSpike[Random.Range(0, AudioManager.Instance.audSpike.Length)], transform.position);
                if (spikeTimer >= trapDuration)
                {
                dmgObj.SetActive(false);
                spikeTimer = 0;
                }
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (type == trapType.Dart)
            {
                if (shootTimer >= shootRate)
                {
                    shoot();
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.audDart[Random.Range(0, AudioManager.Instance.audDart.Length)]);
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
