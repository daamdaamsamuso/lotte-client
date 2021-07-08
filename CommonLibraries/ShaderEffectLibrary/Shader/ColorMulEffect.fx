sampler2D input : register(s0);
sampler2D mapTex : register(s1);

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 color; 
	color = tex2D( input , uv.xy); 
	
	float4 mapColor;
	mapColor = tex2D( mapTex , uv.xy);
	
	color *= mapColor.r;

    return color; 
}