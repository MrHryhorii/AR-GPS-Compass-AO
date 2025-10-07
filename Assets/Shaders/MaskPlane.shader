Shader "GPS/MaskPlane"
{
    SubShader {
        Tags {"RenderType"="Opaque"}
       
        Stencil {
            Ref 1
            Comp always
            Pass replace
        }

        CGPROGRAM
        #pragma surface surf Lambert alpha
 
        struct Input {
            fixed3 Albedo;
        };
 
        void surf (Input IN, inout SurfaceOutput o) {
            o.Albedo = fixed3(0, 0, 0);
            o.Alpha = 0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
