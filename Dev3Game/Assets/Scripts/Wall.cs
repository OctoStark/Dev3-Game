using UnityEngine;

public class Wall : MonoBehaviour
{
    public Animator wallAnim;
    public AudioManager audioManager;

    public void OpenWall()
    {
        if(wallAnim != null)
        {
            if (audioManager != null && audioManager.WallMove != null)
            {
                audioManager.PlaySFX(audioManager.WallMove);
            }
            wallAnim.SetTrigger("Open");
        }
    }

    public void CloseWall()
    {
        if (audioManager != null && audioManager.WallMove != null)
        {
            audioManager.PlaySFX(audioManager.WallMove);
        }
        wallAnim.SetTrigger("Close");
    }
}
