#if UNITY_EDITOR

using System;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

//대딩때 한 쌩 하드코딩.. 극혐 수정할것
namespace CWJ.RuntimeDebugging
{
    [CustomEditor(typeof(InfoGatherer))]
    public class InfoGathererInspector : Editor
    {
        private InfoGatherer gatherer;

        private Text fps, fpsMin, fpsMax, fpsAvg, ramTotal, ramUsed, ramPercent, deviceModel, os, cpu,/* cpuCores, cpuFreq,*/gpu, /*gpuMem,*/ ram;

        private void OnEnable()
        {
            gatherer = target as InfoGatherer;

            if (gatherer.valueTextList == null || gatherer.valueTextList.Length != (int)InfoGatherer.InfoType.thisTypeCount)
            {
                gatherer.infoUpdateActionList = new List<Action>();

                gatherer.valueTextList = new Text[(int)InfoGatherer.InfoType.thisTypeCount];

                gatherer.labelTextList = new Text[(int)InfoGatherer.InfoType.thisTypeCount];
                Debug.LogError((int)InfoGatherer.InfoType.thisTypeCount + " " + gatherer.valueTextList.Length + " " + gatherer.labelTextList.Length);

                gatherer.dataAmount = 0;
                gatherer.totalFPS = 0;
            }
            int index = 0;
            fps = gatherer.valueTextList[index++];
            fpsAvg = gatherer.valueTextList[index++];
            fpsMin = gatherer.valueTextList[index++];
            fpsMax = gatherer.valueTextList[index++];
            ramTotal = gatherer.valueTextList[index++];
            ramUsed = gatherer.valueTextList[index++];
            ramPercent = gatherer.valueTextList[index++];

            deviceModel = gatherer.valueTextList[index++];
            os = gatherer.valueTextList[index++];
            cpu = gatherer.valueTextList[index++];
            //cpuCores = gatherer.valueTextList[index++];
            //cpuFreq = gatherer.valueTextList[index++];
            gpu = gatherer.valueTextList[index++];
            //gpuMem = gatherer.valueTextList[index++];
            ram = gatherer.valueTextList[index++];
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("UI Texts");

            EditorGUI.indentLevel++;

            fps = (Text)EditorGUILayout.ObjectField("FPS:", fps, typeof(Text), true);
            //if (fps)
            //{
            //    EditorGUI.indentLevel++;
            //    gatherer.fpsWarning = EditorGUILayout.Toggle("Low FPS Warning:", gatherer.fpsWarning);
            //    if (gatherer.fpsWarning)
            //    {
            //        EditorGUI.indentLevel++;
            //        gatherer.fpsWarningLimit = EditorGUILayout.IntField(new GUIContent("Warning Limit:", "For values lower than this, the color of the FPS text will be the color you set in the next field."), gatherer.fpsWarningLimit);
            //        gatherer.fpsWarningColor = EditorGUILayout.ColorField("Warning Color:", gatherer.fpsWarningColor);
            //        EditorGUI.indentLevel--;
            //    }
            //    EditorGUI.indentLevel--;
            //}
            fpsAvg = (Text)EditorGUILayout.ObjectField("FPS Average:", fpsAvg, typeof(Text), true);
            fpsMin = (Text)EditorGUILayout.ObjectField("FPS Minimum:", fpsMin, typeof(Text), true);
            fpsMax = (Text)EditorGUILayout.ObjectField("FPS Maximum:", fpsMax, typeof(Text), true);
            ramTotal = (Text)EditorGUILayout.ObjectField("RAM Total:", ramTotal, typeof(Text), true);
            ramUsed = (Text)EditorGUILayout.ObjectField("RAM Used:", ramUsed, typeof(Text), true);
            ramPercent = (Text)EditorGUILayout.ObjectField("RAM Percentage:", ramPercent, typeof(Text), true);
            deviceModel = (Text)EditorGUILayout.ObjectField("Device Model:", deviceModel, typeof(Text), true);
            os = (Text)EditorGUILayout.ObjectField("Operating System:", os, typeof(Text), true);
            cpu = (Text)EditorGUILayout.ObjectField("CPU:", cpu, typeof(Text), true);
            //cpuCores = (Text)EditorGUILayout.ObjectField("CPU Cores:", cpuCores, typeof(Text), true);
            //cpuFreq = (Text)EditorGUILayout.ObjectField("CPU Frequency:", cpuFreq, typeof(Text), true);
            gpu = (Text)EditorGUILayout.ObjectField("GPU:", gpu, typeof(Text), true);
            //gpuMem = (Text)EditorGUILayout.ObjectField("GPU Memory:", gpuMem, typeof(Text), true);
            ram = (Text)EditorGUILayout.ObjectField("RAM:", ram, typeof(Text), true);

            EditorGUI.indentLevel--;

            EditorGUILayout.Space();

            if (GUILayout.Button("Show"))
            {
                gatherer.Show();
            }

            if (GUILayout.Button("Hide"))
            {
                gatherer.Hide();
            }
            int index = 0;
            gatherer.valueTextList[index++] = fps;
            gatherer.valueTextList[index++] = fpsAvg;
            gatherer.valueTextList[index++] = fpsMin;
            gatherer.valueTextList[index++] = fpsMax;
            gatherer.valueTextList[index++] = ramTotal;
            gatherer.valueTextList[index++] = ramUsed;
            gatherer.valueTextList[index++] = ramPercent;
            //
            gatherer.valueTextList[index++] = deviceModel;
            gatherer.valueTextList[index++] = os;
            gatherer.valueTextList[index++] = cpu;
            //gatherer.valueTextList[index++] = cpuCores;
            //gatherer.valueTextList[index++] = cpuFreq;
            gatherer.valueTextList[index++] = gpu;
            //gatherer.valueTextList[index++] = gpuMem;
            gatherer.valueTextList[index++] = ram;

            for (int i = 0; i < gatherer.valueTextList.Length; i++)
            {
                gatherer.labelTextList[i] = gatherer.valueTextList[i].transform.parent.GetComponent<Text>();
            }

            EditorUtility.SetDirty(target);
        }
    }
}

#endif