using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class platform : MonoBehaviour
{
    [SerializeField] Renderer model;
    [SerializeField] float duration;

    bool isBreaking;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isBreaking)
        {
            StartCoroutine(Breaking());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.audBreak[Random.Range(0, AudioManager.Instance.audBreak.Length)]);
            isBreaking = true;
        }
    }
    IEnumerator Breaking()
    {

        model.material.color = Color.red;
        yield return new WaitForSeconds(duration);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.audGone[Random.Range(0, AudioManager.Instance.audGone.Length)]);
        Destroy(gameObject);
    }
}
