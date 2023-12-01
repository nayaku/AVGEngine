using GalgameNovelScript;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

namespace AVGEngine
{
    public class StdLib
    {
        public static void AddToEnv(Interpreter interpreter)
        {
            interpreter.AddToGlobalScope("等待", new Action<float>(Wait));
            interpreter.AddToGlobalScope("输出", new Action<object>((object text) => DialogFunc.Dialog(text.ToString())));

            interpreter.Dialog = DialogFunc.Dialog;
            interpreter.ShowCaseTip = OptionFunc.ShowCaseTip;
            interpreter.ShowOptions = OptionFunc.ShowOptions;

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

    public class DialogFunc
    {
        private static AutoResetEvent _waitEvent = new(false);
        public static void Dialog(string text, string name = "")
        {
            _waitEvent.Reset();
            AVGEngine.RunInMainThread(() =>
            {
                var nameMesh = GameObject.Find("DialogNameText").GetComponent<TextMeshProUGUI>();
                nameMesh.text = name;
                var textMesh = GameObject.Find("DialogText").GetComponent<TextMeshProUGUI>();
                AVGEngine.Instance.StartCoroutine(_Dialog(text.ToString(), textMesh));
            });
            _waitEvent.WaitOne();
        }
        private static IEnumerator _Dialog(string text, TextMeshProUGUI textMesh)
        {
            textMesh.text = "";
            for (int i = 0; i < text.Length; i++)
            {
                textMesh.text += text[i];
                yield return new WaitUntilForSeconds(
                    () => InputControls.NextDialog(),
                    AVGSettings.TextSpeed);
                if (InputControls.NextDialog())
                {
                    textMesh.text = text;
                    yield return null;
                    break;
                }
            }
            yield return new WaitFor(InputControls.NextDialog);
            _waitEvent.Set();
        }
    }
    public class OptionFunc
    {
        private static AutoResetEvent _waitEvent = new(false);
        private static int _optionIndex = -1;
        public static void ShowCaseTip(string text)
        {
            _waitEvent.Reset();
            AVGEngine.RunInMainThread(() =>
            {
                var textMesh = GameObject.Find("DialogText").GetComponent<TextMeshProUGUI>();
                AVGEngine.Instance.StartCoroutine(_ShowCaseTip(text, textMesh));
            });
            _waitEvent.WaitOne();
        }
        private static IEnumerator _ShowCaseTip(string text, TextMeshProUGUI textMesh)
        {
            textMesh.text = "";
            for (int i = 0; i < text.Length; i++)
            {
                textMesh.text += text[i];
                yield return new WaitUntilForSeconds(
                    () => InputControls.NextDialog(),
                    AVGSettings.TextSpeed);
                if (InputControls.NextDialog())
                {
                    textMesh.text = text;
                    yield return null;
                    break;
                }
            }
            _waitEvent.Set();
        }
        public static int ShowOptions(List<string> options)
        {
            _waitEvent.Reset();
            // 显示选项
            AVGEngine.RunInMainThread(() =>
            {
                var optionPanel = GameObject.Find("OptionPanel");
                var optionButtons = Resources.Load<GameObject>("Prefabs/OptionButton");
                for (int i = 0; i < options.Count; i++)
                {
                    var option = options[i];
                    var button = UnityEngine.Object.Instantiate(optionButtons, optionPanel.transform);
                    button.GetComponentInChildren<TextMeshProUGUI>().text = option;
                    var index = i;
                    button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                    {
                        _optionIndex = index;
                        _waitEvent.Set();
                    });
                }
            });
            _waitEvent.WaitOne();
            // 清空选项
            AVGEngine.RunInMainThread(() =>
            {
                var optionPanel = GameObject.Find("OptionPanel");
                foreach (Transform child in optionPanel.transform)
                {
                    UnityEngine.Object.Destroy(child.gameObject);
                }
            });
            return _optionIndex;
        }
    }
}
