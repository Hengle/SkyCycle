#ifndef MMODEMO_CLOUDS_INCLUDE
#define MMODEMO_CLOUDS_INCLUDE

uniform sampler2D    _WispyCloudTexture;
uniform half         _ProceduralCloudAltitude;
uniform half         _WispyCloudDirection;

uniform half4        _WispyDarkness;
uniform half4        _WispyBright;
uniform half4        _WispyColor;
// uniform half         _WispyColorCorrection;

uniform float        _ProceduralCloudSpeed;
uniform half         _WispyCoverage;

float3 RotateAroundYInDegrees (float3 vertex, float degrees)
{
    float alpha = degrees * UNITY_PI / 180.0;
    float sina, cosa;
    sincos(alpha, sina, cosa);
    float2x2 m = float2x2(cosa, -sina, sina, cosa);
    return float3(mul(m, vertex.xz), vertex.y).xzy;
}

float3 CalcAzureCloudsUV(float3 worldPos)
{
    float3 viewDir = normalize(worldPos + float3(0.0,1.0,0.0));

    float3 CloudsUV;
    CloudsUV = RotateAroundYInDegrees(worldPos, _WispyCloudDirection);
	CloudsUV.y *= 1 + (viewDir.y*0.01 + _ProceduralCloudAltitude) *  15;

    return CloudsUV;
}

half4 AzureCloudsFrag(float3 CloudsUV, half3 color)
{
    float3 viewDir  = normalize(CloudsUV);

    float2 cloud_UV1;
    float2 cloud_UV2;

    // cloud_UV1.xy = (viewDir.xz * 0.25) - (0.005  ) + float2(_ProceduralCloudSpeed/10, _ProceduralCloudSpeed);
    // cloud_UV2.xy = (viewDir.xz * 0.5)  - (0.0065 ) + float2(_ProceduralCloudSpeed/20, _ProceduralCloudSpeed);
    cloud_UV1.xy = (viewDir.xz * 0.25) - (0.005  ) + frac(float2(_Time.y *  0.005, _Time.y * 0.001));
    cloud_UV2.xy = (viewDir.xz * 0.5)  - (0.0065 ) + frac(float2(_Time.y * 0.0001 % 1, _Time.y * 0.001 % 1));

    float4 col  = tex2D(_WispyCloudTexture, cloud_UV1.xy );
    // col = pow(col,_WispyColorCorrection);
    float4 col2 = tex2D(_WispyCloudTexture, cloud_UV2.xy );
    // col2 = pow(col2,_WispyColorCorrection);
    float c1 = pow(col.g + col2.g, 0.1);
    float c2 = pow(col2.b * col.r, 0.25);

    float3 cloud1 = lerp(_WispyDarkness.rgb, _WispyBright.rgb, c1);
    float3 cloud2 = lerp(_WispyDarkness.rgb, _WispyColor.rgb, c2) * 1.5;
    float3 cloud3 = lerp(cloud1, cloud2, c1 * c2);
    float cloudExtinction = pow(viewDir.y,5);

    //return half3(lerp(color.rgb, cloud3, saturate(cloudExtinction*pow(c1*c2,_WispyCoverage))));
    return half4(pow(cloud3, 2.2), saturate(cloudExtinction*pow(c1*c2,_WispyCoverage)));
}

sampler2D _XenCloudMainNoiseTex;	
float4 _XenCloudMainNoiseTex_ST;
sampler2D _XenCloudDetailTex;		
float4 _XenCloudDetailTex_ST;
float4 _XenCloudMainNoiseTex_TexelSize;
float4 _XenCloudDetailTex_TexelSize;
float4 _XenCloudMotionSpeed;

half _XenCloudAlphaIntensity;
half _XenCloudAlphaThreshold;
half _XenCloudDetailIntensity;
half _XenCloudBlendIntensity;
fixed4 _XenCloudBrightColor;
fixed4 _XenCloudDarkColor;
half _XenCloudBlendRange;
half _XenCloudHDRCoe;

float DetailBlend2(float range, float detail, float distribution) 
{
    float result = range - (detail-0.5) * distribution;
    return result;
}

float random (float2 uv)
{
    return frac(sin(dot(uv,float2(12.9898,78.233)))*43758.5453123);
}

fixed4 XenCloudsFrag(float3 CloudsUV, fixed3 inColor)
{
    float3 viewDir  = normalize(CloudsUV);
    float cloudExtinction = pow(viewDir.y,5);

    float2 cloud_UV1;
    float2 cloud_UV2;
    float2 cloud_UV_Blend;

    // cloud_UV1.xy = viewDir.xz * _CloudMainNoiseTex_ST.xy + float2(_ProceduralCloudSpeed/10, _ProceduralCloudSpeed);
    // cloud_UV2.xy = viewDir.xz * _CloudDetailTex_ST.xy + float2(_ProceduralCloudSpeed/20, _ProceduralCloudSpeed);
    // cloud_UV_Blend.xy = viewDir.xz * 4;
    cloud_UV1.xy = viewDir.xz * _XenCloudMainNoiseTex_ST.xy + (float2(_Time.y * 0.01 * _XenCloudMotionSpeed.x, _Time.y * 0.01 * _XenCloudMotionSpeed.y));
    cloud_UV2.xy = viewDir.xz * _XenCloudDetailTex_ST.xy + (float2(_Time.y * 0.006 * _XenCloudMotionSpeed.z, _Time.y * 0.006 * _XenCloudMotionSpeed.w));
    cloud_UV_Blend.xy = viewDir.xz * 4 + frac(float2(_Time.y * 0.01 * (_XenCloudMotionSpeed.x + _XenCloudMotionSpeed.z), _Time.y * 0.01 * (_XenCloudMotionSpeed.y + _XenCloudMotionSpeed.w)));

    half mainNoise  = tex2D(_XenCloudMainNoiseTex, cloud_UV1.xy).r;
    half detailNoise  = tex2D(_XenCloudDetailTex, cloud_UV2.xy).r;
    half height = DetailBlend2(mainNoise, detailNoise, _XenCloudDetailIntensity);

    half alphaOut = detailNoise * ((height * _XenCloudAlphaIntensity * 10) - _XenCloudAlphaThreshold);
    alphaOut = smoothstep(detailNoise * _XenCloudAlphaThreshold, 1, alphaOut);

    fixed g = tex2D(_XenCloudMainNoiseTex, cloud_UV_Blend).g;

    half blend = height * _XenCloudBlendRange + _XenCloudBlendIntensity * (g-0.5); 

    fixed3 cloudCol = lerp(_XenCloudBrightColor.rgb, _XenCloudDarkColor, saturate(blend)) * _XenCloudHDRCoe;
    fixed3 finalCol = lerp(inColor, pow(cloudCol, 2.2), saturate(cloudExtinction * alphaOut));
    fixed3 pureCloudCol = lerp(fixed3(0.0, 0.0, 0.0), pow(cloudCol, 2.2), saturate(cloudExtinction * alphaOut));

    //return half4(pow(cloudCol, 2.2), saturate(cloudExtinction * alphaOut));
    return fixed4(finalCol, length(pureCloudCol));
}


#endif