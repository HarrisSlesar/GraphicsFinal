//Shader referenced from Project 2
Shader "postBlend"
{
    Properties
    {
        //Shader property uniforms
        _Texture0 ("_Texture0", 2D) = "white" {}
        _Texture1("_Texture0", 2D) = "white" {}
        _Texture2("_Texture0", 2D) = "white" {}
        _Texture3("_Texture0", 2D) = "white" {}
    }
    SubShader
    {
        Cull Off
        ZTest Always
        ZWrite Off
        Pass
        {
            Blend One One
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag


            #include "UnityCG.cginc"

             struct appdata
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            //Texture sampler Uniforms
            sampler2D _Texture0;
            float4 _Texture0_ST;
            sampler2D _Texture1;
            float4 _Texture1_ST;
            sampler2D _Texture2;
            float4 _Texture2_ST;
            sampler2D _Texture3;
            float4 _Texture3_ST;
          
       
            float exposure = 0.9;
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //Code referenced from https://learnopengl.com/Advanced-Lighting/Bloom
                float gamma = 2.2;

                //Samples all the textures
                float3 hdrColor = tex2D(_Texture0, i.texcoord).xyz;
                float3 blur2Col = tex2D(_Texture1, i.texcoord).xyz;
                float3 blur4Col = tex2D(_Texture2, i.texcoord).xyz;
                float3 blur8Col = tex2D(_Texture3, i.texcoord).xyz;

                //combines the textures from each sampler
                float3 color;
                color = 1.0 - (1.0 - hdrColor) * (1.0 - blur2Col) * (1.0 - blur4Col) * (1.0 - blur8Col);

                // tone mapping
                float3 result = float3(1.0,1.0,1.0) - exp(-color);
                //gamma correct       
                result = pow(result, float3(1.0 / gamma, 1.0 / gamma, 1.0 / gamma));
                
                return float4(result,1.0);
            }
            ENDCG
        }
    }
}
