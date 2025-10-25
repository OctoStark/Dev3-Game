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
    [SerializeField] bool golem;
    [SerializeField] bool theseus;
    [SerializeField] bool guard;

    //public AudioManager aud;


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
        //gameManager.instance.updateGameGoal(1);
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
        if (golem) 
        {
            atkTimer = 0;
            agent.SetDestination(transform.position);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.golemAtk[Random.Range(0, AudioManager.Instance.golemAtk.Length)]);
            anim.SetTrigger("Attack");
        }
        else
        {
            atkTimer = 0;
            agent.SetDestination(transform.position);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.meleeAtk[Random.Range(0, AudioManager.Instance.meleeAtk.Length)]);
            anim.SetTrigger("Attack");
        }
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
        AudioManager.Instance.PlaySFX(AudioManager.Instance.stompAtk[Random.Range(0, AudioManager.Instance.stompAtk.Length)]);
        anim.SetTrigger("Slam");
    }
    void createSlam()
    {
        slamObject = Instantiate(groundSlam, slamPos);
        Destroy(slamObject, 1);
    }
    void shoot()
    {
        if (golem)
        {
            shootTimer = 0;
            agent.SetDestination(transform.position);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.golemThrow[Random.Range(0, AudioManager.Instance.golemThrow.Length)]);
            anim.SetTrigger("Shoot");
        }
        if (theseus)
        {
            shootTimer = 0;
            agent.SetDestination(transform.position);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.waveAtk[Random.Range(0, AudioManager.Instance.waveAtk.Length)]);
            anim.SetTrigger("Shoot");
        }
        else
        {
            shootTimer = 0;
            agent.SetDestination(transform.position);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.meleeAtk[Random.Range(0, AudioManager.Instance.meleeAtk.Length)]);
            anim.SetTrigger("Shoot");
        }

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
            if (HP > 0 && golem)
            {
                HP -= amount;
                //StartCoroutine(flashRed());
                AudioManager.Instance.PlaySFX(AudioManager.Instance.golemHurt[Random.Range(0, AudioManager.Instance.golemHurt.Length)]);
                agent.SetDestination(gameManager.instance.player.transform.position);
                anim.SetBool("TakeDamage", true);
            }
            if (HP > 0 && theseus)
            {
                HP -= amount;
                //StartCoroutine(flashRed());
                AudioManager.Instance.PlaySFX(AudioManager.Instance.theseusHurt[Random.Range(0, AudioManager.Instance.theseusHurt.Length)]);
                agent.SetDestination(gameManager.instance.player.transform.position);
                anim.SetBool("TakeDamage", true);
            }
            if (HP > 0 && guard)
            {
                HP -= amount;
                //StartCoroutine(flashRed());
                AudioManager.Instance.PlaySFX(AudioManager.Instance.humanHurt[Random.Range(0, AudioManager.Instance.humanHurt.Length)]);
                agent.SetDestination(gameManager.instance.player.transform.position);
                anim.SetBool("TakeDamage", true);
            }

            if (HP <= 0 && golem)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.golemDeath[Random.Range(0, AudioManager.Instance.golemDeath.Length)]);
                agent.SetDestination(transform.position);
                anim.SetBool("Death", true);                
                //if (isBoss)
                //{
                //    //open a door to next level or win trigger
                //}
            }
            if (HP <= 0 && (theseus || guard))
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.humanDeath[Random.Range(0, AudioManager.Instance.humanDeath.Length)]);
                agent.SetDestination(transform.position);
                anim.SetBool("Death", true);
                //if (isBoss)
                //{
                //    //open a door to next level or win trigger
                //}
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
        if (golem)
        {
            isPlayingStep = true;
            AudioManager.Instance.PlaySFX(AudioManager.Instance.golemStep[Random.Range(0, AudioManager.Instance.golemStep.Length)]);
            yield return new WaitForSeconds(.5f);
            isPlayingStep = false;
        }
        else
        {
            isPlayingStep = true;
            AudioManager.Instance.PlaySFX(AudioManager.Instance.humanStep[Random.Range(0, AudioManager.Instance.humanStep.Length)]);
            yield return new WaitForSeconds(.5f);
            isPlayingStep = false;
        }
    }
    //IEnumerator flashRed()
    //{
    //    model.material.color = Color.red;
    //    yield return new WaitForSeconds(1);
    //    model.material.color = colorOrig;
    //}


}
