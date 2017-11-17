//-----------------------------------------------------------------------------
// LineMaterial.fx
//
// Copyright © 2015 Wave Engine S.L. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#include "..\Helpers.fxh"

cbuffer Matrices : register(b0)
{
	float4x4 WorldViewProj	: packoffset(c0);
};

cbuffer Parameters : register(b1)
{
	float3 CameraPosition		: packoffset(c0.x);
	float  Time					: packoffset(c0.w);
	float3 NoiseScale			: packoffset(c1.x);
	float Bias					: packoffset(c1.w);
	float3 TimeFactor			: packoffset(c2.x);
	float  UVShiftScale			: packoffset(c2.w);
	float2 UVShiftTimeFactor	: packoffset(c3.x);
	float2 TextureOffset		: packoffset(c3.z);

	float3 AnimationCenter			: packoffset(c4.x);
	float AnimationPlaybackTime		: packoffset(c4.w);
	float AnimationDisplace			: packoffset(c5.x);
	float AnimationScale			: packoffset(c5.y);
	float AnimationPlaybackRate		: packoffset(c5.z);
	float AnimationRotationFactor	: packoffset(c5.w);

	float2 TextureScale				: packoffset(c6.x);

	float4 TintColor				: packoffset(c7.x);
};

struct VS_IN_COLOR
{
	float4 Position			: POSITION;
	float4 Color			: COLOR;
	float2 TexCoord			: TEXCOORD0;	
	float4 AxisSize			: TEXCOORD1;
	float2 UVShift			: TEXCOORD2;
};

struct VS_OUT_COLOR
{
	float4 Position			: SV_POSITION;
	float4 Color 			: COLOR;
	float2 TexCoord			: TEXCOORD0;
};

Texture2D Texture			: register(t0);
SamplerState TextureSampler	: register(s0);

#if NOISE
Texture2D NoiseTexture				: register(t1);
SamplerState NoiseTextureSampler	: register(s1);
#endif // NOISE

#if UVSHIFT
Texture2D UVShiftTexture			: register(t2);
SamplerState UVShiftTextureSampler	: register(s2);
#endif // UV_SHIFT

#if APPEARING
Texture2D TimeShiftTexture				: register(t3);
SamplerState TimeShiftTextureSampler	: register(s3);
#endif // APPEARING

VS_OUT_COLOR vsLine(VS_IN_COLOR input)
{
	VS_OUT_COLOR output = (VS_OUT_COLOR)0;

	float3 position = input.Position;	
	float4 axisSize = input.AxisSize;
	float4 color = input.Color * TintColor;
	
	float3 forwardVector = CameraPosition - position;
	float3 upVector = normalize(axisSize.xyz);
	float3 rightVector = normalize(cross(forwardVector, upVector));
	float size = axisSize.w;

#if NOISE
	float3 noiseOffset = ComputeNoise(NoiseTexture, NoiseTextureSampler, position, NoiseScale, Time, TimeFactor);
	position += noiseOffset;
#endif
	
#if UVSHIFT
	float2 UVShift = input.UVShift;
	color *= ComputeUVShift(UVShiftTexture, UVShiftTextureSampler, UVShift, UVShiftScale, Time, UVShiftTimeFactor);
#endif

#if APPEARING
	ComputeAnimation(
		TimeShiftTexture,
		TimeShiftTextureSampler,
		position,
		color,
		AnimationCenter,
		AnimationScale,
		AnimationPlaybackTime,
		AnimationPlaybackRate,
		AnimationRotationFactor,
		AnimationDisplace,
		position,
		color);
#endif

	position += (rightVector * size);

	output.Position = mul(float4(position, 1.0), WorldViewProj);
	
#if BIAS
	output.Position.z += Bias;
#endif // BIAS

	output.Color = color;
	output.TexCoord = (input.TexCoord * TextureScale) + TextureOffset;

	return output;
}

float4 psLine(VS_OUT_COLOR input) : SV_Target0
{
	float4 texColor = Texture.Sample(TextureSampler, input.TexCoord);
	return texColor * input.Color;
}
