using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Torch : MonoBehaviour
{
    private Light2D light;

    public List<Sprite> sprites = new List<Sprite>();
    public float timeBetweenFrames = 0.5F;
    private SpriteRenderer renderer;
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        light = GetComponent<Light2D>();
        StartCoroutine(Light());
        StartCoroutine(SpriteChanger());
    }

    IEnumerator Light()
    {
        while (true)
        {
            var a = Random.Range(1.26F, 1.9F);
            var b = Random.Range(1.26F, 1.9F);
            for (float t = 0f; t < 1F; t += Time.deltaTime)
            {
                light.intensity = Mathf.Lerp(a, b, t / 1F);
                yield return null;
            }
            light.intensity = b;
        }
    }

    IEnumerator SpriteChanger()
    {
        var i = 0;
        while (true)
        {
            renderer.sprite = sprites[i];
            yield return new WaitForSeconds(timeBetweenFrames);
            i++;
            if (i > sprites.Count-1)
            {
                i = 0;
            }
        }
    }
}
