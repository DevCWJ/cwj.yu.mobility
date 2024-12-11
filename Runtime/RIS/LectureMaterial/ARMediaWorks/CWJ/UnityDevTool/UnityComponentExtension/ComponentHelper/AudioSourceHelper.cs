using System;
using System.Collections;
using System.Collections.Generic;

using CWJ;

using UnityEngine;

[RequireComponent(typeof(AudioSource)), DisallowMultipleComponent]
public class AudioSourceHelper : MonoBehaviour
{
    [GetComponent, ErrorIfNull, SerializeField] private AudioSource audioSource;

    [OnValueChanged(nameof(InspectorModifiedCheck))]
    [SerializeField] private AudioClip _clip;

    public AudioClip clip
    {
        get => _clip;
        set => audioSource.clip = _clip = value;
    }

    private void InspectorModifiedCheck()
    {
        clip = _clip;
        playOnAwake = _playOnAwake;
        volume = _volume;
    }

    [OnValueChanged(nameof(InspectorModifiedCheck))]
    [SerializeField] private bool _playOnAwake;

    public bool playOnAwake
    {
        get => _playOnAwake;
        set => audioSource.playOnAwake = _playOnAwake = value;
    }

    [OnValueChanged(nameof(InspectorModifiedCheck))]
    [SerializeField] private float _volume = 1;

    public float volume
    {
        get => _volume;
        set => audioSource.volume = _volume = value;
    }

    public bool isPlaying
    {
        get => audioSource.isPlaying;
    }

    [Readonly] public bool isPause = false;

    public bool isListAlreadyPlaying => CO_PlayRoutine != null;

    private void Reset()
    {
        audioSource = transform.GetOrAddComponent<AudioSource>();
    }

    private void Awake()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    [InvokeButton]
    public void Play()
    {
        audioSource.Play();
        isPause = false;
    }

    [InvokeButton]
    public void Pause()
    {
        audioSource.Pause();
        isPause = true;
    }

    [InvokeButton]
    public void UnPause()
    {
        audioSource.UnPause();
        isPause = false;
    }

    [InvokeButton]
    public void Stop()
    {
        audioSource.Stop();
        isPause = false;
    }

    public void PlayOneShot(AudioClip audioClip, float volumeScale = 0)
    {
        if (volumeScale == 0)
        {
            audioSource.PlayOneShot(audioClip);
        }
        else
        {
            audioSource.PlayOneShot(audioClip, volumeScale);
        }
    }

    private Queue<AudioClip> audioDelayQue = new Queue<AudioClip>();

    public void PlayDelayList(params AudioClip[] audioClips)
    {
        if (audioClips == null || audioClips.Length == 0) return;

        foreach (var item in audioClips)
        {
            if (item != null)
            {
                audioDelayQue.Enqueue(item);
            }
        }

        if (audioDelayQue.Count > 0)
        {
            if (CO_PlayClipQue == null)
            {
                CO_PlayClipQue = StartCoroutine(DO_PlayDelay());
            }
        }
    }

    private Coroutine CO_PlayClipQue = null;

    private IEnumerator DO_PlayDelay()
    {
        while (isPlaying || isPause)
        {
            yield return null;
        }

        while (audioDelayQue.Count > 0)
        {
            AudioClip audioClip = audioDelayQue.Dequeue();

            clip = audioClip;
            Play();

            yield return StartCoroutine(DelayTime(audioClip.length));
        }

        CO_PlayClipQue = null;
    }

    private IEnumerator DelayTime(float time)
    {
        float t = 0;
        do
        {
            yield return null;

            if (isPlaying)
            {
                t += Time.deltaTime;
            }
        } while (t < time && (isPlaying || isPause));
        yield break;
    }

    public void PlayAudioList(Action<int> clipFinishedCallback = null, params AudioClip[] audioClips)
    {
        if (isListAlreadyPlaying) return;

        CO_PlayRoutine = StartCoroutine(DO_PlayRoutine(clipFinishedCallback, audioClips));
    }

    private Coroutine CO_PlayRoutine = null;

    private IEnumerator DO_PlayRoutine(Action<int> clipFinishedCallback, params AudioClip[] audioClips)
    {
        bool backupLoop = audioSource.loop;
        audioSource.loop = false;

        int audioClipsLength = audioClips.Length;

        for (int i = 0; i < audioClipsLength; i++)
        {
            audioSource.clip = audioClips[i];
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
            clipFinishedCallback?.Invoke(i);
        }

        audioSource.loop = backupLoop;
        CO_PlayRoutine = null;
    }

    //public AudioClip[] audioClips;

    //[ContextButton]
    //private void Test()
    //{
    //    PlayDelayList(audioClips);
    //}
}