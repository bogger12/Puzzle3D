Shader "Unlit/BombStrobe"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR] _StrobeColor ("Strobe Color", Color) = (1,1,1,1)
        _FuseProgress ("Fuse Progress", Range(0,1)) = 0
        _MinStrobeSpeed ("Min Strobe Speed", Float) = 1
        _MaxStrobeSpeed ("Max Strobe Speed", Float) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float3 _StrobeColor;
            float _FuseProgress;
            float _MinStrobeSpeed;
            float _MaxStrobeSpeed;

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

                float strobeSpeed = lerp(_MinStrobeSpeed, _MaxStrobeSpeed, _FuseProgress);
                float visibility = cos(_FuseProgress*strobeSpeed*UNITY_PI*2-UNITY_PI)*0.5+0.5;
                return float4(_StrobeColor, visibility);

            }
            ENDCG
        }
    }
}
