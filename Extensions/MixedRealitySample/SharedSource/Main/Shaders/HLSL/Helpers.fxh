//-----------------------------------------------------------------------------
// Helpers.fxh
//
// Copyright © 2016 Wave Engine S.L. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------

// HELPERS
inline float3 ComputeNoise(Texture2D noiseTexture, SamplerState noiseTextureSampler, float3 position, float2 texCoord, float3 noiseScale, float time, float2 timeFactor)
{	
	float2 timeScale = texCoord + time * timeFactor;
	float3 noiseOffset = noiseTexture.SampleLevel(noiseTextureSampler, (texCoord + timeScale), 0) - float3(0.5f,0.5f,0.5f);

	noiseOffset *= noiseScale;

	return noiseOffset;
}

inline float4 ComputeUVShift(Texture2D uwShiftTexture, SamplerState uwShiftTextureSampler, float2 uw, float scale, float time, float2 timeFactor)
{
	float2 timeScale = time * timeFactor;
	return  uwShiftTexture.SampleLevel(uwShiftTextureSampler, (uw + timeScale), 0);
}

inline float4x4 CreateRotationY(float rotation)
{
	float s = sin(rotation);
	float c = cos(rotation);

	return float4x4(
		c,  0, -s,  0,
		0,  1,  0,  0,
		s,  0,  c,  0,
		0,  0,  0,  1);
}

inline void ComputeAnimation(
	Texture2D timeShiftTexture,
	SamplerState timeShiftTextureSampler,
	float3 position,
	float4 color,
	float3 animationCenter,
	float animationScale,
	float animationPlaybackTime,
	float animationPlaybackRate,
	float animationRotationFactor,
	float animationDisplace,
	out float3 newPosition,
	out float4 newColor 
	)
{
	animationCenter.y = position.y;
	float3 centerDir = animationCenter - position;
	float distance = length(centerDir) * animationScale;
	float timeShift = timeShiftTexture.SampleLevel(timeShiftTextureSampler, distance, 0).r;
	float lerp = smoothstep(0, 1, animationPlaybackTime - timeShift * animationPlaybackRate);
	float invLerp = 1 - lerp;
	float distanceFactor = clamp (0, 40, 1.0f / (0.0001f + timeShift));
	
	centerDir = mul(centerDir, CreateRotationY(invLerp * animationRotationFactor * distanceFactor));
	newPosition = animationCenter - (centerDir * (1 + (animationDisplace * saturate(invLerp * distanceFactor))));
	newColor = color * lerp;
}