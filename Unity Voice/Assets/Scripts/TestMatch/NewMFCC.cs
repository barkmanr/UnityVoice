using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMFCC : MonoBehaviour
{
    [Header("Voice Clips Database")]
    [SerializeField] private List<AudioClip> Clips = new List<AudioClip>();
    public List<float[]> ClipsData = new();

    [Header("Audio Sources")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource SilentSource; // used only for analysis

    [Header("Settings")]
    [SerializeField] private int spectrumSize = 256;
    [SerializeField] private int melBands = 26;

    public float[] NewClipData;

    private void Start()
    {
        StartCoroutine(GetDataOfAll());
    }

    // -----------------------------
    // FEATURE EXTRACTION
    // -----------------------------

    private IEnumerator GetDataOfAll()
    {
        ClipsData.Clear();

        foreach (AudioClip clip in Clips)
        {
            float[] result = null;

            yield return StartCoroutine(
                ExtractVoiceFeatures(clip, data => result = data)
            );

            ClipsData.Add(result);
        }

        Debug.Log("Voice database ready");
    }

    public IEnumerator MatchAudio(AudioClip audioClip)
    {
        float[] result = null;

        yield return StartCoroutine(
            ExtractVoiceFeatures(audioClip, data => result = data)
        );

        NewClipData = result;

        int match = FindBestMatch(NewClipData);
        Debug.Log("Best match index: " + match);
    }

    private IEnumerator ExtractVoiceFeatures(AudioClip clip, System.Action<float[]> onComplete)
    {
        float[] spectrum = new float[spectrumSize];

        // 3 temporal segments
        float[][] segments = new float[3][];
        for (int s = 0; s < 3; s++)
        {
            segments[s] = new float[melBands];
            for (int i = 0; i < melBands; i++)
                segments[s][i] = -100f; // IMPORTANT
        }

        SilentSource.clip = clip;
        SilentSource.volume = 0f;
        SilentSource.Play();

        yield return null;

        float clipLength = clip.length;

        while (SilentSource.isPlaying)
        {
            SilentSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

            if (IsSilent(spectrum))
            {
                yield return null;
                continue;
            }

            // Which third of the clip are we in?
            float t = SilentSource.time / clipLength;
            int segment = Mathf.Clamp((int)(t * 3f), 0, 2);

            float[] mel = SpectrumToMel(spectrum);

            // Max-pool per segment
            for (int i = 0; i < melBands; i++)
                segments[segment][i] = Mathf.Max(segments[segment][i], mel[i]);

            yield return null;
        }

        // Flatten segments into one feature vector
        float[] features = new float[melBands * 3];
        int idx = 0;

        for (int s = 0; s < 3; s++)
        {
            Normalize(segments[s]);

            for (int i = 0; i < melBands; i++)
                features[idx++] = segments[s][i];
        }

        onComplete?.Invoke(features);
    }


    // -----------------------------
    // MEL FEATURES
    // -----------------------------

    private float[] SpectrumToMel(float[] spectrum)
    {
        float[] mel = new float[melBands];
        int binsPerBand = spectrum.Length / melBands;

        for (int band = 0; band < melBands; band++)
        {
            float sum = 0f;
            int start = band * binsPerBand;
            int end = start + binsPerBand;

            for (int i = start; i < end; i++)
                sum += spectrum[i];

            mel[band] = Mathf.Log10(sum + 1e-6f);
        }

        return mel;
    }

    private void Normalize(float[] data)
    {
        float norm = 0f;
        for (int i = 0; i < data.Length; i++)
            norm += data[i] * data[i];

        norm = Mathf.Sqrt(norm) + 1e-6f;

        for (int i = 0; i < data.Length; i++)
            data[i] /= norm;
    }

    // -----------------------------
    // MATCHING
    // -----------------------------

    private int FindBestMatch(float[] newData)
    {
        float bestScore = -1f;
        int bestIndex = -1;

        for (int i = 0; i < ClipsData.Count; i++)
        {
            float score = CosineSimilarity(newData, ClipsData[i]);

            if (score > bestScore)
            {
                bestScore = score;
                bestIndex = i;
            }
            Debug.Log($"Clip {i} score: {score}");
        }

        Debug.Log("Best similarity score: " + bestScore);
        return bestIndex;
    }

    private float CosineSimilarity(float[] a, float[] b)
    {
        float dot = 0f, magA = 0f, magB = 0f;

        for (int i = 0; i < a.Length; i++)
        {
            dot += a[i] * b[i];
            magA += a[i] * a[i];
            magB += b[i] * b[i];
        }

        return dot / (Mathf.Sqrt(magA) * Mathf.Sqrt(magB) + 1e-6f);
    }

    bool IsSilent(float[] spectrum)
    {
        float rms = 0f;
        for (int i = 0; i < spectrum.Length; i++)
            rms += spectrum[i] * spectrum[i];

        rms = Mathf.Sqrt(rms / spectrum.Length);
        return rms < 0.0005f;
    }
}
