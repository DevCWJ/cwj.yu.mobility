#if UNITY_EDITOR
namespace CWJ.Unity.EditorCor.Editor
{
    /// <summary>
    /// Suspends the <see cref="Coroutine_Editor">EditorCoroutine</see> execution for the given amount of seconds, using unscaled time. 
    /// The coroutine execution continues after the specified time has elapsed.
    /// <code>
    /// using System.Collections;
    /// using UnityEngine;
    /// using CWJ.Unity.EditorCor.Editor;
    /// using UnityEditor;
    ///
    /// public class MyEditorWindow : EditorWindow
    /// {
    ///     IEnumerator PrintEachSecond()
    ///     {
    ///         var waitForOneSecond = new EditorWaitForSeconds(1.0f);
    ///
    ///         while (true)
    ///         {
    ///             yield return waitForOneSecond;
    ///             Debug.Log("Printing each second");
    ///         }
    ///     }
    /// }
    /// </code>
    /// </summary>
    public class WaitForSeconds_Editor
    {
        /// <summary>
        /// The time to wait in seconds.
        /// </summary>
        public float WaitTime { get; }

        /// <summary>
        /// Creates a instruction object for yielding inside a generator function.
        /// </summary>
        /// <param name="time">The amount of time to wait in seconds.</param>
        public WaitForSeconds_Editor(float time)
        {
            WaitTime = time;
        }
    }
} 
#endif