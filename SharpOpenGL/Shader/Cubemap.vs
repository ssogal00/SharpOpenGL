#version 450 core

uniform mat4 ViewMatrix;
out vec3 TexCoord;

void main()
{
    vec3[4] vertices = vec3[4]( vec3(-1, -1, 1),
								vec3( 1, -1, 1),
								vec3(-1,  1, 1),
								vec3( 1,  1, 1) );
	
	TexCoord = mat3(ViewMatrix) * vertices[gl_VertexID];

	gl_Position = vec4(vertices[gl_VertexID], 1.0);
}
