using UnityEngine;
using UnityEngine.UI;

public class ManualSwitch : MonoBehaviour
{
    public GameObject switchModel;
    public GameObject promptUI;
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
                switchManager.SequenceSwitch(this);
            }
            else
            {
                switchManager.ResetSequence();
            }

            if (promptUI)
                promptUI.SetActive(false);

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

        hitSwitch = true;

        if (switchAnim != null)
        {
            switchAnim.SetTrigger("TurnOn");
        }

        if (audioManager != null && audioManager.buttonSwitch != null)
        {
            audioManager.PlaySFX(audioManager.buttonSwitch);
        }

    }

    public void ResetSwitch()
    {
        hitSwitch = false;

        if (switchAnim != null)
        {
            switchAnim.SetTrigger("TurnOff");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            if (promptUI)
                promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            if (promptUI)
                promptUI.SetActive(false);

        }
    }



}
