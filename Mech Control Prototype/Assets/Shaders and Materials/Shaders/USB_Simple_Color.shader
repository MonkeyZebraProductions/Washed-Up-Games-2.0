Shader "Unlit/USB_Simple_Color"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1, 1, 1, 1)
        [Space(20)]
        [Toggle] _Enable("Enable ?", Float) = 0

        [KeywordEnum(Off, Red, Blue)]
        _Options("Color Options", Float) = 0

        // declare drawer
        [Enum(Off, 0, Front, 1, Back, 2)]
        _Face("Face Culling", Float) = 0
        [Space(20)]
        [PowerSlider(3.0)]
        _Brightness("Brightness", Range(0.01, 1)) = 0.08
        [IntRange]
        _Samples("Samples", Range(0, 255)) = 100

        [Enum(UnityEngine.Rendering.BlendMode)]
        _SrcBlend("SrcFactor", Float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)]
        _DstBlend("DstFactor", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "RenderType" = "Transparent" }
        LOD 100
        Cull[_Face]

        
        Blend[_SrcBlend][_DstBlend]
        ZWrite On
        ZTest Greater
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #pragma shader_feature _ENABLE_ON
            #pragma multi_compile _OPTIONS_OFF _OPTIONS_RED _OPTIONS_BLUE

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
            float4 _Color;
            float _Brightness;
            int _Samples;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            //fixed4 frag (v2f i) : SV_Target
            //{
            //    // sample the texture
            //    fixed4 col = tex2D(_MainTex, i.uv);
            //    // apply fog
            //    UNITY_APPLY_FOG(i.fogCoord, col);
            //    return col*_Color;
            //}

            half4 frag(v2f i) : SV_Target
            {
                // sample the texture
                half4 col = tex2D(_MainTex, i.uv);
            // generate condition
            #if _OPTIONS_OFF
                #if _ENABLE_ON
                    return col;
                #else
                    return col*_Brightness*_Samples * _Color;
                #endif
            #elif _OPTIONS_RED
                return col * float4(1, 0, 0, 1);


            #elif _OPTIONS_BLUE
                return col * float4(0, 0, 1, 1);
            #endif
            }
            ENDCG
        }
    }
}
