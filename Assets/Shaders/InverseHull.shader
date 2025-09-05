Shader "Unlit/InverseHull"
{
    Properties
    {
        _OutlineColor ("OutlineColor", Color) = (1,0,0,1)
        _OutlineWidth ("OutlineWidth", Range(0,1)) = 1.0
        _NormalVSWorldSpace ("NormalVSWorldSpace", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            // ZWrite On
            // ZTest LEqual 
            Cull Front
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            float _OutlineWidth;
            float4 _OutlineColor;
            float _NormalVSWorldSpace;

            v2f vert (appdata v)
            {
                v2f o;
                float4 objectPos = (v.vertex + v.vertex * _OutlineWidth)*(_NormalVSWorldSpace) + (v.vertex + float4(normalize(v.normal) * _OutlineWidth, 1)) * (1-_NormalVSWorldSpace);
                o.pos = UnityObjectToClipPos(objectPos);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }
    }
}
