/* Freetype GL - A C OpenGL Freetype engine
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


