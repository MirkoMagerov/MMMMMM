using System.Collections;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private Animator sceneTransitionAnimator;
    [SerializeField] private GameObject sceneTransitionCanvas;

    private void Start()
    {
        sceneTransitionCanvas.SetActive(false);
    }

    public IEnumerator StartLevelAnimation()
    {
        sceneTransitionAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        sceneTransitionAnimator.SetTrigger("Idle");
        sceneTransitionCanvas.SetActive(false);
    }

    public IEnumerator EndLevelAnimation()
    {
        sceneTransitionCanvas.SetActive(true);
        sceneTransitionAnimator.SetTrigger("End");
        yield return new WaitForSeconds(1.5f);
    }
}
