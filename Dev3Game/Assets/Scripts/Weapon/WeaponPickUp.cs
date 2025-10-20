using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    [SerializeField] WeaponStats weapon;
    private void OnTriggerEnter(Collider other)
    {
        iPickUp pickupable = other.GetComponent<iPickUp>();

        if (pickupable != null)
        {
            pickupable.getWeaponStats(weapon);
            Destroy(gameObject);

        }
    }

}
