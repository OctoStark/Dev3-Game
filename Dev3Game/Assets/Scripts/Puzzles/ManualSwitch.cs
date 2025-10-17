using Unity.VisualScripting;
using UnityEngine;

public class ManualSwitch : MonoBehaviour
{
    public GameObject switchModel;
    public bool hitSwitch;
    private bool playerNearby;


    public AudioManager audioManager;
    private Animator switchAnim;
    public SwitchManager switchManager;

    private void Start()
    {
        switchAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (playerNearby && Input.GetButtonDown("Interact"))
        {
            if (!hitSwitch)
            {
                //ToggleSwitch();
                switchManager.SequenceSwitch(this);
            }
            else
            {
                switchManager.ResetSequence();
            }


        }
    }
    //private void ToggleSwitch()
    //{
    //    hitSwitch = !hitSwitch;

    //    if (switchModel != null)
    //    {

    //        if (switchAnim != null)
    //        {
    //            switchAnim.SetTrigger(hitSwitch ? "TurnOn" : "TurnOff");
    //        }
    //    }

    //    Debug.Log("Switch " + (hitSwitch ? "On!" : "Off!"));

    //    if (audioManager != null && audioManager.buttonSwitch != null)
    //    {
    //        audioManager.PlaySFX(audioManager.buttonSwitch);
    //    }

    //}

    public void ActivateSwitch()
    {
        //if (hitSwitch) return; // Prevent double activation

        hitSwitch = true;

        if (switchAnim != null)
        {
            switchAnim.SetTrigger("TurnOn");
        }

        if (audioManager != null && audioManager.buttonSwitch != null)
        {
            audioManager.PlaySFX(audioManager.buttonSwitch);
        }

        Debug.Log($"{gameObject.name} activated.");
    }

    public void ResetSwitch()
    {
        hitSwitch = false;

        if (switchAnim != null)
        {
            switchAnim.SetTrigger("TurnOff");
        }

        Debug.Log($"{gameObject.name} reset.");
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



}
