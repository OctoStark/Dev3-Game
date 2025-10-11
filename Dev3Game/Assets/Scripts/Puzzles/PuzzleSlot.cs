using UnityEngine;

public class PuzzleSlot : MonoBehaviour
{
    public KeyItem requiredItem;
    private bool playerNearby;
    public bool isIn = false;

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
                return;
            }

            //if item isn't placed and player is holding the correct item, allow placement
            if (!isIn && SlotManager.instance.heldKey == requiredItem)
            {
                isIn = true;
                SlotManager.instance.ClearHeldItem();
                Debug.Log("Correct item placed!");
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