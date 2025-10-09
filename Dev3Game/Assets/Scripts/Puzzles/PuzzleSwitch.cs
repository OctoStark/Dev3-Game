using UnityEngine;

public class PuzzleSwitch : MonoBehaviour
{
    public GameObject switchObject;
    public bool active;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !active)
        {
            ActivateSwitch();
        }
    }


    void ActivateSwitch()
        {
            active = true;
            if (switchObject)
            {
                switchObject.SetActive(true);
            }
            Debug.Log("Switch activated!");
        }
}
