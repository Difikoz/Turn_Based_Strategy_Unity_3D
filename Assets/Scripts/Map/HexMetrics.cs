using UnityEngine;

namespace WinterUniverse
{
    public static class HexMetrics
    {
        public static float OuterRadius(float hexSize)
        {
            return hexSize;
        }

        public static float InnerRadius(float hexSize)
        {
            return hexSize * 0.866025404f;
        }

        public static Vector3[] Corners(float hexSize, HexOrientation orientation)
        {
            Vector3[] corners = new Vector3[6];
            for (int i = 0; i < 6; i++)
            {
                corners[i] = Corner(hexSize, orientation, i);
            }
            return corners;
        }

        public static Vector3 Corner(float hexSize, HexOrientation orientation, int index)
        {
            float angle = 60f * index;
            if (orientation == HexOrientation.PointyTop)
            {
                angle += 30f;
            }
            Vector3 corner = new(hexSize * Mathf.Cos(angle * Mathf.Deg2Rad), 0f, hexSize * Mathf.Sin(angle * Mathf.Deg2Rad));
            return corner;
        }

        public static Vector3 Center(float hexSize, int x, int z, HexOrientation orientation)
        {
            Vector3 position;
            if (orientation == HexOrientation.PointyTop)
            {
                position.x = (x + z * 0.5f - z / 2) * (InnerRadius(hexSize) * 2f);
                position.y = 0f;
                position.z = z * (OuterRadius(hexSize) * 1.5f);
            }
            else
            {
                position.x = x * (OuterRadius(hexSize) * 1.5f);
                position.y = 0f;
                position.z = (z + x * 0.5f - x / 2) * (InnerRadius(hexSize) * 2f);
            }
            return position;
        }

        public static Vector2 CubeToAxial(Vector3 cube)
        {
            return new(cube.x, cube.y);
        }

        public static Vector2 OffsetToAxial(int x, int z, HexOrientation hexOrientation)
        {
            if (hexOrientation == HexOrientation.PointyTop)
            {
                return OffsetToAxialPointy(x, z);
            }
            else
            {
                return OffsetToAxialFlat(x, z);
            }
        }

        public static Vector2Int OffsetToAxialPointy(int x, int z)
        {
            return new Vector2Int(x - (z - (z & 1)) / 2, z);
        }

        public static Vector2Int OffsetToAxialFlat(int x, int z)
        {
            return new Vector2Int(x, z - (x - (x & 1)) / 2);
        }

        public static Vector2 CubeToOffset(Vector3 cube, HexOrientation hexOrientation)
        {
            return CubeToOffset((int)cube.x, (int)cube.y, (int)cube.z, hexOrientation);
        }

        public static Vector2 CubeToOffset(int x, int y, int z, HexOrientation hexOrientation)
        {
            if (hexOrientation == HexOrientation.PointyTop)
            {
                return CubeToOffsetPointy(x, y, z);
            }
            else
            {
                return CubeToOffsetFlat(x, y, z);
            }
        }

        public static Vector2 CubeToOffsetPointy(int x, int y, int z)
        {
            return new(x + (y - (y & 1)) / 2, y);
        }

        public static Vector2 CubeToOffsetFlat(int x, int y, int z)
        {
            return new(x, y + (x - (x & 1)) / 2);
        }

        public static Vector3 CubeRound(Vector3 cube)
        {
            int rX = Mathf.RoundToInt(cube.x);
            int rY = Mathf.RoundToInt(cube.y);
            int rZ = Mathf.RoundToInt(cube.z);
            float xDiff = Mathf.Abs(rX - cube.x);
            float yDiff = Mathf.Abs(rY - cube.y);
            float zDiff = Mathf.Abs(rZ - cube.z);
            if (xDiff > yDiff && xDiff > zDiff)
            {
                rX = -rY - rZ;
            }
            else if (yDiff > zDiff)
            {
                rY = -rX - rZ;
            }
            else
            {
                rZ = -rX - rY;
            }
            return new(rX, rY, rZ);
        }

        public static Vector2 AxialRound(Vector2 axial)
        {
            return CubeToAxial(CubeRound(AxialToCube(axial.x, axial.y)));
        }

        public static Vector2 CoordinateToAxial(float x, float z, float hexSize, HexOrientation hexOrientation)
        {
            if (hexOrientation == HexOrientation.PointyTop)
            {
                return CoordinateToAxialPointy(x, z, hexSize);
            }
            else
            {
                return CoordinateToAxialFlat(x, z, hexSize);
            }
        }

        private static Vector2 CoordinateToAxialPointy(float x, float z, float hexSize)
        {
            Vector2 hexCoordinates = new()
            {
                x = (Mathf.Sqrt(3) / 3 * x - 1f / 3 * z) / hexSize,
                y = (2f / 3 * z) / hexSize
            };
            return AxialRound(hexCoordinates);
        }

        private static Vector2 CoordinateToAxialFlat(float x, float z, float hexSize)
        {
            Vector2 hexCoordinates = new()
            {
                x = (2f / 3 * x) / hexSize,
                y = (-1f / 3 * x + Mathf.Sqrt(3) / 3 * z) / hexSize
            };
            return AxialRound(hexCoordinates);
        }

        public static Vector2 CoordinateToOffset(float x, float z, float hexSize, HexOrientation hexOrientation)
        {
            return CubeToOffset(AxialToCube(CoordinateToAxial(x, z, hexSize, hexOrientation)), hexOrientation);
        }
        //
        public static Vector3 OffsetToCube(int x, int z, HexOrientation hexOrientation)
        {
            if (hexOrientation == HexOrientation.PointyTop)
            {
                return AxialToCube(OffsetToAxialPointy(x, z));
            }
            else
            {
                return AxialToCube(OffsetToAxialFlat(x, z));
            }
        }

        public static Vector3 AxialToCube(float x, float y)
        {
            return new Vector3(x, y, -x - y);
        }

        public static Vector3 AxialToCube(Vector2 axial)
        {
            return AxialToCube(axial.x, axial.y);
        }
    }
}