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
        [SerializeField] private string landingSceneName;
        [SerializeField] private List<SceneBase> scenePrefabList;
        [SerializeField] private List<TransitionBase> transitionPrefabList;
        [SerializeField] private EventSystem eventSystemPrefab;

        private List<SceneBase> sceneList;
        private List<TransitionBase> transitionList; 

        protected new void Start()
        {
            base.Start();
            Instantiate(eventSystemPrefab);
            CreateScene();
            CreateTransition();
        }

        private void CreateScene()
        {
            var root = new GameObject("SceneRoot");
            sceneList = new();
            scenePrefabList.ForEach(scenePrefab => {
                var scene = Instantiate(scenePrefab, root.transform);
                sceneList.Add(scene);
                if (landingSceneName == scenePrefab.name) scene.Enable().Forget();
            });
        }

        private void CreateTransition()
        {
            var root = new GameObject("TransitionRoot");
            transitionList = new();
            transitionPrefabList.ForEach(transitionPrefab => {
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