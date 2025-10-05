using UnityEngine;

public class pickUp : MonoBehaviour
{
    //[SerializeField] gunStats gun;
    private void OnTriggerEnter(Collider other)
    {

        iPickUp pickupable = other.GetComponent<iPickUp>();

        if(pickupable != null )
        {
            //gun.ammoCur = gun.ammoMax;
            //pickupable.getGunStats(gun);
            Destroy(gameObject);

        }
    }
}
