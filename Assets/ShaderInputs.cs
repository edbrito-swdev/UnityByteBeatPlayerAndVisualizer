using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderInputs : MonoBehaviour
{
    [SerializeField] AudioSource soundSource;
    [SerializeField] Renderer rend;
    [SerializeField] Material material;
    private float realDistortionAmount;

    [SerializeField] ParticleSystem pulsingRing;
    [SerializeField] ParticleSystem smokeMachine1;
    [SerializeField] ParticleSystem smokeMachine2;
    [SerializeField] Animation spinningRing;
    [SerializeField] ParticleSystem fireRing1;
    [SerializeField] ParticleSystem fireRing2;

    private CameraShake cameraEffect;
    

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        material = rend.material;        
        
    }

    // Update is called once per frame
    void Update()
    {
       
        float[] samples = soundSource.GetOutputData(1024, 0);
        float max = 0;
        float loudness = 0;
        int tam = samples.Length;
        foreach (float sample in samples)
        {
            loudness += (sample + 1f) / 2f;

            //if (loudness > max)
            //{
            //    max = loudness;
            //}
        }

        loudness = loudness / tam;
        realDistortionAmount = loudness * 2;
        Debug.Log(realDistortionAmount);

        material.SetFloat("_ExtrudeAmount", realDistortionAmount);

        if(loudness > 0.85 && !pulsingRing.isPlaying)
        {
            ActivatePulse();
            PlaySpinnigFire();
        }

        if (loudness < 0.7 && !smokeMachine1.isPlaying)
        {
            ActivateSmoke();
            //StartCoroutine(cameraEffect.CameraEffect(0.5f, 1.5f));
        }

    }

    void ActivatePulse()
    {
        pulsingRing.Play();
    }

    void ActivateSmoke()
    {
        smokeMachine1.Play();
        smokeMachine2.Play();
    }

    void PlaySpinnigFire()
    {
        spinningRing.Play("Spinning");
        fireRing1.Play();
        fireRing2.Play();
    }

   
}


