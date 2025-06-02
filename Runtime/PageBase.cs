using Cysharp.Threading.Tasks;
using UnityEngine;

namespace com.amabie.SceneTemplateKit
{
    public class PageBase : MonoBehaviour
    {
        protected virtual void Start()
        {
            Disable().Forget();
        }

        #pragma warning disable CS1998
        protected virtual async UniTask OnEnabled()
        {
        }

        #pragma warning disable CS1998
        protected virtual async UniTask OnDisabled()
        {

        }

        public async UniTask Enable()
        {
            ToggleScene(true);
            await OnEnabled();
        }

        public async UniTask Disable()
        {
            ToggleScene(false);
            await OnDisabled();
        }

        protected void ToggleScene(bool isActive)
        {
            for(var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                child.gameObject.SetActive(isActive);
            }
        }
    }
}