Shader "Unlit/FocusPoint"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FocusAmount ("Focus Amount", Range(0,1)) = 0
        _FocusWidth ("Focus Width", Float) = 0.1
        _CenterRadius ("Center Radius", Float) = 0.1
        _OscillationSpeed ("Oscillation Speed", Range(1,10)) = 1
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
            float _FocusAmount;
            float _FocusWidth;
            float _CenterRadius;
            float _OscillationSpeed;

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

                // Get polar coordinates
                float d = distance(i.uv, float2(0.5,0.5));

                float offset = sin(_Time.y*_OscillationSpeed)/2 + 0.5;
                // return float4(offset.xxx,1);
                float focusAmount = _FocusAmount + lerp(0, 0.2, offset);
                // return float4((focusAmount>i.uv.y).xxx,1);

                float focusDistance = (1-focusAmount)*(0.5-_FocusWidth);
                if (step(d,_CenterRadius)) return float4(1,1,1,1);
                else if (focusDistance<=d && d<=(focusDistance+_FocusWidth)) return float4(1,1,1,1);

                discard;

                return col;
            }
            ENDCG
        }
    }
}
