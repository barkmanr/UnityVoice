using System;
using UnityEngine;
using UnityEngine.Windows.Speech;

//https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Windows.Speech.KeywordRecognizer.html
//https://www.youtube.com/watch?v=29vyEOgsW8s
public class KeyRec : MonoBehaviour
{
    private KeywordRecognizer recognizer;
    public string[] keywords = { "hello", "start", "jump", "fire", "point", "face", "pizza", "shit", "front" };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        recognizer = new KeywordRecognizer(keywords,ConfidenceLevel.Low);
        recognizer.OnPhraseRecognized += OnPhraseRecognized;
        recognizer.Start();
    }
    //h
    void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Heard word: " + args.text);
        if (args.text == "start")
        {
            // Do your logic here
        }
    }

    void OnDestroy()
    {
        recognizer?.Stop();
        recognizer?.Dispose();
    }
}
