
float mixInAmount : register(C0);

sampler2D input1 : register(S0);
sampler2D input2 : register(S1);

float4 main(float2 uv : TEXCOORD) : COLOR
{
	float4 color;
	color = tex2D(input1, uv.xy);

	float4 mapColor;

	mapColor = tex2D(input2, uv.xy);

	if (mapColor.r + mapColor.g + mapColor.b < mixInAmount)
	{
		mapColor.rgba = 0.0;
	}
	/*else
	{
	mapColor.g = 1.0;
	}*/
	color *= mapColor.r;

	return color;

}


