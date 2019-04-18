
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


    FragColor = texture(ScreenTex, InTexCoord);	
}