using UnityEditor;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Inspecteur par défaut.
    /// </summary>
    /// <inheritdoc/>
    [CustomEditor(typeof(Object), true)]
    [CanEditMultipleObjects]
    public class ComponentInspector : BaseInspector
    {
        protected override void Initialize()
        {
            //Nothing to do
        }

        protected override void Draw()
        {
            DrawDefault();
        }
    }
}