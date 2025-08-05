Shader "Unlit/DitherShader"
{
    Properties
    {
        _Value ("IDK Value", Float) = 1.0 
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        // LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            // #pragma multi_compile_fog

            #include "Lighting.cginc"

            // compile shader into multiple variants, with and without shadows
            // (we don't care about any lightmaps yet, so skip these variants)
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            // shadow helper functions and macros
            #include "AutoLight.cginc"


            float _Value;

            struct MeshData // per vertex data
            {
                float4 vertex : POSITION; // vertex position
                float3 normal : NORMAL; // normal direction
                float2 uv : TEXCOORD0; // uv coords
            };

            struct v2f // data structure passed from vertex to fragment shader
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION; // clip space position
                float3 normal : NORMAL; // normal direction
            };

            v2f vert (MeshData v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); // converts local space to clip space (MVP matrix mult)
                // o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = v.normal;
                o.uv = v.uv;
                return o;
            }

            // bool 0 1
            // int 
            // float (32 bit float)
            // half (16 bit float)
            // fixed (lower precision) -1 to 1
            // float4 -> half4 -> fixed4

            float4 frag (v2f i) : SV_Target
            {
                // sample the texture
                // fixed4 col = tex2D(_MainTex, i.uv);
                fixed shadow = SHADOW_ATTENUATION(i);
                float4 col = shadow.xxxx;
                return col;
            }
            ENDCG
        }
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}