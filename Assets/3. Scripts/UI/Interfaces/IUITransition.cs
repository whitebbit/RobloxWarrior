using DG.Tweening;

namespace _3._Scripts.UI.Interfaces
{
    public interface IUITransition
    {
        public void SetLinkTransition(IUITransition transition);
        public IUITransition LinkTransition { get; set; }
        public Tween AnimateIn();
        public Tween AnimateOut();
        
        public void ForceIn();
        public void ForceOut();
    }
}