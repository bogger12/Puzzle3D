Shader "Lit/Player Dithered Lit"
{
    Properties
    {
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
        _DitherValue ("DitherValue", Float) = 0.5
        _DitheredColor ("DitheredColor", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags {"LightMode"="ForwardBase" "Queue"="Geometry+1"}

        Pass
        {
            

            ZTest Always

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
                float4 screenPos : TEXCOORD2;

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
                TRANSFER_SHADOW(o);
                o.screenPos = ComputeScreenPos(o.pos);
                return o;
            }

            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;
            float _DitherValue;
            float4 _DitheredColor;

            static const float4x4 bayer_matrix_4x4 = {
                float4(0.0/16, 8.0/16, 2.0/16, 10.0/16),
                float4(12.0/16, 4.0/16, 14.0/16, 6.0/16),
                float4(3.0/16, 11.0/16, 1.0/16, 9.0/16),
                float4(15.0/16, 7.0/16, 13.0/16, 5.0/16)
            };

            fixed4 frag (v2f i) : SV_Target
            {
                float2 screenUV = i.screenPos.xy / i.screenPos.w;
                float sceneDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, screenUV);
                float sceneLinear = LinearEyeDepth(sceneDepth);
                float myLinearDepth = LinearEyeDepth(i.screenPos.z / i.screenPos.w);

                // return float4(myLinearDepth.xxx, 1);

                // _DitherValue = myLinearDepth / 5;

                // Render dither if object is further from camera than depth tex sample
                if (myLinearDepth > sceneLinear + 0.01) {
                    float color = _DitherValue;
                    float2 pixelxy = i.pos.xy;
                    float threshold = bayer_matrix_4x4[int(pixelxy.y)%4][int(pixelxy.x)%4];

                    if (color < threshold) discard;
                    else return _DitheredColor;
                }

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
        
        // shadow casting support
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
