using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

//https://www.youtube.com/watch?v=29vyEOgsW8s
public class VoiceTemp : MonoBehaviour
{
    [SerializeField] private GameObject Cube;
    private KeywordRecognizer recognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    void Start()
    {
        actions.Add("fuck", Forward);
        actions.Add("this", Down);
        actions.Add("shit", Up);
        actions.Add("gnome", Back);



        recognizer = new KeywordRecognizer(actions.Keys.ToArray(),ConfidenceLevel.Low);
        recognizer.OnPhraseRecognized += OnPhraseRecognized;
        recognizer.Start();
    }

    void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        actions[args.text].Invoke();
    }

    public void Forward()
    {
        Cube.transform.position += new Vector3(1, 0, 0);
    }

    public void Down()
    {
        Cube.transform.position += new Vector3(0, -1, 0);
    }

    public void Up() 
    {
        Cube.transform.position += new Vector3(0, 1, 0);
    }
    public void Back() 
    {
        Cube.transform.position += new Vector3(-1, 0, 0);
    }

    void OnDestroy()
    {
        recognizer?.Stop();
        recognizer?.Dispose();
    }
}
