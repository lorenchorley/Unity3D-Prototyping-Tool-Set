using UnityEngine;
using UnityEngine.UI;

public static class ScrollRectExtensions {

    public static void ScrollToTop(this ScrollRect scrollRect) {
        scrollRect.normalizedPosition = new Vector2(scrollRect.normalizedPosition.x, 1);
    }

    public static void ScrollToBottom(this ScrollRect scrollRect) {
        scrollRect.normalizedPosition = new Vector2(scrollRect.normalizedPosition.x, 0);
    }

    public static void ScrollToLeft(this ScrollRect scrollRect) {
        scrollRect.normalizedPosition = new Vector2(0, scrollRect.normalizedPosition.y);
    }

    public static void ScrollToRight(this ScrollRect scrollRect) {
        scrollRect.normalizedPosition = new Vector2(1, scrollRect.normalizedPosition.y);
    }

}

