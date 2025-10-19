using UnityEngine;
using UnityEngine.UI;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] KeyItem key;
    private bool playerNearby;
    public GameObject promptUI;

    void Update()
    {
        if (playerNearby && Input.GetButtonDown("Interact"))
        {
            SlotManager.instance.PickupItem(key);
            Destroy(gameObject);
            if (promptUI)
                promptUI.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            if (promptUI)
                promptUI.SetActive(true);
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            if (promptUI)
                promptUI.SetActive(false);
        }
            
    }

}
