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

        protected List<SceneBase> sceneList;
        protected List<TransitionBase> transitionList;
        protected bool isInitialized;

        protected new void Start()
        {
            base.Start();
            Instantiate(eventSystemPrefab);
            CreateScene();
            CreateTransition();
            isInitialized = true;
        }

        protected void CreateScene()
        {
            var root = new GameObject("SceneRoot");
            sceneList = new();
            scenePrefabList.ForEach(scenePrefab => {
                var scene = Instantiate(scenePrefab, root.transform);
                sceneList.Add(scene);
                if (landingSceneName == scenePrefab.name) EnableLandingScene(scene).Forget();
            });
        }

        protected async UniTask EnableLandingScene(SceneBase scene)
        {
            await UniTask.WaitUntil(() => isInitialized);
            scene.Enable().Forget();
        }

        protected void CreateTransition()
        {
            var root = new GameObject("TransitionRoot");
            transitionList = new();
            transitionPrefabList.ForEach(transitionPrefab =>
            {
                var transition = Instantiate(transitionPrefab, root.transform);
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