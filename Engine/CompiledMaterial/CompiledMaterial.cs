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
using ZeroFormatter;
using ZeroFormatter.Formatters;
using Core.CustomAttribute;
namespace CompiledMaterial
{
namespace GBufferMacro1
{


public class GBufferMacro1 : MaterialBase
{
	public GBufferMacro1() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}

	public void SetDiffuseTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"DiffuseTex", TextureObject);
	}

	public void SetDiffuseTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"DiffuseTex", textureObject, samplerObject);		
	}

	public TextureBase DiffuseTex2D 
	{	
		set => SetTexture(@"DiffuseTex", value);		
	}

	public TextureBase DiffuseTex2D_PointSample
	{	
		set => SetTexture(@"DiffuseTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase DiffuseTex2D_LinearSample
	{	
		set => SetTexture(@"DiffuseTex", value, Sampler.DefaultLinearSampler);
	}	
	public void SetMaskTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"MaskTex", TextureObject);
	}

	public void SetMaskTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"MaskTex", textureObject, samplerObject);		
	}

	public TextureBase MaskTex2D 
	{	
		set => SetTexture(@"MaskTex", value);		
	}

	public TextureBase MaskTex2D_PointSample
	{	
		set => SetTexture(@"MaskTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase MaskTex2D_LinearSample
	{	
		set => SetTexture(@"MaskTex", value, Sampler.DefaultLinearSampler);
	}	
	public void SetMetallicRoughnessTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"MetallicRoughnessTex", TextureObject);
	}

	public void SetMetallicRoughnessTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"MetallicRoughnessTex", textureObject, samplerObject);		
	}

	public TextureBase MetallicRoughnessTex2D 
	{	
		set => SetTexture(@"MetallicRoughnessTex", value);		
	}

	public TextureBase MetallicRoughnessTex2D_PointSample
	{	
		set => SetTexture(@"MetallicRoughnessTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase MetallicRoughnessTex2D_LinearSample
	{	
		set => SetTexture(@"MetallicRoughnessTex", value, Sampler.DefaultLinearSampler);
	}	
	public void SetMetallicTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"MetallicTex", TextureObject);
	}

	public void SetMetallicTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"MetallicTex", textureObject, samplerObject);		
	}

	public TextureBase MetallicTex2D 
	{	
		set => SetTexture(@"MetallicTex", value);		
	}

	public TextureBase MetallicTex2D_PointSample
	{	
		set => SetTexture(@"MetallicTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase MetallicTex2D_LinearSample
	{	
		set => SetTexture(@"MetallicTex", value, Sampler.DefaultLinearSampler);
	}	
	public void SetNormalTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"NormalTex", TextureObject);
	}

	public void SetNormalTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"NormalTex", textureObject, samplerObject);		
	}

	public TextureBase NormalTex2D 
	{	
		set => SetTexture(@"NormalTex", value);		
	}

	public TextureBase NormalTex2D_PointSample
	{	
		set => SetTexture(@"NormalTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase NormalTex2D_LinearSample
	{	
		set => SetTexture(@"NormalTex", value, Sampler.DefaultLinearSampler);
	}	
	public void SetRoughnessTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"RoughnessTex", TextureObject);
	}

	public void SetRoughnessTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"RoughnessTex", textureObject, samplerObject);		
	}

	public TextureBase RoughnessTex2D 
	{	
		set => SetTexture(@"RoughnessTex", value);		
	}

	public TextureBase RoughnessTex2D_PointSample
	{	
		set => SetTexture(@"RoughnessTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase RoughnessTex2D_LinearSample
	{	
		set => SetTexture(@"RoughnessTex", value, Sampler.DefaultLinearSampler);
	}	



    private CameraTransform cameratransform = new CameraTransform();
	public CameraTransform CameraTransform
	{
		get { return cameratransform; }
		set 
		{ 
			cameratransform = value; 
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", cameratransform);
		}
	}
	public OpenTK.Mathematics.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", cameratransform);
		}
	}
    private MaterialProperty materialproperty = new MaterialProperty();
	public MaterialProperty MaterialProperty
	{
		get { return materialproperty; }
		set 
		{ 
			materialproperty = value; 
			this.SetUniformBufferValue< MaterialProperty >(@"MaterialProperty", value);
		}
	}

	public System.UInt32 MaterialProperty_EncodedPBRInfo
	{
		get { return materialproperty.encodedPBRInfo ; }
		set 
		{ 
			materialproperty.encodedPBRInfo = value;
			this.SetUniformBufferValue< MaterialProperty >(@"MaterialProperty", materialproperty);
		}
	}
	public System.Boolean MaterialProperty_MetallicExist
	{
		get { return materialproperty.MetallicExist ; }
		set 
		{ 
			materialproperty.MetallicExist = value;
			this.SetUniformBufferValue< MaterialProperty >(@"MaterialProperty", materialproperty);
		}
	}
	public System.Boolean MaterialProperty_RoghnessExist
	{
		get { return materialproperty.RoghnessExist ; }
		set 
		{ 
			materialproperty.RoghnessExist = value;
			this.SetUniformBufferValue< MaterialProperty >(@"MaterialProperty", materialproperty);
		}
	}
	public System.Boolean MaterialProperty_MaskExist
	{
		get { return materialproperty.MaskExist ; }
		set 
		{ 
			materialproperty.MaskExist = value;
			this.SetUniformBufferValue< MaterialProperty >(@"MaterialProperty", materialproperty);
		}
	}
	public System.Boolean MaterialProperty_NormalExist
	{
		get { return materialproperty.NormalExist ; }
		set 
		{ 
			materialproperty.NormalExist = value;
			this.SetUniformBufferValue< MaterialProperty >(@"MaterialProperty", materialproperty);
		}
	}
	public System.Single MaterialProperty_Metallic
	{
		get { return materialproperty.Metallic ; }
		set 
		{ 
			materialproperty.Metallic = value;
			this.SetUniformBufferValue< MaterialProperty >(@"MaterialProperty", materialproperty);
		}
	}
	public System.Single MaterialProperty_Roughness
	{
		get { return materialproperty.Roughness ; }
		set 
		{ 
			materialproperty.Roughness = value;
			this.SetUniformBufferValue< MaterialProperty >(@"MaterialProperty", materialproperty);
		}
	}
	public System.Boolean MaterialProperty_MetallicRoughnessOneTexture
	{
		get { return materialproperty.MetallicRoughnessOneTexture ; }
		set 
		{ 
			materialproperty.MetallicRoughnessOneTexture = value;
			this.SetUniformBufferValue< MaterialProperty >(@"MaterialProperty", materialproperty);
		}
	}
    private ModelTransform modeltransform = new ModelTransform();
	public ModelTransform ModelTransform
	{
		get { return modeltransform; }
		set 
		{ 
			modeltransform = value; 
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 ModelTransform_Model
	{
		get { return modeltransform.Model ; }
		set 
		{ 
			modeltransform.Model = value;
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", modeltransform);
		}
	}


	public static string GetVSSourceCode()
	{
		return @"#version 450 core
#define VERTEX_PNTT 1


uniform ModelTransform
{
	mat4x4 Model;
};

uniform CameraTransform
{
	mat4x4 View;
	mat4x4 Proj;
};

uniform mat4 NormalMatrix;

//
#if VERTEX_PNTT
#define TANGENT_EXIST 1
#define TEXCOORD_EXIST 1
layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec3 VertexNormal;
layout(location=2) in vec2 TexCoord;
layout(location=3) in vec4 Tangent;

layout(location=0) out vec4 OutPosition;
layout(location=1) out vec3 OutNormal;
layout(location=2) out vec2 OutTexCoord;
layout(location=3) out vec3 OutTangent;
layout(location=4) out vec3 OutBinormal;

#elif VERTEX_PNT
#define TEXCOORD_EXIST 1
layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec3 VertexNormal;
layout(location=2) in vec2 TexCoord;

layout(location=0) out vec4 OutPosition;
layout(location=1) out vec3 OutNormal;
layout(location=2) out vec2 OutTexCoord;

#elif VERTEX_PNC
#define VERTEXCOLOR_EXIST 1
#define TEXCOORD_EXIST 0
layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec3 VertexNormal;
layout(location=2) in vec3 VertexColor;

layout(location=0) out vec4 OutPosition;
layout(location=1) out vec3 OutNormal;
layout(location=2) out vec3 OutVertexColor;
#endif
  
void main()
{	
	//
	mat4 ModelView = View * Model;
	gl_Position = Proj * View * Model * vec4(VertexPosition, 1);

#if TEXCOORD_EXIST	
	OutTexCoord = TexCoord;		
#endif

#if VERTEXCOLOR_EXIST
	OutVertexColor = VertexColor;
#endif

	OutPosition =   (ModelView * vec4(VertexPosition, 1));
	OutNormal =  normalize(mat3(ModelView) * VertexNormal);	

#if TANGENT_EXIST
	OutTangent = normalize(mat3(ModelView) * vec3(Tangent));
	vec3 binormal = (cross( VertexNormal, Tangent.xyz )) * Tangent.w;
	OutBinormal = normalize(mat3(ModelView) * binormal);	
#endif
}";
	}

	public static string GetFSSourceCode()
	{
		return @"#version 450 core
#define VERTEX_PNTT 1

#if VERTEX_PNTT
layout(location=0) in vec4 InPosition;
layout(location=1) in vec3 InNormal;
layout(location=2) in vec2 InTexCoord;
layout(location=3) in vec3 InTangent;
layout(location=4) in vec3 InBinormal;
#if WIRE_FRAME
layout (location=5) noperspective in vec3 GEdgeDistance;
#endif

#elif VERTEX_PNT

layout(location=0) in vec4 InPosition;
layout(location=1) in vec3 InNormal;
layout(location=2) in vec2 InTexCoord;

#elif VERTEX_PNC

layout(location=0) in vec4 InPosition;
layout(location=1) in vec3 InNormal;
layout(location=2) in vec3 InColor;

#endif

#if WIRE_FRAME
uniform LineInfo
{
	float Width;
	vec4 WireframeColor;
};
#endif
layout (location = 0, binding=0) uniform sampler2D DiffuseTex;
layout (location = 1, binding=1) uniform sampler2D NormalTex;
layout (location = 2, binding=2) uniform sampler2D MaskTex;
layout (location = 3, binding=3) uniform sampler2D MetallicTex;
layout (location = 4, binding=4) uniform sampler2D RoughnessTex;
layout(location = 5, binding = 5) uniform sampler2D MetallicRoughnessTex;

layout (location = 0) out vec4 PositionColor;
layout (location = 1) out vec4 DiffuseColor;
layout (location = 2) out vec4 NormalColor;
layout (location = 3) out vec4 VelocityColor;

uniform MaterialProperty
{
	// 00000000 00000000 00000000 00000000
	// 00000000
	// bit position 0 => metallicExist;
	// bit position 1 => roghnessExist;
	// bit position 2 => maskExist;
	// bit position 3 => normalExist;
	// bit position 4 => occlusionExist;


	uint encodedPBRInfo;
	bool MetallicExist;
	bool RoghnessExist;
	bool MaskExist;
	bool NormalExist;
	float Metallic;
	float Roughness;
	bool MetallicRoughnessOneTexture;
};

float GetMetallicValue(vec2 texcoord)
{
	if (MetallicExist)
	{
		if (MetallicRoughnessOneTexture)
		{
			return texture(MetallicRoughnessTex, texcoord).b;
		}
		else
		{
			return texture(MetallicTex, texcoord).b;
		}
	}
	else
	{
		return Metallic;
	}
}

float GetRoughnessValue(vec2 texcoord)
{
	if (MetallicExist)
	{
		if (MetallicRoughnessOneTexture)
		{
			return texture(MetallicRoughnessTex, texcoord).g;
		}
		else
		{
			return texture(RoughnessTex, texcoord).g;
		}
	}
	else
	{
		return Roughness;
	}
}

vec4 GetDiffuseColor(vec2 texcoord)
{
	vec4 diffuseColor = texture(DiffuseTex,texcoord);
#if WIRE_FRAME
	float d = min(GEdgeDistance.x, GEdgeDistance.y);
	d = min(d, GEdgeDistance.z);
	float mixVal = smoothstep(Width-1,	Width+1, d);
	return mix(WireframeColor, diffuseColor, mixVal);	
#else
	return diffuseColor;
#endif
}

void main()
{   
	if(MaskExist)
	{
		vec4 MaskValue= texture(MaskTex, InTexCoord);
		if(MaskValue.x > 0)
		{

			DiffuseColor = GetDiffuseColor(InTexCoord);
		}
		else
		{
			discard;
		}    
	}
	else
	{
		DiffuseColor = GetDiffuseColor(InTexCoord);
	}
	
	DiffuseColor.a = GetRoughnessValue(InTexCoord);    

#if VERTEX_PNTT
	if(NormalExist)
    {
    	mat3 TangentToModelViewSpaceMatrix = mat3( InTangent.x, InTangent.y, InTangent.z, 
								    InBinormal.x, InBinormal.y, InBinormal.z, 
								    InNormal.x, InNormal.y, InNormal.z);    
    
	    vec3 NormalMapNormal = (2.0f * (texture( NormalTex, InTexCoord ).xyz) - vec3(1.0f));
	    vec3 BumpNormal = normalize(TangentToModelViewSpaceMatrix * NormalMapNormal.xyz);

	    NormalColor.xyz = BumpNormal.xyz;
    }
    else
    {
    	NormalColor.xyz = InNormal.xyz;
	}
#else
	NormalColor.xyz = InNormal.xyz;
#endif
    	
	NormalColor.a = GetMetallicValue(InTexCoord);	

    PositionColor = InPosition;
}
";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace GBufferMacro2
{


public class GBufferMacro2 : MaterialBase
{
	public GBufferMacro2() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}

	public void SetDiffuseTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"DiffuseTex", TextureObject);
	}

	public void SetDiffuseTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"DiffuseTex", textureObject, samplerObject);		
	}

	public TextureBase DiffuseTex2D 
	{	
		set => SetTexture(@"DiffuseTex", value);		
	}

	public TextureBase DiffuseTex2D_PointSample
	{	
		set => SetTexture(@"DiffuseTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase DiffuseTex2D_LinearSample
	{	
		set => SetTexture(@"DiffuseTex", value, Sampler.DefaultLinearSampler);
	}	
	public void SetMaskTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"MaskTex", TextureObject);
	}

	public void SetMaskTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"MaskTex", textureObject, samplerObject);		
	}

	public TextureBase MaskTex2D 
	{	
		set => SetTexture(@"MaskTex", value);		
	}

	public TextureBase MaskTex2D_PointSample
	{	
		set => SetTexture(@"MaskTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase MaskTex2D_LinearSample
	{	
		set => SetTexture(@"MaskTex", value, Sampler.DefaultLinearSampler);
	}	
	public void SetMetallicRoughnessTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"MetallicRoughnessTex", TextureObject);
	}

	public void SetMetallicRoughnessTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"MetallicRoughnessTex", textureObject, samplerObject);		
	}

	public TextureBase MetallicRoughnessTex2D 
	{	
		set => SetTexture(@"MetallicRoughnessTex", value);		
	}

	public TextureBase MetallicRoughnessTex2D_PointSample
	{	
		set => SetTexture(@"MetallicRoughnessTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase MetallicRoughnessTex2D_LinearSample
	{	
		set => SetTexture(@"MetallicRoughnessTex", value, Sampler.DefaultLinearSampler);
	}	
	public void SetMetallicTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"MetallicTex", TextureObject);
	}

	public void SetMetallicTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"MetallicTex", textureObject, samplerObject);		
	}

	public TextureBase MetallicTex2D 
	{	
		set => SetTexture(@"MetallicTex", value);		
	}

	public TextureBase MetallicTex2D_PointSample
	{	
		set => SetTexture(@"MetallicTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase MetallicTex2D_LinearSample
	{	
		set => SetTexture(@"MetallicTex", value, Sampler.DefaultLinearSampler);
	}	
	public void SetRoughnessTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"RoughnessTex", TextureObject);
	}

	public void SetRoughnessTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"RoughnessTex", textureObject, samplerObject);		
	}

	public TextureBase RoughnessTex2D 
	{	
		set => SetTexture(@"RoughnessTex", value);		
	}

	public TextureBase RoughnessTex2D_PointSample
	{	
		set => SetTexture(@"RoughnessTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase RoughnessTex2D_LinearSample
	{	
		set => SetTexture(@"RoughnessTex", value, Sampler.DefaultLinearSampler);
	}	



    private CameraTransform cameratransform = new CameraTransform();
	public CameraTransform CameraTransform
	{
		get { return cameratransform; }
		set 
		{ 
			cameratransform = value; 
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", cameratransform);
		}
	}
	public OpenTK.Mathematics.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", cameratransform);
		}
	}
    private MaterialProperty materialproperty = new MaterialProperty();
	public MaterialProperty MaterialProperty
	{
		get { return materialproperty; }
		set 
		{ 
			materialproperty = value; 
			this.SetUniformBufferValue< MaterialProperty >(@"MaterialProperty", value);
		}
	}

	public System.UInt32 MaterialProperty_EncodedPBRInfo
	{
		get { return materialproperty.encodedPBRInfo ; }
		set 
		{ 
			materialproperty.encodedPBRInfo = value;
			this.SetUniformBufferValue< MaterialProperty >(@"MaterialProperty", materialproperty);
		}
	}
	public System.Boolean MaterialProperty_MetallicExist
	{
		get { return materialproperty.MetallicExist ; }
		set 
		{ 
			materialproperty.MetallicExist = value;
			this.SetUniformBufferValue< MaterialProperty >(@"MaterialProperty", materialproperty);
		}
	}
	public System.Boolean MaterialProperty_RoghnessExist
	{
		get { return materialproperty.RoghnessExist ; }
		set 
		{ 
			materialproperty.RoghnessExist = value;
			this.SetUniformBufferValue< MaterialProperty >(@"MaterialProperty", materialproperty);
		}
	}
	public System.Boolean MaterialProperty_MaskExist
	{
		get { return materialproperty.MaskExist ; }
		set 
		{ 
			materialproperty.MaskExist = value;
			this.SetUniformBufferValue< MaterialProperty >(@"MaterialProperty", materialproperty);
		}
	}
	public System.Boolean MaterialProperty_NormalExist
	{
		get { return materialproperty.NormalExist ; }
		set 
		{ 
			materialproperty.NormalExist = value;
			this.SetUniformBufferValue< MaterialProperty >(@"MaterialProperty", materialproperty);
		}
	}
	public System.Single MaterialProperty_Metallic
	{
		get { return materialproperty.Metallic ; }
		set 
		{ 
			materialproperty.Metallic = value;
			this.SetUniformBufferValue< MaterialProperty >(@"MaterialProperty", materialproperty);
		}
	}
	public System.Single MaterialProperty_Roughness
	{
		get { return materialproperty.Roughness ; }
		set 
		{ 
			materialproperty.Roughness = value;
			this.SetUniformBufferValue< MaterialProperty >(@"MaterialProperty", materialproperty);
		}
	}
	public System.Boolean MaterialProperty_MetallicRoughnessOneTexture
	{
		get { return materialproperty.MetallicRoughnessOneTexture ; }
		set 
		{ 
			materialproperty.MetallicRoughnessOneTexture = value;
			this.SetUniformBufferValue< MaterialProperty >(@"MaterialProperty", materialproperty);
		}
	}
    private ModelTransform modeltransform = new ModelTransform();
	public ModelTransform ModelTransform
	{
		get { return modeltransform; }
		set 
		{ 
			modeltransform = value; 
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 ModelTransform_Model
	{
		get { return modeltransform.Model ; }
		set 
		{ 
			modeltransform.Model = value;
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", modeltransform);
		}
	}


	public static string GetVSSourceCode()
	{
		return @"#version 450 core
#define VERTEX_PNT 1


uniform ModelTransform
{
	mat4x4 Model;
};

uniform CameraTransform
{
	mat4x4 View;
	mat4x4 Proj;
};

uniform mat4 NormalMatrix;

//
#if VERTEX_PNTT
#define TANGENT_EXIST 1
#define TEXCOORD_EXIST 1
layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec3 VertexNormal;
layout(location=2) in vec2 TexCoord;
layout(location=3) in vec4 Tangent;

layout(location=0) out vec4 OutPosition;
layout(location=1) out vec3 OutNormal;
layout(location=2) out vec2 OutTexCoord;
layout(location=3) out vec3 OutTangent;
layout(location=4) out vec3 OutBinormal;

#elif VERTEX_PNT
#define TEXCOORD_EXIST 1
layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec3 VertexNormal;
layout(location=2) in vec2 TexCoord;

layout(location=0) out vec4 OutPosition;
layout(location=1) out vec3 OutNormal;
layout(location=2) out vec2 OutTexCoord;

#elif VERTEX_PNC
#define VERTEXCOLOR_EXIST 1
#define TEXCOORD_EXIST 0
layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec3 VertexNormal;
layout(location=2) in vec3 VertexColor;

layout(location=0) out vec4 OutPosition;
layout(location=1) out vec3 OutNormal;
layout(location=2) out vec3 OutVertexColor;
#endif
  
void main()
{	
	//
	mat4 ModelView = View * Model;
	gl_Position = Proj * View * Model * vec4(VertexPosition, 1);

#if TEXCOORD_EXIST	
	OutTexCoord = TexCoord;		
#endif

#if VERTEXCOLOR_EXIST
	OutVertexColor = VertexColor;
#endif

	OutPosition =   (ModelView * vec4(VertexPosition, 1));
	OutNormal =  normalize(mat3(ModelView) * VertexNormal);	

#if TANGENT_EXIST
	OutTangent = normalize(mat3(ModelView) * vec3(Tangent));
	vec3 binormal = (cross( VertexNormal, Tangent.xyz )) * Tangent.w;
	OutBinormal = normalize(mat3(ModelView) * binormal);	
#endif
}";
	}

	public static string GetFSSourceCode()
	{
		return @"#version 450 core
#define VERTEX_PNT 1

#if VERTEX_PNTT
layout(location=0) in vec4 InPosition;
layout(location=1) in vec3 InNormal;
layout(location=2) in vec2 InTexCoord;
layout(location=3) in vec3 InTangent;
layout(location=4) in vec3 InBinormal;
#if WIRE_FRAME
layout (location=5) noperspective in vec3 GEdgeDistance;
#endif

#elif VERTEX_PNT

layout(location=0) in vec4 InPosition;
layout(location=1) in vec3 InNormal;
layout(location=2) in vec2 InTexCoord;

#elif VERTEX_PNC

layout(location=0) in vec4 InPosition;
layout(location=1) in vec3 InNormal;
layout(location=2) in vec3 InColor;

#endif

#if WIRE_FRAME
uniform LineInfo
{
	float Width;
	vec4 WireframeColor;
};
#endif
layout (location = 0, binding=0) uniform sampler2D DiffuseTex;
layout (location = 1, binding=1) uniform sampler2D NormalTex;
layout (location = 2, binding=2) uniform sampler2D MaskTex;
layout (location = 3, binding=3) uniform sampler2D MetallicTex;
layout (location = 4, binding=4) uniform sampler2D RoughnessTex;
layout(location = 5, binding = 5) uniform sampler2D MetallicRoughnessTex;

layout (location = 0) out vec4 PositionColor;
layout (location = 1) out vec4 DiffuseColor;
layout (location = 2) out vec4 NormalColor;
layout (location = 3) out vec4 VelocityColor;

uniform MaterialProperty
{
	// 00000000 00000000 00000000 00000000
	// 00000000
	// bit position 0 => metallicExist;
	// bit position 1 => roghnessExist;
	// bit position 2 => maskExist;
	// bit position 3 => normalExist;
	// bit position 4 => occlusionExist;


	uint encodedPBRInfo;
	bool MetallicExist;
	bool RoghnessExist;
	bool MaskExist;
	bool NormalExist;
	float Metallic;
	float Roughness;
	bool MetallicRoughnessOneTexture;
};

float GetMetallicValue(vec2 texcoord)
{
	if (MetallicExist)
	{
		if (MetallicRoughnessOneTexture)
		{
			return texture(MetallicRoughnessTex, texcoord).b;
		}
		else
		{
			return texture(MetallicTex, texcoord).b;
		}
	}
	else
	{
		return Metallic;
	}
}

float GetRoughnessValue(vec2 texcoord)
{
	if (MetallicExist)
	{
		if (MetallicRoughnessOneTexture)
		{
			return texture(MetallicRoughnessTex, texcoord).g;
		}
		else
		{
			return texture(RoughnessTex, texcoord).g;
		}
	}
	else
	{
		return Roughness;
	}
}

vec4 GetDiffuseColor(vec2 texcoord)
{
	vec4 diffuseColor = texture(DiffuseTex,texcoord);
#if WIRE_FRAME
	float d = min(GEdgeDistance.x, GEdgeDistance.y);
	d = min(d, GEdgeDistance.z);
	float mixVal = smoothstep(Width-1,	Width+1, d);
	return mix(WireframeColor, diffuseColor, mixVal);	
#else
	return diffuseColor;
#endif
}

void main()
{   
	if(MaskExist)
	{
		vec4 MaskValue= texture(MaskTex, InTexCoord);
		if(MaskValue.x > 0)
		{

			DiffuseColor = GetDiffuseColor(InTexCoord);
		}
		else
		{
			discard;
		}    
	}
	else
	{
		DiffuseColor = GetDiffuseColor(InTexCoord);
	}
	
	DiffuseColor.a = GetRoughnessValue(InTexCoord);    

#if VERTEX_PNTT
	if(NormalExist)
    {
    	mat3 TangentToModelViewSpaceMatrix = mat3( InTangent.x, InTangent.y, InTangent.z, 
								    InBinormal.x, InBinormal.y, InBinormal.z, 
								    InNormal.x, InNormal.y, InNormal.z);    
    
	    vec3 NormalMapNormal = (2.0f * (texture( NormalTex, InTexCoord ).xyz) - vec3(1.0f));
	    vec3 BumpNormal = normalize(TangentToModelViewSpaceMatrix * NormalMapNormal.xyz);

	    NormalColor.xyz = BumpNormal.xyz;
    }
    else
    {
    	NormalColor.xyz = InNormal.xyz;
	}
#else
	NormalColor.xyz = InNormal.xyz;
#endif
    	
	NormalColor.a = GetMetallicValue(InTexCoord);	

    PositionColor = InPosition;
}
";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace ScreenSpaceDraw
{


public class ScreenSpaceDraw : MaterialBase
{
	public ScreenSpaceDraw() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}

	public void SetColorTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"ColorTex", TextureObject);
	}

	public void SetColorTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"ColorTex", textureObject, samplerObject);		
	}

	public TextureBase ColorTex2D 
	{	
		set => SetTexture(@"ColorTex", value);		
	}

	public TextureBase ColorTex2D_PointSample
	{	
		set => SetTexture(@"ColorTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase ColorTex2D_LinearSample
	{	
		set => SetTexture(@"ColorTex", value, Sampler.DefaultLinearSampler);
	}	





	public static string GetVSSourceCode()
	{
		return @"#version 450 core


layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec2 TexCoord;

out vec2 OutTexCoord;
  
void main()
{	
	OutTexCoord = TexCoord;	    
	gl_Position = vec4(VertexPosition.xy, 0, 1.0);
}";
	}

	public static string GetFSSourceCode()
	{
		return @"
#version 450 core

in vec2 OutTexCoord;

layout (binding = 0) uniform sampler2D ColorTex;

out vec4 FragColor;

void main() 
{      

    FragColor = texture(ColorTex, OutTexCoord);    
}";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace EquirectangleToCube
{


public class EquirectangleToCube : MaterialBase
{
	public EquirectangleToCube() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}

	public void SetEquirectangularMap2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"EquirectangularMap", TextureObject);
	}

	public void SetEquirectangularMap2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"EquirectangularMap", textureObject, samplerObject);		
	}

	public TextureBase EquirectangularMap2D 
	{	
		set => SetTexture(@"EquirectangularMap", value);		
	}

	public TextureBase EquirectangularMap2D_PointSample
	{	
		set => SetTexture(@"EquirectangularMap", value, Sampler.DefaultPointSampler);
	}

	public TextureBase EquirectangularMap2D_LinearSample
	{	
		set => SetTexture(@"EquirectangularMap", value, Sampler.DefaultLinearSampler);
	}	

	public OpenTK.Mathematics.Matrix4 Projection
	{
		get { return projection; }
		set 
		{
			projection = value;
			SetUniformVariable(@"Projection", projection);			
		}
	}
	private OpenTK.Mathematics.Matrix4 projection;
	public OpenTK.Mathematics.Matrix4 View
	{
		get { return view; }
		set 
		{
			view = value;
			SetUniformVariable(@"View", view);			
		}
	}
	private OpenTK.Mathematics.Matrix4 view;




	public static string GetVSSourceCode()
	{
		return @"#version 450

layout (location = 0) in vec3 Position;

layout (location = 0) out vec3 WorldPos;

uniform mat4 Projection;
uniform mat4 View;

void main()
{
    WorldPos = Position;
    gl_Position =  Projection * View * vec4(WorldPos, 1.0);
}";
	}

	public static string GetFSSourceCode()
	{
		return @"#version 450

layout (location = 0) out vec4 FragColor;

layout (location=0) in vec3 WorldPos;

layout (location = 0, binding=0) uniform sampler2D EquirectangularMap;

const vec2 invAtan = vec2(0.1591, 0.3183);

vec2 SampleSphericalMap(vec3 v)
{
    vec2 uv = vec2(atan(v.z, v.x), asin(v.y));
    uv *= invAtan;
    uv += 0.5;
    return uv;
}

void main()
{       
    vec2 uv = SampleSphericalMap(normalize(WorldPos));
    vec3 color = texture(EquirectangularMap, uv).rgb;    
    FragColor = vec4(color, 1.0);
}";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace GBufferDump
{


public class GBufferDump : MaterialBase
{
	public GBufferDump() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}

	public void SetDiffuseTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"DiffuseTex", TextureObject);
	}

	public void SetDiffuseTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"DiffuseTex", textureObject, samplerObject);		
	}

	public TextureBase DiffuseTex2D 
	{	
		set => SetTexture(@"DiffuseTex", value);		
	}

	public TextureBase DiffuseTex2D_PointSample
	{	
		set => SetTexture(@"DiffuseTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase DiffuseTex2D_LinearSample
	{	
		set => SetTexture(@"DiffuseTex", value, Sampler.DefaultLinearSampler);
	}	
	public void SetMotionBlurTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"MotionBlurTex", TextureObject);
	}

	public void SetMotionBlurTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"MotionBlurTex", textureObject, samplerObject);		
	}

	public TextureBase MotionBlurTex2D 
	{	
		set => SetTexture(@"MotionBlurTex", value);		
	}

	public TextureBase MotionBlurTex2D_PointSample
	{	
		set => SetTexture(@"MotionBlurTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase MotionBlurTex2D_LinearSample
	{	
		set => SetTexture(@"MotionBlurTex", value, Sampler.DefaultLinearSampler);
	}	
	public void SetNormalTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"NormalTex", TextureObject);
	}

	public void SetNormalTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"NormalTex", textureObject, samplerObject);		
	}

	public TextureBase NormalTex2D 
	{	
		set => SetTexture(@"NormalTex", value);		
	}

	public TextureBase NormalTex2D_PointSample
	{	
		set => SetTexture(@"NormalTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase NormalTex2D_LinearSample
	{	
		set => SetTexture(@"NormalTex", value, Sampler.DefaultLinearSampler);
	}	
	public void SetPositionTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"PositionTex", TextureObject);
	}

	public void SetPositionTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"PositionTex", textureObject, samplerObject);		
	}

	public TextureBase PositionTex2D 
	{	
		set => SetTexture(@"PositionTex", value);		
	}

	public TextureBase PositionTex2D_PointSample
	{	
		set => SetTexture(@"PositionTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase PositionTex2D_LinearSample
	{	
		set => SetTexture(@"PositionTex", value, Sampler.DefaultLinearSampler);
	}	



    private Dump dump = new Dump();
	public Dump Dump
	{
		get { return dump; }
		set 
		{ 
			dump = value; 
			this.SetUniformBufferValue< Dump >(@"Dump", value);
		}
	}

	public System.Boolean Dump_PositionDump
	{
		get { return dump.PositionDump ; }
		set 
		{ 
			dump.PositionDump = value;
			this.SetUniformBufferValue< Dump >(@"Dump", dump);
		}
	}
	public System.Boolean Dump_NormalDump
	{
		get { return dump.NormalDump ; }
		set 
		{ 
			dump.NormalDump = value;
			this.SetUniformBufferValue< Dump >(@"Dump", dump);
		}
	}
	public System.Boolean Dump_MetalicDump
	{
		get { return dump.MetalicDump ; }
		set 
		{ 
			dump.MetalicDump = value;
			this.SetUniformBufferValue< Dump >(@"Dump", dump);
		}
	}
	public System.Boolean Dump_DiffuseDump
	{
		get { return dump.DiffuseDump ; }
		set 
		{ 
			dump.DiffuseDump = value;
			this.SetUniformBufferValue< Dump >(@"Dump", dump);
		}
	}
	public System.Boolean Dump_RoughnessDump
	{
		get { return dump.RoughnessDump ; }
		set 
		{ 
			dump.RoughnessDump = value;
			this.SetUniformBufferValue< Dump >(@"Dump", dump);
		}
	}
	public System.Boolean Dump_MotionBlurDump
	{
		get { return dump.MotionBlurDump ; }
		set 
		{ 
			dump.MotionBlurDump = value;
			this.SetUniformBufferValue< Dump >(@"Dump", dump);
		}
	}


	public static string GetVSSourceCode()
	{
		return @"#version 450 core


layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec2 TexCoord;

layout(location=0) out vec2 OutTexCoord;
  
void main()
{	
	OutTexCoord = TexCoord;	    
	gl_Position = vec4(VertexPosition.xy, 0.0, 1.0);
}";
	}

	public static string GetFSSourceCode()
	{
		return @"#version 450

layout (location = 0 , binding = 0) uniform sampler2D PositionTex;
layout (location = 1 , binding = 1) uniform sampler2D DiffuseTex;
layout (location = 2 , binding = 2) uniform sampler2D NormalTex;
layout (location = 3 , binding = 3) uniform sampler2D MotionBlurTex;


layout (location = 0 ) in vec2 InTexCoord;

layout( location = 0 ) out vec4 FragColor;

uniform Dump
{
    bool PositionDump;
    bool NormalDump;
    bool MetalicDump;
    bool DiffuseDump;
    bool RoughnessDump;
    bool MotionBlurDump; 
};

void main() 
{
    if(PositionDump) 
    {   
        FragColor = texture(PositionTex, InTexCoord);
    }
    else if(NormalDump)
    {
        FragColor = texture(NormalTex, InTexCoord);
    }
    else if(DiffuseDump)
    {
        FragColor = texture(DiffuseTex, InTexCoord);
    }
    else if(MetalicDump)
    {
        FragColor = texture(NormalTex,InTexCoord).aaaa;
    }
    else if(RoughnessDump)
    {
   		FragColor = texture(DiffuseTex, InTexCoord).aaaa;
    }
    else if(MotionBlurDump)
    {
        FragColor = texture(MotionBlurTex, InTexCoord);
    }
    else
    {
        FragColor = texture(NormalTex,InTexCoord).aaaa;
    }
}
";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace DeferredLightMaterial
{


public class DeferredLightMaterial : MaterialBase
{
	public DeferredLightMaterial() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}

	public void SetBrdfLUT2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"BrdfLUT", TextureObject);
	}

	public void SetBrdfLUT2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"BrdfLUT", textureObject, samplerObject);		
	}

	public TextureBase BrdfLUT2D 
	{	
		set => SetTexture(@"BrdfLUT", value);		
	}

	public TextureBase BrdfLUT2D_PointSample
	{	
		set => SetTexture(@"BrdfLUT", value, Sampler.DefaultPointSampler);
	}

	public TextureBase BrdfLUT2D_LinearSample
	{	
		set => SetTexture(@"BrdfLUT", value, Sampler.DefaultLinearSampler);
	}	
	public void SetDiffuseTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"DiffuseTex", TextureObject);
	}

	public void SetDiffuseTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"DiffuseTex", textureObject, samplerObject);		
	}

	public TextureBase DiffuseTex2D 
	{	
		set => SetTexture(@"DiffuseTex", value);		
	}

	public TextureBase DiffuseTex2D_PointSample
	{	
		set => SetTexture(@"DiffuseTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase DiffuseTex2D_LinearSample
	{	
		set => SetTexture(@"DiffuseTex", value, Sampler.DefaultLinearSampler);
	}	
	public void SetIrradianceMap2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"IrradianceMap", TextureObject);
	}

	public void SetIrradianceMap2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"IrradianceMap", textureObject, samplerObject);		
	}

	public TextureBase IrradianceMap2D 
	{	
		set => SetTexture(@"IrradianceMap", value);		
	}

	public TextureBase IrradianceMap2D_PointSample
	{	
		set => SetTexture(@"IrradianceMap", value, Sampler.DefaultPointSampler);
	}

	public TextureBase IrradianceMap2D_LinearSample
	{	
		set => SetTexture(@"IrradianceMap", value, Sampler.DefaultLinearSampler);
	}	
	public void SetNormalTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"NormalTex", TextureObject);
	}

	public void SetNormalTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"NormalTex", textureObject, samplerObject);		
	}

	public TextureBase NormalTex2D 
	{	
		set => SetTexture(@"NormalTex", value);		
	}

	public TextureBase NormalTex2D_PointSample
	{	
		set => SetTexture(@"NormalTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase NormalTex2D_LinearSample
	{	
		set => SetTexture(@"NormalTex", value, Sampler.DefaultLinearSampler);
	}	
	public void SetPositionTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"PositionTex", TextureObject);
	}

	public void SetPositionTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"PositionTex", textureObject, samplerObject);		
	}

	public TextureBase PositionTex2D 
	{	
		set => SetTexture(@"PositionTex", value);		
	}

	public TextureBase PositionTex2D_PointSample
	{	
		set => SetTexture(@"PositionTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase PositionTex2D_LinearSample
	{	
		set => SetTexture(@"PositionTex", value, Sampler.DefaultLinearSampler);
	}	
	public void SetPrefilterMap2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"PrefilterMap", TextureObject);
	}

	public void SetPrefilterMap2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"PrefilterMap", textureObject, samplerObject);		
	}

	public TextureBase PrefilterMap2D 
	{	
		set => SetTexture(@"PrefilterMap", value);		
	}

	public TextureBase PrefilterMap2D_PointSample
	{	
		set => SetTexture(@"PrefilterMap", value, Sampler.DefaultPointSampler);
	}

	public TextureBase PrefilterMap2D_LinearSample
	{	
		set => SetTexture(@"PrefilterMap", value, Sampler.DefaultLinearSampler);
	}	



    private CameraTransform cameratransform = new CameraTransform();
	public CameraTransform CameraTransform
	{
		get { return cameratransform; }
		set 
		{ 
			cameratransform = value; 
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", cameratransform);
		}
	}
	public OpenTK.Mathematics.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", cameratransform);
		}
	}


	public static string GetVSSourceCode()
	{
		return @"#version 450

layout (location = 0) in vec3 VertexPosition;
layout (location = 1) in vec2 VertexTexCoord;


layout (location = 0 ) out vec3 OutPosition;
layout (location = 1 ) out vec2 OutTexCoord;

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
		return @"
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
	vec4 Vec4Position = texture(PositionTex, TexCoord);
    vec3 Position = texture(PositionTex, TexCoord).xyz;	

	if(Vec4Position.a == 0)
	{
		FragColor = vec4(Color , 1.0);
		return;
	}

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
";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace GeometryWireframeMaterial2
{


public class GeometryWireframeMaterial2 : MaterialBase
{
	public GeometryWireframeMaterial2() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}

	public void SetDiffuseTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"DiffuseTex", TextureObject);
	}

	public void SetDiffuseTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"DiffuseTex", textureObject, samplerObject);		
	}

	public TextureBase DiffuseTex2D 
	{	
		set => SetTexture(@"DiffuseTex", value);		
	}

	public TextureBase DiffuseTex2D_PointSample
	{	
		set => SetTexture(@"DiffuseTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase DiffuseTex2D_LinearSample
	{	
		set => SetTexture(@"DiffuseTex", value, Sampler.DefaultLinearSampler);
	}	
	public void SetMaskTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"MaskTex", TextureObject);
	}

	public void SetMaskTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"MaskTex", textureObject, samplerObject);		
	}

	public TextureBase MaskTex2D 
	{	
		set => SetTexture(@"MaskTex", value);		
	}

	public TextureBase MaskTex2D_PointSample
	{	
		set => SetTexture(@"MaskTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase MaskTex2D_LinearSample
	{	
		set => SetTexture(@"MaskTex", value, Sampler.DefaultLinearSampler);
	}	
	public void SetMetallicRoughnessTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"MetallicRoughnessTex", TextureObject);
	}

	public void SetMetallicRoughnessTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"MetallicRoughnessTex", textureObject, samplerObject);		
	}

	public TextureBase MetallicRoughnessTex2D 
	{	
		set => SetTexture(@"MetallicRoughnessTex", value);		
	}

	public TextureBase MetallicRoughnessTex2D_PointSample
	{	
		set => SetTexture(@"MetallicRoughnessTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase MetallicRoughnessTex2D_LinearSample
	{	
		set => SetTexture(@"MetallicRoughnessTex", value, Sampler.DefaultLinearSampler);
	}	
	public void SetMetallicTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"MetallicTex", TextureObject);
	}

	public void SetMetallicTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"MetallicTex", textureObject, samplerObject);		
	}

	public TextureBase MetallicTex2D 
	{	
		set => SetTexture(@"MetallicTex", value);		
	}

	public TextureBase MetallicTex2D_PointSample
	{	
		set => SetTexture(@"MetallicTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase MetallicTex2D_LinearSample
	{	
		set => SetTexture(@"MetallicTex", value, Sampler.DefaultLinearSampler);
	}	
	public void SetNormalTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"NormalTex", TextureObject);
	}

	public void SetNormalTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"NormalTex", textureObject, samplerObject);		
	}

	public TextureBase NormalTex2D 
	{	
		set => SetTexture(@"NormalTex", value);		
	}

	public TextureBase NormalTex2D_PointSample
	{	
		set => SetTexture(@"NormalTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase NormalTex2D_LinearSample
	{	
		set => SetTexture(@"NormalTex", value, Sampler.DefaultLinearSampler);
	}	
	public void SetRoughnessTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"RoughnessTex", TextureObject);
	}

	public void SetRoughnessTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"RoughnessTex", textureObject, samplerObject);		
	}

	public TextureBase RoughnessTex2D 
	{	
		set => SetTexture(@"RoughnessTex", value);		
	}

	public TextureBase RoughnessTex2D_PointSample
	{	
		set => SetTexture(@"RoughnessTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase RoughnessTex2D_LinearSample
	{	
		set => SetTexture(@"RoughnessTex", value, Sampler.DefaultLinearSampler);
	}	

	public OpenTK.Mathematics.Matrix4 ViewportMatrix
	{
		get { return viewportmatrix; }
		set 
		{
			viewportmatrix = value;
			SetUniformVariable(@"ViewportMatrix", viewportmatrix);			
		}
	}
	private OpenTK.Mathematics.Matrix4 viewportmatrix;


    private CameraTransform cameratransform = new CameraTransform();
	public CameraTransform CameraTransform
	{
		get { return cameratransform; }
		set 
		{ 
			cameratransform = value; 
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", cameratransform);
		}
	}
	public OpenTK.Mathematics.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", cameratransform);
		}
	}
    private LineInfo lineinfo = new LineInfo();
	public LineInfo LineInfo
	{
		get { return lineinfo; }
		set 
		{ 
			lineinfo = value; 
			this.SetUniformBufferValue< LineInfo >(@"LineInfo", value);
		}
	}

	public System.Single LineInfo_Width
	{
		get { return lineinfo.Width ; }
		set 
		{ 
			lineinfo.Width = value;
			this.SetUniformBufferValue< LineInfo >(@"LineInfo", lineinfo);
		}
	}
	public OpenTK.Mathematics.Vector4 LineInfo_WireframeColor
	{
		get { return lineinfo.WireframeColor ; }
		set 
		{ 
			lineinfo.WireframeColor = value;
			this.SetUniformBufferValue< LineInfo >(@"LineInfo", lineinfo);
		}
	}
    private MaterialProperty materialproperty = new MaterialProperty();
	public MaterialProperty MaterialProperty
	{
		get { return materialproperty; }
		set 
		{ 
			materialproperty = value; 
			this.SetUniformBufferValue< MaterialProperty >(@"MaterialProperty", value);
		}
	}

	public System.UInt32 MaterialProperty_EncodedPBRInfo
	{
		get { return materialproperty.encodedPBRInfo ; }
		set 
		{ 
			materialproperty.encodedPBRInfo = value;
			this.SetUniformBufferValue< MaterialProperty >(@"MaterialProperty", materialproperty);
		}
	}
	public System.Boolean MaterialProperty_MetallicExist
	{
		get { return materialproperty.MetallicExist ; }
		set 
		{ 
			materialproperty.MetallicExist = value;
			this.SetUniformBufferValue< MaterialProperty >(@"MaterialProperty", materialproperty);
		}
	}
	public System.Boolean MaterialProperty_RoghnessExist
	{
		get { return materialproperty.RoghnessExist ; }
		set 
		{ 
			materialproperty.RoghnessExist = value;
			this.SetUniformBufferValue< MaterialProperty >(@"MaterialProperty", materialproperty);
		}
	}
	public System.Boolean MaterialProperty_MaskExist
	{
		get { return materialproperty.MaskExist ; }
		set 
		{ 
			materialproperty.MaskExist = value;
			this.SetUniformBufferValue< MaterialProperty >(@"MaterialProperty", materialproperty);
		}
	}
	public System.Boolean MaterialProperty_NormalExist
	{
		get { return materialproperty.NormalExist ; }
		set 
		{ 
			materialproperty.NormalExist = value;
			this.SetUniformBufferValue< MaterialProperty >(@"MaterialProperty", materialproperty);
		}
	}
	public System.Single MaterialProperty_Metallic
	{
		get { return materialproperty.Metallic ; }
		set 
		{ 
			materialproperty.Metallic = value;
			this.SetUniformBufferValue< MaterialProperty >(@"MaterialProperty", materialproperty);
		}
	}
	public System.Single MaterialProperty_Roughness
	{
		get { return materialproperty.Roughness ; }
		set 
		{ 
			materialproperty.Roughness = value;
			this.SetUniformBufferValue< MaterialProperty >(@"MaterialProperty", materialproperty);
		}
	}
	public System.Boolean MaterialProperty_MetallicRoughnessOneTexture
	{
		get { return materialproperty.MetallicRoughnessOneTexture ; }
		set 
		{ 
			materialproperty.MetallicRoughnessOneTexture = value;
			this.SetUniformBufferValue< MaterialProperty >(@"MaterialProperty", materialproperty);
		}
	}
    private ModelTransform modeltransform = new ModelTransform();
	public ModelTransform ModelTransform
	{
		get { return modeltransform; }
		set 
		{ 
			modeltransform = value; 
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 ModelTransform_Model
	{
		get { return modeltransform.Model ; }
		set 
		{ 
			modeltransform.Model = value;
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", modeltransform);
		}
	}


	public static string GetVSSourceCode()
	{
		return @"#version 450 core
#define VERTEX_PNTT 1


uniform ModelTransform
{
	mat4x4 Model;
};

uniform CameraTransform
{
	mat4x4 View;
	mat4x4 Proj;
};

uniform mat4 NormalMatrix;

//
#if VERTEX_PNTT
#define TANGENT_EXIST 1
#define TEXCOORD_EXIST 1
layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec3 VertexNormal;
layout(location=2) in vec2 TexCoord;
layout(location=3) in vec4 Tangent;

layout(location=0) out vec4 OutPosition;
layout(location=1) out vec3 OutNormal;
layout(location=2) out vec2 OutTexCoord;
layout(location=3) out vec3 OutTangent;
layout(location=4) out vec3 OutBinormal;

#elif VERTEX_PNT
#define TEXCOORD_EXIST 1
layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec3 VertexNormal;
layout(location=2) in vec2 TexCoord;

layout(location=0) out vec4 OutPosition;
layout(location=1) out vec3 OutNormal;
layout(location=2) out vec2 OutTexCoord;

#elif VERTEX_PNC
#define VERTEXCOLOR_EXIST 1
#define TEXCOORD_EXIST 0
layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec3 VertexNormal;
layout(location=2) in vec3 VertexColor;

layout(location=0) out vec4 OutPosition;
layout(location=1) out vec3 OutNormal;
layout(location=2) out vec3 OutVertexColor;
#endif
  
void main()
{	
	//
	mat4 ModelView = View * Model;
	gl_Position = Proj * View * Model * vec4(VertexPosition, 1);

#if TEXCOORD_EXIST	
	OutTexCoord = TexCoord;		
#endif

#if VERTEXCOLOR_EXIST
	OutVertexColor = VertexColor;
#endif

	OutPosition =   (ModelView * vec4(VertexPosition, 1));
	OutNormal =  normalize(mat3(ModelView) * VertexNormal);	

#if TANGENT_EXIST
	OutTangent = normalize(mat3(ModelView) * vec3(Tangent));
	vec3 binormal = (cross( VertexNormal, Tangent.xyz )) * Tangent.w;
	OutBinormal = normalize(mat3(ModelView) * binormal);	
#endif
}";
	}

	public static string GetFSSourceCode()
	{
		return @"#version 450 core
#define VERTEX_PNTT 1
#define WIRE_FRAME 1

#if VERTEX_PNTT
layout(location=0) in vec4 InPosition;
layout(location=1) in vec3 InNormal;
layout(location=2) in vec2 InTexCoord;
layout(location=3) in vec3 InTangent;
layout(location=4) in vec3 InBinormal;
#if WIRE_FRAME
layout (location=5) noperspective in vec3 GEdgeDistance;
#endif

#elif VERTEX_PNT

layout(location=0) in vec4 InPosition;
layout(location=1) in vec3 InNormal;
layout(location=2) in vec2 InTexCoord;

#elif VERTEX_PNC

layout(location=0) in vec4 InPosition;
layout(location=1) in vec3 InNormal;
layout(location=2) in vec3 InColor;

#endif

#if WIRE_FRAME
uniform LineInfo
{
	float Width;
	vec4 WireframeColor;
};
#endif
layout (location = 0, binding=0) uniform sampler2D DiffuseTex;
layout (location = 1, binding=1) uniform sampler2D NormalTex;
layout (location = 2, binding=2) uniform sampler2D MaskTex;
layout (location = 3, binding=3) uniform sampler2D MetallicTex;
layout (location = 4, binding=4) uniform sampler2D RoughnessTex;
layout(location = 5, binding = 5) uniform sampler2D MetallicRoughnessTex;

layout (location = 0) out vec4 PositionColor;
layout (location = 1) out vec4 DiffuseColor;
layout (location = 2) out vec4 NormalColor;
layout (location = 3) out vec4 VelocityColor;

uniform MaterialProperty
{
	// 00000000 00000000 00000000 00000000
	// 00000000
	// bit position 0 => metallicExist;
	// bit position 1 => roghnessExist;
	// bit position 2 => maskExist;
	// bit position 3 => normalExist;
	// bit position 4 => occlusionExist;


	uint encodedPBRInfo;
	bool MetallicExist;
	bool RoghnessExist;
	bool MaskExist;
	bool NormalExist;
	float Metallic;
	float Roughness;
	bool MetallicRoughnessOneTexture;
};

float GetMetallicValue(vec2 texcoord)
{
	if (MetallicExist)
	{
		if (MetallicRoughnessOneTexture)
		{
			return texture(MetallicRoughnessTex, texcoord).b;
		}
		else
		{
			return texture(MetallicTex, texcoord).b;
		}
	}
	else
	{
		return Metallic;
	}
}

float GetRoughnessValue(vec2 texcoord)
{
	if (MetallicExist)
	{
		if (MetallicRoughnessOneTexture)
		{
			return texture(MetallicRoughnessTex, texcoord).g;
		}
		else
		{
			return texture(RoughnessTex, texcoord).g;
		}
	}
	else
	{
		return Roughness;
	}
}

vec4 GetDiffuseColor(vec2 texcoord)
{
	vec4 diffuseColor = texture(DiffuseTex,texcoord);
#if WIRE_FRAME
	float d = min(GEdgeDistance.x, GEdgeDistance.y);
	d = min(d, GEdgeDistance.z);
	float mixVal = smoothstep(Width-1,	Width+1, d);
	return mix(WireframeColor, diffuseColor, mixVal);	
#else
	return diffuseColor;
#endif
}

void main()
{   
	if(MaskExist)
	{
		vec4 MaskValue= texture(MaskTex, InTexCoord);
		if(MaskValue.x > 0)
		{

			DiffuseColor = GetDiffuseColor(InTexCoord);
		}
		else
		{
			discard;
		}    
	}
	else
	{
		DiffuseColor = GetDiffuseColor(InTexCoord);
	}
	
	DiffuseColor.a = GetRoughnessValue(InTexCoord);    

#if VERTEX_PNTT
	if(NormalExist)
    {
    	mat3 TangentToModelViewSpaceMatrix = mat3( InTangent.x, InTangent.y, InTangent.z, 
								    InBinormal.x, InBinormal.y, InBinormal.z, 
								    InNormal.x, InNormal.y, InNormal.z);    
    
	    vec3 NormalMapNormal = (2.0f * (texture( NormalTex, InTexCoord ).xyz) - vec3(1.0f));
	    vec3 BumpNormal = normalize(TangentToModelViewSpaceMatrix * NormalMapNormal.xyz);

	    NormalColor.xyz = BumpNormal.xyz;
    }
    else
    {
    	NormalColor.xyz = InNormal.xyz;
	}
#else
	NormalColor.xyz = InNormal.xyz;
#endif
    	
	NormalColor.a = GetMetallicValue(InTexCoord);	

    PositionColor = InPosition;
}
";
	}

	public static string GetGSSourceCode()
	{
		return @"
#version 460

layout (triangles) in;
layout (triangle_strip, max_vertices=3) out;

layout (location=0) out vec4 GPosition;
layout (location=1) out vec3 GNormal;
layout (location=2) out vec2 GTexCoord;
layout (location=3) out vec3 GTangent;
layout (location=4) out vec3 GBinormal;
layout (location=5) noperspective out vec3 GEdgeDistance;

layout (location=0) in vec4 VPosition[];
layout (location=1) in vec3 VNormal[];
layout (location=2) in vec2 VTexcoord[];
layout (location=3) in vec3 VTangent[];
layout (location=4) in vec3 VBinormal[];

//layout (location=0) out vec3 OutPosition;
//layout (location=1) out vec3 OutNormal;
//layout (location=2) out vec2 OutTexcoord;
//layout (location=3) out vec4 OutTangent;


uniform mat4 ViewportMatrix;

void main()
{
	// Transform each vertex into viewport space
	vec3 p0 = vec3(ViewportMatrix * (gl_in[0].gl_Position / gl_in[0].gl_Position.w));
	vec3 p1 = vec3(ViewportMatrix * (gl_in[1].gl_Position / gl_in[1].gl_Position.w));
	vec3 p2 = vec3(ViewportMatrix * (gl_in[2].gl_Position / gl_in[2].gl_Position.w));
	// Find the altitudes (ha, hb and hc)
	float a = length(p1 - p2);
	float b = length(p2 - p0);
	float c = length(p1 - p0);
	float alpha = acos( (b*b + c*c - a*a) / (2.0*b*c) );
	float beta = acos( (a*a + c*c - b*b) / (2.0*a*c) );
	float ha = abs( c * sin( beta ) );
	float hb = abs( c * sin( alpha ) );
	float hc = abs( b * sin( alpha ) );
	// Send the triangle along with the edge distances
	GEdgeDistance = vec3( ha, 0, 0 );

	GNormal = VNormal[0];
	GPosition = VPosition[0];
	GTexCoord = VTexcoord[0];
	GTangent = VTangent[0];
	GBinormal = VBinormal[0];
	gl_Position = gl_in[0].gl_Position;
	EmitVertex();

	GEdgeDistance = vec3( 0, hb, 0 );
	GNormal = VNormal[1];
	GPosition = VPosition[1];
	GTexCoord = VTexcoord[1];
	GTangent = VTangent[1];
	GBinormal = VBinormal[1];
	gl_Position = gl_in[1].gl_Position;
	EmitVertex();

	GEdgeDistance = vec3( 0, 0, hc );
	GNormal = VNormal[2];
	GPosition = VPosition[2];
	GTexCoord = VTexcoord[2];
	GTangent = VTangent[2];
	GBinormal = VBinormal[2];
	gl_Position = gl_in[2].gl_Position;
	EmitVertex();

	EndPrimitive();
}";
	}
}
}
namespace BasicMaterial
{


public class BasicMaterial : MaterialBase
{
	public BasicMaterial() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}




