using System.Collections;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private Animator sceneTransitionAnimator;

    public IEnumerator StartLevelAnimation()
    {
        sceneTransitionAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        sceneTransitionAnimator.SetTrigger("Idle");
    }

    public IEnumerator EndLevelAnimation()
    {
        sceneTransitionAnimator.SetTrigger("End");
        yield return new WaitForSeconds(1.5f);
    }
}
