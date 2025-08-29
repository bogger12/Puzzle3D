// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/WarpRefract"
{
    Properties
    {
        _MainTex ("Normal Map (Bump)", 2D) = "white" {}
        _RefractionStrength ("Refraction Strength", Range (0, 0.2)) = 0
        _WarpTiling ("WarpTiling", Float) = 1
        _Fresnel ("Fresnel", Float) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        // Cull Off
        ZWrite Off

        GrabPass { "_GrabTexture" }

        Pass
        {
            Tags { "LightMode"="ForwardBase" }
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
                float4 grabPos : TEXCOORD1;
                float3 wPos : TEXCOORD2;
                float3 normal : NORMAL;
            };

            sampler2D _MainTex;
            sampler2D _GrabTexture;
            float _RefractionStrength;
            float _WarpTiling;
            float _Fresnel;


            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.grabPos = ComputeGrabScreenPos(o.pos);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.wPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float3 V = normalize(_WorldSpaceCameraPos.xyz - i.wPos.xyz);
                float3 N = normalize(i.normal);
                float fresnel = pow(1.0f - dot(V,N), _Fresnel);
                // return float4(fresnel.xxx, 1);

                // float2 offset = N.xy * _RefractionStrength;
                float2 offset = float2(sin(i.uv.y * _WarpTiling), cos(i.uv.x * _WarpTiling)) * _RefractionStrength;

                // float2 screenUV = i.grabPos.xy / i.grabPos.w + offset * fresnel;
                float2 screenUV = i.grabPos.xy / i.grabPos.w + offset;
                


                return saturate(tex2D(_GrabTexture, screenUV));
            }
            ENDCG
        }
    }
}
