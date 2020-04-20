using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Fader : MonoBehaviour
{
    [SerializeField] float fadeTime = 1f;
    [SerializeField] float fadeAwaitTime = 1f;
    private CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        Fader fader = FindObjectOfType<Fader>();

        if (fader != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeInImmediate()
    {
        canvasGroup.alpha = 1;
    }

    public IEnumerator FadeOut()
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 1f / fadeTime * Time.deltaTime;
            AudioListener.volume += 1f / fadeTime * Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator FadeIn()
    {
        while (canvasGroup.alpha < 1)
        {
            AudioListener.volume -= 1f / fadeTime * Time.deltaTime;
            canvasGroup.alpha += 1f / fadeTime * Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(fadeAwaitTime);
    }
}
