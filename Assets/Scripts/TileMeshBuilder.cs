using UnityEngine;

public static class TileMeshBuilder
{

    public static Mesh BuildTileMesh(float borderThickness)
    {
        borderThickness = Mathf.Clamp(borderThickness, 0f, 0.45f); //forces the border to stay within a safe range for the tile.

        // Define the Vertices for the full tile (outer quad)
        Vector3 v0 = new Vector3(0f, 0f, 0f); //btm left
        Vector3 v1 = new Vector3(1f, 0f, 0f); //btm right
        Vector3 v2 = new Vector3(1f, 1f, 0f); //top right
        Vector3 v3 = new Vector3(0f, 1f, 0f); //top left

        // defining the verticies for the "inner tile" so the other tile will show behind
        float borderSize = borderThickness;
        Vector3 v4 = new Vector3(borderSize, borderSize, 0f); // innerv0 btm left, up/right bordersize
        Vector3 v5 = new Vector3(1f - borderSize, borderSize, 0f); //innerv1 - btm right, 1-borderSize left, up brdSz
        Vector3 v6 = new Vector3(1f - borderSize, 1f - borderSize, 0f); // innerv2 upper right, 1-brd left, 1-brd from top.
        Vector3 v7 = new Vector3(borderSize, 1f - borderSize, 0f); //innerv3- upper left, brdsz right, 1-brd from top.

        Vector3[] verticies = new Vector3[]
        {
            v0,v1,v2,v3, //outer quad
            v4,v5,v6,v7 // inner quad
        };

        // Define the triangles for Unity (squares are so hard, it's okay, bud. You'll learn one day...)
        int[] triangles = new int[]
        {
            /* forward winding was causing issues, unity was culling the back face.
            //outer quad
            0,1,2,
            0,2,3,
        
            //inner quad
            4,5,6,
            4,6,7
            */
            
            //reverse winding.
            //outer
            0,2,1,
            0,3,2,
            //inner
            4,6,5,
            4,7,6



        };

        // Defining the UVs 
        //outer UVs
        Vector2 uv0 = new Vector2(0f, 0f); //btm l
        Vector2 uv1 = new Vector2(1f, 0f); //btm r
        Vector2 uv2 = new Vector2(1f, 1f); //top r
        Vector2 uv3 = new Vector2(0f, 1f); //top l
        //inner UVs 
        Vector2 uv4 = new Vector2(borderSize, borderSize); //inner btm l
        Vector2 uv5 = new Vector2(1f - borderSize, borderSize);    //inner btm r
        Vector2 uv6 = new Vector2(1f - borderSize, 1f - borderSize); //inner top r
        Vector2 uv7 = new Vector2(borderSize, 1f - borderSize);    //inner top l

        Vector2[] uvs = new Vector2[]
        {
            uv0,uv1,uv2,uv3, //outer
            uv4,uv5,uv6,uv7 //inner
        };

        //Colors
        Color borderColor = Color.black;
        Color fillColor = Color.green;

        //define color array to apply to the quad.
        Color[] colors = new Color[]
        {
            borderColor,borderColor,borderColor,borderColor, // outer quad color
            fillColor,fillColor,fillColor,fillColor // inner quad color
        };

        //Actually assemble this mesh now: (this is the whole tile)
        Mesh mesh = new Mesh();
        mesh.name = "TileWithBorder";

        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.colors = colors;

        mesh.SetTriangles(triangles, 0); // Defining a submesh to allow the mesh to be loaded over it? I think?

        mesh.RecalculateNormals(); //FOR SOME REASON, UNITY NEEDS THIS?!?!?
        mesh.RecalculateBounds(); //calc the bounding box

        //overriding the bounds to prevent Unity from culling it.
        mesh.bounds = new Bounds(
            new Vector3(0.5f, 0.5f, 0f),
            new Vector3(2f, 2f, 1f)
        );

        return mesh;

    }
}


