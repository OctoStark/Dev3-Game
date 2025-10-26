using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PuzzleSlot : MonoBehaviour
{
    public KeyItem requiredItem;
    private bool playerNearby;
    public bool isIn = false;
    public AudioManager audioManager;
    public Wall wallControl;

    public GameObject gemKey;
    public GameObject promptUI;
    public GameObject keyPromptUI;

    void Update()
    {
        if (playerNearby && Input.GetButtonDown("Interact"))
        {
            //if item is placed and player isnt holding anything, it can retrive it
            if (isIn && SlotManager.instance.heldKey == null)
            {
                SlotManager.instance.SetHeldItem(requiredItem);
                isIn = false;
                audioManager.PlaySFX(audioManager.incorrectItem);
                gemKey.SetActive(false);

                if (wallControl != null)
                {
                    wallControl.CloseWall();
                }

                if (promptUI)
                    promptUI.SetActive(false);
                if (keyPromptUI)
                    keyPromptUI.SetActive(false);

                return;
            }

            //if item isn't placed and player is holding the correct item, allow placement
            if (!isIn && SlotManager.instance.heldKey == requiredItem)
            {
                isIn = true;
                SlotManager.instance.ClearHeldItem();
                audioManager.PlaySFX(audioManager.correctItem);
                gemKey.SetActive(true);
                if (wallControl != null)
                {
                    wallControl.OpenWall();
                }

                if (promptUI)
                    promptUI.SetActive(false);
                if (keyPromptUI)
                    keyPromptUI.SetActive(false);
            }
            else if (!isIn)
            {
                audioManager.PlaySFX(audioManager.incorrectItem);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;

            if (isIn)
            {
                if (promptUI)
                    promptUI.SetActive(false);
                if (keyPromptUI)
                    keyPromptUI.SetActive(false);
                return;
            }

            if (SlotManager.instance.heldKey == requiredItem) { 
            if (promptUI)
                promptUI.SetActive(true);
            } else
            {
                if (keyPromptUI)
                    keyPromptUI.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            if (promptUI)
                promptUI.SetActive(false);

            if (keyPromptUI)
                keyPromptUI.SetActive(false);
        }
    }

}