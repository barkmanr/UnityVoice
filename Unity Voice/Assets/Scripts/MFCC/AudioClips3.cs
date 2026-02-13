using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
//https://docs.unity3d.com/6000.3/Documentation/ScriptReference/AudioSource.GetSpectrumData.html
public class AudioClips3 : MonoBehaviour
{ //compare 3 clips
    [SerializeField] AudioClip clip1;
    [SerializeField] AudioClip clip2;
    [SerializeField] AudioClip clip3;
    [SerializeField] AudioSource audioSource1;
    [SerializeField] AudioSource audioSource2;
    [SerializeField] AudioSource audioSource3;
    [SerializeField] AudioClip compareClip;
    [SerializeField] AudioSource audioSource4;

    private List<float[]> Amplitude = new();
    private float[] compareAmp;

    private List<float[]> Spectrums1 = new();
    private List<float[]> Spectrums2 = new();
    private List<float[]> Spectrums3 = new();
    private List<float[]> Spectrums4 = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Amplitudes();
        StartPlaying();
    }

    private void StartPlaying()
    {
        audioSource1.clip = clip1;
        audioSource1.Play();

        audioSource2.clip = clip2;
        audioSource2.Play();

        audioSource3.clip = clip3;
        audioSource3.Play();

        audioSource4.clip = compareClip;
        audioSource4.Play();

    }

    public void Amplitudes() //values between -1 and 1 to show loadness?
    {
        float[] samples1 = new float[clip1.samples * clip1.channels];
        clip1.GetData(samples1, 0);
        Amplitude.Add(samples1);
        
        float[] samples2 = new float[clip2.samples * clip2.channels];
        clip2.GetData(samples2, 0);
        Amplitude.Add(samples2);

        float[] samples3 = new float[clip3.samples * clip3.channels];
        clip3.GetData(samples3, 0);
        Amplitude.Add(samples3);

        float[] samples4 = new float[compareClip.samples * compareClip.channels];
        compareClip.GetData(samples4, 0);
        compareAmp = samples4;
    }

    public void SpectrumsGets()
    {
        //Fast Fourier Transform
        if (audioSource1.isPlaying)
        {
            float[] spectrum1 = new float[256]; //power of 2
            audioSource1.GetSpectrumData(spectrum1, 0, FFTWindow.BlackmanHarris);
            Spectrums1.Add(spectrum1);


            // Loop through the populated array
            // Start the loop from 1 and to 1 less than the length, so the loop can draw lines between adjacent bins. 

            for (int i = 1; i < spectrum1.Length - 1; i++)
            {
                Debug.DrawLine(new Vector3(i - 1, spectrum1[i] + 10, 0), new Vector3(i, spectrum1[i + 1] + 10, 0), Color.red);
                Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum1[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrum1[i]) + 10, 2), Color.cyan);
                Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum1[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrum1[i] - 10, 1), Color.green);
                Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum1[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrum1[i]), 3), Color.blue);
            }
        }
        if (audioSource2.isPlaying)
        {
            float[] spectrum2 = new float[256]; //power of 2
            audioSource2.GetSpectrumData(spectrum2, 0, FFTWindow.Rectangular);
            Spectrums2.Add(spectrum2);
        }
        if (audioSource3.isPlaying)
        {
            float[] spectrum3 = new float[256]; //power of 2
            audioSource3.GetSpectrumData(spectrum3, 0, FFTWindow.Rectangular);
            Spectrums3.Add(spectrum3);
        }
        if (audioSource4.isPlaying)
        {
            float[] spectrum4 = new float[256]; //power of 2
            audioSource4.GetSpectrumData(spectrum4, 0, FFTWindow.Rectangular);
            Spectrums4.Add(spectrum4);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SpectrumsGets();
    }
}
