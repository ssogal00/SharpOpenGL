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
namespace SharpOpenGL
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

	public OpenTK.Vector3 ColorBlock_Value
	{
		get { return colorblock.Value ; }
		set 
		{ 
			colorblock.Value = value;
			this.SetUniformBufferValue< ColorBlock >(@"ColorBlock", ref colorblock);
			//this.SetUniformBufferMemberValue< OpenTK.Vector3 >(@"ColorBlock", ref value, 0 );
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

	public OpenTK.Matrix4 Transform_Model
	{
		get { return transform.Model ; }
		set 
		{ 
			transform.Model = value;
			this.SetUniformBufferValue< Transform >(@"Transform", ref transform);
			//this.SetUniformBufferMemberValue< OpenTK.Matrix4 >(@"Transform", ref value, 0 );
		}
	}
	public OpenTK.Matrix4 Transform_View
	{
		get { return transform.View ; }
		set 
		{ 
			transform.View = value;
			this.SetUniformBufferValue< Transform >(@"Transform", ref transform);
			//this.SetUniformBufferMemberValue< OpenTK.Matrix4 >(@"Transform", ref value, 64 );
		}
	}
	public OpenTK.Matrix4 Transform_Proj
	{
		get { return transform.Proj ; }
		set 
		{ 
			transform.Proj = value;
			this.SetUniformBufferValue< Transform >(@"Transform", ref transform);
			//this.SetUniformBufferMemberValue< OpenTK.Matrix4 >(@"Transform", ref value, 128 );
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

	public OpenTK.Matrix4 Transform_Model
	{
		get { return transform.Model ; }
		set 
		{ 
			transform.Model = value;
			this.SetUniformBufferValue< Transform >(@"Transform", ref transform);
			//this.SetUniformBufferMemberValue< OpenTK.Matrix4 >(@"Transform", ref value, 0 );
		}
	}
	public OpenTK.Matrix4 Transform_View
	{
		get { return transform.View ; }
		set 
		{ 
			transform.View = value;
			this.SetUniformBufferValue< Transform >(@"Transform", ref transform);
			//this.SetUniformBufferMemberValue< OpenTK.Matrix4 >(@"Transform", ref value, 64 );
		}
	}
	public OpenTK.Matrix4 Transform_Proj
	{
		get { return transform.Proj ; }
		set 
		{ 
			transform.Proj = value;
			this.SetUniformBufferValue< Transform >(@"Transform", ref transform);
			//this.SetUniformBufferMemberValue< OpenTK.Matrix4 >(@"Transform", ref value, 128 );
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

	public void SetColorTex2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"ColorTex", TextureObject);
	}

	public TextureBase ColorTex2D 
	{	
		get { return colortex;}
		set 
		{	
			colortex = value;
			SetTexture(@"ColorTex", colortex);			
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
	gl_Position = vec4(VertexPosition.xyz, 1.0);
}";
	}

	public static string GetFSSourceCode()
	{
		return @"
#version 450 core

in vec2 OutTexCoord;

uniform sampler2D ColorTex;

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

	public void SetDiffuseTex2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"DiffuseTex", TextureObject);
	}

	public TextureBase DiffuseTex2D 
	{	
		get { return diffusetex;}
		set 
		{	
			diffusetex = value;
			SetTexture(@"DiffuseTex", diffusetex);			
		}
	}

	private TextureBase diffusetex = null;
	public void SetMaskTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"MaskTex", TextureObject);
	}

	public void SetMaskTex2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"MaskTex", TextureObject);
	}

	public TextureBase MaskTex2D 
	{	
		get { return masktex;}
		set 
		{	
			masktex = value;
			SetTexture(@"MaskTex", masktex);			
		}
	}

	private TextureBase masktex = null;
	public void SetMetalicTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"MetalicTex", TextureObject);
	}

	public void SetMetalicTex2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"MetalicTex", TextureObject);
	}

	public TextureBase MetalicTex2D 
	{	
		get { return metalictex;}
		set 
		{	
			metalictex = value;
			SetTexture(@"MetalicTex", metalictex);			
		}
	}

	private TextureBase metalictex = null;
	public void SetNormalTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"NormalTex", TextureObject);
	}

	public void SetNormalTex2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"NormalTex", TextureObject);
	}

	public TextureBase NormalTex2D 
	{	
		get { return normaltex;}
		set 
		{	
			normaltex = value;
			SetTexture(@"NormalTex", normaltex);			
		}
	}

	private TextureBase normaltex = null;
	public void SetRoughnessTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"RoughnessTex", TextureObject);
	}

	public void SetRoughnessTex2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"RoughnessTex", TextureObject);
	}

	public TextureBase RoughnessTex2D 
	{	
		get { return roughnesstex;}
		set 
		{	
			roughnesstex = value;
			SetTexture(@"RoughnessTex", roughnesstex);			
		}
	}

	private TextureBase roughnesstex = null;


	
	public System.Int32 DiffuseMapExist
	{
		get { return diffusemapexist; }
		set 
		{
			diffusemapexist = value;
			SetUniformVarData(@"DiffuseMapExist", diffusemapexist);			
		}
	}
	private System.Int32 diffusemapexist;
	
	public OpenTK.Vector3 DiffuseOverride
	{
		get { return diffuseoverride; }
		set 
		{
			diffuseoverride = value;
			SetUniformVarData(@"DiffuseOverride", diffuseoverride);			
		}
	}
	private OpenTK.Vector3 diffuseoverride;
	
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
	
	public System.Int32 MetalicExist
	{
		get { return metalicexist; }
		set 
		{
			metalicexist = value;
			SetUniformVarData(@"MetalicExist", metalicexist);			
		}
	}
	private System.Int32 metalicexist;
	
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
	
	public System.Int32 RoughnessExist
	{
		get { return roughnessexist; }
		set 
		{
			roughnessexist = value;
			SetUniformVarData(@"RoughnessExist", roughnessexist);			
		}
	}
	private System.Int32 roughnessexist;


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

	public OpenTK.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Matrix4 >(@"CameraTransform", ref value, 0 );
		}
	}
	public OpenTK.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Matrix4 >(@"CameraTransform", ref value, 64 );
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

	public OpenTK.Matrix4 ModelTransform_Model
	{
		get { return modeltransform.Model ; }
		set 
		{ 
			modeltransform.Model = value;
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", ref modeltransform);
			//this.SetUniformBufferMemberValue< OpenTK.Matrix4 >(@"ModelTransform", ref value, 0 );
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

layout (location = 0, binding=0) uniform sampler2D DiffuseTex;
layout (location = 1, binding=1) uniform sampler2D NormalTex;
layout (location = 2, binding=2) uniform sampler2D MaskTex;
layout (location = 3, binding=3) uniform sampler2D MetalicTex;
layout (location = 4, binding=4) uniform sampler2D RoughnessTex;

uniform int MetalicExist;
uniform int MaskMapExist;
uniform int NormalMapExist;
uniform int RoughnessExist;
uniform int DiffuseMapExist;

uniform float Metalic = 0;
uniform float Roughness = 0;
uniform vec3 DiffuseOverride;

void main()
{   
    if(MaskMapExist > 0)
    {
    	vec4 MaskValue= texture(MaskTex, InTexCoord);
    	if(MaskValue.x > 0)
    	{
    		DiffuseColor = texture(DiffuseTex, InTexCoord);           
            //DiffuseColor.xyz = pow(DiffuseColor.xyz, vec3(1.0/2.2));  
    	}
    	else
    	{
    		discard;
    	}
    }
    else
    {
        if(DiffuseMapExist > 0)
    	{
            DiffuseColor = texture(DiffuseTex, InTexCoord);
            //DiffuseColor.xyz = pow(DiffuseColor.xyz, vec3(1.0/2.2));  
        }
        else
        {
            DiffuseColor = vec4(DiffuseOverride,0);
           // DiffuseColor.xyz = pow(DiffuseColor.xyz, vec3(1.0/2.2));  
        }
    }

    if(RoughnessExist > 0)
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

    if(NormalMapExist > 0)
    {
        vec3 NormalMapNormal = (2.0f * (texture( NormalTex, InTexCoord ).xyz) - vec3(1.0f));
	    vec3 BumpNormal = normalize(TangentToModelViewSpaceMatrix * NormalMapNormal.xyz);
	
        NormalColor.xyz = BumpNormal.xyz;
    }
    else
    {
        NormalColor.xyz = InNormal.xyz;
    }

    if(MetalicExist > 0)
    {
        NormalColor.a = texture(MetalicTex, InTexCoord).x;        
    }
    else
    {
        NormalColor.a = Metalic;
    }

    PositionColor = InPosition;
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

	public OpenTK.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Matrix4 >(@"CameraTransform", ref value, 0 );
		}
	}
	public OpenTK.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Matrix4 >(@"CameraTransform", ref value, 64 );
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

	public OpenTK.Matrix4 ModelTransform_Model
	{
		get { return modeltransform.Model ; }
		set 
		{ 
			modeltransform.Model = value;
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", ref modeltransform);
			//this.SetUniformBufferMemberValue< OpenTK.Matrix4 >(@"ModelTransform", ref value, 0 );
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

	public void SetDiffuseTex2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"DiffuseTex", TextureObject);
	}

	public TextureBase DiffuseTex2D 
	{	
		get { return diffusetex;}
		set 
		{	
			diffusetex = value;
			SetTexture(@"DiffuseTex", diffusetex);			
		}
	}

	private TextureBase diffusetex = null;
	public void SetNormalTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"NormalTex", TextureObject);
	}

	public void SetNormalTex2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"NormalTex", TextureObject);
	}

	public TextureBase NormalTex2D 
	{	
		get { return normaltex;}
		set 
		{	
			normaltex = value;
			SetTexture(@"NormalTex", normaltex);			
		}
	}

	private TextureBase normaltex = null;
	public void SetPositionTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"PositionTex", TextureObject);
	}

	public void SetPositionTex2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"PositionTex", TextureObject);
	}

	public TextureBase PositionTex2D 
	{	
		get { return positiontex;}
		set 
		{	
			positiontex = value;
			SetTexture(@"PositionTex", positiontex);			
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

	public System.Int32 Dump_PositionDump
	{
		get { return dump.PositionDump ; }
		set 
		{ 
			dump.PositionDump = value; 
			this.SetUniformBufferValue< Dump >(@"Dump", ref dump);
			//this.SetUniformBufferMemberValue< System.Int32 >(@"Dump", ref value, 0 );
		}
	}
	public System.Int32 Dump_NormalDump
	{
		get { return dump.NormalDump ; }
		set 
		{ 
			dump.NormalDump = value; 
			this.SetUniformBufferValue< Dump >(@"Dump", ref dump);
			//this.SetUniformBufferMemberValue< System.Int32 >(@"Dump", ref value, 4 );
		}
	}
	public System.Int32 Dump_MetalicDump
	{
		get { return dump.MetalicDump ; }
		set 
		{ 
			dump.MetalicDump = value; 
			this.SetUniformBufferValue< Dump >(@"Dump", ref dump);
			//this.SetUniformBufferMemberValue< System.Int32 >(@"Dump", ref value, 8 );
		}
	}
	public System.Int32 Dump_DiffuseDump
	{
		get { return dump.DiffuseDump ; }
		set 
		{ 
			dump.DiffuseDump = value; 
			this.SetUniformBufferValue< Dump >(@"Dump", ref dump);
			//this.SetUniformBufferMemberValue< System.Int32 >(@"Dump", ref value, 12 );
		}
	}
	public System.Int32 Dump_RoughnessDump
	{
		get { return dump.RoughnessDump ; }
		set 
		{ 
			dump.RoughnessDump = value; 
			this.SetUniformBufferValue< Dump >(@"Dump", ref dump);
			//this.SetUniformBufferMemberValue< System.Int32 >(@"Dump", ref value, 16 );
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


layout (location = 0 ) in vec2 InTexCoord;

layout( location = 0 ) out vec4 FragColor;

uniform Dump
{
    int PositionDump;
    int NormalDump;
    int MetalicDump;
    int DiffuseDump;
    int RoughnessDump;
};

void main() 
{
    if(PositionDump > 0) 
    {   
        FragColor = texture(PositionTex, InTexCoord);
    }
    else if(NormalDump > 0)
    {
        FragColor = texture(NormalTex, InTexCoord);
    }
    else if(DiffuseDump > 0)
    {
        FragColor = texture(DiffuseTex, InTexCoord);
    }
    else if(MetalicDump > 0)
    {
        FragColor = texture(NormalTex,InTexCoord).aaaa;
    }
    else if(RoughnessDump > 0)
    {
   		FragColor = texture(DiffuseTex, InTexCoord).aaaa;
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


	public OpenTK.Matrix4 Model
	{
		get { return model; }
		set 
		{
			model = value;
			SetUniformVarData(@"Model", model);			
		}
	}
	private OpenTK.Matrix4 model;



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

	public OpenTK.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Matrix4 >(@"CameraTransform", ref value, 0 );
		}
	}
	public OpenTK.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Matrix4 >(@"CameraTransform", ref value, 64 );
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

	public void SetSpecularTex2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"SpecularTex", TextureObject);
	}

	public TextureBase SpecularTex2D 
	{	
		get { return speculartex;}
		set 
		{	
			speculartex = value;
			SetTexture(@"SpecularTex", speculartex);			
		}
	}

	private TextureBase speculartex = null;

	public OpenTK.Matrix4 Model
	{
		get { return model; }
		set 
		{
			model = value;
			SetUniformVarData(@"Model", model);			
		}
	}
	private OpenTK.Matrix4 model;



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

	public OpenTK.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Matrix4 >(@"CameraTransform", ref value, 0 );
		}
	}
	public OpenTK.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Matrix4 >(@"CameraTransform", ref value, 64 );
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

	public void SetColorTex2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"ColorTex", TextureObject);
	}

	public TextureBase ColorTex2D 
	{	
		get { return colortex;}
		set 
		{	
			colortex = value;
			SetTexture(@"ColorTex", colortex);			
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

	public void SetColorTex2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"ColorTex", TextureObject);
	}

	public TextureBase ColorTex2D 
	{	
		get { return colortex;}
		set 
		{	
			colortex = value;
			SetTexture(@"ColorTex", colortex);			
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

	public void SetDiffuseTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"DiffuseTex", TextureObject);
	}

	public void SetDiffuseTex2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"DiffuseTex", TextureObject);
	}

	public TextureBase DiffuseTex2D 
	{	
		get { return diffusetex;}
		set 
		{	
			diffusetex = value;
			SetTexture(@"DiffuseTex", diffusetex);			
		}
	}

	private TextureBase diffusetex = null;
	public void SetNormalTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"NormalTex", TextureObject);
	}

	public void SetNormalTex2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"NormalTex", TextureObject);
	}

	public TextureBase NormalTex2D 
	{	
		get { return normaltex;}
		set 
		{	
			normaltex = value;
			SetTexture(@"NormalTex", normaltex);			
		}
	}

	private TextureBase normaltex = null;
	public void SetPositionTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"PositionTex", TextureObject);
	}

	public void SetPositionTex2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"PositionTex", TextureObject);
	}

	public TextureBase PositionTex2D 
	{	
		get { return positiontex;}
		set 
		{	
			positiontex = value;
			SetTexture(@"PositionTex", positiontex);			
		}
	}

	private TextureBase positiontex = null;


	
	public OpenTK.Vector3[] LightColors
	{
		get { return lightcolors; }
		set 
		{
			lightcolors = value;
			SetUniformVarData(@"lightColors", ref lightcolors);			
		}
	}
	private OpenTK.Vector3[] lightcolors;
	
	public System.Int32 LightCount
	{
		get { return lightcount; }
		set 
		{
			lightcount = value;
			SetUniformVarData(@"lightCount", lightcount);			
		}
	}
	private System.Int32 lightcount;
	
	public OpenTK.Vector2[] LightMinMaxs
	{
		get { return lightminmaxs; }
		set 
		{
			lightminmaxs = value;
			SetUniformVarData(@"lightMinMaxs", ref lightminmaxs);			
		}
	}
	private OpenTK.Vector2[] lightminmaxs;
	
	public OpenTK.Vector3[] LightPositions
	{
		get { return lightpositions; }
		set 
		{
			lightpositions = value;
			SetUniformVarData(@"lightPositions", ref lightpositions);			
		}
	}
	private OpenTK.Vector3[] lightpositions;



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

	public OpenTK.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value; 
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Matrix4 >(@"CameraTransform", ref value, 0 );
		}
	}
	public OpenTK.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value; 
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Matrix4 >(@"CameraTransform", ref value, 64 );
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
	
    float InvLenH = 1 / sqrt( 2 + 2 * LoV );
	
    float NoH = clamp(( NoL + NoV ) * InvLenH, 0.0, 1.0);
	
    float VoH = clamp( InvLenH + InvLenH * LoV , 0.0, 1.0);
	
    NoL = clamp(NoL,0.0,1.0);
	
    NoV = clamp(abs(NoV) + 1e-5,0.0,1.0);

	// Generalized microfacet specular
	float D = D_GGX( LobeRoughness[1], NoH ) * LobeEnergy[1];
	float Vis = Vis_SmithJointApprox( LobeRoughness[1], NoV, NoL );
	vec3 F = F_Schlick( SpecularColor, VoH );

	vec3 Diffuse = Diffuse_Lambert( DiffuseColor );

	return Diffuse * LobeEnergy[2] + (D * Vis) * F;    
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

    
    //vec3 ambient = vec3(0.03) * albedo * ao;
    //vec3 ambient = vec3(0.03) * albedo;
    vec3 ambient = vec3(0);
    //vec3 ambient = albedo ;
    vec3 color = ambient + Lo;
	
    //color = color / (color + vec3(1.0));
    color = pow(color, vec3(1.0/2.2));  
   
    FragColor = vec4(color, 1.0);	
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

	public void SettexCubemap2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"texCubemap", TextureObject);
	}

	public TextureBase TexCubemap2D 
	{	
		get { return texcubemap;}
		set 
		{	
			texcubemap = value;
			SetTexture(@"texCubemap", texcubemap);			
		}
	}

	private TextureBase texcubemap = null;

	public OpenTK.Matrix4 ModelMatrix
	{
		get { return modelmatrix; }
		set 
		{
			modelmatrix = value;
			SetUniformVarData(@"ModelMatrix", modelmatrix);			
		}
	}
	private OpenTK.Matrix4 modelmatrix;
	public OpenTK.Matrix4 ProjMatrix
	{
		get { return projmatrix; }
		set 
		{
			projmatrix = value;
			SetUniformVarData(@"ProjMatrix", projmatrix);			
		}
	}
	private OpenTK.Matrix4 projmatrix;
	public OpenTK.Matrix4 ViewMatrix
	{
		get { return viewmatrix; }
		set 
		{
			viewmatrix = value;
			SetUniformVarData(@"ViewMatrix", viewmatrix);			
		}
	}
	private OpenTK.Matrix4 viewmatrix;





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

