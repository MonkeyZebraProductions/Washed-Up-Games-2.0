Shader "Unlit/USB_Shader_TAN"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)
        _Sections("Sections", Range(2, 100)) = 100
        _Speed("Speed", Range(0, 10)) = 1
        
        [Enum(UnityEngine.Rendering.BlendMode)]
        _SrcBlend("SrcFactor", Float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)]
        _DstBlend("DstFactor", Float) = 1
    }
    SubShader
    {
        Tags {"RenderType" = "Transparent" "Queue" = "Transparent"}
        Blend[_SrcBlend][_DstBlend]
        LOD 100

        Lighting Off
        

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "noiseSimplex.cginc"

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

            float4 _Color;
            float _Sections;
            float _Speed;

            uniform float _NoiseFrequency, _NoiseScale, _NoiseSpeed, _PixelOffset;

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
                float4 tanCol = clamp(0, abs(tan((i.uv.y - ((_Time.x) * _Speed)) * _Sections)), 1);
                tanCol *= _Color;
                fixed4 col = tex2D(_MainTex, i.uv) * tanCol;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
