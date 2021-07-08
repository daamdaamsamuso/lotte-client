sampler2D input : register(s0);

float4 colorKey : register(c0);
float threshold : register(c1);

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 color; 
	color = tex2D( input , uv.xy); 
	
	if(colorKey.r - color.r <= threshold && colorKey.g - color.g <= threshold && colorKey.b - color.b <= threshold)
	{
		color.rgba = 0;
	}

    return color; 
}