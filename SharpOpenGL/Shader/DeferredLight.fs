
#version 450

layout (location = 0 , binding = 0) uniform sampler2D PositionTex;
layout (location = 1 , binding = 1) uniform sampler2D DiffuseTex;
layout (location = 2 , binding = 2) uniform sampler2D NormalTex;
layout (location = 3 , binding = 3) uniform samplerCube IrradianceMap;

layout (location = 0 ) in vec3 InPosition;
layout (location = 1 ) in vec2 InTexCoord;
layout( location = 0 ) out vec4 FragColor;


const float PI = 3.1415926535;


float Square( float x )
{
	return x*x;
}

vec2 Square( vec2 x )
{
	return x*x;
}

vec3 Square( vec3 x )
{
	return x*x;
}

vec4 Square( vec4 x )
{
	return x*x;
}

float Pow2( float x )
{
	return x*x;
}

vec2 Pow2( vec2 x )
{
	return x*x;
}

vec3 Pow2( vec3 x )
{
	return x*x;
}

vec4 Pow2( vec4 x )
{
	return x*x;
}

float Pow3( float x )
{
	return x*x*x;
}

vec2 Pow3( vec2 x )
{
	return x*x*x;
}

vec3 Pow3( vec3 x )
{
	return x*x*x;
}

vec4 Pow3( vec4 x )
{
	return x*x*x;
}

float Pow4( float x )
{
	float xx = x*x;
	return xx * xx;
}

vec2 Pow4( vec2 x )
{
	vec2 xx = x*x;
	return xx * xx;
}

vec3 Pow4( vec3 x )
{
	vec3 xx = x*x;
	return xx * xx;
}

vec4 Pow4( vec4 x )
{
	vec4 xx = x*x;
	return xx * xx;
}

float Pow5( float x )
{
	float xx = x*x;
	return xx * xx * x;
}

vec2 Pow5( vec2 x )
{
	vec2 xx = x*x;
	return xx * xx * x;
}

vec3 Pow5( vec3 x )
{
	vec3 xx = x*x;
	return xx * xx * x;
}

vec4 Pow5( vec4 x )
{
	vec4 xx = x*x;
	return xx * xx * x;
}

float DistributionGGX(vec3 N, vec3 H, float roughness)
{
    float a = roughness*roughness;
    float a2 = a*a;
    float NdotH = max(dot(N, H), 0.0);
    float NdotH2 = NdotH*NdotH;

    float nom   = a2;
    float denom = (NdotH2 * (a2 - 1.0) + 1.0);
    denom = PI * denom * denom;

    return nom / max(denom, 0.001); // prevent divide by zero for roughness=0.0 and NdotH=1.0
}

float GeometrySchlickGGX(float NdotV, float roughness)
{
    float r = (roughness + 1.0);
    float k = (r*r) / 8.0;

    float nom   = NdotV;
    float denom = NdotV * (1.0 - k) + k;

    return nom / denom;
}

float GeometrySmith(vec3 N, vec3 V, vec3 L, float roughness)
{
    float NdotV = max(dot(N, V), 0.0);
    float NdotL = max(dot(N, L), 0.0);
    float ggx2  = GeometrySchlickGGX(NdotV, roughness);
    float ggx1  = GeometrySchlickGGX(NdotL, roughness);
	
    return ggx1 * ggx2;
}

vec3 fresnelSchlick(float cosTheta, vec3 F0)
{
    return F0 + (1.0 - F0) * pow(1.0 - cosTheta, 5.0);    
}  

vec3 fresnelSchlickRoughness(float cosTheta, vec3 F0, float roughness)
{
    return F0 + (max(vec3(1.0 - roughness), F0) - F0) * pow(1.0 - cosTheta, 5.0);
}

vec3 Diffuse_Lambert( vec3 DiffuseColor )
{
	return DiffuseColor * ( 1 / PI);
}


uniform int lightCount;
uniform vec3 lightPositions[64];
uniform vec3 lightColors[64];
uniform vec2 lightMinMaxs[64];

