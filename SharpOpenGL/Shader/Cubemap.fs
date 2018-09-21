

#version 450 core

layout (location=0) in vec3 TexCoord;

layout (location=0, binding=0) uniform samplerCube texCubemap;

layout (location=0) out vec4 Color;

void main()
{
    // Color = texture(texCubemap, TexCoord) * vec4(1,0,0,0);
    Color = vec4(1,0,0,0);
}