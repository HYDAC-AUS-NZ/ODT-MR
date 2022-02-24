using UnityEngine;

using HYDACDB.PRO;
using LightShaft.Scripts;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private SocProductCallbacks productCallbacks;
    [SerializeField] private GameObject tutorialObject;
    [SerializeField] private YoutubeVideoController videoController;

    private void OnEnable()
    {
        productCallbacks.EToggleTutorial += OnToggleTutorial;
    }

    private void OnDisable()
    {
        productCallbacks.EToggleTutorial -= OnToggleTutorial;
    }

    private void OnToggleTutorial(bool toggle)
    {
        if (toggle)
            videoController.Play();
        else
            videoController.Pause();

        tutorialObject.SetActive(toggle);
    }
}
