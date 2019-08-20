
#version 450 core

layout (location = 0 ) in vec3 InPosition;
layout (location = 1 ) in vec3 InColor;

layout (location = 0) out vec4 PositionColor;
layout (location = 1) out vec4 DiffuseColor;
layout (location = 2) out vec4 NormalColor;
layout (location = 3) out vec4 VelocityColor;

void main() 
{      
    DiffuseColor = vec4(InColor, 1); 
}