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

        protected Dictionary<string, SceneBase> sceneDict = new();
        protected Dictionary<string, TransitionBase> transitionDict = new();
        protected bool isInitialized;
        public bool IsInitialized => isInitialized;

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
            scenePrefabList.ForEach(scenePrefab =>
            {
                var scene = Instantiate(scenePrefab, root.transform);
                sceneDict.Add(scene.name, scene);
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
            transitionPrefabList.ForEach(transitionPrefab =>
            {
                var transition = Instantiate(transitionPrefab, root.transform);
                transitionDict.Add(transition.name, transition);
            });
        }

        public T GetScene<T>()
        {
            return sceneDict.Values.FirstOrDefault(scene => scene is T).GetComponent<T>();
        }

        public T GetTransition<T>()
        {
            return transitionDict.Values.FirstOrDefault(transition => transition is T).GetComponent<T>();
        }

        public SceneBase GetScene(string name)
        {
            return sceneDict.FirstOrDefault(kvp => kvp.Key == name).Value;
        }

        public TransitionBase GetTransition(string name)
        {
            return transitionDict.FirstOrDefault(kvp => kvp.Key == name).Value;
        }
    }

    public class SceneHandlerException : Exception
    {
        public SceneHandlerException(string message) : base(message) { }
    }
}