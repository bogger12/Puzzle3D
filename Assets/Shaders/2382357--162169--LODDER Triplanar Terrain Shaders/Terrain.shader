Shader "Toon/Lit Tri Planar Normal" {
	Properties{
// set by terrain engine
		[HideInInspector] _Control ("Control (RGBA)", 2D) = "red" {}
		[HideInInspector] _Splat3 ("Layer 3 (A)", 2D) = "white" {}
		[HideInInspector] _Splat2 ("Layer 2 (B)", 2D) = "white" {}
		[HideInInspector] _Splat1 ("Layer 1 (G)", 2D) = "white" {}
		[HideInInspector] _Splat0 ("Layer 0 (R)", 2D) = "white" {}
		[HideInInspector] _Normal3 ("Normal 3 (A)", 2D) = "bump" {}
		[HideInInspector] _Normal2 ("Normal 2 (B)", 2D) = "bump" {}
		[HideInInspector] _Normal1 ("Normal 1 (G)", 2D) = "bump" {}
		[HideInInspector] _Normal0 ("Normal 0 (R)", 2D) = "bump" {}
		[HideInInspector] [Gamma] _Metallic0 ("Metallic 0", Range(0.0, 1.0)) = 0.0	
		[HideInInspector] [Gamma] _Metallic1 ("Metallic 1", Range(0.0, 1.0)) = 0.0	
		[HideInInspector] [Gamma] _Metallic2 ("Metallic 2", Range(0.0, 1.0)) = 0.0	
		[HideInInspector] [Gamma] _Metallic3 ("Metallic 3", Range(0.0, 1.0)) = 0.0
		[HideInInspector] _Smoothness0 ("Smoothness 0", Range(0.0, 1.0)) = 1.0	
		[HideInInspector] _Smoothness1 ("Smoothness 1", Range(0.0, 1.0)) = 1.0	
		[HideInInspector] _Smoothness2 ("Smoothness 2", Range(0.0, 1.0)) = 1.0	
		[HideInInspector] _Smoothness3 ("Smoothness 3", Range(0.0, 1.0)) = 1.0

		// used in fallback on old cards & base map
		// [HideInInspector] _MainTex ("BaseMap (RGB)", 2D) = "white" {}
		// [HideInInspector] _Color ("Main Color", Color) = (1,1,1,1)
	//	_Normals0 ("Normals0", Range(0.001, 5.0)) = 1
	//	_Normals1 ("Normals1", Range(0.001, 5.0)) = 1
	//	_Normals2 ("Normals2", Range(0.001, 5.0)) = 1
	//	_Normals3 ("Normals3", Range(0.001, 5.0)) = 1
		 _Color0 ("Color0", Color) = (1,1,1,1)
		 _Color1 ("Color1", Color) = (1,1,1,1)
		 _Color2 ("Color2", Color) = (1,1,1,1)
		 _Color3 ("Color3", Color) = (1,1,1,1)
		 _tiles0x ("tile0X", float) = 0.03
		 _tiles0y ("tile0Y", float) = 0.03
		 _tiles0z ("tile0Z", float) = 0.03
		 _tiles1x ("tile1X", float) = 0.03
		 _tiles1y ("tile1Y", float) = 0.03
		 _tiles1z ("tile1Z", float) = 0.03
		 _tiles2x ("tile2X", float) = 0.03
		 _tiles2y ("tile2Y", float) = 0.03
		 _tiles2z ("tile2Z", float) = 0.03
		 _tiles3x ("tile3X", float) = 0.03
		 _tiles3y ("tile3Y", float) = 0.03
		 _tiles3z ("tile3Z", float) = 0.03
		//  _offset0x ("offset0X", float) = 0
		//  _offset0y ("offset0Y", float) = 0
		//  _offset0z ("offset0Z", float) = 0
		//  _offset1x ("offset1X", float) = 0
		//  _offset1y ("offset1Y", float) = 0
		//  _offset1z ("offset1Z", float) = 0
		//  _offset2x ("offset2X", float) = 0
		//  _offset2y ("offset2Y", float) = 0
		//  _offset2z ("offset2Z", float) = 0
		//  _offset3x ("offset3X", float) = 0
		//  _offset3y ("offset3Y", float) = 0
		//  _offset3z ("offset3Z", float) = 0



		_Color("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex("Top Texture", 2D) = "white" {}
		_TopColor("Top Color", Color) = (1,1,1,1)
		// _NormalT("Top Normal", 2D) = "bump" {}
		_MainTexSide("Side/Bottom Texture", 2D) = "white" {}
		_SideColor("Side Color", Color) = (1,1,1,1)
		// _Normal("Side/Bottom Normal", 2D) = "bump" {}
		// _Ramp("Toon Ramp (RGB)", 2D) = "gray" {}
		_Noise("Noise", 2D) = "white" {}
		_Scale("Top Scale", Range(-2,2)) = 1
		_SideScale("Side Scale", Range(-2,2)) = 1
		_NoiseScale("Noise Scale", Range(-2,2)) = 1
		_TopSpread("TopSpread", Range(-2,2)) = 1
		_EdgeWidth("EdgeWidth", Range(0,0.5)) = 1
		_EdgeColor("Edge Color", Color) = (0.5,0.5,0.5,1)
		_RimPower("Rim Power", Range(-2,20)) = 1
		_RimColor("Rim Color Top", Color) = (0.5,0.5,0.5,1)
		_RimColor2("Rim Color Side/Bottom", Color) = (0.5,0.5,0.5,1)
		_DetailThreshold("Detail Threshold", Float) = 0.5
	}

		SubShader{
		Tags {
			"SplatCount" = "4"
			"Queue" = "Geometry-100"
			"RenderType" = "Opaque"
		}
		// LOD 200

		CGPROGRAM
// #pragma surface surf ToonRamp
		#pragma surface surf ToonRamp vertex:SplatmapVert finalcolor:SplatmapFinalColor finalgbuffer:SplatmapFinalGBuffer fullforwardshadows
		#pragma target 3.0
		// needs more than 8 texcoords
		#pragma exclude_renderers gles
		#include "UnityPBSLighting.cginc"

		#pragma multi_compile __ _TERRAIN_NORMAL_MAP
		#define TERRAIN_STANDARD_SHADER
		#define TERRAIN_SURFACE_OUTPUT SurfaceOutputStandard

		#include "TerrainSplatmapCustom.cginc"



		sampler2D _Ramp;

	// custom lighting function that uses a texture ramp based
	// on angle between light direction and normal
#pragma lighting ToonRamp exclude_path:prepass
	inline half4 LightingToonRamp(SurfaceOutputStandard s, half3 lightDir, half atten)
	{
#ifndef USING_DIRECTIONAL_LIGHT
		lightDir = normalize(lightDir);
#endif

		half d = dot(s.Normal, lightDir)*0.5 + 0.5;
		half3 ramp = tex2D(_Ramp, float2(d,d)).rgb;

		half4 c;
		// c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
		c.rgb = s.Albedo * _LightColor0.rgb * (atten * 2);
		c.a = 0;
		return c;
	}


	sampler2D _MainTex, _MainTexSide, _Normal, _Noise, _NormalT;
	float4 _Color, _RimColor, _RimColor2, _EdgeColor;
	float _RimPower;
	float  _TopSpread, _EdgeWidth;
	float _Scale, _SideScale, _NoiseScale;


	half _Metallic0;
	half _Metallic1;
	half _Metallic2;
	half _Metallic3;

    half _Smoothness0;
    half _Smoothness1;
    half _Smoothness2;
    half _Smoothness3;

	float3 _TopColor;
	float3 _SideColor;

	float _DetailThreshold;
	// struct Input {
	// 	float2 uv_MainTex : TEXCOORD0;
	// 	float3 worldPos; // world position built-in value
	// 	float3 worldNormal; INTERNAL_DATA // world normal built-in value
	// 		float3 viewDir;// view direction built-in value we're using for rimlight
	// };

	void surf(Input IN, inout SurfaceOutputStandard o) {

		// clamp (saturate) and increase(pow) the worldnormal value to use as a blend between the projected textures
		float3 worldNormalE = WorldNormalVector(IN, o.Normal);
		float3 blendNormal = saturate(pow(worldNormalE * 1.4,4));


		// normal noise triplanar for x, y, z sides
		float3 xn = tex2D(_Noise, IN.worldPos.zy * _NoiseScale);
		float3 yn = tex2D(_Noise, IN.worldPos.zx * _NoiseScale);
		float3 zn = tex2D(_Noise, IN.worldPos.xy * _NoiseScale);

		// lerped together all sides for noise texture
		float3 noisetexture = zn;
		noisetexture = lerp(noisetexture, xn, blendNormal.x);
		noisetexture = lerp(noisetexture, yn, blendNormal.y);

		// triplanar for top texture for x, y, z sides
		float3 xm = tex2D(_MainTex, IN.worldPos.zy * _Scale);
		float3 zm = tex2D(_MainTex, IN.worldPos.xy * _Scale);
		float3 ym = tex2D(_MainTex, IN.worldPos.zx * _Scale);

		// lerped together all sides for top texture
		float3 toptexture = zm;
		toptexture = lerp(toptexture, xm, blendNormal.x);
		toptexture = lerp(toptexture, ym, blendNormal.y);

		// triplanar for top normal for x, y, z sides

		float3 xnnt = UnpackNormal(tex2D(_NormalT, IN.worldPos.zy * _Scale));
		float3 znnt = UnpackNormal(tex2D(_NormalT, IN.worldPos.xy * _Scale));
		float3 ynnt = UnpackNormal(tex2D(_NormalT, IN.worldPos.zx * _Scale));

		// lerped together all sides for top normal
		float3 toptextureNormal = znnt;
		toptextureNormal = lerp(toptextureNormal, xnnt, blendNormal.x);
		toptextureNormal = lerp(toptextureNormal, ynnt, blendNormal.y);

		// triplanar for side normal for x, y, z sides
		float3 xnn = UnpackNormal(tex2D(_Normal, IN.worldPos.zy * _SideScale));
		float3 znn = UnpackNormal(tex2D(_Normal, IN.worldPos.xy * _SideScale));
		float3 ynn = UnpackNormal(tex2D(_Normal, IN.worldPos.zx * _SideScale));

		// lerped together all sides for side normal
		float3 sidetextureNormal = znn;
		sidetextureNormal = lerp(sidetextureNormal, xnn, blendNormal.x);
		sidetextureNormal = lerp(sidetextureNormal, ynn, blendNormal.y);


		// triplanar for side and bottom texture, x,y,z sides
		float3 x = tex2D(_MainTexSide, IN.worldPos.zy * _SideScale);
		float3 y = tex2D(_MainTexSide, IN.worldPos.zx * _SideScale);
		float3 z = tex2D(_MainTexSide, IN.worldPos.xy * _SideScale);

		// lerped together all sides for side bottom texture
		float3 sidetexture = z;
		sidetexture = lerp(sidetexture, x, blendNormal.x);
		sidetexture = lerp(sidetexture, y, blendNormal.y);



		// dot product of world normal and surface normal + noise
		float worldNormalDotNoise = dot(o.Normal + (noisetexture.y + (noisetexture * 0.5)), worldNormalE.y);

		// if dot product is higher than the top spread slider, multiplied by triplanar mapped top texture
		// step is replacing an if statement to avoid branching :
		// if (worldNormalDotNoise > _TopSpread{ o.Albedo = toptexture}
		float3 topTextureResult = step(_TopSpread + _EdgeWidth, worldNormalDotNoise) * toptexture * _TopColor;
		float3 topNormalResult = step(_TopSpread, worldNormalDotNoise) * toptextureNormal;

		// if dot product is lower than the top spread slider, multiplied by triplanar mapped side/bottom texture
		float3 sideTextureResult = step(worldNormalDotNoise, _TopSpread) * sidetexture * _SideColor;
		float3 sideNormalResult = step(worldNormalDotNoise, _TopSpread) * sidetextureNormal;

		// if dot product is in between the two, make the texture darker
		float3 topTextureEdgeResult = (step(_TopSpread , worldNormalDotNoise) * step(worldNormalDotNoise, _TopSpread + _EdgeWidth)) * _EdgeColor;


		half4 splat_control;
        half weight;
        fixed4 mixedDiffuse;
        half4 defaultSmoothness = half4(_Smoothness0, _Smoothness1, _Smoothness2, _Smoothness3);
        SplatmapMix(IN, defaultSmoothness, splat_control, weight, mixedDiffuse, o.Normal);

		// final normal
		// o.Normal = topNormalResult + sideNormalResult;
		// final albedo color 
		float3 triplanarTex = topTextureResult + sideTextureResult + topTextureEdgeResult;

		// float whiteValue = saturate(dot(mixedDiffuse.rgb, float3(0.333, 0.333, 0.333))*20);
		float maskValue = _DetailThreshold>0 ? int(mixedDiffuse.a>_DetailThreshold) : mixedDiffuse.a;
		o.Albedo = lerp(triplanarTex, mixedDiffuse.rgba, maskValue);
		// o.Albedo = triplanarTex+ mixedDiffuse.rgb;
		// o.Albedo = whiteValue.xxx;
		// o.Albedo = mixedDiffuse.rgb;
		// o.Albedo = mixedDiffuse.aaa;
		o.Albedo *= _Color;

		// o.Albedo = IN.worldPos/100;

		o.Alpha = weight;
		// o.Smoothness = mixedDiffuse.a;
		o.Metallic = dot(splat_control, half4(_Metallic0, _Metallic1, _Metallic2, _Metallic3));

		// adding the fuzzy rimlight(rim) on the top texture, and the harder rimlight (rim2) on the side/bottom texture
		// rim light for fuzzy top texture
		half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));

		// rim light for side/bottom texture
		half rim2 = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
		o.Emission = step(_TopSpread + _EdgeWidth, worldNormalDotNoise) * _RimColor.rgb * pow(rim, _RimPower) + step(worldNormalDotNoise, _TopSpread) * _RimColor2.rgb * pow(rim2, _RimPower);


	}
	ENDCG

	}

	// Dependency "AddPassShader" = "Hidden/TerrainEngine/Splatmap/Standard-AddPass"
	// Dependency "BaseMapShader" = "Hidden/TerrainEngine/Splatmap/Standard-Base"

	Fallback "Nature/Terrain/Diffuse"
}