using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private RandomSound engineSound;
    
    [SerializeField] private float thrustForce = 500f;
    [SerializeField] private float rotateSpeed = 100f;
    
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem leftThrusterParticles;
    [SerializeField] ParticleSystem rightThrusterParticles;

    private Rigidbody _rb;
    private AudioSource _audio;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _audio = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _rb.AddRelativeForce(Vector3.up * Time.deltaTime * thrustForce);
            if (!_audio.isPlaying)
            {
                _audio.PlayOneShot(engineSound.GetSound());
            }
            if (!mainEngineParticles.isPlaying)
            {
                mainEngineParticles.Play();
            }

        }
        else
        {
            mainEngineParticles.Stop();
            _audio.Stop();
        }
    }

    void ProcessRotation()
    {
        float rotate = -Input.GetAxis("Horizontal");
        if (rotate != 0)
        {
            _rb.angularVelocity = new Vector3(0, 0, rotate * Time.deltaTime * rotateSpeed); 
            
            if (rotate > 0)
            {
                if (!rightThrusterParticles.isPlaying)
                {
                    rightThrusterParticles.Play();
                }
            }
            else
            {
                if (!leftThrusterParticles.isPlaying)
                {
                    leftThrusterParticles.Play();
                }
            }
        }
        else
        {
            rightThrusterParticles.Stop();
            leftThrusterParticles.Stop();
        }
    }

    private void OnDisable()
    {
        _audio.Stop();
    }
}
