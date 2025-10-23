using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class platform : MonoBehaviour
{
    [SerializeField] Renderer model;
    [SerializeField] float duration;

    public AudioManager audioManager;
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] audBreak;
    [Range(0, 1)][SerializeField] float audBreakVol;
    [SerializeField] AudioClip[] audGone;
    [Range(0, 1)][SerializeField] float audGoneVol;

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
            aud.PlayOneShot(audBreak[Random.Range(0, audBreak.Length)], audBreakVol);
            isBreaking = true;
        }
    }
    IEnumerator Breaking()
    {

        model.material.color = Color.red;
        yield return new WaitForSeconds(duration);
        aud.PlayOneShot(audGone[Random.Range(0, audGone.Length)], audGoneVol);
        Destroy(gameObject);
    }
}