    private ColorBlock colorblock = new ColorBlock();
	public ColorBlock ColorBlock
	{
		get { return colorblock; }
		set 
		{ 
			colorblock = value; 
			this.SetUniformBufferValue< ColorBlock >(@"ColorBlock", value);
		}
	}

	public OpenTK.Mathematics.Vector3 ColorBlock_Value
	{
		get { return colorblock.Value ; }
		set 
		{ 
			colorblock.Value = value;
			this.SetUniformBufferValue< ColorBlock >(@"ColorBlock", colorblock);
		}
	}
    private Transform transform = new Transform();
	public Transform Transform
	{
		get { return transform; }
		set 
		{ 
			transform = value; 
			this.SetUniformBufferValue< Transform >(@"Transform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 Transform_Model
	{
		get { return transform.Model ; }
		set 
		{ 
			transform.Model = value;
			this.SetUniformBufferValue< Transform >(@"Transform", transform);
		}
	}
	public OpenTK.Mathematics.Matrix4 Transform_View
	{
		get { return transform.View ; }
		set 
		{ 
			transform.View = value;
			this.SetUniformBufferValue< Transform >(@"Transform", transform);
		}
	}
	public OpenTK.Mathematics.Matrix4 Transform_Proj
	{
		get { return transform.Proj ; }
		set 
		{ 
			transform.Proj = value;
			this.SetUniformBufferValue< Transform >(@"Transform", transform);
		}
	}


	public static string GetVSSourceCode()
	{
		return @"#version 450 core


uniform Transform
{
	mat4x4 Model;
	mat4x4 View;
	mat4x4 Proj;
};

uniform ColorBlock
{
	vec3 Value;
};

layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec2 TexCoord;

out vec3 Color;
out vec2 OutTexCoord;

void main()
{
	Color = vec3(1,0,0);
	OutTexCoord = TexCoord;
	gl_Position = ( Proj * View * Model) * vec4(VertexPosition, 1);
}";
	}

	public static string GetFSSourceCode()
	{
		return @"
#version 450 core

in vec3 Color;
in vec2 OutTexCoord;
out vec4 FragColor;



void main()
{   
    FragColor = vec4(1,0,0,1);
}";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace SimpleMaterial
{


public class SimpleMaterial : MaterialBase
{
	public SimpleMaterial() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}




    private Transform transform = new Transform();
	public Transform Transform
	{
		get { return transform; }
		set 
		{ 
			transform = value; 
			this.SetUniformBufferValue< Transform >(@"Transform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 Transform_Model
	{
		get { return transform.Model ; }
		set 
		{ 
			transform.Model = value;
			this.SetUniformBufferValue< Transform >(@"Transform", transform);
		}
	}
	public OpenTK.Mathematics.Matrix4 Transform_View
	{
		get { return transform.View ; }
		set 
		{ 
			transform.View = value;
			this.SetUniformBufferValue< Transform >(@"Transform", transform);
		}
	}
	public OpenTK.Mathematics.Matrix4 Transform_Proj
	{
		get { return transform.Proj ; }
		set 
		{ 
			transform.Proj = value;
			this.SetUniformBufferValue< Transform >(@"Transform", transform);
		}
	}


	public static string GetVSSourceCode()
	{
		return @"#version 450 core


uniform Transform
{
	mat4x4 Model;
	mat4x4 View;
	mat4x4 Proj;
};

layout(location=0) in vec3 VertexPosition;

void main()
{		
	gl_Position = ( Proj * View * Model) * vec4(VertexPosition, 1);
}";
	}

	public static string GetFSSourceCode()
	{
		return @"
#version 450 core

in vec3 Color;
in vec2 OutTexCoord;
out vec4 FragColor;

void main()
{   
    FragColor = vec4(1,0,0,0);
}";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace GBufferDraw
{


public class GBufferDraw : MaterialBase
{
	public GBufferDraw() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}

	public void SetDiffuseTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"DiffuseTex", TextureObject);
	}

	public void SetDiffuseTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"DiffuseTex", textureObject, samplerObject);		
	}

	public TextureBase DiffuseTex2D 
	{	
		set => SetTexture(@"DiffuseTex", value);		
	}

	public TextureBase DiffuseTex2D_PointSample
	{	
		set => SetTexture(@"DiffuseTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase DiffuseTex2D_LinearSample
	{	
		set => SetTexture(@"DiffuseTex", value, Sampler.DefaultLinearSampler);
	}	
	public void SetMaskTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"MaskTex", TextureObject);
	}

	public void SetMaskTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"MaskTex", textureObject, samplerObject);		
	}

	public TextureBase MaskTex2D 
	{	
		set => SetTexture(@"MaskTex", value);		
	}

	public TextureBase MaskTex2D_PointSample
	{	
		set => SetTexture(@"MaskTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase MaskTex2D_LinearSample
	{	
		set => SetTexture(@"MaskTex", value, Sampler.DefaultLinearSampler);
	}	
	public void SetMetallicTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"MetallicTex", TextureObject);
	}

	public void SetMetallicTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"MetallicTex", textureObject, samplerObject);		
	}

	public TextureBase MetallicTex2D 
	{	
		set => SetTexture(@"MetallicTex", value);		
	}

	public TextureBase MetallicTex2D_PointSample
	{	
		set => SetTexture(@"MetallicTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase MetallicTex2D_LinearSample
	{	
		set => SetTexture(@"MetallicTex", value, Sampler.DefaultLinearSampler);
	}	
	public void SetNormalTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"NormalTex", TextureObject);
	}

	public void SetNormalTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"NormalTex", textureObject, samplerObject);		
	}

