using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

public class ChunkRenderer : MonoBehaviour
{
    public Mesh tileMesh;
    public Material tileMaterial;
    public float tileSize = 1f;
    public float borderThickness = 0.05f; //5% of the total tile width, total possible range 0-0.45 defined in builder.



    private WorldManager world;


    private void Awake()
    {
        //test mesh for when the custom mesh fails.
        //tileMesh = Resources.GetBuiltinResource<Mesh>("Quad.fbx");

        //Build the tile mesh with border width for renderer.
        tileMesh = TileMeshBuilder.BuildTileMesh(borderThickness);
        //Debug.Log($"TileMesh verts: {tileMesh.vertexCount}, triangles: {tileMesh.triangles.Length}");
    
    }

    void Start()
    {
        world = WorldManager.Instance; //create a new world manager on program start.
    }


    void Update()
    {
        if(world==null)
        {
            Debug.LogError("WorldManager instance is NULL.");
            return;
        }

        foreach (var kvp in world.Chunks)
        {
            RenderChunk(kvp.Value);
        }

        //Debug.Log("CL: "+ world.Chunks.Count);
    }


    private void RenderChunk(Chunk chunk)
    {
        int size = world.chunkSize;
        float tileSize = world.tileSize;

        var matrices = new List<Matrix4x4>();
        //data structure to contain: position, rotation, and scale of individual tile in each chunk). 

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++) //YEAH YEAH YEAH, I KNOW. AGAIN. O(n^2). Fix it if you want it different.
            {
                //determine the transform required to place to tile where it needs to go, find the world position.
                Vector3 pos = new Vector3(
                    (chunk.index.x * size + x) * tileSize,
                    (chunk.index.y * size + y) * tileSize,
                    0); //flat, 2D plane. no z.

                Matrix4x4 matrix = Matrix4x4.TRS( //create a transform matrix
                    pos,
                    Quaternion.identity, //no rotation
                    Vector3.one * tileSize); //scale

                matrices.Add(matrix);
            }
        }

        //Debug.Log($"RenderChunk {chunk.index} matrices: {matrices.Count}");

        for (int i = 0; i < matrices.Count; i += 1023) //iterate through each chunk, drawing each tile. 1023 is apparently, Unity API hard limit.
        {
            int tileBatch = Mathf.Min(1023, matrices.Count - i);
            Graphics.DrawMeshInstanced(
                tileMesh,
                0,
                tileMaterial,
                matrices.GetRange(i, tileBatch));
        }



    }
}
