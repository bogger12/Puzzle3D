Shader "Unlit/Refractive"
{
    Properties
    {
        _EnvTex ("Environment", Cube) = "gray" {}
        _MainTex ("MainTex", 2D) = "white" {}
        _TexVisibility ("TexVisibility", Range (0,1)) = 0.1
        _Fresnel ("Fresnel", Float) = 1
        _Transparency ("Transparency", Range (0,1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend One One  

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float4 screenPos : TEXCOORD1;
                float3 wPos : TEXCOORD2;
                float3 normal : NORMAL;
            };

            samplerCUBE _EnvTex;
            sampler _MainTex;
            float _TexVisibility;
            float _Fresnel;
            float _Transparency;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.screenPos = ComputeScreenPos(o.pos);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.wPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float3 col = tex2D(_MainTex, i.uv);

                float3 V = normalize(_WorldSpaceCameraPos - i.wPos);
                float3 N = normalize(i.normal);
                float fresnel = pow(1.0f - dot(V,N), _Fresnel);

                float3 reflectDir = reflect(-V, N);
                float3 refractDir = refract(-V, N, 1.54);

                float3 reflectColor = texCUBE(_EnvTex, reflectDir).rgb;
                float3 refractColor = texCUBE(_EnvTex, refractDir).rgb;
                return float4(fresnel * reflectColor, _Transparency);
                // return float4(fresnel * reflectColor, 0.5);
            }
            ENDCG
        }
        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha 

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            sampler _MainTex;
            float _TexVisibility;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float3 col = tex2D(_MainTex, i.uv);
                return float4(col, _TexVisibility);
            }
            ENDCG
        }
    }
}