	public TextureBase NormalTex2D 
	{	
		set => SetTexture(@"NormalTex", value);		
	}

	public TextureBase NormalTex2D_PointSample
	{	
		set => SetTexture(@"NormalTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase NormalTex2D_LinearSample
	{	
		set => SetTexture(@"NormalTex", value, Sampler.DefaultLinearSampler);
	}	
	public void SetRoughnessTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"RoughnessTex", TextureObject);
	}

	public void SetRoughnessTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"RoughnessTex", textureObject, samplerObject);		
	}

	public TextureBase RoughnessTex2D 
	{	
		set => SetTexture(@"RoughnessTex", value);		
	}

	public TextureBase RoughnessTex2D_PointSample
	{	
		set => SetTexture(@"RoughnessTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase RoughnessTex2D_LinearSample
	{	
		set => SetTexture(@"RoughnessTex", value, Sampler.DefaultLinearSampler);
	}	

	public System.Boolean DiffuseMapExist
	{
		get { return diffusemapexist; }
		set 
		{
			diffusemapexist = value;
			SetUniformVariable(@"DiffuseMapExist", diffusemapexist);			
		}
	}
	private System.Boolean diffusemapexist;
	public OpenTK.Mathematics.Vector3 DiffuseOverride
	{
		get { return diffuseoverride; }
		set 
		{
			diffuseoverride = value;
			SetUniformVariable(@"DiffuseOverride", diffuseoverride);			
		}
	}
	private OpenTK.Mathematics.Vector3 diffuseoverride;
	public System.Int32 LightChannel
	{
		get { return lightchannel; }
		set 
		{
			lightchannel = value;
			SetUniformVariable(@"LightChannel", lightchannel);			
		}
	}
	private System.Int32 lightchannel;
	public System.Boolean MaskMapExist
	{
		get { return maskmapexist; }
		set 
		{
			maskmapexist = value;
			SetUniformVariable(@"MaskMapExist", maskmapexist);			
		}
	}
	private System.Boolean maskmapexist;
	public System.Single Metalic
	{
		get { return metalic; }
		set 
		{
			metalic = value;
			SetUniformVariable(@"Metalic", metalic);			
		}
	}
	private System.Single metalic;
	public System.Boolean MetallicExist
	{
		get { return metallicexist; }
		set 
		{
			metallicexist = value;
			SetUniformVariable(@"MetallicExist", metallicexist);			
		}
	}
	private System.Boolean metallicexist;
	public System.Boolean NormalMapExist
	{
		get { return normalmapexist; }
		set 
		{
			normalmapexist = value;
			SetUniformVariable(@"NormalMapExist", normalmapexist);			
		}
	}
	private System.Boolean normalmapexist;
	public System.Single Roughness
	{
		get { return roughness; }
		set 
		{
			roughness = value;
			SetUniformVariable(@"Roughness", roughness);			
		}
	}
	private System.Single roughness;
	public System.Boolean RoughnessExist
	{
		get { return roughnessexist; }
		set 
		{
			roughnessexist = value;
			SetUniformVariable(@"RoughnessExist", roughnessexist);			
		}
	}
	private System.Boolean roughnessexist;


    private CameraTransform cameratransform = new CameraTransform();
	public CameraTransform CameraTransform
	{
		get { return cameratransform; }
		set 
		{ 
			cameratransform = value; 
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", cameratransform);
		}
	}
	public OpenTK.Mathematics.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", cameratransform);
		}
	}
    private ModelTransform modeltransform = new ModelTransform();
	public ModelTransform ModelTransform
	{
		get { return modeltransform; }
		set 
		{ 
			modeltransform = value; 
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 ModelTransform_Model
	{
		get { return modeltransform.Model ; }
		set 
		{ 
			modeltransform.Model = value;
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", modeltransform);
		}
	}
    private PrevTransform prevtransform = new PrevTransform();
	public PrevTransform PrevTransform
	{
		get { return prevtransform; }
		set 
		{ 
			prevtransform = value; 
			this.SetUniformBufferValue< PrevTransform >(@"PrevTransform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 PrevTransform_PrevProj
	{
		get { return prevtransform.PrevProj ; }
		set 
		{ 
			prevtransform.PrevProj = value;
			this.SetUniformBufferValue< PrevTransform >(@"PrevTransform", prevtransform);
		}
	}
	public OpenTK.Mathematics.Matrix4 PrevTransform_PrevModel
	{
		get { return prevtransform.PrevModel ; }
		set 
		{ 
			prevtransform.PrevModel = value;
			this.SetUniformBufferValue< PrevTransform >(@"PrevTransform", prevtransform);
		}
	}
	public OpenTK.Mathematics.Matrix4 PrevTransform_PrevView
	{
		get { return prevtransform.PrevView ; }
		set 
		{ 
			prevtransform.PrevView = value;
			this.SetUniformBufferValue< PrevTransform >(@"PrevTransform", prevtransform);
		}
	}


	public static string GetVSSourceCode()
	{
		return @"#version 450 core

uniform ModelTransform
{
	mat4x4 Model;
};

uniform CameraTransform
{
	mat4x4 View;
	mat4x4 Proj;
};

uniform PrevTransform
{
	mat4 PrevProj;
	mat4 PrevModel;
	mat4 PrevView;	
};

uniform mat4 NormalMatrix;

uniform vec3 Value;


layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec3 VertexNormal;
layout(location=2) in vec2 TexCoord;
layout(location=3) in vec4 Tangent;


layout(location=0) out vec4 OutPosition;
layout(location=1) out vec2 OutTexCoord;
layout(location=2) out vec3 OutNormal;
layout(location=3) out vec3 OutTangent;
layout(location=4) out vec3 OutBinormal;
layout(location=5) out vec3 NDCPos;
layout(location=6) out vec3 PrevNDCPos;

  
void main()
{	
	mat4 ModelView = View * Model;

	OutTexCoord = TexCoord;
	gl_Position = Proj * View * Model * vec4(VertexPosition, 1);
	OutPosition =   (ModelView * vec4(VertexPosition, 1));
	
	OutNormal =  normalize(mat3(ModelView) * VertexNormal);	

	OutTangent = normalize(mat3(ModelView) * vec3(Tangent));

	vec3 binormal = (cross( VertexNormal, Tangent.xyz )) * Tangent.w;
	OutBinormal = normalize(mat3(ModelView) * binormal);	

	NDCPos = gl_Position.xyz / gl_Position.w;
	vec4 PrevPos = PrevProj * PrevView * PrevModel * vec4(VertexPosition,1.0);
	PrevNDCPos = PrevPos.xyz / PrevPos.w;
}";
	}

	public static string GetFSSourceCode()
	{
		return @"
#version 450 core


layout(location=0) in vec4 InPosition;
layout(location=1) in vec2 InTexCoord;
layout(location=2) in vec3 InNormal;
layout(location=3) in vec3 InTangent;
layout(location=4) in vec3 InBinormal;
layout(location=5) in vec3 InNDCPos;
layout(location=6) in vec3 InPrevNDCPos;


layout (location = 0) out vec4 PositionColor;
layout (location = 1) out vec4 DiffuseColor;
layout (location = 2) out vec4 NormalColor;
layout (location = 3) out vec4 VelocityColor;

layout (location = 0, binding=0) uniform sampler2D DiffuseTex;
layout (location = 1, binding=1) uniform sampler2D NormalTex;
layout (location = 2, binding=2) uniform sampler2D MaskTex;
layout (location = 3, binding=3) uniform sampler2D MetallicTex;
layout (location = 4, binding=4) uniform sampler2D RoughnessTex;

uniform bool MetallicExist;
uniform bool MaskMapExist;
uniform bool NormalMapExist;
uniform bool RoughnessExist;
uniform bool DiffuseMapExist;
uniform int LightChannel = 0;

uniform float Metalic = 0;
uniform float Roughness = 0;
uniform vec3 DiffuseOverride;

void main()
{   
    if(MaskMapExist)
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
        if(DiffuseMapExist)
    	{
            DiffuseColor = texture(DiffuseTex, InTexCoord);            
        }
        else
        {
            DiffuseColor = vec4(DiffuseOverride,0);           
        }
    }

    if(RoughnessExist)
    {
        DiffuseColor.a = texture(RoughnessTex, InTexCoord).x;
    }
    else
    {
        DiffuseColor.a = Roughness;
    }

    if(InPosition.w == 0)
    {
        DiffuseColor = vec4(1,0,0,0);
    }

    mat3 TangentToModelViewSpaceMatrix = mat3( InTangent.x, InTangent.y, InTangent.z, 
								    InBinormal.x, InBinormal.y, InBinormal.z, 
								    InNormal.x, InNormal.y, InNormal.z);

    if(NormalMapExist)
    {
        vec3 NormalMapNormal = (2.0f * (texture( NormalTex, InTexCoord ).xyz) - vec3(1.0f));
	    vec3 BumpNormal = normalize(TangentToModelViewSpaceMatrix * NormalMapNormal.xyz);
	
        NormalColor.xyz = BumpNormal.xyz;
    }
    else
    {
        NormalColor.xyz = InNormal.xyz;
    }

    if(MetallicExist)
    {
        NormalColor.a = texture(MetallicTex, InTexCoord).x;        
    }
    else
    {
        NormalColor.a = Metalic;
    }

    PositionColor = InPosition;
	PositionColor.a = LightChannel;

    VelocityColor = vec4((InNDCPos - InPrevNDCPos) , 1.0f) / 2.0f;
}
";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace CubemapConvolution
{


public class CubemapConvolution : MaterialBase
{
	public CubemapConvolution() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}

	public void SetEnvironmentMap2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"EnvironmentMap", TextureObject);
	}

	public void SetEnvironmentMap2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"EnvironmentMap", textureObject, samplerObject);		
	}

	public TextureBase EnvironmentMap2D 
	{	
		set => SetTexture(@"EnvironmentMap", value);		
	}

	public TextureBase EnvironmentMap2D_PointSample
	{	
		set => SetTexture(@"EnvironmentMap", value, Sampler.DefaultPointSampler);
	}

	public TextureBase EnvironmentMap2D_LinearSample
	{	
		set => SetTexture(@"EnvironmentMap", value, Sampler.DefaultLinearSampler);
	}	

	public OpenTK.Mathematics.Matrix4 Projection
	{
		get { return projection; }
		set 
		{
			projection = value;
			SetUniformVariable(@"Projection", projection);			
		}
	}
	private OpenTK.Mathematics.Matrix4 projection;
	public OpenTK.Mathematics.Matrix4 View
	{
		get { return view; }
		set 
		{
			view = value;
			SetUniformVariable(@"View", view);			
		}
	}
	private OpenTK.Mathematics.Matrix4 view;




	public static string GetVSSourceCode()
	{
		return @"#version 450

layout (location = 0) in vec3 Position;

layout (location = 0) out vec3 WorldPos;

uniform mat4 Projection;
uniform mat4 View;

void main()
{
    WorldPos = Position;
    gl_Position =  Projection * View * vec4(WorldPos, 1.0);
}";
	}

	public static string GetFSSourceCode()
	{
		return @"
#version 450 core


layout (location = 0) out vec4 FragColor;

layout (location=0) in vec3 WorldPos;

layout (location = 0, binding=0) uniform samplerCube EnvironmentMap;

const float PI = 3.14159265359;

void main()
{		
	// The world vector acts as the normal of a tangent surface
    // from the origin, aligned to WorldPos. Given this normal, calculate all
    // incoming radiance of the environment. The result of this radiance
    // is the radiance of light coming from -Normal direction, which is what
    // we use in the PBR shader to sample irradiance.
    
    vec3 N = normalize(WorldPos);

    vec3 irradiance = vec3(0.0);   
    
    // tangent space calculation from origin point

    vec3 up    = vec3(0.0, 1.0, 0.0);
    vec3 right = cross(up, N);
    up            = cross(N, right);
       
    float sampleDelta = 0.025;
    float nrSamples = 0.0;
    
    for(float phi = 0.0; phi < 2.0 * PI; phi += sampleDelta)
    {
        for(float theta = 0.0; theta < 0.5 * PI; theta += sampleDelta)
        {
            // spherical to cartesian (in tangent space)            
            vec3 tangentSample = vec3(sin(theta) * cos(phi),  sin(theta) * sin(phi), cos(theta));
            // tangent space to world
            vec3 sampleVec = tangentSample.x * right + tangentSample.y * up + tangentSample.z * N; 

            irradiance += texture(EnvironmentMap, sampleVec).rgb * cos(theta) * sin(theta);
            nrSamples++;
        }
    }
    
    irradiance = PI * irradiance * (1.0 / float(nrSamples));
    
    FragColor = vec4(irradiance, 1.0);
}";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace GBufferInstanced
{


public class GBufferInstanced : MaterialBase
{
	public GBufferInstanced() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}

	public void SetDiffuseTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"DiffuseTex", TextureObject);
	}

	public void SetDiffuseTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"DiffuseTex", textureObject, samplerObject);		
	}

	public TextureBase DiffuseTex2D 
	{	
		set => SetTexture(@"DiffuseTex", value);		
	}

	public TextureBase DiffuseTex2D_PointSample
	{	
		set => SetTexture(@"DiffuseTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase DiffuseTex2D_LinearSample
	{	
		set => SetTexture(@"DiffuseTex", value, Sampler.DefaultLinearSampler);
	}	
	public void SetMetalicTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"MetalicTex", TextureObject);
	}

	public void SetMetalicTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"MetalicTex", textureObject, samplerObject);		
	}

	public TextureBase MetalicTex2D 
	{	
		set => SetTexture(@"MetalicTex", value);		
	}

	public TextureBase MetalicTex2D_PointSample
	{	
		set => SetTexture(@"MetalicTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase MetalicTex2D_LinearSample
	{	
		set => SetTexture(@"MetalicTex", value, Sampler.DefaultLinearSampler);
	}	
	public void SetNormalTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"NormalTex", TextureObject);
	}

	public void SetNormalTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"NormalTex", textureObject, samplerObject);		
	}

	public TextureBase NormalTex2D 
	{	
		set => SetTexture(@"NormalTex", value);		
	}

	public TextureBase NormalTex2D_PointSample
	{	
		set => SetTexture(@"NormalTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase NormalTex2D_LinearSample
	{	
		set => SetTexture(@"NormalTex", value, Sampler.DefaultLinearSampler);
	}	
	public void SetRoughnessTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"RoughnessTex", TextureObject);
	}

	public void SetRoughnessTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"RoughnessTex", textureObject, samplerObject);		
	}

	public TextureBase RoughnessTex2D 
	{	
		set => SetTexture(@"RoughnessTex", value);		
	}

	public TextureBase RoughnessTex2D_PointSample
	{	
		set => SetTexture(@"RoughnessTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase RoughnessTex2D_LinearSample
	{	
		set => SetTexture(@"RoughnessTex", value, Sampler.DefaultLinearSampler);
	}	

	public System.Boolean DiffuseMapExist
	{
		get { return diffusemapexist; }
		set 
		{
			diffusemapexist = value;
			SetUniformVariable(@"DiffuseMapExist", diffusemapexist);			
		}
	}
	private System.Boolean diffusemapexist;
	public OpenTK.Mathematics.Vector3 DiffuseOverride
	{
		get { return diffuseoverride; }
		set 
		{
			diffuseoverride = value;
			SetUniformVariable(@"DiffuseOverride", diffuseoverride);			
		}
	}
	private OpenTK.Mathematics.Vector3 diffuseoverride;
	public System.Int32 LightChannel
	{
		get { return lightchannel; }
		set 
		{
			lightchannel = value;
			SetUniformVariable(@"LightChannel", lightchannel);			
		}
	}
	private System.Int32 lightchannel;
	public System.Boolean MetalicExist
	{
		get { return metalicexist; }
		set 
		{
			metalicexist = value;
			SetUniformVariable(@"MetalicExist", metalicexist);			
		}
	}
	private System.Boolean metalicexist;
	public System.Int32 MetallicCount
	{
		get { return metalliccount; }
		set 
		{
			metalliccount = value;
			SetUniformVariable(@"MetallicCount", metalliccount);			
		}
	}
	private System.Int32 metalliccount;
	public System.Boolean NormalMapExist
	{
		get { return normalmapexist; }
		set 
		{
			normalmapexist = value;
			SetUniformVariable(@"NormalMapExist", normalmapexist);			
		}
	}
	private System.Boolean normalmapexist;
	public System.Int32 RoughnessCount
	{
		get { return roughnesscount; }
		set 
		{
			roughnesscount = value;
			SetUniformVariable(@"RoughnessCount", roughnesscount);			
		}
	}
	private System.Int32 roughnesscount;
	public System.Boolean RoughnessExist
	{
		get { return roughnessexist; }
		set 
		{
			roughnessexist = value;
			SetUniformVariable(@"RoughnessExist", roughnessexist);			
		}
	}
	private System.Boolean roughnessexist;


    private CameraTransform cameratransform = new CameraTransform();
	public CameraTransform CameraTransform
	{
		get { return cameratransform; }
		set 
		{ 
			cameratransform = value; 
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", cameratransform);
		}
	}
	public OpenTK.Mathematics.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", cameratransform);
		}
	}
    private ModelTransform modeltransform = new ModelTransform();
	public ModelTransform ModelTransform
	{
		get { return modeltransform; }
		set 
		{ 
			modeltransform = value; 
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 ModelTransform_Model
	{
		get { return modeltransform.Model ; }
		set 
		{ 
			modeltransform.Model = value;
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", modeltransform);
		}
	}


	public static string GetVSSourceCode()
	{
		return @"#version 450 core

uniform ModelTransform
{
	mat4x4 Model;
};

uniform CameraTransform
{
	mat4x4 View;
	mat4x4 Proj;
};

uniform mat4 NormalMatrix;


layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec3 VertexNormal;
layout(location=2) in vec2 TexCoord;
layout(location=3) in vec4 Tangent;



layout(location=0) out vec4 OutPosition;
layout(location=1) out vec2 OutTexCoord;
layout(location=2) out vec3 OutNormal;
layout(location=3) out vec3 OutTangent;
layout(location=4) out vec3 OutBinormal;
layout(location=5) out vec2 OutMetallicRoughness;

uniform int RoughnessCount;
uniform int MetallicCount;
  
void main()
{
	int metallicIndex = gl_InstanceID / MetallicCount;
	int roughnessIndex = gl_InstanceID % RoughnessCount;	

	float metallicValue = float(metallicIndex)  / float(MetallicCount);	
	float roughnessValue = float(roughnessIndex) / float(RoughnessCount);

	vec4 translation = vec4( 10, roughnessValue * 150 + 10, metallicValue * 150 - 75, 1);

	OutMetallicRoughness = vec2(metallicValue,  roughnessValue);

	mat4 ModelView = View * Model;

	OutTexCoord = TexCoord;
	gl_Position = Proj * ModelView *  (vec4(VertexPosition, 1) + translation);
	OutPosition =   (ModelView *  (vec4(VertexPosition, 1) + translation) );
	
	OutNormal =  normalize(mat3(ModelView) * VertexNormal);	

	OutTangent = normalize(mat3(ModelView) * vec3(Tangent));

	vec3 binormal = (cross( VertexNormal, Tangent.xyz )) * Tangent.w;
	OutBinormal = normalize(mat3(ModelView) * binormal);	
	
}";
	}

	public static string GetFSSourceCode()
	{
		return @"
#version 450 core


layout(location=0) in vec4 InPosition;
layout(location=1) in vec2 InTexCoord;
layout(location=2) in vec3 InNormal;
layout(location=3) in vec3 InTangent;
layout(location=4) in vec3 InBinormal;
layout(location=5) in vec2 MetallicRoughness;


layout (location = 0) out vec4 PositionColor;
layout (location = 1) out vec4 DiffuseColor;
layout (location = 2) out vec4 NormalColor;

layout (location = 0, binding=0) uniform sampler2D DiffuseTex;
layout (location = 1, binding=1) uniform sampler2D NormalTex;
layout (location = 2, binding=2) uniform sampler2D MaskTex;
layout (location = 3, binding=3) uniform sampler2D MetalicTex;
layout (location = 4, binding=4) uniform sampler2D RoughnessTex;

uniform bool MetalicExist;
uniform bool MaskMapExist;
uniform bool NormalMapExist;
uniform bool RoughnessExist;
uniform bool DiffuseMapExist;
uniform int LightChannel = 0;
uniform vec3 DiffuseOverride;

void main()
{   
    if(DiffuseMapExist)
	{
        DiffuseColor = texture(DiffuseTex, InTexCoord);            
    }
    else
    {
        DiffuseColor = vec4(DiffuseOverride,0);           
    }    

    if(RoughnessExist)
    {
        DiffuseColor.a = texture(RoughnessTex, InTexCoord).x;
    }
    else
    {
        DiffuseColor.a = MetallicRoughness.y;
    }

    if(InPosition.w == 0)
    {
        DiffuseColor = vec4(1,0,0,0);
    }

    mat3 TangentToModelViewSpaceMatrix = mat3( InTangent.x, InTangent.y, InTangent.z, 
								    InBinormal.x, InBinormal.y, InBinormal.z, 
								    InNormal.x, InNormal.y, InNormal.z);

    if(NormalMapExist)
    {
        vec3 NormalMapNormal = (2.0f * (texture( NormalTex, InTexCoord ).xyz) - vec3(1.0f));
	    vec3 BumpNormal = normalize(TangentToModelViewSpaceMatrix * NormalMapNormal.xyz);
	
        NormalColor.xyz = BumpNormal.xyz;
    }
    else
    {
        NormalColor.xyz = InNormal.xyz;
    }

    if(MetalicExist)
    {
        NormalColor.a = texture(MetalicTex, InTexCoord).x;        
    }
    else
    {
        NormalColor.a = MetallicRoughness.x;
    }

    PositionColor = InPosition;
	PositionColor.a = LightChannel;
}
";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace GBufferWithoutTexture
{


public class GBufferWithoutTexture : MaterialBase
{
	public GBufferWithoutTexture() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}




    private CameraTransform cameratransform = new CameraTransform();
	public CameraTransform CameraTransform
	{
		get { return cameratransform; }
		set 
		{ 
			cameratransform = value; 
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", cameratransform);
		}
	}
	public OpenTK.Mathematics.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", cameratransform);
		}
	}
    private ModelTransform modeltransform = new ModelTransform();
	public ModelTransform ModelTransform
	{
		get { return modeltransform; }
		set 
		{ 
			modeltransform = value; 
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 ModelTransform_Model
	{
		get { return modeltransform.Model ; }
		set 
		{ 
			modeltransform.Model = value;
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", modeltransform);
		}
	}
    private PrevTransform prevtransform = new PrevTransform();
	public PrevTransform PrevTransform
	{
		get { return prevtransform; }
		set 
		{ 
			prevtransform = value; 
			this.SetUniformBufferValue< PrevTransform >(@"PrevTransform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 PrevTransform_PrevProj
	{
		get { return prevtransform.PrevProj ; }
		set 
		{ 
			prevtransform.PrevProj = value;
			this.SetUniformBufferValue< PrevTransform >(@"PrevTransform", prevtransform);
		}
	}
	public OpenTK.Mathematics.Matrix4 PrevTransform_PrevModel
	{
		get { return prevtransform.PrevModel ; }
		set 
		{ 
			prevtransform.PrevModel = value;
			this.SetUniformBufferValue< PrevTransform >(@"PrevTransform", prevtransform);
		}
	}
	public OpenTK.Mathematics.Matrix4 PrevTransform_PrevView
	{
		get { return prevtransform.PrevView ; }
		set 
		{ 
			prevtransform.PrevView = value;
			this.SetUniformBufferValue< PrevTransform >(@"PrevTransform", prevtransform);
		}
	}


	public static string GetVSSourceCode()
	{
		return @"#version 450 core

uniform ModelTransform
{
	mat4x4 Model;
};

uniform CameraTransform
{
	mat4x4 View;
	mat4x4 Proj;
};

uniform PrevTransform
{
	mat4 PrevProj;
	mat4 PrevModel;
	mat4 PrevView;	
};

uniform mat4 NormalMatrix;

uniform vec3 Value;


layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec3 VertexNormal;
layout(location=2) in vec2 TexCoord;
layout(location=3) in vec4 Tangent;


layout(location=0) out vec4 OutPosition;
layout(location=1) out vec2 OutTexCoord;
layout(location=2) out vec3 OutNormal;
layout(location=3) out vec3 OutTangent;
layout(location=4) out vec3 OutBinormal;
layout(location=5) out vec3 NDCPos;
layout(location=6) out vec3 PrevNDCPos;

  
void main()
{	
	mat4 ModelView = View * Model;

	OutTexCoord = TexCoord;
	gl_Position = Proj * View * Model * vec4(VertexPosition, 1);
	OutPosition =   (ModelView * vec4(VertexPosition, 1));
	
	OutNormal =  normalize(mat3(ModelView) * VertexNormal);	

	OutTangent = normalize(mat3(ModelView) * vec3(Tangent));

	vec3 binormal = (cross( VertexNormal, Tangent.xyz )) * Tangent.w;
	OutBinormal = normalize(mat3(ModelView) * binormal);	

	NDCPos = gl_Position.xyz / gl_Position.w;
	vec4 PrevPos = PrevProj * PrevView * PrevModel * vec4(VertexPosition,1.0);
	PrevNDCPos = PrevPos.xyz / PrevPos.w;
}";
	}

	public static string GetFSSourceCode()
	{
		return @"
#version 450 core


layout(location=0) in vec4 InPosition;
layout(location=1) in vec2 InTexCoord;
layout(location=2) in vec3 InNormal;
layout(location=3) in vec3 InTangent;
layout(location=4) in vec3 InBinormal;


layout (location = 0) out vec4 PositionColor;
layout (location = 1) out vec4 DiffuseColor;
layout (location = 2) out vec4 NormalColor;

void main()
{   
    DiffuseColor = vec4(0.9,0.0,0.0,0);

    NormalColor.xyz = InNormal.xyz;
    
    NormalColor.a = 0;    

    PositionColor = InPosition;
}";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace GBufferPNC
{


public class GBufferPNC : MaterialBase
{
	public GBufferPNC() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}


	public OpenTK.Mathematics.Matrix4 Model
	{
		get { return model; }
		set 
		{
			model = value;
			SetUniformVariable(@"Model", model);			
		}
	}
	private OpenTK.Mathematics.Matrix4 model;


    private CameraTransform cameratransform = new CameraTransform();
	public CameraTransform CameraTransform
	{
		get { return cameratransform; }
		set 
		{ 
			cameratransform = value; 
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", cameratransform);
		}
	}
	public OpenTK.Mathematics.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", cameratransform);
		}
	}


	public static string GetVSSourceCode()
	{
		return @"#version 450 core



uniform	mat4x4 Model;


uniform CameraTransform
{
	mat4x4 View;
	mat4x4 Proj;
};

uniform mat4 NormalMatrix;


layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec3 VertexNormal;
layout(location=2) in vec3 VertexColor;



layout(location=0) out vec4 OutPosition;
layout(location=1) out vec3 OutColor;
layout(location=2) out vec3 OutNormal;


  
void main()
{	
	mat4 ModelView = View * Model;
	OutColor = VertexColor;
	gl_Position = Proj * View * Model * vec4(VertexPosition, 1);
	OutPosition =   (ModelView * vec4(VertexPosition, 1));	
	OutNormal =  normalize(mat3(ModelView) * VertexNormal);	
}";
	}

	public static string GetFSSourceCode()
	{
		return @"
#version 450 core

layout(location=0) in vec4 InPosition;
layout(location=1) in vec3 InVertexColor;
layout(location=2) in vec3 InNormal;



layout (location = 0) out vec4 PositionColor;
layout (location = 1) out vec4 DiffuseColor;
layout (location = 2) out vec4 NormalColor;


void main()
{   	
	DiffuseColor = vec4(InVertexColor, 1.0);    	
    NormalColor = vec4(InNormal.xyz,0);    
    PositionColor = InPosition;
}";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace GBufferCubeTest
{


public class GBufferCubeTest : MaterialBase
{
	public GBufferCubeTest() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}

	public void SetEquirectangularMap2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"EquirectangularMap", TextureObject);
	}

	public void SetEquirectangularMap2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"EquirectangularMap", textureObject, samplerObject);		
	}

	public TextureBase EquirectangularMap2D 
	{	
		set => SetTexture(@"EquirectangularMap", value);		
	}

	public TextureBase EquirectangularMap2D_PointSample
	{	
		set => SetTexture(@"EquirectangularMap", value, Sampler.DefaultPointSampler);
	}

	public TextureBase EquirectangularMap2D_LinearSample
	{	
		set => SetTexture(@"EquirectangularMap", value, Sampler.DefaultLinearSampler);
	}	



    private CameraTransform cameratransform = new CameraTransform();
	public CameraTransform CameraTransform
	{
		get { return cameratransform; }
		set 
		{ 
			cameratransform = value; 
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", cameratransform);
		}
	}
	public OpenTK.Mathematics.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", cameratransform);
		}
	}
    private ModelTransform modeltransform = new ModelTransform();
	public ModelTransform ModelTransform
	{
		get { return modeltransform; }
		set 
		{ 
			modeltransform = value; 
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 ModelTransform_Model
	{
		get { return modeltransform.Model ; }
		set 
		{ 
			modeltransform.Model = value;
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", modeltransform);
		}
	}


	public static string GetVSSourceCode()
	{
		return @"#version 450 core

uniform ModelTransform
{
	mat4x4 Model;
};

uniform CameraTransform
{
	mat4x4 View;
	mat4x4 Proj;
};


layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec3 VertexNormal;

layout(location=0) out vec4 OutPosition;
layout(location=1) out vec3 OutNormal;
layout(location=2) out vec3 LocalPosition;
  
void main()
{	
	mat4 ModelView = View * Model;

	LocalPosition = VertexPosition;

	gl_Position = Proj * View * Model * vec4(VertexPosition, 1);
	OutPosition =   (ModelView * vec4(VertexPosition, 1));
	
	OutNormal =  normalize(mat3(ModelView) * VertexNormal);		
}";
	}

	public static string GetFSSourceCode()
	{
		return @"
#version 450 core


layout(location=0) in vec4 InPosition;
layout(location=1) in vec3 InNormal;
layout(location=2) in vec3 InLocalPosition;


layout (location = 0) out vec4 PositionColor;
layout (location = 1) out vec4 DiffuseColor;
layout (location = 2) out vec4 NormalColor;

layout (location=0, binding=0) uniform sampler2D EquirectangularMap;

const vec2 invAtan = vec2(0.1591, -0.3183);
vec2 SampleSphericalMap(vec3 v)
{
    vec2 uv = vec2(atan(v.z, v.x), asin(v.y));
    uv *= invAtan;
    uv += 0.5;
    return uv;
}

void main()
{  
    vec2 uv = SampleSphericalMap(normalize(InLocalPosition));
    vec3 color = texture(EquirectangularMap, uv).rgb;   
    
    DiffuseColor = vec4(color, 1.0);
    NormalColor = vec4(InNormal, 1);
    PositionColor = InPosition;
}
";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace GBufferPNCT
{


public class GBufferPNCT : MaterialBase
{
	public GBufferPNCT() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}

	public void SetSpecularTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"SpecularTex", TextureObject);
	}

	public void SetSpecularTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"SpecularTex", textureObject, samplerObject);		
	}

	public TextureBase SpecularTex2D 
	{	
		set => SetTexture(@"SpecularTex", value);		
	}

	public TextureBase SpecularTex2D_PointSample
	{	
		set => SetTexture(@"SpecularTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase SpecularTex2D_LinearSample
	{	
		set => SetTexture(@"SpecularTex", value, Sampler.DefaultLinearSampler);
	}	

	public OpenTK.Mathematics.Matrix4 Model
	{
		get { return model; }
		set 
		{
			model = value;
			SetUniformVariable(@"Model", model);			
		}
	}
	private OpenTK.Mathematics.Matrix4 model;


    private CameraTransform cameratransform = new CameraTransform();
	public CameraTransform CameraTransform
	{
		get { return cameratransform; }
		set 
		{ 
			cameratransform = value; 
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", cameratransform);
		}
	}
	public OpenTK.Mathematics.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", cameratransform);
		}
	}


	public static string GetVSSourceCode()
	{
		return @"#version 450 core



uniform	mat4x4 Model;


uniform CameraTransform
{
	mat4x4 View;
	mat4x4 Proj;
};

uniform mat4 NormalMatrix;


layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec3 VertexNormal;
layout(location=2) in vec3 VertexColor;
layout(location=3) in vec2 TexCoord;

layout(location=0) out vec4 OutPosition;
layout(location=1) out vec3 OutColor;
layout(location=2) out vec3 OutNormal;
layout(location=3) out vec2 OutTexCoord;


  
void main()
{	
	mat4 ModelView = View * Model;
	OutColor = VertexColor;
	gl_Position = Proj * View * Model * vec4(VertexPosition, 1);
	OutPosition =   (ModelView * vec4(VertexPosition, 1));	
	OutNormal =  normalize(mat3(ModelView) * VertexNormal);
	OutTexCoord = TexCoord;
}";
	}

	public static string GetFSSourceCode()
	{
		return @"
#version 450 core


layout(location=0) in vec4 InPosition;
layout(location=1) in vec3 InVertexColor;
layout(location=2) in vec3 InNormal;
layout(location=3) in vec2 InTexCoord;


layout (location = 0) out vec4 PositionColor;
layout (location = 1) out vec4 DiffuseColor;
layout (location = 2) out vec4 NormalColor;

layout (location = 0, binding=0) uniform sampler2D SpecularTex;

void main()
{   	
	DiffuseColor = vec4(InVertexColor, 1.0);    	
    NormalColor = vec4(InNormal.xyz,0);
    NormalColor.a = texture(SpecularTex, InTexCoord).x;
    PositionColor = InPosition;
}";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace GBufferPNT
{


public class GBufferPNT : MaterialBase
{
	public GBufferPNT() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}

	public void SetDiffuseTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"DiffuseTex", TextureObject);
	}

	public void SetDiffuseTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"DiffuseTex", textureObject, samplerObject);		
	}

	public TextureBase DiffuseTex2D 
	{	
		set => SetTexture(@"DiffuseTex", value);		
	}

	public TextureBase DiffuseTex2D_PointSample
	{	
		set => SetTexture(@"DiffuseTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase DiffuseTex2D_LinearSample
	{	
		set => SetTexture(@"DiffuseTex", value, Sampler.DefaultLinearSampler);
	}	



    private CameraTransform cameratransform = new CameraTransform();
	public CameraTransform CameraTransform
	{
		get { return cameratransform; }
		set 
		{ 
			cameratransform = value; 
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", cameratransform);
		}
	}
	public OpenTK.Mathematics.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", cameratransform);
		}
	}
    private ModelTransform modeltransform = new ModelTransform();
	public ModelTransform ModelTransform
	{
		get { return modeltransform; }
		set 
		{ 
			modeltransform = value; 
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 ModelTransform_Model
	{
		get { return modeltransform.Model ; }
		set 
		{ 
			modeltransform.Model = value;
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", modeltransform);
		}
	}


	public static string GetVSSourceCode()
	{
		return @"#version 450 core


uniform ModelTransform
{
	mat4x4 Model;
};

uniform CameraTransform
{
	mat4x4 View;
	mat4x4 Proj;
};

uniform mat4 NormalMatrix;


layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec3 VertexNormal;
layout(location=2) in vec2 TexCoord;

layout(location=0) out vec4 OutPosition;
layout(location=1) out vec3 OutNormal;
layout(location=2) out vec2 OutTexCoord;


  
void main()
{	
	mat4 ModelView = View * Model;
	gl_Position = Proj * View * Model * vec4(VertexPosition, 1);
	OutPosition =   (ModelView * vec4(VertexPosition, 1));	
	OutNormal =  normalize(mat3(ModelView) * VertexNormal);
	OutTexCoord = TexCoord;
}";
	}

	public static string GetFSSourceCode()
	{
		return @"
#version 450 core


layout(location=0) in vec4 InPosition;
layout(location=1) in vec3 InNormal;
layout(location=2) in vec2 InTexCoord;



layout (location = 0) out vec4 PositionColor;
layout (location = 1) out vec4 DiffuseColor;
layout (location = 2) out vec4 NormalColor;


layout (location = 0, binding=0) uniform sampler2D DiffuseTex;
layout (location = 1, binding=1) uniform sampler2D NormalTex;
layout (location = 2, binding=2) uniform sampler2D MaskTex;
layout (location = 3, binding=3) uniform sampler2D MetalicTex;
layout (location = 4, binding=4) uniform sampler2D RoughnessTex;

void main()
{   	
	DiffuseColor = texture(DiffuseTex, InTexCoord);
    NormalColor = vec4(InNormal.xyz,0);    
    PositionColor = InPosition;
}";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace GBufferPNTT
{


public class GBufferPNTT : MaterialBase
{
	public GBufferPNTT() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}

	public void SetDiffuseTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"DiffuseTex", TextureObject);
	}

	public void SetDiffuseTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"DiffuseTex", textureObject, samplerObject);		
	}

	public TextureBase DiffuseTex2D 
	{	
		set => SetTexture(@"DiffuseTex", value);		
	}

	public TextureBase DiffuseTex2D_PointSample
	{	
		set => SetTexture(@"DiffuseTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase DiffuseTex2D_LinearSample
	{	
		set => SetTexture(@"DiffuseTex", value, Sampler.DefaultLinearSampler);
	}	



    private CameraTransform cameratransform = new CameraTransform();
	public CameraTransform CameraTransform
	{
		get { return cameratransform; }
		set 
		{ 
			cameratransform = value; 
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", cameratransform);
		}
	}
	public OpenTK.Mathematics.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", cameratransform);
		}
	}
    private ModelTransform modeltransform = new ModelTransform();
	public ModelTransform ModelTransform
	{
		get { return modeltransform; }
		set 
		{ 
			modeltransform = value; 
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 ModelTransform_Model
	{
		get { return modeltransform.Model ; }
		set 
		{ 
			modeltransform.Model = value;
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", modeltransform);
		}
	}


	public static string GetVSSourceCode()
	{
		return @"#version 450 core


uniform ModelTransform
{
	mat4x4 Model;
};

uniform CameraTransform
{
	mat4x4 View;
	mat4x4 Proj;
};

uniform mat4 NormalMatrix;


layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec3 VertexNormal;
layout(location=2) in vec2 TexCoord;
layout(location=3) in vec4 Tangent;

layout(location=0) out vec4 OutPosition;
layout(location=1) out vec3 OutNormal;
layout(location=2) out vec2 OutTexCoord;
layout(location=3) out vec4 OutTangent;

  
void main()
{	
	mat4 ModelView = View * Model;
	gl_Position = Proj * View * Model * vec4(VertexPosition, 1);
	OutPosition =   (ModelView * vec4(VertexPosition, 1));	
	OutNormal =  normalize(mat3(ModelView) * VertexNormal);
	OutTexCoord = TexCoord;
	OutTangent = Tangent;
}";
	}

	public static string GetFSSourceCode()
	{
		return @"
#version 450 core


layout(location=0) in vec4 InPosition;
layout(location=1) in vec3 InNormal;
layout(location=2) in vec2 InTexCoord;
layout(location=3) in vec4 InTangent;



layout (location = 0) out vec4 PositionColor;
layout (location = 1) out vec4 DiffuseColor;
layout (location = 2) out vec4 NormalColor;


layout (location = 0, binding=0) uniform sampler2D DiffuseTex;
layout (location = 1, binding=1) uniform sampler2D NormalTex;
layout (location = 2, binding=2) uniform sampler2D MaskTex;
layout (location = 3, binding=3) uniform sampler2D MetalicTex;
layout (location = 4, binding=4) uniform sampler2D RoughnessTex;

void main()
{   	
	DiffuseColor = texture(DiffuseTex, InTexCoord);
    NormalColor = vec4(InNormal.xyz,0);    
    PositionColor = InPosition;
}";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace Blur
{


public class Blur : MaterialBase
{
	public Blur() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}

	public void SetColorTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"ColorTex", TextureObject);
	}

	public void SetColorTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"ColorTex", textureObject, samplerObject);		
	}

	public TextureBase ColorTex2D 
	{	
		set => SetTexture(@"ColorTex", value);		
	}

	public TextureBase ColorTex2D_PointSample
	{	
		set => SetTexture(@"ColorTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase ColorTex2D_LinearSample
	{	
		set => SetTexture(@"ColorTex", value, Sampler.DefaultLinearSampler);
	}	

	public System.Boolean Horizontal
	{
		get { return horizontal; }
		set 
		{
			horizontal = value;
			SetUniformVariable(@"horizontal", horizontal);			
		}
	}
	private System.Boolean horizontal;
	public System.Single[] Weight
	{
		get { return weight; }
		set 
		{
			weight = value;
			SetUniformVariable(@"weight", weight);			
		}
	}
	private System.Single[] weight;




	public static string GetVSSourceCode()
	{
		return @"#version 450

layout (location = 0) in vec3 VertexPosition;
layout (location = 1) in vec2 VertexTexCoord;

layout (location = 0) out vec2 TexCoord;

void main()
{
    TexCoord = VertexTexCoord;  
	gl_Position = vec4(VertexPosition.xy, 0.0, 1.0);
}
";
	}

	public static string GetFSSourceCode()
	{
		return @"#version 450

out vec4 FragColor;
  
layout (location = 0) in vec2 TexCoords;

uniform sampler2D ColorTex;
  
uniform bool horizontal;
uniform float weight[5] = float[] (0.227027, 0.1945946, 0.1216216, 0.054054, 0.016216);

void main()
{             
    vec2 tex_offset = 1.0 / textureSize(ColorTex, 0); // gets size of single texel
    vec3 result = texture(ColorTex, TexCoords).rgb * weight[0]; // current fragment's contribution
    if(horizontal)
    {
        for(int i = 1; i < 5; ++i)
        {
            result += texture(ColorTex, TexCoords + vec2(tex_offset.x * i, 0.0)).rgb * weight[i];
            result += texture(ColorTex, TexCoords - vec2(tex_offset.x * i, 0.0)).rgb * weight[i];
        }
    }
    else
    {
        for(int i = 1; i < 5; ++i)
        {
            result += texture(ColorTex, TexCoords + vec2(0.0, tex_offset.y * i)).rgb * weight[i];
            result += texture(ColorTex, TexCoords - vec2(0.0, tex_offset.y * i)).rgb * weight[i];
        }
    }
    FragColor = vec4(result, 1.0);
}";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace BloomMaterial
{


public class BloomMaterial : MaterialBase
{
	public BloomMaterial() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}

	public void SetColorTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"ColorTex", TextureObject);
	}

	public void SetColorTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"ColorTex", textureObject, samplerObject);		
	}

	public TextureBase ColorTex2D 
	{	
		set => SetTexture(@"ColorTex", value);		
	}

	public TextureBase ColorTex2D_PointSample
	{	
		set => SetTexture(@"ColorTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase ColorTex2D_LinearSample
	{	
		set => SetTexture(@"ColorTex", value, Sampler.DefaultLinearSampler);
	}	





	public static string GetVSSourceCode()
	{
		return @"#version 450

layout (location = 0) in vec3 VertexPosition;
layout (location = 1) in vec2 VertexTexCoord;


layout (location = 0 ) out vec3 OutPosition;
layout (location = 1 ) out vec2 OutTexCoord;

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
		return @"#version 450


layout (location = 0 , binding = 0) uniform sampler2D ColorTex;

layout (location = 0 ) in vec3 InPosition;
layout (location = 1 ) in vec2 InTexCoord;

layout( location = 0 ) out vec4 FragColor;


void main() 
{
    vec4 color = texture(ColorTex, InTexCoord);
    float brightness = dot(color.xyz, vec3(0.2126, 0.7152, 0.0722));
    //float brightness = length(color.xyz);

    if(brightness >= 0.98)
        FragColor = vec4(color.xyz, 1.0f);
    else
        FragColor = vec4(0,0,0,1);
}";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace CubemapMaterial
{


public class CubemapMaterial : MaterialBase
{
	public CubemapMaterial() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}

	public void SettexCubemap2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"texCubemap", TextureObject);
	}

	public void SettexCubemap2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"texCubemap", textureObject, samplerObject);		
	}

	public TextureBase TexCubemap2D 
	{	
		set => SetTexture(@"texCubemap", value);		
	}

	public TextureBase TexCubemap2D_PointSample
	{	
		set => SetTexture(@"texCubemap", value, Sampler.DefaultPointSampler);
	}

	public TextureBase TexCubemap2D_LinearSample
	{	
		set => SetTexture(@"texCubemap", value, Sampler.DefaultLinearSampler);
	}	

	public System.Int32 LightChannel
	{
		get { return lightchannel; }
		set 
		{
			lightchannel = value;
			SetUniformVariable(@"LightChannel", lightchannel);			
		}
	}
	private System.Int32 lightchannel;
	public OpenTK.Mathematics.Matrix4 ModelMatrix
	{
		get { return modelmatrix; }
		set 
		{
			modelmatrix = value;
			SetUniformVariable(@"ModelMatrix", modelmatrix);			
		}
	}
	private OpenTK.Mathematics.Matrix4 modelmatrix;
	public OpenTK.Mathematics.Matrix4 ProjMatrix
	{
		get { return projmatrix; }
		set 
		{
			projmatrix = value;
			SetUniformVariable(@"ProjMatrix", projmatrix);			
		}
	}
	private OpenTK.Mathematics.Matrix4 projmatrix;
	public OpenTK.Mathematics.Matrix4 ViewMatrix
	{
		get { return viewmatrix; }
		set 
		{
			viewmatrix = value;
			SetUniformVariable(@"ViewMatrix", viewmatrix);			
		}
	}
	private OpenTK.Mathematics.Matrix4 viewmatrix;




	public static string GetVSSourceCode()
	{
		return @"#version 450 core

layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec3 VertexNormal;
layout(location=2) in vec2 TexCoord;
layout(location=3) in vec4 Tangent;

uniform mat4 ViewMatrix;
uniform mat4 ProjMatrix;
uniform mat4 ModelMatrix;

layout(location=0) out vec3 OutTexCoord;
layout(location=1) out vec2 TexCoord2;
  
void main()
{	
	vec4 vPosition = ProjMatrix * ViewMatrix * ModelMatrix * vec4(VertexPosition, 1.0);
	OutTexCoord = VertexPosition.xyz;
	TexCoord2 = TexCoord;
	//gl_Position = vec4(vPosition.xy, 0, 1.0);
	gl_Position = vPosition.xyww;
}
";
	}

	public static string GetFSSourceCode()
	{
		return @"
#version 450 core

layout (location=0) in vec3 CubemapTexCoord;
layout (location=1) in vec2 InTexCoord;

layout (location=0, binding=0) uniform samplerCube texCubemap;


layout (location = 0) out vec4 PositionColor;
layout (location = 1) out vec4 DiffuseColor;
layout (location = 2) out vec4 NormalColor;
layout (location = 3) out vec4 VelocityColor;

uniform int LightChannel = 0;

void main()
{
    vec4 Color = texture(texCubemap, -CubemapTexCoord);    
    DiffuseColor = Color;
	PositionColor.a = LightChannel;
}";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace MSGBufferMaterial
{


public class MSGBufferMaterial : MaterialBase
{
	public MSGBufferMaterial() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}


	public System.Int32 MaskMapExist
	{
		get { return maskmapexist; }
		set 
		{
			maskmapexist = value;
			SetUniformVariable(@"MaskMapExist", maskmapexist);			
		}
	}
	private System.Int32 maskmapexist;
	public System.Int32 NormalMapExist
	{
		get { return normalmapexist; }
		set 
		{
			normalmapexist = value;
			SetUniformVariable(@"NormalMapExist", normalmapexist);			
		}
	}
	private System.Int32 normalmapexist;
	public System.Int32 SpecularMapExist
	{
		get { return specularmapexist; }
		set 
		{
			specularmapexist = value;
			SetUniformVariable(@"SpecularMapExist", specularmapexist);			
		}
	}
	private System.Int32 specularmapexist;


    private CameraTransform cameratransform = new CameraTransform();
	public CameraTransform CameraTransform
	{
		get { return cameratransform; }
		set 
		{ 
			cameratransform = value; 
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", cameratransform);
		}
	}
	public OpenTK.Mathematics.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", cameratransform);
		}
	}
    private ModelTransform modeltransform = new ModelTransform();
	public ModelTransform ModelTransform
	{
		get { return modeltransform; }
		set 
		{ 
			modeltransform = value; 
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 ModelTransform_Model
	{
		get { return modeltransform.Model ; }
		set 
		{ 
			modeltransform.Model = value;
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", modeltransform);
		}
	}


	public static string GetVSSourceCode()
	{
		return @"#version 450 core

uniform ModelTransform
{
	mat4x4 Model;
};

uniform CameraTransform
{
	mat4x4 View;
	mat4x4 Proj;
};

uniform mat4 NormalMatrix;

uniform vec3 Value;


layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec3 VertexNormal;
layout(location=2) in vec2 TexCoord;
layout(location=3) in vec4 Tangent;


layout(location=0) out vec4 OutPosition;
layout(location=1) out vec2 OutTexCoord;
layout(location=2) out vec3 OutNormal;
layout(location=3) out vec3 OutTangent;
layout(location=4) out vec3 OutBinormal;

  
void main()
{	
	mat4 ModelView = View * Model;

	OutTexCoord = TexCoord;
	gl_Position = Proj * View * Model * vec4(VertexPosition, 1);
	OutPosition =   (ModelView * vec4(VertexPosition, 1));
	
	OutNormal =  normalize(mat3(ModelView) * VertexNormal);	

	OutTangent = normalize(mat3(ModelView) * vec3(Tangent));

	vec3 binormal = (cross( VertexNormal, Tangent.xyz )) * Tangent.w;
	OutBinormal = normalize(mat3(ModelView) * binormal);	
}";
	}

	public static string GetFSSourceCode()
	{
		return @"
#version 450 core


layout(location=0) in vec4 InPosition;
layout(location=1) in vec2 InTexCoord;
layout(location=2) in vec3 InNormal;
layout(location=3) in vec3 InTangent;
layout(location=4) in vec3 InBinormal;


layout (location = 0) out vec4 PositionColor;
layout (location = 1) out vec4 DiffuseColor;
layout (location = 2) out vec4 NormalColor;

layout (location = 0, binding=0) uniform sampler2DMS DiffuseTex;
layout (location = 1, binding=1) uniform sampler2DMS NormalTex;
layout (location = 2, binding=2) uniform sampler2DMS MaskTex;
layout (location = 3, binding=3) uniform sampler2DMS SpecularTex;

uniform int SpecularMapExist;
uniform int MaskMapExist;
uniform int NormalMapExist;

void main()
{   
    ivec2 TexCoord = ivec2(InTexCoord);
    if(MaskMapExist > 0)
    {
        vec4 MaskValue = texelFetch(MaskTex, TexCoord,0);
    	if(MaskValue.x > 0)
    	{
    		DiffuseColor = texelFetch(DiffuseTex, TexCoord,0);            
    	}
    	else
    	{
    		discard;
    	}
    }
    else
    {
    	DiffuseColor = texelFetch(DiffuseTex, TexCoord,0);
    }

    if(InPosition.w == 0)
    {
        DiffuseColor = vec4(1,0,0,0);
    }

    mat3 TangentToModelViewSpaceMatrix = mat3( InTangent.x, InTangent.y, InTangent.z, 
								    InBinormal.x, InBinormal.y, InBinormal.z, 
								    InNormal.x, InNormal.y, InNormal.z);

    if(NormalMapExist > 0)
    {
        vec3 NormalMapNormal = (2.0f * (texelFetch( NormalTex, TexCoord,0 ).xyz) - vec3(1.0f));
	    vec3 BumpNormal = normalize(TangentToModelViewSpaceMatrix * NormalMapNormal.xyz);
	
        NormalColor.xyz = BumpNormal.xyz;
    }
    else
    {
        NormalColor.xyz = InNormal.xyz;
    }

    if(SpecularMapExist > 0)
    {
        NormalColor.a = texelFetch(SpecularTex, TexCoord,0).x;
    }
    else
    {
        NormalColor.a = 0;
    }

    PositionColor = InPosition;
}";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace DepthVisualizeMaterial
{


public class DepthVisualizeMaterial : MaterialBase
{
	public DepthVisualizeMaterial() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}

	public void SetDepthTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"DepthTex", TextureObject);
	}

	public void SetDepthTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"DepthTex", textureObject, samplerObject);		
	}

	public TextureBase DepthTex2D 
	{	
		set => SetTexture(@"DepthTex", value);		
	}

	public TextureBase DepthTex2D_PointSample
	{	
		set => SetTexture(@"DepthTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase DepthTex2D_LinearSample
	{	
		set => SetTexture(@"DepthTex", value, Sampler.DefaultLinearSampler);
	}	

	public System.Single Far
	{
		get { return far; }
		set 
		{
			far = value;
			SetUniformVariable(@"Far", far);			
		}
	}
	private System.Single far;
	public System.Single Near
	{
		get { return near; }
		set 
		{
			near = value;
			SetUniformVariable(@"Near", near);			
		}
	}
	private System.Single near;




	public static string GetVSSourceCode()
	{
		return @"#version 450 core


layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec2 TexCoord;

layout(location=0) out vec2 OutTexCoord;


void main()
{	
	OutTexCoord = TexCoord;	    
	gl_Position = vec4(VertexPosition.xyz, 1.0);
}";
	}

	public static string GetFSSourceCode()
	{
		return @"
#version 450 core

layout (location=0) in vec2 InTexCoord;
layout (location=0) out vec4 FragColor;

layout (location=0, binding=0) uniform sampler2D DepthTex;

uniform float Near; 
uniform float Far;

void main() 
{   
	float depth = texture(DepthTex, InTexCoord).x;
	float linearizedDepth = (2.0 * Near) / (Far + Near - depth * (Far - Near));
    FragColor = vec4(linearizedDepth, linearizedDepth, linearizedDepth, 1.0f);
}";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace FontRenderMaterial
{


public class FontRenderMaterial : MaterialBase
{
	public FontRenderMaterial() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}

	public void SetFontTexture2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"FontTexture", TextureObject);
	}

	public void SetFontTexture2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"FontTexture", textureObject, samplerObject);		
	}

	public TextureBase FontTexture2D 
	{	
		set => SetTexture(@"FontTexture", value);		
	}

	public TextureBase FontTexture2D_PointSample
	{	
		set => SetTexture(@"FontTexture", value, Sampler.DefaultPointSampler);
	}

	public TextureBase FontTexture2D_LinearSample
	{	
		set => SetTexture(@"FontTexture", value, Sampler.DefaultLinearSampler);
	}	

	public OpenTK.Mathematics.Vector2 ScreenSize
	{
		get { return screensize; }
		set 
		{
			screensize = value;
			SetUniformVariable(@"ScreenSize", screensize);			
		}
	}
	private OpenTK.Mathematics.Vector2 screensize;




	public static string GetVSSourceCode()
	{
		return @"#version 450 core

layout(location=0) in vec2 VertexPosition;
layout(location=1) in vec2 VertexTexCoord;
layout(location=2) in vec4 VertexColor;

uniform vec2 ScreenSize;

layout(location=0) out vec2 TexCoord;
layout(location=1) out vec4 OutColor;
  
void main()
{	
	TexCoord = VertexTexCoord;
	OutColor = VertexColor;
	float fX = ((VertexPosition.x - ScreenSize.x * .5f) * 2.f) / ScreenSize.x;
	float fY = ((VertexPosition.y - ScreenSize.y * .5f) * 2.f) / ScreenSize.y;

	gl_Position = vec4(fX, -fY, 0.0, 1.0);
}";
	}

	public static string GetFSSourceCode()
	{
		return @"#version 450 core

layout(location=0) in vec2 TexCoord;
layout(location=1) in vec4 Color;

layout(binding=0) uniform sampler2D FontTexture;

layout(location = 0) out vec4 PositionColor;
layout(location = 1) out vec4 DiffuseColor;
layout(location = 2) out vec4 NormalColor;
layout(location = 3) out vec4 VelocityColor;

void main()
{   	
    DiffuseColor = texture(FontTexture, TexCoord);
    DiffuseColor.a = texture(FontTexture, TexCoord).r;
}";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace FontBoxRenderMaterial
{


public class FontBoxRenderMaterial : MaterialBase
{
	public FontBoxRenderMaterial() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}


	public System.Single BoxAlpha
	{
		get { return boxalpha; }
		set 
		{
			boxalpha = value;
			SetUniformVariable(@"BoxAlpha", boxalpha);			
		}
	}
	private System.Single boxalpha;
	public OpenTK.Mathematics.Vector3 BoxColor
	{
		get { return boxcolor; }
		set 
		{
			boxcolor = value;
			SetUniformVariable(@"BoxColor", boxcolor);			
		}
	}
	private OpenTK.Mathematics.Vector3 boxcolor;
	public OpenTK.Mathematics.Vector2 ScreenSize
	{
		get { return screensize; }
		set 
		{
			screensize = value;
			SetUniformVariable(@"ScreenSize", screensize);			
		}
	}
	private OpenTK.Mathematics.Vector2 screensize;




	public static string GetVSSourceCode()
	{
		return @"#version 450 core

layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec2 VertexTexCoord;

uniform vec2 ScreenSize;

out vec2 TexCoord;
  
void main()
{	
	TexCoord = VertexTexCoord;
	float fX = ((VertexPosition.x - ScreenSize.x * .5f) * 2.f) / ScreenSize.x;
	float fY = ((VertexPosition.y - ScreenSize.y * .5f) * 2.f) / ScreenSize.y;

	gl_Position = vec4(fX, fY, 0.0, 1.0);
}";
	}

	public static string GetFSSourceCode()
	{
		return @"#version 450 core

in vec2 TexCoord;

uniform vec3 BoxColor;
uniform float BoxAlpha;

out vec4 FragColor;

void main()
{   
    FragColor =vec4(BoxColor, BoxAlpha);
}";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace GridRenderMaterial
{


public class GridRenderMaterial : MaterialBase
{
	public GridRenderMaterial() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}


	public OpenTK.Mathematics.Vector3 LineColor
	{
		get { return linecolor; }
		set 
		{
			linecolor = value;
			SetUniformVariable(@"LineColor", linecolor);			
		}
	}
	private OpenTK.Mathematics.Vector3 linecolor;


    private CameraTransform cameratransform = new CameraTransform();
	public CameraTransform CameraTransform
	{
		get { return cameratransform; }
		set 
		{ 
			cameratransform = value; 
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", cameratransform);
		}
	}
	public OpenTK.Mathematics.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", cameratransform);
		}
	}
    private ModelTransform modeltransform = new ModelTransform();
	public ModelTransform ModelTransform
	{
		get { return modeltransform; }
		set 
		{ 
			modeltransform = value; 
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 ModelTransform_Model
	{
		get { return modeltransform.Model ; }
		set 
		{ 
			modeltransform.Model = value;
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", modeltransform);
		}
	}


	public static string GetVSSourceCode()
	{
		return @"#version 450 core

uniform ModelTransform
{
	mat4x4 Model;
};

uniform CameraTransform
{
	mat4x4 View;
	mat4x4 Proj;
};

layout(location=0) in vec3 VertexPosition;
  
void main()
{	
	gl_Position = Proj * View * Model * vec4(VertexPosition, 1);
}";
	}

	public static string GetFSSourceCode()
	{
		return @"#version 450 core

uniform vec3 LineColor;

out vec4 FragColor;

void main()
{   	
    FragColor =vec4(LineColor, 1);
}";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace ThreeDTextRenderMaterial
{


public class ThreeDTextRenderMaterial : MaterialBase
{
	public ThreeDTextRenderMaterial() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}

	public void SetFontTexture2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"FontTexture", TextureObject);
	}

	public void SetFontTexture2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"FontTexture", textureObject, samplerObject);		
	}

	public TextureBase FontTexture2D 
	{	
		set => SetTexture(@"FontTexture", value);		
	}

	public TextureBase FontTexture2D_PointSample
	{	
		set => SetTexture(@"FontTexture", value, Sampler.DefaultPointSampler);
	}

	public TextureBase FontTexture2D_LinearSample
	{	
		set => SetTexture(@"FontTexture", value, Sampler.DefaultLinearSampler);
	}	

	public OpenTK.Mathematics.Matrix4 Model
	{
		get { return model; }
		set 
		{
			model = value;
			SetUniformVariable(@"Model", model);			
		}
	}
	private OpenTK.Mathematics.Matrix4 model;
	public OpenTK.Mathematics.Matrix4 Proj
	{
		get { return proj; }
		set 
		{
			proj = value;
			SetUniformVariable(@"Proj", proj);			
		}
	}
	private OpenTK.Mathematics.Matrix4 proj;
	public OpenTK.Mathematics.Vector3 TextColor
	{
		get { return textcolor; }
		set 
		{
			textcolor = value;
			SetUniformVariable(@"TextColor", textcolor);			
		}
	}
	private OpenTK.Mathematics.Vector3 textcolor;
	public OpenTK.Mathematics.Matrix4 View
	{
		get { return view; }
		set 
		{
			view = value;
			SetUniformVariable(@"View", view);			
		}
	}
	private OpenTK.Mathematics.Matrix4 view;




	public static string GetVSSourceCode()
	{
		return @"#version 450 core

uniform mat4x4 Model;
uniform mat4x4 View;
uniform mat4x4 Proj;

layout(location=0) in vec3 VertexPosition;
layout(location=1) in vec2 TexCoord;

layout(location=0) out vec2 OutTexCoord;
  
void main()
{	
	gl_Position = Proj * View * Model * vec4(VertexPosition, 1);
	OutTexCoord = TexCoord;	
}";
	}

	public static string GetFSSourceCode()
	{
		return @"#version 450 core

layout(location=0) in vec2 TexCoord;

uniform vec3 TextColor;
uniform sampler2D FontTexture;

out vec4 FragColor;

void main()
{   
	vec4 TexCol = texture(FontTexture, TexCoord);
    FragColor =vec4(TextColor,TexCol.a);
}";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace ResolveMaterial
{


public class ResolveMaterial : MaterialBase
{
	public ResolveMaterial() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}

	public void SetColorTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"ColorTex", TextureObject);
	}

	public void SetColorTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"ColorTex", textureObject, samplerObject);		
	}

	public TextureBase ColorTex2D 
	{	
		set => SetTexture(@"ColorTex", value);		
	}

	public TextureBase ColorTex2D_PointSample
	{	
		set => SetTexture(@"ColorTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase ColorTex2D_LinearSample
	{	
		set => SetTexture(@"ColorTex", value, Sampler.DefaultLinearSampler);
	}	
	public void SetMotionTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"MotionTex", TextureObject);
	}

	public void SetMotionTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"MotionTex", textureObject, samplerObject);		
	}

	public TextureBase MotionTex2D 
	{	
		set => SetTexture(@"MotionTex", value);		
	}

	public TextureBase MotionTex2D_PointSample
	{	
		set => SetTexture(@"MotionTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase MotionTex2D_LinearSample
	{	
		set => SetTexture(@"MotionTex", value, Sampler.DefaultLinearSampler);
	}	





	public static string GetVSSourceCode()
	{
		return @"#version 450

layout (location = 0) in vec3 VertexPosition;
layout (location = 1) in vec2 VertexTexCoord;

layout (location = 0) out vec2 TexCoord;

void main()
{
    TexCoord = VertexTexCoord;  
	gl_Position = vec4(VertexPosition.xy, 0.0, 1.0);
}
";
	}

	public static string GetFSSourceCode()
	{
		return @"#version 450

out vec4 FragColor;
  
layout (location = 0) in vec2 TexCoords;

layout (binding = 0) uniform sampler2D ColorTex;
layout (binding = 1) uniform sampler2D BlurTex; 
layout (binding = 2) uniform sampler2D MotionTex; 


void main()
{
	vec4 vVelocity = texture(MotionTex, TexCoords);

    vec4 c = texture( ColorTex, TexCoords );

	if(length (vVelocity.xy) > 0)
    {        
        vec4 vPrevTickColor = texture(ColorTex, TexCoords + vVelocity.xy) * 0.4;        
        vPrevTickColor += texture(ColorTex, TexCoords + vVelocity.xy * 2) * 0.3;
        vPrevTickColor += texture(ColorTex, TexCoords + vVelocity.xy * 3) * 0.2;
        vPrevTickColor += texture(ColorTex, TexCoords + vVelocity.xy * 4) * 0.1;

        c = (c + vPrevTickColor) / 2.0f;
    }

	FragColor = c;	
}";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace SSAOMaterial
{


public class SSAOMaterial : MaterialBase
{
	public SSAOMaterial() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}

	public void SetNormalTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"NormalTex", TextureObject);
	}

	public void SetNormalTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"NormalTex", textureObject, samplerObject);		
	}

	public TextureBase NormalTex2D 
	{	
		set => SetTexture(@"NormalTex", value);		
	}

	public TextureBase NormalTex2D_PointSample
	{	
		set => SetTexture(@"NormalTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase NormalTex2D_LinearSample
	{	
		set => SetTexture(@"NormalTex", value, Sampler.DefaultLinearSampler);
	}	
	public void SetPositionTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"PositionTex", TextureObject);
	}

	public void SetPositionTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"PositionTex", textureObject, samplerObject);		
	}

	public TextureBase PositionTex2D 
	{	
		set => SetTexture(@"PositionTex", value);		
	}

	public TextureBase PositionTex2D_PointSample
	{	
		set => SetTexture(@"PositionTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase PositionTex2D_LinearSample
	{	
		set => SetTexture(@"PositionTex", value, Sampler.DefaultLinearSampler);
	}	
	public void SetRandTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"RandTex", TextureObject);
	}

	public void SetRandTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"RandTex", textureObject, samplerObject);		
	}

	public TextureBase RandTex2D 
	{	
		set => SetTexture(@"RandTex", value);		
	}

	public TextureBase RandTex2D_PointSample
	{	
		set => SetTexture(@"RandTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase RandTex2D_LinearSample
	{	
		set => SetTexture(@"RandTex", value, Sampler.DefaultLinearSampler);
	}	

	public OpenTK.Mathematics.Matrix4 ProjectionMatrix
	{
		get { return projectionmatrix; }
		set 
		{
			projectionmatrix = value;
			SetUniformVariable(@"ProjectionMatrix", projectionmatrix);			
		}
	}
	private OpenTK.Mathematics.Matrix4 projectionmatrix;
	public System.Single Radius
	{
		get { return radius; }
		set 
		{
			radius = value;
			SetUniformVariable(@"Radius", radius);			
		}
	}
	private System.Single radius;
	public OpenTK.Mathematics.Vector3[] SampleKernel
	{
		get { return samplekernel; }
		set 
		{
			samplekernel = value;
			SetUniformVariable(@"SampleKernel", samplekernel);			
		}
	}
	private OpenTK.Mathematics.Vector3[] samplekernel;
	public OpenTK.Mathematics.Vector2 ScreenSize
	{
		get { return screensize; }
		set 
		{
			screensize = value;
			SetUniformVariable(@"ScreenSize", screensize);			
		}
	}
	private OpenTK.Mathematics.Vector2 screensize;




	public static string GetVSSourceCode()
	{
		return @"#version 450

layout (location = 0) in vec3 VertexPosition;
layout (location = 1) in vec2 VertexTexCoord;


layout (location = 0 ) out vec3 OutPosition;
layout (location = 1 ) out vec2 OutTexCoord;

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



layout (location = 0 ) in vec3 InPosition;
layout (location = 1 ) in vec2 InTexCoord;

layout (location = 0) out float FragColor;

uniform mat4 ProjectionMatrix;

const int kernelSize = 64;

uniform vec3 SampleKernel[kernelSize];
uniform float Radius = 0.55;
uniform vec2 ScreenSize;

layout(binding=0) uniform sampler2D PositionTex;
layout(binding=1) uniform sampler2D NormalTex;
layout(binding=2) uniform sampler2D ColorTex;
layout(binding=4) uniform sampler2D RandTex;
layout(binding=5) uniform sampler2D DepthTex;

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

    FragColor = AoData;
    //FragColor = vec4(AoData, AoData, AoData, 1.0f);
    
}
";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace LUTGenerateMaterial
{


public class LUTGenerateMaterial : MaterialBase
{
	public LUTGenerateMaterial() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}






	public static string GetVSSourceCode()
	{
		return @"#version 450

layout (location = 0) in vec3 InPosition;
layout (location = 1) in vec2 InTexCoords;

layout (location = 0) out vec2 TexCoords;

void main()
{
    TexCoords = InTexCoords;
	gl_Position = vec4(InPosition, 1.0);
}";
	}

	public static string GetFSSourceCode()
	{
		return @"
#version 450

layout (location=0) out vec4 FragColor;
layout (location=0) in vec2 TexCoords;

const float PI = 3.14159265359;
// ----------------------------------------------------------------------------
// http://holger.dammertz.org/stuff/notes_HammersleyOnHemisphere.html
// efficient VanDerCorpus calculation.
float RadicalInverse_VdC(uint bits) 
{
     bits = (bits << 16u) | (bits >> 16u);
     bits = ((bits & 0x55555555u) << 1u) | ((bits & 0xAAAAAAAAu) >> 1u);
     bits = ((bits & 0x33333333u) << 2u) | ((bits & 0xCCCCCCCCu) >> 2u);
     bits = ((bits & 0x0F0F0F0Fu) << 4u) | ((bits & 0xF0F0F0F0u) >> 4u);
     bits = ((bits & 0x00FF00FFu) << 8u) | ((bits & 0xFF00FF00u) >> 8u);
     return float(bits) * 2.3283064365386963e-10; // / 0x100000000
}
// ----------------------------------------------------------------------------
vec2 Hammersley(uint i, uint N)
{
    return vec2(float(i)/float(N), RadicalInverse_VdC(i));
}
// ----------------------------------------------------------------------------
vec3 ImportanceSampleGGX(vec2 Xi, vec3 N, float roughness)
{
    float a = roughness*roughness;
    
    float phi = 2.0 * PI * Xi.x;
    float cosTheta = sqrt((1.0 - Xi.y) / (1.0 + (a*a - 1.0) * Xi.y));
    float sinTheta = sqrt(1.0 - cosTheta*cosTheta);
    
    // from spherical coordinates to cartesian coordinates - halfway vector
    vec3 H;
    H.x = cos(phi) * sinTheta;
    H.y = sin(phi) * sinTheta;
    H.z = cosTheta;
    
    // from tangent-space H vector to world-space sample vector
    vec3 up          = abs(N.z) < 0.999 ? vec3(0.0, 0.0, 1.0) : vec3(1.0, 0.0, 0.0);
    vec3 tangent   = normalize(cross(up, N));
    vec3 bitangent = cross(N, tangent);
    
    vec3 sampleVec = tangent * H.x + bitangent * H.y + N * H.z;
    return normalize(sampleVec);
}
// ----------------------------------------------------------------------------
float GeometrySchlickGGX(float NdotV, float roughness)
{
    // note that we use a different k for IBL
    float a = roughness;
    float k = (a * a) / 2.0;

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
vec2 IntegrateBRDF(float NdotV, float roughness)
{
    vec3 V;
    V.x = sqrt(1.0 - NdotV*NdotV);
    V.y = 0.0;
    V.z = NdotV;

    float A = 0.0;
    float B = 0.0; 

    vec3 N = vec3(0.0, 0.0, 1.0);
    
    const uint SAMPLE_COUNT = 1024u;
    for(uint i = 0u; i < SAMPLE_COUNT; ++i)
    {
        // generates a sample vector that's biased towards the
        // preferred alignment direction (importance sampling).
        vec2 Xi = Hammersley(i, SAMPLE_COUNT);
        vec3 H = ImportanceSampleGGX(Xi, N, roughness);
        vec3 L = normalize(2.0 * dot(V, H) * H - V);

        float NdotL = max(L.z, 0.0);
        float NdotH = max(H.z, 0.0);
        float VdotH = max(dot(V, H), 0.0);

        if(NdotL > 0.0)
        {
            float G = GeometrySmith(N, V, L, roughness);
            float G_Vis = (G * VdotH) / (NdotH * NdotV);
            float Fc = pow(1.0 - VdotH, 5.0);

            A += (1.0 - Fc) * G_Vis;
            B += Fc * G_Vis;
        }
    }
    A /= float(SAMPLE_COUNT);
    B /= float(SAMPLE_COUNT);
    return vec2(A, B);
}
// ----------------------------------------------------------------------------
void main() 
{
    vec2 integratedBRDF = IntegrateBRDF(TexCoords.x, TexCoords.y);
    FragColor.xy = integratedBRDF;    
}";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace PrefilterMaterial
{


public class PrefilterMaterial : MaterialBase
{
	public PrefilterMaterial() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}

	public void SetEnvironmentMap2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"EnvironmentMap", TextureObject);
	}

	public void SetEnvironmentMap2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"EnvironmentMap", textureObject, samplerObject);		
	}

	public TextureBase EnvironmentMap2D 
	{	
		set => SetTexture(@"EnvironmentMap", value);		
	}

	public TextureBase EnvironmentMap2D_PointSample
	{	
		set => SetTexture(@"EnvironmentMap", value, Sampler.DefaultPointSampler);
	}

	public TextureBase EnvironmentMap2D_LinearSample
	{	
		set => SetTexture(@"EnvironmentMap", value, Sampler.DefaultLinearSampler);
	}	

	public OpenTK.Mathematics.Matrix4 Projection
	{
		get { return projection; }
		set 
		{
			projection = value;
			SetUniformVariable(@"Projection", projection);			
		}
	}
	private OpenTK.Mathematics.Matrix4 projection;
	public OpenTK.Mathematics.Matrix4 View
	{
		get { return view; }
		set 
		{
			view = value;
			SetUniformVariable(@"View", view);			
		}
	}
	private OpenTK.Mathematics.Matrix4 view;
	public System.Single Roughness
	{
		get { return roughness; }
		set 
		{
			roughness = value;
			SetUniformVariable(@"roughness", roughness);			
		}
	}
	private System.Single roughness;




	public static string GetVSSourceCode()
	{
		return @"#version 450

layout (location = 0) in vec3 Position;

layout (location = 0) out vec3 WorldPos;

uniform mat4 Projection;
uniform mat4 View;

void main()
{
    WorldPos = Position;
    gl_Position =  Projection * View * vec4(WorldPos, 1.0);
}";
	}

	public static string GetFSSourceCode()
	{
		return @"#version 450


layout (location = 0) out vec4 FragColor;

layout (location = 0) in vec3 WorldPos;

layout (location = 0, binding=0) uniform samplerCube EnvironmentMap;

uniform float roughness;

const float PI = 3.14159265359;

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
// http://holger.dammertz.org/stuff/notes_HammersleyOnHemisphere.html
// efficient VanDerCorpus calculation.
float RadicalInverse_VdC(uint bits) 
{
     bits = (bits << 16u) | (bits >> 16u);
     bits = ((bits & 0x55555555u) << 1u) | ((bits & 0xAAAAAAAAu) >> 1u);
     bits = ((bits & 0x33333333u) << 2u) | ((bits & 0xCCCCCCCCu) >> 2u);
     bits = ((bits & 0x0F0F0F0Fu) << 4u) | ((bits & 0xF0F0F0F0u) >> 4u);
     bits = ((bits & 0x00FF00FFu) << 8u) | ((bits & 0xFF00FF00u) >> 8u);
     return float(bits) * 2.3283064365386963e-10; // / 0x100000000
}
// ----------------------------------------------------------------------------
vec2 Hammersley(uint i, uint N)
{
	return vec2(float(i)/float(N), RadicalInverse_VdC(i));
}
// ----------------------------------------------------------------------------
vec3 ImportanceSampleGGX(vec2 Xi, vec3 N, float roughness)
{
	float a = roughness*roughness;
	
	float phi = 2.0 * PI * Xi.x;
	float cosTheta = sqrt((1.0 - Xi.y) / (1.0 + (a*a - 1.0) * Xi.y));
	float sinTheta = sqrt(1.0 - cosTheta*cosTheta);
	
	// from spherical coordinates to cartesian coordinates - halfway vector
	vec3 H;
	H.x = cos(phi) * sinTheta;
	H.y = sin(phi) * sinTheta;
	H.z = cosTheta;
	
	// from tangent-space H vector to world-space sample vector
	vec3 up          = abs(N.z) < 0.999 ? vec3(0.0, 0.0, 1.0) : vec3(1.0, 0.0, 0.0);
	vec3 tangent   = normalize(cross(up, N));
	vec3 bitangent = cross(N, tangent);
	
	vec3 sampleVec = tangent * H.x + bitangent * H.y + N * H.z;
	return normalize(sampleVec);
}
// ----------------------------------------------------------------------------
void main()
{		
    vec3 N = normalize(WorldPos);
    
    // make the simplyfying assumption that V equals R equals the normal 
    vec3 R = N;
    vec3 V = R;

    const uint SAMPLE_COUNT = 1024u;
    vec3 prefilteredColor = vec3(0.0);
    float totalWeight = 0.0;
    
    for(uint i = 0u; i < SAMPLE_COUNT; ++i)
    {
        // generates a sample vector that's biased towards the preferred alignment direction (importance sampling).
        vec2 Xi = Hammersley(i, SAMPLE_COUNT);
        vec3 H = ImportanceSampleGGX(Xi, N, roughness);
        vec3 L  = normalize(2.0 * dot(V, H) * H - V);

        float NdotL = max(dot(N, L), 0.0);
        if(NdotL > 0.0)
        {
            // sample from the environment's mip level based on roughness/pdf
            float D   = DistributionGGX(N, H, roughness);
            float NdotH = max(dot(N, H), 0.0);
            float HdotV = max(dot(H, V), 0.0);
            float pdf = D * NdotH / (4.0 * HdotV) + 0.0001; 

            float resolution = 512.0; // resolution of source cubemap (per face)
            float saTexel  = 4.0 * PI / (6.0 * resolution * resolution);
            float saSample = 1.0 / (float(SAMPLE_COUNT) * pdf + 0.0001);

            float mipLevel = roughness == 0.0 ? 0.0 : 0.5 * log2(saSample / saTexel); 
            
            prefilteredColor += textureLod(EnvironmentMap, L, mipLevel).rgb * NdotL;
            totalWeight      += NdotL;
        }
    }

    prefilteredColor = prefilteredColor / totalWeight;

    FragColor = vec4(prefilteredColor, 1.0);
}";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace FXAAMaterial
{


public class FXAAMaterial : MaterialBase
{
	public FXAAMaterial() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}

	public void SetScreenTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"ScreenTex", TextureObject);
	}

	public void SetScreenTex2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"ScreenTex", textureObject, samplerObject);		
	}

	public TextureBase ScreenTex2D 
	{	
		set => SetTexture(@"ScreenTex", value);		
	}

	public TextureBase ScreenTex2D_PointSample
	{	
		set => SetTexture(@"ScreenTex", value, Sampler.DefaultPointSampler);
	}

	public TextureBase ScreenTex2D_LinearSample
	{	
		set => SetTexture(@"ScreenTex", value, Sampler.DefaultLinearSampler);
	}	

	public OpenTK.Mathematics.Vector2 InverseScreenSize
	{
		get { return inversescreensize; }
		set 
		{
			inversescreensize = value;
			SetUniformVariable(@"InverseScreenSize", inversescreensize);			
		}
	}
	private OpenTK.Mathematics.Vector2 inversescreensize;




	public static string GetVSSourceCode()
	{
		return @"#version 450

layout (location = 0) in vec3 VertexPosition;
layout (location = 1) in vec2 VertexTexCoord;


layout (location = 0 ) out vec3 OutPosition;
layout (location = 1 ) out vec2 OutTexCoord;

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
		return @"
//http://blog.simonrodriguez.fr/articles/30-07-2016_implementing_fxaa.html

#version 450 core

layout (location = 0 ) in vec3 InPosition;
layout (location = 1 ) in vec2 InTexCoord;

layout (location = 0) out vec4 FragColor;

layout(binding=0) uniform sampler2D ScreenTex;



uniform vec2 InverseScreenSize;

float Rgb2Luma(vec3 rgb)
{
	return sqrt(dot(rgb, vec3(0.299,0.587,0.114)));
}

float QUALITY(int i)
{
	//#define FXAA_QUALITY__PS 12
    //#define FXAA_QUALITY__P0 1.0
    //#define FXAA_QUALITY__P1 1.0
    //#define FXAA_QUALITY__P2 1.0
    //#define FXAA_QUALITY__P3 1.0
    //#define FXAA_QUALITY__P4 1.0
    //#define FXAA_QUALITY__P5 1.5
    //#define FXAA_QUALITY__P6 2.0
    //#define FXAA_QUALITY__P7 2.0
    //#define FXAA_QUALITY__P8 2.0
    //#define FXAA_QUALITY__P9 2.0
    //#define FXAA_QUALITY__P10 4.0
    //#define FXAA_QUALITY__P11 8.0

	if ( i == 0 || i == 1 || i == 2 || i == 3 || i == 4 )
	{
		return 1.0;
	}
	else if(i == 5)
	{
		return 1.5;
	}
	else if(i == 6 || i == 7 || i == 8 || i == 9)
	{
		return 2;
	}
	else if (i == 10)
	{
		return 4;
	}
	else
	{
		return 8;
	}

	return 0;
}

float EDGE_THRESHOLD_MIN = 0.0312;
float EDGE_THRESHOLD_MAX = 0.125;

void main()
{
	vec4 colorCenter = texture(ScreenTex,InTexCoord);

	// Luma at the current fragment
	float lumaCenter = Rgb2Luma(colorCenter.rgb);

	// Luma at the four direct neighbours of the current fragment.
	float lumaDown = Rgb2Luma(textureOffset(ScreenTex,InTexCoord,ivec2(0,-1)).rgb);
	float lumaUp = Rgb2Luma(textureOffset(ScreenTex,InTexCoord,ivec2(0,1)).rgb);
	float lumaLeft = Rgb2Luma(textureOffset(ScreenTex,InTexCoord,ivec2(-1,0)).rgb);
	float lumaRight = Rgb2Luma(textureOffset(ScreenTex,InTexCoord,ivec2(1,0)).rgb);

	// Find the maximum and minimum luma around the current fragment.
	float lumaMin = min(lumaCenter,min(min(lumaDown,lumaUp),min(lumaLeft,lumaRight)));
	float lumaMax = max(lumaCenter,max(max(lumaDown,lumaUp),max(lumaLeft,lumaRight)));

	// Compute the delta.
	float lumaRange = lumaMax - lumaMin;

	// If the luma variation is lower that a threshold (or if we are in a really dark area), we are not on an edge, don't perform any AA.
	if(lumaRange < max(EDGE_THRESHOLD_MIN, lumaMax * EDGE_THRESHOLD_MAX))
	{
		FragColor = colorCenter;
		return;
	}
	////////////////////////////////////////////////////////////
	// Query the 4 remaining corners lumas.
	float lumaDownLeft = Rgb2Luma(textureOffset(ScreenTex,InTexCoord,ivec2(-1,-1)).rgb);
	float lumaUpRight = Rgb2Luma(textureOffset(ScreenTex,InTexCoord,ivec2(1,1)).rgb);
	float lumaUpLeft = Rgb2Luma(textureOffset(ScreenTex,InTexCoord,ivec2(-1,1)).rgb);
	float lumaDownRight = Rgb2Luma(textureOffset(ScreenTex,InTexCoord,ivec2(1,-1)).rgb);

	// Combine the four edges lumas (using intermediary variables for future computations with the same values).
	float lumaDownUp = lumaDown + lumaUp;
	float lumaLeftRight = lumaLeft + lumaRight;

	// Same for corners
	float lumaLeftCorners = lumaDownLeft + lumaUpLeft;
	float lumaDownCorners = lumaDownLeft + lumaDownRight;
	float lumaRightCorners = lumaDownRight + lumaUpRight;
	float lumaUpCorners = lumaUpRight + lumaUpLeft;

	// Compute an estimation of the gradient along the horizontal and vertical axis.
	float edgeHorizontal =  abs(-2.0 * lumaLeft + lumaLeftCorners)  + abs(-2.0 * lumaCenter + lumaDownUp ) * 2.0    + abs(-2.0 * lumaRight + lumaRightCorners);
	float edgeVertical =    abs(-2.0 * lumaUp + lumaUpCorners)      + abs(-2.0 * lumaCenter + lumaLeftRight) * 2.0  + abs(-2.0 * lumaDown + lumaDownCorners);

	// Is the local edge horizontal or vertical ?
	bool isHorizontal = (edgeHorizontal >= edgeVertical);

	////////////////////////////////////////////////////////////
	// Select the two neighboring texels lumas in the opposite direction to the local edge.
	float luma1 = isHorizontal ? lumaDown : lumaLeft;
	float luma2 = isHorizontal ? lumaUp : lumaRight;
	// Compute gradients in this direction.
	float gradient1 = luma1 - lumaCenter;
	float gradient2 = luma2 - lumaCenter;

	// Which direction is the steepest ?
	bool is1Steepest = abs(gradient1) >= abs(gradient2);

	// Gradient in the corresponding direction, normalized.
	float gradientScaled = 0.25*max(abs(gradient1),abs(gradient2));
	////////////////////////////////////////////////////////////

	////////////////////////////////////////////////////////////
	// Choose the step size (one pixel) according to the edge direction.
	float stepLength = isHorizontal ? InverseScreenSize.y : InverseScreenSize.x;

	// Average luma in the correct direction.
	float lumaLocalAverage = 0.0;

	if(is1Steepest)
	{
		// Switch the direction
		stepLength = - stepLength;
		lumaLocalAverage = 0.5*(luma1 + lumaCenter);
	} 
	else 
	{
		lumaLocalAverage = 0.5*(luma2 + lumaCenter);
	}

	// Shift UV in the correct direction by half a pixel.
	vec2 currentUv = InTexCoord;
	if(isHorizontal)
	{
		currentUv.y += stepLength * 0.5;
	}
	else 
	{
		currentUv.x += stepLength * 0.5;
	}
	////////////////////////////////////////////////////////////

	////////////////////////////////////////////////////////////
	// Compute offset (for each iteration step) in the right direction.
	vec2 offset = isHorizontal ? vec2(InverseScreenSize.x,0.0) : vec2(0.0,InverseScreenSize.y);
	// Compute UVs to explore on each side of the edge, orthogonally. The QUALITY allows us to step faster.
	vec2 uv1 = currentUv - offset;
	vec2 uv2 = currentUv + offset;

	// Read the lumas at both current extremities of the exploration segment, and compute the delta wrt to the local average luma.
	float lumaEnd1 = Rgb2Luma(texture(ScreenTex,uv1).rgb);
	float lumaEnd2 = Rgb2Luma(texture(ScreenTex,uv2).rgb);
	lumaEnd1 -= lumaLocalAverage;
	lumaEnd2 -= lumaLocalAverage;

	// If the luma deltas at the current extremities are larger than the local gradient, we have reached the side of the edge.
	bool reached1 = abs(lumaEnd1) >= gradientScaled;
	bool reached2 = abs(lumaEnd2) >= gradientScaled;
	bool reachedBoth = reached1 && reached2;

	// If the side is not reached, we continue to explore in this direction.
	if(!reached1)
	{
		uv1 -= offset;
	}

	if(!reached2)
	{
		uv2 += offset;
	}   
	////////////////////////////////////////////////////////////
	// If both sides have not been reached, continue to explore.
	if(!reachedBoth)
	{
		int ITERATIONS = 12;
		for(int i = 2; i < ITERATIONS; i++)
		{
			// If needed, read luma in 1st direction, compute delta.
			if(!reached1)
			{
				lumaEnd1 = Rgb2Luma(texture(ScreenTex, uv1).rgb);
				lumaEnd1 = lumaEnd1 - lumaLocalAverage;
			}
			// If needed, read luma in opposite direction, compute delta.
			if(!reached2)
			{
				lumaEnd2 = Rgb2Luma(texture(ScreenTex, uv2).rgb);
				lumaEnd2 = lumaEnd2 - lumaLocalAverage;
			}
			// If the luma deltas at the current extremities is larger than the local gradient, we have reached the side of the edge.
			reached1 = abs(lumaEnd1) >= gradientScaled;
			reached2 = abs(lumaEnd2) >= gradientScaled;
			reachedBoth = reached1 && reached2;

			// If the side is not reached, we continue to explore in this direction, with a variable quality.
			if(!reached1)
			{
				uv1 -= offset * QUALITY(i);
			}
			if(!reached2)
			{
				uv2 += offset * QUALITY(i);
			}

			// If both sides have been reached, stop the exploration.
			if(reachedBoth){ break;}
		}
	}

	// Compute the distances to each extremity of the edge.
	float distance1 = isHorizontal ? (InTexCoord.x - uv1.x) : (InTexCoord.y - uv1.y);
	float distance2 = isHorizontal ? (uv2.x - InTexCoord.x) : (uv2.y - InTexCoord.y);

	// In which direction is the extremity of the edge closer ?
	bool isDirection1 = distance1 < distance2;
	float distanceFinal = min(distance1, distance2);

	// Length of the edge.
	float edgeThickness = (distance1 + distance2);

	// UV offset: read in the direction of the closest side of the edge.
	float pixelOffset = - distanceFinal / edgeThickness + 0.5;

	// Is the luma at center smaller than the local average ?
	bool isLumaCenterSmaller = lumaCenter < lumaLocalAverage;

	// If the luma at center is smaller than at its neighbour, the delta luma at each end should be positive (same variation).
	// (in the direction of the closer side of the edge.)
	bool correctVariation = ((isDirection1 ? lumaEnd1 : lumaEnd2) < 0.0) != isLumaCenterSmaller;

	// If the luma variation is incorrect, do not offset.
	float finalOffset = correctVariation ? pixelOffset : 0.0;

	// Sub-pixel shifting
	// Full weighted average of the luma over the 3x3 neighborhood.
	float lumaAverage = (1.0/12.0) * (2.0 * (lumaDownUp + lumaLeftRight) + lumaLeftCorners + lumaRightCorners);
	// Ratio of the delta between the global average and the center luma, over the luma range in the 3x3 neighborhood.
	float subPixelOffset1 = clamp(abs(lumaAverage - lumaCenter)/lumaRange,0.0,1.0);
	float subPixelOffset2 = (-2.0 * subPixelOffset1 + 3.0) * subPixelOffset1 * subPixelOffset1;
	// Compute a sub-pixel offset based on this delta.
	float SUBPIXEL_QUALITY = 0.75;
	float subPixelOffsetFinal = subPixelOffset2 * subPixelOffset2 * SUBPIXEL_QUALITY;


	// Pick the biggest of the two offsets.
	finalOffset = max(finalOffset,subPixelOffsetFinal);

	// Compute the final UV coordinates.
	vec2 finalUv = InTexCoord;
	if(isHorizontal)
	{
		finalUv.y += finalOffset * stepLength;
	} 
	else 
	{
		finalUv.x += finalOffset * stepLength;
	}
	
    // Read the color at the new UV coordinates, and use it.
	vec4 finalColor = texture(ScreenTex, finalUv);
	FragColor = finalColor;
}";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace TBNMaterial
{


public class TBNMaterial : MaterialBase
{
	public TBNMaterial() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}




    private CameraTransform cameratransform = new CameraTransform();
	public CameraTransform CameraTransform
	{
		get { return cameratransform; }
		set 
		{ 
			cameratransform = value; 
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", cameratransform);
		}
	}
	public OpenTK.Mathematics.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", cameratransform);
		}
	}
    private ModelTransform modeltransform = new ModelTransform();
	public ModelTransform ModelTransform
	{
		get { return modeltransform; }
		set 
		{ 
			modeltransform = value; 
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", value);
		}
	}

	public OpenTK.Mathematics.Matrix4 ModelTransform_Model
	{
		get { return modeltransform.Model ; }
		set 
		{ 
			modeltransform.Model = value;
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", modeltransform);
		}
	}


	public static string GetVSSourceCode()
	{
		return @"
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

";
	}

	public static string GetFSSourceCode()
	{
		return @"
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
}";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}
namespace SignedDistanceField
{


public class SignedDistanceField : MaterialBase
{
	public SignedDistanceField() 
	 : base (GetVSSourceCode(), GetFSSourceCode(), GetGSSourceCode())
	{	
	}

