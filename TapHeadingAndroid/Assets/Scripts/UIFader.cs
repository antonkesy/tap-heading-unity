/*
MIT License

Copyright (c) 2021 Anton Kesy

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */
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