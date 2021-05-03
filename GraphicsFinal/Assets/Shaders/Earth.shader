Shader "Earth"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1) //The color of our object
        _Tex("Texture", 2D) = "white" {}
        _Tex1("Texture", 2D) = "white" {}
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
            sampler2D _Tex;
            float4 _Tex_ST;
            sampler2D _Tex1;
            float4 _Tex1_ST;
            uniform float4 _Color; //Use the above variables in here
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

            sampler2D _MainTex;

            fixed4 frag(v2f i) : SV_Target
            {
               float3 normalDirection = normalize(i.normal);
                float3 viewDirection = normalize(_WorldSpaceCameraPos - i.posWorld.xyz);

                float3 lightpos = float3(unity_4LightPosX0.x, unity_4LightPosY0.x, unity_4LightPosZ0.x);
                //float3 lightpos = _WorldSpaceLightPos0;
                float3 lightColor = unity_LightColor[0].rgb;
                //float3 lightColor = _LightColor0.rgb;


                float3 vert2LightSource = lightpos - i.posWorld.xyz;
                float oneOverDistance = 1.0 / length(vert2LightSource);
                float3 lightDirection = lightpos - i.posWorld.xyz * 1;

                float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb; //Ambient component
                float3 diffuseReflection = lightColor * max(0.0, dot(normalDirection, lightDirection)); //Diffuse component



                float3 color = diffuseReflection*tex2D(_Tex, i.texcoord);
                float3 color1 = (float3(1.0,1.0,1.0)-diffuseReflection)*tex2D(_Tex1, i.texcoord);



                //return float4(color, 1.0);
                return float4(color + color1, 1.0);

            }
            ENDCG
        }
    }
}