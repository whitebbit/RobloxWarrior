namespace _3._Scripts.Abilities.Interfaces
{
    public interface IAbility
    {
        public string DescriptionID { get; }
        public bool CanUse { get; }
        public void UseAbility();
    }
}