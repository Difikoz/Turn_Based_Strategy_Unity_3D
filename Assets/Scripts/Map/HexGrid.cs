using UnityEngine;

namespace WinterUniverse
{
    public class HexGrid : Singleton<HexGrid>
    {
        [field: SerializeField] public HexOrientation HexOrientation { get; private set; }
        [field: SerializeField] public int Width { get; private set; }
        [field: SerializeField] public int Height { get; private set; }
        [field: SerializeField] public float HexSize { get; private set; }
        [field: SerializeField] public GameObject HexPrefab { get; private set; }

        private void OnDrawGizmos()
        {
            for (int z = 0; z < Height; z++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Vector3 center = HexMetrics.Center(HexSize, x, z, HexOrientation) + transform.position;
                    for (int s = 0; s < HexMetrics.Corners(HexSize, HexOrientation).Length; s++)
                    {
                        Gizmos.DrawLine(center + HexMetrics.Corners(HexSize, HexOrientation)[s % 6], center + HexMetrics.Corners(HexSize, HexOrientation)[(s + 1) % 6]);
                    }
                }
            }
        }
    }
}