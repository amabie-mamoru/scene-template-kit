using com.amabie.SceneTemplateKit;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SceneB : SceneBase
{
    [SerializeField] private Button button;

    protected override void Start()
    {
        base.Start();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        GoTo<TransitionToA>().Forget();
    }
}
