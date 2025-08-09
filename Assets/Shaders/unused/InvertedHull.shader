Shader "Custom/InvertedHull"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _OutlineWidth ("OutlineWidth", Range(0,1)) = 1.0
        _NormalVSWorldSpace ("NormalVSWorldSpace", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        // Pass
		// {
        //     Tags {"Queue"="Geometry+1"}

		// 	Name "InvertedHull Greater"
        //     ZWrite Off
        //     ZTest Greater 
        //     Cull Front
        //     // Weird Z-sorting issues with Cull Back
		// 	CGPROGRAM
		// 		#pragma vertex vert
		// 		#pragma fragment frag
		// 		#include "UnityCG.cginc"

		// 		struct v2f {
		// 			float4 pos			: POSITION;
		// 		};
        //         float _OutlineWidth;
        //         float _NormalVSWorldSpace;

		// 		v2f vert (appdata_full v)
		// 		{
		// 			v2f o;
        //             float4 objectPos = (v.vertex + v.vertex * _OutlineWidth)*(1-_NormalVSWorldSpace) + (v.vertex + float4(v.normal * _OutlineWidth, 1)) * _NormalVSWorldSpace;
		// 			o.pos = UnityObjectToClipPos(objectPos);
		// 			return o;
		// 		}

		// 		half4 frag( v2f i ) : COLOR
		// 		{
        //             return float4(0,0,0,1);
		// 		}
		// 	ENDCG			
		// }
        Pass
		{

			Name "InvertedHull"
            Tags { "Queue"="Overlay" "RenderType"="Opaque" }
            // ZWrite On
            // ZTest LEqual 
            Cull Front
            // Weird Z-sorting issues with Cull Back
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				struct v2f {
					float4 pos			: POSITION;
				};
                float _OutlineWidth;
                float _NormalVSWorldSpace;

				v2f vert (appdata_full v)
				{
					v2f o;
                    float4 objectPos = (v.vertex + v.vertex * _OutlineWidth)*(1-_NormalVSWorldSpace) + (v.vertex + float4(v.normal * _OutlineWidth, 1)) * _NormalVSWorldSpace;
					o.pos = UnityObjectToClipPos(objectPos);
					return o;
				}

				half4 frag( v2f i ) : SV_Target
				{
                    return float4(0,0,0,1);
				}
			ENDCG			
		}
        
        
        ZWrite On
        ZTest LEqual 
        Cull Back

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
