using GalgameNovelScript;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using TMPro;
using UnityEngine;

namespace AVGEngine
{
    public class AVGEngine : MonoBehaviour
    {
        private static AVGEngine _instance;
        private Queue<Action> _mainThreadActions = new();

        public static AVGEngine Instance => _instance;
        private Thread _scriptThread;
        public static void RunInMainThread(Action action)
        {
            Instance._mainThreadActions.Enqueue(action);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < _mainThreadActions.Count; i++)
            {
                _mainThreadActions.Dequeue().Invoke();
            }
        }

        void Awake()
        {
            _instance = this;
            // 新建一个线程来运行脚本
            var text = Resources.Load<TextAsset>(AVGSettings.MainScriptPath).text + "\n";
            _scriptThread = new Thread(() => Run(text));
            _scriptThread.Start();
        }
        void Run(string text)
        {
            var lexer = new Lexer(text);
            var parser = new Parser(lexer);
            var tree = parser.Parse();

            var interpreter = new Interpreter(tree);
            StdLib.AddToEnv(interpreter);
            interpreter.Interpret();
        }
    }
}