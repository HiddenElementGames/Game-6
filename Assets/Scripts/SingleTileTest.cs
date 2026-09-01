using UnityEngine;

public class SingleTileTest : MonoBehaviour
{
    public Material tileMaterial;

    void OnRenderObject()
    {
        Mesh m = TileMeshBuilder.BuildTileMesh(0.05f);

        var matrix = Matrix4x4.TRS(
            new Vector3(0f, 0f, 0f),
            Quaternion.identity,
            Vector3.one
        );

        Graphics.DrawMesh(m, matrix, tileMaterial, 0);
    }
}