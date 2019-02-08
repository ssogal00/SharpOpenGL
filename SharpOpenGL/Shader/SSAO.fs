#version 330

in vec2 TexCoord;

uniform sampler2D gDepthMap;
uniform sampler2D gNormalMap;
uniform sampler2D gPositionMap;
uniform sampler2D gNoiseMap;

uniform mat4 BiasMatrix;
uniform mat4 ProjectionMatrix;

uniform float fOcclusionRadius;
uniform int nSampleCount;

uniform int nKernelSize;
uniform vec4 vKernelOffsets[128];

uniform float fFarClipDistance;

in vec3 vFrustumRay;

layout( location = 0 ) out vec4 FragColor;

float OcclusionFunction(float distZ)
{
    float fOcclusion = 0.0f;

    if(distZ > 0.05f)
    {
        float fadeLength = 2.0f - 0.2f;

        fOcclusion = clamp(((2.0f - distZ) / fadeLength) , 0.0f, 1.0f);
    }

    return fOcclusion;
}

float ssao(in mat3 kernelBasis, in vec3 originPos, in float radius) 
{
	float occlusion = 0.0;
	for (int i = 0; i < nKernelSize; ++i) {
	//	get sample position:
		vec3 samplePos = kernelBasis * vKernelOffsets[i].xyz;
		samplePos = samplePos * radius + originPos;
		
	//	project sample position:
		vec4 offset = ProjectionMatrix * vec4(samplePos, 1.0);
		offset.xy /= offset.w; // only need xy
		offset.xy = offset.xy * 0.5 + 0.5; // scale/bias to texcoords
		
	//	get sample depth:
		float sampleDepth = -texture(gNormalMap, offset.xy).a * fFarClipDistance;
		
		float rangeCheck = smoothstep(0.0, 1.0, radius / abs(originPos.z - sampleDepth));
		occlusion += rangeCheck * step(-sampleDepth, -samplePos.z);
	}
	
	occlusion = 1.0 - (occlusion / float(nKernelSize));
    //occlusion = (occlusion / float(nKernelSize));
    
    //return occlusion;
	return pow(occlusion, 2.0f);
}

void main() 
{    
//	get noise texture coords:
	vec2 vNoiseTexCoord = TexCoord * 600 / 4;
    

//	get view space origin:
	float fOriginDepth = texture(gNormalMap, TexCoord).a;

    if(fOriginDepth == 0)
    {
        FragColor = vec4(1);
        return;
    }

	vec3 vOriginPos = vFrustumRay * fOriginDepth;

//	get view space normal:
	vec3 vNormal = texture(gNormalMap, TexCoord).xyz;    
		
//	construct kernel basis matrix:	
    vec3 vNoiseVector = texture(gNoiseMap, vNoiseTexCoord).xyz * 2.0 - 1.0f;

	vec3 tangent = normalize(vNoiseVector - vNormal * dot(vNoiseVector, vNormal));
	vec3 bitangent = cross(tangent, vNormal);
	mat3 kernelBasis = mat3(tangent, bitangent, vNormal);
	
	FragColor = vec4(ssao(kernelBasis, vOriginPos, fOcclusionRadius));

}
