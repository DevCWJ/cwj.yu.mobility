﻿//THIS CHARACTER CONTROLLER IS VERY ROUGH AND IS MEANT FOR DEMO PURPOSES ONLY. YOU MAY USE IT IN YOUR GAME, BUT DON'T EXPECT IT TO BE GOOD.

using UnityEngine;
using Random = UnityEngine.Random;
using CWJ.PhysicsSoundFx;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private AudioSource _audioSource;

    [SerializeField] private float _movementSpeed = 1f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _footstepSoundDelay = 0.5f;
    [SerializeField] private Transform _grounded;

    [SerializeField] private PhysicsSoundDictionary _footstepSounds;
    [SerializeField] private PhysicsSoundDictionary _landingSounds;

    private float _footstepTimer;
    private Vector3 _movement;
    private bool _isGrounded;
    private bool _shouldJump;

    private PhysicMaterial _groundedOn;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * _movementSpeed;
        _shouldJump = Input.GetButton("Jump");

        var wasGrounded = _isGrounded;
        var groundedOn = _groundedOn;
        _isGrounded = Physics.Linecast(_rigidbody.position, _grounded.position, out var hitInfo);
        _groundedOn = hitInfo.collider ? hitInfo.collider.sharedMaterial : null;

        if (groundedOn != _groundedOn)
        {
            _footstepSounds.UpdateActiveAudioClips(_groundedOn);
        }

        PlayFootsteps();

        if (!wasGrounded && _isGrounded)
        {
            PlayRandomSoundFromArray(_landingSounds.GetClipsFromMaterial(_groundedOn));
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.position += _movement * Time.deltaTime;

        if (!_shouldJump) { return; }

        _shouldJump = false;
        if (!_isGrounded) { return; }

        _rigidbody.AddForce(Vector3.up * _jumpForce);
    }

    private void PlayFootsteps()
    {
        _footstepTimer += Time.deltaTime;

        if (!(_movement.magnitude > 0) || !_isGrounded) { return; }

        if (_footstepTimer < _footstepSoundDelay) { return; }

        PlayRandomSoundFromArray(_footstepSounds.ActiveAudioClips);
        _footstepTimer = 0;
    }

    private void PlayRandomSoundFromArray(AudioClip[] clips)
    {
        var index = Random.Range(0, clips.Length);

        _audioSource.PlayOneShot(clips[index]);
    }
}

