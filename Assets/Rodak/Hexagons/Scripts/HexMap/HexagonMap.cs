using System;

namespace Rodak.Hexagons.HexMap
{
    public class HexagonMap<T> : AHexagonMap<T, HexagonChunk<T>>
    {
        private Func<Hexagon, Hexagon, T> createValue;

        public HexagonMap(
            int chunkSize,
            Func<Hexagon, Hexagon, T> createValue
            ) : base(chunkSize)
        {
            this.createValue = createValue;
        }

        protected override HexagonChunk<T> CreateChunk(Hexagon chunkPosition)
        {
            return new(chunkPosition, ChunkSize, createValue);
        }
    }
}