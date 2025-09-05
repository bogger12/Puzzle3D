Shader "Custom/Vine"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BurnTexture ("Burn Texture", 2D) = "white" {}
        _BurnAmount ("Burn Amount", Range(0,2)) = 0
        _BaseVSBurnTex ("Base Tex VS Burn Tex", Range(0,1)) = 0.5
        }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alpha:fade addshadow

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BurnTexture;

        struct Input
        {
            float2 uv_MainTex;
        };

        float _BurnAmount;
        float _BaseVSBurnTex;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input i, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, i.uv_MainTex);
            fixed4 b = tex2D (_BurnTexture, i.uv_MainTex + float2(0,-(_BurnAmount+_Time.x*10)));
            float4 burnedTex = lerp(c, b, _BaseVSBurnTex);
            o.Smoothness = 0;
            o.Alpha = c.a;

            float maxDistance = 0.1;
            float signedDistanceToBurnLevel = ((_BurnAmount*(1+maxDistance))-maxDistance)-i.uv_MainTex.y;
            float distanceToBurnLevel = abs(signedDistanceToBurnLevel);
            if (signedDistanceToBurnLevel>0 && signedDistanceToBurnLevel <= 1) {
                o.Albedo = burnedTex.rgb;
            } else {
                if (signedDistanceToBurnLevel > 1+maxDistance) discard;
                if (distanceToBurnLevel<maxDistance) {
                    o.Albedo = lerp(burnedTex.rgb, c.rgb, smoothstep(0,maxDistance,distanceToBurnLevel));
                } else if (abs(signedDistanceToBurnLevel-1)<maxDistance) {
                    o.Albedo = lerp(burnedTex.rgb, c.rgb, smoothstep(0,maxDistance,abs(signedDistanceToBurnLevel-1)));
                    o.Alpha = smoothstep(maxDistance,0,abs(signedDistanceToBurnLevel-1));
                }
                else o.Albedo = c.rgb;
                
            }
            
        }
        ENDCG
    }
    FallBack "Diffuse"
}
