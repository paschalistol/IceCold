Shader "Unlit/Read"
{
	Properties{
			_Color("Tint", Color) = (0, 0, 0, 1)
			_MainTex("Texture", 2D) = "white" {}
			_Smoothness("Smoothness", Range(0, 1)) = 0
			_Metallic("Metalness", Range(0, 1)) = 0
			[HDR] _Emission("Emission", color) = (0,0,0)

			[IntRange] _StencilRef("Stencil Reference Value", Range(0,255)) = 0
					  _BumpMap("NormalMap", 2D) = "bump" {}
	}
		SubShader{
			Tags{ "RenderType" = "Opaque" "Queue" = "Geometry"}

			//stencil operation
			Stencil{
				Ref[_StencilRef]
				Comp Equal
			}

			CGPROGRAM

			#pragma surface surf Standard fullforwardshadows
			#pragma target 3.0

			sampler2D _MainTex;
			fixed4 _Color;
			sampler2D _BumpMap;
			half _Smoothness;
			half _Metallic;
			half3 _Emission;

			struct Input {
				float2 uv_MainTex;
				float2 uv_BumpMap;
			};

			void surf(Input i, inout SurfaceOutputStandard o) {
				fixed4 col = tex2D(_MainTex, i.uv_MainTex);
				col *= _Color;
				o.Albedo = tex2D(_MainTex, i.uv_MainTex).rgb;
				o.Metallic = _Metallic;
				o.Smoothness = _Smoothness;
				o.Emission = _Emission;
				o.Normal = UnpackNormal(tex2D(_BumpMap, i.uv_BumpMap));
			}
			ENDCG
			}
				FallBack "Standard"
}
