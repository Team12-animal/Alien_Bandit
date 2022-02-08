Shader "Unlit/RainningOnWindow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Size("Size",float) = 1
        _TimeforSetting("Time",float) = 1
        _Distortion("Distortion",range(-6,6)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            //Using define is a smart way to search every place,if define something what will tell Unity when compile the shader!
            //in this example when Using "S",unity will get smoothstep(a,b,t);
            #define S(a,b,t) smoothstep(a,b,t)

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
         
            //Using Properties in Shader Code
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Size;
            float _TimeforSetting;
            float _Distortion;
         
            //VertexShader
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            //make a random pattern
            float RandomBoxPattern(float2 p)
            {
                p = frac(p*float2(123.34f,345.45f));
                p += dot(p,p + 34.345f);
                return frac(p.x * p.y);
            }

            //Fragment Shader
            fixed4 frag (v2f i) : SV_Target
            {
                float timeOfYVelue = fmod(_Time.y +_TimeforSetting,7200);
                //Init return color;
                float4 col = 0;
                
                //Take the uv corner getting form VertexShader, and expose on the corner with parameter;
                float2 aspect = float2(2,1);
                float2 uv = i.uv *_Size * aspect;
                uv.y += timeOfYVelue * 0.25f;

                //-0.5f set the origin to middle;
                float2 gv = frac(uv)-0.5f;

                //make id to each quad box
                float2 id = floor(uv);

                //make a drop path;
                float n = RandomBoxPattern(id);//for noise 0 or 1;
                timeOfYVelue += n * 6.28341; //6.2831 = 2 PI
                float w = i.uv.y * 10;
                float x = (n - 0.5f) * 0.8f; //the velue between -0.4f~0.4f; 
                x += (0.4-abs(x)) * sin(3*w)*pow(sin(w),6) * 0.45f;//(0.4-abs(x))make sure the velue between x(-0.4f~0.4f);
                float y = -sin(timeOfYVelue + sin(timeOfYVelue + sin(timeOfYVelue)*0.5f))*0.45f;
                y -= (gv.x-x)*(gv.x-x);
                float2 dropPos = (gv-float2(x,y)) /aspect;
                float drop = S(0.05f,0.03f,length(dropPos));

                //make a tail path
                float2 trailPos = (gv-float2(x,timeOfYVelue *0.25f)) /aspect;
                trailPos.y = (frac(trailPos.y *6)-0.5f)/6;
                float trail = S(0.03f,0.01f,length(trailPos));
                //make trail drop slow down;
                float fogTrail = S(-0.05f,0.05f,dropPos.y);
                //make trail transparent when uv near by Y=1;
                trail *= S(0.5f,y,gv.y);
                trail *= fogTrail;
                fogTrail *= S(0.05f,0.04f,abs(dropPos.x));

                col += fogTrail * 0.5f;
                col += trail;
                col += drop;

                //if(gv.x > 0.48f || gv.y >0.49f)col = float4(1,0,0,1);//print red lines 
                //col.rg = id *0.1f;
                //col = RandomBoxPattern(id);

                float2 offsetPos = drop*dropPos + trail*trailPos;
                col = tex2D(_MainTex,i.uv+offsetPos * _Distortion);

                return col;
            }
            ENDCG
        }
    }
}
