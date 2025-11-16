using System;

namespace Rodak.Hexagons.HexEditor
{
    /// <summary>
    /// A mutable class designed to hold hexagon coordinates for use in editors or inspectors.
    /// It supports implicit conversion to and from the immutable Hexagon class.
    /// </summary>
    [Serializable]
    public class EditableHexagon
    {
        /// <summary>
        /// Allows implicit conversion from EditableHexagon to the immutable Hexagon.
        /// </summary>
        public static implicit operator Hexagon(EditableHexagon editableHexagon) => new(editableHexagon.Q, editableHexagon.R, editableHexagon.S);
        /// <summary>
        /// Allows implicit conversion from the immutable Hexagon to EditableHexagon.
        /// </summary>
        public static implicit operator EditableHexagon(Hexagon hexagon) => new(hexagon.Q, hexagon.R, hexagon.S);

        /// <summary>
        /// Q axis position.
        /// </summary>
        public int Q;
        /// <summary>
        /// R axis position.
        /// </summary>
        public int R;
        /// <summary>
        /// S axis position.
        /// </summary>
        public int S;

        /// <summary>
        /// Initializes a new instance of the EditableHexagon class with coordinates (0, 0, 0).
        /// </summary>
        public EditableHexagon() : this(0, 0, 0) { }
        /// <summary>
        /// Initializes a new instance of the EditableHexagon class with specified coordinates.
        /// </summary>
        /// <param name="q">Q axis position</param>
        /// <param name="r">R axis position</param>
        /// <param name="s">S axis position</param>
        public EditableHexagon(int q, int r, int s)
        {
            Q = q;
            R = r;
            S = s;
        }

        /// <summary>
        /// Converts the editable coordinates to an immutable Hexagon object.
        /// </summary>
        /// <returns>A new Hexagon instance.</returns>
        public Hexagon ToHexagon()
        {
            return (Hexagon)this;
        }

        /// <summary>
        /// Checks if the current coordinates define a valid hexagon (Q + R + S == 0).
        /// </summary>
        /// <returns>True if the hexagon is valid, false otherwise.</returns>
        public bool IsValid()
        {
            return Q + R + S == 0;
        }

        /// <summary>
        /// Returns a string representation of the EditableHexagon.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"EditableHexagon[{(Hexagon)this}]";
        }
    }
}