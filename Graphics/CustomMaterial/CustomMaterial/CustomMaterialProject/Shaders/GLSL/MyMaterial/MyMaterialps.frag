#ifdef GL_ES
precision mediump float;
#endif

uniform sampler2D DiffuseMap;

varying vec2 outTexCoord;

void main(void)
{
	gl_FragColor = texture2D(DiffuseMap, outTexCoord);
}
