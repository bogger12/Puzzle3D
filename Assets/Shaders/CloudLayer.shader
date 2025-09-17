Shader "Custom/CloudLayer"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _ScrollSpeed("Scroll Speed", Vector) = (1,1,1,1)
        _MultNoise ("Noise Multiply", 2D) = "white" {}
        _MultNoiseAdd ("Noise Multiply Add", Range(0,1)) = 0.5
        _MultNoiseScrollSpeed("Noise Mult Scroll Speed", Vector) = (1,1,1,1)
        _CloudThreshold1("Cloud Threshold 1", Float) = 0.2
        _CloudThreshold2("Cloud Threshold 2", Float) = 0.6
        _DistanceThreshold("DistanceThreshold", Range(0,0.5)) = 0.1
        _MaxDistance("Max Distance", Float) = 100
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Opaque" }
        LOD 200
        Cull Off

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard alpha:fade

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _MultNoise;

        float4 _MainTex_ST;   // Unity auto-populates this (_MainTex tiling/offset)
        float4 _MultNoise_ST;   // Unity auto-populates this (_MainTex tiling/offset)

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        float _CloudThreshold1;
        float _CloudThreshold2;
        float _DistanceThreshold;
        float2 _ScrollSpeed;
        float2 _MultNoiseScrollSpeed;
        float _MultNoiseAdd;

        float _MaxDistance;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 scrolleduvs = IN.worldPos.xz*(_MainTex_ST/10) + _ScrollSpeed*_Time.x;
            float2 scrolledmultuvs = IN.worldPos.xz*(_MultNoise_ST/10) + _MultNoiseScrollSpeed*_Time.x;
            float mask = tex2D (_MainTex, scrolleduvs);
            mask *= tex2D (_MultNoise, scrolledmultuvs)+_MultNoiseAdd;
            float threshDistance = min(abs(_CloudThreshold1-mask),abs(_CloudThreshold2-mask));

            if (threshDistance>_DistanceThreshold) discard;

            float viewDistance = distance(_WorldSpaceCameraPos,IN.worldPos);
            clip(_MaxDistance-viewDistance);

            // Albedo comes from a texture tinted by color
            // fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = _Color;
            // Metallic and smoothness come from slider variables
            o.Metallic = 0.0;
            o.Smoothness = 0.0;
            o.Alpha = min(smoothstep(_MaxDistance, 50, viewDistance), smoothstep(5, 15, viewDistance));

            // kill fragments below cutoff for shadows
            clip(o.Alpha - _DistanceThreshold);
        }
        ENDCG
    }
    // FallBack "Transparent/Cutout/VertexLit"
}
