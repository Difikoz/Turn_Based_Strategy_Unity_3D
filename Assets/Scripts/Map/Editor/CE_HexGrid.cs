using UnityEditor;
using UnityEngine;

namespace WinterUniverse
{
    [CustomEditor(typeof(HexGrid))]
    public class CE_HexGrid : Editor
    {
        private void OnSceneGUI()
        {
            HexGrid hexGrid = (HexGrid)target;
            for (int z = 0; z < hexGrid.Height; z++)
            {
                for (int x = 0; x < hexGrid.Width; x++)
                {
                    Vector3 center = HexMetrics.Center(hexGrid.HexSize, x, z, hexGrid.HexOrientation) + hexGrid.transform.position;
                    int centerX = x;
                    int centerZ = z;
                    Vector3 cubeCoord = HexMetrics.OffsetToCube(centerX, centerZ, hexGrid.HexOrientation);
                    Handles.Label(center + Vector3.forward * 0.5f, $"[{centerX}, {centerZ}]");
                    Handles.Label(center, $"({cubeCoord.x}, {cubeCoord.y}, {cubeCoord.z})");
                }
            }
        }
    }
}