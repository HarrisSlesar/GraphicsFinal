//Blur shader referenced from Project 2
Shader "postBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AxisX ("_AxisX", Float) = 0.0
        _AxisY("_AxisY", Float) = 0.0

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

            //Uniforms
            sampler2D _MainTex;
            float4 _MainTex_ST;
            uniform float _AxisX; //X axis uniform
            uniform float _AxisY;//Y axis uniform

            //Weights and code referenced from https://learnopengl.com/Advanced-Lighting/Bloom
            float weight[5];

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            

            fixed4 frag(v2f i) : SV_Target
            {
                //filling the weight array
                weight[0] = 0.227027;
                weight[1] = 0.1945946;
                weight[2] = 0.1216216;
                weight[3] = 0.054054;
                weight[4] = 0.016216;
                float2 axis = float2(_AxisX, _AxisY); //Makes the axis vector
                
                // sample the texture
                float3 c = tex2D(_MainTex, i.texcoord).xyz * weight[0];

                //Code inspiration from https://learnopengl.com/Advanced-Lighting/Bloom

                //blurs the image on the axis
                for (int j = 1; j < 5; j++)
                {
                    c += tex2D(_MainTex, i.texcoord + float2(axis * j)).xyz * weight[j]; 
                    c += tex2D(_MainTex, i.texcoord - float2(axis * j)).xyz * weight[j];
                }
                

                return float4(c,1.0);
            }
            ENDCG
        }
    }
}
