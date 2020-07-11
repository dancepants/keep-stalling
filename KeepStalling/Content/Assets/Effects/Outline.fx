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

static const float PI = 3.14159265;

float Size;
float TextureWidth;
float TextureHeight;
float3 Color;
float Transparency;

float4 CreateOutline(VertexShaderOutput input) : COLOR
{
	float xOffset = Size / TextureWidth;
	float yOffset = Size / TextureHeight;
	float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates);
	float alpha = color.a;

	alpha += tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(-xOffset, -yOffset)).a;
	alpha += tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(0, -yOffset)).a;
	alpha += tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(xOffset, -yOffset)).a;
	alpha += tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(xOffset, 0)).a;
	alpha += tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(xOffset, yOffset)).a;
	alpha += tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(0, yOffset)).a;
	alpha += tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(-xOffset, yOffset)).a;
	alpha += tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(-xOffset, 0)).a;

	float4 result = float4(0, 0, 0, 0);

	if (alpha > 0.0)
	{
		result = float4(Color, 1 - Transparency);
	}

	return result;
}

float4 NormalPass(VertexShaderOutput input) : COLOR
{
	return tex2D(SpriteTextureSampler, input.TextureCoordinates) * input.Color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL CreateOutline();
	}

	pass P1
	{
		PixelShader = compile PS_SHADERMODEL NormalPass();
	}
};