
#version 450

layout (location = 0 , binding = 0) uniform sampler2D PositionTex;
layout (location = 1 , binding = 1) uniform sampler2D DiffuseTex;
layout (location = 2 , binding = 2) uniform sampler2D NormalTex;
layout (location = 3 , binding = 3) uniform sampler2D BrdfLUT;
layout (location = 4 , binding = 4) uniform samplerCube IrradianceMap;
layout (location = 5 , binding = 5) uniform samplerCube PrefilterMap;

layout (location = 0 ) in vec3 InPosition;
layout (location = 1 ) in vec2 InTexCoord;
layout( location = 0 ) out vec4 FragColor;


const float PI = 3.1415926535;

// ----------------------------------------------------------------------------
float DistributionGGX(vec3 N, vec3 H, float roughness)
{
    float a = roughness*roughness;
    float a2 = a*a;
    float NdotH = max(dot(N, H), 0.0);
    float NdotH2 = NdotH*NdotH;

    float nom   = a2;
    float denom = (NdotH2 * (a2 - 1.0) + 1.0);
    denom = PI * denom * denom;

    return nom / denom;
}
// ----------------------------------------------------------------------------
float GeometrySchlickGGX(float NdotV, float roughness)
{
    float r = (roughness + 1.0);
    float k = (r*r) / 8.0;

    float nom   = NdotV;
    float denom = NdotV * (1.0 - k) + k;

    return nom / denom;
}
// ----------------------------------------------------------------------------
float GeometrySmith(vec3 N, vec3 V, vec3 L, float roughness)
{
    float NdotV = max(dot(N, V), 0.0);
    float NdotL = max(dot(N, L), 0.0);
    float ggx2 = GeometrySchlickGGX(NdotV, roughness);
    float ggx1 = GeometrySchlickGGX(NdotL, roughness);

    return ggx1 * ggx2;
}
// ----------------------------------------------------------------------------
vec3 fresnelSchlick(float cosTheta, vec3 F0)
{
    return F0 + (1.0 - F0) * pow(1.0 - cosTheta, 5.0);
}
// ----------------------------------------------------------------------------
vec3 fresnelSchlickRoughness(float cosTheta, vec3 F0, float roughness)
{
    return F0 + (max(vec3(1.0 - roughness), F0) - F0) * pow(1.0 - cosTheta, 5.0);
}   
// ----------------------------------------------------------------------------

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
    vec3 R = reflect(V, N); 	
	
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
        float lightRadius = lightMinMaxs[i].y;

        float attenuation = pow( clamp(1 - pow(distance / lightRadius, 4), 0, 1) , 2) / (distance * distance * 0.1 + 1);

        //float distanceFactor = ((50-1) / (lightMinMaxs[i].y - lightMinMaxs[i].x)) * (distance - lightMinMaxs[i].x) + 1;
        //float attenuation = 1.0 / (distanceFactor * distanceFactor);                
        
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
    vec3 F = fresnelSchlickRoughness(max(dot(N, V), 0.0), F0, roughness);
    
    vec3 kS = F;
    vec3 kD = 1.0 - kS;
    kD *= 1.0 - metallic;     
    
    vec3 irradiance = texture(IrradianceMap, N).rgb;
    vec3 diffuse      = irradiance * albedo;
    
    // sample both the pre-filter map and the BRDF lut and combine them together as per the Split-Sum approximation to get the IBL specular part.
    const float MAX_REFLECTION_LOD = 4.0;
    vec3 prefilteredColor = textureLod(PrefilterMap, R,  roughness * MAX_REFLECTION_LOD).rgb;    
    vec2 brdf  = texture(BrdfLUT, vec2(max(dot(N, V), 0.0), roughness)).rg;
    vec3 specular = prefilteredColor * (F * brdf.x + brdf.y);    

    vec3 ambient = (kD * diffuse + specular);
    
    vec3 color = ambient + Lo;

    // HDR tonemapping
    color = color / (color + vec3(1.0));
    // gamma correct
    color = pow(color, vec3(1.0/2.2)); 

    FragColor = vec4(color , 1.0);
}
