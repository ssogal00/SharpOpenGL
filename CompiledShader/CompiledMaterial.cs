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

layout (location = 0, binding=0) uniform sampler2D DiffuseTex;
layout (location = 1, binding=1) uniform sampler2D NormalTex;
layout (location = 2, binding=2) uniform sampler2D MaskTex;
layout (location = 3, binding=3) uniform sampler2D SpecularTex;

uniform int SpecularMapExist;
uniform int MaskMapExist;
uniform int NormalMapExist;

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

    if(SpecularMapExist > 0)
    {
        NormalColor.a = texture(SpecularTex, InTexCoord).x;
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
	public System.Int32 Dump_SpecularDump
	{
		get { return dump.SpecularDump ; }
		set 
		{ 
			dump.SpecularDump = value; 
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
    int SpecularDump;
    int DiffuseDump;
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
    else if(SpecularDump > 0)
    {
        FragColor = texture(NormalTex,InTexCoord).aaaa;
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


	
	public OpenTK.Vector2[] BlurOffsets
	{
		get { return bluroffsets; }
		set 
		{
			bluroffsets = value;
			SetUniformVarData(@"BlurOffsets", ref bluroffsets);			
		}
	}
	private OpenTK.Vector2[] bluroffsets;
	
	public OpenTK.Vector2[] BlurWeights
	{
		get { return blurweights; }
		set 
		{
			blurweights = value;
			SetUniformVarData(@"BlurWeights", ref blurweights);			
		}
	}
	private OpenTK.Vector2[] blurweights;




	public static string GetVSSourceCode()
	{
		return @"#version 450

layout (location = 0) in vec3 VertexPosition;
layout (location = 1) in vec2 VertexTexCoord;

out vec2 TexCoord;

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

in vec2 TexCoord;

uniform sampler2D ColorTex;
uniform vec2 BlurOffsets[9];
uniform vec2 BlurWeights[9];

layout( location = 0 ) out vec4 FragColor;

void main() 
{
	vec4 color = vec4(0,0,0,0);
    
    for( int i = 0; i < 9; i++ )
    {
       color += (texture(ColorTex, (TexCoord + BlurOffsets[i]))) * BlurWeights[i].x;        
    }
	        
    FragColor = color;
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


	
	public OpenTK.Vector3 LobeEnergy
	{
		get { return lobeenergy; }
		set 
		{
			lobeenergy = value;
			SetUniformVarData(@"LobeEnergy", lobeenergy);			
		}
	}
	private OpenTK.Vector3 lobeenergy;
	
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


    private Light light = new Light();
	public Light Light
	{
		get { return light; }
		set 
		{ 
			light = value; 
			this.SetUniformBufferValue< Light >(@"Light", ref value);
		}
	}

	public OpenTK.Vector3 Light_LightDir
	{
		get { return light.LightDir ; }
		set 
		{ 
			light.LightDir = value;
			this.SetUniformBufferValue< Light >(@"Light", ref light);
			//this.SetUniformBufferMemberValue< OpenTK.Vector3 >(@"Light", ref value, 0 );
		}
	}
	public OpenTK.Vector3 Light_LightAmbient
	{
		get { return light.LightAmbient ; }
		set 
		{ 
			light.LightAmbient = value;
			this.SetUniformBufferValue< Light >(@"Light", ref light);
			//this.SetUniformBufferMemberValue< OpenTK.Vector3 >(@"Light", ref value, 16 );
		}
	}
	public OpenTK.Vector3 Light_LightDiffuse
	{
		get { return light.LightDiffuse ; }
		set 
		{ 
			light.LightDiffuse = value;
			this.SetUniformBufferValue< Light >(@"Light", ref light);
			//this.SetUniformBufferMemberValue< OpenTK.Vector3 >(@"Light", ref value, 32 );
		}
	}
	public OpenTK.Vector3 Light_LightSpecular
	{
		get { return light.LightSpecular ; }
		set 
		{ 
			light.LightSpecular = value;
			this.SetUniformBufferValue< Light >(@"Light", ref light);
			//this.SetUniformBufferMemberValue< OpenTK.Vector3 >(@"Light", ref value, 48 );
		}
	}
	public System.Single Light_LightSpecularShininess
	{
		get { return light.LightSpecularShininess ; }
		set 
		{ 
			light.LightSpecularShininess = value;
			this.SetUniformBufferValue< Light >(@"Light", ref light);
			//this.SetUniformBufferMemberValue< System.Single >(@"Light", ref value, 60 );
		}
	}



	public static string GetVSSourceCode()
	{
		return @"#version 450

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
		return @"#version 450

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
