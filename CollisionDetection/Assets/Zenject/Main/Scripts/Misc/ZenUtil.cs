using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Diagnostics;
using UnityEngine;
using ModestTree;

namespace Zenject
{
    public class ZenUtil
    {
        public static void LoadScene(string levelName)
        {
            LoadSceneInternal(levelName, false, null, null);
        }

        public static void LoadScene(string levelName, Action<DiContainer> preBindings)
        {
            LoadSceneInternal(levelName, false, preBindings, null);
        }

        public static void LoadScene(
            string levelName, Action<DiContainer> preBindings, Action<DiContainer> postBindings)
        {
            LoadSceneInternal(levelName, false, preBindings, postBindings);
        }

        public static void LoadSceneAdditive(string levelName)
        {
            LoadSceneInternal(levelName, true, null, null);
        }

        public static void LoadSceneAdditive(string levelName, Action<DiContainer> preBindings)
        {
            LoadSceneInternal(levelName, true, preBindings, null);
        }

        public static void LoadSceneAdditive(
            string levelName, Action<DiContainer> preBindings, Action<DiContainer> postBindings)
        {
            LoadSceneInternal(levelName, true, preBindings, postBindings);
        }

        static void LoadSceneInternal(
            string levelName, bool isAdditive, Action<DiContainer> preBindings, Action<DiContainer> postBindings)
        {
            if (preBindings != null)
            {
                CompositionRoot.BeforeInstallHooks += preBindings;
            }

            if (postBindings != null)
            {
                CompositionRoot.AfterInstallHooks += postBindings;
            }

            if (isAdditive)
            {
                Application.LoadLevelAdditive(levelName);
            }
            else
            {
                Application.LoadLevel(levelName);
            }
        }

        // This method can be used to load the given scene and perform injection on its contents
        // Note that the scene we're loading can have [Inject] flags however it should not have
        // its own composition root
        public static IEnumerator LoadSceneAdditiveWithContainer(
            string levelName, DiContainer parentContainer)
        {
            var rootObjectsBeforeLoad = GameObject.FindObjectsOfType<Transform>().Where(x => x.parent == null).ToList();

            Application.LoadLevelAdditive(levelName);

            // Wait one frame for objects to be added to the scene heirarchy
            yield return null;

            var rootObjectsAfterLoad = GameObject.FindObjectsOfType<Transform>().Where(x => x.parent == null).ToList();

            foreach (var newObject in rootObjectsAfterLoad.Except(rootObjectsBeforeLoad).Select(x => x.gameObject))
            {
                Assert.That(newObject.GetComponent<CompositionRoot>() == null,
                    "LoadSceneAdditiveWithContainer does not expect a container to exist in the loaded scene");

                InjectionHelper.InjectChildGameObjects(parentContainer, newObject);
            }
        }
    }
}
