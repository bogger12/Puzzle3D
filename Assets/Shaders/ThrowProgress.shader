Shader "Unlit/ThrowProgress"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Progress ("Progress", Range(0,1)) = 0
        _ColorFilled("ColorFilled", Color) = (0,1,0,1)
        _ColorEmpty("ColorEmpty", Color) = (0.5,0.5,0.5,1)
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Progress;
            float3 _ColorEmpty;
            float3 _ColorFilled;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // return float4(i.uv, 0, 1);
                

                float distanceFromCenter = distance(float2(0.5,0.5), i.uv);
                float angleFromCenterDegrees = atan2(-i.uv.x+0.5,-i.uv.y+0.5) / (UNITY_PI*2) + 0.5;

                if (distanceFromCenter > 0.2 && distanceFromCenter < 0.5) {
                    float3 col = angleFromCenterDegrees<_Progress ? _ColorFilled : _ColorEmpty;
                    return float4(col, 1);
                } else discard;
                return float4(1,1,0,1);
            }
            ENDCG
        }
    }
}
