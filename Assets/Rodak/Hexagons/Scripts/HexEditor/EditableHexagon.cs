using System;

namespace Rodak.Hexagons.HexEditor
{
    [Serializable]
    public class EditableHexagon
    {
        public static implicit operator Hexagon(EditableHexagon editableHexagon) => new(editableHexagon.Q, editableHexagon.R, editableHexagon.S);
        public static implicit operator EditableHexagon(Hexagon hexagon) => new(hexagon.Q, hexagon.R, hexagon.S);

        public int Q;
        public int R;
        public int S;

        public EditableHexagon() : this(0, 0, 0) { }
        public EditableHexagon(int q, int r, int s)
        {
            Q = q;
            R = r;
            S = s;
        }

        public Hexagon ToHexagon()
        {
            return (Hexagon)this;
        }

        public bool IsValid()
        {
            return Q + R + S == 0;
        }

        public override string ToString()
        {
            return $"EditableHexagon[{(Hexagon)this}]";
        }
    }
}