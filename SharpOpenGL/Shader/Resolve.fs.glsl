#version 450

out vec4 FragColor;
  
layout (location = 0) in vec2 TexCoords;

layout (binding = 0) uniform sampler2D ColorTex;
layout (binding = 1) uniform sampler2D BlurTex; 
layout (binding = 2) uniform sampler2D MotionTex; 


void main()
{
	vec4 vVelocity = texture(MotionTex, TexCoords);

    vec4 c = texture( ColorTex, TexCoords );

	if(length (vVelocity.xy) > 0)
    {        
        vec4 vPrevTickColor = texture(ColorTex, TexCoords + vVelocity.xy) * 0.4;        
        vPrevTickColor += texture(ColorTex, TexCoords + vVelocity.xy * 2) * 0.3;
        vPrevTickColor += texture(ColorTex, TexCoords + vVelocity.xy * 3) * 0.2;
        vPrevTickColor += texture(ColorTex, TexCoords + vVelocity.xy * 4) * 0.1;

        c = (c + vPrevTickColor) / 2.0f;
    }

	FragColor = c;	
}