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
    [SerializeField] bool isHuman;
    [SerializeField] bool isMelee;


    public enemyTrigger fleeTrigger;
    public enemyTrigger sightTrigger;
    public enemyTrigger atkTrigger;

    //Color colorOrig;

    float shootTimer;
    float roamTimer;
    bool playerInTrigger;
    bool attack;
    float angleToPlayer;
    float stoppingDistOrig;
    int FOVOrig;
    bool isPlayingStep;

    Vector3 startingPos;
    Vector3 playerDir;
    Vector3 flee;

    void Awake()
    {
        stoppingDistOrig = agent.stoppingDistance;
        fleeTrigger.triggerEnter += fleeEnter;
        fleeTrigger.triggerExit += fleeExit;
        sightTrigger.triggerEnter += sightEnter;
        sightTrigger.triggerExit += sightExit;
        atkTrigger.triggerEnter += atkEnter;
        atkTrigger.triggerExit += atkExit;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // colorOrig = model.material.color;
        startingPos = transform.position;
        FOVOrig = FOV;
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;

        setanimLocomotion();


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

    void setanimLocomotion()
    {
        float agentSpeedCur = agent.velocity.normalized.magnitude;
        float animSpeedCur = anim.GetFloat("Speed");

        anim.SetFloat("Speed", Mathf.Lerp(animSpeedCur, agentSpeedCur, Time.deltaTime * animTransSpeed));
    }

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
        if (!isPlayingStep)
        {
            StartCoroutine(playStep());
        }
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
                if (agent.remainingDistance >= agent.stoppingDistance)
                {
                    agent.SetDestination(gameManager.instance.player.transform.position);
                    if (!isPlayingStep)
                    {
                        StartCoroutine(playStep());
                    }
                }

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    faceTarget();
                }
                if (attack)
                {
                    anim.SetTrigger("Attack");
                    if (shootTimer >= shootRate)
                    {
                        if (isMelee)
                        {                            
                            AudioSource.PlayClipAtPoint(AudioManager.Instance.meleeAtk[Random.Range(0, AudioManager.Instance.meleeAtk.Length)], transform.position);
                            shoot();
                        }
                        else
                        {
                            AudioSource.PlayClipAtPoint(AudioManager.Instance.bowAtk[Random.Range(0, AudioManager.Instance.bowAtk.Length)], transform.position);
                            shoot();
                        }
                    }
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
        {
            playerInTrigger = true;
            agent.stoppingDistance = stoppingDistOrig;
        }
            
        
    }

    public void sightExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            agent.stoppingDistance = 0;
        }
    }

    private void atkExit(Collider other)
    {
        //stop attack animation
        if (other.CompareTag("Player"))
        {
            attack = false;
        }
    }

    private void atkEnter(Collider other)
    {
        //attack animation
        if (other.CompareTag("Player"))
        {
            attack = true;
        }
    }

    private void fleeExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            attack = true;
            agent.stoppingDistance = stoppingDistOrig;
            //faceTarget();
            FOV = FOVOrig;
            agent.SetDestination(gameManager.instance.player.transform.position);
            //if (!isPlayingStep)
            //{
            //    StartCoroutine(playStep());
            //}
        }
    }

    private void fleeEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            attack = false;
            FOV = 180;
            NavMeshHit hit;
            flee = transform.position - gameManager.instance.player.transform.position;
            //agent.SetDestination(flee.normalized * 20);
            NavMesh.SamplePosition(flee, out hit, 120, 1);
            if (!isPlayingStep)
            {
                StartCoroutine(playStep());
            }
        }

    }
    void shoot()
    {
        shootTimer = 0;

        //anim.SetTrigger("Attack");
    }

    void createBullet()
    {
        Instantiate(bullet, shootPos.position, transform.rotation);
    }
    public void takeDamage(int amount)
    {
        if (HP > 0 && isHuman)
        {
            HP -= amount;
            //StartCoroutine(flashRed());
            AudioSource.PlayClipAtPoint(AudioManager.Instance.humanHurt[Random.Range(0, AudioManager.Instance.humanHurt.Length)], transform.position);
            anim.SetBool("TakeDamage", true);
            agent.SetDestination(gameManager.instance.player.transform.position);
            if (!isPlayingStep)
            {
                StartCoroutine(playStep());
            }
        }
        if (HP > 0 && !isHuman)
        {
            HP -= amount;
            //StartCoroutine(flashRed());
            AudioSource.PlayClipAtPoint(AudioManager.Instance.skeleHurt[Random.Range(0, AudioManager.Instance.skeleHurt.Length)], transform.position);
            anim.SetBool("TakeDamage", true);
            agent.SetDestination(gameManager.instance.player.transform.position);
            if (!isPlayingStep)
            {
                StartCoroutine(playStep());
            }
        }

        if (HP <= 0 && isHuman)
        {
            AudioSource.PlayClipAtPoint(AudioManager.Instance.humanDeath[Random.Range(0, AudioManager.Instance.humanDeath.Length)], transform.position);
            agent.SetDestination(transform.position);
            anim.SetBool("Death", true);
        }
        if (HP <= 0 && !isHuman)
        {
            AudioSource.PlayClipAtPoint(AudioManager.Instance.skeleDeath[Random.Range(0, AudioManager.Instance.skeleDeath.Length)], transform.position);
            agent.SetDestination(transform.position);
            anim.SetBool("Death", true);
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
    IEnumerator playStep()
    {
        if (isHuman)
        {
            isPlayingStep = true;
            AudioSource.PlayClipAtPoint(AudioManager.Instance.humanStep[Random.Range(0, AudioManager.Instance.humanStep.Length)], transform.position);
            yield return new WaitForSeconds(.5f);
            isPlayingStep = false;
        }
        else
        {
            isPlayingStep = true;
            AudioSource.PlayClipAtPoint(AudioManager.Instance.skeleStep[Random.Range(0, AudioManager.Instance.skeleStep.Length)], transform.position);
            yield return new WaitForSeconds(.5f);
            isPlayingStep = false;
        }
    }
    //IEnumerator flashRed()
    //{
    //    model.material.color = Color.red;
    //    yield return new WaitForSeconds(0.1f);
    //    model.material.color = colorOrig;
    //}
}
