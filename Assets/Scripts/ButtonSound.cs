using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound; // Звук клика
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clickSound;
        audioSource.playOnAwake = false; // Не проигрывать звук сразу
    }

    public void PlayClickSound()
    {
        audioSource.PlayOneShot(clickSound); // Проигрываем звук клика
    }
}