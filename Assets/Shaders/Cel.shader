Shader "Lit/Cel Shade"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Threshold ("Threshold", Range(-1,1)) = 0.3
        _LightColor ("LightColor", Color) = (1,1,1,1)
        _ShadowColor ("ShadowColor", Color) = (0,0,0,1)
        _Fresnel ("Fresnel", Float) = 1
        _UseDiffuse ("Use Diffuse", Integer) = 1
    }
    SubShader
    {

        Pass
        {
            Tags {"LightMode"="ForwardBase"  "RenderType"="Opaque" }
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
                float3 normal : NORMAL;
                SHADOW_COORDS(1) // put shadows data into TEXCOORD1
                float3 wPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float3 _LightColor;
            float3 _ShadowColor;
            float _Threshold;
            float _Fresnel;
            float _UseDiffuse;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.wPos = mul(unity_ObjectToWorld, v.vertex);
                // compute shadows data
                TRANSFER_SHADOW(o);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed shadow = SHADOW_ATTENUATION(i);

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                float3 N = normalize(i.normal);
                float3 L = normalize(_WorldSpaceLightPos0.xyz); // vector from surface to light source
                float3 V = normalize(_WorldSpaceCameraPos - i.wPos);
                float fresnel = pow(dot(V,N), _Fresnel);
                float lambert = (dot( N, L)) * fresnel;
                if (!_UseDiffuse) lambert = fresnel;
                // return float4(fresnel.xxx, 1);
                return lambert*shadow >= _Threshold ? float4(_LightColor,1) : float4(_ShadowColor,1);
            }
            ENDHLSL
        }
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
