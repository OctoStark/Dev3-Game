using UnityEngine;

public class ManualSwitch : MonoBehaviour
{
    public GameObject switchModel;
    public bool hitSwitch;
    private bool playerNearby;
    public AudioManager audioManager;

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
        hitSwitch = !hitSwitch;
        
        if (switchModel != null)
        {
            Renderer rend = switchModel.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material.color = hitSwitch ? Color.green : Color.red;
            }
        }

        Debug.Log("Switch " + (hitSwitch ? "On!" : "Off!"));

        if (audioManager != null && audioManager.buttonSwitch != null) {
            audioManager.PlaySFX(audioManager.buttonSwitch);
        }

    }

}
