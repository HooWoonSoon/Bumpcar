Shader"BYCW/MyShaderURP"
{
	Properties
	{
		_FrontTex("Front Texture", 2D) = "white" {}
        _BackTex("Back Texture", 2D) = "white" {}
		_Angle("Rotation Angle", Range(0,180)) = 0
	}

	SubShader
	{
		Pass
		{
Cull Off
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

struct a2v
{
    float4 pos : POSITION;
    float2 uv : TEXCOORD0;
};

struct v2f
{
    float4 svpos : SV_POSITION;
    float2 svuv : TEXCOORD0;
};

			TEXTURE2D(_FrontTex);
			SAMPLER(sampler_FrontTex);
			TEXTURE2D(_BackTex);
			SAMPLER(sampler_BackTex);
float _Angle;

v2f vert(a2v i)
{
    float sins;
    float coss;
    sincos(radians(_Angle), sins, coss);
				// Rotation based on z matrix
    float4x4 rotateMatrix =
    {
        coss, sins, 0, 0,
					-sins, coss, 0, 0,
					0, 0, 1, 0,
					0, 0, 0, 1.0
    };

    v2f o;
    i.pos -= float4(5, 0, 0, 0);
    i.pos.y = sin(i.pos.x * 0.6f) * sins;
    i.pos = mul(rotateMatrix, i.pos);
    i.pos += float4(5, 0, 0, 0);
    o.svpos = TransformObjectToHClip(i.pos);
    o.svuv = i.uv;
    return o;
}

half4 frag(v2f u) : SV_Target
{
    if (_Angle <= 65)
        return SAMPLE_TEXTURE2D(_FrontTex, sampler_FrontTex, u.svuv);
    else
        return SAMPLE_TEXTURE2D(_BackTex, sampler_BackTex, u.svuv);
}
			ENDHLSL
		}
	}
}
