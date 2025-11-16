using System;
using System.Collections.Generic;
using System.Linq;

namespace Rodak.Hexagons.HexMap
{
    public abstract class AHexagonMap<TTile, GChunk> where GChunk : HexagonChunk<TTile>
    {
        private readonly Dictionary<Hexagon, GChunk> chunks = new();
        public readonly int ChunkSize;

        public AHexagonMap(int chunkSize)
        {
            ChunkSize = chunkSize;
        }

        protected abstract GChunk CreateChunk(Hexagon chunkPosition);

        public List<Hexagon> ChunkHexagons => chunks.Keys.ToList();

        protected void AddChunk(Hexagon chunkPosition, GChunk chunk)
        {
            chunks.Add(chunkPosition, chunk);
        }

        public void ForEach(Action<GChunk> action)
        {
            ChunkHexagons.ForEach((Hexagon chunkPosition) =>
            {
                action(chunks[chunkPosition]);
            });
        }

        public bool HasChunk(Hexagon chunkPosition)
        {
            return chunks.ContainsKey(chunkPosition);
        }

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

        public bool TryGetHexagonsChunk(Hexagon hexagonPosition, out GChunk chunk)
        {
            Hexagon chunkPosition = GetChunkPosition(hexagonPosition);
            return TryGetChunk(chunkPosition, out chunk);
        }
    }
}