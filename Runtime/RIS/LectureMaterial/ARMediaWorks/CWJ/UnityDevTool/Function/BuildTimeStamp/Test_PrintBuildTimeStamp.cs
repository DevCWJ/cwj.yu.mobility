using System.IO;

using UnityEngine;
using UnityEngine.UI;

namespace CWJ.BuildTimeStamp
{
    public class Test_PrintBuildTimeStamp : MonoBehaviour
    {
        public Text timeStampText;

        private void Start()
        {
            UpdateTimeStamp();
        }

        [InvokeButton]
        private void UpdateTimeStamp()
        {
            if (!PathUtil.IsFileExists(BuildTimeStampMngr.TimeStampTxtPath, false, false))
            {
                timeStampText.text = "";
                return;
            }
            StreamReader reader = new StreamReader(BuildTimeStampMngr.TimeStampTxtPath);
            timeStampText.text = reader.ReadLine();
            reader.Close();
        }
    }

}