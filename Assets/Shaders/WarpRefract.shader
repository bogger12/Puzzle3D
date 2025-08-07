// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/WarpRefract"
{
    Properties
    {
        _MainTex ("Normal Map (Bump)", 2D) = "white" {}
        _RefractionStrength ("Refraction Strength", Range (0, 0.2)) = 0
        _WarpTiling ("WarpTiling", Float) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Back
        ZWrite Off

        GrabPass { "_GrabTexture" }

        Pass
        {
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
                // float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 grabPos : TEXCOORD1;
                // float3 wPos : TEXCOORD2;
                // float3 normal : NORMAL;
            };

            sampler2D _MainTex;
            sampler2D _GrabTexture;
            float _RefractionStrength;
            float _WarpTiling;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.grabPos = ComputeGrabScreenPos(o.vertex);
                // o.wPos = mul(unity_ObjectToWorld, v.vertex);
                // o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                // float3 N = UnpackNormal(tex2D(_MainTex, i.uv));

                // float3 V = normalize(_WorldSpaceCameraPos - i.wPos);
                // float fresnel = pow(1.0 - dot(V,i.normal), _Fresnel);

                // float2 offset = N.xy * _RefractionStrength;
                float2 offset = float2(sin(i.uv.y * _WarpTiling), cos(i.uv.x * _WarpTiling)) * _RefractionStrength;

                float2 screenUV = i.grabPos.xy / i.grabPos.w + offset;
                


                return tex2D(_GrabTexture, screenUV);
            }
            ENDCG
        }
    }
}
