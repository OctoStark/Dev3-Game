using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Unity.VisualScripting;

public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Renderer model;
    [SerializeField] Transform headPos;
    
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject itemDrop;

    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int FOV;
    [SerializeField] int roamDist;
    [SerializeField] int roamPauseTimer;
    [SerializeField] float shootRate;
    [SerializeField] int animTransSpeed;

    public enemyTrigger fleeTrigger;
    public enemyTrigger sightTrigger;
    public enemyTrigger atkTrigger;

    //Color colorOrig;

    float shootTimer;
    float roamTimer;
    bool playerInTrigger;
    float angleToPlayer;
    float stoppingDistOrig;
    int FOVOrig;

    Vector3 startingPos;
    Vector3 playerDir;

    void Awake()
    {
        fleeTrigger.triggerEnter += fleeEnter;
        fleeTrigger.triggerExit += fleeExit;
        sightTrigger.triggerEnter += sightEnter;
        sightTrigger.triggerExit += sightExit;
        atkTrigger.triggerEnter += atkEnter;
        atkTrigger.triggerExit += atkExit;
    }

    private void atkExit(Collider collider)
    {
        //stop attack animation
    }

    private void atkEnter(Collider collider)
    {
        //attack animation
    }

    private void fleeExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            FOV = FOVOrig;
            faceTarget();
            agent.SetDestination(gameManager.instance.player.transform.position);
        }
    }

    private void fleeEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            FOV = 180;
            agent.SetDestination(-gameManager.instance.player.transform.position);
        }
           
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // colorOrig = model.material.color;
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        FOVOrig = FOV;
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;

        //setanimLocomotion();


        if (agent.remainingDistance <.01f)
        {
            roamTimer += Time.deltaTime;
        }

        if (playerInTrigger && !canSeePlayer())
        {
            checkRoam();
        }
        else if (!playerInTrigger)
        {
            checkRoam();
        }
    }

    //void setanimLocomotion()
    //{
    //    float agentSpeedCur = agent.velocity.normalized.magnitude;
    //    float animSpeedCur = anim.GetFloat("Speed");

    //    anim.SetFloat("Speed", Mathf.Lerp(animSpeedCur, agentSpeedCur, Time.deltaTime * animTransSpeed));
    //}

    void checkRoam()
    {
        if (roamTimer >= roamPauseTimer && agent.remainingDistance < .01f)
        {
            roam();
        }
    }

    void roam()
    {
        roamTimer = 0;
        agent.stoppingDistance = 0;

        Vector3 ranPos = Random.insideUnitSphere * roamDist;
        ranPos += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(ranPos, out hit, roamDist, 1);
        agent.SetDestination(hit.position);
    }

    bool canSeePlayer()
    { 
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);
        RaycastHit hit;
        Debug.DrawRay(headPos.position, new Vector3(playerDir.x, transform.position.y, playerDir.z));
        if (Physics.Raycast(headPos.position, new Vector3(playerDir.x, transform.position.y, playerDir.z), out hit))
        {
            if (angleToPlayer <= FOV && hit.collider.CompareTag("Player"))
            {
                agent.SetDestination(gameManager.instance.player.transform.position);

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    faceTarget();
                }
                if (shootTimer >= shootRate)
                {
                    shoot();
                }

                agent.stoppingDistance = stoppingDistOrig;
                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }
    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }
    public void sightEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInTrigger = true;
        
    }
    public void sightExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            agent.stoppingDistance = 0;
        }
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
        if (HP > 0)
        {
            HP -= amount;
            //StartCoroutine(flashRed());
            agent.SetDestination(gameManager.instance.player.transform.position);
        }

        if (HP <= 0)
        {
            Destroy(gameObject);
            Instantiate(itemDrop, transform.position, transform.rotation);
        }
    }
    //IEnumerator flashRed()
    //{
    //    model.material.color = Color.red;
    //    yield return new WaitForSeconds(0.1f);
    //    model.material.color = colorOrig;
    //}


}
