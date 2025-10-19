using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] Color origColor;
    private MeshRenderer pressRenderer;
    public AudioManager audioManager;
    private Animator pressureAnim;
    public Wall wallControl;

    private bool playSound = false;
    private bool isDown = false;

    private void Start()
    {
        pressureAnim = GetComponentInChildren<Animator>();
        pressRenderer = GetComponentInChildren<MeshRenderer>();
        if (pressRenderer != null)
        {
            pressRenderer.material.color = origColor;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Moving Object") || other.CompareTag("Player"))
        {
            float distance = Vector3.Distance(transform.position, other.transform.position);

            if (distance > 0.05f)
            {
                Rigidbody box = other.GetComponent<Rigidbody>();
            }

            if (!isDown)
            {
                pressureAnim.SetTrigger("PressDown");
                isDown = true;
                if (wallControl != null)
                {
                    wallControl.OpenWall();
                }
            }

            if (pressRenderer != null)
            {
                pressRenderer.material.color = Color.blue;
            }

            if (!playSound && audioManager != null && audioManager.pressureOn != null)
            {
                audioManager.PlaySFX(audioManager.pressureOn);
                playSound = true;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Moving Object") || other.CompareTag("Player"))
        {
            if (pressRenderer != null)
            {
                pressRenderer.material.color = origColor;
            }

            if (pressureAnim != null)
            {
                pressureAnim.SetTrigger("PressUp");
            }

            if (wallControl != null)
            {
                wallControl.CloseWall();
            }

            playSound = false;
            isDown = false;
        }
    }
}
