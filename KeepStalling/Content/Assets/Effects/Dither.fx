#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0
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

//static int meyer[16] = { 0, 8, 2, 10, 12, 4, 14, 6, 3, 11, 1, 9, 15, 7, 13, 5 };   
//
//float TextureWidth;
//float TextureHeight;
//
//float4 MainPS(VertexShaderOutput input) : COLOR
//{
//	float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates);
//	float4 closestColor = float4(1 ,1, 1, 1);
//	float4 secondClosestColor = float4(0, 0, 0, 1);
//
//	int x = int(fmod(input.TextureCoordinates.x * TextureWidth, 4.0));
//	int y = int(fmod(input.TextureCoordinates.y * TextureHeight, 4.0));
//	float d = meyer[y * 4 + x] / 16.0;
//	float distance = abs(closestColor.r - color.r);
//
//	return (distance < d) ? closestColor : secondClosestColor;
//}


struct ClosestsColors
{
	float4 Closest;
	float4 SecondClosest;
};
                                
static int meyer[64] = 
{ 
	 0, 32,  8, 40,  2, 34, 10, 42,
    48, 16, 56, 24, 50, 18, 58, 26,
    12, 44,  4, 36, 14, 46,  6, 38,
    60, 28, 52, 20, 62, 30, 54, 22,
     3, 35, 11, 43,  1, 33,  9, 41,
    51, 19, 59, 27, 49, 17, 57, 25,
    15, 47,  7, 39, 13, 45,  5, 37,
    63, 31, 55, 23, 61, 29, 53, 21 
};

float3 Palette[4];
float TextureWidth;
float TextureHeight;

ClosestsColors FindClosest (float4 color)
{
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

	ClosestsColors result;
	result.Closest = float4(Palette[index].rgb, 1);

	result.SecondClosest = float4(Palette[max(0, index - 1)].rgb, 1);
	if (index - 1 < 0) result.SecondClosest = float4(Palette[1].rgb, 1);

	//result.SecondClosest = float4(Palette[max(3, index + 1)].rgb, 1);
	//if (index + 1 > 3) result.SecondClosest = float4(Palette[3].rgb, 1);
	
	return result;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates);

	if (color.a == 0) return color;

	ClosestsColors result = FindClosest(color);

	int x = int(fmod(input.TextureCoordinates.x * TextureWidth, 8.0));
	int y = int(fmod(input.TextureCoordinates.y * TextureHeight, 8.0));
	float d = meyer[y * 8 + x] / 64.0;
	float distance = abs(result.Closest.r - color.r) + abs(result.Closest.g - color.g) + abs(result.Closest.b - color.b);

	return (distance < d) ? result.Closest : result.SecondClosest;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};