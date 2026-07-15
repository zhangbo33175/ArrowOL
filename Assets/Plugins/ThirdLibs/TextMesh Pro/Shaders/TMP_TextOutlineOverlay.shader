Shader "TextMeshPro/Mobile/TMP_TextOutlineOverlay"
{
    Properties
    {
        [HDR]_FaceColor("Face Color", Color) = (1,1,1,1)
        _FaceDilate("Face Dilate", Range(-1,1)) = 0

        [HDR]_OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth("Outline Thickness", Range(0,1)) = 0
        _OutlineSoftness("Outline Softness", Range(0,1)) = 0

        [HDR]_UnderlayColor("Border Color", Color) = (0,0,0,.5)
        _UnderlayOffsetX("Border OffsetX", Range(-10,10)) = 0
        _UnderlayOffsetY("Border OffsetY", Range(-10,10)) = 0
        _UnderlayDilate("Border Dilate", Range(-1,1)) = 0
        _UnderlaySoftness("Border Softness", Range(0,10)) = 0

        [HDR]_TopGold("Top Color", Color) = (1, 0.96, 0.55, 1)
        [HDR]_BottomGold("Bottom Color", Color) = (1, 0.62, 0.02, 1)

        _WeightNormal("Weight Normal", float) = 0
        _WeightBold("Weight Bold", float) = .5

        _MainTex("Font Atlas", 2D) = "white" {}
        _TextureWidth("Texture Width", float) = 512
        _TextureHeight("Texture Height", float) = 512
        _GradientScale("Gradient Scale", float) = 5
        _ScaleX("Scale X", float) = 1
        _ScaleY("Scale Y", float) = 1
        _PerspectiveFilter("Perspective Correction", Range(0, 1)) = 0.875
        _Sharpness("Sharpness", Range(-1,1)) = 0

        _VertexOffsetX("Vertex OffsetX", float) = 0
        _VertexOffsetY("Vertex OffsetY", float) = 0

        _ClipRect("Clip Rect", vector) = (-32767, -32767, 32767, 32767)
        _MaskSoftnessX("Mask SoftnessX", float) = 0
        _MaskSoftnessY("Mask SoftnessY", float) = 0

        _StencilComp("Stencil Comparison", Float) = 8
        _Stencil("Stencil ID", Float) = 0
        _StencilOp("Stencil Operation", Float) = 0
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255

        _CullMode("Cull Mode", Float) = 0
        _ColorMask("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull [_CullMode]
        ZWrite Off
        Lighting Off
        Fog { Mode Off }
        ZTest [unity_GUIZTestMode]
        Blend One OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature __ OUTLINE_ON
            #pragma shader_feature __ UNDERLAY_ON UNDERLAY_INNER

            #pragma multi_compile __ UNITY_UI_CLIP_RECT
            #pragma multi_compile __ UNITY_UI_ALPHACLIP

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            fixed4 _FaceColor;
            float _FaceDilate;
            fixed4 _OutlineColor;
            float _OutlineWidth;
            float _OutlineSoftness;

            fixed4 _UnderlayColor;
            float _UnderlayOffsetX;
            float _UnderlayOffsetY;
            float _UnderlayDilate;
            float _UnderlaySoftness;

            fixed4 _TopGold;
            fixed4 _BottomGold;

            float _WeightNormal;
            float _WeightBold;
            float _ScaleRatioA;
            float _GradientScale;
            float _ScaleX;
            float _ScaleY;
            float _PerspectiveFilter;
            float _Sharpness;

            float _VertexOffsetX;
            float _VertexOffsetY;
            float4 _ClipRect;
            float _MaskSoftnessX;
            float _MaskSoftnessY;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                fixed4 color : COLOR;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 faceColor : COLOR;
                fixed4 outlineColor : COLOR1;
                float4 texcoord0 : TEXCOORD0;
                half4 param : TEXCOORD1;
                half4 mask : TEXCOORD2;
                float2 uv : TEXCOORD3;
                float posY : TEXCOORD4;
                #if (UNDERLAY_ON | UNDERLAY_INNER)
                float4 texcoord1 : TEXCOORD5;
                half2 underlayParam : TEXCOORD6;
                #endif
            };

            v2f vert(appdata_t i)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(i);
                UNITY_TRANSFER_INSTANCE_ID(i, o);

                _ScaleRatioA = 1.0;

                float bold = step(i.texcoord1.y, 0);
                float4 vert = i.vertex;
                vert.x += _VertexOffsetX;
                vert.y += _VertexOffsetY;
                o.vertex = UnityObjectToClipPos(vert);

                o.posY = i.texcoord0.y;

                float2 pixelSize = o.vertex.w;
                pixelSize /= float2(_ScaleX, _ScaleY) * abs(mul((float2x2)UNITY_MATRIX_P, _ScreenParams.xy));

                float scale = rsqrt(dot(pixelSize, pixelSize));
                scale *= abs(i.texcoord1.y) * _GradientScale * (_Sharpness + 1);

                float weight = lerp(_WeightNormal, _WeightBold, bold) / 4.0;
                weight = (weight + _FaceDilate) * _ScaleRatioA * 0.5;

                float layerScale = scale;
                scale /= 1 + (_OutlineSoftness * _ScaleRatioA * scale);
                float bias = (0.5 - weight) * scale - 0.5;
                float outline = _OutlineWidth * _ScaleRatioA * 0.5 * scale;

                float opacity = i.color.a;
                #if (UNDERLAY_ON | UNDERLAY_INNER)
                opacity = 1.0;
                #endif

                fixed4 faceColor = fixed4(i.color.rgb, opacity) * _FaceColor;
                faceColor.rgb *= faceColor.a;

                fixed4 outlineColor = _OutlineColor;
                outlineColor.a *= opacity;
                outlineColor.rgb *= outlineColor.a;
                outlineColor = lerp(faceColor, outlineColor, sqrt(min(1.0, outline * 2)));

                #if (UNDERLAY_ON | UNDERLAY_INNER)
                layerScale /= 1 + ((_UnderlaySoftness * _ScaleRatioA) * layerScale);
                float layerBias = (.5 - weight) * layerScale - .5 - ((_UnderlayDilate * _ScaleRatioA) * .5 * layerScale);
                float x = -(_UnderlayOffsetX * _ScaleRatioA) * _GradientScale / _TextureWidth;
                float y = -(_UnderlayOffsetY * _ScaleRatioA) * _GradientScale / _TextureHeight;
                float2 layerOffset = float2(x, y);
                #endif

                float4 clampedRect = clamp(_ClipRect, -2e10, 2e10);
                float2 maskUV = (vert.xy - clampedRect.xy) / (clampedRect.zw - clampedRect.xy);

                o.faceColor = faceColor;
                o.outlineColor = outlineColor;
                o.texcoord0 = float4(i.texcoord0.x, i.texcoord0.y, maskUV.x, maskUV.y);
                o.param = half4(scale, bias - outline, bias + outline, bias);
                o.mask = half4(vert.xy * 2 - clampedRect.xy - clampedRect.zw, 0.25 / (0.25 * half2(_MaskSoftnessX, _MaskSoftnessY) + pixelSize.xy));
                o.uv = i.texcoord0;

                #if (UNDERLAY_ON || UNDERLAY_INNER)
                o.texcoord1 = float4(i.texcoord0 + layerOffset, i.color.a, 0);
                o.underlayParam = half2(layerScale, layerBias);
                #endif

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);

                half d = tex2D(_MainTex, i.texcoord0.xy).a * i.param.x;
                half face = saturate(d - i.param.w);

                fixed4 c = i.faceColor * face;

                #ifdef OUTLINE_ON
                half outlineShape = saturate(d - i.param.z);
                c = lerp(i.outlineColor, i.faceColor, outlineShape);
                c *= saturate(d - i.param.y);
                #endif

                // 真正生效的上下渐变
                half t = i.posY;
                fixed3 gradient = lerp(_BottomGold.rgb, _TopGold.rgb, t);
                c.rgb = lerp(c.rgb, gradient, face);

                #if UNDERLAY_ON
                d = tex2D(_MainTex, i.texcoord1.xy).a * i.underlayParam.x;
                c += float4(_UnderlayColor.rgb * _UnderlayColor.a, _UnderlayColor.a) * saturate(d - i.underlayParam.y) * (1 - c.a);
                #endif

                #if UNITY_UI_CLIP_RECT
                half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(i.mask.xy)) * i.mask.zw);
                c *= m.x * m.y;
                #endif

                #if (UNDERLAY_ON | UNDERLAY_INNER)
                c *= i.texcoord1.z;
                #endif

                #if UNITY_UI_ALPHACLIP
                clip(c.a - 0.001);
                #endif

                return c;
            }
            ENDCG
        }
    }
    CustomEditor "TMPOverlayShaderGUI"
}