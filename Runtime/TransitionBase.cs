using Cysharp.Threading.Tasks;

namespace com.amabie.SceneTemplateKit
{
    public class TransitionBase : PageBase
    {
        protected async UniTask GoTo<T>() where T : SceneBase
        {
            var scene = SceneHandler.Instance.GetScene<T>();
            await scene.Enable();
            SceneHandler.Instance.ChangeScene(scene);
            Disable().Forget();
        }
    }
}