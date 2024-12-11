using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Serialization;
using UnityEngine.UI;

//??이거 Action을 뭐 이따구로 써놨지 나중에 수정할것 많은 메소드등록, 호출에는 System.Action보다 UnityEvent가 나음
//PC에서는 정확히나오는거같은데 AOS에서는 틀리는경우가있는듯.
namespace CWJ.RuntimeDebugging
{
    [Serializable]
    public class InfoGatherer : MonoBehaviour
    {
        public Canvas rootCanvas;

        [HideInInspector]
        public bool showOnStart = false;

        [HideInInspector]
        public bool loadMinMaxAvgData;

        [HideInInspector]
        public int updateFrequency = 5;

        [HideInInspector]
        public int dataAmount, fpsWarningLimit;

        [HideInInspector]
        public int totalFPS;

        [HideInInspector]
        public bool fpsWarning;

        [HideInInspector]
        public Color fpsWarningColor, fpsInitialColor;

        [HideInInspector]
        public List<Action> infoUpdateActionList;

        [HideInInspector]
        public Text[] valueTextList;

        [HideInInspector]
        public Text[] labelTextList;

        public enum InfoType
        {
            FPS,
            FPSMin,
            FPSMax,
            FPSAvg,
            RamTotal,
            RamUsed,
            RamPercent,

            DeviceModel,
            OperatingSystem,
            CPU,

            //CPUCores,
            //CPUFrequency,
            GPU,

            //GPUMemory,

            RAM,

            thisTypeCount
        }

        [HideInInspector]
        public int fps, min, max, avg;

        [HideInInspector]
        public bool init;

        private float timer, period;

        private void Initialize()
        {
            init = true;
            if (loadMinMaxAvgData)
            {
                int temp = 0;

                temp = PlayerPrefs.GetInt("FPSMin", -9);
                if (temp == -9)
                {
                    min = 1000000; PlayerPrefs.SetInt("FPSMin", min);
                }
                else
                    min = temp;

                temp = PlayerPrefs.GetInt("FPSMax", -9);
                if (temp == -9)
                {
                    max = 0; PlayerPrefs.SetInt("FPSMax", max);
                }
                else
                    max = temp;

                temp = PlayerPrefs.GetInt("TotalFPS", -9);
                if (temp == -9)
                {
                    totalFPS = 0; dataAmount = 0; PlayerPrefs.SetInt("TotalFPS", 0); PlayerPrefs.SetInt("FPSDataAmount", (int)dataAmount);
                }
                else
                {
                    totalFPS = temp; dataAmount = PlayerPrefs.GetInt("FPSDataAmount");
                }
            }
            else
            {
                min = 1000000; max = 0; avg = 0; dataAmount = 0; totalFPS = 0;
            }

            infoUpdateActionList = new List<Action>();

            if (updateFrequency <= 0)
                updateFrequency = 10;

            period = 1f / updateFrequency;
            timer = 0;
            int index = 0;
            if (valueTextList[index++])
            {
                infoUpdateActionList.Add(UpdateFPS);
                fpsInitialColor = valueTextList[0].color;
            }
            if (valueTextList[index++])
            {
                if (!infoUpdateActionList.Contains(UpdateFPS))
                    infoUpdateActionList.Add(UpdateFPS);

                infoUpdateActionList.Add(UpdateFPSAvg);
            }
            if (valueTextList[index++])
            {
                if (!infoUpdateActionList.Contains(UpdateFPS))
                    infoUpdateActionList.Add(UpdateFPS);

                infoUpdateActionList.Add(UpdateFPSMin);
            }

            if (valueTextList[index++])
            {
                if (!infoUpdateActionList.Contains(UpdateFPS))
                    infoUpdateActionList.Add(UpdateFPS);

                infoUpdateActionList.Add(UpdateFPSMax);
            }

            if (valueTextList[index++])
            {
                infoUpdateActionList.Add(UpdateRamTotal);
            }
            if (valueTextList[index++])
            {
                infoUpdateActionList.Add(UpdateRamUsed);
            }
            if (valueTextList[index++])
            {
                infoUpdateActionList.Add(UpdateRamPercentage);
            }
            if (valueTextList[index++])
            {
                UpdateDeviceModel();
            }
            if (valueTextList[index++])
            {
                UpdateOS();
            }
            if (valueTextList[index++])
            {
                UpdateCPU();
            }

            //if (valueTextList[index++])
            //{
            //    UpdateCPUCores();
            //}

            //if (valueTextList[index++])
            //{
            //    UpdateCPUFreq();
            //}

            if (valueTextList[index++])
            {
                UpdateGPU();
            }

            //if (valueTextList[index++])
            //{
            //    UpdateGPUMem();
            //}

            if (valueTextList[index++])
            {
                UpdateRAM();
            }

            if (showOnStart)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        private void OnEnable()
        {
            rootCanvas.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            rootCanvas.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!init)
                Initialize();

            if (timer >= period)
            {
                timer -= period;

                for (int i = 0; i < infoUpdateActionList.Count; i++)
                {
                    infoUpdateActionList[i]();
                }
            }
            timer += Time.deltaTime;
        }

