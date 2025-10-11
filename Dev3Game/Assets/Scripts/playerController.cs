using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static pickUp;

public class playerController : MonoBehaviour, IDamage, iPickUp
{
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] CharacterController controller;

    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int gravity;
    [SerializeField] Animator anim;

    [SerializeField] List<WeaponStats> weaponList = new List<WeaponStats>();
    [SerializeField] GameObject weaponModel;
    [SerializeField] public int HP;
    [SerializeField] int AttackDamage;
    [SerializeField] float AttackRate;
    [SerializeField] int hitRange;
    [SerializeField] private int rageMax;

    [SerializeField] Vector3 shrinkScale;
    [SerializeField] float shrinkDuration;


    //[SerializeField] AudioSource aud;
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

    private float _pushPower = 2.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOrig = HP;
        originalScale = transform.localScale;
        //spawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Speed", controller.velocity.normalized.magnitude);
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * hitRange, Color.yellow);

        if (!gameManager.instance.isPaused)
        {
        movement();
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

        if (Input.GetButtonDown("Fire1") && weaponList.Count > 0 && hitTimer >= AttackRate)
        attack();
        selectWeapon();
        //reload();
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
        hitTimer = 0;
        //     gunList[gunListPos].ammoCur--;
        //     aud.PlayOneShot(weaponList[weaponListPos].hitSound[Random.Range(0, weaponList[weaponListPos].hitSound.Length)], weaponList[weaponListPos].hitSoundVol);
        updatePlayerUI();

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, hitRange, ~ignoreLayer))
        {
            //Instantiate(gunList[gunListPos].hitEffect, hit.point, Quaternion.identity);
            Debug.Log(hit.collider.name);

            IDamage dmg = hit.collider.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.takeDamage(AttackDamage);
            }
        }
    }

    void reload()
    {
       // if (Input.GetButtonDown("Reload")) 
   //         gunList[gunListPos].ammoCur = gunList[gunListPos].ammoMax;
       // updatePlayerUI();
    }

    public void takeDamage(int amount)
    {
       // aud.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], audHurtVol);
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
        hitRange = weaponList[weaponListPos].hitRange;
        AttackRate = weaponList[weaponListPos].AttackRate;

        weaponModel.GetComponent<MeshFilter>().sharedMesh = weaponList[weaponListPos].weaponModel.GetComponent<MeshFilter>().sharedMesh;
        weaponModel.GetComponent<MeshRenderer>().sharedMaterial = weaponList[weaponListPos].weaponModel.GetComponent<MeshRenderer>().sharedMaterial;

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
        HP += healthAmount;
        if (HP > HPOrig)
        {
            HP = HPOrig;
        }
        updatePlayerUI();
    }


    public void getPickUpStat(pickUp pickup)
    {
        switch (pickup.Type)
        {
            case pickUp.PickupType.Zeus:
                if (!zuesBuffActive)
                {
                    zuesBuffActive = true;
                    AttackDamage *= pickup.Amount;
                    AttackRate *= pickup.Amount;
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
                }
                else
                {
                    return;
                }
                break;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Moving Object")
        {
            Rigidbody box = hit.collider.GetComponent<Rigidbody>();

            if (box != null)
            {
                Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
                box.isKinematic = false;
                box.linearVelocity = pushDirection * _pushPower;
            }
        }
    }

    public void ApplyShrinkCurse()
    {
        if (!isShrunk)
        {
            transform.localScale = shrinkScale;
            isShrunk = true;
            StartCoroutine(RemoveShrinkCurseAfterDelay());
        }
    }

    IEnumerator RemoveShrinkCurseAfterDelay()
    {
        yield return new WaitForSeconds(shrinkDuration);
        transform.localScale = originalScale;
        isShrunk = false;
    }


}
