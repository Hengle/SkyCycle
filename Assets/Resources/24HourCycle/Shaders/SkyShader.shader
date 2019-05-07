Shader "Custom/SkyShader"
{
	Properties
	{
		_BottomColor ("BottomColor", Color) = (0,0,1,1)
		_MiddleColor("MiddleColor", Color) = (0,1,0,1)
		_TopColor("TopColor", Color) = (1,0,0,1)
		_RadiusHeight("RadiusHeight", float) = 0.5
		_MiddleTime("MiddleTime", float) = 0.5
		_SkyStartHeight("SkyStartHeight", float) = 0.0
	}
	SubShader
	{
		Tags { "Queue" = "Background" "RenderType" = "Background" "PreviewType" = "Skybox" }
		Cull Off
		ZWrite Off

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		#pragma multi_compile __ MIDDLEPOINT_ON

		sampler2D _MainTex;

		struct Input
		{
			float2 uv_MainTex;
			float3 worldPos;
		};

		fixed4 _BottomColor;
		fixed4 _MiddleColor;
		fixed4 _TopColor;
		float _MiddleTime;
		float _RadiusHeight;
		float _SkyStartHeight;

		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			// TODO : add begin and end Y
			fixed4 bottomColor = _BottomColor;
			fixed4 topColor = _TopColor;
#ifdef MIDDLEPOINT_ON
			float middlePos = _RadiusHeight * _MiddleTime * 0.5 + _SkyStartHeight;
			float lerpFactor = 0.0f;
			if (IN.worldPos.y > middlePos)
			{
				lerpFactor = (IN.worldPos.y - middlePos) / (_RadiusHeight + _SkyStartHeight - middlePos);
				bottomColor = _MiddleColor;
			}
			else
			{
				lerpFactor = IN.worldPos.y / middlePos;
				topColor = _MiddleColor;
			}
#endif
#ifndef MIDDLEPOINT_ON
			float lerpFactor = (IN.worldPos.y - _SkyStartHeight) / _RadiusHeight;
#endif
			lerpFactor = saturate(lerpFactor);
			fixed4 c = lerp(bottomColor, topColor, lerpFactor);
			o.Albedo = _TopColor.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback Off
}
