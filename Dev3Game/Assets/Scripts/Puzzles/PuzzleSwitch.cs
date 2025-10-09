using UnityEngine;

public class PuzzleSwitch : MonoBehaviour
{
    public GameObject switchObject;
    public bool isActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            ActivateSwitch();
        }
    }


    void ActivateSwitch()
        {
            isActivated = true;
            if (switchObject != null)
            {
                switchObject.SetActive(true);
            }
            Debug.Log("Switch activated!");
        }
}
