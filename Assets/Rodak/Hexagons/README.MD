# Just Hexagons

A bundle of scripts that help with doing stuff with hexagons

Author: [Radek Titěra](https://github.com/Rodak123)

## Todo

- Ring
- Movement range
- Line drawing
- Reflection

## Contents

- [Just Hexagons](#just-hexagons)
  - [Todo](#todo)
  - [Contents](#contents)
  - [Hexagon](#hexagon)
  - [Editor](#editor)
  - [Geometry](#geometry)
  - [Geometry 3D](#geometry-3d)
  - [Grid](#grid)
  - [Map](#map)
  - [Binary](#binary)

## Hexagon

The `Hexagon` class defines an immutable hexagon definition. It uses **axial (Q, R) coordinates** for storage, with the third coordinate **S** automatically calculated: **Q + R + S = 0**.

This class includes methods for:

- **Distance calculation** between two hexagons.
- **Rounding** floating-point coordinates to the nearest valid integer hexagon.
- **Vector operations** (addition, subtraction) and **scalar operations** (multiplication, division) with rounding/flooring options.

---

## Editor

The `EditableHexagon` class is a **mutable** version of the `Hexagon` struct. It is marked as `[Serializable]` to work in the Unity Inspector.

- It has public fields **Q**, **R**, and **S**.
- It supports **implicit conversion** to and from the immutable `Hexagon` struct.
- It includes an `IsValid()` method to check if the coordinates satisfy the $\text{Q} + \text{R} + \text{S} = 0$ constraint.
- It automatically ensures a valid hexagon

---

## Geometry

The `HexagonGeometry` static class provides **2D geometry calculations** for hexagons based on their coordinates.

Key functions include:

- Calculating the **center position** (`Vector2`) of a hexagon.
- Getting the **corner** or **side center position** (`Vector2`) of a hexagon given an index.
- Converting a **2D point** to its corresponding **Hexagon coordinate**.
- Calculating the **angle** and **direction** of corners and the hexagon itself.
- Methods for **rotating** a hexagon coordinate.

---

## Geometry 3D

The `HexagonGeometry3D` static class extends geometry functions into **3D space** using a `PlacementPlane` for projection.

Key functions include:

- Calculating the **center**, **corner**, and **side center position** as `Vector3` based on a specified `PlacementPlane`.
- Converting a **3D point** (`Vector3`) to its corresponding **Hexagon coordinate**.

The `HexagonMesh` static class provides methods to generate a **Unity Mesh** for a single hexagon or a prism structure.

---

## Grid

This section deals with data structures that store hexagon-based data.

The `HexagonGrid<T>` class is a simple data structure that stores values of type **T** mapped to a collection of **Hexagon coordinates**.

---

## Map

The `AHexagonMap<T, GChunk>` abstract class and its implementation, `HexagonMap<T>`, provide a framework for a large or **infinite hexagon map** by organizing tiles into manageable **chunks**.

The `HexagonChunk<T>` class represents a fixed-size group of **Hexagon tiles** that are generated and managed together.

Key concepts:

- **Chunking:** Tiles are grouped into chunks based on a `ChunkSize`.
- **Lazy Loading:** Provides methods to get a chunk, creating it only if it doesn't already exist.

---

## Binary

The `BinaryHexagon` static class provides **extension methods** for C#'s `BinaryReader` and `BinaryWriter`.

Key functions include:

- `Write(this BinaryWriter bw, Hexagon hexagon)`: Serializes a Hexagon's Q and R coordinates.
- `ReadHexagon(this BinaryReader br)`: Deserializes a Hexagon from binary data.
