Shader "Unlit/Waterfall"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (0,0,1,1)
        _SecondaryColor ("Secondary Color", Color) = (1,0,1,1)
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _FoamWidth ("Foam Width", Float) = 0.1
        _FoamColor ("Foam Color", Color) = (1,1,1,1)
        _SolidFoam ("Foam is Solid", Range(0,1)) = 0
        _WiggleIntensity ("Wiggle Intensity", Float) = 1
        _WiggleLength ("Wiggle Length", Float) = 1
        _WiggleSpeed ("Wiggle Speed", Float) = 100
        _FlowSpeed ("Flow Speed", Float) = 1

        _TopMaskLevel ("Top Mask Level", Range(0,1)) = 0.3
        _BottomMaskLevel ("Bottom Mask Level", Range(0,1)) = 0.3
        _StepLevels ("Step Levels", Vector) = (1.5, 0.1, 0, 0)
        
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }

        Pass {
            ZWrite On
            ColorMask 0
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha 
            ZTest LEqual
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 tangent : TANGENT; // Tangent vector
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 wPos : TEXCOORD1;
                float3 tangent : TANGENT; // Tangent vector
                float3 normal : NORMAL;
            };

            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;

            #define NUM_OBJECTS 4
            float4 _DisplacementObjects[NUM_OBJECTS];

            float _FoamWidth;
            float3 _FoamColor;
            float _SolidFoam;
            float _WiggleIntensity;
            float _WiggleLength;
            float _WiggleSpeed;
            float _FlowSpeed;

            float _TopMaskLevel;
            float _BottomMaskLevel;

            float3 _MainColor;
            float3 _SecondaryColor;

            float2 _StepLevels;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.tangent = normalize(mul((float3x3)unity_ObjectToWorld, v.tangent.xyz));
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.wPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            float withinDisplacementArea(float3 objectDistance, float maxDistance) {
                float displacementMask = smoothstep(maxDistance, 0, length(objectDistance)) * (objectDistance.y <= 0) + 
                    smoothstep(maxDistance, 0, length(objectDistance.xz)) * (objectDistance.y > 0);
                return saturate(displacementMask);
            }

            fixed4 frag (v2f i) : SV_Target
            {

                float waterNoiseMask = smoothstep(0,1,saturate((_TopMaskLevel-abs(1-i.uv.y))*(1/_TopMaskLevel))) + smoothstep(_BottomMaskLevel,0,i.uv.y);

                // return float4(waterNoiseMask.xxx,1);
                float horizontalOffset = sin(i.uv.x+_Time.x*_WiggleSpeed*100+i.wPos.y*_WiggleLength)*_WiggleIntensity;;


                i.uv.y += _Time.x * _FlowSpeed;
                i.uv.x += horizontalOffset*0.1;
                float3 noise = tex2D(_NoiseTex, TRANSFORM_TEX(i.uv, _NoiseTex));


                noise = saturate(smoothstep(_StepLevels.x, _StepLevels.y, waterNoiseMask+noise));


                float waterMask = 0;

                float3 newWorldPos = i.wPos.xyz + i.tangent*horizontalOffset;

                for (int ob=0; ob<NUM_OBJECTS; ob++) {
                    float3 objectDistance = _DisplacementObjects[ob].xyz-newWorldPos;
                    float displacementDistance = 0.7;
                    float insideDisplacedWater = withinDisplacementArea(objectDistance, displacementDistance);
                    float insideDisplacedWaterFoam = withinDisplacementArea(objectDistance, displacementDistance + _FoamWidth);
                    // return float4(insideDisplacedWater.xxx, 1);
                    float withinBelowWater = withinDisplacementArea(objectDistance, displacementDistance + _FoamWidth);
                    if (withinDisplacementArea(objectDistance, displacementDistance)>0) return float4(0,0,0,0); // Transparent
                    else if (withinBelowWater>0) { 
                        waterMask = max(smoothstep(_SolidFoam, _FoamWidth/2, withinBelowWater), waterMask); 
                    }
                }
                
                // return float4(waterMask.xxx, 1);
                fixed4 output = float4(lerp(lerp(_SecondaryColor, _MainColor, noise),_FoamColor, waterMask), 1);
                

                return output;
            }
            ENDCG
        }
    }
}
