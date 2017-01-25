#ifdef GL_ES
	precision mediump float;
#endif

uniform float Threshold;
uniform float BurnSize;

uniform sampler2D DiffuseMap;
uniform sampler2D OpacityMap;
uniform sampler2D BurnMap;

varying vec2 outTexCoord0;
varying vec2 outTexCoord1;

void main(void)
{
	vec4 color = texture2D(DiffuseMap, outTexCoord0);
    vec4 ramp = texture2D(OpacityMap, outTexCoord1);

	float test = ramp.x - Threshold;

	if(test < 0.0)
	{
		discard;
	}

	if (test < BurnSize && Threshold > 0.0 && Threshold < 1.0)
	{
		color += texture2D(BurnMap, vec2(test * (1.0 / BurnSize), 0));
	}
	
	gl_FragColor = color;
}