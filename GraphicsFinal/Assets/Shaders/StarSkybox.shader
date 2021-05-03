Shader "StarSkybox" {
    Properties{
       _Cube("Environment Map", Cube) = "white" {}
        _Cube0("Noise Map", Cube) = "white" {}
    }

        SubShader{
           Tags { "Queue" = "Background"  }

           Pass {
              ZWrite Off
              Cull Off

              CGPROGRAM
              #pragma vertex vert
              #pragma fragment frag

        // User-specified uniforms
        samplerCUBE _Cube;
        samplerCUBE _Cube0;
        struct vertexInput {
           float4 vertex : POSITION;
           float3 texcoord : TEXCOORD0;
        };

        struct vertexOutput {
           float4 vertex : SV_POSITION;
           float3 texcoord : TEXCOORD0;
        };





        vertexOutput vert(vertexInput input)
        {
           vertexOutput output;
           output.vertex = UnityObjectToClipPos(input.vertex);
           output.texcoord = input.texcoord;
           return output;
        }

        fixed4 frag(vertexOutput input) : COLOR
        {
            float4 color = texCUBE(_Cube, input.texcoord);
            float4 noise = texCUBE(_Cube0, input.texcoord);
            float4 returnColor = color;

            if (noise.a > 0)
            {
                returnColor = float4(0.0, 0.0, 0.0, 0.0);
            }

            return returnColor;
        }
        ENDCG
     }
    }
}