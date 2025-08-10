Shader "Unlit/Flame"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FlameTex ("Flame Texture", 2D) = "white" {}
        _ScaleX ("Scale X", Float) = 1
        _ScaleY ("Scale Y", Float) = 1

        _Color1 ("Color1", Color) = (1,0,0,1)
        _Color2 ("Color2", Color) = (1,0,0,1)
        
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
            // make fog work
            #pragma multi_compile_fog

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
            sampler2D _FlameTex;
            float _ScaleX;
            float _ScaleY;
            float3 _Color1;
            float3 _Color2;

            const float3 vect3Zero = float3(0.0, 0.0, 0.0);
            v2f vert (appdata v)
            {
                v2f o;
                // o.vertex = UnityObjectToClipPos(v.vertex + float3(0,_ScaleX,));

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                float4 camPos = float4(UnityObjectToViewPos(vect3Zero).xyz, 1.0);    // UnityObjectToViewPos(pos) is equivalent to mul(UNITY_MATRIX_MV, float4(pos, 1.0)).xyz,
                                                                                    // This gives us the camera's origin in 3D space (the position (0,0,0) in Camera Space)

                float4 viewDir = float4(v.vertex.x, v.vertex.y-0.5, 0.0, 0.0) * float4(_ScaleX, _ScaleY, 1.0, 1.0);            // Since w is 0.0, in homogeneous coordinates this represents a vector direction instead of a position
                float4 outPos = mul(UNITY_MATRIX_P, camPos - viewDir);            // Add the camera position and direction, then multiply by UNITY_MATRIX_P to get the new projected vert position

                o.vertex = outPos;
                o.uv = v.uv;

                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // return float4((sin(_Time.x*20)/2+0.5).xxx, 1);
                // float verticalOffset = sin(_Time.x*100+i.uv.y) + sin(_Time.x*50+i.uv.y) + sin(_Time.x*225+i.uv.y);

                float horizontalOffset = sin(((i.uv.y)-_Time.x*10)*10)*0.05;

                float2 warpeduvs = i.uv*float2(1.10,1) - float2(0.05,0) + float2(horizontalOffset, 0);
                // sample the texture
                if (any(warpeduvs < 0.0) || any(warpeduvs > 1.0)) discard;
                fixed4 col = tex2D(_MainTex, warpeduvs);
                // return col;
                if (col.r > 0.5) discard;
                warpeduvs.y = i.uv.y;

                float flameColor = tex2D(_FlameTex, warpeduvs + float2(sin(((i.uv.y))*10)*0.05,-_Time.x*10)).x;
                
                return float4(lerp(_Color1, _Color2, flameColor), 1);
                // } else discard;
                return float4(0,0,0,0);
            }
            ENDCG
        }
    }
}
