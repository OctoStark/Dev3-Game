using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
   public void resume()
    {
        gameManager.instance.stateUnpause();
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.instance.stateUnpause();
    }

    public void quit()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void respawn()
    {
        gameManager.instance.playerScript.spawnPlayer();
        gameManager.instance.stateUnpause();
    }

}
