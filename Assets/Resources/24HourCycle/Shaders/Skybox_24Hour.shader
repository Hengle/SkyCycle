Shader "Custom/Skybox_24Hour"
{
    Properties
    {
		[Header(Sky)]
		_RadiusHeight("Radius Height", float) = 1
		_SkyStartHeight("Sky StartHeight", float) = 0.0
		_SkyRotation("Sky Rotation", float) = 0.0
		_SkyMaskIntensity("Sky Mask Intensity", Range(0,1.75)) = 0.5

		[Header(Sun)]
		[KeywordEnum(None, Simple, High Quality)] _SunDisk("Sun Disk", Int) = 2
		_SunSize("Sun Size", Range(0, 1)) = 0.04
		_SunColor("Sun Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_SunSizeConvergence("Sun Size Convergence", Range(1, 10)) = 5
		_SunDiskIntensity("Sun Disk Intensity", Range(0, 5)) = 1.0
		_SunLightCloudIntensity("Sun Light Cloud Intensity", Range(0, 2)) = 1.0
		_SunLightSkyIntensity("Sun Light Sky Intensity", Range(0, 1)) = 0.1

		[Header(Moon)]
		[KeywordEnum(None, Simple, High Quality)] _MoonDisk("Moon Disk", Int) = 0
		_MoonSize("Moon Size", Range(0, 1)) = 0.04
		_MoonColor("Moon Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_MoonSizeConvergence("Moon Size Convergence", Range(1, 10)) = 5
		_MoonDiskIntensity("Moon Disk Intensity", Range(0, 5)) = 0.1
		_MoonLightCloudIntensity("Moon Light Cloud Intensity", Range(0, 2)) = 0.1
		_MoonLightSkyIntensity("Moon Light Sky Intensity", Range(0, 1)) = 0.1

		[Header(Star)]
		_StarNoise("Star Noise", 2D) = "white" {}

		[Header(Ground)]
		_GroundColor("Ground", Color) = (.369, .349, .341, 1)

		[Header(AzureClouds)]
		_WispyCloudTexture("Clouds Texture",2D) = "black"{}
		_WispyCoverage("Coverage", Range(0, 5)) = 1
		_WispyDarkness("Darkness", Color) = (0.57,0.75,1,1)
		_WispyBright("Bright", Color) = (0.2,0.11,0.06,1)
		_WispyColor("Wispy Color", Color) = (1,0.91,0.83,1)

		[Header(XenClouds)]
		_ProceduralCloudAltitude("Xen Cloud Altitude", Range(0,1)) = 0.4
		_WispyCloudDirection("Xen Cloud Direction", Range(-180, 180)) = 0
		_XenCloudMainNoiseTex("Xen Cloud Main Noise", 2D) = "white" {}
		_XenCloudDetailTex("Xen Cloud Detail Noise", 2D) = "white" {}
		_XenCloudBrightColor("Xen Cloud Bright Color", Color) = (1,1,1,1)
		_XenCloudBrightColorCoe("XenCloud BrightColor Coe", Range(0, 1)) = 1.0
		_XenCloudDarkColor("Xen Cloud Dark Color", Color) = (1,1,1,1)
		_XenCloudDarkColorCoe("XenCloud Dark Coe", Range(0, 1)) = 0.5
		_XenCloudHDRCoe("XenCloud HDR Coe", Range(0, 5)) = 1.0
		_XenCloudAlphaIntensity("Xen Cloud Alpha", Range(0, 1)) = 0.5
		_XenCloudAlphaThreshold("Xen Cloud Threshold", Range(-3, 1)) = 0.55
		_XenCloudDetailIntensity("Xen Cloud Detail Intensity", Range(-5, 5)) = 1.6
		_XenCloudBlendIntensity("Xen Cloud Blend Texture Intensity", Range(-2, 2)) = 0.75
		_XenCloudBlendRange("Xen Cloud Blend Height Range", Range(0, 2)) = 0.254
		_XenCloudMaskIntensity("Xen Cloud Mask Intensity", Range(0, 1.75)) = 0.5

		[Header(AgileCloud)]
		_AgileCloudMainColor("AgileCloud MainColor", Color) = (1, 1, 1, 1)
		_AgileCloudMainTex("AgileCloud Texture", Cube) = "white" {}
		_AgileCloudBrightColor("AgileCloud BrightColor", Color) = (1, 1, 1, 1)
		_AgileCloudBrightColorCoe("AgileCloud BrightColor Coe", Range(0, 2)) = 0.2
		_AgileCloudDarkColor("AgileCloud DarkColor", Color) = (1, 1, 1, 1)
		_AgileCloudDarkColorCoe("AgileCloud DarkColor Coe", Range(0, 2)) = 0.2
		_AgileCloudSecondColor("AgileCloud SecondColor", Color) = (1, 1, 1, 1)
		_AgileCloudSecondColorCoe("AgileCloud SecondColor Coe", Range(0, 2)) = 0.2
		_AgileCloudRimColor("AgileCloud RimColor", Color) = (1, 1, 1, 1)
		_AgileCloudRimColorCoe("AgileCloud RimColor Coe", Range(0, 2)) = 0.2
		_AgileCloudLuminance("AgileCloud Luminance", Range(0, 1)) = 0
		_AgileCloudSkyColorUp("AgileCloud SkyColorUp", Color) = (0, 0, 1, 1)
		_AgileCloudSkyColorDown("AgileCloud SkyColorDown", Color) = (0, 0, 1, 1)
		_AgileCloudHeight("AgileCloud Height", Range(0, 10)) = 1
		_AgileCloudPower("AgileCloud Power", Range(0, 2)) = 0.2
		_AgileCloudAlpha("AgileCloud alpha", Range(0, 1)) = 1

		[Header(Star)]
		_StarIntensity("StarIntensity", Range(0, 2)) = 1.0

		[Header(Fog)]
		_FogDistIntensity("FogDistIntensity", Range(0, 10)) = 1.0
    }
    SubShader
    {
		Tags { "Queue" = "Background" "RenderType" = "Background" "PreviewType" = "Skybox" }
		Cull Off
		ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma multi_compile __ _MIDDLEPOINT_ON
			#pragma multi_compile __ _STAR_ON
			#pragma multi_compile _SUNDISK_NONE _SUNDISK_SIMPLE _SUNDISK_HIGH_QUALITY
			#pragma multi_compile _MOONDISK_NONE _MOONDISK_SIMPLE _MOONDISK_HIGH_QUALITY
			#pragma multi_compile __ _AGILECLOUD_ON
			#pragma multi_compile __ _XENCLOUD_ON
			#pragma multi_compile __ _HEIGHT_FOG

            #include "UnityCG.cginc"
			#include "AzureClouds.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f 
			{
				float4 vertex : SV_POSITION;
				float3 texcoord : TEXCOORD0;
				float4 worldPos : TEXCOORD1;
				float3 agileCloudsUV : TEXCOORD2;
				float3 azureCloudsUV : TEXCOORD3;
				float4 screenPos : TEXCOORD4;
#if defined(_HEIGHT_FOG)
				half fogAmount : TEXCOORD5;
#endif
				UNITY_VERTEX_OUTPUT_STEREO
			};

			fixed4 _BottomColor[2];
			fixed4 _MiddleColor[2];
			fixed4 _TopColor[2];
			float _MiddleTime[2];
			float _TopTime[2];
			float _SkyBlendLerpFactor;
			float _RadiusHeight;
			float _SkyStartHeight;
			float _SkyRotation;
			float _SunSize;
			float3 _SunDir;
			fixed4 _SunColor;
			half _SunSizeConvergence;
			float _SunDiskIntensity;
			float _SunLightCloudIntensity;
			float _SunLightSkyIntensity;
			float _MoonSize;
			float3 _MoonDir;
			fixed4 _MoonColor;
			half _MoonSizeConvergence;
			float _MoonDiskIntensity;
			float _MoonLightCloudIntensity;
			float _MoonLightSkyIntensity;
			fixed4 _GroundColor;
			float _IsShowStar;
			sampler2D _StarNoise;
			float _XenCloudMaskIntensity;
			float _SkyMaskIntensity;

			float _XenCloudBrightColorCoe;
			float _XenCloudDarkColorCoe;

			samplerCUBE _AgileCloudMainTex;
			float4 _AgileCloudMainTex_ST;
			fixed4 _AgileCloudMainColor;
			fixed4 _AgileCloudBrightColor;
			float _AgileCloudBrightColorCoe;
			fixed4 _AgileCloudDarkColor;
			float _AgileCloudDarkColorCoe;
			fixed4 _AgileCloudSecondColor;
			float _AgileCloudSecondColorCoe;
			fixed4 _AgileCloudRimColor;
			float _AgileCloudRimColorCoe;
			float _AgileCloudLuminance;
			float _AgileCloudHeight;
			float _AgileCloudPower;
			fixed4 _AgileCloudSkyColorUp;
			fixed4 _AgileCloudSkyColorDown;
			float _AgileCloudAlpha;

			float _StarIntensity;

			half4 _FogInfo; // fogInfo : start distance, density multiplier, height multiplier, fog max height
			half4 _FogColor0;
			half4 _FogColor1;
			half4 _FogColor2;
			half _FogDistIntensity;

			#define SKY_GROUND_THRESHOLD 0.02
			#define MIE_G (-0.990)
			#define MIE_G2 0.9801

			// sun disk rendering:
			// no sun disk - the fastest option
			#define SKYBOX_DISK_NONE 0
			// simplistic sun disk - without mie phase function
			#define SKYBOX_DISK_SIMPLE 1
			// full calculation - uses mie phase function
			#define SKYBOX_DISK_HQ 2

			// uncomment this line and change SKYBOX_DISK_SIMPLE to override material settings
			// #define SKYBOX_SUNDISK SKYBOX_DISK_SIMPLE

#ifndef SKYBOX_SUNDISK
	#if defined(_SUNDISK_NONE)
		#define SKYBOX_SUNDISK SKYBOX_DISK_NONE
	#elif defined(_SUNDISK_SIMPLE)
		#define SKYBOX_SUNDISK SKYBOX_DISK_SIMPLE
	#else
		#define SKYBOX_SUNDISK SKYBOX_DISK_HQ
	#endif
#endif

#ifndef SKYBOX_MOONDISK
	#if defined(_MOONDISK_NONE)
		#define SKYBOX_MOONDISK SKYBOX_DISK_NONE
	#elif defined(_MOONDISK_NONE)
		#define SKYBOX_MOONDISK SKYBOX_DISK_SIMPLE
	#else
		#define SKYBOX_MOONDISK SKYBOX_DISK_HQ
	#endif
#endif

			half Pow4(half x)
			{
				half result = x * x;
				return result * result;
			}

			half calcFogAmountVS(half3 worldPos)
			{
				float dist = _FogInfo.x + 300 * _FogDistIntensity;
				half height01 = saturate((1.0 - worldPos.y) * _FogInfo.w * _FogInfo.z);
				half heightCoef = Pow4(height01);
				half d = max(0.0, dist - _FogInfo.x);
				half c = _FogInfo.y * max(heightCoef, 0.1);
				half fogAmount = (1.0 - exp(-d * c));
				return fogAmount * fogAmount;
			}

            v2f vert (appdata v)
            {
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				float3 rotated = RotateAroundYInDegrees(v.vertex, _SkyRotation);
				o.vertex = UnityObjectToClipPos(rotated);
				o.texcoord = v.vertex.xyz;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.azureCloudsUV = CalcAzureCloudsUV(o.worldPos);
				o.agileCloudsUV = o.worldPos;
				o.screenPos = ComputeScreenPos(o.vertex);
#if defined(_HEIGHT_FOG)
				o.fogAmount = calcFogAmountVS(o.worldPos);
#endif
				return o;
            }

			// Calculates the Mie phase function
			half getMiePhase(half eyeCos, half eyeCos2)
			{
				half temp = 1.0 + MIE_G2 - 2.0 * MIE_G * eyeCos;
				temp = pow(temp, pow(_SunSize, 0.65) * 10);
				temp = max(temp, 1.0e-4); // prevent division by zero, esp. in half precision
				temp = 1.5 * ((1.0 - MIE_G2) / (2.0 + MIE_G2)) * (1.0 + eyeCos2) / temp;

				return temp;
			}

			// Calculates the sun shape
			half calcSunDiskAttenuation(half3 lightPos, half3 ray)
			{
#if SKYBOX_SUNDISK == SKYBOX_DISK_SIMPLE
				half3 delta = lightPos - ray;
				half dist = length(delta);
				half spot = (1.0 - smoothstep(0.0, _SunSize, dist)) * _SunDiskIntensity;
				return spot * spot;
#else // SKYBOX_DISK_HQ
				half focusedEyeCos = pow(saturate(dot(lightPos, ray)), _SunSizeConvergence);
				return getMiePhase(-focusedEyeCos, focusedEyeCos * focusedEyeCos) * _SunDiskIntensity * (1.0 + ray.y) * 0.5;
#endif
			}

			// Calculates the moon shape
			half calcMoonDiskAttenuation(half3 lightPos, half3 ray)
			{
				half3 delta = lightPos - ray;
				half dist = length(delta);
				half spot = (1.0 - smoothstep(0.0, _MoonSize, dist));
				return step(0.05, spot) * _MoonDiskIntensity * ray.y;

				/*half focusedEyeCos = pow(saturate(dot(lightPos, ray)), _MoonSizeConvergence);
				return getMiePhase(-focusedEyeCos, focusedEyeCos * focusedEyeCos) * _MoonDiskIntensity;*/

				//return pow(dot(lightPos, ray), 5.0) * _MoonDiskIntensity;
			}

			// Calculates the sun light
			half calcSunLightAttenuation(half3 lightPos, half3 ray)
			{
				return pow(dot(lightPos, ray), 3.0) * _SunLightCloudIntensity * (1.0 + ray.y) * 0.5;
			}

			// Calculates the moon light
			half calcMoonLightAttenuation(half3 lightPos, half3 ray)
			{
				return pow(dot(lightPos, ray), 5.0) * _MoonLightCloudIntensity * ray.y;
			}

			// Get random from position
			int noiseByPosition(float3 position)
			{
				float3 p = position;
				return ((p.x * 6617) + (p.y * 7015) + (p.z * 7273)) % 5963;
			}

			float rand(float3 myVector)
			{
				return sin(dot(myVector, float3(12.9898, 78.233, 45.5432))) * 43758.5453;
			}

			float mod(float x, float y) {
				return x - y * floor(x / y);
			}

			half3 blendFog(half3 col, half fogAmount, half3 lightDir, half3 viewDir)
			{
				half3 fogColor = _FogColor1.rgb * saturate(viewDir.y * 5.0 + 1.0) + _FogColor0.rgb;
				half VoL = saturate(dot(-viewDir, lightDir));
				fogColor = fogColor + _FogColor2.rgb * VoL * VoL;
				col = col * (1.0 - fogAmount * fogAmount) + fogColor * fogAmount;
				col = clamp(col, 0.0, 4.0);
				return col;
			}

            fixed4 frag (v2f i) : SV_Target
            {
				// TODO : add begin and end Y
				fixed4 bottomColor;
				fixed4 topColor;
				fixed4 skyColor[2];
				float lerpFactor = 0.0f;
#ifdef _MIDDLEPOINT_ON
				for (int index = 0; index < 2; ++index)
				{
					float middlePos = _RadiusHeight * _MiddleTime[index] + _SkyStartHeight;
					float topPos = _RadiusHeight * _TopTime[index] + _SkyStartHeight;
					if (i.texcoord.y > middlePos)
					{
						lerpFactor = (i.texcoord.y - middlePos) / (topPos + _SkyStartHeight - middlePos);
						bottomColor = _MiddleColor[index];
						topColor = _TopColor[index];
					}
					else
					{
						lerpFactor = i.texcoord.y / middlePos;
						bottomColor = _BottomColor[index];
						topColor = _MiddleColor[index];
					}
					lerpFactor = saturate(lerpFactor);
					skyColor[index] = lerp(bottomColor, topColor, lerpFactor);
				}
#else
				for (int index = 0; index < 2; ++index)
				{
					float topPos = _RadiusHeight + _SkyStartHeight;
					lerpFactor = (i.texcoord.y - _SkyStartHeight) / topPos;
					lerpFactor = saturate(lerpFactor);
					bottomColor = _BottomColor[index];
					topColor = _TopColor[index];
					skyColor[index] = lerp(bottomColor, topColor, lerpFactor);
				}
#endif

				half3 ray = normalize(i.worldPos);
				half y = ray.y / SKY_GROUND_THRESHOLD;

				fixed4 outSkyColor = lerp(skyColor[0], skyColor[1], _SkyBlendLerpFactor);
				half skyColorIntensity = length(outSkyColor.xyz);
				half skyMask = 1.0 - smoothstep(0.00, _SkyMaskIntensity, outSkyColor);
				skyMask = pow(skyMask, 2.0) * pow(ray.y, 2.0);

				fixed4 outColor = 0.0;

				fixed4 outSunDiskColor = 0.0;
				fixed4 outMoonDiskColor = 0.0;
				fixed4 outSunLightColor = 0.0;
				fixed4 outMoonLightColor = 0.0;

				if (y > 0.0)
				{
					// 计算太阳
#if SKYBOX_SUNDISK != SKYBOX_DISK_NONE
					outSunDiskColor = saturate(_SunColor * calcSunDiskAttenuation(_SunDir, ray));
#endif
					outSunLightColor = saturate(_SunColor * calcSunLightAttenuation(_SunDir, ray));
					// 计算月亮
#if SKYBOX_MOONDISK != SKYBOX_DISK_NONE
					outMoonDiskColor = saturate(_MoonColor * calcMoonDiskAttenuation(_MoonDir, ray) * skyMask);
#endif
					outMoonLightColor = saturate(_MoonColor * calcMoonLightAttenuation(_MoonDir, ray));
				}

				outColor += outSkyColor;
				outColor += outSunDiskColor;
				outColor += outMoonDiskColor;
				outColor += outSunLightColor * _SunLightSkyIntensity;
				outColor += outMoonLightColor * _MoonLightSkyIntensity;

				// Agile Cloud
				fixed4 outAgileCloudColor = 0.0;
#if _AGILECLOUD_ON
				outColor = clamp(0.0, 1.0, outColor);
				_AgileCloudRimColor.a = length(outSunLightColor.rgb + outMoonLightColor.rgb) * _AgileCloudRimColorCoe;
				fixed4 col = texCUBE(_AgileCloudMainTex, i.agileCloudsUV);
				fixed3 agileDarkColor = _AgileCloudDarkColor.rgb * _AgileCloudDarkColor.a;
				fixed3 agileSecondColor = (col.g - col.r) * _AgileCloudSecondColor.rgb *_AgileCloudSecondColor.a;
				fixed3 agileRimColor = _AgileCloudRimColor.rgb * col.b * _AgileCloudRimColor.a * 1.5;
				fixed3 agileAddColor = agileDarkColor + agileSecondColor + agileRimColor;
				fixed3 agileBrightColor = (1 - col.r) * _AgileCloudLuminance + _AgileCloudBrightColor.rgb * col.r * (1 - _AgileCloudBrightColor.w)
					+ col.r * _AgileCloudBrightColor.w + _AgileCloudBrightColor.rgb * col.g * _AgileCloudBrightColor.a;
				float height = _AgileCloudHeight / (i.worldPos.y + 1);
				outAgileCloudColor = fixed4((agileBrightColor + agileAddColor) * _AgileCloudMainColor.rgb, col.a * _AgileCloudMainColor.a * (1 - height + 0.2));
				outColor = lerp(outColor, outAgileCloudColor, outAgileCloudColor.a);
#endif

				// Xen Cloud
				fixed4 outXenCloudColor = 0.0;
#if _XENCLOUD_ON
				outColor = clamp(0.0, 1.0, outColor);
				//_XenCloudBrightColor.rgb *= length(outSkyColor.rgb) * _XenCloudBrightColorCoe;
				//_XenCloudDarkColor.rgb *= length(outSkyColor.rgb) * _XenCloudDarkColorCoe;
				_XenCloudBrightColor.rgb += (outSunLightColor.rgb + outMoonLightColor.rgb) * _XenCloudBrightColorCoe;
				_XenCloudDarkColor.rgb += (outSunLightColor.rgb + outMoonLightColor.rgb) * _XenCloudDarkColorCoe;
				if (y > 0.0)
				{
					outXenCloudColor = XenCloudsFrag(i.azureCloudsUV, outColor.rgb);
				}
				outColor = outXenCloudColor;
#endif

#ifdef _STAR_ON 
				// 计算星星
				fixed4 starColor = 1.0 * _StarIntensity;
				fixed4 outStarColor = 0.0;
				float3 rayNormalized = floor(ray.xyz * 512.0) / 512.0;
				float noiseModulo = rand(rayNormalized);
				if ((y > 0.0) && (mod(noiseModulo, 1024.0) < 0.05))
				{
					outStarColor += starColor * (1.0 - step(0.01, length(outMoonDiskColor))) 
						* skyMask * (1.0 - step(0.01, outXenCloudColor.w)) * (1.0 - step(0.01, length(outAgileCloudColor.rgb * outAgileCloudColor.a)));
				}
				outColor += outStarColor;
#endif			

				// 计算地面
				outColor = lerp(outColor, _GroundColor, saturate(-y));

#if defined(_HEIGHT_FOG)
				if (y > 0.0)
				{
					half3 lightDir = _WorldSpaceLightPos0.xyz;
					outColor.rgb = blendFog(outColor.rgb, i.fogAmount, lightDir, ray);
				}
#endif

				return outColor;
            }
            ENDCG
        }
    }
	Fallback Off
}
