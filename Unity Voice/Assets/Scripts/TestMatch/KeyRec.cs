using System;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class KeyRec : MonoBehaviour
{
    private KeywordRecognizer recognizer;
    public string[] keywords = { "hello", "start", "jump", "fire" };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        recognizer = new KeywordRecognizer(keywords);
        recognizer.OnPhraseRecognized += OnPhraseRecognized;
        recognizer.Start();
    }

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
