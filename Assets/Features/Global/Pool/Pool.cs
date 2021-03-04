using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WeekAnkama
{
    public class Pool<T> : MonoBehaviour where T : MonoBehaviour 
    {
        [SerializeField] private List<T> objectPool;
        private List<T> usedObjectOfPool;
        [SerializeField] private int actualNumberOfObject = 0;
        [SerializeField] private T prefab;
        [SerializeField] private Transform selfTransform;
        private bool isInitialized = false;

        public T Prefab
        {
            get { return prefab; }
            set { prefab = value; }
        }
        public bool IsInitialized => isInitialized;

        public int ActualNumberOfObject => actualNumberOfObject;
        public int GetPoolCount => objectPool.Count;

        public void Init(Transform self)
        {
            selfTransform = self;
            isInitialized = true;
        }

        public T GetObjectInPool()
        {
            if (objectPool.Count == 0) {            
                return null;
            } 
            if(usedObjectOfPool == null)
            {
                usedObjectOfPool = new List<T>();
            }
            T obj = objectPool[0];
            objectPool.Remove(obj);
            usedObjectOfPool.Add(obj);
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void ReturnObject(T toDisable)
        {
            if (!usedObjectOfPool.Contains(toDisable))
            {
                return;
            }
            toDisable.gameObject.SetActive(false);
            usedObjectOfPool.Remove(toDisable);
            objectPool.Add(toDisable);
        }

    #if UNITY_EDITOR
        public void Generate(int nbToGenerate)
        {
            if (objectPool == null) return;

            for (int nb = 0; nb < nbToGenerate; nb++)
            {
                T t = Instantiate(prefab);
                t.transform.parent = selfTransform;
                t.gameObject.SetActive(false);
                objectPool.Add(t);
            }
            actualNumberOfObject += nbToGenerate;
        }
        public void Remove(int nbToRemove)
        {
            if (objectPool == null) return;

            if (nbToRemove > objectPool.Count)
            {
                Reset();
            }
            else
            {
                actualNumberOfObject -= nbToRemove;
                for(int index = 0; index < nbToRemove; index++)
                {
                    DestroyImmediate(objectPool[index].gameObject);
                }
                objectPool.RemoveRange(0, nbToRemove);
            }        
        }
        public T GetAt(int index)
        {
            return objectPool[index];
        }

        public void Reset()
        {
            if (objectPool == null) return;
            for (int index = 0; index < objectPool.Count; index++)
            {
                if (objectPool[index] == null) continue;
                DestroyImmediate(objectPool[index].gameObject);
            }
            objectPool.Clear();
            actualNumberOfObject = 0;
        }
    #endif
    }

}
