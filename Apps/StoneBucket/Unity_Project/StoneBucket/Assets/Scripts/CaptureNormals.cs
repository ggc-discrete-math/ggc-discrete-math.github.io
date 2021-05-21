using UnityEngine;
using System.Collections;

public class CaptureNormals : MonoBehaviour
{
    private Material mat;

    void OnPostRender()
    {
        if(!mat)
        {
            mat = new Material("Shader \"Hidden/SetAlpha\" {" +
            "SubShader {" +
            "    Pass {" +
            "        ZTest Always Cull Off ZWrite Off" +
            "        SetTexture [_CameraNormalsTexture] { combine texture } " +
            "   } " +
            "   }" +
            "}"
            );
        }


        GL.PushMatrix();
        GL.LoadOrtho();
        for(int i = 0 ; i < mat.passCount ; ++i )
        {
            mat.SetPass(i);
            GL.Begin(GL.QUADS);
            GL.TexCoord(new Vector3(0, 0, 0));
            GL.Vertex3(0, 0, 0.1f);
            GL.TexCoord(new Vector3(1, 0, 0));
            GL.Vertex3(1, 0, 0.1f);
            GL.TexCoord(new Vector3(1, 1, 0));
            GL.Vertex3(1, 1, 0.1f);
            GL.TexCoord(new Vector3(0, 1, 0));
            GL.Vertex3(0, 1, 0.1f);
            GL.End();
        }

        GL.PopMatrix();



    }
}
