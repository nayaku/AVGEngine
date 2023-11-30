using GalgameNovelScript;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

namespace AVGEngine
{
    public class StdLib
    {
        public static void AddToEnv(Interpreter interpreter)
        {
            interpreter.AddToGlobalScope("Êä³ö", new Action<object>(Log));
            interpreter.AddToGlobalScope("µÈ´ý", new Action<float>(Wait));
            interpreter.AddToGlobalScope("Êä³ö", new Action<string>(PrintFunc.Print));
        }
        public static void Log(object obj)
        {
            if (AVGSettings.IsDebug)
            {
                Debug.Log(obj);
            }
        }
        public static void Wait(float seconds)
        {
            Thread.Sleep((int)(seconds * 1000));
        }
    }

    public class PrintFunc
    {
        private static AutoResetEvent _waitEvent = new(false);
        public static void Print(object text)
        {
            _waitEvent.Reset();
            AVGEngine.RunInMainThread(() =>
            {
                var textMesh = GameObject.Find("DialogText").GetComponent<TextMeshProUGUI>();
                Debug.Assert(textMesh != null);
                AVGEngine.Instance.StartCoroutine(_Print(text.ToString(), textMesh));
            });
            _waitEvent.WaitOne();
        }
        private static IEnumerator _Print(string text, TextMeshProUGUI textMesh)
        {
            Debug.Log("Print: " + text);
            textMesh.text = "";
            for (int i = 0; i < text.Length; i++)
            {
                textMesh.text += text[i];
                yield return new WaitUntilForSeconds(
                    () => Input.GetMouseButtonDown(0),
                    AVGSettings.TextSpeed);
                if (Input.GetMouseButtonDown(0))
                {
                    textMesh.text = text;
                    yield return null;
                    break;
                }
            }
            yield return new WaitForMouseDown();
            Debug.Log("Print: " + text + " Done");
            _waitEvent.Set();
        }
    }
}
