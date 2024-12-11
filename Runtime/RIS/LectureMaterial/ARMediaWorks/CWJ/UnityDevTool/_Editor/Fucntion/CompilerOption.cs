#if UNITY_EDITOR

namespace CWJ.EditorOnly
{
    using UnityEditor;

    public static class ScriptOptions
    {
        //Auto Refresh

        //kAutoRefresh has two posible values
        //0 = Auto Refresh Disabled
        //1 = Auto Refresh Enabled


        //This is called when you click on the 'Tools/Auto Refresh' and toggles its value
        [MenuItem("Tools/Auto Refresh")]
        static void AutoRefreshToggle()
        {
            var status = EditorPrefs.GetInt("kAutoRefresh");
            if (status == 1)
                EditorPrefs.SetInt("kAutoRefresh", 0);
            else
                EditorPrefs.SetInt("kAutoRefresh", 1);
        }


        //This is called before 'Tools/Auto Refresh' is shown to check the current value
        //of kAutoRefresh and update the checkmark
        [MenuItem("Tools/Auto Refresh", true)]
        static bool AutoRefreshToggleValidation()
        {
            var status = EditorPrefs.GetInt("kAutoRefresh");
            if (status == 1)
                Menu.SetChecked("Tools/Auto Refresh", true);
            else
                Menu.SetChecked("Tools/Auto Refresh", false);
            return true;
        }


        //Script Compilation During Play


        //ScriptCompilationDuringPlay has three posible values
        //0 = Recompile And Continue Playing
        //1 = Recompile After Finished Playing
        //2 = Stop Playing And Recompile


        //The following methods assing the three possible values to ScriptCompilationDuringPlay
        //depending on the option you selected
        [MenuItem("Tools/Script Compilation During Play/Recompile And Continue Playing")]
        static void ScriptCompilationToggleOption0()
        {
            EditorPrefs.SetInt("ScriptCompilationDuringPlay", 0);
        }


        [MenuItem("Tools/Script Compilation During Play/Recompile After Finished Playing")]
        static void ScriptCompilationToggleOption1()
        {
            EditorPrefs.SetInt("ScriptCompilationDuringPlay", 1);
        }


        [MenuItem("Tools/Script Compilation During Play/Stop Playing And Recompile")]
        static void ScriptCompilationToggleOption2()
        {
            EditorPrefs.SetInt("ScriptCompilationDuringPlay", 2);
        }


        //This is called before 'Tools/Script Compilation During Play/Recompile And Continue Playing'
        //is shown to check for the current value of ScriptCompilationDuringPlay and update the checkmark
        [MenuItem("Tools/Script Compilation During Play/Recompile And Continue Playing", true)]
        static bool ScriptCompilationValidation()
        {
            //Here, we uncheck all options before we show them
            Menu.SetChecked("Tools/Script Compilation During Play/Recompile And Continue Playing", false);
            Menu.SetChecked("Tools/Script Compilation During Play/Recompile After Finished Playing", false);
            Menu.SetChecked("Tools/Script Compilation During Play/Stop Playing And Recompile", false);


            var status = EditorPrefs.GetInt("ScriptCompilationDuringPlay");


            //Here, we put the checkmark on the current value of ScriptCompilationDuringPlay
            switch (status)
            {
                case 0:
                    Menu.SetChecked("Tools/Script Compilation During Play/Recompile And Continue Playing", true);
                    break;
                case 1:
                    Menu.SetChecked("Tools/Script Compilation During Play/Recompile After Finished Playing", true);
                    break;
                case 2:
                    Menu.SetChecked("Tools/Script Compilation During Play/Stop Playing And Recompile", true);
                    break;
            }
            return true;
        }
    }
}
#endif 