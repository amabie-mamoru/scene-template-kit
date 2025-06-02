using System;
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

        protected List<SceneBase> sceneList;
        protected List<TransitionBase> transitionList;
        protected bool isInitialized;

        protected override void Start()
        {
            base.Start();
            ValidateEventSystem();
            CreateScene();
            CreateTransition();
            isInitialized = true;
        }

        protected void ValidateEventSystem()
        {
            var eventSystem = FindFirstObjectByType<EventSystem>();
            // EventSystem がないとボタンの onClick が反応しないのでチェックする
            if (eventSystem == null)
            {
                throw new SceneHandlerException("EventSystem をシーンに配置してください");
            }
        }

        protected void CreateScene()
        {
            var root = new GameObject("SceneRoot");
            sceneList = new();
            scenePrefabList.ForEach(scenePrefab =>
            {
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

    public class SceneHandlerException : Exception
    {
        public SceneHandlerException(string message) : base(message) { }
    }
}