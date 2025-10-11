using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] KeyItem key;
    private bool playerNearby;

    void Update()
    {
        if (playerNearby && Input.GetButtonDown("Interact"))
        {
            SlotManager.instance.PickupItem(key);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearby = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearby = false;
    }

}
