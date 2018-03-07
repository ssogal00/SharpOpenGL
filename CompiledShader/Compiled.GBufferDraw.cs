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
namespace SharpOpenGL.GBufferDraw
{

public class GBufferDraw : MaterialBase
{
	public GBufferDraw() 
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
	public void SetMaskTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"MaskTex", TextureObject);
	}

	public void SetMaskTex2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"MaskTex", TextureObject);
	}
	public void SetNormalTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"NormalTex", TextureObject);
	}

	public void SetNormalTex2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"NormalTex", TextureObject);
	}

	public static string GetVSSourceCode()
	{
		return @"#version 430 core


uniform Transform
{
	mat4x4 Model;
	mat4x4 View;
	mat4x4 Proj;
};

uniform mat4 NormalMatrix;

uniform vec3 Value;


layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec3 VertexNormal;
layout(location=2) in vec2 TexCoord;
layout(location=3) in vec4 Tangent;


layout(location=0) out vec3 OutPosition;
layout(location=1) out vec2 OutTexCoord;
layout(location=2) out vec3 OutNormal;
layout(location=3) out vec3 OutTangent;
layout(location=4) out vec3 OutBinormal;

  
void main()
{	
	mat4 ModelView = View * Model;

	OutTexCoord = TexCoord;
	gl_Position = Proj * View * Model * vec4(VertexPosition, 1);
	OutPosition =   (ModelView * vec4(VertexPosition, 1)).xyz;
	
	OutNormal =  normalize(mat3(NormalMatrix) * VertexNormal);	

	OutTangent = normalize(mat3(NormalMatrix) * vec3(Tangent));

	vec3 binormal = normalize(cross( VertexNormal, Tangent.xyz )) * Tangent.w ;
	OutBinormal = binormal;
}";
	}

	public static string GetFSSourceCode()
	{
		return @"
#version 430 core


layout(location=0) in vec3 InPosition;
layout(location=1) in vec2 InTexCoord;
layout(location=2) in vec3 InNormal;
layout(location=3) in vec3 InTangent;
layout(location=4) in vec3 InBinormal;


layout (location = 0) out vec4 PositionColor;
layout (location = 1) out vec4 DiffuseColor;
layout (location = 2) out vec4 NormalColor;

uniform sampler2D DiffuseTex;
uniform sampler2D NormalTex;
uniform sampler2D MaskTex;

uniform int MaskMapExist;

void main()
{   
    if(MaskMapExist > 0)
    {
    	vec4 MaskValue= texture(MaskTex, InTexCoord);
    	if(MaskValue.x > 0)
    	{
    		DiffuseColor = texture(DiffuseTex, InTexCoord);            
    	}
    	else
    	{
    		discard;
    	}
    }
    else
    {
    	DiffuseColor = texture(DiffuseTex, InTexCoord);
    }

    mat3 TangentToModelViewSpaceMatrix = mat3( InTangent.x, InTangent.y, InTangent.z, 
								   InBinormal.x, InBinormal.y, InBinormal.z, 
								   InNormal.x, InNormal.y, InNormal.z);
    

    vec4 NormalMapNormal = (2.0f * (texture( NormalTex, InTexCoord )) - 1.0f);			
	vec3 BumpNormal = normalize(TangentToModelViewSpaceMatrix * NormalMapNormal.xyz);			

    if(length(BumpNormal) > 0)
	{
		BumpNormal = normalize(BumpNormal);
	}
	else
	{
		BumpNormal = vec3(1,1,1);
	}

	// NormalColor = texture(NormalTex, InTexCoord);
    NormalColor.xyz = BumpNormal;
    PositionColor = vec4(InPosition, 0);
}";
	}
}
}
