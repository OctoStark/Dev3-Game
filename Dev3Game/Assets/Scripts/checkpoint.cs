using System.Collections;
using UnityEngine;

public class checkpoint : MonoBehaviour
{
    [SerializeField] Renderer model;

    Color colorOrig;

    public void Start()
    {
        colorOrig = model.material.color;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameManager.instance.playerSpawnPos.transform.position !=  transform.position)
        {
            gameManager.instance.playerSpawnPos.transform.position = transform.position;
            StartCoroutine(checkpointFeedback());
        }
    }

    IEnumerator checkpointFeedback()
    {
        gameManager.instance.checkpointPopup.SetActive(true);
        model.material.color = Color.blue;
        yield return new WaitForSeconds(.5f);
        model.material.color = colorOrig;
        gameManager.instance.checkpointPopup.SetActive(false);
    }
}
