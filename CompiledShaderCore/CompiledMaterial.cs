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
namespace BasicMaterial
{


public class BasicMaterial : MaterialBase
{
	public BasicMaterial() 
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





    private ColorBlock colorblock = new ColorBlock();
	public ColorBlock ColorBlock
	{
		get { return colorblock; }
		set 
		{ 
			colorblock = value; 
			this.SetUniformBufferValue< ColorBlock >(@"ColorBlock", ref value);
		}
	}

	public OpenTK.Mathematics.Vector3 ColorBlock_Value
	{
		get { return colorblock.Value ; }
		set 
		{ 
			colorblock.Value = value;
			this.SetUniformBufferValue< ColorBlock >(@"ColorBlock", ref colorblock);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Vector3 >(@"ColorBlock", ref value, 0 );
		}
	}

    private Transform transform = new Transform();
	public Transform Transform
	{
		get { return transform; }
		set 
		{ 
			transform = value; 
			this.SetUniformBufferValue< Transform >(@"Transform", ref value);
		}
	}

	public OpenTK.Mathematics.Matrix4 Transform_Model
	{
		get { return transform.Model ; }
		set 
		{ 
			transform.Model = value;
			this.SetUniformBufferValue< Transform >(@"Transform", ref transform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"Transform", ref value, 0 );
		}
	}
	public OpenTK.Mathematics.Matrix4 Transform_View
	{
		get { return transform.View ; }
		set 
		{ 
			transform.View = value;
			this.SetUniformBufferValue< Transform >(@"Transform", ref transform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"Transform", ref value, 64 );
		}
	}
	public OpenTK.Mathematics.Matrix4 Transform_Proj
	{
		get { return transform.Proj ; }
		set 
		{ 
			transform.Proj = value;
			this.SetUniformBufferValue< Transform >(@"Transform", ref transform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"Transform", ref value, 128 );
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
}
}
namespace SimpleMaterial
{


public class SimpleMaterial : MaterialBase
{
	public SimpleMaterial() 
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





    private Transform transform = new Transform();
	public Transform Transform
	{
		get { return transform; }
		set 
		{ 
			transform = value; 
			this.SetUniformBufferValue< Transform >(@"Transform", ref value);
		}
	}

	public OpenTK.Mathematics.Matrix4 Transform_Model
	{
		get { return transform.Model ; }
		set 
		{ 
			transform.Model = value;
			this.SetUniformBufferValue< Transform >(@"Transform", ref transform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"Transform", ref value, 0 );
		}
	}
	public OpenTK.Mathematics.Matrix4 Transform_View
	{
		get { return transform.View ; }
		set 
		{ 
			transform.View = value;
			this.SetUniformBufferValue< Transform >(@"Transform", ref transform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"Transform", ref value, 64 );
		}
	}
	public OpenTK.Mathematics.Matrix4 Transform_Proj
	{
		get { return transform.Proj ; }
		set 
		{ 
			transform.Proj = value;
			this.SetUniformBufferValue< Transform >(@"Transform", ref transform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"Transform", ref value, 128 );
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
}
}
namespace ScreenSpaceDraw
{


public class ScreenSpaceDraw : MaterialBase
{
	public ScreenSpaceDraw() 
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

	public void SetColorTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"ColorTex", TextureObject);
	}

	public void SetColorTex2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"ColorTex", TextureObject, SamplerObject);
	}

	public TextureBase ColorTex2D 
	{	
		set 
		{	
			colortex = value;
			SetTexture(@"ColorTex", colortex);
		}
	}

