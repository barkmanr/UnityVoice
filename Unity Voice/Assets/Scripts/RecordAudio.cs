using UnityEngine;
//https://www.youtube.com/watch?v=faLbKJ_AufU
public class RecordAudio : MonoBehaviour
{
    private AudioClip recClip;
    [SerializeField] AudioSource audioSource;

    public void StartRecording()
    {
        if(Microphone.devices.Length <= 0) { Debug.Log("No Mike"); return; }
        string device = Microphone.devices[0];
        int sampleRate = 44100;
        int lengthSec = 1000; //at most in sec

        recClip = Microphone.Start(device,false,lengthSec,sampleRate);
    }

    public void PlayRecording()
    {
        if (recClip == null) { Debug.Log("Empty Rec"); }
        audioSource.clip = recClip;
        audioSource.Play();
    }

    public void StopRecording()
    {
        Microphone.End(null);
    }
}
