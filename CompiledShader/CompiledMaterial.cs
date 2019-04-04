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

uniform bool MetalicExist;
uniform bool MaskMapExist;
uniform bool NormalMapExist;
uniform bool RoughnessExist;
uniform bool DiffuseMapExist;

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

	public void SetEquirectangularMap2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"EquirectangularMap", TextureObject);
	}

	public TextureBase EquirectangularMap2D 
	{	
		get { return equirectangularmap;}
		set 
		{	
			equirectangularmap = value;
			SetTexture(@"EquirectangularMap", equirectangularmap);			
		}
	}

	private TextureBase equirectangularmap = null;

	public OpenTK.Matrix4 Projection
	{
		get { return projection; }
		set 
		{
			projection = value;
			SetUniformVarData(@"Projection", projection);			
		}
	}
	private OpenTK.Matrix4 projection;
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

	public void SetEnvironmentMap2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"EnvironmentMap", TextureObject);
	}

	public TextureBase EnvironmentMap2D 
	{	
		get { return environmentmap;}
		set 
		{	
			environmentmap = value;
			SetTexture(@"EnvironmentMap", environmentmap);			
		}
	}

	private TextureBase environmentmap = null;

	public OpenTK.Matrix4 Projection
	{
		get { return projection; }
		set 
		{
			projection = value;
			SetUniformVarData(@"Projection", projection);			
		}
	}
	private OpenTK.Matrix4 projection;
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

	public void SetEquirectangularMap2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"EquirectangularMap", TextureObject);
	}

	public TextureBase EquirectangularMap2D 
	{	
		get { return equirectangularmap;}
		set 
		{	
			equirectangularmap = value;
			SetTexture(@"EquirectangularMap", equirectangularmap);			
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

	public void SetBrdfLUT2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"BrdfLUT", TextureObject);
	}

	public void SetBrdfLUT2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"BrdfLUT", TextureObject);
	}

	public TextureBase BrdfLUT2D 
	{	
		get { return brdflut;}
		set 
		{	
			brdflut = value;
			SetTexture(@"BrdfLUT", brdflut);			
		}
	}

	private TextureBase brdflut = null;
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
	public void SetIrradianceMap2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"IrradianceMap", TextureObject);
	}

	public void SetIrradianceMap2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"IrradianceMap", TextureObject);
	}

	public TextureBase IrradianceMap2D 
	{	
		get { return irradiancemap;}
		set 
		{	
			irradiancemap = value;
			SetTexture(@"IrradianceMap", irradiancemap);			
		}
	}

	private TextureBase irradiancemap = null;
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
	public void SetPrefilterMap2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"PrefilterMap", TextureObject);
	}

	public void SetPrefilterMap2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"PrefilterMap", TextureObject);
	}

	public TextureBase PrefilterMap2D 
	{	
		get { return prefiltermap;}
		set 
		{	
			prefiltermap = value;
			SetTexture(@"PrefilterMap", prefiltermap);			
		}
	}

	private TextureBase prefiltermap = null;


	
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
    vec3 F = fresnelSchlickRoughness(max(dot(N, V), 0.0), F0, roughness);
    
    vec3 kS = F;
    vec3 kD = 1.0 - kS;
    kD *= 1.0 - metallic;     
    
    vec3 irradiance = texture(IrradianceMap, N).rgb;
    vec3 diffuse      = irradiance * albedo;
    
    // sample both the pre-filter map and the BRDF lut and combine them together as per the Split-Sum approximation to get the IBL specular part.
    const float MAX_REFLECTION_LOD = 4.0;
    vec3 prefilteredColor = textureLod(PrefilterMap, R,  roughness * MAX_REFLECTION_LOD).rgb;    
    //vec3 prefilteredColor = texelFetch(PrefilterMap, R,  roughness * MAX_REFLECTION_LOD).rgb;    
    vec2 brdf  = texture(BrdfLUT, vec2(max(dot(N, V), 0.0), roughness)).rg;
    vec3 specular = prefilteredColor * (F * brdf.x + brdf.y);

    //vec3 ambient = (kD * diffuse + specular) * ao;

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

	public void SetBlurTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"BlurTex", TextureObject);
	}

	public void SetBlurTex2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"BlurTex", TextureObject);
	}

	public TextureBase BlurTex2D 
	{	
		get { return blurtex;}
		set 
		{	
			blurtex = value;
			SetTexture(@"BlurTex", blurtex);			
		}
	}

	private TextureBase blurtex = null;
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
uniform sampler2D BlurTex; 

void main()
{
	FragColor = texture(ColorTex, TexCoords) + texture(BlurTex, TexCoords);
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
	public void SetRandTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"RandTex", TextureObject);
	}

	public void SetRandTex2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"RandTex", TextureObject);
	}

	public TextureBase RandTex2D 
	{	
		get { return randtex;}
		set 
		{	
			randtex = value;
			SetTexture(@"RandTex", randtex);			
		}
	}

	private TextureBase randtex = null;


	
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
	
	public OpenTK.Vector3[] SampleKernel
	{
		get { return samplekernel; }
		set 
		{
			samplekernel = value;
			SetUniformVarData(@"SampleKernel", ref samplekernel);			
		}
	}
	private OpenTK.Vector3[] samplekernel;
	
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

layout (location = 0) out vec4 FragColor;

uniform mat4 ProjectionMatrix;

const int kernelSize = 64;

uniform vec3 SampleKernel[kernelSize];
uniform float Radius = 0.55;
uniform vec2 ScreenSize;

layout(binding=0) uniform sampler2D PositionTex;
layout(binding=1) uniform sampler2D NormalTex;
layout(binding=2) uniform sampler2D ColorTex;
layout(binding=4) uniform sampler2D RandTex;

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

    FragColor = vec4(AoData, AoData, AoData, 1.0f);
    
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

	public void SetEnvironmentMap2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"EnvironmentMap", TextureObject);
	}

	public TextureBase EnvironmentMap2D 
	{	
		get { return environmentmap;}
		set 
		{	
			environmentmap = value;
			SetTexture(@"EnvironmentMap", environmentmap);			
		}
	}

	private TextureBase environmentmap = null;

	public OpenTK.Matrix4 Projection
	{
		get { return projection; }
		set 
		{
			projection = value;
			SetUniformVarData(@"Projection", projection);			
		}
	}
	private OpenTK.Matrix4 projection;
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

}