	public TextureBase ColorTex2D_PointSample
	{	
		set 
		{	
			colortex = value;
			SetTexture(@"ColorTex", colortex, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase ColorTex2D_LinearSample
	{	
		set 
		{	
			colortex = value;
			SetTexture(@"ColorTex", colortex, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase colortex = null;






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
}
}
namespace GBufferDraw
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

	public void SetDiffuseTex2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"DiffuseTex", TextureObject, SamplerObject);
	}

	public TextureBase DiffuseTex2D 
	{	
		set 
		{	
			diffusetex = value;
			SetTexture(@"DiffuseTex", diffusetex);
		}
	}

	public TextureBase DiffuseTex2D_PointSample
	{	
		set 
		{	
			diffusetex = value;
			SetTexture(@"DiffuseTex", diffusetex, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase DiffuseTex2D_LinearSample
	{	
		set 
		{	
			diffusetex = value;
			SetTexture(@"DiffuseTex", diffusetex, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase diffusetex = null;
	public void SetMaskTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"MaskTex", TextureObject);
	}

	public void SetMaskTex2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"MaskTex", TextureObject, SamplerObject);
	}

	public TextureBase MaskTex2D 
	{	
		set 
		{	
			masktex = value;
			SetTexture(@"MaskTex", masktex);
		}
	}

	public TextureBase MaskTex2D_PointSample
	{	
		set 
		{	
			masktex = value;
			SetTexture(@"MaskTex", masktex, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase MaskTex2D_LinearSample
	{	
		set 
		{	
			masktex = value;
			SetTexture(@"MaskTex", masktex, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase masktex = null;
	public void SetMetalicTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"MetalicTex", TextureObject);
	}

	public void SetMetalicTex2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"MetalicTex", TextureObject, SamplerObject);
	}

	public TextureBase MetalicTex2D 
	{	
		set 
		{	
			metalictex = value;
			SetTexture(@"MetalicTex", metalictex);
		}
	}

	public TextureBase MetalicTex2D_PointSample
	{	
		set 
		{	
			metalictex = value;
			SetTexture(@"MetalicTex", metalictex, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase MetalicTex2D_LinearSample
	{	
		set 
		{	
			metalictex = value;
			SetTexture(@"MetalicTex", metalictex, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase metalictex = null;
	public void SetNormalTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"NormalTex", TextureObject);
	}

	public void SetNormalTex2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"NormalTex", TextureObject, SamplerObject);
	}

	public TextureBase NormalTex2D 
	{	
		set 
		{	
			normaltex = value;
			SetTexture(@"NormalTex", normaltex);
		}
	}

	public TextureBase NormalTex2D_PointSample
	{	
		set 
		{	
			normaltex = value;
			SetTexture(@"NormalTex", normaltex, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase NormalTex2D_LinearSample
	{	
		set 
		{	
			normaltex = value;
			SetTexture(@"NormalTex", normaltex, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase normaltex = null;
	public void SetRoughnessTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"RoughnessTex", TextureObject);
	}

	public void SetRoughnessTex2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"RoughnessTex", TextureObject, SamplerObject);
	}

	public TextureBase RoughnessTex2D 
	{	
		set 
		{	
			roughnesstex = value;
			SetTexture(@"RoughnessTex", roughnesstex);
		}
	}

	public TextureBase RoughnessTex2D_PointSample
	{	
		set 
		{	
			roughnesstex = value;
			SetTexture(@"RoughnessTex", roughnesstex, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase RoughnessTex2D_LinearSample
	{	
		set 
		{	
			roughnesstex = value;
			SetTexture(@"RoughnessTex", roughnesstex, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase roughnesstex = null;


	
	public System.Boolean DiffuseMapExist
	{
		get { return diffusemapexist; }
		set 
		{
			diffusemapexist = value;
			SetUniformVarData(@"DiffuseMapExist", diffusemapexist);			
		}
	}
	private System.Boolean diffusemapexist;
	
	public OpenTK.Mathematics.Vector3 DiffuseOverride
	{
		get { return diffuseoverride; }
		set 
		{
			diffuseoverride = value;
			SetUniformVarData(@"DiffuseOverride", diffuseoverride);			
		}
	}
	private OpenTK.Mathematics.Vector3 diffuseoverride;
	
	public System.Int32 LightChannel
	{
		get { return lightchannel; }
		set 
		{
			lightchannel = value;
			SetUniformVarData(@"LightChannel", lightchannel);			
		}
	}
	private System.Int32 lightchannel;
	
	public System.Boolean MaskMapExist
	{
		get { return maskmapexist; }
		set 
		{
			maskmapexist = value;
			SetUniformVarData(@"MaskMapExist", maskmapexist);			
		}
	}
	private System.Boolean maskmapexist;
	
	public System.Single Metalic
	{
		get { return metalic; }
		set 
		{
			metalic = value;
			SetUniformVarData(@"Metalic", metalic);			
		}
	}
	private System.Single metalic;
	
	public System.Boolean MetalicExist
	{
		get { return metalicexist; }
		set 
		{
			metalicexist = value;
			SetUniformVarData(@"MetalicExist", metalicexist);			
		}
	}
	private System.Boolean metalicexist;
	
	public System.Boolean NormalMapExist
	{
		get { return normalmapexist; }
		set 
		{
			normalmapexist = value;
			SetUniformVarData(@"NormalMapExist", normalmapexist);			
		}
	}
	private System.Boolean normalmapexist;
	
	public System.Single Roughness
	{
		get { return roughness; }
		set 
		{
			roughness = value;
			SetUniformVarData(@"Roughness", roughness);			
		}
	}
	private System.Single roughness;
	
	public System.Boolean RoughnessExist
	{
		get { return roughnessexist; }
		set 
		{
			roughnessexist = value;
			SetUniformVarData(@"RoughnessExist", roughnessexist);			
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
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref value);
		}
	}

	public OpenTK.Mathematics.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"CameraTransform", ref value, 0 );
		}
	}
	public OpenTK.Mathematics.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"CameraTransform", ref value, 64 );
		}
	}

    private ModelTransform modeltransform = new ModelTransform();
	public ModelTransform ModelTransform
	{
		get { return modeltransform; }
		set 
		{ 
			modeltransform = value; 
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", ref value);
		}
	}

	public OpenTK.Mathematics.Matrix4 ModelTransform_Model
	{
		get { return modeltransform.Model ; }
		set 
		{ 
			modeltransform.Model = value;
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", ref modeltransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"ModelTransform", ref value, 0 );
		}
	}

    private PrevTransform prevtransform = new PrevTransform();
	public PrevTransform PrevTransform
	{
		get { return prevtransform; }
		set 
		{ 
			prevtransform = value; 
			this.SetUniformBufferValue< PrevTransform >(@"PrevTransform", ref value);
		}
	}

	public OpenTK.Mathematics.Matrix4 PrevTransform_PrevProj
	{
		get { return prevtransform.PrevProj ; }
		set 
		{ 
			prevtransform.PrevProj = value;
			this.SetUniformBufferValue< PrevTransform >(@"PrevTransform", ref prevtransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"PrevTransform", ref value, 0 );
		}
	}
	public OpenTK.Mathematics.Matrix4 PrevTransform_PrevModel
	{
		get { return prevtransform.PrevModel ; }
		set 
		{ 
			prevtransform.PrevModel = value;
			this.SetUniformBufferValue< PrevTransform >(@"PrevTransform", ref prevtransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"PrevTransform", ref value, 64 );
		}
	}
	public OpenTK.Mathematics.Matrix4 PrevTransform_PrevView
	{
		get { return prevtransform.PrevView ; }
		set 
		{ 
			prevtransform.PrevView = value;
			this.SetUniformBufferValue< PrevTransform >(@"PrevTransform", ref prevtransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"PrevTransform", ref value, 128 );
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
layout (location = 3, binding=3) uniform sampler2D MetalicTex;
layout (location = 4, binding=4) uniform sampler2D RoughnessTex;

uniform bool MetalicExist;
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

    if(MetalicExist)
    {
        NormalColor.a = texture(MetalicTex, InTexCoord).x;        
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
}
}
namespace EquirectangleToCube
{


public class EquirectangleToCube : MaterialBase
{
	public EquirectangleToCube() 
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

	public void SetEquirectangularMap2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"EquirectangularMap", TextureObject);
	}

	public void SetEquirectangularMap2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"EquirectangularMap", TextureObject, SamplerObject);
	}

	public TextureBase EquirectangularMap2D 
	{	
		set 
		{	
			equirectangularmap = value;
			SetTexture(@"EquirectangularMap", equirectangularmap);
		}
	}

	public TextureBase EquirectangularMap2D_PointSample
	{	
		set 
		{	
			equirectangularmap = value;
			SetTexture(@"EquirectangularMap", equirectangularmap, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase EquirectangularMap2D_LinearSample
	{	
		set 
		{	
			equirectangularmap = value;
			SetTexture(@"EquirectangularMap", equirectangularmap, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase equirectangularmap = null;

	public OpenTK.Mathematics.Matrix4 Projection
	{
		get { return projection; }
		set 
		{
			projection = value;
			SetUniformVarData(@"Projection", projection);			
		}
	}
	private OpenTK.Mathematics.Matrix4 projection;
	public OpenTK.Mathematics.Matrix4 View
	{
		get { return view; }
		set 
		{
			view = value;
			SetUniformVarData(@"View", view);			
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
}
}
namespace CubemapConvolution
{


public class CubemapConvolution : MaterialBase
{
	public CubemapConvolution() 
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

	public void SetEnvironmentMap2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"EnvironmentMap", TextureObject);
	}

	public void SetEnvironmentMap2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"EnvironmentMap", TextureObject, SamplerObject);
	}

	public TextureBase EnvironmentMap2D 
	{	
		set 
		{	
			environmentmap = value;
			SetTexture(@"EnvironmentMap", environmentmap);
		}
	}

	public TextureBase EnvironmentMap2D_PointSample
	{	
		set 
		{	
			environmentmap = value;
			SetTexture(@"EnvironmentMap", environmentmap, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase EnvironmentMap2D_LinearSample
	{	
		set 
		{	
			environmentmap = value;
			SetTexture(@"EnvironmentMap", environmentmap, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase environmentmap = null;

	public OpenTK.Mathematics.Matrix4 Projection
	{
		get { return projection; }
		set 
		{
			projection = value;
			SetUniformVarData(@"Projection", projection);			
		}
	}
	private OpenTK.Mathematics.Matrix4 projection;
	public OpenTK.Mathematics.Matrix4 View
	{
		get { return view; }
		set 
		{
			view = value;
			SetUniformVarData(@"View", view);			
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
}
}
namespace GBufferInstanced
{


public class GBufferInstanced : MaterialBase
{
	public GBufferInstanced() 
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

	public void SetDiffuseTex2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"DiffuseTex", TextureObject, SamplerObject);
	}

	public TextureBase DiffuseTex2D 
	{	
		set 
		{	
			diffusetex = value;
			SetTexture(@"DiffuseTex", diffusetex);
		}
	}

	public TextureBase DiffuseTex2D_PointSample
	{	
		set 
		{	
			diffusetex = value;
			SetTexture(@"DiffuseTex", diffusetex, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase DiffuseTex2D_LinearSample
	{	
		set 
		{	
			diffusetex = value;
			SetTexture(@"DiffuseTex", diffusetex, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase diffusetex = null;
	public void SetMetalicTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"MetalicTex", TextureObject);
	}

	public void SetMetalicTex2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"MetalicTex", TextureObject, SamplerObject);
	}

	public TextureBase MetalicTex2D 
	{	
		set 
		{	
			metalictex = value;
			SetTexture(@"MetalicTex", metalictex);
		}
	}

	public TextureBase MetalicTex2D_PointSample
	{	
		set 
		{	
			metalictex = value;
			SetTexture(@"MetalicTex", metalictex, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase MetalicTex2D_LinearSample
	{	
		set 
		{	
			metalictex = value;
			SetTexture(@"MetalicTex", metalictex, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase metalictex = null;
	public void SetNormalTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"NormalTex", TextureObject);
	}

	public void SetNormalTex2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"NormalTex", TextureObject, SamplerObject);
	}

	public TextureBase NormalTex2D 
	{	
		set 
		{	
			normaltex = value;
			SetTexture(@"NormalTex", normaltex);
		}
	}

	public TextureBase NormalTex2D_PointSample
	{	
		set 
		{	
			normaltex = value;
			SetTexture(@"NormalTex", normaltex, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase NormalTex2D_LinearSample
	{	
		set 
		{	
			normaltex = value;
			SetTexture(@"NormalTex", normaltex, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase normaltex = null;
	public void SetRoughnessTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"RoughnessTex", TextureObject);
	}

	public void SetRoughnessTex2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"RoughnessTex", TextureObject, SamplerObject);
	}

	public TextureBase RoughnessTex2D 
	{	
		set 
		{	
			roughnesstex = value;
			SetTexture(@"RoughnessTex", roughnesstex);
		}
	}

	public TextureBase RoughnessTex2D_PointSample
	{	
		set 
		{	
			roughnesstex = value;
			SetTexture(@"RoughnessTex", roughnesstex, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase RoughnessTex2D_LinearSample
	{	
		set 
		{	
			roughnesstex = value;
			SetTexture(@"RoughnessTex", roughnesstex, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase roughnesstex = null;

	public System.Int32 MetallicCount
	{
		get { return metalliccount; }
		set 
		{
			metalliccount = value;
			SetUniformVarData(@"MetallicCount", metalliccount);			
		}
	}
	private System.Int32 metalliccount;
	public System.Int32 RoughnessCount
	{
		get { return roughnesscount; }
		set 
		{
			roughnesscount = value;
			SetUniformVarData(@"RoughnessCount", roughnesscount);			
		}
	}
	private System.Int32 roughnesscount;

	
	public System.Boolean DiffuseMapExist
	{
		get { return diffusemapexist; }
		set 
		{
			diffusemapexist = value;
			SetUniformVarData(@"DiffuseMapExist", diffusemapexist);			
		}
	}
	private System.Boolean diffusemapexist;
	
	public OpenTK.Mathematics.Vector3 DiffuseOverride
	{
		get { return diffuseoverride; }
		set 
		{
			diffuseoverride = value;
			SetUniformVarData(@"DiffuseOverride", diffuseoverride);			
		}
	}
	private OpenTK.Mathematics.Vector3 diffuseoverride;
	
	public System.Int32 LightChannel
	{
		get { return lightchannel; }
		set 
		{
			lightchannel = value;
			SetUniformVarData(@"LightChannel", lightchannel);			
		}
	}
	private System.Int32 lightchannel;
	
	public System.Boolean MetalicExist
	{
		get { return metalicexist; }
		set 
		{
			metalicexist = value;
			SetUniformVarData(@"MetalicExist", metalicexist);			
		}
	}
	private System.Boolean metalicexist;
	
	public System.Boolean NormalMapExist
	{
		get { return normalmapexist; }
		set 
		{
			normalmapexist = value;
			SetUniformVarData(@"NormalMapExist", normalmapexist);			
		}
	}
	private System.Boolean normalmapexist;
	
	public System.Boolean RoughnessExist
	{
		get { return roughnessexist; }
		set 
		{
			roughnessexist = value;
			SetUniformVarData(@"RoughnessExist", roughnessexist);			
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
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref value);
		}
	}

	public OpenTK.Mathematics.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"CameraTransform", ref value, 0 );
		}
	}
	public OpenTK.Mathematics.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"CameraTransform", ref value, 64 );
		}
	}

    private ModelTransform modeltransform = new ModelTransform();
	public ModelTransform ModelTransform
	{
		get { return modeltransform; }
		set 
		{ 
			modeltransform = value; 
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", ref value);
		}
	}

	public OpenTK.Mathematics.Matrix4 ModelTransform_Model
	{
		get { return modeltransform.Model ; }
		set 
		{ 
			modeltransform.Model = value;
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", ref modeltransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"ModelTransform", ref value, 0 );
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
}
}
namespace GBufferWithoutTexture
{


public class GBufferWithoutTexture : MaterialBase
{
	public GBufferWithoutTexture() 
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





    private CameraTransform cameratransform = new CameraTransform();
	public CameraTransform CameraTransform
	{
		get { return cameratransform; }
		set 
		{ 
			cameratransform = value; 
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref value);
		}
	}

	public OpenTK.Mathematics.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"CameraTransform", ref value, 0 );
		}
	}
	public OpenTK.Mathematics.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"CameraTransform", ref value, 64 );
		}
	}

    private ModelTransform modeltransform = new ModelTransform();
	public ModelTransform ModelTransform
	{
		get { return modeltransform; }
		set 
		{ 
			modeltransform = value; 
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", ref value);
		}
	}

	public OpenTK.Mathematics.Matrix4 ModelTransform_Model
	{
		get { return modeltransform.Model ; }
		set 
		{ 
			modeltransform.Model = value;
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", ref modeltransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"ModelTransform", ref value, 0 );
		}
	}

    private PrevTransform prevtransform = new PrevTransform();
	public PrevTransform PrevTransform
	{
		get { return prevtransform; }
		set 
		{ 
			prevtransform = value; 
			this.SetUniformBufferValue< PrevTransform >(@"PrevTransform", ref value);
		}
	}

	public OpenTK.Mathematics.Matrix4 PrevTransform_PrevProj
	{
		get { return prevtransform.PrevProj ; }
		set 
		{ 
			prevtransform.PrevProj = value;
			this.SetUniformBufferValue< PrevTransform >(@"PrevTransform", ref prevtransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"PrevTransform", ref value, 0 );
		}
	}
	public OpenTK.Mathematics.Matrix4 PrevTransform_PrevModel
	{
		get { return prevtransform.PrevModel ; }
		set 
		{ 
			prevtransform.PrevModel = value;
			this.SetUniformBufferValue< PrevTransform >(@"PrevTransform", ref prevtransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"PrevTransform", ref value, 64 );
		}
	}
	public OpenTK.Mathematics.Matrix4 PrevTransform_PrevView
	{
		get { return prevtransform.PrevView ; }
		set 
		{ 
			prevtransform.PrevView = value;
			this.SetUniformBufferValue< PrevTransform >(@"PrevTransform", ref prevtransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"PrevTransform", ref value, 128 );
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
}
}
namespace GBufferDump
{


public class GBufferDump : MaterialBase
{
	public GBufferDump() 
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

	public void SetDiffuseTex2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"DiffuseTex", TextureObject, SamplerObject);
	}

	public TextureBase DiffuseTex2D 
	{	
		set 
		{	
			diffusetex = value;
			SetTexture(@"DiffuseTex", diffusetex);
		}
	}

	public TextureBase DiffuseTex2D_PointSample
	{	
		set 
		{	
			diffusetex = value;
			SetTexture(@"DiffuseTex", diffusetex, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase DiffuseTex2D_LinearSample
	{	
		set 
		{	
			diffusetex = value;
			SetTexture(@"DiffuseTex", diffusetex, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase diffusetex = null;
	public void SetMotionBlurTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"MotionBlurTex", TextureObject);
	}

	public void SetMotionBlurTex2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"MotionBlurTex", TextureObject, SamplerObject);
	}

	public TextureBase MotionBlurTex2D 
	{	
		set 
		{	
			motionblurtex = value;
			SetTexture(@"MotionBlurTex", motionblurtex);
		}
	}

	public TextureBase MotionBlurTex2D_PointSample
	{	
		set 
		{	
			motionblurtex = value;
			SetTexture(@"MotionBlurTex", motionblurtex, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase MotionBlurTex2D_LinearSample
	{	
		set 
		{	
			motionblurtex = value;
			SetTexture(@"MotionBlurTex", motionblurtex, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase motionblurtex = null;
	public void SetNormalTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"NormalTex", TextureObject);
	}

	public void SetNormalTex2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"NormalTex", TextureObject, SamplerObject);
	}

	public TextureBase NormalTex2D 
	{	
		set 
		{	
			normaltex = value;
			SetTexture(@"NormalTex", normaltex);
		}
	}

	public TextureBase NormalTex2D_PointSample
	{	
		set 
		{	
			normaltex = value;
			SetTexture(@"NormalTex", normaltex, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase NormalTex2D_LinearSample
	{	
		set 
		{	
			normaltex = value;
			SetTexture(@"NormalTex", normaltex, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase normaltex = null;
	public void SetPositionTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"PositionTex", TextureObject);
	}

	public void SetPositionTex2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"PositionTex", TextureObject, SamplerObject);
	}

	public TextureBase PositionTex2D 
	{	
		set 
		{	
			positiontex = value;
			SetTexture(@"PositionTex", positiontex);
		}
	}

	public TextureBase PositionTex2D_PointSample
	{	
		set 
		{	
			positiontex = value;
			SetTexture(@"PositionTex", positiontex, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase PositionTex2D_LinearSample
	{	
		set 
		{	
			positiontex = value;
			SetTexture(@"PositionTex", positiontex, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase positiontex = null;





    private Dump dump = new Dump();
	public Dump Dump
	{
		get { return dump; }
		set 
		{ 
			dump = value; 
			this.SetUniformBufferValue< Dump >(@"Dump", ref dump);
		}
	}

	public System.Boolean Dump_PositionDump
	{
		get { return dump.PositionDump ; }
		set 
		{ 
			dump.PositionDump = value; 
			this.SetUniformBufferValue< Dump >(@"Dump", ref dump);
			//this.SetUniformBufferMemberValue< System.Boolean >(@"Dump", ref value, 0 );
		}
	}
	public System.Boolean Dump_NormalDump
	{
		get { return dump.NormalDump ; }
		set 
		{ 
			dump.NormalDump = value; 
			this.SetUniformBufferValue< Dump >(@"Dump", ref dump);
			//this.SetUniformBufferMemberValue< System.Boolean >(@"Dump", ref value, 4 );
		}
	}
	public System.Boolean Dump_MetalicDump
	{
		get { return dump.MetalicDump ; }
		set 
		{ 
			dump.MetalicDump = value; 
			this.SetUniformBufferValue< Dump >(@"Dump", ref dump);
			//this.SetUniformBufferMemberValue< System.Boolean >(@"Dump", ref value, 8 );
		}
	}
	public System.Boolean Dump_DiffuseDump
	{
		get { return dump.DiffuseDump ; }
		set 
		{ 
			dump.DiffuseDump = value; 
			this.SetUniformBufferValue< Dump >(@"Dump", ref dump);
			//this.SetUniformBufferMemberValue< System.Boolean >(@"Dump", ref value, 12 );
		}
	}
	public System.Boolean Dump_RoughnessDump
	{
		get { return dump.RoughnessDump ; }
		set 
		{ 
			dump.RoughnessDump = value; 
			this.SetUniformBufferValue< Dump >(@"Dump", ref dump);
			//this.SetUniformBufferMemberValue< System.Boolean >(@"Dump", ref value, 16 );
		}
	}
	public System.Boolean Dump_MotionBlurDump
	{
		get { return dump.MotionBlurDump ; }
		set 
		{ 
			dump.MotionBlurDump = value; 
			this.SetUniformBufferValue< Dump >(@"Dump", ref dump);
			//this.SetUniformBufferMemberValue< System.Boolean >(@"Dump", ref value, 20 );
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
}
}
namespace GBufferPNC
{


public class GBufferPNC : MaterialBase
{
	public GBufferPNC() 
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


	public OpenTK.Mathematics.Matrix4 Model
	{
		get { return model; }
		set 
		{
			model = value;
			SetUniformVarData(@"Model", model);			
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
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref value);
		}
	}

	public OpenTK.Mathematics.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"CameraTransform", ref value, 0 );
		}
	}
	public OpenTK.Mathematics.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"CameraTransform", ref value, 64 );
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
}
}
namespace GBufferCubeTest
{


public class GBufferCubeTest : MaterialBase
{
	public GBufferCubeTest() 
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

	public void SetEquirectangularMap2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"EquirectangularMap", TextureObject);
	}

	public void SetEquirectangularMap2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"EquirectangularMap", TextureObject, SamplerObject);
	}

	public TextureBase EquirectangularMap2D 
	{	
		set 
		{	
			equirectangularmap = value;
			SetTexture(@"EquirectangularMap", equirectangularmap);
		}
	}

	public TextureBase EquirectangularMap2D_PointSample
	{	
		set 
		{	
			equirectangularmap = value;
			SetTexture(@"EquirectangularMap", equirectangularmap, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase EquirectangularMap2D_LinearSample
	{	
		set 
		{	
			equirectangularmap = value;
			SetTexture(@"EquirectangularMap", equirectangularmap, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase equirectangularmap = null;




    private CameraTransform cameratransform = new CameraTransform();
	public CameraTransform CameraTransform
	{
		get { return cameratransform; }
		set 
		{ 
			cameratransform = value; 
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref value);
		}
	}

	public OpenTK.Mathematics.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"CameraTransform", ref value, 0 );
		}
	}
	public OpenTK.Mathematics.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"CameraTransform", ref value, 64 );
		}
	}

    private ModelTransform modeltransform = new ModelTransform();
	public ModelTransform ModelTransform
	{
		get { return modeltransform; }
		set 
		{ 
			modeltransform = value; 
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", ref value);
		}
	}

	public OpenTK.Mathematics.Matrix4 ModelTransform_Model
	{
		get { return modeltransform.Model ; }
		set 
		{ 
			modeltransform.Model = value;
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", ref modeltransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"ModelTransform", ref value, 0 );
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
}
}
namespace GBufferPNCT
{


public class GBufferPNCT : MaterialBase
{
	public GBufferPNCT() 
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

	public void SetSpecularTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"SpecularTex", TextureObject);
	}

	public void SetSpecularTex2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"SpecularTex", TextureObject, SamplerObject);
	}

	public TextureBase SpecularTex2D 
	{	
		set 
		{	
			speculartex = value;
			SetTexture(@"SpecularTex", speculartex);
		}
	}

	public TextureBase SpecularTex2D_PointSample
	{	
		set 
		{	
			speculartex = value;
			SetTexture(@"SpecularTex", speculartex, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase SpecularTex2D_LinearSample
	{	
		set 
		{	
			speculartex = value;
			SetTexture(@"SpecularTex", speculartex, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase speculartex = null;

	public OpenTK.Mathematics.Matrix4 Model
	{
		get { return model; }
		set 
		{
			model = value;
			SetUniformVarData(@"Model", model);			
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
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref value);
		}
	}

	public OpenTK.Mathematics.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"CameraTransform", ref value, 0 );
		}
	}
	public OpenTK.Mathematics.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"CameraTransform", ref value, 64 );
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
}
}
namespace Blur
{


public class Blur : MaterialBase
{
	public Blur() 
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

	public void SetColorTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"ColorTex", TextureObject);
	}

	public void SetColorTex2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"ColorTex", TextureObject, SamplerObject);
	}

	public TextureBase ColorTex2D 
	{	
		set 
		{	
			colortex = value;
			SetTexture(@"ColorTex", colortex);
		}
	}

	public TextureBase ColorTex2D_PointSample
	{	
		set 
		{	
			colortex = value;
			SetTexture(@"ColorTex", colortex, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase ColorTex2D_LinearSample
	{	
		set 
		{	
			colortex = value;
			SetTexture(@"ColorTex", colortex, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase colortex = null;


	
	public System.Boolean Horizontal
	{
		get { return horizontal; }
		set 
		{
			horizontal = value;
			SetUniformVarData(@"horizontal", horizontal);			
		}
	}
	private System.Boolean horizontal;
	
	public System.Single[] Weight
	{
		get { return weight; }
		set 
		{
			weight = value;
			SetUniformVarData(@"weight", ref weight);			
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
}
}
namespace BloomMaterial
{


public class BloomMaterial : MaterialBase
{
	public BloomMaterial() 
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

	public void SetColorTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"ColorTex", TextureObject);
	}

	public void SetColorTex2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"ColorTex", TextureObject, SamplerObject);
	}

	public TextureBase ColorTex2D 
	{	
		set 
		{	
			colortex = value;
			SetTexture(@"ColorTex", colortex);
		}
	}

	public TextureBase ColorTex2D_PointSample
	{	
		set 
		{	
			colortex = value;
			SetTexture(@"ColorTex", colortex, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase ColorTex2D_LinearSample
	{	
		set 
		{	
			colortex = value;
			SetTexture(@"ColorTex", colortex, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase colortex = null;






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
}
}
namespace LightMaterial
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

	public void SetBrdfLUT2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"BrdfLUT", TextureObject);
	}

	public void SetBrdfLUT2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"BrdfLUT", TextureObject, SamplerObject);
	}

	public TextureBase BrdfLUT2D 
	{	
		set 
		{	
			brdflut = value;
			SetTexture(@"BrdfLUT", brdflut);
		}
	}

	public TextureBase BrdfLUT2D_PointSample
	{	
		set 
		{	
			brdflut = value;
			SetTexture(@"BrdfLUT", brdflut, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase BrdfLUT2D_LinearSample
	{	
		set 
		{	
			brdflut = value;
			SetTexture(@"BrdfLUT", brdflut, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase brdflut = null;
	public void SetDiffuseTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"DiffuseTex", TextureObject);
	}

	public void SetDiffuseTex2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"DiffuseTex", TextureObject, SamplerObject);
	}

	public TextureBase DiffuseTex2D 
	{	
		set 
		{	
			diffusetex = value;
			SetTexture(@"DiffuseTex", diffusetex);
		}
	}

	public TextureBase DiffuseTex2D_PointSample
	{	
		set 
		{	
			diffusetex = value;
			SetTexture(@"DiffuseTex", diffusetex, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase DiffuseTex2D_LinearSample
	{	
		set 
		{	
			diffusetex = value;
			SetTexture(@"DiffuseTex", diffusetex, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase diffusetex = null;
	public void SetIrradianceMap2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"IrradianceMap", TextureObject);
	}

	public void SetIrradianceMap2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"IrradianceMap", TextureObject, SamplerObject);
	}

	public TextureBase IrradianceMap2D 
	{	
		set 
		{	
			irradiancemap = value;
			SetTexture(@"IrradianceMap", irradiancemap);
		}
	}

	public TextureBase IrradianceMap2D_PointSample
	{	
		set 
		{	
			irradiancemap = value;
			SetTexture(@"IrradianceMap", irradiancemap, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase IrradianceMap2D_LinearSample
	{	
		set 
		{	
			irradiancemap = value;
			SetTexture(@"IrradianceMap", irradiancemap, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase irradiancemap = null;
	public void SetNormalTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"NormalTex", TextureObject);
	}

	public void SetNormalTex2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"NormalTex", TextureObject, SamplerObject);
	}

	public TextureBase NormalTex2D 
	{	
		set 
		{	
			normaltex = value;
			SetTexture(@"NormalTex", normaltex);
		}
	}

	public TextureBase NormalTex2D_PointSample
	{	
		set 
		{	
			normaltex = value;
			SetTexture(@"NormalTex", normaltex, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase NormalTex2D_LinearSample
	{	
		set 
		{	
			normaltex = value;
			SetTexture(@"NormalTex", normaltex, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase normaltex = null;
	public void SetPositionTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"PositionTex", TextureObject);
	}

	public void SetPositionTex2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"PositionTex", TextureObject, SamplerObject);
	}

	public TextureBase PositionTex2D 
	{	
		set 
		{	
			positiontex = value;
			SetTexture(@"PositionTex", positiontex);
		}
	}

	public TextureBase PositionTex2D_PointSample
	{	
		set 
		{	
			positiontex = value;
			SetTexture(@"PositionTex", positiontex, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase PositionTex2D_LinearSample
	{	
		set 
		{	
			positiontex = value;
			SetTexture(@"PositionTex", positiontex, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase positiontex = null;
	public void SetPrefilterMap2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"PrefilterMap", TextureObject);
	}

	public void SetPrefilterMap2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"PrefilterMap", TextureObject, SamplerObject);
	}

	public TextureBase PrefilterMap2D 
	{	
		set 
		{	
			prefiltermap = value;
			SetTexture(@"PrefilterMap", prefiltermap);
		}
	}

	public TextureBase PrefilterMap2D_PointSample
	{	
		set 
		{	
			prefiltermap = value;
			SetTexture(@"PrefilterMap", prefiltermap, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase PrefilterMap2D_LinearSample
	{	
		set 
		{	
			prefiltermap = value;
			SetTexture(@"PrefilterMap", prefiltermap, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase prefiltermap = null;





    private CameraTransform cameratransform = new CameraTransform();
	public CameraTransform CameraTransform
	{
		get { return cameratransform; }
		set 
		{ 
			cameratransform = value; 
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
		}
	}

	public OpenTK.Mathematics.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value; 
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"CameraTransform", ref value, 0 );
		}
	}
	public OpenTK.Mathematics.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value; 
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"CameraTransform", ref value, 64 );
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
}
}
namespace CubemapMaterial
{


public class CubemapMaterial : MaterialBase
{
	public CubemapMaterial() 
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

	public void SettexCubemap2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"texCubemap", TextureObject);
	}

	public void SettexCubemap2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"texCubemap", TextureObject, SamplerObject);
	}

	public TextureBase TexCubemap2D 
	{	
		set 
		{	
			texcubemap = value;
			SetTexture(@"texCubemap", texcubemap);
		}
	}

	public TextureBase TexCubemap2D_PointSample
	{	
		set 
		{	
			texcubemap = value;
			SetTexture(@"texCubemap", texcubemap, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase TexCubemap2D_LinearSample
	{	
		set 
		{	
			texcubemap = value;
			SetTexture(@"texCubemap", texcubemap, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase texcubemap = null;

	public OpenTK.Mathematics.Matrix4 ModelMatrix
	{
		get { return modelmatrix; }
		set 
		{
			modelmatrix = value;
			SetUniformVarData(@"ModelMatrix", modelmatrix);			
		}
	}
	private OpenTK.Mathematics.Matrix4 modelmatrix;
	public OpenTK.Mathematics.Matrix4 ProjMatrix
	{
		get { return projmatrix; }
		set 
		{
			projmatrix = value;
			SetUniformVarData(@"ProjMatrix", projmatrix);			
		}
	}
	private OpenTK.Mathematics.Matrix4 projmatrix;
	public OpenTK.Mathematics.Matrix4 ViewMatrix
	{
		get { return viewmatrix; }
		set 
		{
			viewmatrix = value;
			SetUniformVarData(@"ViewMatrix", viewmatrix);			
		}
	}
	private OpenTK.Mathematics.Matrix4 viewmatrix;

	
	public System.Int32 LightChannel
	{
		get { return lightchannel; }
		set 
		{
			lightchannel = value;
			SetUniformVarData(@"LightChannel", lightchannel);			
		}
	}
	private System.Int32 lightchannel;




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
}
}
namespace MSGBufferMaterial
{


public class MSGBufferMaterial : MaterialBase
{
	public MSGBufferMaterial() 
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



	
	public System.Int32 MaskMapExist
	{
		get { return maskmapexist; }
		set 
		{
			maskmapexist = value;
			SetUniformVarData(@"MaskMapExist", maskmapexist);			
		}
	}
	private System.Int32 maskmapexist;
	
	public System.Int32 NormalMapExist
	{
		get { return normalmapexist; }
		set 
		{
			normalmapexist = value;
			SetUniformVarData(@"NormalMapExist", normalmapexist);			
		}
	}
	private System.Int32 normalmapexist;
	
	public System.Int32 SpecularMapExist
	{
		get { return specularmapexist; }
		set 
		{
			specularmapexist = value;
			SetUniformVarData(@"SpecularMapExist", specularmapexist);			
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
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref value);
		}
	}

	public OpenTK.Mathematics.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"CameraTransform", ref value, 0 );
		}
	}
	public OpenTK.Mathematics.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"CameraTransform", ref value, 64 );
		}
	}

    private ModelTransform modeltransform = new ModelTransform();
	public ModelTransform ModelTransform
	{
		get { return modeltransform; }
		set 
		{ 
			modeltransform = value; 
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", ref value);
		}
	}

	public OpenTK.Mathematics.Matrix4 ModelTransform_Model
	{
		get { return modeltransform.Model ; }
		set 
		{ 
			modeltransform.Model = value;
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", ref modeltransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"ModelTransform", ref value, 0 );
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
}
}
namespace DepthVisualizeMaterial
{


public class DepthVisualizeMaterial : MaterialBase
{
	public DepthVisualizeMaterial() 
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

	public void SetDepthTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"DepthTex", TextureObject);
	}

	public void SetDepthTex2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"DepthTex", TextureObject, SamplerObject);
	}

	public TextureBase DepthTex2D 
	{	
		set 
		{	
			depthtex = value;
			SetTexture(@"DepthTex", depthtex);
		}
	}

	public TextureBase DepthTex2D_PointSample
	{	
		set 
		{	
			depthtex = value;
			SetTexture(@"DepthTex", depthtex, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase DepthTex2D_LinearSample
	{	
		set 
		{	
			depthtex = value;
			SetTexture(@"DepthTex", depthtex, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase depthtex = null;


	
	public System.Single Far
	{
		get { return far; }
		set 
		{
			far = value;
			SetUniformVarData(@"Far", far);			
		}
	}
	private System.Single far;
	
	public System.Single Near
	{
		get { return near; }
		set 
		{
			near = value;
			SetUniformVarData(@"Near", near);			
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
}
}
namespace FontRenderMaterial
{


public class FontRenderMaterial : MaterialBase
{
	public FontRenderMaterial() 
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

	public void SetFontTexture2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"FontTexture", TextureObject);
	}

	public void SetFontTexture2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"FontTexture", TextureObject, SamplerObject);
	}

	public TextureBase FontTexture2D 
	{	
		set 
		{	
			fonttexture = value;
			SetTexture(@"FontTexture", fonttexture);
		}
	}

	public TextureBase FontTexture2D_PointSample
	{	
		set 
		{	
			fonttexture = value;
			SetTexture(@"FontTexture", fonttexture, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase FontTexture2D_LinearSample
	{	
		set 
		{	
			fonttexture = value;
			SetTexture(@"FontTexture", fonttexture, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase fonttexture = null;

	public OpenTK.Mathematics.Vector2 ScreenSize
	{
		get { return screensize; }
		set 
		{
			screensize = value;
			SetUniformVarData(@"ScreenSize", screensize);			
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

uniform vec3 TextColor;
uniform sampler2D FontTexture;

out vec4 FragColor;

void main()
{   
	vec4 TexCol = texture(FontTexture, TexCoord);
    FragColor =vec4(1,0,0,TexCol.a);
}";
	}
}
}
namespace FontBoxRenderMaterial
{


public class FontBoxRenderMaterial : MaterialBase
{
	public FontBoxRenderMaterial() 
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


	public OpenTK.Mathematics.Vector2 ScreenSize
	{
		get { return screensize; }
		set 
		{
			screensize = value;
			SetUniformVarData(@"ScreenSize", screensize);			
		}
	}
	private OpenTK.Mathematics.Vector2 screensize;

	
	public System.Single BoxAlpha
	{
		get { return boxalpha; }
		set 
		{
			boxalpha = value;
			SetUniformVarData(@"BoxAlpha", boxalpha);			
		}
	}
	private System.Single boxalpha;
	
	public OpenTK.Mathematics.Vector3 BoxColor
	{
		get { return boxcolor; }
		set 
		{
			boxcolor = value;
			SetUniformVarData(@"BoxColor", boxcolor);			
		}
	}
	private OpenTK.Mathematics.Vector3 boxcolor;




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
}
}
namespace GridRenderMaterial
{


public class GridRenderMaterial : MaterialBase
{
	public GridRenderMaterial() 
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



	
	public OpenTK.Mathematics.Vector3 LineColor
	{
		get { return linecolor; }
		set 
		{
			linecolor = value;
			SetUniformVarData(@"LineColor", linecolor);			
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
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref value);
		}
	}

	public OpenTK.Mathematics.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"CameraTransform", ref value, 0 );
		}
	}
	public OpenTK.Mathematics.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"CameraTransform", ref value, 64 );
		}
	}

    private ModelTransform modeltransform = new ModelTransform();
	public ModelTransform ModelTransform
	{
		get { return modeltransform; }
		set 
		{ 
			modeltransform = value; 
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", ref value);
		}
	}

	public OpenTK.Mathematics.Matrix4 ModelTransform_Model
	{
		get { return modeltransform.Model ; }
		set 
		{ 
			modeltransform.Model = value;
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", ref modeltransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"ModelTransform", ref value, 0 );
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
}
}
namespace ThreeDTextRenderMaterial
{


public class ThreeDTextRenderMaterial : MaterialBase
{
	public ThreeDTextRenderMaterial() 
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

	public void SetFontTexture2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"FontTexture", TextureObject);
	}

	public void SetFontTexture2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"FontTexture", TextureObject, SamplerObject);
	}

	public TextureBase FontTexture2D 
	{	
		set 
		{	
			fonttexture = value;
			SetTexture(@"FontTexture", fonttexture);
		}
	}

	public TextureBase FontTexture2D_PointSample
	{	
		set 
		{	
			fonttexture = value;
			SetTexture(@"FontTexture", fonttexture, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase FontTexture2D_LinearSample
	{	
		set 
		{	
			fonttexture = value;
			SetTexture(@"FontTexture", fonttexture, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase fonttexture = null;

	public OpenTK.Mathematics.Matrix4 Model
	{
		get { return model; }
		set 
		{
			model = value;
			SetUniformVarData(@"Model", model);			
		}
	}
	private OpenTK.Mathematics.Matrix4 model;
	public OpenTK.Mathematics.Matrix4 Proj
	{
		get { return proj; }
		set 
		{
			proj = value;
			SetUniformVarData(@"Proj", proj);			
		}
	}
	private OpenTK.Mathematics.Matrix4 proj;
	public OpenTK.Mathematics.Matrix4 View
	{
		get { return view; }
		set 
		{
			view = value;
			SetUniformVarData(@"View", view);			
		}
	}
	private OpenTK.Mathematics.Matrix4 view;

	
	public OpenTK.Mathematics.Vector3 TextColor
	{
		get { return textcolor; }
		set 
		{
			textcolor = value;
			SetUniformVarData(@"TextColor", textcolor);			
		}
	}
	private OpenTK.Mathematics.Vector3 textcolor;




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
}
}
namespace ResolveMaterial
{


public class ResolveMaterial : MaterialBase
{
	public ResolveMaterial() 
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

	public void SetColorTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"ColorTex", TextureObject);
	}

	public void SetColorTex2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"ColorTex", TextureObject, SamplerObject);
	}

	public TextureBase ColorTex2D 
	{	
		set 
		{	
			colortex = value;
			SetTexture(@"ColorTex", colortex);
		}
	}

	public TextureBase ColorTex2D_PointSample
	{	
		set 
		{	
			colortex = value;
			SetTexture(@"ColorTex", colortex, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase ColorTex2D_LinearSample
	{	
		set 
		{	
			colortex = value;
			SetTexture(@"ColorTex", colortex, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase colortex = null;
	public void SetMotionTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"MotionTex", TextureObject);
	}

	public void SetMotionTex2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"MotionTex", TextureObject, SamplerObject);
	}

	public TextureBase MotionTex2D 
	{	
		set 
		{	
			motiontex = value;
			SetTexture(@"MotionTex", motiontex);
		}
	}

	public TextureBase MotionTex2D_PointSample
	{	
		set 
		{	
			motiontex = value;
			SetTexture(@"MotionTex", motiontex, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase MotionTex2D_LinearSample
	{	
		set 
		{	
			motiontex = value;
			SetTexture(@"MotionTex", motiontex, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase motiontex = null;






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
}
}
namespace SSAOMaterial
{


public class SSAOMaterial : MaterialBase
{
	public SSAOMaterial() 
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

	public void SetNormalTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"NormalTex", TextureObject);
	}

	public void SetNormalTex2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"NormalTex", TextureObject, SamplerObject);
	}

	public TextureBase NormalTex2D 
	{	
		set 
		{	
			normaltex = value;
			SetTexture(@"NormalTex", normaltex);
		}
	}

	public TextureBase NormalTex2D_PointSample
	{	
		set 
		{	
			normaltex = value;
			SetTexture(@"NormalTex", normaltex, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase NormalTex2D_LinearSample
	{	
		set 
		{	
			normaltex = value;
			SetTexture(@"NormalTex", normaltex, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase normaltex = null;
	public void SetPositionTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"PositionTex", TextureObject);
	}

	public void SetPositionTex2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"PositionTex", TextureObject, SamplerObject);
	}

	public TextureBase PositionTex2D 
	{	
		set 
		{	
			positiontex = value;
			SetTexture(@"PositionTex", positiontex);
		}
	}

	public TextureBase PositionTex2D_PointSample
	{	
		set 
		{	
			positiontex = value;
			SetTexture(@"PositionTex", positiontex, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase PositionTex2D_LinearSample
	{	
		set 
		{	
			positiontex = value;
			SetTexture(@"PositionTex", positiontex, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase positiontex = null;
	public void SetRandTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"RandTex", TextureObject);
	}

	public void SetRandTex2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"RandTex", TextureObject, SamplerObject);
	}

	public TextureBase RandTex2D 
	{	
		set 
		{	
			randtex = value;
			SetTexture(@"RandTex", randtex);
		}
	}

	public TextureBase RandTex2D_PointSample
	{	
		set 
		{	
			randtex = value;
			SetTexture(@"RandTex", randtex, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase RandTex2D_LinearSample
	{	
		set 
		{	
			randtex = value;
			SetTexture(@"RandTex", randtex, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase randtex = null;


	
	public OpenTK.Mathematics.Matrix4 ProjectionMatrix
	{
		get { return projectionmatrix; }
		set 
		{
			projectionmatrix = value;
			SetUniformVarData(@"ProjectionMatrix", projectionmatrix);			
		}
	}
	private OpenTK.Mathematics.Matrix4 projectionmatrix;
	
	public System.Single Radius
	{
		get { return radius; }
		set 
		{
			radius = value;
			SetUniformVarData(@"Radius", radius);			
		}
	}
	private System.Single radius;
	
	public OpenTK.Mathematics.Vector3[] SampleKernel
	{
		get { return samplekernel; }
		set 
		{
			samplekernel = value;
			SetUniformVarData(@"SampleKernel", ref samplekernel);			
		}
	}
	private OpenTK.Mathematics.Vector3[] samplekernel;
	
	public OpenTK.Mathematics.Vector2 ScreenSize
	{
		get { return screensize; }
		set 
		{
			screensize = value;
			SetUniformVarData(@"ScreenSize", screensize);			
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
}
}
namespace LUTGenerateMaterial
{


public class LUTGenerateMaterial : MaterialBase
{
	public LUTGenerateMaterial() 
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
}
}
namespace PrefilterMaterial
{


public class PrefilterMaterial : MaterialBase
{
	public PrefilterMaterial() 
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

	public void SetEnvironmentMap2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"EnvironmentMap", TextureObject);
	}

	public void SetEnvironmentMap2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"EnvironmentMap", TextureObject, SamplerObject);
	}

	public TextureBase EnvironmentMap2D 
	{	
		set 
		{	
			environmentmap = value;
			SetTexture(@"EnvironmentMap", environmentmap);
		}
	}

	public TextureBase EnvironmentMap2D_PointSample
	{	
		set 
		{	
			environmentmap = value;
			SetTexture(@"EnvironmentMap", environmentmap, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase EnvironmentMap2D_LinearSample
	{	
		set 
		{	
			environmentmap = value;
			SetTexture(@"EnvironmentMap", environmentmap, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase environmentmap = null;

	public OpenTK.Mathematics.Matrix4 Projection
	{
		get { return projection; }
		set 
		{
			projection = value;
			SetUniformVarData(@"Projection", projection);			
		}
	}
	private OpenTK.Mathematics.Matrix4 projection;
	public OpenTK.Mathematics.Matrix4 View
	{
		get { return view; }
		set 
		{
			view = value;
			SetUniformVarData(@"View", view);			
		}
	}
	private OpenTK.Mathematics.Matrix4 view;

	
	public System.Single Roughness
	{
		get { return roughness; }
		set 
		{
			roughness = value;
			SetUniformVarData(@"roughness", roughness);			
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
}
}
namespace HDAOMaterial
{


public class HDAOMaterial : MaterialBase
{
	public HDAOMaterial() 
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

	public void SetNormalMap2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"NormalMap", TextureObject);
	}

	public void SetNormalMap2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"NormalMap", TextureObject, SamplerObject);
	}

	public TextureBase NormalMap2D 
	{	
		set 
		{	
			normalmap = value;
			SetTexture(@"NormalMap", normalmap);
		}
	}

	public TextureBase NormalMap2D_PointSample
	{	
		set 
		{	
			normalmap = value;
			SetTexture(@"NormalMap", normalmap, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase NormalMap2D_LinearSample
	{	
		set 
		{	
			normalmap = value;
			SetTexture(@"NormalMap", normalmap, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase normalmap = null;
	public void SetPositionMap2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"PositionMap", TextureObject);
	}

	public void SetPositionMap2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"PositionMap", TextureObject, SamplerObject);
	}

	public TextureBase PositionMap2D 
	{	
		set 
		{	
			positionmap = value;
			SetTexture(@"PositionMap", positionmap);
		}
	}

	public TextureBase PositionMap2D_PointSample
	{	
		set 
		{	
			positionmap = value;
			SetTexture(@"PositionMap", positionmap, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase PositionMap2D_LinearSample
	{	
		set 
		{	
			positionmap = value;
			SetTexture(@"PositionMap", positionmap, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase positionmap = null;


	
	public System.Single AcceptAngle
	{
		get { return acceptangle; }
		set 
		{
			acceptangle = value;
			SetUniformVarData(@"AcceptAngle", acceptangle);			
		}
	}
	private System.Single acceptangle;
	
	public OpenTK.Mathematics.Vector4 HDAOAcceptRadius
	{
		get { return hdaoacceptradius; }
		set 
		{
			hdaoacceptradius = value;
			SetUniformVarData(@"HDAOAcceptRadius", hdaoacceptradius);			
		}
	}
	private OpenTK.Mathematics.Vector4 hdaoacceptradius;
	
	public System.Single HDAOIntensity
	{
		get { return hdaointensity; }
		set 
		{
			hdaointensity = value;
			SetUniformVarData(@"HDAOIntensity", hdaointensity);			
		}
	}
	private System.Single hdaointensity;
	
	public OpenTK.Mathematics.Vector4 HDAORejectRadius
	{
		get { return hdaorejectradius; }
		set 
		{
			hdaorejectradius = value;
			SetUniformVarData(@"HDAORejectRadius", hdaorejectradius);			
		}
	}
	private OpenTK.Mathematics.Vector4 hdaorejectradius;
	
	public System.Single NormalScale
	{
		get { return normalscale; }
		set 
		{
			normalscale = value;
			SetUniformVarData(@"NormalScale", normalscale);			
		}
	}
	private System.Single normalscale;
	
	public OpenTK.Mathematics.Vector2 RTSize
	{
		get { return rtsize; }
		set 
		{
			rtsize = value;
			SetUniformVarData(@"RTSize", rtsize);			
		}
	}
	private OpenTK.Mathematics.Vector2 rtsize;




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

layout (location = 0) in vec2 TexCoord;

layout (binding = 0) uniform sampler2D PositionMap;
layout (binding = 1) uniform sampler2D NormalMap;
uniform float FarClipDistance;
uniform vec2 RTSize;
uniform float NormalScale;
uniform vec2 InvFocalLen;
uniform float AcceptAngle;
uniform vec4 HDAORejectRadius;
uniform vec4 HDAOAcceptRadius;
uniform float HDAOIntensity;


// Gather defines
#define RING_1    (1)
#define RING_2    (2)
#define RING_3    (3)
#define RING_4    (4)
#define NUM_RING_1_GATHERS    (2)
#define NUM_RING_2_GATHERS    (6)
#define NUM_RING_3_GATHERS    (12)
#define NUM_RING_4_GATHERS    (20)

//const vec2 test[2] = vec2[2]( vec2(1,1), vec2(0,0) );

// Ring sample pattern
const vec2 g_f2HDAORingPattern[NUM_RING_4_GATHERS] = vec2[20]
(
    // Ring 1
    vec2( 1, -1 ),
    vec2( 0, 1 ),
    
    // Ring 2
    vec2( 0, 3 ),
	vec2( 2, 1 ),
    vec2( 3, -1 ),
    vec2( 1, -3 ),
        
    // Ring 3
    vec2( 1, -5 ),
    vec2( 3, -3 ),
    vec2( 5, -1 ),
    vec2( 4, 1 ),
    vec2( 2, 3 ),
    vec2( 0, 5 ),
    
    // Ring 4
    vec2( 0, 7 ),
    vec2( 2, 5 ),
    vec2( 4, 3 ),
    vec2( 6, 1 ),
    vec2( 7, -1 ),
    vec2( 5, -3 ),
    vec2( 3, -5 ),
    vec2( 1, -7 )
);

// Ring weights
const vec4 g_f4HDAORingWeight[NUM_RING_4_GATHERS] = vec4[20]
(
    // Ring 1 (Sum = 5.30864)
    vec4( 1.00000, 0.50000, 0.44721, 0.70711 ),
    vec4( 0.50000, 0.44721, 0.70711, 1.00000 ),
    
    // Ring 2 (Sum = 6.08746)
    vec4( 0.30000, 0.29104, 0.37947, 0.40000 ),
    vec4( 0.42426, 0.33282, 0.37947, 0.53666 ),
    vec4( 0.40000, 0.30000, 0.29104, 0.37947 ),
    vec4( 0.53666, 0.42426, 0.33282, 0.37947 ),
    
    // Ring 3 (Sum = 6.53067)
    vec4( 0.31530, 0.29069, 0.24140, 0.25495 ),
    vec4( 0.36056, 0.29069, 0.26000, 0.30641 ),
    vec4( 0.26000, 0.21667, 0.21372, 0.25495 ),
    vec4( 0.29069, 0.24140, 0.25495, 0.31530 ),
    vec4( 0.29069, 0.26000, 0.30641, 0.36056 ),
    vec4( 0.21667, 0.21372, 0.25495, 0.26000 ),
    
    // Ring 4 (Sum = 7.00962)
    vec4( 0.17500, 0.17365, 0.19799, 0.20000 ),
    vec4( 0.22136, 0.20870, 0.24010, 0.25997 ),
    vec4( 0.24749, 0.21864, 0.24010, 0.28000 ),
    vec4( 0.22136, 0.19230, 0.19799, 0.23016 ),
    vec4( 0.20000, 0.17500, 0.17365, 0.19799 ),
    vec4( 0.25997, 0.22136, 0.20870, 0.24010 ),
    vec4( 0.28000, 0.24749, 0.21864, 0.24010 ),
    vec4( 0.23016, 0.22136, 0.19230, 0.19799 )
);

const float g_fRingWeightsTotal[RING_4] = float[4](5.30864, 11.39610, 17.92677, 24.93639);

#define NUM_NORMAL_LOADS (4)
const ivec2 g_i2NormalLoadPattern[NUM_NORMAL_LOADS] = ivec2[4]
(
    ivec2( 1, 8 ),
	ivec2( 8, -1 ),
    ivec2( 5, 4 ),
    ivec2( 4, -4 )
);


ivec2 ClampToScreen(ivec2 i2ScreenCoord)
{           
    if( float(i2ScreenCoord.x) > (RTSize.x - 1.f)  &&  float(i2ScreenCoord.y) > (RTSize.y - 1.f) )
    {        
        return ivec2(RTSize - vec2(1,1));
    }
    else if( i2ScreenCoord.x < 0 && i2ScreenCoord.y < 0)
    {
        return ivec2(0,0);
    }
    else
    {
        return i2ScreenCoord;
    }
}

//----------------------------------------------------------------------------------------
// Helper function to Gather samples 
//----------------------------------------------------------------------------------------
vec4 GatherSamples( sampler2D Tex, vec2 f2TexCoord, int iComponent)
{
    vec4 f4Ret;    
    
    f4Ret = textureGather(Tex , f2TexCoord, iComponent);
        
    return f4Ret;
}

vec4 GatherSamples2(sampler2D Tex, vec2 f2TexCoord, int iComponent)
{
	vec4 f4Ret;    
    
	if(iComponent == 0)
    {
		f4Ret.x = textureOffset(Tex, f2TexCoord, ivec2(0,1)).x;
		f4Ret.y = textureOffset(Tex, f2TexCoord, ivec2(1,1)).x;
		f4Ret.z = textureOffset(Tex, f2TexCoord, ivec2(1,0)).x;
		f4Ret.w = textureOffset(Tex, f2TexCoord, ivec2(0,0)).x;
	}
	else if(iComponent == 1)
    {
		f4Ret.x = textureOffset(Tex, f2TexCoord, ivec2(0,1)).y;
		f4Ret.y = textureOffset(Tex, f2TexCoord, ivec2(1,1)).y;
		f4Ret.z = textureOffset(Tex, f2TexCoord, ivec2(1,0)).y;
		f4Ret.w = textureOffset(Tex, f2TexCoord, ivec2(0,0)).y;
	}
	else if(iComponent == 2)
    {
		f4Ret.x = textureOffset(Tex, f2TexCoord, ivec2(0,1)).z;
		f4Ret.y = textureOffset(Tex, f2TexCoord, ivec2(1,1)).z;
		f4Ret.z = textureOffset(Tex, f2TexCoord, ivec2(1,0)).z;
		f4Ret.w = textureOffset(Tex, f2TexCoord, ivec2(0,0)).z;
	}
	else if(iComponent == 3)
    {
		f4Ret.x = textureOffset(Tex, f2TexCoord, ivec2(0,1)).w;
		f4Ret.y = textureOffset(Tex, f2TexCoord, ivec2(1,1)).w;
		f4Ret.z = textureOffset(Tex, f2TexCoord, ivec2(1,0)).w;
		f4Ret.w = textureOffset(Tex, f2TexCoord, ivec2(0,0)).w;
	}
        
    return f4Ret;
}


//----------------------------------------------------------------------------------
vec3 uv_to_eye(vec2 uv, float eye_z)
{
    uv = (uv * vec2(2.0, 2.0) - vec2(1.0, 1.0));
    return vec3(uv * InvFocalLen * eye_z, eye_z);
}

//----------------------------------------------------------------------------------
vec3 GetCameraXYZ(ivec2 uv)
{   	
    // float fDepth = texelFetch(NormalMap, uv, 0).a;	    
    float fDepth = abs(texelFetch(PositionMap, uv, 0).z);
    return uv_to_eye(vec2(uv / RTSize), abs(fDepth));
}

vec3 GetCameraXYZFloat(vec2 uv)
{
	//float fDepth = texture(NormalMap, uv, 0).a;	    
    float fDepth = abs(texture(PositionMap, uv, 0).z);
    return uv_to_eye(uv, abs(fDepth));
}



//--------------------------------------------------------------------------------------
// Used as an early rejection test - based on geometry
//--------------------------------------------------------------------------------------
float GeometryRejectionTest( ivec2 i2ScreenCoord)
{
    vec3 f3N[3];
    vec3 f3Pos[3];
    vec3 f3Dir[2];
    float fDot;
    float fSummedDot = 0.0f;
    ivec2 i2MirrorPattern;
    ivec2 i2OffsetScreenCoord;
    ivec2 i2MirrorOffsetScreenCoord;
    float fDepth;
    
    //fDepth = g_txDepth.Load( ivec3( i2ScreenCoord, 0 ) ).x;
    //f3Pos[0] = GetCameraXYZFromDepth( fDepth, i2ScreenCoord );
    
    
    f3Pos[0] = GetCameraXYZ( i2ScreenCoord );
    f3N[0] = texelFetch(NormalMap, i2ScreenCoord, 0).xyz;   
    f3Pos[0] -= ( f3N[0] * NormalScale );

    for( int iNormal=0; iNormal < NUM_NORMAL_LOADS; iNormal++ )
    {
        i2MirrorPattern = ( g_i2NormalLoadPattern[iNormal] + ivec2( 1, 1 ) ) * ivec2( -1, -1 );
        i2OffsetScreenCoord = i2ScreenCoord + g_i2NormalLoadPattern[iNormal];
        i2MirrorOffsetScreenCoord = i2ScreenCoord + i2MirrorPattern;
        
        // Clamp our test to screen coordinates

        i2OffsetScreenCoord = ClampToScreen(i2OffsetScreenCoord);
        i2MirrorOffsetScreenCoord = ClampToScreen(i2MirrorOffsetScreenCoord);        

        //fDepth = g_txDepth.Load( ivec3( i2OffsetScreenCoord, 0 ) ).x;
        
        // fDepth = texelFetch( NormalMap, i2OffsetScreenCoord, 0 ).a;        
        fDepth = abs(texelFetch( PositionMap, i2OffsetScreenCoord, 0 ).z);
        f3Pos[1] = GetCameraXYZ( i2OffsetScreenCoord );

        //fDepth = g_txDepth.Load( ivec3( i2MirrorOffsetScreenCoord, 0 ) ).x;

        //fDepth = texelFetch( NormalMap, i2MirrorOffsetScreenCoord, 0 ).a;
        fDepth = abs(texelFetch( PositionMap, i2MirrorOffsetScreenCoord, 0 ).z);

        f3Pos[2] = GetCameraXYZ( i2MirrorOffsetScreenCoord );
        
        f3N[1] = texelFetch(NormalMap, i2OffsetScreenCoord, 0).xyz;
        f3N[2] = texelFetch(NormalMap, i2MirrorOffsetScreenCoord, 0).xyz;
                
        f3Pos[1] -= ( f3N[1] * NormalScale );
        f3Pos[2] -= ( f3N[2] * NormalScale );
        
        f3Dir[0] = f3Pos[1] - f3Pos[0];
        f3Dir[1] = f3Pos[2] - f3Pos[0];
        
        f3Dir[0] = normalize( f3Dir[0] );
        f3Dir[1] = normalize( f3Dir[1] );
        
        fDot = dot( f3Dir[0], f3Dir[1] );
        
        fSummedDot += ( fDot + 2.0f );
    }
        
    return ( fSummedDot * 0.125f );
}


//--------------------------------------------------------------------------------------
// Used as an early rejection test - based on normals
//--------------------------------------------------------------------------------------
float NormalRejectionTest( ivec2 i2ScreenCoord, bool b10_1 )
{
    vec3 f3N1;
    vec3 f3N2;
    float fDot = 0.f;
    float fSummedDot = 0.0f;
    ivec2 i2MirrorPattern;
    ivec2 i2OffsetScreenCoord;
    ivec2 i2MirrorOffsetScreenCoord;

    for( int iNormal=0; iNormal<NUM_NORMAL_LOADS; iNormal++ )
    {
        i2MirrorPattern = ( g_i2NormalLoadPattern[iNormal] + ivec2( 1, 1 ) ) * ivec2( -1, -1 );
        i2OffsetScreenCoord = i2ScreenCoord + g_i2NormalLoadPattern[iNormal];
        i2MirrorOffsetScreenCoord = i2ScreenCoord + i2MirrorPattern;
        
        // Clamp our test to screen coordinates
        i2OffsetScreenCoord = ClampToScreen(i2OffsetScreenCoord);
        i2MirrorOffsetScreenCoord = ClampToScreen(i2MirrorOffsetScreenCoord);                   
                        
        f3N1 = texelFetch(NormalMap, i2OffsetScreenCoord, 0 ).xyz;
        f3N2 = texelFetch(NormalMap, i2MirrorOffsetScreenCoord, 0).xyz;                 
        
        fDot = dot( f3N1, f3N2 );
        
        fSummedDot += ( fDot > AcceptAngle ) ? ( 0.0f ) : ( 1.0f - ( abs( fDot ) * 0.25f ) );
    }
        
    return ( 0.5f + fSummedDot * 0.25f  );
}

vec4 GatherZSamples( sampler2D Tex, vec2 f2TexCoord)
{
    vec4 f4Ret;
    vec4 f4Gather;
    
    // depth is recorded in alpha channel    
    f4Gather = GatherSamples(Tex, f2TexCoord, 3);
    
    return f4Gather;
}


vec4 ComponentwiseCompareGreater(vec4 A, vec4 B)
{
	vec4 vResult;

	vResult.x = A.x > B.x ? 1.f : 0.0f;
	vResult.y = A.y > B.y ? 1.f : 0.0f;
	vResult.z = A.z > B.z ? 1.f : 0.0f;
	vResult.w = A.w > B.w ? 1.f : 0.0f;

	return vResult;
}

vec4 ComponentwiseCompareSmaller(vec4 A, vec4 B)
{
	vec4 vResult;

	vResult.x = A.x < B.x ? 1.f : 0.0f;
	vResult.y = A.y < B.y ? 1.f : 0.0f;
	vResult.z = A.z < B.z ? 1.f : 0.0f;
	vResult.w = A.w < B.w ? 1.f : 0.0f;

	return vResult;
}

layout( location = 0 ) out vec4 FragColor;

void main() 
{   
     // Locals
    ivec2 i2ScreenCoord;
    vec2 f2ScreenCoord_10_1;
    vec2 f2ScreenCoord;
    vec2 f2MirrorScreenCoord;
    vec2 f2TexCoord_10_1;
    vec2 f2MirrorTexCoord_10_1;
    vec2 f2TexCoord;
    vec2 f2MirrorTexCoord;
    vec2 f2InvRTSize;
    float fCenterZ;
    float fOffsetCenterZ;
    float fCenterNormalZ;
    vec4 f4SampledZ[2];
    vec4 f4OffsetSampledZ[2];
    vec4 f4SampledNormalZ[2];
    vec4 f4Diff;
    vec4 f4Compare[2];
    vec4 f4Occlusion = vec4(0.0f);
    float fOcclusion;
    int iGather;
    float fDot = 1.0f;
    vec2 f2KernelScale = vec2( RTSize.x / 1200.0f, RTSize.y / 1200.0f );
    vec3 f3CameraPos;
    bool bUseNormals = true;
    bool b10_1 = false;
	const int iNumRings = RING_3;
	const int iNumRingGathers = NUM_RING_3_GATHERS;
                            
    // Compute integer screen coord, and store off the inverse of the RT Size
    f2InvRTSize = 1.0f / RTSize;
    f2ScreenCoord = TexCoord * RTSize;
	i2ScreenCoord = ivec2( f2ScreenCoord );	

    // Test the normals to see if we should apply occlusion
    fDot = NormalRejectionTest( i2ScreenCoord, b10_1 );       
    
	
    if( fDot > 0.5f )
    {   
        // Sample the center pixel for camera Z
        if( b10_1 )
        {
            // For Gather we need to snap the screen coords
            f2ScreenCoord_10_1 = vec2( i2ScreenCoord );
            f2TexCoord = vec2( f2ScreenCoord_10_1 * f2InvRTSize );
        }
        else
        {
            f2TexCoord = vec2( f2ScreenCoord * f2InvRTSize );
        }

        // float fDepth = texture( NormalMap, f2TexCoord ).a;        
        float fDepth = abs(texture( PositionMap, f2TexCoord ).z);        
        fCenterZ = fDepth;
        
        if( bUseNormals )
        {
            if( b10_1 )
            {
                //fCenterNormalZ = g_txNormalsZ.SampleLevel( g_SamplePoint, f2TexCoord, 0 ).x;
                fCenterNormalZ = texture(NormalMap, f2TexCoord).z;
            }
            else
            {
                //fCenterNormalZ = g_txNormals.SampleLevel( g_SamplePoint, f2TexCoord, 0 ).x;
                fCenterNormalZ = texture(NormalMap, f2TexCoord).z;
            }
            fOffsetCenterZ = fCenterZ + fCenterNormalZ * NormalScale;
        }
            
        // Loop through each gather location, and compare with its mirrored location
        for( iGather=0; iGather < iNumRingGathers; iGather++ )
        {
            f2MirrorScreenCoord = ( ( f2KernelScale * g_f2HDAORingPattern[iGather] ) + vec2( 1.0f, 1.0f ) ) * vec2( -1.0f, -1.0f );
            
            f2TexCoord = vec2( ( f2ScreenCoord + ( f2KernelScale * g_f2HDAORingPattern[iGather] ) ) * f2InvRTSize );
            f2MirrorTexCoord = vec2( ( f2ScreenCoord + ( f2MirrorScreenCoord ) ) * f2InvRTSize );
            
            // Sample
            if( b10_1 )
            {
                f2TexCoord_10_1 = vec2( ( f2ScreenCoord_10_1 + ( ( f2KernelScale * g_f2HDAORingPattern[iGather] ) + vec2( 1.0f, 1.0f ) ) ) * f2InvRTSize );
                f2MirrorTexCoord_10_1 = vec2( ( f2ScreenCoord_10_1 + ( f2MirrorScreenCoord + vec2( 1.0f, 1.0f ) ) ) * f2InvRTSize );
                
                f4SampledZ[0] = GatherZSamples( NormalMap, f2TexCoord_10_1 );
                f4SampledZ[1] = GatherZSamples( NormalMap, f2MirrorTexCoord_10_1 );
            }
            else
            {
                f4SampledZ[0] = GatherZSamples( NormalMap, f2TexCoord );
                f4SampledZ[1] = GatherZSamples( NormalMap, f2MirrorTexCoord );
            }
                        
            // Detect valleys
            f4Diff = vec4(fCenterZ) - f4SampledZ[0];   
            
            f4Compare[0] = ComponentwiseCompareSmaller(f4Diff, HDAORejectRadius);
            f4Compare[0] *= ComponentwiseCompareGreater(f4Diff, HDAOAcceptRadius);            

            //f4Compare[0] = ( f4Diff < HDAORejectRadius.xxxx ) ? ( 1.0f ) : ( 0.0f );
            //f4Compare[0] *= ( f4Diff > HDAOAcceptRadius.xxxx ) ? ( 1.0f ) : ( 0.0f );
            
            f4Diff = vec4(fCenterZ) - f4SampledZ[1];

            //f4Compare[1] = ( f4Diff < HDAORejectRadius.xxxx ) ? ( 1.0f ) : ( 0.0f );
            //f4Compare[1] *= ( f4Diff > HDAOAcceptRadius.xxxx ) ? ( 1.0f ) : ( 0.0f );

			f4Compare[1] = ComponentwiseCompareSmaller(f4Diff, HDAORejectRadius);
            f4Compare[1] *= ComponentwiseCompareGreater(f4Diff, HDAOAcceptRadius);			            
            
            f4Occlusion.xyzw += ( g_f4HDAORingWeight[iGather].xyzw * ( f4Compare[0].xyzw * f4Compare[1].zwxy ) * fDot );			
                
            if( bUseNormals )
            {
                // Sample normals
                if( b10_1 )
                {        
                    f4SampledNormalZ[0] = GatherSamples( NormalMap, f2TexCoord_10_1, 2 );
                    f4SampledNormalZ[1] = GatherSamples( NormalMap, f2MirrorTexCoord_10_1, 2 );
                }
                else
                {
                    f4SampledNormalZ[0] = GatherSamples( NormalMap, f2TexCoord, 2 );
                    f4SampledNormalZ[1] = GatherSamples( NormalMap, f2MirrorTexCoord, 2 );
                }
                    
                // Scale normals
                f4OffsetSampledZ[0] = f4SampledZ[0] + ( f4SampledNormalZ[0] * NormalScale );
                f4OffsetSampledZ[1] = f4SampledZ[1] + ( f4SampledNormalZ[1] * NormalScale );
                            
                // Detect valleys
                f4Diff = vec4(fOffsetCenterZ) - f4OffsetSampledZ[0];

                f4Compare[0] = ComponentwiseCompareSmaller( f4Diff , HDAORejectRadius);
                f4Compare[0] *= ComponentwiseCompareGreater( f4Diff , HDAOAcceptRadius);
                
                f4Diff = vec4(fOffsetCenterZ) - f4OffsetSampledZ[1];

                f4Compare[1] = ComponentwiseCompareSmaller( f4Diff , HDAORejectRadius ) ;
                f4Compare[1] *= ComponentwiseCompareGreater( f4Diff , HDAOAcceptRadius );
                
                f4Occlusion.xyzw += ( g_f4HDAORingWeight[iGather].xyzw * ( f4Compare[0].xyzw * f4Compare[1].zwxy ) * fDot );    
            }
        }
    }	
                    
    // Finally calculate the HDAO occlusion value
    if( bUseNormals )
    {
        fOcclusion = ( ( f4Occlusion.x + f4Occlusion.y + f4Occlusion.z + f4Occlusion.w ) / ( 3.0f * g_fRingWeightsTotal[iNumRings - 1] ) );
    }
    else
    {
        fOcclusion = ( ( f4Occlusion.x + f4Occlusion.y + f4Occlusion.z + f4Occlusion.w ) / ( 2.0f * g_fRingWeightsTotal[iNumRings - 1] ) );
    }

    fOcclusion *= ( HDAOIntensity );	

    fOcclusion = 1.0f - clamp( fOcclusion , 0.0, 1.f);    
    
    FragColor = vec4(fOcclusion);
}
";
	}
}
}
namespace FXAAMaterial
{


public class FXAAMaterial : MaterialBase
{
	public FXAAMaterial() 
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

	public void SetScreenTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"ScreenTex", TextureObject);
	}

	public void SetScreenTex2D(Core.Texture.TextureBase TextureObject, Sampler SamplerObject)
	{
		SetTexture(@"ScreenTex", TextureObject, SamplerObject);
	}

	public TextureBase ScreenTex2D 
	{	
		set 
		{	
			screentex = value;
			SetTexture(@"ScreenTex", screentex);
		}
	}

	public TextureBase ScreenTex2D_PointSample
	{	
		set 
		{	
			screentex = value;
			SetTexture(@"ScreenTex", screentex, Sampler.DefaultPointSampler);
		}
	}

	public TextureBase ScreenTex2D_LinearSample
	{	
		set 
		{	
			screentex = value;
			SetTexture(@"ScreenTex", screentex, Sampler.DefaultLinearSampler);
		}
	}

	private TextureBase screentex = null;


	
	public OpenTK.Mathematics.Vector2 InverseScreenSize
	{
		get { return inversescreensize; }
		set 
		{
			inversescreensize = value;
			SetUniformVarData(@"InverseScreenSize", inversescreensize);			
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
}
}
namespace TBNMaterial
{


public class TBNMaterial : MaterialBase
{
	public TBNMaterial() 
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





    private CameraTransform cameratransform = new CameraTransform();
	public CameraTransform CameraTransform
	{
		get { return cameratransform; }
		set 
		{ 
			cameratransform = value; 
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref value);
		}
	}

	public OpenTK.Mathematics.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"CameraTransform", ref value, 0 );
		}
	}
	public OpenTK.Mathematics.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"CameraTransform", ref value, 64 );
		}
	}

    private ModelTransform modeltransform = new ModelTransform();
	public ModelTransform ModelTransform
	{
		get { return modeltransform; }
		set 
		{ 
			modeltransform = value; 
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", ref value);
		}
	}

	public OpenTK.Mathematics.Matrix4 ModelTransform_Model
	{
		get { return modeltransform.Model ; }
		set 
		{ 
			modeltransform.Model = value;
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", ref modeltransform);
			//this.SetUniformBufferMemberValue< OpenTK.Mathematics.Matrix4 >(@"ModelTransform", ref value, 0 );
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
}
}

}
