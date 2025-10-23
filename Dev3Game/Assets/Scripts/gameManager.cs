using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.TimeZoneInfo;


public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] TMP_Text gameGoulCountText;
    public Image RagebarImage;
    private Color flashColor = Color.white;
    private float flashSpeed = 5f;

    private Color originalColor;
    private bool isFlashing;

    public Image playerHPBar;
    public Image playerRageBar;
    public GameObject playerDamageFlash;
    public GameObject poseidonsBlessingScreen;
    public GameObject zuesBlessingScreen;
    public GameObject herasCurse;
    public GameObject athenasCurse;
    public GameObject TutorialPopupScreen;
    public GameObject RageScreen;
    public TMP_Text ammoCur, ammoMax;

    public GameObject playerSpawnPos;
    public GameObject checkpointPopup;
    public GameObject player;
    public GameObject loadLevel;
    public playerController playerScript;



    public Animator transition;



    public bool isPaused;

    int gameGoalCount;

    float timeScaleOrig;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
        timeScaleOrig = Time.timeScale;

        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();

        if (RagebarImage == null)
        {
            RagebarImage = GetComponent<Image>();
        }
        originalColor = RagebarImage.color;
    }


    // Update is called once per frame
    void Update()
    {
        //TutorialPopup.SetActive(true);
       // yield return new WaitForSeconds(4f);
       // TutorialPopup.SetActive(false);

        if(RagebarImage.fillAmount >= 1f && !isFlashing)
        {
            StartCoroutine(FlashBar());
        }
        else if(RagebarImage.fillAmount < 1f && isFlashing)
        {
            StopAllCoroutines();
            RagebarImage.color = originalColor;
            isFlashing = false;
        }
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(true);
            }
            else if (menuActive == menuPause)
            {
                stateUnpause();
            }
        }
    }

    private IEnumerator FlashBar()
    {
        isFlashing = true;

        while(RagebarImage.fillAmount >= 1f)
        {
            float t = Mathf.PingPong(Time.time * flashSpeed, 1f);
            RagebarImage.color = Color.Lerp(originalColor, flashColor, t);
            yield return null;
        }

        RagebarImage.color = originalColor;
        isFlashing = false;
    }
    public void statePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void stateUnpause()
    {
        isPaused = !isPaused;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }

    //public void updateGameGoal(int amount)
    //{
    //    gameGoalCount += amount;
    //    gameGoulCountText.text = gameGoalCount.ToString("F0");
    //    if (gameGoalCount <= 0)
    //    {
    //        youWin();
    //    }
    //}
    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }

    public void youWin(bool win)
    {
        statePause();
        menuActive = menuWin;
        menuActive.SetActive(true);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        

    }

}
