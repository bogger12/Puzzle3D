Shader "Hidden/MyCustomEffect"
{
  HLSLINCLUDE
// StdLib.hlsl holds pre-configured vertex shaders (VertDefault), varying structs (VaryingsDefault), and most of the data you need to write common effects.
      #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
      TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
      TEXTURE2D_SAMPLER2D(_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture);

      float3 SampleNormal(float2 uv)
{
#if defined(SOURCE_GBUFFER)
    float3 norm = SAMPLE_TEXTURE2D(_CameraGBufferTexture2, sampler_CameraGBufferTexture2, uv).xyz;
    norm = norm * 2 - any(norm); // gets (0,0,0) when norm == 0
    norm = mul((float3x3)unity_WorldToCamera, norm);
#if defined(VALIDATE_NORMALS)
    norm = normalize(norm);
#endif
    return norm;
#else
    float4 cdn = SAMPLE_TEXTURE2D(_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture, uv);
    return DecodeViewNormalStereo(cdn) * float3(1.0, 1.0, -1.0);
#endif
}
      
// Lerp the pixel color with the luminance using the _Blend uniform.
      float _Blend;
      float4 Frag(VaryingsDefault i) : SV_Target
      {
        //   float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
            float3 normal = SampleNormal(i.texcoord);
            return float4(normal,1);

          // Compute the luminance for the current pixel
//           float luminance = dot(color.rgb, float3(0.2126729, 0.7151522, 0.0721750));
//           color.rgb = lerp(color.rgb, luminance.xxx, _Blend.xxx);
// // Return the result
//           return color;
      }
  ENDHLSL
  SubShader
  {
      Cull Off ZWrite Off ZTest Always
      Pass
      {
          HLSLPROGRAM
              #pragma vertex VertDefault
              #pragma fragment Frag
          ENDHLSL
      }
  }
}

