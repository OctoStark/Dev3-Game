using UnityEngine;

public class Axe : MonoBehaviour
{
    [SerializeField] int hitDamage;
    [SerializeField] float hitRate;
    [SerializeField] int hitRange;

    public AudioClip[] hitSound;
    [Range(0, 1)] public float hitSoundVol;
}
