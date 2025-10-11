using UnityEngine;

[CreateAssetMenu]
public class WeaponStats : ScriptableObject
{
    public GameObject weaponModel;

    [Range(1, 50)]public int AttackDamage;
    [Range(0.1f, 3)] public float AttackRate;
    [Range(1, 100)] public int hitRange;

    public ParticleSystem hitEffect;
    public AudioClip[] hitSound;
    [Range(0, 1)] public float hitSoundVol;
}
