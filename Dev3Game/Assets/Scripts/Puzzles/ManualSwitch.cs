using UnityEngine;

public class ManualSwitch : MonoBehaviour
{
    public GameObject switchObject;
    public bool isActivated = false;

    private bool isPlayerNearby = false;

    private void Update()
    {
        if (isPlayerNearby && Input.GetButtonDown("Interact"))
        {
            ToggleSwitch();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    private void ToggleSwitch()
    {
        isActivated = !isActivated;

        if (switchObject != null)
        {
            Renderer rend = switchObject.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material.color = isActivated ? Color.green : Color.red;
            }
        }

        Debug.Log("Switch " + (isActivated ? "activated!" : "deactivated!"));
    }

}
