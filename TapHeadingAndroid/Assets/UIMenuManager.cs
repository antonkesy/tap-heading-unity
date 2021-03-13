using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuManager : MonoBehaviour
{
    [SerializeField] private UIFader[] faders;
    [SerializeField] private UIFader highScoreTextFader;
    [SerializeField] private UIFader gameTitleFader;
    [SerializeField] private UIFader soundOffFader;
    [SerializeField] private UIFader soundOnFader;
    [SerializeField] private UIFader aboutButtonFader;
    [SerializeField] private UIFader leaderboardButtonFader;
    [SerializeField] private UIFader tapToStartFader;

    internal void FadeIn()
    {
        foreach (var fader in faders)
        {
            fader.Fade(true);
        }
    }

    internal void FadeOut()
    {
        foreach (var fader in faders)
        {
            fader.Fade(false);
        }
    }
}