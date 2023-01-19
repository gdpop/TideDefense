Shader "TideDefense/WetnessSimulationShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _EvaporationSpeed ("Evaporation Speed", float) = 0.1
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

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _EvaporationSpeed;

            float _SegmentsProgress[12];

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the color of the buffer and evaporate a part of it
                float bufferWetness = tex2D(_MainTex, i.uv).r - _EvaporationSpeed;

                // New wetness by segment
                float index = floor(i.uv.x / (1.0 / _SegmentsProgress.Length));
                float segmentProgress = _SegmentsProgress[index];
                // float4 col = float4(index, index, index, 1);

                // Artificial value of the wave
                float segmentWetness = step(i.uv.y, segmentProgress);

                // New wetness is applied to the current wet zone
                float result = clamp(bufferWetness + segmentWetness, 0, 1);
                
                // Return color
                float4 col = float4(result, result, result, 1);
                return col;
            }
            ENDCG
        }
    }
}
