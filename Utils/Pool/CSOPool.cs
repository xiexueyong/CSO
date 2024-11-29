using System;
using System.Collections.Generic;
using System.Linq;
using CSOEngine.Object;
using CSOEngine.Utils;
using UnityEngine;

namespace CSOEngine.Utils.Pool {
    /// <summary>
    /// c#类通用对象池工厂
    /// </summary>
    public  class CSOPool {
        private static readonly Dictionary<Type, object> _pools = new ();
        public static T Get<T>() where T:new(){
            var type = typeof(T);
            if (!_pools.ContainsKey(type)) {
                _pools[type] = new CSharpObjectPool<T>();
            }
            return  ((CSharpObjectPool<T>)_pools[type]).Get();
        }
        
        public static  void Release<T>(T obj) where T:class,new(){
            var type = typeof(T);
            if (!_pools.ContainsKey(type)) {
                _pools[type] = new CSharpObjectPool<T>();
            }
            ((CSharpObjectPool<T>)_pools[type]).Release(obj);
        }

        private static List<(Type, int, int)> PoolCapacity = new List<(Type, int, int)>(5);

        public static void SetCapacity<T>(int InitLength,int MaxLength)
        {
            PoolCapacity.Add((typeof(T),InitLength,MaxLength));
        }
        public static (Type,int,int) GetCapacity<T>()
        {
            Type type = typeof(T);
            foreach (var capa in PoolCapacity)
            {
                if (type == capa.Item1 || 
                    type.IsSubclassOf(capa.Item1) ||
                    type.GetInterfaces().Contains(capa.Item1))
                {
                    return capa;
                }
            }
            
            return (null,5,20);
        }
        
    }

    public class CSharpObjectPool<T>  where T : new(){
        private readonly Stack<T> _pool;
        private readonly int _maxSize;
        public CSharpObjectPool()
        {
            (Type,int,int) capacity = CSOPool.GetCapacity<T>();
            _pool = new Stack<T>(capacity.Item2);
            _maxSize = capacity.Item3;
        }
        
        public T Get()
        {
            T t =  _pool.Count > 0 ? _pool.Pop() : new T();
            if (t is ICSOPoolObject po)
            {
                if (po.UseDefaultReset)
                {
                    ReflectionUtils.ResetFields(t);
                }
                po.OnPOActivate();
                po.InPool = false;
            }
            else
            {
                ReflectionUtils.ResetFields(t); 
            }
            return t;
        }
        
        public void Release(T obj)
        {
            if(obj == null)
                return;
            
            if (obj is ICSOPoolObject poolObject)
            {
                poolObject.OnPODeactivate();
            }
            
            if (_pool.Count < _maxSize)
            {
                if (obj is ICSOPoolObject po)
                {
                    po.InPool = true;
                }
#if UNITY_EDITOR
                if (_pool.Contains(obj))
                {
                    Debug.LogError("repeat push to pool");
                    throw new Exception("重复回收同一个对象："+obj.ToString());
                }
#endif
                _pool.Push(obj);
            }
        }
        
        public void Clear()
        {
            _pool.Clear();
        }
        
        public int Count => _pool.Count;
    }
}