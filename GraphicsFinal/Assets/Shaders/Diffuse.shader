Shader "Diffuse"
{
    Properties
    {
        [NoScaleOffset] _Tex("Texture", 2D) = "white" {}
    }
        SubShader
    {
        Pass
        {
            Tags {"LightMode" = "ForwardBase"}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

            //Uniform sampler
            sampler2D _Tex;
            float4 _Tex_ST;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float3 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
                float4 posWorld : TEXCOORD1;

            };

            v2f vert(appdata v)
            {
                v2f o;

                o.posWorld = mul(unity_ObjectToWorld, v.vertex); //Calculate the world position for our point
                o.normal = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz); //Calculate the normal
                o.pos = UnityObjectToClipPos(v.vertex); //And the position
                o.texcoord = TRANSFORM_TEX(v.texcoord, _Tex);

                return o;
            }


            fixed4 frag(v2f i) : SV_Target
            {
                float3 normalDirection = normalize(i.normal); //Normal
                float3 viewDirection = normalize(_WorldSpaceCameraPos - i.posWorld.xyz); //View direction

                float3 lightpos = float3(unity_4LightPosX0.x, unity_4LightPosY0.x, unity_4LightPosZ0.x); //Light position of point light
                //float3 lightpos = _WorldSpaceLightPos0; //Light position of directional light
                float3 lightColor = unity_LightColor[0].rgb; //Light color of point light
                //float3 lightColor = _LightColor0.rgb; //Light position of directional light

                //light direction
                float3 vert2LightSource = lightpos - i.posWorld.xyz;
                float oneOverDistance = 1.0 / length(vert2LightSource);
                float3 lightDirection = lightpos - i.posWorld.xyz * 1;

                
                float3 diffuseReflection = lightColor * max(0.0, dot(normalDirection, lightDirection)); //Diffuse component
                

                float3 color = diffuseReflection * tex2D(_Tex, i.texcoord);
                return float4(color, 1.0);
            }
            ENDCG
        }
        
    }
}