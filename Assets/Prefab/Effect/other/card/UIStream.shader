Shader "Custom/UIStreamer"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        
        [Header(Streamer Settings)]
        _FlowColor ("Flow Color", Color) = (1, 1, 1, 1) // 流光颜色
        _FlowSpeed ("Speed", Range(0, 10)) = 3.0       // 速度
        _FlowWidth ("Width", Range(0, 1)) = 0.2        // 宽度
        _FlowAngle ("Angle (Tan)", Range(-5, 5)) = 0.5 // 倾斜角度
        _Interval ("Interval (Sec)", Float) = 2.0      // 循环间隔时间
        
        // UGUI 必需的属性，用于支持Mask和裁切
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _WriteMask ("Stencil Write Mask", Float) = 255
        _ReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_ReadMask]
            WriteMask [_WriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;

            // 流光参数
            fixed4 _FlowColor;
            float _FlowSpeed;
            float _FlowWidth;
            float _FlowAngle;
            float _Interval;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                OUT.color = v.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

                // --- 流光逻辑开始 ---
                
                // 1. 计算当前时间进度 (0 到 Interval 之间循环)
                float timeVal = _Time.y * _FlowSpeed;
                // 使用 fmod 制造循环，如果 Interval <= 0 则视为连续
                float loopTime = (_Interval > 0) ? fmod(timeVal, _Interval + 2.0) : timeVal; 
                // (+2.0 是为了留一段空白时间，不让光连得太紧)

                // 2. 计算流光的UV坐标 (考虑倾斜)
                // 核心算法：x + y * tan(angle)
                float uvPos = IN.texcoord.x + IN.texcoord.y * _FlowAngle;

                // 3. 计算流光条带的位置 (从 -1 移动到 2 左右以覆盖全图)
                float flowPos = loopTime - 0.5; 

                // 4. 计算当前像素距离流光中心的距离
                float diff = abs(uvPos - flowPos);

                // 5. 根据距离生成光强 (使用 smoothstep 制作边缘柔和的光条)
                // 如果在宽度范围内，则发光
                float highlight = smoothstep(_FlowWidth, 0, diff);

                // 6. 叠加颜色
                // 只有在原图有像素的地方才发光 (color.a)
                color.rgb += _FlowColor.rgb * highlight * _FlowColor.a * color.a;

                // --- 流光逻辑结束 ---

                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                return color;
            }
            ENDCG
        }
    }
}