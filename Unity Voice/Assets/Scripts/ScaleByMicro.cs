using UnityEngine;

public class ScaleByMicro : MonoBehaviour
{
    public Vector3 minScale;
    public Vector3 maxScale;

    public AudioDetection AD;

    public float loudnessSensibility = 100;
    public float threshold = 0.1f;

    // Update is called once per frame
    void Update()
    {
        Loudness();
    }

    private void Loudness()
    {

        float loudness = AD.GetLoudnessFromMicrophone() * loudnessSensibility;

        if (loudness < threshold)
        {
            loudness = 0;
        }

        transform.localScale = Vector3.Lerp(minScale, maxScale, loudness);
    }
}
