namespace _3._Scripts.UI.Interfaces
{
    public interface IUIElement
    {
        public IUITransition InTransition { get; set; }
        public IUITransition OutTransition { get; set; }
        
        public bool Enabled { get; set; }
        
        public void Initialize();
        
        public void ForceOpen();
        public void ForceClose();
    }
}