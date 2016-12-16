cbuffer Matrices : register(b0)
{
	float4x4	WorldViewProj						: packoffset(c0);
	float4x4	World								: packoffset(c4);
	float4x4	WorldInverseTranspose				: packoffset(c8);
};

cbuffer Parameters : register(b1)
{
	float				Threshold : packoffset(c0.x);
	float			    BurnSize : packoffset(c0.y);
};

Texture2D DiffuseTexture 			: register(t0);
Texture2D OpacityRamp				: register(t1);
Texture2D BurnTexture				: register(t2);
SamplerState Sampler1			 	: register(s0);
SamplerState Sampler2			 	: register(s1);
SamplerState Sampler3 =
sampler_state
{
	AddressU = Clamp;
	AddressV = Clamp;
};

struct VS_IN
{
	float4 Position : POSITION;	
	float2 TexCoord1 : TEXCOORD0;
	float2 TexCoord2 : TEXCOORD1;
};

struct VS_OUT
{
	float4 Position : SV_POSITION;
	float2 TexCoord1 : TEXCOORD0;
	float2 TexCoord2 : TEXCOORD1;
};


VS_OUT vsDisappear(VS_IN input)
{
	VS_OUT output = (VS_OUT)0;

	output.Position = mul(input.Position, WorldViewProj);
	output.TexCoord1 = input.TexCoord1;
	output.TexCoord2 = input.TexCoord2;

	return output;
}

float4 psDisappear(VS_OUT input) : SV_Target0
{
	float4 color = DiffuseTexture.Sample(Sampler1, input.TexCoord1);
	float4 ramp = OpacityRamp.Sample(Sampler2, input.TexCoord2);

	half test = ramp.x - Threshold;

	clip(test);

	if (test < BurnSize && Threshold > 0 && Threshold < 1)
	{
		color += BurnTexture.Sample(Sampler3, float2(test * (1 / BurnSize), 0));
	}

	return color;
}