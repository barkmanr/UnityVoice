using System.Collections.Generic;
using UnityEngine;

public class AudioClip3 : MonoBehaviour
{
    [SerializeField] List<AudioClip> Clips = new List<AudioClip>();
    public List<float[]> ClipsData = new();
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource SilentSource; //Used to get data of files
    public float[] NewClipData;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
