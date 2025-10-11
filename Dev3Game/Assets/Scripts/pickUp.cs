using UnityEngine;
using System.Collections;
using System;

public class pickUp : MonoBehaviour
{
    //[SerializeField] gunStats gun;

    public enum PickupType { Zeus, Poseidon }

    [SerializeField] PickupType type;
    [SerializeField] int amount;

    public PickupType Type => type;
    public int Amount => amount;



    private void OnTriggerEnter(Collider other)
    {

        iPickUp pickupable = other.GetComponent<iPickUp>();

        if(pickupable != null)
        {
            //gun.ammoCur = gun.ammoMax;
            //pickupable.getGunStats(gun);
            pickupable.getPickUpStat(this);
                Destroy(gameObject);

        }
    }
}
