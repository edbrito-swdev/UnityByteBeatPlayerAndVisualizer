using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  

    public IEnumerator CameraEffect(float duration, float intensity) 
    {
        Vector3 cameraOrigin = transform.localPosition;

        float timeElapsed = 0.0f;

        while(timeElapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * intensity;
            float y = Random.Range(-1f, 1f) * intensity;

            transform.localPosition = new Vector3(x, y, cameraOrigin.z);

            timeElapsed += Time.deltaTime;

            yield return null;

        }

        transform.localPosition = cameraOrigin;
    }
}
