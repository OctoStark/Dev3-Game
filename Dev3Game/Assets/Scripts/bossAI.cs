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

    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] float shootRate;
    [SerializeField] float slamRate;
    [SerializeField] int animTransSpeed;
    [SerializeField] GameObject itemDrop;
    [SerializeField] bool isBoss;

    //Color colorOrig;

    GameObject slamObject;

    float shootTimer;
    float slamTimer;
    bool playerInTrigger;
    float angleToPlayer;
    public bool defenseMode;

    Vector3 startingPos;
    Vector3 playerDir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager.instance.updateGameGoal(1);
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        slamTimer += Time.deltaTime;
        playerDir = gameManager.instance.player.transform.position - headPos.position;

        // setanimLocomotion();

        if (playerInTrigger && slamTimer >= slamRate && !defenseMode)
        {
            slam();
        }
        else if (playerInTrigger && !defenseMode)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                faceTarget();
            }
            //normal attack
        }
        else if (!playerInTrigger && shootTimer >= shootRate && !defenseMode)
        {
            shoot();
        }
        else if (!playerInTrigger && !defenseMode)
        {
            agent.SetDestination(gameManager.instance.player.transform.position);
        }
        else if (defenseMode)
        {
            agent.SetDestination(transform.position);
        }
    }

    //void setanimLocomotion()
    //{
    //    float agentSpeedCur = agent.velocity.normalized.magnitude;
    //    float animSpeedCur = anim.GetFloat("Speed");

    //    anim.SetFloat("Speed", Mathf.Lerp(animSpeedCur, agentSpeedCur, Time.deltaTime * animTransSpeed));
    //}

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInTrigger = true;
        
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            agent.stoppingDistance = 0;
        }
    }
    void slam()
    {
        slamTimer = 0;
        slamObject = Instantiate(groundSlam, slamPos);
        Destroy(slamObject, 1);
        
        
    }
    void shoot()
    {
        shootTimer = 0;
        //anim.SetTrigger("Shoot");
        createBullet();
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
                agent.SetDestination(gameManager.instance.player.transform.position);
                //faceTarget();
            }

            if (HP <= 0)
            {
                //gameManager.instance.updateGameGoal(-1);
                Destroy(gameObject);
                Instantiate(itemDrop, transform.position, transform.rotation);
                if (isBoss)
                {
                    //gameManager.instance.youWin();
                }
            }
        }
    }
    IEnumerator flashRed()
    {
        //model.material.color = Color.red;
        yield return new WaitForSeconds(1);
        //model.material.color = colorOrig;
    }


}
