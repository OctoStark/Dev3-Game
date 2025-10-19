using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Unity.VisualScripting;

public class bossAI : MonoBehaviour, IDamage
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Renderer model;
    [SerializeField] Transform headPos;
    
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject groundSlam;
    [SerializeField] Transform slamPos;
    //[SerializeField] GameObject punch;
    //[SerializeField] Transform punchPos;

    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] float atkRate;
    [SerializeField] float shootRate;
    [SerializeField] float slamRate;
    [SerializeField] int animTransSpeed;
    [SerializeField] GameObject itemDrop;
    [SerializeField] bool isBoss;

    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] audStep;
    [Range(0, 1)][SerializeField] float audStepVol;
    [SerializeField] AudioClip[] audAtk;
    [Range(0, 1)][SerializeField] float audAtkVol;
    [SerializeField] AudioClip[] audSlam;
    [Range(0, 1)][SerializeField] float audSlamVol;
    [SerializeField] AudioClip[] audShoot;
    [Range(0, 1)][SerializeField] float audShootVol;
    [SerializeField] AudioClip[] audHurt;
    [Range(0, 1)][SerializeField] float audHurtVol;
    [SerializeField] AudioClip[] audDeath;
    [Range(0, 1)][SerializeField] float audDeathVol;

    //Color colorOrig;

    GameObject slamObject;
    //GameObject punchObj;

    float atkTimer;
    float shootTimer;
    float slamTimer;
    bool playerInTrigger;
    float angleToPlayer;
    float stoppingDistOrig;
    public bool defenseMode;
    bool isPlayingStep;

    Vector3 startingPos;
    Vector3 playerDir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager.instance.updateGameGoal(1);
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        atkTimer += Time.deltaTime;
        shootTimer += Time.deltaTime;
        slamTimer += Time.deltaTime;
        playerDir = gameManager.instance.player.transform.position - headPos.position;

        setanimLocomotion();

        if (playerInTrigger && slamTimer >= slamRate && !defenseMode)
        {
            slam();
        }
        if (playerInTrigger && !defenseMode)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                faceTarget();
            }
            //normal attack
            if (atkTimer >= atkRate)
            {
                attack();
            }
        }
        if (!playerInTrigger && shootTimer >= shootRate && !defenseMode)
        {
            shoot();
        }
        if (!playerInTrigger && !defenseMode)
        {
            agent.SetDestination(gameManager.instance.player.transform.position);
            if (!isPlayingStep)
            {
                StartCoroutine(playStep());
            }
        }
        if (defenseMode)
        {
            agent.SetDestination(transform.position);
        }
    }

    void setanimLocomotion()
    {
        float agentSpeedCur = agent.velocity.normalized.magnitude;
        float animSpeedCur = anim.GetFloat("Speed");

        anim.SetFloat("Speed", Mathf.Lerp(animSpeedCur, agentSpeedCur, Time.deltaTime * animTransSpeed));
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInTrigger = true;
            agent.stoppingDistance = stoppingDistOrig;
        
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            agent.stoppingDistance = 0;
        }
    }
    void attack()
    {
        atkTimer = 0;
        agent.SetDestination(transform.position);
        aud.PlayOneShot(audAtk[Random.Range(0, audAtk.Length)], audAtkVol);
        anim.SetTrigger("Attack");
    }
    void createPunch()
    {
        //punchObj = Instantiate(punch, punchPos);
        //Debug.Log("punch");
        //Destroy(punchObj, 10);
    }
    void slam()
    {
        slamTimer = 0;
        agent.SetDestination(transform.position);
        aud.PlayOneShot(audSlam[Random.Range(0, audSlam.Length)], audSlamVol);
        anim.SetTrigger("Slam");
    }
    void createSlam()
    {
        slamObject = Instantiate(groundSlam, slamPos);
        Destroy(slamObject, 1);
    }
    void shoot()
    {
        shootTimer = 0;
        agent.SetDestination(transform.position);
        aud.PlayOneShot(audShoot[Random.Range(0, audShoot.Length)], audShootVol);
        anim.SetTrigger("Shoot");
    }

    void createBullet()
    {
        Instantiate(bullet, shootPos.position, transform.rotation);
    }
    public void takeDamage(int amount)
    {
        if (defenseMode)
        {

        }
        else
        {
            if (HP > 0)
            {
                HP -= amount;
                //StartCoroutine(flashRed());
                aud.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], audHurtVol);
                agent.SetDestination(gameManager.instance.player.transform.position);
                anim.SetBool("TakeDamage", true);
            }

            if (HP <= 0)
            {
                aud.PlayOneShot(audDeath[Random.Range(0, audDeath.Length)], audDeathVol);
                agent.SetDestination(transform.position);
                anim.SetBool("Death", true);                
                if (isBoss)
                {
                    //open a door to next level or win trigger
                }
            }
        }
    }
    void hit()
    {
        anim.SetBool("TakeDamage", false);
    }

    void dead()
    {
        Destroy(gameObject);
        Instantiate(itemDrop, transform.position, transform.rotation);
    }
    IEnumerator AtkWait()
    {
        yield return new WaitForSeconds(1);
    }
    IEnumerator playStep()
    {
        isPlayingStep = true;
        aud.PlayOneShot(audStep[Random.Range(0, audStep.Length)], audStepVol);
        yield return new WaitForSeconds(.5f);
        isPlayingStep = false;
    }
    //IEnumerator flashRed()
    //{
    //    model.material.color = Color.red;
    //    yield return new WaitForSeconds(1);
    //    model.material.color = colorOrig;
    //}


}
