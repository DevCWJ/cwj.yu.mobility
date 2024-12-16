#if UNITY_EDITOR
using System.Collections;

using UnityEngine;

namespace CWJ.Unity.EditorCor.Editor
{
    public static class EditorCoroutineUtil
    {
        /// <summary>
        /// Starts an <see cref ="Coroutine_Editor">EditorCoroutine</see> with the specified owner object. 
        /// If the garbage collector collects the owner object, while the resulting coroutine is still executing, the coroutine will stop running.
        /// <code>
        /// using System.Collections;
        /// using CWJ.Unity.EditorCor.Editor;
        /// using UnityEditor;
        ///
        /// public class ExampleWindow : EditorWindow
        /// {
        ///     int m_Updates = 0;
        ///     void OnEnable()
        ///     {
        ///         EditorCoroutineUtility.StartCoroutine(CountEditorUpdates(), this);
        ///     }
        ///
        ///     IEnumerator CountEditorUpdates()
        ///     {
        ///         while (true)
        ///         {
        ///             ++m_Updates;
        ///             yield return null;
        ///         }
        ///     }
        /// }
        /// </code>
        /// </summary>
        /// <param name="routine"> IEnumerator to iterate over. </param>
        /// <param name="owner">Object owning the coroutine. </param>
        /// <remarks>
        /// Only types that don't inherit from <see cref="UnityEngine.Object">UnityEngine.Object</see> will get collected the next time the GC runs instead of getting null-ed immediately.
        /// </remarks>
        /// <returns>A handle to an <see cref="Coroutine_Editor">EditorCoroutine</see>.</returns>
        public static Coroutine_Editor StartCoroutine(IEnumerator routine, object owner)
        {
            return new Coroutine_Editor(routine, owner);
        }

        /// <summary>
        /// This method starts an <see cref="Coroutine_Editor">EditorCoroutine</see> without an owning object. The <see cref="Coroutine_Editor">EditorCoroutine</see> runs until it completes or is canceled using <see cref="StopCoroutine(Coroutine_Editor)">StopCoroutine</see>.
        /// <code>
        /// using System.Collections;
        /// using CWJ.Unity.EditorCor.Editor;
        /// using UnityEditor;
        /// using UnityEngine;
        ///
        /// public class ExampleWindow : EditorWindow
        /// {
        ///     void OnEnable()
        ///     {
        ///         EditorCoroutineUtility.StartCoroutineOwnerless(LogTimeSinceStartup());
        ///     }
        ///
        ///     IEnumerator LogTimeSinceStartup()
        ///     {
        ///         while (true)
        ///         {
        ///             Debug.LogFormat("Time since startup: {0} s", Time.realtimeSinceStartup);
        ///             yield return null;
        ///         }
        ///     }
        /// }
        /// </code>
        /// </summary>
        /// <param name="routine"> Generator function to execute. </param>
        /// <returns>A handle to an <see cref="Coroutine_Editor">EditorCoroutine.</see></returns>
        public static Coroutine_Editor StartCoroutineOwnerless(IEnumerator routine)
        {
            return new Coroutine_Editor(routine);
        }

        /// <summary>
        /// Immediately stop an <see cref="Coroutine_Editor">EditorCoroutine</see>. This method is safe to call on an already completed <see cref="Coroutine_Editor">EditorCoroutine</see>.
        /// <code>
        /// using System.Collections;
        /// using CWJ.Unity.EditorCor.Editor;
        /// using UnityEditor;
        /// using UnityEngine;
        ///
        /// public class ExampleWindow : EditorWindow
        /// {
        ///     EditorCoroutine m_LoggerCoroutine;
        ///     void OnEnable()
        ///     {
        ///         m_LoggerCoroutine = EditorCoroutineUtility.StartCoroutineOwnerless(LogRunning());
        ///     }
        ///     
        ///     void OnDisable()
        ///     {
        ///         EditorCoroutineUtility.StopCoroutine(m_LoggerCoroutine);
        ///     }
        ///
        ///     IEnumerator LogRunning()
        ///     {
        ///         while (true)
        ///         {
        ///             Debug.Log("Running");
        ///             yield return null;
        ///         }
        ///     }
        /// }
        /// </code>
        /// </summary>
        /// <param name="coroutine">A handle to an <see cref="Coroutine_Editor">EditorCoroutine.</see></param>
        public static void StopCoroutine(Coroutine_Editor coroutine)
        {
            if (coroutine == null)
            {
                Debug.LogAssertion("EditorCoroutine handle is null.");
                return;
            }
            coroutine.Stop();
        }
    }
} 
#endif