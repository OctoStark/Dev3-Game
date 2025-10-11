using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    [SerializeField] WeaponStats weapon;
    private void OnTriggerEnter(Collider other)
    {
        iPickUp pickupable = other.GetComponent<iPickUp>();

        if (pickupable != null)
        {
            //gun.ammoCur = gun.ammoMax;
            pickupable.getWeaponStats(weapon);
            Destroy(gameObject);

        }
    }

}
