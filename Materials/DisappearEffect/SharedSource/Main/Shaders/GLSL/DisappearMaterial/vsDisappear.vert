uniform mat4 WorldViewProj;

attribute vec3 Position0;
attribute vec2 TextureCoordinate0;
attribute vec2 TextureCoordinate1;

varying vec2 outTexCoord0;
varying vec2 outTexCoord1;

void main(void)
{
	gl_Position = WorldViewProj * vec4(Position0, 1);
	outTexCoord0 = TextureCoordinate0;
	outTexCoord1 = TextureCoordinate1;
}