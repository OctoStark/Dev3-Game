using UnityEngine;

public class PuzzleSlot : MonoBehaviour
{
    public KeyItem requiredItem;
    private bool playerNearby;
    public bool isIn = false;
    public AudioManager audioManager;
    public Wall wallControl;

    public GameObject gemKey;

    void Update()
    {
        if (playerNearby && Input.GetButtonDown("Interact"))
        {
            //if item is placed and player isnt holding anything, it can retrive it
            if (isIn && SlotManager.instance.heldKey == null)
            {
                SlotManager.instance.SetHeldItem(requiredItem);
                isIn = false;
                Debug.Log("Item taken back.");

                if (wallControl != null)
                {
                    wallControl.CloseWall();
                }
                return;
            }

            //if item isn't placed and player is holding the correct item, allow placement
            if (!isIn && SlotManager.instance.heldKey == requiredItem)
            {
                isIn = true;
                SlotManager.instance.ClearHeldItem();
                Debug.Log("Correct item placed!");
                audioManager.PlaySFX(audioManager.correctItem);
                gemKey.SetActive(true);
                if (wallControl != null)
                {
                    wallControl.OpenWall();
                }
            }
            else if (!isIn)
            {
                Debug.Log("Wrong item.");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }

}