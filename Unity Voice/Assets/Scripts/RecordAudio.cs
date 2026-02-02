using UnityEngine;
//https://www.youtube.com/watch?v=faLbKJ_AufU
public class RecordAudio : MonoBehaviour //ui buttons to do the 3 rec stuff
{
    public AudioClip recClip;
    [SerializeField] AudioSource audioSource;

    public void StartRecording()
    {
        if(Microphone.devices.Length <= 0) { Debug.Log("No Mike"); return; }

        string device = Microphone.devices[0]; //first micro
        int sampleRate = 44100;
        int MaxSec = 1000; //at most in sec

        recClip = Microphone.Start(device,false,MaxSec,sampleRate);
        //Device, IsLoop,MaxSecs,samplerate
    }

    public void PlayRecording()
    {
        if (recClip == null) { Debug.Log("Empty Rec"); return; }
        audioSource.clip = recClip;
        audioSource.Play();
    }

    public void StopRecording()
    {
        Microphone.End(null);
    }
}
