using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] Color origColor;
    private MeshRenderer pressRenderer;

    private void Start()
    {
        pressRenderer = GetComponentInChildren<MeshRenderer>();
        if (pressRenderer != null)
        {
            pressRenderer.material.color = origColor;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Moving Object") || other.CompareTag("Player"))
        {
            float distance = Vector3.Distance(transform.position, other.transform.position);
            Debug.Log("Distance: " + distance);

            if (distance > 0.05f)
            {
                Rigidbody box = other.GetComponent<Rigidbody>();
                if(box != null)
                {
                    box.isKinematic = true;
                }
            }

            if (pressRenderer != null)
            {
                pressRenderer.material.color = Color.blue;
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
        }
    }
}
