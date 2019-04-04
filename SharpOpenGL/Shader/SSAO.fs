#version 430



layout (location = 0 ) in vec3 InPosition;
layout (location = 1 ) in vec2 InTexCoord;

layout (location = 0) out vec4 FragColor;

uniform mat4 ProjectionMatrix;

const int kernelSize = 64;

uniform vec3 SampleKernel[kernelSize];
uniform float Radius = 0.55;
uniform vec2 ScreenSize;

layout(binding=0) uniform sampler2D PositionTex;
layout(binding=1) uniform sampler2D NormalTex;
layout(binding=2) uniform sampler2D ColorTex;
layout(binding=4) uniform sampler2D RandTex;

void main() 
{
    vec2 randScale = vec2( ScreenSize.x / 4.0, ScreenSize.y / 4.0 );

    // Create the random tangent space matrix

    vec3 randDir = normalize( texture(RandTex, InTexCoord.xy * randScale).xyz );
    vec3 n = normalize( texture(NormalTex, InTexCoord).xyz );
    vec3 biTang = cross( n, randDir );

    
    if( length(biTang) < 0.0001 )  // If n and randDir are parallel, n is in x-y plane
    {
        biTang = cross( n, vec3(0,0,1));
    }

    biTang = normalize(biTang);
    vec3 tang = cross(biTang, n);
    mat3 toCamSpace = mat3(tang, biTang, n);

    float occlusionSum = 0.0;
    vec3 camPos = texture(PositionTex, InTexCoord).xyz;
    for( int i = 0; i < kernelSize; i++ ) 
    {
        vec3 samplePos = camPos + Radius * (toCamSpace * SampleKernel[i]);

        // Project point
        vec4 p = ProjectionMatrix * vec4(samplePos,1);
        p *= 1.0 / p.w;
        p.xyz = p.xyz * 0.5 + 0.5;

        // Access camera space z-coordinate at that point
        float surfaceZ = texture(PositionTex, p.xy).z;
        float zDist = surfaceZ - camPos.z;
        
        // Count points that ARE occluded
        if( zDist >= 0.0 && zDist <= Radius && surfaceZ > samplePos.z ) occlusionSum += 1.0;
    }

    float occ = occlusionSum / kernelSize;
    float AoData = 1.0 - occ;

    FragColor = vec4(AoData, AoData, AoData, 1.0f);
    
}
