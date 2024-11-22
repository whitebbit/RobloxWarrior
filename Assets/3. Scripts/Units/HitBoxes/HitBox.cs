using _3._Scripts.Units.Interfaces;
using UnityEngine;

namespace _3._Scripts.Units.HitBoxes
{
    public abstract class HitBox : MonoBehaviour, IWeaponVisitor
    {
        [SerializeField] protected Unit unit;
        
        public abstract void Visit(float damage);
        
    }
}