using System;
using System.Collections;
using UnityEngine;

public class UIFader : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    [SerializeField] private float duration = 1f;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        Debug.Log(_canvasGroup);
    }

    public void Fade(bool fadeIn)
    {
        gameObject.SetActive(true);
        StartCoroutine(DoFade(_canvasGroup, fadeIn ? 0 : 1, fadeIn ? 1 : 0, fadeIn));
    }

    private IEnumerator DoFade(CanvasGroup canvasGroup, float start, float end, bool endState)
    {
        float counter = 0f;

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