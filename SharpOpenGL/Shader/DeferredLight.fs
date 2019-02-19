#version 450

layout (location = 0 , binding = 0) uniform sampler2D PositionTex;
layout (location = 1 , binding = 1) uniform sampler2D DiffuseTex;
layout (location = 2 , binding = 2) uniform sampler2D NormalTex;

layout (location = 0 ) in vec3 InPosition;
layout (location = 1 ) in vec2 InTexCoord;

layout( location = 0 ) out vec4 FragColor;

uniform float Roughness;
uniform vec3 LobeEnergy;

uniform Light
{
  vec3 LightDir;
  vec3 LightAmbient;
  vec3 LightDiffuse;
  vec3 LightSpecular;
  float LightSpecularShininess;  
};

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

vec3 Diffuse_Lambert( vec3 DiffuseColor )
{
	return DiffuseColor * ( 1 / PI);
}

// GGX / Trowbridge-Reitz
float D_GGX( float a2, float NoH )
{
	float d = ( NoH * a2 - NoH ) * NoH + 1;	// 2 mad
	return a2 / ( PI*d*d );					// 4 mul, 1 rcp
}

// Tuned to match behavior of Vis_Smith
float Vis_Schlick( float a2, float NoV, float NoL )
{
	float k = sqrt(a2) * 0.5;
	float Vis_SchlickV = NoV * (1 - k) + k;
	float Vis_SchlickL = NoL * (1 - k) + k;
	return 0.25 / ( Vis_SchlickV * Vis_SchlickL );
}

// Appoximation of joint Smith term for GGX
float Vis_SmithJointApprox( float a2, float NoV, float NoL )
{
	float a = sqrt(a2);
	float Vis_SmithV = NoL * ( NoV * ( 1 - a ) + a );
	float Vis_SmithL = NoV * ( NoL * ( 1 - a ) + a );
	//return 0.5 * rcp( Vis_SmithV + Vis_SmithL );
    return 0.5 / ( Vis_SmithV + Vis_SmithL );
}


// [Schlick 1994]
vec3 F_Schlick( vec3 SpecularColor, float VoH )
{
	float Fc = Pow5( 1 - VoH );					// 1 sub, 3 mul
	//return Fc + (1 - Fc) * SpecularColor;		// 1 add, 3 mad
	
	// Anything less than 2% is physically impossible and is instead considered to be shadowing
	// return saturate( 50.0 * SpecularColor.g ) * Fc + (1 - Fc) * SpecularColor;
    return clamp( 50.0 * SpecularColor.g ,0.0,1.0) * Fc + (1 - Fc) * SpecularColor;
	
}

vec3 F_Fresnel( vec3 SpecularColor, float VoH )
{
	vec3 SpecularColorSqrt = sqrt( clamp( vec3(0, 0, 0), vec3(0.99, 0.99, 0.99), SpecularColor ) );
	vec3 n = ( 1 + SpecularColorSqrt ) / ( 1 - SpecularColorSqrt );
	vec3 g = sqrt( n*n + VoH*VoH - 1 );
	return 0.5 * Square( (g - VoH) / (g + VoH) ) * ( 1 + Square( ((g+VoH)*VoH - 1) / ((g-VoH)*VoH + 1) ) );
}

vec3 StandardShading( vec3 DiffuseColor, vec3 SpecularColor, vec3 LobeRoughness, vec3 LobeEnergy, vec3 L, vec3 V, vec3 N )
{
	float NoL = dot(N, L);
	float NoV = dot(N, V);
	float LoV = dot(L, V);
	//float InvLenH = rsqrt( 2 + 2 * LoV );
    float InvLenH = 1 / sqrt( 2 + 2 * LoV );
	//float NoH = saturate( ( NoL + NoV ) * InvLenH );
    float NoH = clamp(( NoL + NoV ) * InvLenH, 0.0, 1.0);
	//float VoH = saturate( InvLenH + InvLenH * LoV );
    float VoH = clamp( InvLenH + InvLenH * LoV , 0.0, 1.0);
	//NoL = saturate(NoL);
    NoL = clamp(NoL,0.0,1.0);
	//NoV = saturate(abs(NoV) + 1e-5);
    NoV = clamp(abs(NoV) + 1e-5,0.0,1.0);

	// Generalized microfacet specular
	float D = D_GGX( LobeRoughness[1], NoH ) * LobeEnergy[1];
	float Vis = Vis_SmithJointApprox( LobeRoughness[1], NoV, NoL );
	vec3 F = F_Schlick( SpecularColor, VoH );

	vec3 Diffuse = Diffuse_Lambert( DiffuseColor );

	return Diffuse * LobeEnergy[2] + (D * Vis) * F;    
}


vec4 GetCookTorrance(vec3 vNormal, vec3 vLightDir, vec3 ViewDir, vec3 Half, vec3 Ambient, vec3 Diffuse)
{		
	vec3 N = normalize(vNormal);
	vec3 L = normalize(vLightDir);
	vec3 V = normalize(ViewDir);
	vec3 H = normalize(Half);
	
	float NH = clamp(dot(N,H),0.0,1.0);
	float VH = clamp(dot(V,H),0.0,1.0);
	float NV = clamp(dot(N,V),0.0,1.0);
	float NL = clamp(dot(L,N),0.0,1.0);
	
	const float m = 0.2f;
	
	float NH2 = NH*NH;
	float m2 = m*m;
	float D = (1/m2*NH2*NH2) * (exp(-((1-NH2) /(m2*NH2))));
		
	float G = min(1.0f, min((2*NH*NL) / VH, (2*NH*NV)/VH));
	
	float F = 0.01 + (1-0.01) * pow((1-NV),5.0f);
	
	const float PI = 3.1415926535;
	
	float S = (F * D * G) / (PI * NL * NV);	
	
	
	return vec4( (Ambient + (NL * clamp( 1.5f * ((0.7f * NL * 1.f) + (0.3f*S)) , 0.0, 1.0) )) * Diffuse.xyz, 1.0f);
}


void main() 
{
    // Calc Texture Coordinate
	vec2 TexCoord = InTexCoord;
        	
    // Fetch Geometry info from G-buffer
	vec3 Color = texture(DiffuseTex, TexCoord).xyz;
	vec4 Normal = normalize(texture(NormalTex, TexCoord));
    vec3 Position = texture(PositionTex, TexCoord).xyz;
    
	float dotValue = max(dot(LightDir, Normal.xyz), 0.0);
	vec3 DiffuseColor = LightDiffuse * Color * dotValue;
	
	vec3 ViewDir = -normalize(Position);
	vec3 Half = normalize(LightDir + ViewDir);

	vec4 FinalColor;
    FinalColor.xyz = StandardShading(Color, vec3(Normal.a), vec3(Roughness), LobeEnergy, LightDir, ViewDir, Normal.xyz);    
    FragColor = FinalColor;
}
