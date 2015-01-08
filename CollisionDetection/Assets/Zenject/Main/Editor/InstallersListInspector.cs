using System.Collections.Generic;
using System.Linq;
using Zenject;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Zenject
{
    internal class InstallersListInspector : UnityInspectorListEditor
    {
        protected override string[] PropertyNames
        {
            get
            {
                return new[] { "Installers" };
            }
        }

        protected override string[] PropertyDescriptions
        {
            get
            {
                return new[] { "Sorted array of custom installers for your scene" };
            }
        }

        protected override bool DisplayAllProperties
        {
            get
            {
                return false;
            }
        }
    }

    // Unfortunately unity only allows one CustomEditor attribute per class so we use this workaround:
    [CustomEditor(typeof(GlobalInstallerConfig))]
    internal sealed class GlobalInstallerConfigEditor : InstallersListInspector
    {
    }

    [CustomEditor(typeof(CompositionRoot))]
    internal sealed class CompositionRootEditor : InstallersListInspector
    {
        public override void OnInspectorGUI()
        {
            var compRoot = this.target as CompositionRoot;
            compRoot.OnlyInjectWhenActive = EditorGUILayout.Toggle("Only Inject When Active", compRoot.OnlyInjectWhenActive);
            base.OnInspectorGUI();
        }
    }

    [CustomEditor(typeof(SceneDecoratorCompositionRoot))]
    internal sealed class SceneDecoratorCompositionRootEditor : InstallersListInspector
    {
        protected override bool DisplayAllProperties
        {
            get
            {
                return true;
            }
        }
    }
}
