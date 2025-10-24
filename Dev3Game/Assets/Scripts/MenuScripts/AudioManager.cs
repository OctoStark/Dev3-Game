using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource gameMusicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource gameSFXSource;

    [Header("Player Audio")]
    public AudioClip pickupSFX;
    public AudioClip[] audStep;
    public AudioClip[] audJump;
    public AudioClip[] audHurt;

    [Header("Audio Clip")]
    public AudioClip[] axeHit;
    public AudioClip[] spearHit;
    public AudioClip buttonSwitch;
    public AudioClip WallMove;
    public AudioClip hitWall;
    public AudioClip breakWall;
    public AudioClip shrinkSound;
    public AudioClip switchWeapon;
    public AudioClip pressureOn;
    public AudioClip correctItem;
    public AudioClip incorrectItem;
    public AudioClip breakCrate;
    public AudioClip healthDrink;
    public AudioClip objectMove;
    public AudioClip backgroundMenu;
    public AudioClip backgroundGame;

    [Header("Enemy Audio")]
    public AudioClip[] humanStep;
    public AudioClip[] meleeAtk;
    public AudioClip[] bowAtk;
    public AudioClip[] humanHurt;
    public AudioClip[] humanDeath;
    public AudioClip[] skeleStep;
    public AudioClip[] skeleHurt;
    public AudioClip[] skeleDeath;
    public AudioClip[] golemStep;
    public AudioClip[] golemAtk;
    public AudioClip[] golemThrow;
    public AudioClip[] golemHurt;
    public AudioClip[] golemDeath;
    public AudioClip[] stompAtk;
    public AudioClip[] theseusHurt;
    public AudioClip[] waveAtk;

    [Header("Traps")]
    public AudioClip[] audBreak;
    public AudioClip[] audGone;
    public AudioClip[] audSpike;
    public AudioClip[] audDart;

    public static AudioManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        musicSource.clip = backgroundMenu;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        gameSFXSource.PlayOneShot(clip);
    }
}
