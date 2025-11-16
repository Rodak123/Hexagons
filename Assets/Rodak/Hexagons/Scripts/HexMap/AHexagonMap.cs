using System;
using System.Collections.Generic;
using System.Linq;

namespace Rodak.Hexagons.HexMap
{
    /// <summary>
    /// Base class for a map that manages hexagon tiles organized into chunks.
    /// This allows for maps that are potentially infinite or very large.
    /// </summary>
    /// <typeparam name="TTile">The type of value stored in each hexagon tile.</typeparam>
    /// <typeparam name="GChunk">The type of the hexagon chunk, which must inherit from HexagonChunk of type TTile.</typeparam>
    public abstract class AHexagonMap<TTile, GChunk> where GChunk : HexagonChunk<TTile>
    {
        private readonly Dictionary<Hexagon, GChunk> chunks = new();
        /// <summary>
        /// The size parameter used to define the dimensions of each chunk.
        /// </summary>
        public readonly int ChunkSize;

        /// <summary>
        /// Initializes a new instance of the AHexagonMap class.
        /// </summary>
        /// <param name="chunkSize">The size parameter for chunk creation.</param>
        public AHexagonMap(int chunkSize)
        {
            ChunkSize = chunkSize;
        }

        /// <summary>
        /// Creates a new chunk instance at a specified position.
        /// </summary>
        /// <param name="chunkPosition">The Hexagon coordinate of the chunk's center.</param>
        /// <returns>A new chunk object.</returns>
        protected abstract GChunk CreateChunk(Hexagon chunkPosition);

        /// <summary>
        /// Gets a list of the Hexagon coordinates for all loaded chunks.
        /// </summary>
        public List<Hexagon> ChunkHexagons => chunks.Keys.ToList();

        /// <summary>
        /// Adds a chunk to the map's dictionary.
        /// </summary>
        /// <param name="chunkPosition">The Hexagon coordinate of the chunk.</param>
        /// <param name="chunk">The chunk object to add.</param>
        protected void AddChunk(Hexagon chunkPosition, GChunk chunk)
        {
            chunks.Add(chunkPosition, chunk);
        }

        /// <summary>
        /// Performs an action on every loaded chunk in the map.
        /// </summary>
        /// <param name="action">The action to perform on each chunk.</param>
        public void ForEach(Action<GChunk> action)
        {
            ChunkHexagons.ForEach((Hexagon chunkPosition) =>
            {
                action(chunks[chunkPosition]);
            });
        }

        /// <summary>
        /// Checks if a chunk at the specified position is currently loaded.
        /// </summary>
        /// <param name="chunkPosition">The Hexagon coordinate of the chunk.</param>
        /// <returns>True if the chunk is loaded, false otherwise.</returns>
        public bool HasChunk(Hexagon chunkPosition)
        {
            return chunks.ContainsKey(chunkPosition);
        }

        /// <summary>
        /// Gets the chunk at the specified position, creating and loading it if it does not exist.
        /// </summary>
        /// <param name="chunkPosition">The Hexagon coordinate of the chunk.</param>
        /// <param name="wasCreated">Output parameter: true if the chunk was created, false if it already existed.</param>
        /// <returns>The existing or newly created chunk.</returns>
        public GChunk GetOrCreateChunk(Hexagon chunkPosition, out bool wasCreated)
        {
            if (HasChunk(chunkPosition))
            {
                wasCreated = false;
                return chunks[chunkPosition];
            }

            wasCreated = true;
            GChunk chunk = CreateChunk(chunkPosition);
            AddChunk(chunkPosition, chunk);
            return chunk;
        }

        /// <summary>
        /// Attempts to retrieve a loaded chunk at the specified position.
        /// </summary>
        /// <param name="chunkPosition">The Hexagon coordinate of the chunk.</param>
        /// <param name="chunk">Output parameter: the chunk if found, otherwise default.</param>
        /// <returns>True if the chunk was found, false otherwise.</returns>
        public bool TryGetChunk(Hexagon chunkPosition, out GChunk chunk)
        {
            if (!HasChunk(chunkPosition))
            {
                chunk = default;
                return false;
            }

            chunk = chunks[chunkPosition];
            return true;
        }

        /// <summary>
        /// Calculates the Hexagon coordinate of the chunk that contains a given hexagon position.
        /// </summary>
        /// <param name="hexagonPosition">The Hexagon coordinate of the tile.</param>
        /// <returns>The Hexagon coordinate of the containing chunk.</returns>
        public Hexagon GetChunkPosition(Hexagon hexagonPosition)
        {
            int sizeAcross = HexagonChunk.GetSizeAcross(ChunkSize);

            Hexagon nearestChunkPosition = hexagonPosition / sizeAcross;

            Hexagon[] possibleChunkOffsets = { Hexagon.Zero, Hexagon.QAxis, -Hexagon.QAxis, Hexagon.RAxis, -Hexagon.RAxis, Hexagon.SAxis, -Hexagon.SAxis };
            foreach (Hexagon chunkOffset in possibleChunkOffsets)
            {
                Hexagon chunkPosition = nearestChunkPosition + chunkOffset;
                Hexagon chunkCenter = chunkPosition * sizeAcross;
                for (int i = -ChunkSize; i <= ChunkSize; i++)
                {
                    for (int j = -ChunkSize; j <= ChunkSize; j++)
                    {
                        Hexagon currentHexagonPosition = chunkCenter + new Hexagon(i, j);
                        if (currentHexagonPosition == hexagonPosition)
                            return chunkPosition;
                    }
                }
            }

            return nearestChunkPosition;
        }

        /// <summary>
        /// Attempts to retrieve the chunk that contains the specified hexagon position.
        /// </summary>
        /// <param name="hexagonPosition">The Hexagon coordinate of the tile.</param>
        /// <param name="chunk">Output parameter: the chunk if found, otherwise default.</param>
        /// <returns>True if the containing chunk was found, false otherwise.</returns>
        public bool TryGetHexagonsChunk(Hexagon hexagonPosition, out GChunk chunk)
        {
            Hexagon chunkPosition = GetChunkPosition(hexagonPosition);
            return TryGetChunk(chunkPosition, out chunk);
        }
    }
}