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

    //[SerializeField] List<gunStats> gunList = new List<gunStats>();
    //[SerializeField] GameObject gunModel;
    [SerializeField] int HP;
    [SerializeField] int AttackDamage;
    [SerializeField] float AttackRate;
    [SerializeField] int shootDist;
    [SerializeField] int rageMax;

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

    int HPOrig;
    int gunListPos;
    int RageOrig;
    int rageAdd = 1;

    float shootTimer;

    int jumpCount;

    bool isSprinting;
    bool isPlayingStep;
    bool TakingDamage;
    bool zuesBuffActive = false;
    bool poseidonBuffActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOrig = HP;
        //spawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward *shootDist, Color.yellow);

        if (!gameManager.instance.isPaused)
        {
        movement();
        }
        sprint();
    }
    void movement()
    {
        shootTimer += Time.deltaTime;
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

       // if (Input.GetButton("Fire1") && gunList.Count > 0 && gunList[gunListPos].ammoCur > 0 && shootTimer >= shootRate)
            shoot();
        selectGun();
        reload();
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
    void shoot()
    {
        shootTimer = 0;
   //     gunList[gunListPos].ammoCur--;
   //     aud.PlayOneShot(gunList[gunListPos].shootSound[Random.Range(0, gunList[gunListPos].shootSound.Length)], gunList[gunListPos].shootSoundVol);
        updatePlayerUI();

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreLayer))
        {
   //         Instantiate(gunList[gunListPos].hitEffect, hit.point, Quaternion.identity);
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
    }
    public void updatePlayerUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;

        if(TakingDamage == true)
        {
            gameManager.instance.playerRageBar.fillAmount = (float)rageAdd / rageMax;
        }

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

    void selectGun()
    {
        //if(Input.GetAxis("Mouse ScrollWheel") > 0 && gunListPos < gunList.Count - 1)
        //{
        //    gunListPos++;
        //    changeGun();
        //}
        //else if (Input.GetAxis("Mouse ScrollWheel") < 0 && gunListPos > 0)
        //{
        //    gunListPos--;
        //    changeGun();
        //}
    }

    //public void getGunStats(gunStats gun)
    //{
    //    gunList.Add(gun);
    //    gunListPos = gunList.Count - 1;
    //    changeGun();
    //}

    //void changeGun()
    //{
    //    shootDamage = gunList[gunListPos].shootDamage;
    //    shootDist = gunList[gunListPos].shootDist;
    //    shootRate = gunList[gunListPos].shootRate;

    //    gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[gunListPos].gunModel.GetComponent<MeshFilter>().sharedMesh;
    //    gunModel.GetComponent<MeshRenderer>().sharedMaterial= gunList[gunListPos].gunModel.GetComponent<MeshRenderer>().sharedMaterial;

    //    updatePlayerUI();
    //}

   // public void spawnPlayer()
   // {
    //    controller.transform.position = gameManager.instance.playerSpawnPos.transform.position;

    //    HP = HPOrig;
      //  updatePlayerUI();
   // }

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

    public void getPickUpStat(pickUp pickup)
    {
        switch (pickup.Type)
        {
            case pickUp.PickupType.Zeus:
                if (!zuesBuffActive)
                {
                    zuesBuffActive = true;
                    AttackDamage *= 2;
                    AttackRate *= 2f;
                }
                else
                {
                    return;
                }
                    break;

            case pickUp.PickupType.Poseidon:
                if (!poseidonBuffActive)
                {
                    speed *= 2;
                    sprintMod *= 2;
                }
                else
                {
                    return;
                }
                    break;
        }
    }
}
