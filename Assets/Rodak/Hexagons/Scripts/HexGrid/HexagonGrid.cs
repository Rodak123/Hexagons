using System;
using System.Collections.Generic;
using System.Linq;

namespace Rodak.Hexagons.HexGrid
{
    /// <summary>
    /// Defines a grid of hexagons, using cubic coordinates (i, j, k where i+j+k=0),
    /// that stores a value of type T for each hexagon.
    /// The grid shape is a rhombus (or diamond) centered at (0, 0).
    /// </summary>
    /// <typeparam name="T">The type of value stored in each hexagon cell.</typeparam>
    public class HexagonGrid<T>
    {
        private readonly Dictionary<Hexagon, T> values = new();

        /// <summary>
        /// Gets a list of all <see cref="Hexagon"/> positions currently in the grid.
        /// </summary>
        public List<Hexagon> Hexagons => values.Keys.ToList();

        /// <summary>
        /// Gets the radial size of the grid, which is the maximum coordinate magnitude
        /// from the center (0, 0). A size of 0 is one hexagon; a size of 1 is 7 hexagons.
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// Gets the number of hexagons across the longest dimension (center to opposite edge).
        /// This is calculated as (Size * 2) + 1.
        /// </summary>
        public int SizeAcross => Size * 2 + 1;

        /// <summary>
        /// Gets the value associated with a specific <see cref="Hexagon"/> position.
        /// </summary>
        /// <returns>The value.</returns>
        /// <exception cref="IndexOutOfRangeException">When the <see cref="Hexagon"/> position is not in this grid.</exception>
        public T this[Hexagon position]
        {
            get
            {
                if (TryGetValue(position, out T value))
                    return value;
                throw new IndexOutOfRangeException($"{position} is not in this grid");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HexagonGrid{T}"/> class.
        /// The grid is generated as a rhombus of a given size centered at (0, 0).
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if <paramref name="size"/> is negative.</exception>
        public HexagonGrid(int size, Func<Hexagon, T> createValue)
        {
            if (size < 0) throw new ArgumentException($"{nameof(size)} of a {nameof(HexagonGrid<T>)} must be positive");
            Size = size;

            for (int i = -Size; i <= Size; i++)
            {
                for (int j = -Size; j <= Size; j++)
                {
                    // This condition ensures the generated shape is a rhombus centered at (0,0,0)
                    // The implicit k coordinate is -(i + j). The condition checks if |k| <= Size.
                    if (i + j > Size || i + j < -Size) continue;
                    Hexagon position = new(i, j);
                    values.Add(position, createValue(position));
                }
            }
        }

        /// <summary>
        /// Executes an action for every hexagon and its associated value in the grid.
        /// </summary>
        public void ForEach(Action<Hexagon, T> action)
        {
            Hexagons.ForEach((Hexagon position) =>
            {
                action(position, values[position]);
            });
        }

        /// <summary>
        /// Checks if a specific <see cref="Hexagon"/> position is contained within the grid bounds.
        /// </summary>
        /// <returns><c>true</c> if the position is in the grid; otherwise, <c>false</c>.</returns>
        public bool ContainsHexagon(Hexagon position)
        {
            return values.ContainsKey(position);
        }

        /// <summary>
        /// Attempts to retrieve the value associated with a specific <see cref="Hexagon"/> position.
        /// </summary>
        /// <returns><c>true</c> if the grid contains the position; otherwise, <c>false</c>.</returns>
        public bool TryGetValue(Hexagon position, out T value)
        {
            return values.TryGetValue(position, out value);
        }

        /// <summary>
        /// Attempts to set a new value for a specific <see cref="Hexagon"/> position.
        /// </summary>
        /// <returns><c>true</c> if the position exists and was updated; otherwise, <c>false</c>.</returns>
        public bool SetValue(Hexagon position, T value)
        {
            if (!values.ContainsKey(position))
                return false;
            values[position] = value;
            return true;
        }

        /// <summary>
        /// Returns a string that represents the current grid.
        /// </summary>
        /// <returns>A string in the format "HexGrid[Size, Type]".</returns>
        public override string ToString()
        {
            return $"HexGrid[{Size}, {typeof(T)}]";
        }
    }
}