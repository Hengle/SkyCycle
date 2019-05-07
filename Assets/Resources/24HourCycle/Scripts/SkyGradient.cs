using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyGradient : MonoBehaviour
{
    public Gradient gradient;
    public GradientColorKey[] colorKey;
    public GradientAlphaKey[] alphaKey;

    private Color lastBottomColor;
    private Color lastMiddleColor;
    private Color lastTopColor;
    private float lastMiddleTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Matrix4x4 localToWorld = transform.localToWorldMatrix;
        float radius = mesh.bounds.extents.y * localToWorld.lossyScale.y;
        Shader.SetGlobalFloat("_RadiusHeight", radius);
        Shader.SetGlobalFloat("_SkyStartHeight", localToWorld.m13);
    }

    // Update is called once per frame
    void Update()
    {
        colorKey = gradient.colorKeys;
        if(colorKey.Length == 2)
        {
            Shader.DisableKeyword("MIDDLEPOINT_ON");
            if (!lastBottomColor.Equals(colorKey[0].color))
            {
                Shader.SetGlobalColor("_BottomColor", colorKey[0].color);
                lastBottomColor = colorKey[0].color;
            }
            if (!lastTopColor.Equals(colorKey[1].color))
            {
                Shader.SetGlobalColor("_TopColor", colorKey[1].color);
                lastTopColor = colorKey[1].color;
            }
        }
        else if(colorKey.Length == 3)
        {
            Shader.EnableKeyword("MIDDLEPOINT_ON");
            if (!lastBottomColor.Equals(colorKey[0].color))
            {
                Shader.SetGlobalColor("_BottomColor", colorKey[0].color);
                lastBottomColor = colorKey[0].color;
            }
            if (!lastMiddleColor.Equals(colorKey[1].color))
            {
                Shader.SetGlobalColor("_MiddleColor", colorKey[1].color);
                lastMiddleColor = colorKey[1].color;
            }
            if (!lastTopColor.Equals(colorKey[2].color))
            {
                Shader.SetGlobalColor("_TopColor", colorKey[2].color);
                lastTopColor = colorKey[2].color;
            }
            if (!lastMiddleTime.Equals(colorKey[1].time))
            {
                Shader.SetGlobalFloat("_MiddleTime", colorKey[1].time);
                lastMiddleTime = colorKey[1].time;
            }
        }
    }
}
