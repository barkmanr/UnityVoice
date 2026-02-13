using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
//https://www.w3.org/TR/speech-grammar/
//example.grxml
public class GrammarRec : MonoBehaviour
{
    private GrammarRecognizer recognizer;

    void Start()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "example.grxml");
        if (File.Exists(filePath)) //takes a minute
        {
            recognizer = new GrammarRecognizer(filePath,ConfidenceLevel.Low);
            recognizer.OnPhraseRecognized += OnPhraseRecognized;
            recognizer.Start();
            Debug.Log("Grammar Recognizer active!");
        }
        else
        {
            Debug.LogError("Grammar file missing at: " + filePath);
        }

        foreach (var device in Microphone.devices)
        {
            Debug.Log("Microphone detected: " + device);
        }
    }

    void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Heard Item");
    }
    void OnDestroy()
    {
        recognizer?.Stop();
        recognizer?.Dispose();
    }
}
