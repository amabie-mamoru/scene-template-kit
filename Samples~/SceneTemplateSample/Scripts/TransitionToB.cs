using com.amabie.SceneTemplateKit;
using Cysharp.Threading.Tasks;

public class TransitionToB : TransitionBase
{
    protected override async UniTask OnEnabled()
    {
        await UniTask.Delay(3000);
        await GoTo<SceneB>();
    }
}