uniform CameraTransform
{
	mat4x4 View;
	mat4x4 Proj;
};


void main() 
{
    // Calc Texture Coordinate
	vec2 TexCoord = InTexCoord;
        	
    // Fetch Geometry info from G-buffer
	vec3 Color = texture(DiffuseTex, TexCoord).xyz;
	vec4 Normal = normalize(texture(NormalTex, TexCoord));
    vec3 Position = texture(PositionTex, TexCoord).xyz;	

    vec3 albedo     = pow(texture(DiffuseTex, TexCoord).rgb, vec3(2.2));
    //vec3 albedo     = texture(DiffuseTex, TexCoord).rgb;
    float metallic  = clamp(texture(NormalTex, TexCoord).a , 0.0f, 1.0f);
    float roughness = clamp(texture(DiffuseTex, TexCoord).a, 0.0f, 1.0f);
    vec3 N = normalize(texture(NormalTex, TexCoord).xyz);
    vec3 V = -normalize(Position);

    vec3 F0 = vec3(0.04); 
    F0 = mix(F0, albedo, metallic);
	           
    // reflectance equation
    vec3 Lo = vec3(0.0);
    for(int i = 0; i < lightCount; ++i) 
    {
        // calculate per-light radiance        
        vec4 lightPosInViewSpace = View *  vec4(lightPositions[i], 1);
        vec3 L = normalize(lightPosInViewSpace.xyz - Position);        
        vec3 H = normalize(V + L);
        
        float distance = clamp(length(lightPosInViewSpace.xyz - Position), lightMinMaxs[i].x, lightMinMaxs[i].y);        
        float distanceFactor = ((50-1) / (lightMinMaxs[i].y - lightMinMaxs[i].x)) * (distance - lightMinMaxs[i].x) + 1;
        float attenuation = 1.0 / (distanceFactor * distanceFactor);                
        
        vec3 radiance     = lightColors[i] * attenuation;
        // cook-torrance brdf
        float NDF = DistributionGGX(N, H, roughness);        
        float G   = GeometrySmith(N, V, L, roughness);        
        vec3 F    = fresnelSchlick(clamp(dot(H, V), 0.0, 1.0), F0);              
        
        vec3 nominator    = NDF * G * F; 
        float denominator = 4 * max(dot(N, V), 0.0) * max(dot(N, L), 0.0);
        vec3 specular = nominator / max(denominator, 0.001); // prevent divide by zero for NdotV=0.0 or NdotL=0.0
        
        // kS is equal to Fresnel
        vec3 kS = F;
        // for energy conservation, the diffuse and specular light can't
        // be above 1.0 (unless the surface emits light); to preserve this
        // relationship the diffuse component (kD) should equal 1.0 - kS.
        vec3 kD = vec3(1.0) - kS;
        // multiply kD by the inverse metalness such that only non-metals 
        // have diffuse lighting, or a linear blend if partly metal (pure metals
        // have no diffuse light).
        kD *= 1.0 - metallic;	  

        // scale light by NdotL
        float NdotL = max(dot(N, L), 0.0);

        // add to outgoing radiance Lo
        Lo += (kD * albedo / PI + specular) * radiance * NdotL;  // note that we already multiplied the BRDF by the Fresnel (kS) so we won't multiply by kS again
    }   

    
    // ambient lighting (we now use IBL as the ambient term)
    vec3 kS = fresnelSchlick(max(dot(N, V), 0.0), F0);
    vec3 kD = 1.0 - kS;
    kD *= 1.0 - metallic;	  
    vec3 irradiance = texture(IrradianceMap, N).rgb;
    vec3 diffuse = irradiance * albedo;
    vec3 ambient = (kD * diffuse);    
    
    vec3 color = ambient + Lo;

	
    //color = color / (color + vec3(1.0));
    color = pow(color, vec3(1.0/2.2));  
   
    FragColor = vec4(color, 1.0);	
}
