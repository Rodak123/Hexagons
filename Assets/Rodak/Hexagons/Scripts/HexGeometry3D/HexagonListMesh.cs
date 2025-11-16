using System.Collections.Generic;
using Rodak.Hexagons.HexUtils;

namespace Rodak.Hexagons.HexGeometry3D
{
    public static class HexagonListMesh
    {
        public static MeshBuilder GetMesh(this List<Hexagon> hexagonList, bool flipped)
        {
            MeshBuilder meshBuillder = new();

            for (int i = 0; i < hexagonList.Count; i++)
            {
                Hexagon hexagon = hexagonList[i];
                MeshBuilder hexagonMesh = hexagon.GetMesh(flipped);

                meshBuillder.Append(hexagonMesh);
            }

            return meshBuillder;
        }
    }

}