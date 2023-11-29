using GalgameNovelScript;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

namespace AVGEngine
{
    public class AVGEngine : MonoBehaviour
    {
        public Canvas canvas;
        // Start is called before the first frame update
        void Start()
        {
        var text = File.ReadAllText(Application.dataPath + "/主剧本.txt") + "\n";
        var lexer = new Lexer(text);
        var parser = new Parser(lexer);
        var tree = parser.Parse();

        var interpreter = new Interpreter(tree);
        interpreter.AddToGlobalScope("输出", new Action<object>(Debug.Log));
        interpreter.Interpret();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}