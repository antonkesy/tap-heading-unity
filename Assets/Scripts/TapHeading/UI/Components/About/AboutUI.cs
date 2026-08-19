using System.Collections.Generic;
using TapHeading.Services.Google;
using UnityEngine;

namespace TapHeading.UI.Components.About
{
    public class AboutUI : MonoBehaviour
    {
        [SerializeField] private GameObject aboutPanel;
        [SerializeField] private GameObject[] toHide;

        private readonly List<GameObject> _hidden = new List<GameObject>();

        public void Open()
        {
            Social.ReportProgress(GPGSIds.achievement_thank_you, 0.0f, null);
            GooglePlayServicesManager.Instance.ThankYouAchievement();

            aboutPanel.SetActive(true);

            foreach (var o in toHide)
            {
                if (o.activeSelf)
                {
                    _hidden.Add(o);
                    o.SetActive(false);
                }
            }
        }

        public void Close()
        {
            aboutPanel.SetActive(false);
            foreach (var o in _hidden)
            {
                o.SetActive(true);
            }

            _hidden.Clear();
        }

        public bool IsOpen()
        {
            return aboutPanel.activeSelf;
        }
    }
}