using UnityEngine;

namespace WinterUniverse
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class HexGridMeshGenerator : MonoBehaviour
    {
        [field: SerializeField] public HexGrid HexGrid { get; private set; }
        [field: SerializeField] public MeshFilter MeshFilter { get; private set; }
        [field: SerializeField] public MeshRenderer MeshRenderer { get; private set; }
        [field: SerializeField] public MeshCollider MeshCollider { get; private set; }
        [field: SerializeField] public Material GridMaterial { get; private set; }
        [field: SerializeField] public LayerMask GridLayer { get; private set; }

        private void OnValidate()
        {
            HexGrid = GetComponent<HexGrid>();
            MeshFilter = GetComponent<MeshFilter>();
            MeshRenderer = GetComponent<MeshRenderer>();
            MeshCollider = GetComponent<MeshCollider>();
        }

        private void Awake()
        {
            HexGrid = GetComponent<HexGrid>();
            MeshFilter = GetComponent<MeshFilter>();
            MeshRenderer = GetComponent<MeshRenderer>();
            MeshCollider = GetComponent<MeshCollider>();
        }

        public void CreateHexMesh()
        {
            CreateHexMesh(HexGrid.Width, HexGrid.Height, HexGrid.HexSize, HexGrid.HexOrientation, GridLayer);
        }

        public void ClearHexGridMesh()
        {
            if (MeshFilter.sharedMesh == null)
            {
                return;
            }
            MeshFilter.sharedMesh.Clear();
            MeshCollider.sharedMesh.Clear();
        }

        public void CreateHexMesh(int width, int height, float hexSize, HexOrientation hexOrientation, LayerMask gridLayer)
        {
            ClearHexGridMesh();
            Vector3[] vertices = new Vector3[7 * width * height];
            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector3 center = HexMetrics.Center(hexSize, x, z, hexOrientation);
                    vertices[(z * width + x) * 7] = center;
                    for (int s = 0; s < HexMetrics.Corners(hexSize, hexOrientation).Length; s++)
                    {
                        vertices[(z * width + x) * 7 + s + 1] = center + HexMetrics.Corners(hexSize, hexOrientation)[s % 6];
                    }
                }
            }
            int[] triangles = new int[3 * 6 * width * height];
            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int s = 0; s < HexMetrics.Corners(hexSize, hexOrientation).Length; s++)
                    {
                        int cornerIndex = s + 2 > 6 ? s + 2 - 6 : s + 2;
                        triangles[3 * 6 * (z * width + x) + s * 3 + 0] = (z * width + x) * 7;
                        triangles[3 * 6 * (z * width + x) + s * 3 + 1] = (z * width + x) * 7 + s + 1;
                        triangles[3 * 6 * (z * width + x) + s * 3 + 2] = (z * width + x) * 7 + cornerIndex;
                    }
                }
            }
            Mesh mesh = new()
            {
                name = "Hex Mesh",
                vertices = vertices,
                triangles = triangles,
            };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.Optimize();
            mesh.RecalculateUVDistributionMetrics();
            MeshFilter.sharedMesh = mesh;
            MeshCollider.sharedMesh = mesh;
            MeshRenderer.material = GridMaterial;
            int gridLayerIndex = GetLayerIndex(gridLayer);
            gameObject.layer = gridLayerIndex;
        }

        private int GetLayerIndex(LayerMask gridLayer)
        {
            int layerMaskValue = gridLayer.value;
            for (int i = 0; i < 32; i++)
            {
                if (((1 << i) & layerMaskValue) != 0)
                {
                    return i;
                }
            }
            return 0;
        }
    }
}