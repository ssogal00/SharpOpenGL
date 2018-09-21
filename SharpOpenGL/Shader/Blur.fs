#version 450

in vec2 TexCoord;

uniform sampler2D ColorTex;
uniform vec2 BlurOffsets[9];
uniform vec2 BlurWeights[9];

layout( location = 0 ) out vec4 FragColor;

void main() 
{
	vec4 color = vec4(0,0,0,0);
    
    //for( int i = 0; i < 9; i++ )
    //{
     //   color += (texture(ColorTex, (TexCoord + BlurOffsets[i]))) * BlurWeights[i].x;        
//    }
	        
    FragColor = vec4(TexCoord, 0, 0);
}