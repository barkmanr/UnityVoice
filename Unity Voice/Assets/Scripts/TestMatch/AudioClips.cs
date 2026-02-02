using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class AudioClips : MonoBehaviour
{
    [SerializeField] List<AudioClip> Clips = new List<AudioClip>();
    [SerializeField] AudioSource audioSource;

    private void Start()
    {
        //StartCoroutine(PlayAll());
    }

    private IEnumerator PlayAll()
    {
        for (int i = 0; i < Clips.Count; i++)
        {
            audioSource.clip = Clips[i];
            float length = audioSource.clip.length;
            audioSource.Play();
            yield return new WaitForSeconds(length);
        }
    }
}
