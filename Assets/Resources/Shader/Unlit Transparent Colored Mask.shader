// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "S2Custom/UI/Unlit Transparent Colored Mask"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
        _TexMask ("Mask (RGB), Alpha (A)", 2D) = "white" {}
		_MainTexSizeX("Main Tex Size X", float)  = 0.0
		_MainTexSizeY("Main Tex Size Y", float)  = 0.0
        _MaskOffsetX("Mask Offset X", float)     = 0.0
		_MaskOffsetY("Mask Offset Y", float)     = 0.0
        _MaskSizeX("Mask Size X", float)         = 0.0
        _MaskSizeY("Mask Size Y", float)         = 0.0
	}
	
	SubShader
	{
		LOD 200

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
            sampler2D _TexMask;
			float4 _MainTex_ST;
            float4 _TexMask_ST;
	
			uniform float _MainTexSizeX;
			uniform float _MainTexSizeY;
			uniform float _MaskOffsetX;
			uniform float _MaskOffsetY;
			uniform float _MaskSizeX;
			uniform float _MaskSizeY;
            
			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			v2f o;

			v2f vert (appdata_t v)
			{
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color;
				return o;
			}
            
			float GetUVToDecalU(float fPixel)
			{
			    return (fPixel / _MainTexSizeX);
			}
			float GetUVToDecalV(float fPixel)
			{
			    return (fPixel / _MainTexSizeY);
			}
			float GetSizeRatioX(float fPixel)
			{
			    return (_MainTexSizeX / fPixel);
			}
			float GetSizeRatioY(float fPixel)
			{
			    return (_MainTexSizeY / fPixel);
			}
			float GetDecalUV(float fOri, float fDecal, float fSizeRatio)
			{
				return (fOri - fDecal) * fSizeRatio;
			}
			
			fixed4 frag (v2f IN) : COLOR
			{
                fixed4 f4BackColor = tex2D(_MainTex, IN.texcoord) * IN.color;

                float2 f2MaskUV;
                f2MaskUV.x = GetDecalUV(IN.texcoord.x, GetUVToDecalU(_MaskOffsetX - (_MaskSizeX * 0.5)), GetSizeRatioX(_MaskSizeX));
                f2MaskUV.y = GetDecalUV(IN.texcoord.y, GetUVToDecalV(_MaskOffsetY - (_MaskSizeY * 0.5)), GetSizeRatioY(_MaskSizeY));
                fixed4 f4MaskColor = tex2D (_TexMask, f2MaskUV);
				//f4BackColor.a = f4MaskColor.a;
                f4BackColor.a = lerp(0.0, f4BackColor.a, f4MaskColor.a);
                
				return f4BackColor;
			}
			ENDCG
		}
	}

	SubShader
	{
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse
			
			SetTexture [_MainTex]
			{
				Combine Texture * Primary
			}
		}
	}
}
