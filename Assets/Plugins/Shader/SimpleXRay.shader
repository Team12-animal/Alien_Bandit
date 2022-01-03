Shader "Examples/SimpleXRay"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (1,1,1,1)
        _RimPower ("Rim", Float) = 0.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }

        Pass
        {
        Blend SrcAlpha One
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldPosition : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
            };
           
            fixed4 _MainColor;
            float _RimPower;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPosition = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 camDir = UnityWorldSpaceViewDir(i.worldPosition);
                float fV = 1.0 - saturate(dot(i.worldNormal, normalize(camDir)));

                fixed4 col = _MainColor*pow (fV, _RimPower);
                return col;
            }
            ENDCG
        }
    }
}
