using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioClips : MonoBehaviour
{
    [SerializeField] List<AudioClip> Clips = new List<AudioClip>();
    public List<float[]> ClipsData = new();
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource SilentSource; //Used to get data of files
    public float[] NewClipData;

    private void Start()
    {
        StartCoroutine(GetDataOfAll());
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

    private IEnumerator GetDataOfAll()
    {
        foreach (AudioClip clip in Clips) //get datum
        {
            float[] spectrum = new float[512]; //size of float?
            float[] accumulator = new float[512];

            SilentSource.clip = clip;
            SilentSource.volume = 0.0f;
            SilentSource.Play();

            yield return null; //wait a frame

            while (SilentSource.isPlaying)
            {
                SilentSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

                for (int i = 0; i < spectrum.Length; i++)
                {
                    accumulator[i] += spectrum[i];
                }

                yield return null; //wait a frame
            }

            for (int i = 0; i < accumulator.Length; i++)
            {
                accumulator[i] /= Mathf.Max(1, clip.length * 60f); // approx frames
            }

            ClipsData.Add(accumulator);
        }
        Debug.Log("Done");
    }

    public IEnumerator MatchAudio(AudioClip audioClip) //get index or -1
    {
        float[] spectrum = new float[512]; //size of float?
        float[] accumulator = new float[512];

        SilentSource.clip = audioClip;
        SilentSource.volume = 0.0f;
        SilentSource.Play();

        yield return null; //wait a frame

        while (SilentSource.isPlaying)
        {
            SilentSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

            for (int i = 0; i < spectrum.Length; i++)
            {
                accumulator[i] += spectrum[i];
            }

            yield return null; //wait a frame
        }

        for (int i = 0; i < accumulator.Length; i++)
        {
            accumulator[i] /= Mathf.Max(1, audioClip.length * 60f); // approx frames
        }

        NewClipData = accumulator;
        Debug.Log(audioClip.length);
    }

}
