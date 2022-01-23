using MarchingCubes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class TestExportOBJ : MonoBehaviour
{
    public TestFishVisualizer FishVisu;
    LineRenderer _renderer;
    private void Start()
    {
        Execute = false;
        _renderer = this.gameObject.GetComponent<LineRenderer>();
    }
    List<Vector3> vertices;
    //List<Tuple<Vector3, Vector3, Vector3>> triangles;
    void ExportOBJ()
    {
        StringBuilder sb = new StringBuilder();
        StringBuilder sbVertices = new StringBuilder();
        StringBuilder sbTriangles = new StringBuilder();
        vertices = new List<Vector3>();
        //triangles = new List<Tuple<Vector3, Vector3, Vector3>>();

        int verticesCount = 0;
        Transform t = this.gameObject.transform;
       // foreach (Transform transform in this.gameObject.transform)
        {

            var lod = t;//.GetChild(1);
            var mesh = lod.GetComponent<MeshFilter>().sharedMesh;
            
            int iCol = 0;
            //var origVertices = mesh.vertices;
            var origVertices = FishVisu.RetrievedVertices;
            int startIdx = 0;// FishVisu.RetrievedVertices.Length; // vertices count
            for (int i =0; i < origVertices.Length; i+= 2) // Keijiro store vertex and normal side by side
            {
                var vPos = origVertices[i];// + transform.localPosition;
                //var vCol = mesh.colors[iCol];
                sbVertices.AppendLine(string.Format("v {0:0.00#} {1:0.00#} {2:0.00#}", vPos.x,vPos.y,vPos.z)); // Vertices VertexColors
                ++verticesCount;
                vertices.Add(vPos);
                iCol++;
            }

            var origTriangles = mesh.triangles;
            origTriangles = FishVisu.RetrievedIndices;
            for (int i = 0; i < origTriangles.Length; i += 3)
            {
                var aIdx = 1+startIdx + origTriangles[i];
                var bIdx = 1+startIdx + origTriangles[i + 1];
                var cIdx = 1+startIdx + origTriangles[i + 2];
                //if (i % 2 == 0)
                //    triangles.Add(new Tuple<Vector3, Vector3, Vector3>(vertices[cIdx], vertices[bIdx], vertices[aIdx]));
                //else
                //    triangles.Add(new Tuple<Vector3, Vector3, Vector3>(vertices[aIdx], vertices[bIdx], vertices[cIdx]));
                sbTriangles.AppendLine(string.Format("f {0} {1} {2}", aIdx, bIdx, cIdx));
            }
        }

        sb.Append(sbVertices).Append(sbTriangles);

        File.WriteAllText(@"C:\Users\z0rg_2080\Desktop\Fish.obj", sb.ToString());
    }
    public bool Execute;
    int count = 0;
    // Update is called once per frame
    void Update()
    {
        if (false)//triangles != null)
        {
            //if (Input.GetKeyDown(KeyCode.LeftArrow))
            //    count = Math.Max(0, count - 1);
            //if (Input.GetKeyDown(KeyCode.RightArrow))
            //    count = Math.Min(triangles.Count - 1, count + 1);

            //if (Math.IEEERemainder(Time.realtimeSinceStartup, 0.5) < 0.05)
            //    count++;
            //if (count >= triangles.Count)
            //    count = 0;

            //_renderer.SetPosition(0, triangles[count].Item1 + Vector3.up * 5.0f);
            //_renderer.SetPosition(1, triangles[count].Item2 + Vector3.up * 5.0f);
            //_renderer.SetPosition(2, triangles[count].Item3 + Vector3.up * 5.0f);

        }

        if (Execute)
        {
            ExportOBJ();
            Execute = false;
        }


    }
}
