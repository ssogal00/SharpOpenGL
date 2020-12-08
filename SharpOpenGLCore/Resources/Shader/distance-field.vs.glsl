/* Freetype GL - A C OpenGL Freetype engine
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
