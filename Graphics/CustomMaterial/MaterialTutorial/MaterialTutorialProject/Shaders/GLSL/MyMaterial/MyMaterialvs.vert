uniform mat4	WorldViewProj;

uniform float	Time;

attribute vec3 Position0;
attribute vec3 Normal0;
attribute vec2 TextureCoordinate0;

varying vec2 outTexCoord;

void main(void)
{
	float offsetScale = abs(sin(Time + (TextureCoordinate0.y * 16.0))) * 0.25;
	vec3 vectorOffset = Normal0 * offsetScale;
	gl_Position = WorldViewProj * vec4(Position0 + vectorOffset, 1);
	outTexCoord = TextureCoordinate0;
}