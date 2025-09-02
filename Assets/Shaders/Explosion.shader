Shader "Unlit/Explosion"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR] _EdgeColor ("Edge Color", Color) = (1,1,1,1)
        _MinFresnel ("MinFresnel", Float) = 1
        _MaxFresnel ("MaxFresnel", Float) = 1
        _MinSize ("MinSize", Float) = 0
        _MaxSize ("MaxSize", Float) = 2
        _ExplodeProgress ("Explode Progress", Range(0,1)) = 1
        _TransparencyFade ("Transparency Fade", Range(0,1)) = 1

        _WarpTiling ("WarpTiling", Float) = 1
        _RefractionStrength ("Refraction Strength", Range (0, 0.2)) = 0
        _ScrollSpeed ("ScrollSpeed", Float) = 1

    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Opaque" }
        LOD 100

        GrabPass { "_GrabTexture" }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float3 wPos : TEXCOORD1;
                float4 grabPos : TEXCOORD2;
            };

            sampler2D _GrabTexture;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float3 _EdgeColor;

            float _MinFresnel;
            float _MaxFresnel;

            float _MinSize;
            float _MaxSize;
            float _ExplodeProgress;
            float _TransparencyFade;

            float _WarpTiling;
            float _RefractionStrength;
            float _ScrollSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                float vertexExtrude = lerp(_MinSize, _MaxSize, _ExplodeProgress);
                o.vertex = UnityObjectToClipPos(v.vertex * vertexExtrude);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.wPos = mul(unity_ObjectToWorld, v.vertex);
                o.grabPos = ComputeGrabScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                float3 distToCamera = _WorldSpaceCameraPos - i.wPos;
                float3 V = normalize(distToCamera);
                float3 N = normalize(i.normal);
                float fresnelExp = lerp(_MinFresnel, _MaxFresnel, _ExplodeProgress);
                float fresnel = pow(1.0f - dot(V,N), fresnelExp);

                float2 offset = float2(sin((i.grabPos.x*sin(_Time.x))*_WarpTiling), cos((i.grabPos.y+_Time.x*_ScrollSpeed)*_WarpTiling)) * _RefractionStrength;

                fresnel *= _TransparencyFade;
                offset *= _TransparencyFade;

                float2 screenUV = i.grabPos.xy / i.grabPos.w + offset / length(distToCamera);
                // return float4(length(distToCamera).xxx/10,1);

                float4 grabColor = tex2D(_GrabTexture, screenUV);

                return float4(grabColor.rgb*(1-fresnel) + _EdgeColor*fresnel, 1);
            }
            ENDCG
        }
    }
}
