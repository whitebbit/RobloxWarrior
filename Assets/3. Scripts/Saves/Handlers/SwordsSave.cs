using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace _3._Scripts.Saves.Handlers
{
    [Serializable]
    public class SwordsSave : SaveHandler<SwordSave>
    {
        public int maxSwordsCount = 50;
        public int requiredCountForMerge = 3;

        public override bool IsCurrent(SwordSave obj)
        {
            return obj.uid == current.uid;
        }

        public event Action<SwordSave> OnSetCurrent;
        
        public override void SetCurrent(SwordSave obj)
        {
            current = obj;
            OnSetCurrent?.Invoke(current);
        }

        public override void Unlock(SwordSave obj)
        {
            if (Unlocked(obj)) return;

            unlocked.Add(obj);
        }

        public override bool Unlocked(SwordSave obj)
        {
            return unlocked.Exists(s => s.uid == obj.uid);
        }

        public event Action<SwordSave> OnDelete;

        public void Delete(SwordSave obj)
        {
            if (obj.uid == current.uid) return;

            unlocked.Remove(obj);
            OnDelete?.Invoke(obj);
        }

        public bool TryMergeObject(SwordSave targetObject)
        {
            if (targetObject.starCount == 5) return false;
            
            var matchingObjects = unlocked
                .Where(o => o.id == targetObject.id && o.starCount == targetObject.starCount)
                .ToList();

            if (matchingObjects.Count < requiredCountForMerge) return false; 
            foreach (var obj in matchingObjects.Take(requiredCountForMerge))
            {
                if (obj.uid != targetObject.uid) 
                {
                    Delete(obj);
                }
            }

            targetObject.starCount++;

            return true; 
        }
        
        public void MergeAll()
        {
            // Продолжаем до тех пор, пока можно выполнить хотя бы один мердж
            bool hasMerged;

            do
            {
                hasMerged = false;

                // Копируем список для безопасной итерации
                var objectsToCheck = unlocked.ToList();

                foreach (var targetObject in objectsToCheck)
                {
                    // Если удалось выполнить мердж для текущего объекта
                    if (TryMergeObject(targetObject))
                    {
                        hasMerged = true; // Указываем, что произошел мердж
                        break; // Прерываем текущий цикл, чтобы обновить список и начать заново
                    }
                }
            } while (hasMerged); // Пока был выполнен хотя бы один мердж
        }

        public int GetMergeableCount(SwordSave targetObject)
        {
            return unlocked.Count(o => o.id == targetObject.id && o.starCount == targetObject.starCount);
        }
    }
}