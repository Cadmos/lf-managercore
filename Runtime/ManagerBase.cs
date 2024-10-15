using System;
using System.Collections.Generic;
using LF.Interfaces.Runtime;
using UnityEngine;

namespace LF.ManagerCore.Runtime
{
    [Serializable]
    public abstract class ManagerBase : MonoBehaviour, IManager
    {
        private static Dictionary<Type, ManagerBase> _instances = new ();
        
        public static T GetInstance<T>() where T : ManagerBase
        {
            _instances.TryGetValue(typeof(T), out ManagerBase instance);
            return instance as T;
        }

        protected float initializationProgress;
        public float InitializationProgress => initializationProgress;
        public bool IsInitialized { get; }
        
        protected event Action OnInitializationComplete;

        protected void RaiseInitializationComplete()
        {
            OnInitializationComplete?.Invoke();
        }
        
        event Action IManager.OnInitializationComplete
        {
            add => OnInitializationComplete += value;
            remove => OnInitializationComplete -= value;
        }

        public virtual void AwakeManager()         {
            var type = GetType();
            if (_instances.ContainsKey(type))
            {
                Destroy(gameObject);
            }
            else
            {
                _instances[type] = this;
                DontDestroyOnLoad(gameObject);
            }
        }
        public virtual void StartManager() { }
        public virtual void UpdateManager() { }
        public virtual void FixedUpdateManager() { }
        public virtual void LateUpdateManager() { }
        public virtual void OnEnableManager() { }
        public virtual void OnDisableManager() { }
        public virtual void OnDestroyManager() { }
        public virtual void ShutdownManager() { }
    }
}