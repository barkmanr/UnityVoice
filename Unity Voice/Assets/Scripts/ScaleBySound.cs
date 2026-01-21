using UnityEngine;
//https://www.youtube.com/watch?v=dzD0qP8viLw
public class ScaleBySound : MonoBehaviour
{
    public AudioSource audioSource;
    public Vector3 minScale;
    public Vector3 maxScale;

    public AudioDetection AD;

    public float loudnessSensibility = 100;
    public float threshold =0.1f;

    // Update is called once per frame
    void Update()
    {
        Loudness();
    }

    private void Loudness()
    {
        if(audioSource.clip == null) { return; }

        float loudness = AD.GetLoudnessFromAudioClip(audioSource.timeSamples, audioSource.clip) * loudnessSensibility;

        if (loudness < threshold)
        {
            loudness = 0;
        }

        transform.localScale = Vector3.Lerp(minScale, maxScale, loudness);
    }
}
