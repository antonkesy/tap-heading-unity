using System.Collections;
using UnityEngine;

public class UIFader : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private bool _isFadeIn = true;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Fade(bool fadeIn, float duration)
    {
        if (_isFadeIn == fadeIn)
        {
            return;
        }

        _isFadeIn = fadeIn;

        gameObject.SetActive(true);
        StartCoroutine(DoFade(_canvasGroup, fadeIn ? 0 : 1, fadeIn ? 1 : 0, fadeIn, duration));
    }

    private IEnumerator DoFade(CanvasGroup canvasGroup, float start, float end, bool endState, float duration)
    {
        var counter = 0f;

        while (counter < duration)
        {
            if (!endState)
            {
                counter += Time.deltaTime * 6;
            }

            counter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, counter / duration);

            yield return null;
        }

        gameObject.SetActive(endState);
    }
}