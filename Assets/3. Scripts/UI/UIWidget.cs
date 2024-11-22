namespace _3._Scripts.UI
{
    public abstract class UIWidget : UIElement
    {
        public void SetScreen(UIScreen screen, int siblingIndex = -1)
        {
            transform.SetParent(screen.transform);
            
            if (siblingIndex < 0)
                transform.SetAsLastSibling();
            else
                transform.SetSiblingIndex(siblingIndex);
        }
    }
}