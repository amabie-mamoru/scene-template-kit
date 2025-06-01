using UnityEngine;

namespace com.amabie.SceneTemplateKit
{
    public interface IGameObjectFactory
    {
        T Instantiate<T>(T prefab, Transform parent = null) where T : UnityEngine.Object;
    }
}