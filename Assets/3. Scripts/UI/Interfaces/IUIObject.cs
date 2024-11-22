namespace _3._Scripts.UI.Interfaces
{
    public interface IUIObject<out T>
    {
        public string TitleID { get; }
        public T Icon { get; }
        public string DescriptionID { get; }
    }
}