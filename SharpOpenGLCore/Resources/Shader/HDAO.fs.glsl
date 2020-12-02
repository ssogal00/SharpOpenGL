#version 450

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
