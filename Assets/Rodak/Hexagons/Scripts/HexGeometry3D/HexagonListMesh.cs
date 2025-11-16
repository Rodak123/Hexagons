using System.Collections.Generic;
using Rodak.Hexagons.HexUtils;

namespace Rodak.Hexagons.HexGeometry3D
{
    /**
     * Provides methods for generating combined 3D meshes from a list of Hexagons.
     */
    public static class HexagonListMesh
    {
        /// <summary>
        /// Creates a new MeshBuilder with appended hexagon faces.
        /// </summary>
        /// <param name="hexagonList">Hexagons</param>
        /// <param name="flipped">Wheter to flip the normals</param>
        /// <returns>MeshBuilder with appended hexagon faces</returns>
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