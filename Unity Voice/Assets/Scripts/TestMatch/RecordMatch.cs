using UnityEngine;
using UnityEngine.Audio;

public class RecordMatch : MonoBehaviour
{
    public AudioClip recClip;
    [SerializeField] AudioSource audioSource;
    private string device;
    int sampleRate = 44100;
    int maxSec = 1000;

    private float time = 0.0f; //need to know when to trim
    private bool isRecord = false;
    public void StartRecording()
    {
        if (Microphone.devices.Length <= 0) { Debug.Log("No Mike"); return; }

        device = Microphone.devices[0];


        time = 0.0f;
        isRecord = true;
        recClip = Microphone.Start(device, false, maxSec, sampleRate);
        //Device, IsLoop,MaxSecs,samplerate
    }

    private void Update()
    {
        if (isRecord)
        {
            time += Time.deltaTime;
        }
    }

    public void StopRecording() //compare audio (stole)
    {
        int samplesRecorded = Microphone.GetPosition(device);
        isRecord = false;
        Microphone.End(null);

        float[] data = new float[samplesRecorded * recClip.channels];
        recClip.GetData(data, 0);

        AudioClip trimmedClip = AudioClip.Create(
            "TrimmedRecording",
            samplesRecorded,
            recClip.channels,
            sampleRate,
            false
        );

        trimmedClip.SetData(data, 0);


        StartCoroutine(GetComponent<NewMFCC>().MatchAudio(trimmedClip));
    }
}
