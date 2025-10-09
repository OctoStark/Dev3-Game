using UnityEngine;

public class ManualSwitch : MonoBehaviour
{
    public GameObject switchObject;
    public bool active;
    private bool playerNearby;

    private void Update()
    {
        if (playerNearby && Input.GetButtonDown("Interact"))
        {
            ToggleSwitch();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }

    private void ToggleSwitch()
    {
        active = !active;

        if (switchObject)
        {
            Renderer rend = switchObject.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material.color = active ? Color.green : Color.red;
            }
        }

        Debug.Log("Switch " + (active ? "activated!" : "deactivated!"));
    }

}
