using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static pickUp;

public class playerController : MonoBehaviour, IDamage, iPickUp
{
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] CharacterController controller;
    [SerializeField] Transform cameraHolder;
    [SerializeField] Animator anim;
    public AudioManager audioManager;


    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int gravity;
    [SerializeField] float attackRange;
    //[SerializeField] int attackDamage;


    [SerializeField] List<WeaponStats> weaponList = new List<WeaponStats>();
    [SerializeField] GameObject weaponModel;
    [SerializeField] public int HP;
    [SerializeField] int AttackDamage;
    [SerializeField] float AttackRate;
    [SerializeField] int hitRange;
    [SerializeField] private int rageMax;
    [SerializeField] float animTransSpeed;

    [SerializeField] Vector3 shrinkScale;
    [SerializeField] float shrinkDuration;
   
    [SerializeField] float lookSpeed = 2f;
    [SerializeField] bool isFreeLooking = false;

    float pitch = 0f;


    //[SerializeField] AudioSource aud;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pickupSFX;
    [SerializeField] AudioClip[] audStep;
    [Range(0, 1)][SerializeField] float audStepVol;
    [SerializeField] AudioClip[] audJump;
    [Range(0, 1)][SerializeField] float audJumpVol;
    [SerializeField] AudioClip[] audHurt;
    [Range(0, 1)][SerializeField] float audHurtVol;

    Vector3 moveDir;
    Vector3 playerVel;
    PickupType type;

    public int HPOrig;
    private Vector3 originalScale;
    public int origAttackDamage;
    float origAttackRate;
    int origSpeed;
    int origSprintMod;
    int weaponListPos;
    int RageOrig;
    float targetRageFill;
    int rageAdd;

    float hitTimer;

    int jumpCount;

    bool isSprinting;
    bool isPlayingStep;
    bool TakingDamage;
    bool zuesBuffActive = false;
    bool poseidonBuffActive = false;
    bool athenaDebuffActive = false;
    bool heraDebuffActive = false;
    private bool isShrunk = false;
    public bool isBlocking = false;
    private bool hasPlayedPush = false;

    private float _pushPower = 2.0f;
   

    //Rage Dash
    [SerializeField] float rageDashSpeed = 25f;
    [SerializeField] float rageDuration = .5f;
    [SerializeField] float rageDamageMod;
    [SerializeField] float rageKnockbackForce;
    [SerializeField] float rageHitRadius = 1.5f;
    [SerializeField] LayerMask enemyLayer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();

        HPOrig = HP;
        originalScale = transform.localScale;
        origAttackDamage = AttackDamage;
        origAttackRate = AttackRate;
        origSpeed = speed;
        origSprintMod = sprintMod;
        //spawnPlayer();
        //StartCoroutine(flashTutScreen());
    }

    // Update is called once per frame
    void Update()
    {
       
        isFreeLooking = Input.GetButton("FreeLook"); 

        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        if (isFreeLooking && !gameManager.instance.isPaused)
        {
            // Rotate cameraHolder independently
            cameraHolder.Rotate(Vector3.up, mouseX, Space.World);
            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, -60f, 60f);
            cameraHolder.localRotation = Quaternion.Euler(pitch, cameraHolder.localEulerAngles.y, 0f);
        }
        else if (!isFreeLooking && !gameManager.instance.isPaused)
        {
            // Rotate player body with mouseX
            transform.Rotate(Vector3.up, mouseX);

            // Rotate camera pitch only
            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, -60f, 60f);
            cameraHolder.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }


        if (Input.GetButtonDown("Block")) 
        {
            StartBlocking();
        }

        if (Input.GetButtonUp("Block"))
        {
            StopBlocking();
        }


        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * hitRange, Color.yellow);

        if (!gameManager.instance.isPaused)
        {
        movement();
            Rage();
        }
        sprint();
        
    }
    void movement()
    {
    
        hitTimer += Time.deltaTime;
        if (controller.isGrounded)
        {
            if (moveDir.normalized.magnitude > .3f && !isPlayingStep)
            {
                StartCoroutine(playStep());
            }

            jumpCount = 0;
            playerVel = Vector3.zero;
        }
        else
        {
            playerVel.y -= gravity * Time.deltaTime;
        }
            moveDir = (Input.GetAxis("Horizontal") * transform.right) +
                      (Input.GetAxis("Vertical") * transform.forward);

        controller.Move(moveDir * speed * Time.deltaTime);

        jump();
        controller.Move(playerVel * Time.deltaTime);

        if (Input.GetButtonDown("Fire1") && weaponList.Count > 0 )
        attack();
        selectWeapon();
        setanimLocomotion();
        //reload();
       
    }
    void setanimLocomotion()
    {
        // Use input direction magnitude for smoother animation
        float agentSpeedCur = moveDir.magnitude * (isSprinting ? 2f : 1f); // Scale to match blend tree thresholds
        float animSpeedCur = anim.GetFloat("Speed");

        float smoothedSpeed = Mathf.Lerp(animSpeedCur, agentSpeedCur, Time.deltaTime * animTransSpeed);
        anim.SetFloat("Speed", smoothedSpeed);




    }
    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount <  jumpMax)
        {
           // aud.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol);
            jumpCount++;
            playerVel.y = jumpSpeed;
        }
    }
    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
            isSprinting = false;
        }
    }
    void attack()
    {


        Debug.Log("Player attacked");
        string weaponName = weaponList[weaponListPos].weaponModel.name.ToLower();

        if (weaponName.Contains("doubleaxe") && audioManager.axeHit.Length > 0)
        {
            AudioClip clip = audioManager.axeHit[Random.Range(0, audioManager.axeHit.Length)];
            audioManager.PlaySFX(clip);
        }
        else if (weaponName.Contains("spear") && audioManager.spearHit.Length > 0)
        {
            AudioClip clip = audioManager.spearHit[Random.Range(0, audioManager.spearHit.Length)];
            audioManager.PlaySFX(clip);
        }

            Vector3 attackOrigin = transform.position + transform.forward;
        Collider[] hits = Physics.OverlapSphere(attackOrigin, attackRange, enemyLayer);

        foreach (Collider hit in hits)
        {
            IDamage Dmg = hit.GetComponent<IDamage>();
            if (Dmg != null)
            {
                Dmg.takeDamage(AttackDamage);
                Debug.Log("Hit " + hit.name + "for " + AttackDamage + "damage.");
            }
        }
    

    }

    void StartBlocking()
    {
        isBlocking = true;
        anim.SetBool("Block", true);

    }

    void StopBlocking()
    {
        isBlocking = false;
        anim.SetBool("Block", false);
    }



    // void reload()
    // {
    //    // if (Input.GetButtonDown("Reload")) 
    ////         gunList[gunListPos].ammoCur = gunList[gunListPos].ammoMax;
    //    // updatePlayerUI();
    // }

    public void takeDamage(int amount)
    {
       // aud.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], audHurtVol);
        if (isBlocking)
        {
            return;
        }
        HP -= amount;
        updatePlayerUI();
        StartCoroutine(flashDamage());
        if (HP <= 0)
        {
            //Hey, I'm dead!!
            gameManager.instance.youLose();
        }

        TakingDamage = true;
        rageAdd += 1;
        if (rageAdd >= rageMax)
        {
            rageAdd = rageMax;

            updatePlayerUI();
        }
    }
    public void updatePlayerUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
        gameManager.instance.playerRageBar.fillAmount = (float)rageAdd / rageMax;

        //if (gunList.Count > 0)
        //{
        //    gameManager.instance.ammoCur.text = gunList[gunListPos].ammoCur.ToString("F0");
        //    gameManager.instance.ammoMax.text = gunList[gunListPos].ammoMax.ToString("F0");
        //}

    }

    IEnumerator flashDamage()
    {
        gameManager.instance.playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(.1f);
        gameManager.instance.playerDamageFlash.SetActive(false);
    }

    //IEnumerator flashTutScreen()
    //{
    //    gameManager.instance.TutorialPopupScreen.SetActive(true);
    //    yield return new WaitForSeconds(10f);
    //    gameManager.instance.TutorialPopupScreen.SetActive(false);
    //}

    IEnumerator flashPosBlessing()
    {
        gameManager.instance.poseidonsBlessingScreen.SetActive(true);
        yield return new WaitForSeconds(10f);
        gameManager.instance.poseidonsBlessingScreen.SetActive(false);
    }

    IEnumerator flashzuesBlessing()
    {
        gameManager.instance.zuesBlessingScreen.SetActive(true);
        yield return new WaitForSeconds(10f);
        gameManager.instance.zuesBlessingScreen.SetActive(false);
    }

    IEnumerator flashHeraCurse()
    {
        gameManager.instance.herasCurse.SetActive(true);
        yield return new WaitForSeconds(10f);
        gameManager.instance.herasCurse.SetActive(false);
    }

    IEnumerator flashAthenaCurse()
    {
        gameManager.instance.athenasCurse.SetActive(true);
        yield return new WaitForSeconds(10f);
        gameManager.instance.athenasCurse.SetActive(false);
    }

    private IEnumerator ZuesBuffDuration()
    {
        yield return new WaitForSeconds(10f);

        AttackDamage = origAttackDamage;
        AttackRate = origAttackRate;

        zuesBuffActive = false;
    }

    private IEnumerator PoseidonBuffDuration()
    {
        yield return new WaitForSeconds(10f);

        speed = origSpeed;
        sprintMod = origSprintMod;

        poseidonBuffActive = false;
    }

    private IEnumerator HerasDuration()
    {
        yield return new WaitForSeconds(10f);

        AttackDamage = origAttackDamage;

        athenaDebuffActive = false;
    }

    void selectWeapon()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && weaponListPos < weaponList.Count - 1)
        {
            weaponListPos++;
            changeWeapon();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && weaponListPos > 0)
        {
            weaponListPos--;
            changeWeapon();
        }

    }

    public void getWeaponStats(WeaponStats weapon)
    {
        weaponList.Add(weapon);
        weaponListPos = weaponList.Count - 1;
        changeWeapon();
    }

    void changeWeapon()
    {
        AttackDamage = weaponList[weaponListPos].AttackDamage;
        attackRange = weaponList[weaponListPos].hitRange;
        AttackRate = weaponList[weaponListPos].AttackRate;

        weaponModel.GetComponent<MeshFilter>().sharedMesh = weaponList[weaponListPos].weaponModel.GetComponent<MeshFilter>().sharedMesh;
        weaponModel.GetComponent<MeshRenderer>().sharedMaterial = weaponList[weaponListPos].weaponModel.GetComponent<MeshRenderer>().sharedMaterial;

        updatePlayerUI();
    }

     public void spawnPlayer()
     {
        controller.transform.position = gameManager.instance.playerSpawnPos.transform.position;

        HP = HPOrig;
      updatePlayerUI();
     }

    IEnumerator playStep()
    {
        isPlayingStep = true;
        //aud.PlayOneShot(audStep[Random.Range(0, audStep.Length)], audStepVol);
        if (isSprinting)
        {
            yield return new WaitForSeconds(.3f);
        }
        else
        {
            yield return new WaitForSeconds(.5f);
        }
        isPlayingStep = false;
    }

    public void AddHealth(int healthAmount)
    {
        if (audioManager != null && audioManager.healthDrink != null)
        {
            audioManager.PlaySFX(audioManager.healthDrink);
        }
        HP += healthAmount;
        if (HP > HPOrig)
        {
            HP = HPOrig;
        }
        updatePlayerUI();
    }


    public void getPickUpStat(pickUp pickup)
    {
        if (pickup == null)
        {
            Debug.LogWarning("Pickup is null - cannot apply buff/debuff");
            return;
        }

        if(pickup.Amount <= 0)
        {
            Debug.LogWarning("Invalid pickup amount: {pickup.Amount}");
            return;
        }
        switch (pickup.Type)
        {
            case pickUp.PickupType.Zeus:
                if (!zuesBuffActive)
                {
                    zuesBuffActive = true;
                    AttackDamage *= pickup.Amount;
                    AttackRate *= pickup.Amount;
                    PlayPickupSFX(pickupSFX);
                    StartCoroutine(ZuesBuffDuration());
                    StartCoroutine(flashzuesBlessing());
                }
                else
                {
                    return;
                }
                    break;

            case pickUp.PickupType.Poseidon:
                if (!poseidonBuffActive)
                {
                    speed *= pickup.Amount;
                    sprintMod *= pickup.Amount;
                    PlayPickupSFX(pickupSFX);
                    StartCoroutine(PoseidonBuffDuration());
                    StartCoroutine(flashPosBlessing());
                }
                else
                {
                    return;
                }
                    break;

            case pickUp.PickupType.Athena:
                if (!athenaDebuffActive)
                {
                    athenaDebuffActive = true;
                    AttackDamage -= pickup.Amount;
                    PlayPickupSFX(pickupSFX);
                    StartCoroutine(HerasDuration());
                    StartCoroutine(flashAthenaCurse());
                }
                else
                {
                    return;
                }
                break;

            case pickUp.PickupType.Hera:
                if (!heraDebuffActive)
                {
                    heraDebuffActive = true;
                    ApplyShrinkCurse();
                    PlayPickupSFX(pickupSFX);
                    StartCoroutine(flashHeraCurse());
                    StartCoroutine(RemoveShrinkCurseAfterDelay());
                }
                else
                {
                    return;
                }
                break;

        }
    }

    private void Rage()
    {

        if (Input.GetButtonDown("Rage"))
        {
            Debug.Log("Rage button Pressed");
        }
        if(rageAdd == rageMax && Input.GetButtonDown("Rage"))
        {
            Debug.Log("Rage Dash Activated");
            StartCoroutine(RageDash());
  
        }
    }

    IEnumerator RageDash()
    {
        float elapsed = 0f;
        Vector3 dashDir = transform.forward;

        rageAdd = 0;
        gameManager.instance.playerRageBar.fillAmount = 0;

        bool prevControlEnabled = controller.enabled;
        controller.enabled = true;

        while (elapsed < rageDuration)
        {
            elapsed += Time.deltaTime;

            controller.Move(dashDir * rageDashSpeed * Time.deltaTime);

            Collider[] hitEnemies = Physics.OverlapSphere(transform.position + dashDir * rageHitRadius, rageHitRadius, enemyLayer);

            foreach(Collider enemy in hitEnemies)
            {
                IDamage dmg = enemy.GetComponent<IDamage>();
                if (dmg != null)
                {
                    dmg.takeDamage(Mathf.RoundToInt(AttackDamage * rageDamageMod));

                    Rigidbody rb = enemy.GetComponent<Rigidbody>();
                    if(rb != null)
                    {
                        rb.AddForce(dashDir * rageKnockbackForce, ForceMode.Impulse);
                    }
                }
            }

            yield return null;
        }
        controller.enabled = prevControlEnabled;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Moving Object")
        {
            Rigidbody box = hit.collider.GetComponent<Rigidbody>();

            if (box != null)
            {
                Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
                //box.isKinematic = false;
                box.linearVelocity = pushDirection * _pushPower;
                if (!hasPlayedPush && audioManager != null && audioManager.objectMove != null)
                {
                    audioManager.PlaySFX(audioManager.objectMove);
                    hasPlayedPush = true;
                    Invoke(nameof(ResetPushSound), 1f);
                }
            }
        }
    }

    private void ResetPushSound()
    {
        hasPlayedPush = false;
    }

    public void ApplyShrinkCurse()
    {
        if (!isShrunk)
        {
            transform.localScale = shrinkScale;
            isShrunk = true;
            if (audioManager != null && audioManager.shrinkSound != null)
            {
                audioManager.PlaySFX(audioManager.shrinkSound);
            }
            StartCoroutine(RemoveShrinkCurseAfterDelay());
        }
    }

    IEnumerator RemoveShrinkCurseAfterDelay()
    {
        yield return new WaitForSeconds(shrinkDuration);
        transform.localScale = originalScale;
        isShrunk = false;
    }

    private void PlayPickupSFX(AudioClip clip)
    {
        if(audioSource != null && clip != null)
        {
            audioSource.pitch = Random.Range(.95f, 1.05f);
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Missing AudioSource or AudioClip for pickup SFX.");
        }
    }


}
