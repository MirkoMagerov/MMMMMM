using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI textMeshPro;
    public float scaleFactor = 1.1f;
    public float transitionDuration = 0.2f;

    private Vector3 originalScale;
    private Vector3 targetScale;
    private float scaleSpeed;

    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        originalScale = textMeshPro.transform.localScale;
        scaleSpeed = 1f / transitionDuration;
        targetScale = transform.localScale;
    }

    private void Update()
    {
        textMeshPro.transform.localScale = Vector3.Lerp(textMeshPro.transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * scaleFactor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
    }
}
