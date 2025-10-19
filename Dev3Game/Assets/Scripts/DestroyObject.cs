using UnityEngine;

public class DestroyObject : MonoBehaviour, IDamage
{
    [SerializeField] GameObject destObject;
    [SerializeField] Animator anim;
    public AudioManager audioManager;

    [SerializeField] private AudioClip destroySound;
    [SerializeField] int HP;
    [SerializeField] GameObject itemDrop;

    public void takeDamage(int amount)
    {
        if (HP > 0)
        {
            HP -= amount;

        }

        if (HP <= 0)
        {
            if (audioManager != null && destroySound != null)
            {
                audioManager.PlaySFX(destroySound);
            }

            Destroy(transform.parent.gameObject);
            Instantiate(itemDrop, transform.parent.position, transform.parent.rotation);
        }
    }
}
