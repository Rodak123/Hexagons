namespace Rodak.Hexagons.Demo.MapMesh
{
    public class MapTile
    {
        public int Value { get; private set; }

        public MapTile(int value)
        {
            Value = value;
        }

        public float GetHeight(float stepHeight) => (Value + 1) * stepHeight;
    }
}