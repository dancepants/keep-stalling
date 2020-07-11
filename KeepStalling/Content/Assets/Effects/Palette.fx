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

float3 Palette[4];

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates);

	if (color.a == 0) return color;

	float differences[4];
	int i = 0;

	[unroll(4)]
	for (i = 0; i < 4; i++)
	{
		differences[i] = abs(Palette[i].r - color.r) + abs(Palette[i].g - color.g) + abs(Palette[i].b - color.b);
	}

	int index = 0;

	[unroll(3)]
	for (i = 1; i < 4; i++)
	{
		if (differences[i] < differences[index])
		{
			index = i;
		}
	}
	
	return float4(Palette[index].rgb, 1);
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};