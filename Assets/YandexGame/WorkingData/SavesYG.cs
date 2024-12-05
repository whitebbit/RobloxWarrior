
using _3._Scripts.Saves;
using _3._Scripts.Saves.Handlers;
using UnityEngine.Serialization;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;
        
        // Ваши сохранения

        public bool defaultLoaded;
        
        public WalletSave walletSave = new();
        public AbilitiesSave abilitiesSave = new();
        public PlayerStatsSave stats = new();
        public SwordsSave swordsSave = new();
        public WorldSave worldSave = new();
        
        // Вы можете выполнить какие-то действия при загрузке сохранений
        public SavesYG()
        {
            // Допустим, задать значения по умолчанию для отдельных элементов массива
        }
    }
}
