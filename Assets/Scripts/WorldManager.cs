
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class WorldManager : MonoBehaviour 
{
    /* this will hold these things...
    [x] world coordinate understanding
    [ ] chunk gen
    [x] chunk loading/unloading
    [ ] tile data storage
    [ ] terain gen
    [ ] city spawning
    [ ] pathing access rules
    [ ] danger value updater
    */

    public static WorldManager Instance;

    public int chunkSize = 32;
    public float tileSize = 1.0f;
    public int loadRadius = 2; //number of chunks to keep loaded
    public IReadOnlyDictionary<Vector2Int, Chunk> Chunks => chunks; // allows read only access to the current dict of created chunks.

    private Dictionary<Vector2Int, Chunk> chunks = new(); //"World" -> chunk Dictionary.

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        LoadChunks(Vector2Int.zero);
    }    


    //World Coordinate Conversion Manager
    public Vector2Int WorldToTile(Vector2 worldPos)
    {
        int x = Mathf.FloorToInt(worldPos.x / tileSize);
        int y = Mathf.FloorToInt(worldPos.y / tileSize);
        return new Vector2Int(x, y); //returns tile coordinate at worldPos.
    } //return the tile at worldPos

    public Vector2Int TileToChunk(Vector2Int tilePos) //find which chunk a specific tile belongs to.
    {
        int chunkX = Mathf.FloorToInt((float)tilePos.x / chunkSize); //typecast to avoid trunkation
        int chunkY = Mathf.FloorToInt((float)tilePos.y / chunkSize);
        return new Vector2Int(chunkX, chunkY); //return chunk coordinate which contain tilePos.
    }

    public Vector2Int WorldToChunk(Vector2 worldPos) //"shortcut" function for code clarity.
    {
        return TileToChunk(WorldToTile(worldPos));
    }
    //End World Coordinate Conversion Manager


    //ChunkLoader
    private void LoadChunks(Vector2Int centerChunk)
    {
        for (int x = -loadRadius; x <= loadRadius; x++)
        {
            for (int y = -loadRadius; y <= loadRadius; y++) //yeahyeahyeah I know, O(n^2). Feel free to optimize.
            {
                Vector2Int index = centerChunk + new Vector2Int(x, y);

                if (!chunks.ContainsKey(index))//check if we need to generate the chunk.
                {
                    GenerateChunk(index); //create it if we need to.
                }
            }
        }
    }

    //centerChunk will come from a WorldToChunk in an Update(), followed by a LoadChunks();.

    private void UnloadChunks(Vector2Int centerChunk)
    {
        List<Vector2Int> chunksToRemove = new(); //Define new remove Dict.

        foreach (var kvp in chunks)
        {
            Vector2Int index = kvp.Key;
            int distanceFromCenter = Mathf.Abs(index.x - centerChunk.x) + Mathf.Abs(index.y - centerChunk.y);

            if (distanceFromCenter > loadRadius + 1)
            {
                chunksToRemove.Add(index); //add the chunk to the removal list
            }
        }

        foreach (var index in chunksToRemove)
        {
            chunks.Remove(index); //far away chunks are unloaded.
        }
    }
    //End ChunkLoader

    //Chunk Generator
    private void GenerateChunk(Vector2Int index)
    {
        Chunk chunk = new Chunk();
        chunk.index = index;
        chunk.tiles = new Tile[chunkSize, chunkSize]; //define the square chunk boundaries.

        //placeholder generation setup
        for (int x = 0; x < chunkSize; x++) //generate "random" tile for each gridspace. 
        {
            for (int y = 0; y < chunkSize; y++)
            {
                chunk.tiles[x, y] = new Tile
                {
                    tileType = TileType.Grass,
                    tileDanger = 0,
                    moveable = true,
                    isCity = false
                };
            }
        }

        chunks[index] = chunk;
        //Debug.Log("GenChunK: " + index);
    }
    //End ChunkGenerator

}