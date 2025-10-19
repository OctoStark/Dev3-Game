using UnityEngine;
using System.Collections.Generic;

public class SwitchManager : MonoBehaviour
{
    public List<ManualSwitch> switchOrder;
    private int currIndex = 0;
    public Wall wallControl;
    public AudioManager audioManager;

    public void SequenceSwitch(ManualSwitch manualSwitch)
    {
        if (switchOrder[currIndex] == manualSwitch)
        {
            manualSwitch.ActivateSwitch();
            currIndex++;

            if (currIndex >= switchOrder.Count)
            {
                Debug.Log("All switches activated in correct order!");
                if (audioManager != null && audioManager.correctItem != null)
                {
                    audioManager.PlaySFX(audioManager.correctItem);
                }

                if (wallControl != null)
                {
                    wallControl.OpenWall();
                }

            }
        }
        else
        {
            Debug.Log("Wrong switch! Resetting sequence.");

            ResetSequence();
        }

    }

    public void ResetSequence()
    {
        foreach (var sw in switchOrder)
        {
            sw.ResetSwitch(); // Reset visuals/state
        }

        currIndex = 0;

        if (audioManager != null && audioManager.incorrectItem != null)
        {
            audioManager.PlaySFX(audioManager.incorrectItem);
        }
        if (wallControl != null)
        {
            wallControl.CloseWall();
        }
    }


}
