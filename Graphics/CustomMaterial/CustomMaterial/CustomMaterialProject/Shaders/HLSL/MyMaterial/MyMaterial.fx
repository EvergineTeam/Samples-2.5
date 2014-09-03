cbuffer Matrices : register(b0)
{
    float4x4	WorldViewProj						: packoffset(c0);
    float4x4	World								: packoffset(c4);
    float4x4	WorldInverseTranspose				: packoffset(c8);
};

cbuffer Parameters : register(b1)
{
    float				Time				: packoffset(c0.x);
};

Texture2D DiffuseTexture 			: register(t0);
SamplerState DiffuseTextureSampler 	: register(s0);

struct VS_IN
{
	float4 Position : POSITION;
	float3 Normal	: NORMAL0;
	float2 TexCoord : TEXCOORD0;
};

struct VS_OUT
{
	float4 Position : SV_POSITION;
	float2 TexCoord : TEXCOORD0;
};


VS_OUT vsMyMaterial( VS_IN input )
{
    VS_OUT output = (VS_OUT)0;

	float offsetScale = abs(sin(Time + (input.TexCoord.y * 16.0))) * 0.25;
	float4 vectorOffset = float4(input.Normal, 0) * offsetScale;
    output.Position = mul(input.Position + vectorOffset, WorldViewProj);
	output.TexCoord = input.TexCoord;

    return output;
}

float4 psMyMaterial( VS_OUT input ) : SV_Target0
{
    return DiffuseTexture.Sample(DiffuseTextureSampler, input.TexCoord);
}