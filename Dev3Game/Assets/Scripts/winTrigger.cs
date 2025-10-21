using UnityEngine;

public class winTrigger : MonoBehaviour
{
    public LevelLoader levelLoader;
    [SerializeField] bool isFinalLevel;
    bool finished = false;
    private void Awake()
    {
    levelLoader = GetComponent<LevelLoader>();        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (finished == true)
        {
            if (isFinalLevel)
            {
                gameManager.instance.youWin(true);
            }
            else
            {
                gameManager.instance.LoadNextLevel();        
            }
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            finished = true;
        }

    }
}
