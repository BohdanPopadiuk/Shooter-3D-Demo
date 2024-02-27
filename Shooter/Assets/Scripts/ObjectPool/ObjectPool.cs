using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ObjectPool
{
    public class ObjectPool<T> : PoolBase<T> where T : Component
    {
        #region constructors

        public ObjectPool(T prefab, int preloadCount, Action<T> initialize = null)
            : base(() => Preload(prefab, initialize), GetAction, ReturnAction, preloadCount) { }

        #endregion
        
        #region public Methods

        public static T Preload(T prefab, Action<T> setPrefab)
        {
            T newObj = Object.Instantiate(prefab);

            if (setPrefab != null)
            {
                setPrefab(newObj);//
            }
            
            newObj.gameObject.SetActive(false);
            return newObj;
        }
        public static void GetAction(T @object) => @object.gameObject.SetActive(true);
        public static void ReturnAction(T @object) => @object.gameObject.SetActive(false);

        #endregion
    }
}