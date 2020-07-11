#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float2 Direction;
float Size;
float TextureWidth;
float TextureHeight;
float3 Color;
float Transparency;

float4 CreateShadow(VertexShaderOutput input) : COLOR
{
	float2 scaledSize = float2(Size / TextureWidth, Size / TextureHeight);
	float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates);
	float alpha = color.a;

	alpha += tex2D(SpriteTextureSampler, input.TextureCoordinates - scaledSize * Direction).a;

	if (alpha >= 1) return float4(Color, 1 - Transparency); 

	return float4(0, 0, 0, 0);
}

float4 NormalPass(VertexShaderOutput input) : COLOR
{
	return tex2D(SpriteTextureSampler, input.TextureCoordinates) * input.Color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL CreateShadow();
	}
	pass P1
	{
		PixelShader = compile PS_SHADERMODEL NormalPass();
	}
};