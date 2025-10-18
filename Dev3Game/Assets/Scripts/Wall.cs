using UnityEngine;

public class Wall : MonoBehaviour
{
    public Animator wallAnim;

    public void OpenWall()
    {
        if(wallAnim != null)
        {
            wallAnim.SetTrigger("Open");
        }
    }

    public void CloseWall()
    {
        wallAnim.SetTrigger("Close");
    }
}
