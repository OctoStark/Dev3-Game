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
