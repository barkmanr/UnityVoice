using UnityEngine;
using UnityEngine.Audio;

public class RecordMatch : MonoBehaviour
{
    public AudioClip recClip;
    [SerializeField] AudioSource audioSource;
    public void StartRecording()
    {
        if (Microphone.devices.Length <= 0) { Debug.Log("No Mike"); return; }

        string device = Microphone.devices[0]; //first micro
        int sampleRate = 44100;
        int MaxSec = 1000; //at most in sec

        recClip = Microphone.Start(device, false, MaxSec, sampleRate);
        //Device, IsLoop,MaxSecs,samplerate
    }

    public void StopRecording() //compare audio
    {
        Microphone.End(null);
        StartCoroutine(GetComponent<AudioClips>().MatchAudio(recClip));
    }
}
