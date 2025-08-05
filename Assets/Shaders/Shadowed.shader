Shader "Lit/Diffuse With Shadows"
{
    Properties
    {
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
        _DitherValue ("DitherValue", Float) = 0.5
        _DitheredColor ("DitheredColor", Color) = (1,1,1,1)
    }
    SubShader
    {
        Pass
        {
            Tags {"LightMode"="ForwardBase"}
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            // compile shader into multiple variants, with and without shadows
            // (we don't care about any lightmaps yet, so skip these variants)
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            // shadow helper functions and macros
            #include "AutoLight.cginc"


            struct MeshData // per vertex data
            {
                float4 vertex : POSITION; // vertex position
                float3 normal : NORMAL; // normal direction
                float2 uv : TEXCOORD0; // uv coords
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                SHADOW_COORDS(1) // put shadows data into TEXCOORD1
                fixed3 diff : COLOR0;
                fixed3 ambient : COLOR1;
                float4 pos : SV_POSITION;
                float2 viewPos : TEXCOORD2;
            };
            v2f vert (MeshData v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                o.diff = nl * _LightColor0.rgb;
                o.ambient = ShadeSH9(half4(worldNormal,1));
                // compute shadows data
                TRANSFER_SHADOW(o)
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // compute shadow attenuation (1.0 = fully lit, 0.0 = fully shadowed)
                fixed shadow = SHADOW_ATTENUATION(i);
                // darken light's illumination with shadow, keep ambient intact
                fixed3 lighting = i.diff * shadow + i.ambient;
                col.rgb *= lighting;
                return col;
            }
            ENDHLSL
        }
        
        Pass
        {
            Tags {"LightMode"="ForwardBase"}

            ZTest Greater

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct MeshData // per vertex data
            {
                float4 vertex : POSITION; // vertex position
                float3 normal : NORMAL; // normal direction
                float2 uv : TEXCOORD0; // uv coords
            };
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0; // uv coords
                float2 viewPos : TEXCOORD1;
            };
            v2f vert (MeshData v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.viewPos = UnityObjectToViewPos(v.vertex);
                return o;
            }

            float _DitherValue;
            float4 _DitheredColor;

            int bayer_n = 4;
            static const float4x4 bayer_matrix_4x4 = {
                float4(0.0/16, 8.0/16, 2.0/16, 10.0/16),
                float4(12.0/16, 4.0/16, 14.0/16, 6.0/16),
                float4(3.0/16, 11.0/16, 1.0/16, 9.0/16),
                float4(15.0/16, 7.0/16, 13.0/16, 5.0/16)
            };

            fixed4 frag (v2f i) : SV_Target
            {
                float2 viewPos = ((i.viewPos+1)/2);
                
                float color = _DitherValue;
                // float2 pixelxy = floor(i.vertex*50); // 20 pixels wide
                float2 pixelxy = i.vertex; // 20 pixels wide

                float threshold = bayer_matrix_4x4[int(pixelxy.y)%4][int(pixelxy.x)%4];

                if (color < threshold) discard;
                return _DitheredColor;
            }
            ENDHLSL
        }

        // shadow casting support
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
