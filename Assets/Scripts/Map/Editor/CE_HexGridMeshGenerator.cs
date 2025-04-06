using UnityEditor;
using UnityEngine;

namespace WinterUniverse
{
    [CustomEditor(typeof(HexGridMeshGenerator))]
    public class CE_HexGridMeshGenerator : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            HexGridMeshGenerator generator = (HexGridMeshGenerator)target;
            if (GUILayout.Button("Generate"))
            {
                generator.CreateHexMesh();
            }
            if (GUILayout.Button("Clear"))
            {
                generator.ClearHexGridMesh();
            }
        }
    }
}