using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Rendering;

public class ByteBeatPlayer : MonoBehaviour
{

    public LineRenderer lineRenderer;
    public LineRenderer otherLineRenderer;
    private const int spectrumSize = 256;
    private UnityEngine.Vector3[] positionsGreen;
    private UnityEngine.Vector3[] positionsBlue;
/// <summary>
    /// Plays bytebeat.
    /// </summary>
    /// <param name="time">Time to generate bytebeat</param>
    /// <param name="freq">Frequency to play bytebeat</param>
    /// <param name="beat">Delegate with parameter of float[], returns nothing.
    /// Tip: how to convert bytebeat to floatbeat (ranged 0f - 1f) => (byte)(/ your bytebeat /) / 256f
    /// </param>
    /// <param name="namel">Name. namel because of name is taken by MonoBehaviour.</param>
    /// <returns></returns>
    private IEnumerator PlayBytebeat(float time, int freq, AudioClip.PCMReaderCallback beat, string namel = "bytebeat")
    {
        AudioClip clip = AudioClip.Create(namel, (int)(time * freq), 1, freq, false, beat);
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.Play();
        yield return new WaitWhile(() => source.isPlaying);
        Destroy(source);
    }

    private void Start()
    {
        // Setup line renderer
        positionsGreen = new UnityEngine.Vector3[spectrumSize];
        positionsBlue = new UnityEngine.Vector3[spectrumSize];
        // Setup bytebeat
        int elapsedCycles = 0;
        StartCoroutine(PlayBytebeat(30, 8000, data =>
        {
            for (int t = elapsedCycles * 4096; t < data.Length + (elapsedCycles * 4096); t++)
            {
                data[t - (elapsedCycles * 4096)] = (byte)( t/30 | t%128+128 - t*4 | t%64 ) / 256f;
            }

            elapsedCycles++;
        }));
    }

void Update()
    {
        float[] spectrum = new float[spectrumSize];
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();

        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
        int i = 1;
        for ( ; i < spectrum.Length - 1; i++)
        {
            
            Debug.DrawLine(new UnityEngine.Vector3(i - 1, spectrum[i] + 10, 0), new UnityEngine.Vector3(i, spectrum[i + 1] + 10, 0), Color.red);
            Debug.DrawLine(new UnityEngine.Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), new UnityEngine.Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
            Debug.DrawLine(new UnityEngine.Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), new UnityEngine.Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
            Debug.DrawLine(new UnityEngine.Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), new UnityEngine.Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.blue);

            positionsGreen[i-1].Set(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3);            
            positionsBlue[i-1].Set(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2);
        }
        positionsGreen[i].Set(Mathf.Log(i), Mathf.Log(spectrum[i]), 3);
        positionsBlue[i].Set(i, Mathf.Log(spectrum[i]) + 10, 2);
        lineRenderer.SetPositions(positionsGreen);
        otherLineRenderer.SetPositions(positionsBlue);
    }
}
