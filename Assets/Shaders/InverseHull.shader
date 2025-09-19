Shader "Unlit/InverseHull"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,0,0,1)
        _OutlineWidth ("OutlineWidth", Range(0,1)) = 1.0
        _NormalVSWorldSpace ("NormalVSWorldSpace", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque" }
        LOD 100

        Pass
        {

            Cull Front           // inverse hull
            ZWrite On           // don't write to depth
            ZTest LEqual         // still compare against scene depth

            Stencil
            {
                Ref 1
                Comp NotEqual        // only draw where stencil != 1 (outside base mesh)
                Pass Keep
            }

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
            float4 _Color;
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
                return _Color;
            }
            ENDCG
        }
    }
}
