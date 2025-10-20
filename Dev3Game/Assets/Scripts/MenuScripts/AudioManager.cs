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

    private void Start()
    {
        musicSource.clip = backgroundMenu;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
