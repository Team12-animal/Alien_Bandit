Shader "Unlit/RainningOnWindow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Size("Size",float) = 1
        _TimeforSetting("Time",float) = 1
        _Distortion("Distortion",range(-6,6)) = 1
        _Blur("Blur",range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Transparent"}
        LOD 100

            GrabPass{"_GabTexture"}
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

            struct v2f//get all the information from VertexShader to Fragment Shader
            {
                float2 uv : TEXCOORD0;
                float4 grabUV:TEXCOORD1;
                float4 vertex : SV_POSITION;
            };
         
            //Using Properties in Shader Code
            sampler2D _MainTex;
            sampler2D _GabTexture;
            float4 _MainTex_ST;
            float _Size;
            float _TimeforSetting;
            float _Distortion;
            float _Blur;
         
            //VertexShader
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.grabUV = UNITY_PROJ_COORD(ComputeScreenPos(o.vertex));
                return o;
            }

            //make a random pattern
            float RandomBoxPattern(float2 p)
            {
                p = frac(p*float2(123.34f,345.45f));
                p += dot(p,p + 34.345f);
                return frac(p.x * p.y);
            }

            //make more drops by different layer
            float3 Layer(float2 UV,float t)
            {
                //Take the uv corner getting form VertexShader, and expose on the corner with parameter;
                float2 aspect = float2(2,1);
                float2 uv = UV *_Size * aspect;
                uv.y += t * 0.25f;

                //-0.5f set the origin to middle;
                float2 gv = frac(uv)-0.5f;

                //make id to each quad box
                float2 id = floor(uv);

                //make a drop path;
                float n = RandomBoxPattern(id);//for noise 0 or 1;
                t += n * 6.28341; //6.2831 = 2 PI
                float w = UV.y * 10;
                float x = (n - 0.5f) * 0.8f; //the velue between -0.4f~0.4f; 
                x += (0.4-abs(x)) * sin(3*w)*pow(sin(w),6) * 0.45f;//(0.4-abs(x))make sure the velue between x(-0.4f~0.4f);
                float y = -sin(t + sin(t + sin(t)*0.5f))*0.45f;
                y -= (gv.x-x)*(gv.x-x);
                float2 dropPos = (gv-float2(x,y)) /aspect;
                float drop = S(0.05f,0.03f,length(dropPos));

                //make a tail path
                float2 trailPos = (gv-float2(x,t *0.25f)) /aspect;
                trailPos.y = (frac(trailPos.y *6)-0.5f)/6;
                float trail = S(0.03f,0.01f,length(trailPos));
                //make trail drop slow down;
                float fogTrail = S(-0.05f,0.05f,dropPos.y);
                //make trail transparent when uv near by Y=1;
                trail *= S(0.5f,y,gv.y);
                trail *= fogTrail;
                fogTrail *= S(0.05f,0.04f,abs(dropPos.x));

                //col += fogTrail * 0.5f;
                //col += trail;
                //col += drop;
                //if(gv.x > 0.48f || gv.y >0.49f)col = float4(1,0,0,1);//print red lines 
                //col.rg = id *0.1f;
                //col = RandomBoxPattern(id);

                float2 offsetPos = drop*dropPos + trail*trailPos;

                return float3(offsetPos,fogTrail);
            }

            //Fragment Shader
            fixed4 frag (v2f i) : SV_Target
            {
                float timeOfYVelue = fmod(_Time.y +_TimeforSetting,7200);
                //Init return color;
                float4 col = 0;

                //more Layer
                float3 drops = Layer(i.uv,timeOfYVelue);
                drops += Layer(i.uv * 1.23 + 7.54 , timeOfYVelue);
                drops += Layer(i.uv * 1.35 + 1.54 , timeOfYVelue);
                drops += Layer(i.uv * 1.57 - 7.54 , timeOfYVelue);
                
                float fade = 1-saturate(fwidth(i.uv) * 60);
                float blur = _Blur * 3 * (1-drops.z * fade);
                col = tex2Dlod(_MainTex,float4(i.uv + drops.xy * _Distortion,0,blur));


                float2 projUV = i.grabUV.xy / i.grabUV.w;
                projUV += drops.xy * _Distortion * fade;
                blur *= 0.01;

                const float numSamples = 1;
                float a = RandomBoxPattern(i.uv) * 6.2831;
                for(float i =0 ; i < numSamples ; i++)
                {
                    float2 offsetBlur = float2(sin(a),cos(a))*blur;
                    float d = frac(sin((i+1)*546.0f)*5424.0f);
                    d = sqrt(d);
                    offsetBlur *= d;
                    col = tex2D(_GabTexture,projUV + offsetBlur);
                    a++;
                }
                col /= numSamples;
                
                
                return col*0.9f;
            }
            ENDCG
        }
    }
}