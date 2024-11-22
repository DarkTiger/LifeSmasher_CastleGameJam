using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    Vector3 originalPosition;


    void Start()
    {
        originalPosition = transform.localPosition;
    }

    public void Shake(float duration, float intensity, float delay)
    {
        StartCoroutine(ShakeCoroutine(duration, intensity, delay));
    }

    private IEnumerator ShakeCoroutine(float duration, float intensity, float delay)
    {
        yield return new WaitForSeconds(delay);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float x = Random.Range(-intensity, intensity);
            float y = Random.Range(-intensity, intensity);

            transform.localPosition += new Vector3(x, y, 0f);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
