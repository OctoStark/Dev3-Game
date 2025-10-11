using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public static SlotManager instance;
    public KeyItem heldKey;

    private void Awake()
    {
        instance = this;
    }

    public void PickupItem(KeyItem keyItem)
    {
        heldKey = keyItem;
        Debug.Log("Picked up: " + keyItem.keyID);
    }

    public void SetHeldItem(KeyItem keyItem)
    {
        heldKey = keyItem;
        Debug.Log("Item set to: " + keyItem.keyID);
    }


    public void ClearHeldItem()
    {
        heldKey = null;
        Debug.Log("Item cleared.");
    }

}
