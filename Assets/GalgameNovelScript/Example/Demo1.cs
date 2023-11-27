using GalgameNovelScript;
using System;
using System.IO;
using UnityEngine;

public class Demo1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var text = File.ReadAllText(Application.dataPath + "/GalgameNovelScript/Example/Demo1.txt") + "\n";
        var lexer = new Lexer(text);
        var parser = new Parser(lexer);
        var tree = parser.Parse();

        var interpreter = new Interpreter(tree);
        interpreter.AddToGlobalScope("Êä³ö", new Action<object>(Debug.Log));
        interpreter.Interpret();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
