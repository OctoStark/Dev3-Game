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

    [Header("Audio Clip")]
    public AudioClip backgroundMenu;
    public AudioClip backgroundGame;
    public AudioClip buttonSwitch;
    public AudioClip WallMove;
    public AudioClip DoorOpen;
    public AudioClip shrinkSound;
    public AudioClip HitSound;
    public AudioClip pressureOn;
    public AudioClip correctItem;

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
