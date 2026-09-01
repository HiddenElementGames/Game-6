using UnityEngine;

public class TileDebugGameObjectScript : MonoBehaviour
{
    public Material tileMaterial;

    void Awake()
    {
        var mf = gameObject.AddComponent<MeshFilter>();
        var mr = gameObject.AddComponent<MeshRenderer>();

        mf.mesh = TileMeshBuilder.BuildTileMesh(0.05f);
        mr.material = tileMaterial;
    }

}
