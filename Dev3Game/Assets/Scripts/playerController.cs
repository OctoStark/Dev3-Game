using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerController : MonoBehaviour, IDamage, iPickUp
{
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] CharacterController controller;

    [SerializeField] public int HP;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int gravity;

    [SerializeField] List<WeaponStats> weaponList = new List<WeaponStats>();
    [SerializeField] GameObject weaponModel;

    [SerializeField] int hitDamage;
    [SerializeField] float hitRate;
    [SerializeField] int hitRange;

    //[SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] audStep;
    [Range(0, 1)][SerializeField] float audStepVol;
    [SerializeField] AudioClip[] audJump;
    [Range(0, 1)][SerializeField] float audJumpVol;
    [SerializeField] AudioClip[] audHurt;
    [Range(0, 1)][SerializeField] float audHurtVol;

    Vector3 moveDir;
    Vector3 playerVel;

    public int HPOrig;
    int weaponListPos;

    float hitTimer;

    int jumpCount;

    bool isSprinting;
    bool isPlayingStep;

    private float _pushPower = 2.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOrig = HP;
        //spawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
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

        if (Input.GetButtonDown("Fire1") && weaponList.Count > 0 && hitTimer >= hitRate)
        attack();
        selectWeapon();
        //  reload();
    }

    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
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
            //Instantiate(weaponList[weaponListPos].hitEffect, hit.point, Quaternion.identity);
            Debug.Log(hit.collider.name);

            IDamage dmg = hit.collider.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.takeDamage(hitDamage);
            }
        }
    }

    // void reload()
    // {
    //    if (Input.GetButtonDown("Reload")) 
    //         gunList[gunListPos].ammoCur = gunList[gunListPos].ammoMax;
    //    updatePlayerUI();
    //  }

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
    }
    public void updatePlayerUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;

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

    public void AddHealth(int healthAmount)
    {
        HP += healthAmount;
        if (HP > HPOrig)
        {
            HP = HPOrig;
        }
        updatePlayerUI();
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
        hitDamage = weaponList[weaponListPos].hitDamage;
        hitRange = weaponList[weaponListPos].hitRange;
        hitRate = weaponList[weaponListPos].hitRate;

        weaponModel.GetComponent<MeshFilter>().sharedMesh = weaponList[weaponListPos].weaponModel.GetComponent<MeshFilter>().sharedMesh;
        weaponModel.GetComponent<MeshRenderer>().sharedMaterial = weaponList[weaponListPos].weaponModel.GetComponent<MeshRenderer>().sharedMaterial;

        updatePlayerUI();
    }

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

}
