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
    }

    public void SetHeldItem(KeyItem keyItem)
    {
        heldKey = keyItem;
    }


    public void ClearHeldItem()
    {
        heldKey = null;
    }

}
