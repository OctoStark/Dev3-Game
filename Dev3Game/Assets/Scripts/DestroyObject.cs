using UnityEngine;

public class DestroyObject : MonoBehaviour, IDamage
{
    [SerializeField] GameObject destObject;
    [SerializeField] Animator anim;
    public AudioManager audioManager;

    [SerializeField] int HP;
    [SerializeField] GameObject itemDrop;

    public void takeDamage(int amount)
    {
        if (HP > 0)
        {
            HP -= amount;
            //audioManager.PlaySFX(audioManager.buttonSwitch);

        }

        if (HP <= 0)
        {
            Destroy(gameObject);
            Instantiate(itemDrop, transform.parent.position, transform.parent.rotation);
        }
    }
}
