using com.amabie.SceneTemplateKit;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SceneB : SceneBase
{
    [SerializeField] private Button button;

    protected new void Awake()
    {
        base.Awake();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        GoTo<TransitionToA>().Forget();
    }
}
