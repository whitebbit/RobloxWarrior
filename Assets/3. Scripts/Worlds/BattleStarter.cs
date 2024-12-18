using System;
using _3._Scripts.Game;
using _3._Scripts.Sounds;
using _3._Scripts.Tutorial;
using _3._Scripts.UI.Extensions;
using UnityEngine;

namespace _3._Scripts.Worlds
{
    public class BattleStarter : MonoBehaviour
    {
        [SerializeField] private CountdownTimer timer;
        private BoxCollider _boxCollider;
        private BattleArena _battleArena;

        private void OnValidate()
        {
            _boxCollider = GetComponent<BoxCollider>();
        }

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
        }

        private void Start()
        {
            timer.StopCountdown();
        }

        public void SetBattleArena(BattleArena battleArena) => _battleArena = battleArena;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Player.Player _)) return;

            timer.StartCountdown(() =>
            {
                GameEvents.BeforeBattle();
                _battleArena.StartBattle(GameContext.StartWaveNumber);
                AudioManager.Instance.StopLoop("lobby_music");
                AudioManager.Instance.PlaySound("battle_music", loop:true);
            }, 5);
            
            TutorialManager.Instance.DisableStep("battle");
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out Player.Player _)) return;

            timer.StopCountdown();
        }

        private void OnDrawGizmos()
        {
            var color = Color.yellow;

            UnityEngine.Gizmos.color = color;
            UnityEngine.Gizmos.DrawWireCube(transform.position, _boxCollider.size);
        }
    }
}