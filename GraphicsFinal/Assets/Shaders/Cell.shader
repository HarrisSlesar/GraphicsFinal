Shader "Unlit/Cell"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        
        [HDR]
        _AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)
        [HDR]
        _SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
        _Glossiness("Glossiness", Float) = 32
          [HDR]
        _RimColor("Rim Color", Color) = (1,1,1,1)
        _RimAmount("Rim Amount", Range(0, 1)) = 0.716
        _RimThreshold("Rim Threshold", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Tags { "LightMode" = "ForwardBase"
        "PassFlags" = "OnlyDirectional"}
        

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag


            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {

                float2 uv : TEXCOORD0;
                float3 viewDir : TEXCOORD1;

                float3 worldNormal : NORMAL;
                float4 vertex : SV_POSITION;

            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _Texture1;
            float4 _Texture1_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = WorldSpaceViewDir(v.vertex);


                return o;
            }

            float4 _AmbientColor;
            float _Glossiness;
            float4 _SpecularColor;
            float4 _RimColor;
            float _RimAmount;
            float _RimThreshold;

            fixed4 frag(v2f i) : SV_Target
            {
                float3 lightpos = _WorldSpaceLightPos0;

                float3 normal = normalize(i.worldNormal);
                float NdotL = dot(lightpos, normal);
                float lightIntensity = smoothstep(0, 0.01, NdotL);

                float3 viewDir = normalize(i.viewDir);

                float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
                float NdotH = dot(normal, halfVector);

                

                float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
                float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
                float4 specular = specularIntensitySmooth * _SpecularColor;

                float4 rimDot = 1 - dot(viewDir, normal);
                float rimIntensity = rimDot * NdotL;
                rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
                float4 rim = rimIntensity * _RimColor;

                
                float4 light = lightIntensity * _LightColor0;

                float4 col = tex2D(_MainTex, i.uv);
                

                float4 lightFull = _AmbientColor + light + specular + rim;

                return col * lightFull;
            }
            ENDCG
        }
    }
}