	public ShaderProgram GetProgramObject()
	{
		return mMaterialProgram;
	}

	public void Use()
	{
		mMaterialProgram.UseProgram();
	}

	public void SetFontTexture2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"FontTexture", TextureObject);
	}

	public void SetFontTexture2D(Core.Texture.TextureBase textureObject, Sampler samplerObject)
	{
		SetTexture(@"FontTexture", textureObject, samplerObject);		
	}

	public TextureBase FontTexture2D 
	{	
		set => SetTexture(@"FontTexture", value);		
	}

	public TextureBase FontTexture2D_PointSample
	{	
		set => SetTexture(@"FontTexture", value, Sampler.DefaultPointSampler);
	}

	public TextureBase FontTexture2D_LinearSample
	{	
		set => SetTexture(@"FontTexture", value, Sampler.DefaultLinearSampler);
	}	





	public static string GetVSSourceCode()
	{
		return @"/* Freetype GL - A C OpenGL Freetype engine
 *
 * Distributed under the OSI-approved BSD 2-Clause License.  See accompanying
 * file `LICENSE` for more details.
 */

#version 450

layout (location = 0) in vec3 VertexPosition;
layout (location = 1) in vec2 VertexTexCoord;

layout (location = 0) out vec2 OutTexCoord;


uniform mat4 Model;
uniform mat4 View;
uniform mat4 Projection;
uniform vec4 u_color;

//attribute vec3 vertex;
//attribute vec2 tex_coord;
//attribute vec4 color;


void main(void)
{
	OutTexCoord.xy = VertexTexCoord.xy;	

    
    // gl_TexCoord[0].xy = tex_coord.xy;
    // gl_FrontColor     = color * u_color;    

    gl_Position = vec4(VertexPosition.xy, 0, 1.0);

    // gl_Position       = Projection*(View*(Model*vec4(VertexPosition,1.0)));
}
";
	}

	public static string GetFSSourceCode()
	{
		return @"/* Freetype GL - A C OpenGL Freetype engine
 *
 * Distributed under the OSI-approved BSD 2-Clause License.  See accompanying
 * file `LICENSE` for more details.
 */

#version 450 core

layout (binding = 0) uniform sampler2D FontTexture;

       vec3 glyph_color    = vec3(1.0,1.0,1.0);
const float glyph_center   = 0.50;
       vec3 outline_color  = vec3(0.0,0.0,0.0);
const float outline_center = 0.55;
       vec3 glow_color     = vec3(1.0,1.0,1.0);
const float glow_center    = 1.25;




layout (location = 0 ) in vec2 InTexCoord;

layout (location = 0) out vec4 FragColor;

void main(void)
{
    vec4  color = texture(FontTexture, InTexCoord.xy);
    float dist  = color.r;
    float width = fwidth(dist);
    float alpha = smoothstep(glyph_center-width, glyph_center+width, dist);

    // Smooth
    // gl_FragColor = vec4(glyph_color, alpha);

    // Outline
    // float mu = smoothstep(outline_center-width, outline_center+width, dist);
    // vec3 rgb = mix(outline_color, glyph_color, mu);
    // gl_FragColor = vec4(rgb, max(alpha,mu));

    // Glow
    //vec3 rgb = mix(glow_color, glyph_color, alpha);
    //float mu = smoothstep(glyph_center, glow_center, sqrt(dist));
    //gl_FragColor = vec4(rgb, max(alpha,mu));

    // Glow + outline
    vec3 rgb = mix(glow_color, glyph_color, alpha);
    float mu = smoothstep(glyph_center, glow_center, sqrt(dist));
    color = vec4(rgb, max(alpha,mu));
    float beta = smoothstep(outline_center-width, outline_center+width, dist);
    rgb = mix(outline_color, color.rgb, beta);
    FragColor = vec4(rgb, max(color.a,beta));    
}


";
	}

	public static string GetGSSourceCode()
	{
		return @"";
	}
}
}

}
