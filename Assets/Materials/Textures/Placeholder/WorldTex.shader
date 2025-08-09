Shader "Unlit/WorldTex"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            // compile shader into multiple variants, with and without shadows
            // (we don't care about any lightmaps yet, so skip these variants)
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            // shadow helper functions and macros
            #include "AutoLight.cginc"
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                SHADOW_COORDS(1) // put shadows data into TEXCOORD1
                float3 wPos : TEXCOORD2;
                float3 normal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                // compute shadows data
                TRANSFER_SHADOW(o);
                o.wPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // return float4(i.wPos, 1);
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.wPos.xz);

                fixed shadow = SHADOW_ATTENUATION(i);
                // darken light's illumination with shadow, keep ambient intact

                // Lighting calculation
                
                float3 N = normalize(i.normal);
                float3 L = _WorldSpaceLightPos0.xyz; // vector from surface to light source
                float3 lambert = saturate(dot( N, L));
                float3 diff = lambert * _LightColor0.xyz;

                // specular lighting
                float3 V = normalize(_WorldSpaceCameraPos - i.wPos);
                float3 H = normalize(L + V);
                // float3 R = reflect(-L, N); // used for Phong

                float3 spec = saturate(dot(H,N)) * (lambert > 0); // Blinn-Phong

                float specularExponent = exp2(0.5*8) + 2;
                spec = pow(spec, specularExponent);
                spec *= _LightColor0.rgb;

                float3 ambient = 0.1;

                float3 lighting = (diff * shadow + ambient + spec * shadow);
                
                return float4(col*lighting,1);
            }
            ENDCG
        }
    }
}
