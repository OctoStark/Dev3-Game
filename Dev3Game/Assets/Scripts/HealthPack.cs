using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public int healAmount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController player = other.GetComponent<playerController>();
            if (player && player.HP < player.HPOrig)
            {
                player.AddHealth(healAmount);
                Destroy(gameObject);
            }
        }
    }
}
