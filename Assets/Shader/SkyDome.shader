Shader "Custom/SkyDome" {
	Properties{
		_HogeTex("Base", 2D) = "white" {}
	}

		SubShader{
		Tags{ "Queue" = "Transparent" }
		Cull Front

		CGPROGRAM

#pragma surface surf Lambert alpha

		sampler2D _HogeTex;

	struct Input {
		float2 uv_HogeTex;
	};

	void surf(Input IN, inout SurfaceOutput o) {
		half4 color = tex2D(_HogeTex, IN.uv_HogeTex);
		o.Albedo = color.rgb;
		o.Alpha = color.a;
	}

	ENDCG
	}
	FallBack "Diffuse"
}
