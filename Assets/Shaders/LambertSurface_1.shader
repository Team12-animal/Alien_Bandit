Shader "Custom/LambertSurface_1"
{
    Properties
    {
        _SpecColor("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
        _Shininess("Shininess", Range(0.0, 100.0)) = 1.0
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _BumpMap("Bumpmap", 2D) = "bump" {}
        _Cube("Cubemap", CUBE) = "" {}
        _Amount("Extrude", Float) = 0.0
        _ColorTint("Tint", Color) = (0.5, 0.5, 0.5, 1)

    }
    SubShader{
        Tags { "RenderType" = "Opaque" }
        CGPROGRAM
           #pragma surface surf BlinnPhong vertex:verterModifier finalcolor:mycolor
           float _Shininess;
          sampler2D _MainTex;
          sampler2D _BumpMap;
  
          struct Input {
              float2 uv_MainTex;
              float2 uv_BumpMap;

          };
          float _Amount;
          void verterModifier(inout appdata_full v) {
              //v.vertex.xyz += v.normal * _Amount;
              v.vertex.y += v.normal * _Amount;
          }
          fixed4 _ColorTint;
          void mycolor(Input IN, SurfaceOutput o,
              inout fixed4 color)
          {
              color *= _ColorTint;
          }


          void surf(Input IN, inout SurfaceOutput o) {
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex);
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

            o.Specular = _Shininess;
            o.Gloss = 1;
          }
      ENDCG
   }
   Fallback "Diffuse"
}
