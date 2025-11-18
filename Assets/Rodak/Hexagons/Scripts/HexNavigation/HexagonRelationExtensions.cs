
using System.Collections.Generic;
using UnityEngine.InputSystem.Utilities;

namespace Rodak.Hexagons.HexNavigation
{
    /// <summary>
    ///  Extends the hexagon class with many relation related functions.
    /// </summary>
    public static class HexagonRelationExtensions
    {
        /// <summary>
        /// Neighboring hexagons to the origin hexagon.
        /// </summary>
        public static readonly ReadOnlyArray<Hexagon> Neighbours = new Hexagon[] {
            new(1, 0, -1),
            new(0, 1, -1),
            new(-1, 1, 0),
            new(-1, 0, 1),
            new(0, -1, 1),
            new(1, -1, 0)
        };

        /// <summary>
        /// Diagonal hexagons to the origin hexagon.
        /// </summary>
        public static readonly ReadOnlyArray<Hexagon> Diagonals = new Hexagon[] {
            new(1, 1, -2),
            new(-1, 2, -1),
            new(-2, 1, 1),
            new(-1, -1, 2),
            new(1, -2, 1),
            new(2, -1, -1)
        };

        /// <summary>
        /// Calculates the neighbors to the hexagon.
        /// </summary>
        /// <returns>All neighbors.</returns>
        public static List<Hexagon> GetNeighbors(this Hexagon hexagon)
        {
            List<Hexagon> neighbors = new();
            foreach (Hexagon direction in Neighbours)
                neighbors.Add(hexagon + direction);
            return neighbors;
        }

        /// <summary>
        /// Calculates the diagonals to the hexagon.
        /// </summary>
        /// <returns>All diagonals.</returns>
        public static List<Hexagon> GetDiagonals(this Hexagon hexagon)
        {
            List<Hexagon> neighbors = new();
            foreach (Hexagon direction in Diagonals)
                neighbors.Add(hexagon + direction);
            return neighbors;
        }
    }
}