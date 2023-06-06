Shader "Custom/Sonar"
{
	Properties
	{
		_MainTex ("Sprite Texture", 2D) = "white" {}
		_HasBlackWhite ("BW Support (0/1)", Float) = 1.0

		_Ambience ("Ambience", Float) = 1.0
		_SonarPosition ("Sonar Position", Vector) = (100.0, 0.0, 0.0, 0.0)
		_SonarRadius ("Sonar Radius", Float) = 0.0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent"
			"RenderType"="Transparent"
		}

		Cull Off
		ZWrite Off
		Lighting Off
		Fog { Mode Off }
		Blend One OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 position : SV_POSITION;
				half2 texcoord : TEXCOORD0;
			};

			//------- VERTEX -------

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.position = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;

				return OUT;
			}

			//------- FRAGMENT -------

			sampler2D _MainTex;
			float _HasBlackWhite;

			float _Ambience;
			float4 _SonarPosition;
			float _SonarRadius;

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 coloredTexel = tex2D(_MainTex, IN.texcoord);
				fixed4 bwTexel = tex2D(_MainTex, IN.texcoord - float2(0, 0.5));

				float sonarIntensity = (_SonarRadius - distance(IN.position.xy, _SonarPosition.xy)) * 10.0 / _SonarRadius;
				sonarIntensity = clamp(sonarIntensity, 0.0, 1.0);
				float texelIntensity = clamp(_Ambience + sonarIntensity, 0.0, 1.0);

				float4 c;
				c.a = coloredTexel.a;

				if(_HasBlackWhite){
					if(texelIntensity > 0.75){
						c.rgb = lerp(bwTexel, coloredTexel, (texelIntensity-0.75) * 4.0) * c.a;
					}else{
						c.rgb = clamp(bwTexel * texelIntensity * 1.333333, 0.0, 1.0) * c.a;
					}
				}else{
					c.rgb = lerp(0.0, coloredTexel, (texelIntensity-0.5) * 2.0) * c.a;
				}

				return c;
			}
			ENDCG
		}
	}
}