        public void ResetFPSMin()
        {
            min = fps;
        }

        public void ResetFPSMax()
        {
            max = fps;
        }

        public void ResetFPSAvg()
        {
            avg = fps;

            totalFPS = 0;

            dataAmount = 0;
        }

        public void Show()
        {
            rootCanvas.enabled = true;
        }

        public void Hide()
        {
            rootCanvas.enabled = false;
        }

        public void SetFPSWarning(int limit, Color color)
        {
            fpsWarningLimit = limit;
            fpsWarningColor = color;
            fpsWarning = true;
        }

        public bool GetEnabled(InfoType type)
        {
            switch (type)
            {
                case InfoType.FPS:
                    return infoUpdateActionList.Contains(UpdateFPS) && valueTextList[(int)type] != null;

                case InfoType.FPSMin:
                    return infoUpdateActionList.Contains(UpdateFPSMin) && valueTextList[(int)type] != null;

                case InfoType.FPSMax:
                    return infoUpdateActionList.Contains(UpdateFPSMax) && valueTextList[(int)type] != null;

                case InfoType.FPSAvg:
                    return infoUpdateActionList.Contains(UpdateFPSAvg) && valueTextList[(int)type] != null;

                case InfoType.RamTotal:
                    return infoUpdateActionList.Contains(UpdateRamTotal) && valueTextList[(int)type] != null;

                case InfoType.RamUsed:
                    return infoUpdateActionList.Contains(UpdateRamUsed) && valueTextList[(int)type] != null;

                case InfoType.RamPercent:
                    return infoUpdateActionList.Contains(UpdateRamPercentage) && valueTextList[(int)type] != null;

                case InfoType.OperatingSystem:
                    return valueTextList[(int)type] != null;

                case InfoType.CPU:
                    return valueTextList[(int)type] != null;

                //case InfoType.CPUCores:
                //    return valueTextList[(int)type] != null;

                //case InfoType.CPUFrequency:
                //    return valueTextList[(int)type] != null;

                case InfoType.GPU:
                    return valueTextList[(int)type] != null;

                //case InfoType.GPUMemory:
                //    return valueTextList[(int)type] != null;

                case InfoType.RAM:
                    return valueTextList[(int)type] != null;
            }

            return false;
        }

        public void SetEnabled(InfoType type, bool enabled, Text text = null)
        {
            switch (type)
            {
                case InfoType.FPS:
                    if (enabled) { EnableInfo(UpdateFPS, (int)type, text); fpsInitialColor = text == null ? valueTextList[0].color : text.color; } else DisableInfo(UpdateFPS);
                    break;

                case InfoType.FPSMin:
                    if (enabled) EnableInfo(UpdateFPSMin, (int)type, text); else DisableInfo(UpdateFPSMin);
                    break;

                case InfoType.FPSMax:
                    if (enabled) EnableInfo(UpdateFPSMax, (int)type, text); else DisableInfo(UpdateFPSMax);
                    break;

                case InfoType.FPSAvg:
                    if (enabled) EnableInfo(UpdateFPSAvg, (int)type, text); else DisableInfo(UpdateFPSAvg);
                    break;

                case InfoType.RamTotal:
                    if (enabled) EnableInfo(UpdateRamTotal, (int)type, text); else DisableInfo(UpdateRamTotal);
                    break;

                case InfoType.RamUsed:
                    if (enabled) EnableInfo(UpdateRamUsed, (int)type, text); else DisableInfo(UpdateRamUsed);
                    break;

                case InfoType.RamPercent:
                    if (enabled) EnableInfo(UpdateRamPercentage, (int)type, text); else DisableInfo(UpdateRamPercentage);
                    break;

                case InfoType.OperatingSystem:
                    if (enabled) EnableInfo(UpdateOS, (int)type, text); else DisableInfo(UpdateOS);
                    break;

                case InfoType.CPU:
                    if (enabled) EnableInfo(UpdateCPU, (int)type, text); else DisableInfo(UpdateCPU);
                    break;

                //case InfoType.CPUCores:
                //    if (enabled) EnableInfo(UpdateCPUCores, (int)type, text); else DisableInfo(UpdateCPUCores);
                //    break;

                //case InfoType.CPUFrequency:
                //    if (enabled) EnableInfo(UpdateCPUFreq, (int)type, text); else DisableInfo(UpdateCPUFreq);
                //    break;

                case InfoType.GPU:
                    if (enabled) EnableInfo(UpdateGPU, (int)type, text); else DisableInfo(UpdateGPU);
                    break;

                //case InfoType.GPUMemory:
                //    if (enabled) EnableInfo(UpdateGPUMem, (int)type, text); else DisableInfo(UpdateGPUMem);
                //    break;

                case InfoType.RAM:
                    if (enabled) EnableInfo(UpdateRAM, (int)type, text); else DisableInfo(UpdateRAM);
                    break;
            }
        }

