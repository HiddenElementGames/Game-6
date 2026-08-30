using UnityEngine;
public class Chunk
{
    public Vector2Int index; //(Chunk X value, Chunk Y value)
    public Tile[,] tiles; // [32,32] 32x32 chunk for example.
}
