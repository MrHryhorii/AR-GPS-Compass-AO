Shader "GPS/NonMask"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _AOTex ("AO", 2D) = "white" {}
        _BumpMap ("Bumpmap", 2D) = "bump" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
 
        CGPROGRAM
        #pragma surface surf Standard
 
        struct Input {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float2 uv2_AOTex;
        };

            fixed4 _Color;
            sampler2D _MainTex;
            sampler2D _BumpMap;
            sampler2D _AOTex;
            half _Glossiness;
            half _Metallic;

            void surf (Input IN, inout SurfaceOutputStandard o) {
                half4 c = tex2D (_MainTex, IN.uv_MainTex);
                half ao = tex2D(_AOTex, IN.uv2_AOTex.xy).r;
                half4 output_col = c * _Color;
                o.Albedo = output_col.rgb;
                o.Alpha = output_col.a;
                o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
                o.Smoothness = _Glossiness;
                o.Metallic = _Metallic;
                o.Occlusion = ao;
            }
        ENDCG
    }
    FallBack "Diffuse"
}
