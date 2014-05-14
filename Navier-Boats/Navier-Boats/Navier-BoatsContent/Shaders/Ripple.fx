
// Texture Samplers
sampler mainColor : register(s0);
sampler normalMap : register(s1);

float time;
bool water;


float4 PixelShaderFunction(float4 color : COLOR0, float2 uv : TEXCOORD0) : COLOR0
{

	float4 finalColor = tex2D(mainColor, uv);

			// Sample the normal map
			float2 normalUVup = uv;
			float2 normalUVdown = uv;

			normalUVup.x += time * 0.045f;
			normalUVdown.x += time * 0.015f;
				
			normalUVup.y += time * 0.015f;
			normalUVdown.y += time * 0.045f;
	
			float4 directionUp = tex2D(normalMap, normalUVup) * 2 -1;
			float4 directionDown = tex2D(normalMap, normalUVdown) * 2 -1;

			// Average the two directions
			float4 direction = normalize((directionUp + directionDown) / 2);
	
			float4 waterTint = tex2D(normalMap, uv + direction.xy * 0.02f) * direction * 5.5;
			waterTint.a = 1.0;
			waterTint.b *= .85;

			float3 lightDir = normalize(float3(1,1,1));
			float lightAmount = saturate( dot(lightDir, direction) * 100 );
	
			lightAmount *= (sin( 2 * time ) + 2.0f)/2;

			// Use the normal map's XY direction to alter
			// where we sample the actual main texture
		    finalColor = tex2D(mainColor, uv + direction.xy * 0.02f) * waterTint;
			finalColor.rg *= lightAmount;
			finalColor.a=1;


			finalColor *= 1.25;
		

	// All done
	return finalColor;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
