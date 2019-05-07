Shader "Custom/CloudTest"
{
    Properties
    {
        _MainColor("MainColor", Color) = (1, 1, 1, 1)
        _MainTex ("Texture", 2D) = "white" {}
        _BrightColor("BrightColor", Color) = (1, 1, 1, 1)
        _DarkColor("DarkColor", Color) = (1, 1, 1, 1)
        _SecondColor("SecondColor", Color) = (1, 1, 1, 1)
        _RimColor("RimColor", Color) = (1, 1, 1, 1)
        _DayTime ("DayTime", Range(0,1)) = 0
        _SkyColorUp("SkyColorUp",Color)=(0,0,1,1)
        _SkyColorDown("SkyColorDown",Color)=(0,0,1,1)
        _Height("Height",Range(0,10))=1
        _Power("Power",Range(0,2))=0.2
        _Alpha("alpha",Range(0,1))=1

        
    }
    SubShader
    {
        Tags{ 
            "Queue"="Transparent" 
				"IgnoreProjector"="True" "RenderType"="Transparent" 
				"LightMode"="ForwardBase"
        }
        //LOD 100

        Pass
        {

            ZWrite off
			Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM

            #pragma target 3.0

            #pragma vertex vert  
			#pragma fragment frag       
			
			#include "AutoLight.cginc"

            
            //#pragma fragment frag
            // make fog work
            //#pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 PosWorld : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _MainColor;
            fixed4 _BrightColor;
            fixed4 _DarkColor;
            fixed4 _SecondColor;
            fixed4 _RimColor;
            float _DayTime;
            float _Height; 
            float _Power;
            fixed4 _SkyColorUp; 
            fixed4 _SkyColorDown;
            float _Alpha;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.PosWorld = mul(unity_ObjectToWorld, v.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed3 DarkColor = _DarkColor.rgb * _DarkColor.a;
               // fixed3 SecondColor = (col.g - col.r )*_SecondColor.rgb *_SecondColor.a;
                fixed3 SecondColor = (col.g - col.r )*_SecondColor.rgb *_SecondColor.a;
                fixed3 RimColor = _RimColor.rgb * col.b *_RimColor.a * 1.5;
                fixed3 AddColor = DarkColor + SecondColor + RimColor;
                //fixed3 AddColor = DarkColor  + RimColor;
                //float clampResult4 = clamp( ase_worldPos.y , 0.0 , 10.0 );
                //float height = clamp( i.PosWorld.y , 0.0 , _Height );
                float height = _Height/(i.PosWorld.y+1);
                height = pow(height, _Power);
                fixed4 Skybak = lerp(_SkyColorUp,_SkyColorDown,height);

                fixed3 BrightColor = (1-col.r)*_DayTime+_BrightColor.rgb*col.r*(1-_BrightColor.w)+col.r*_BrightColor.w+_BrightColor.rgb*col.g*_BrightColor.a;

                //fixed3 BrightColor = col.r;


                fixed4 finalColor = lerp (Skybak,fixed4 (( BrightColor + AddColor)* _MainColor.rgb,1), col.a* _MainColor.a*(1-height+0.2));
                //fixed4 finalColor =fixed4(height,height,height,1);
                //fixed4 finalColor = fixed4 ( ( BrightColor + AddColor)* _MainColor.rgb, col.a* _MainColor.a);
               
                //fixed3 Brig = fixed3 ((1-_BrightColor.w)+col.r*_BrightColor.w,(1-_BrightColor.w)+col.r*_BrightColor.w,(1-_BrightColor.w)+col.r*_BrightColor.w);

                //fixed4 finalColor = fixed4 (_BrightColor.rgb*col.r*Brig ,col.a* _MainColor.a);
                
                //fixed4 finalColor = fixed4(BrightColor,1);
                
                //UNITY_APPLY_FOG(i.fogCoord, finalColor);
                return finalColor; 
            }
            ENDCG
        }
    }
}
