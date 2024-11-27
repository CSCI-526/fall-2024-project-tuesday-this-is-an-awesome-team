Shader "Custom/DangerShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness ("Outline Thickness", Range(0.01, 0.1)) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
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
                float4 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _OutlineColor;
            float _OutlineThickness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float4 texColor = tex2D(_MainTex, uv);

                // 计算邻近像素采样，制造锯齿效果
                float outline = 0.0;
                float2 offsets[4] = {
                    float2(-_OutlineThickness, 0),
                    float2(_OutlineThickness, 0),
                    float2(0, -_OutlineThickness),
                    float2(0, _OutlineThickness)
                };

                for (int j = 0; j < 4; j++)
                {
                    float4 neighborColor = tex2D(_MainTex, uv + offsets[j]);
                    outline += step(0.1, 1.0 - neighborColor.a); // 检测透明度变化
                }

                // 如果邻近区域有明显变化，显示锯齿边框
                if (outline > 0.0)
                {
                    return _OutlineColor;
                }
                
                // 否则显示原始纹理
                return texColor;
            }
            ENDCG
        }
    }
}



