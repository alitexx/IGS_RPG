using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollSync : MonoBehaviour
{
    public ScrollRect scrollRect;
    public float scrollSpeed = 10f;

    void Update()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;
        if (selected != null && selected.transform.IsChildOf(scrollRect.content))
        {
            RectTransform selectedRectTransform = selected.GetComponent<RectTransform>();

            // Calculate the top and bottom of the selected item in content space
            float minY = selectedRectTransform.anchoredPosition.y;
            float maxY = minY + selectedRectTransform.rect.height;

            // Calculate the visible area in content space
            float viewportHeight = scrollRect.GetComponent<RectTransform>().rect.height;
            float contentHeight = scrollRect.content.rect.height;

            float scrollMinY = -scrollRect.content.anchoredPosition.y;
            float scrollMaxY = scrollMinY + viewportHeight;

            // Adjust scroll if selected item is out of bounds
            if (minY > scrollMinY)
            {
                // Allow it to reach 0 (top of the content)
                float scrollPos = Mathf.Clamp01((minY + selectedRectTransform.rect.height) / (contentHeight - viewportHeight));
                scrollRect.verticalNormalizedPosition = Mathf.Lerp(scrollRect.verticalNormalizedPosition, scrollPos*3, Time.deltaTime * scrollSpeed);
            }
            else if (maxY < scrollMaxY)
            {
                // Allow it to reach 1 (bottom of the content)
                float scrollPos = Mathf.Clamp01(minY / (contentHeight - viewportHeight));
                scrollRect.verticalNormalizedPosition = Mathf.Lerp(scrollRect.verticalNormalizedPosition, scrollPos*3, Time.deltaTime * scrollSpeed);
            }
        }
    }
}
