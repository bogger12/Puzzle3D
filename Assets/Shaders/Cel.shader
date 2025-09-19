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
        _IgnoreForwardAddShadow ("Ignore Forward Add Shadow", Integer) = 1
    }
    SubShader
    {

        Pass
        {
            Stencil
            {
                Ref 11
                Comp Always
                Pass Replace
            }

            Tags {"LightMode"="ForwardBase"  "RenderType"="Opaque" }
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
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
                UNITY_FOG_COORDS(2)
                float3 wPos : TEXCOORD3;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float3 _LightColor;
            float3 _ShadowColor;
            float _Threshold;
            float _Fresnel;
            int _UseDiffuse;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.wPos = mul(unity_ObjectToWorld, v.vertex);
                // compute shadows data
                TRANSFER_SHADOW(o);
                UNITY_TRANSFER_FOG(o, o.pos); 
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
                float4 color = lambert*shadow >= _Threshold ? float4(_LightColor,1) : float4(_ShadowColor,1);
                UNITY_APPLY_FOG(i.fogCoord, color); 
                return color;
            }
            ENDHLSL
        }
        
        // Shadow pass that only shadows for main directional light
        Pass
        {
            Tags {"LightMode"="ShadowCaster"}

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            #include "UnityCG.cginc"

            struct v2f { 
                V2F_SHADOW_CASTER;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                return o;
            }

            int _IgnoreForwardAddShadow;

            float4 frag(v2f i) : SV_Target
            {
                // Kill if this shadowmap render is for a point light
                #ifdef SHADOWS_CUBE
                    if (_IgnoreForwardAddShadow) discard;
                #endif

                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDHLSL
        }
    }
}
