using System.Collections.Generic;
using System.Linq;
using com.amabie.SingletonKit;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace com.amabie.SceneTemplateKit
{
    public class SceneHandler : SingletonMonoBehaviour<SceneHandler>
    {
        [SerializeField] protected string landingSceneName;
        [SerializeField] protected List<SceneBase> scenePrefabList;
        [SerializeField] protected List<TransitionBase> transitionPrefabList;
        [SerializeField] protected EventSystem eventSystemPrefab;
        [Header("自動初期化(default: true)。false にする場合は Initialize() を実施してください。")]
        [Tooltip("そのプロパティの真意：VContainer など Instantiate に独自の実装が必要な場合、false にして Initialize の引数で LifetimeScope.Instantiate() を実装することでシングルトンを維持しつつ、エラーなしにコンパイルできるようにするため")]
        [SerializeField] protected bool autoInitialize = true;

        protected List<SceneBase> sceneList;
        protected List<TransitionBase> transitionList;
        protected bool isInitialized;

        protected IGameObjectFactory _factory;

        protected override void Start()
        {
            base.Start();
            if (autoInitialize) Initialize();
        }

        /// <summary>
        /// VContainer などのように Instantiate は違うメソッドで実施しないとエラーになる場合をフォローするためのパブリックメソッド
        /// 基本は autoInitialize は true のままとして、このメソッドは実行しない。
        /// 以下のクラスは VContainer を使う側で実装してほしい。なぜなら、 scene-template-kit には VContainer が入っていないから。
        /// scene-template-kit 側に VContainer を入れると関係ないプロジェクトで VContainer を要求してしまうので。
        /// 
        /// <code>
        /// public class VContainerGameObjectFactory : IGameObjectFactory
        /// {
        ///     public T Instantiate<T>(T prefab, Transform parent = null) where T : UnityEngine.Object
        ///     {
        ///         return  LifetimeScope.Instantiate(prefab, parent);
        ///     }
        /// }
        /// </code>
        /// </summary>
        public void Initialize(IGameObjectFactory factory = null)
        {
            _factory = factory == null ? new DefaultGameObjectFactory() : factory;
            Instantiate(eventSystemPrefab);
            CreateScene();
            CreateTransition();
            isInitialized = true;
        }

        protected void CreateScene()
        {
            var root = new GameObject("SceneRoot");
            sceneList = new();
            scenePrefabList.ForEach(scenePrefab =>
            {
                var scene = _factory.Instantiate(scenePrefab, root.transform);
                sceneList.Add(scene);
                if (landingSceneName == scenePrefab.name) EnableLandingScene(scene).Forget();
            });
        }

        protected async UniTask EnableLandingScene(SceneBase scene)
        {
            // autoInitialize 出ない場合は無限ループするのを回避するため
            if (autoInitialize)
            {
                await UniTask.WaitUntil(() => isInitialized);
            }
            scene.Enable().Forget();
        }

        protected void CreateTransition()
        {
            var root = new GameObject("TransitionRoot");
            transitionList = new();
            transitionPrefabList.ForEach(transitionPrefab =>
            {
                var transition = _factory.Instantiate(transitionPrefab, root.transform);
                transitionList.Add(transition);
            });
        }

        public T GetScene<T>()
        {
            return sceneList.FirstOrDefault(scene => scene is T).GetComponent<T>();
        }

        public T GetTransition<T>()
        {
            return transitionList.FirstOrDefault(transition => transition is T).GetComponent<T>();
        }
    }
}