layout (location=0) out vec4 FragColor;

void main()
{
    vec4 Color = texture(texCubemap, -CubemapTexCoord);
    FragColor = Color;
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

	public OpenTK.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Matrix4 >(@"CameraTransform", ref value, 0 );
		}
	}
	public OpenTK.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Matrix4 >(@"CameraTransform", ref value, 64 );
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

	public OpenTK.Matrix4 ModelTransform_Model
	{
		get { return modeltransform.Model ; }
		set 
		{ 
			modeltransform.Model = value;
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", ref modeltransform);
			//this.SetUniformBufferMemberValue< OpenTK.Matrix4 >(@"ModelTransform", ref value, 0 );
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

	public void SetDepthTex2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"DepthTex", TextureObject);
	}

	public TextureBase DepthTex2D 
	{	
		get { return depthtex;}
		set 
		{	
			depthtex = value;
			SetTexture(@"DepthTex", depthtex);			
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
uniform float Far;
uniform float Near;

void main() 
{   
    FragColor = texture(DepthTex, InTexCoord);

    if(FragColor.x > 100)
    {
        FragColor.xyz = vec3(1);
    }
    else
    {
        FragColor.xyz = vec3((FragColor.x - Near) / (Far - Near));
    }
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

	public void SetFontTexture2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"FontTexture", TextureObject);
	}

	public TextureBase FontTexture2D 
	{	
		get { return fonttexture;}
		set 
		{	
			fonttexture = value;
			SetTexture(@"FontTexture", fonttexture);			
		}
	}

	private TextureBase fonttexture = null;

	public OpenTK.Vector2 ScreenSize
	{
		get { return screensize; }
		set 
		{
			screensize = value;
			SetUniformVarData(@"ScreenSize", screensize);			
		}
	}
	private OpenTK.Vector2 screensize;





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


	public OpenTK.Vector2 ScreenSize
	{
		get { return screensize; }
		set 
		{
			screensize = value;
			SetUniformVarData(@"ScreenSize", screensize);			
		}
	}
	private OpenTK.Vector2 screensize;

	
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
	
	public OpenTK.Vector3 BoxColor
	{
		get { return boxcolor; }
		set 
		{
			boxcolor = value;
			SetUniformVarData(@"BoxColor", boxcolor);			
		}
	}
	private OpenTK.Vector3 boxcolor;




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



	
	public OpenTK.Vector3 LineColor
	{
		get { return linecolor; }
		set 
		{
			linecolor = value;
			SetUniformVarData(@"LineColor", linecolor);			
		}
	}
	private OpenTK.Vector3 linecolor;


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

	public OpenTK.Matrix4 CameraTransform_View
	{
		get { return cameratransform.View ; }
		set 
		{ 
			cameratransform.View = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Matrix4 >(@"CameraTransform", ref value, 0 );
		}
	}
	public OpenTK.Matrix4 CameraTransform_Proj
	{
		get { return cameratransform.Proj ; }
		set 
		{ 
			cameratransform.Proj = value;
			this.SetUniformBufferValue< CameraTransform >(@"CameraTransform", ref cameratransform);
			//this.SetUniformBufferMemberValue< OpenTK.Matrix4 >(@"CameraTransform", ref value, 64 );
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

	public OpenTK.Matrix4 ModelTransform_Model
	{
		get { return modeltransform.Model ; }
		set 
		{ 
			modeltransform.Model = value;
			this.SetUniformBufferValue< ModelTransform >(@"ModelTransform", ref modeltransform);
			//this.SetUniformBufferMemberValue< OpenTK.Matrix4 >(@"ModelTransform", ref value, 0 );
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

	public void SetFontTexture2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"FontTexture", TextureObject);
	}

	public TextureBase FontTexture2D 
	{	
		get { return fonttexture;}
		set 
		{	
			fonttexture = value;
			SetTexture(@"FontTexture", fonttexture);			
		}
	}

	private TextureBase fonttexture = null;

	public OpenTK.Matrix4 Model
	{
		get { return model; }
		set 
		{
			model = value;
			SetUniformVarData(@"Model", model);			
		}
	}
	private OpenTK.Matrix4 model;
	public OpenTK.Matrix4 Proj
	{
		get { return proj; }
		set 
		{
			proj = value;
			SetUniformVarData(@"Proj", proj);			
		}
	}
	private OpenTK.Matrix4 proj;
	public OpenTK.Matrix4 View
	{
		get { return view; }
		set 
		{
			view = value;
			SetUniformVarData(@"View", view);			
		}
	}
	private OpenTK.Matrix4 view;

	
	public OpenTK.Vector3 TextColor
	{
		get { return textcolor; }
		set 
		{
			textcolor = value;
			SetUniformVarData(@"TextColor", textcolor);			
		}
	}
	private OpenTK.Vector3 textcolor;




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
namespace SSAO
{


public class SSAO : MaterialBase
{
	public SSAO() 
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

	public void SetgNoiseMap2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"gNoiseMap", TextureObject);
	}

	public void SetgNoiseMap2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"gNoiseMap", TextureObject);
	}

	public TextureBase GNoiseMap2D 
	{	
		get { return gnoisemap;}
		set 
		{	
			gnoisemap = value;
			SetTexture(@"gNoiseMap", gnoisemap);			
		}
	}

	private TextureBase gnoisemap = null;
	public void SetgNormalMap2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"gNormalMap", TextureObject);
	}

	public void SetgNormalMap2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"gNormalMap", TextureObject);
	}

	public TextureBase GNormalMap2D 
	{	
		get { return gnormalmap;}
		set 
		{	
			gnormalmap = value;
			SetTexture(@"gNormalMap", gnormalmap);			
		}
	}

	private TextureBase gnormalmap = null;


	
	public OpenTK.Matrix4 ProjectionMatrix
	{
		get { return projectionmatrix; }
		set 
		{
			projectionmatrix = value;
			SetUniformVarData(@"ProjectionMatrix", projectionmatrix);			
		}
	}
	private OpenTK.Matrix4 projectionmatrix;
	
	public System.Single FFarClipDistance
	{
		get { return ffarclipdistance; }
		set 
		{
			ffarclipdistance = value;
			SetUniformVarData(@"fFarClipDistance", ffarclipdistance);			
		}
	}
	private System.Single ffarclipdistance;
	
	public System.Single FOcclusionRadius
	{
		get { return focclusionradius; }
		set 
		{
			focclusionradius = value;
			SetUniformVarData(@"fOcclusionRadius", focclusionradius);			
		}
	}
	private System.Single focclusionradius;
	
	public System.Int32 NKernelSize
	{
		get { return nkernelsize; }
		set 
		{
			nkernelsize = value;
			SetUniformVarData(@"nKernelSize", nkernelsize);			
		}
	}
	private System.Int32 nkernelsize;
	
	public OpenTK.Vector4[] VKernelOffsets
	{
		get { return vkerneloffsets; }
		set 
		{
			vkerneloffsets = value;
			SetUniformVarData(@"vKernelOffsets", ref vkerneloffsets);			
		}
	}
	private OpenTK.Vector4[] vkerneloffsets;




	public static string GetVSSourceCode()
	{
		return @"#version 330

layout (location = 0) in vec3 VertexPosition;
layout (location = 1) in vec2 VertexTexCoord;
layout (location = 2) in vec3 FrustumVector;

out vec3 vFrustumRay;
out vec2 TexCoord;

void main()
{
    TexCoord = VertexTexCoord;
    
	gl_Position = vec4(VertexPosition.xy, 0.0, 1.0);

	vFrustumRay = FrustumVector;
}
";
	}

	public static string GetFSSourceCode()
	{
		return @"#version 330

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
";
	}
}
}

}
