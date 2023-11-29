using GalgameNovelScript;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class AVGEngine : MonoBehaviour
{
    public TextMeshPro dialog;
    // Start is called before the first frame update
    void Start()
    {
        var text = File.ReadAllText(Application.dataPath + "/������.txt") + "\n";
        var lexer = new Lexer(text);
        var parser = new Parser(lexer);
        var tree = parser.Parse();

        var interpreter = new Interpreter(tree);
        interpreter.AddToGlobalScope("���", new Action<object>(Debug.Log));
        interpreter.Interpret();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
