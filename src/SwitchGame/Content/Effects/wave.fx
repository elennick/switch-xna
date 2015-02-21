float2 DisplacementScroll;
sampler2D input : register(s0); 

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
    float4 Color; 
	uv.x = uv.x + (cos(uv.y*20)*(DisplacementScroll / 30)); 
	Color = tex2D( input , uv.xy);
	
    return Color; 
}

technique Wave
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 main();
    }
}