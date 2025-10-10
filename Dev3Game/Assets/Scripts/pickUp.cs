using UnityEngine;
using System.Collections;
using System;

public class pickUp : MonoBehaviour
{
    //[SerializeField] gunStats gun;

    public enum PickupType { Zeus, Poseidon }

    [SerializeField] PickupType type;

    private void OnTriggerEnter(Collider other)
    {

        iPickUp pickupable = other.GetComponent<iPickUp>();

        if(pickupable != null && type == PickupType.Zeus)
        {
            //gun.ammoCur = gun.ammoMax;
            //pickupable.getGunStats(gun);
            
            Destroy(gameObject);

        }
    }
}
