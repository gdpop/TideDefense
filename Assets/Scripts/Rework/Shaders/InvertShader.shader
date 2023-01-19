Shader "Unlit/TonFils"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _EvaporationSpeed ("Evaporation Speed", float) = 0.1
        _Value ("Value", float) = 0
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _EvaporationSpeed;
            float _Value;

            float _SegmentsValue[2];

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float bufferColor = tex2D(_MainTex, i.uv).r - _EvaporationSpeed;

                float value = step(i.uv.x, _Value);
                float result = clamp(bufferColor + value, 0, 1);
                float4 col = float4(result, result, result, 1);
                // float4 col = float4(value, value, value, 1);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
