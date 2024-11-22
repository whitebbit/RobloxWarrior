using System;
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

        public void SetBattleArena(BattleArena battleArena)=> _battleArena = battleArena;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Player.Player _)) return;

            timer.StartCountdown(_battleArena.StartBattle, 10);
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