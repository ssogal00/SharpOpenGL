/* Freetype GL - A C OpenGL Freetype engine
 *
 * Distributed under the OSI-approved BSD 2-Clause License.  See accompanying
 * file `LICENSE` for more details.
 */

#version 450

layout (location = 0) in vec3 VertexPosition;
layout (location = 1) in vec2 VertexTexCoord;
layout (location = 2) in vec4 VertexColor;

layout (location = 0 ) out vec3 OutPosition;
layout (location = 1 ) out vec2 OutTexCoord;
layout (location = 2 ) out vec4 OutVertexColor;

uniform mat4 u_model;
uniform mat4 u_view;
uniform mat4 u_projection;
uniform vec4 u_color;

//attribute vec3 vertex;
//attribute vec2 tex_coord;
//attribute vec4 color;


void main(void)
{
	OutTexCoord.xy = VertexTexCoord.xy;
	OutVertexColor = VertexColor * u_color;
    
    // gl_TexCoord[0].xy = tex_coord.xy;
    // gl_FrontColor     = color * u_color;    
    gl_Position       = u_projection*(u_view*(u_model*vec4(VertexPosition,1.0)));    
}
