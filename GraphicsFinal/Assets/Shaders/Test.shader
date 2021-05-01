Shader "Unlit/Test"
{
	Properties
	{
		_MainColor("Main Color", Color) = (1, 1, 1, 1)
	}

		SubShader
	{
	   Pass
	   {
		   CGPROGRAM
		   #pragma vertex vert
		   #pragma fragment frag

		   struct appdata
		   {
			   float4 vertex : POSITION;
		   };

		   struct v2f
		   {
			   float4 vertex : SV_POSITION;
		   };

		   float4 _MainColor;

		   v2f vert(appdata v)
		   {
			   v2f o;
			   o.vertex = UnityObjectToClipPos(v.vertex);
			   return o;
		   }

		   float4 frag(v2f i) : SV_Target
		   {
			   return _MainColor;
		   }

		   ENDCG
	   }
	}
}
