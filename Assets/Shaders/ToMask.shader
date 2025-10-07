Shader "GPS/ToMask"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _BumpMap ("Bumpmap", 2D) = "bump" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque"}
        //LOD 100

        Stencil {
            Ref 1
            Comp NotEqual
            Pass keep
        }
 
        CGPROGRAM
        #pragma surface surf Standard
 
        struct Input {
            float2 uv_MainTex;
            float2 uv_BumpMap;
        };

            fixed4 _Color;
            sampler2D _MainTex;
            sampler2D _BumpMap;
            half _Glossiness;
            half _Metallic;

            void surf (Input IN, inout SurfaceOutputStandard o) {
                half4 c = tex2D (_MainTex, IN.uv_MainTex);
                half4 output_col = c * _Color;
                o.Albedo = output_col.rgb;
                o.Alpha = output_col.a;
                o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
                o.Smoothness = _Glossiness;
                o.Metallic = _Metallic;
            }
        ENDCG
    }
    FallBack "Diffuse"
}
