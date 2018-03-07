using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Core;
using Core.Buffer;
using Core.OpenGLShader;
using Core.Texture;
using Core.VertexCustomAttribute;
using Core.MaterialBase;
namespace SharpOpenGL.LightMaterial
{

public class LightMaterial : MaterialBase
{
	public LightMaterial() 
	 : base (GetVSSourceCode(), GetFSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return MaterialProgram;
	}

	public void Use()
	{
		MaterialProgram.UseProgram();
	}

	public void SetDiffuseTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"DiffuseTex", TextureObject);
	}

	public void SetDiffuseTex2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"DiffuseTex", TextureObject);
	}
	public void SetNormalTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"NormalTex", TextureObject);
	}

	public void SetNormalTex2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"NormalTex", TextureObject);
	}
	public void SetPositionTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"PositionTex", TextureObject);
	}

	public void SetPositionTex2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"PositionTex", TextureObject);
	}

	public static string GetVSSourceCode()
	{
		return @"#version 430

layout (location = 0) in vec3 VertexPosition;
layout (location = 1) in vec2 VertexTexCoord;


layout (location = 0 ) out vec3 OutPosition;
layout (location = 1 ) out vec2 OutTexCoord;

uniform Light
{
  vec3 LightDir;
  vec3 LightAmbient;
  vec3 LightDiffuse;
  vec3 LightSpecular;
  float LightSpecularShininess;  
};

void main()
{    
	gl_Position = vec4(VertexPosition.xy, 0.0, 1.0);

	OutPosition = vec3(VertexPosition.xy, 0.0);
	OutTexCoord = VertexTexCoord;
}
";
	}

	public static string GetFSSourceCode()
	{
		return @"#version 430

uniform sampler2D PositionTex;
uniform sampler2D DiffuseTex;
uniform sampler2D NormalTex;

layout (location = 0 ) in vec3 InPosition;
layout (location = 1 ) in vec2 InTexCoord;

layout( location = 0 ) out vec4 FragColor;

uniform Light
{
  vec3 LightDir;
  vec3 LightAmbient;
  vec3 LightDiffuse;
  vec3 LightSpecular;
  float LightSpecularShininess;  
};


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
	vec4 Normal = texture(NormalTex, TexCoord);
    vec3 Position = texture(PositionTex, TexCoord).xyz;
    
	float dotValue = max(dot(LightDir, Normal.xyz), 0.0);
	vec3 DiffuseColor = LightDiffuse * Color * dotValue;
	
	vec3 ViewDir = -normalize(Position);
	vec3 Half = normalize(LightDir + ViewDir);
	
	vec4 FinalColor;
    
    FinalColor = GetCookTorrance(Normal.xyz, LightDir, ViewDir, Half, LightAmbient, Color);
        
    FragColor = FinalColor;
}
";
	}
}
}
