using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySoundOnCollide : MonoBehaviour
{

    public AudioClip[] clips;

    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySound()
    {
        int index = Random.Range(0, clips.Length - 1);
        Debug.Log("Playing sound " + index);
        audioSource.resource = clips[index];
        audioSource.Play();
    }
    
    private void OnCollisionEnter(Collision other) {
        PlaySound();
    }

}
