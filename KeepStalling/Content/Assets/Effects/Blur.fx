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
float Offset;
float TextureWidth;
float TextureHeight;

float4 Blur(VertexShaderOutput input) : COLOR
{
	float2 scaledSize = float2(Offset / TextureWidth, Offset / TextureHeight);
	float2 center = scaledSize * 0.5;
	float4 color = float4(0, 0, 0, 0);

	color += tex2D(SpriteTextureSampler, input.TextureCoordinates + center + Direction * scaledSize * -5) * 0.035822;
	color += tex2D(SpriteTextureSampler, input.TextureCoordinates + center + Direction * scaledSize * -4) * 0.058790;
	color += tex2D(SpriteTextureSampler, input.TextureCoordinates + center + Direction * scaledSize * -3) * 0.086425;
	color += tex2D(SpriteTextureSampler, input.TextureCoordinates + center + Direction * scaledSize * -2) * 0.113806;
	color += tex2D(SpriteTextureSampler, input.TextureCoordinates + center + Direction * scaledSize * -1) * 0.134240;
	color += tex2D(SpriteTextureSampler, input.TextureCoordinates + center + Direction * scaledSize *  0) * 0.141836;
	color += tex2D(SpriteTextureSampler, input.TextureCoordinates + center + Direction * scaledSize *  1) * 0.134240;
	color += tex2D(SpriteTextureSampler, input.TextureCoordinates + center + Direction * scaledSize *  2) * 0.113806;
	color += tex2D(SpriteTextureSampler, input.TextureCoordinates + center + Direction * scaledSize *  3) * 0.086425;
	color += tex2D(SpriteTextureSampler, input.TextureCoordinates + center + Direction * scaledSize *  4) * 0.058790;
	color += tex2D(SpriteTextureSampler, input.TextureCoordinates + center + Direction * scaledSize *  5) * 0.035822;

	return color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL Blur();
	}
};