        private void EnableInfo(Action a, int i, Text text)
        {
            if (!infoUpdateActionList.Contains(a))
            {
                if (!valueTextList[i] && !text)
                {
                    Debug.LogError((InfoType)i + "text UI 없음");
                    return;
                }
                infoUpdateActionList.Add(a);

                if (text)
                {
                    valueTextList[i] = text;
                }
            }
        }

        private void DisableInfo(Action a)
        {
            if (infoUpdateActionList.Contains(a))
                infoUpdateActionList.Remove(a);
        }

        public void UpdateFPS()
        {
            fps = (int)(1f / Time.deltaTime);

            if (valueTextList[0] != null)
            {
                valueTextList[0].text = fps.ToString();
                if (fpsWarning)
                {
                    if (fps < fpsWarningLimit)
                    {
                        valueTextList[0].color = fpsWarningColor;
                    }
                    else
                    {
                        valueTextList[0].color = fpsInitialColor;
                    }
                }
            }
        }

        public void UpdateFPSAvg()
        {
            dataAmount++;

            totalFPS += fps;

            avg = (int)((float)totalFPS / dataAmount);

            PlayerPrefs.SetInt("TotalFPS", (int)totalFPS);
            PlayerPrefs.SetInt("FPSDataAmount", (int)dataAmount);

            valueTextList[1].text = avg.ToString();
        }

        public void UpdateFPSMin()
        {
            if (fps < min && fps != 0)
            {
                min = fps;
                PlayerPrefs.SetInt("FPSMin", min);
            }

            valueTextList[2].text = min.ToString();
        }

        public void UpdateFPSMax()
        {
            if (fps > max)
            {
                max = fps;
                PlayerPrefs.SetInt("FPSMax", max);
            }

            valueTextList[3].text = max.ToString();
        }

        private float totalMemeory;
        private float allocatedMemory;
        private float percentageMemory;

        public void UpdateRamTotal()
        {
            totalMemeory = (Profiler.GetTotalReservedMemoryLong() / 1000000f);
            valueTextList[4].text = totalMemeory.ToString("0") + "MB";
        }

        public void UpdateRamUsed()
        {
            allocatedMemory = (Profiler.GetTotalAllocatedMemoryLong() / 1000000f);
            valueTextList[5].text = allocatedMemory.ToString("0") + "MB";
        }

        public void UpdateRamPercentage()
        {
            percentageMemory = (allocatedMemory / totalMemeory) * 100;
            valueTextList[6].text = percentageMemory.ToString("0.00") + "%";
        }

        public void UpdateDeviceModel()
        {
            valueTextList[7].text = SystemInfo.deviceModel;
        }

        public void UpdateOS()
        {
            valueTextList[8].text = SystemInfo.operatingSystem;
        }

        public void UpdateCPU()
        {
            if (SystemInfo.processorType != null)
                valueTextList[9].text = SystemInfo.processorType + " (" + SystemInfo.processorCount.ToString() + "core)";
        }

        //public void UpdateCPUCores()
        //{
        //    valueTextList[7].text = "";
        //}

        //public void UpdateCPUFreq()
        //{
        //    valueTextList[8].text = "";
        //}

        public void UpdateGPU()
        {
            valueTextList[10].text = SystemInfo.graphicsDeviceName + " (" + SystemInfo.graphicsMemorySize.ToString() + "MB)";
        }

        //public void UpdateGPUMem()
        //{
        //    valueTextList[10].text = "";
        //}

        public void UpdateRAM()
        {
            valueTextList[11].text = SystemInfo.systemMemorySize.ToString() + "MB";
        }
    }
}