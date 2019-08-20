
#version 450

uniform ModelTransform
{
    mat4x4 Model;
};

uniform CameraTransform
{
    mat4x4 View;
    mat4x4 Proj;
};



layout (location = 0) in vec3 VertexPosition;
layout (location = 1) in vec3 VertexColor;


layout (location = 0 ) out vec3 OutPosition;
layout (location = 1 ) out vec3 OutColor;

void main()
{   
    gl_Position = Proj * View * Model * vec4(VertexPosition, 1);
    OutPosition = gl_Position.xyz;    
    OutColor = VertexColor;
}

