using UnityEngine;

public class ColorChange : MonoBehaviour
{
    public Color[] colors;
    private int currColor = 0;
    private Renderer testRenderer;
    private bool playerNearby;

    private void Start()
    {
        testRenderer = GetComponent<Renderer>();
        if (colors.Length > 0)
        {
            testRenderer.material.color = colors[currColor];
        }

    }

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            ChangeColor();
        }
    }

    public void ChangeColor()
    {
        currColor = (currColor + 1) % colors.Length;
        testRenderer.material.color = colors[currColor];
    }

    public void ResetColor()
    {
        currColor = 0;
        if (colors.Length > 0)
        {
            testRenderer.material.color = colors[currColor];
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }

}
