Shader "Lit/Player Dithered Lit"
{
    Properties
    {
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
        _DitherValue ("DitherValue", Float) = 0.5
        _DitheredColor ("DitheredColor", Color) = (1,1,1,1)
        _ShadeThreshold ("ShadeThreshold", Float) = 0.3
        _HighlightThreshold ("HighlightThreshold", Float) = 0.8
    }
    SubShader
    {

        Pass
        {
            Tags {"LightMode"="ForwardBase" "Queue"="Geometry"}
            ZWrite On

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
                // fixed3 diff : COLOR0;
                // fixed3 ambient : COLOR1;
                float4 pos : SV_POSITION;
                float4 screenPos : TEXCOORD2;
                float3 normal : NORMAL;
                float3 wPos : TEXCOORD3;
            };
            v2f vert (MeshData v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.normal = UnityObjectToWorldNormal(v.normal);
                // compute shadows data
                TRANSFER_SHADOW(o);
                o.screenPos = ComputeScreenPos(o.pos);
                o.wPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            sampler2D _MainTex;

            // Cel Shading
            float _ShadeThreshold;
            float _HighlightThreshold;

            fixed4 frag (v2f i) : SV_Target
            {
                float2 screenUV = i.screenPos.xy / i.screenPos.w;

                fixed4 col = tex2D(_MainTex, i.uv);
                // compute shadow attenuation (1.0 = fully lit, 0.0 = fully shadowed)
                fixed shadow = SHADOW_ATTENUATION(i);
                // darken light's illumination with shadow, keep ambient intact

                // Lighting calculation
                
                float3 N = normalize(i.normal);
                float3 L = normalize(_WorldSpaceLightPos0.xyz); // vector from surface to light source
                float3 diff = saturate(dot( N, L));

                // specular lighting
                float3 V = normalize(_WorldSpaceCameraPos - i.wPos);
                float3 H = normalize(L + V);
                // float3 R = reflect(-L, N); // used for Phong

                float3 spec = saturate(dot(H,N)) * (diff > 0); // Blinn-Phong

                float specularExponent = exp2(0.5*8) + 2;
                spec = pow(spec, specularExponent);

                float3 ambient = 0.1;

                float lighting = (diff * shadow + ambient + spec * shadow);

                // Cel Shading Quantization
                if (lighting < _ShadeThreshold) lighting = 0.1;
                else if (lighting >= _HighlightThreshold) lighting = 1;
                else lighting = 0.5;

                col.rgb *= _LightColor0.rgb * lighting;
                return col;
            }
            ENDHLSL
        }

        Pass
        {
            Tags {"LightMode"="ForwardAdd" "Queue"="Geometry"}
            ZWrite On
            Blend One One
            BlendOp Max

            // ZTest Always

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            // compile shader into multiple variants, with and without shadows
            // (we don't care about any lightmaps yet, so skip these variants)
            #pragma multi_compile_fwdadd_fullshadows
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
                // fixed3 diff : COLOR0;
                // fixed3 ambient : COLOR1;
                float4 pos : SV_POSITION;
                float4 screenPos : TEXCOORD2;
                float3 normal : NORMAL;
                float3 wPos : TEXCOORD3;
            };
            v2f vert (MeshData v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.normal = UnityObjectToWorldNormal(v.normal);
                // compute shadows data
                TRANSFER_SHADOW(o);
                o.screenPos = ComputeScreenPos(o.pos);
                o.wPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            sampler2D _MainTex;

            // Cel Shading
            float _ShadeThreshold;
            float _HighlightThreshold;

            fixed4 frag (v2f i) : SV_Target
            {
                float2 screenUV = i.screenPos.xy / i.screenPos.w;

                fixed4 col = tex2D(_MainTex, i.uv);
                // compute shadow attenuation (1.0 = fully lit, 0.0 = fully shadowed)
                fixed shadow = SHADOW_ATTENUATION(i);
                // darken light's illumination with shadow, keep ambient intact

                // Lighting calculation
                
                float3 N = normalize(i.normal);
                float3 L = normalize(_WorldSpaceLightPos0.xyz-i.wPos); // vector from surface to light source
                float3 diff = saturate(dot( N, L));

                // specular lighting
                float3 V = normalize(_WorldSpaceCameraPos - i.wPos);
                float3 H = normalize(L + V);
                // float3 R = reflect(-L, N); // used for Phong

                float3 spec = saturate(dot(H,N)) * (diff > 0); // Blinn-Phong

                float specularExponent = exp2(0.5*8) + 2;
                spec = pow(spec, specularExponent);

                float3 ambient = 0.1;

                float lighting = (diff * shadow + ambient + spec * shadow);

                // Cel Shading Quantization
                if (lighting < _ShadeThreshold) lighting = 0.1;
                else if (lighting >= _HighlightThreshold) lighting = 1;
                else lighting = 0.5;

                col.rgb *= _LightColor0.rgb * lighting;
                return col;
            }
            ENDHLSL
        }
        Pass
        {
            Tags {"LightMode"="Always" "Queue"="Transparent+1"}
            ZWrite Off
            ZTest Greater
            // This tests if 10 is Greater than the current value of the stencil buffer
            Stencil
            {
                Ref 10
                Comp Greater
                Pass Replace
            } 

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct MeshData // per vertex data
            {
                float4 vertex : POSITION; // vertex position
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };
            v2f vert (MeshData v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

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
                float color = _DitherValue;
                float2 pixelxy = i.pos.xy;
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
