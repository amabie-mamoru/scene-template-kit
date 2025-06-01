using UnityEngine;

namespace com.amabie.SceneTemplateKit
{
    public class DefaultGameObjectFactory : IGameObjectFactory
    {
        public T Instantiate<T>(T prefab, Transform parent = null) where T : UnityEngine.Object
        {
            return UnityEngine.Object.Instantiate(prefab, parent);
        }
    }
}