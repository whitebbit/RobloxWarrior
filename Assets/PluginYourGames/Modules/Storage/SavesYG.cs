using _3._Scripts.Saves;
using _3._Scripts.Saves.Handlers;

namespace YG
{
    public partial class SavesYG
    {
        public bool defaultLoaded;
        
        public WalletSave walletSave = new();
        public AbilitiesSave abilitiesSave = new();
        public PlayerStatsSave stats = new();
        public SwordsSave swordsSave = new();
        public WorldSave worldSave = new();
        public HeroesSave heroesSave = new();
        public PlayerCharacterSave characterSave = new();

        public string qualityName = "performance";
    }
}