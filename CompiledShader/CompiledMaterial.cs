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
	public void SetSpecularTex2D(Core.Texture.TextureBase TextureObject)
	{
		SetTexture(@"SpecularTex", TextureObject);
	}

	public void SetSpecularTex2D(int TextureObject, Sampler sampler)
	{
		SetTexture(@"SpecularTex", TextureObject);
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
    
    //for( int i = 0; i < 9; i++ )
    //{
     //   color += (texture(ColorTex, (TexCoord + BlurOffsets[i]))) * BlurWeights[i].x;        
//    }
	        
    FragColor = vec4(TexCoord, 0, 0);
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


    // vec3 diffuse = max(dot(Normal.xyz, LightDir), 0.0) * Color * LightDiffuse;
    vec3 diffuse = Color;
    // vec4 diffuse = GetCookTorrance(Normal.xyz, LightDir, ViewDir, Half, LightAmbient, Color);

    vec3 specular = pow(max(dot(Normal.xyz, Half), 0) , 64.0) * vec3(Normal.a);


	vec4 FinalColor;
    FinalColor.xyz = diffuse.xyz ;
    
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

}
