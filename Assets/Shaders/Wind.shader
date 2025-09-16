Shader "Unlit/Wind"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR] _MainColor ("Main Color", Color) = (1,1,1,1)
        _FresnelColor ("Fresnel Color", Color) = (1,1,1,1)
        _Fresnel ("Fresnel", Float) = 1
        _StepLevels ("Step Levels", Vector) = (0.5, 0.6, 0,0)
        _Speed ("Speed", Float) = 1

        _VertexOffset ("Vertex Offset", Float) = 0.5
        _VertexOffsetSpeed ("Vertex Offset Speed", Float) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
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
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                float3 wPos : TEXCOORD1;
            };

            struct v2f
            {
                float4 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 vertex : SV_POSITION;
                float3 wPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _MainColor;

            float2 _StepLevels;

            float _Speed;
            
            float4 _FresnelColor;
            float _Fresnel;

            float _VertexOffset;
            float _VertexOffsetSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                o.uv = float4(TRANSFORM_TEX(v.uv, _MainTex), v.uv);
                float noiseSampleX = tex2Dlod(_MainTex, float4(o.uv+_Time.xx,0,0)).r;
                float noiseSampleY = tex2Dlod(_MainTex, float4((o.uv+float2(0.5,0.5))+_Time.xx,0,0)).r;

                // float2 newuvs = o.uv * 1 + float2(0,_Time.x*_Speed);
                float4 vertexWorld = mul(unity_ObjectToWorld, v.vertex) + _VertexOffset * float4(sin(_VertexOffsetSpeed*_Time.x*noiseSampleX+_VertexOffsetSpeed*o.uv.x)*noiseSampleX,sin(_VertexOffsetSpeed*_Time.x*noiseSampleX+_VertexOffsetSpeed*o.uv.y)*noiseSampleY,0,0);
                float4 vertexObjectNew = mul(unity_WorldToObject, vertexWorld);
                o.vertex = UnityObjectToClipPos(vertexObjectNew);
                o.wPos = mul(unity_ObjectToWorld, v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 V = normalize(_WorldSpaceCameraPos - i.wPos);
                float3 N = normalize(i.normal);
                float fresnel = pow(1.0f - dot(V,N), _Fresnel);
                float fresnelVis = smoothstep(1,0.9,i.uv.w);

                float2 newuvs = i.uv * 1 + float2(0,_Time.x*_Speed);
                // sample the texture
                fixed4 col = tex2D(_MainTex, newuvs);

                float stepMult = saturate(smoothstep(_StepLevels.x, _StepLevels.y, col.r));

                // return _MainColor * col * stepMult + fresnel * _FresnelColor;
                return _MainColor * col * stepMult * fresnel * fresnelVis;
            }
            ENDCG
        }
    }
}
