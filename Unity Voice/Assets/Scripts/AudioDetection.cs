using UnityEngine;
//https://www.youtube.com/watch?v=dzD0qP8viLw

public class AudioDetection : MonoBehaviour
{
    public int sampleWindow = 64;
    private AudioClip microphoneClip;

    private void Start()
    {
        MicrophoneToAudioClip();
    }
    public void MicrophoneToAudioClip()
    {
        if(Microphone.devices.Length == 0) { Debug.Log("No Mike2");  return; }
        string microphoneName = Microphone.devices[0];
        microphoneClip = Microphone.Start(microphoneName,true,20,AudioSettings.outputSampleRate);
    }

    public float GetLoudnessFromMicrophone()
    {
        if (Microphone.devices.Length == 0) { Debug.Log("No Mike3"); return 0; }
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]),microphoneClip);
    }

    public float GetLoudnessFromAudioClip(int _clipPosition, AudioClip _clip)
    {
        int startPosition = _clipPosition - sampleWindow;

        if (startPosition < 0) { return 0; }

        float[] waveData = new float[sampleWindow];
        _clip.GetData(waveData, startPosition);

        //compute loudness
        float totalLoudness = 0;

        for (int i = 0; i < sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }
        return totalLoudness/sampleWindow;
    }
}
