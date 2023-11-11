Shader "Ocean City/Waving Tree URP" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
        _ShakeDisplacement ("Displacement", Range (0, 1.0)) = 1.0
        _ShakeTime ("Shake Time", Range (0, 1.0)) = 1.0
        _ShakeWindspeed ("Shake Windspeed", Range (0, 1.0)) = 1.0
        _ShakeBending ("Shake Bending", Range (0, 1.0)) = 1.0
    }
    
    SubShader {
        Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
        LOD 200
        Cull Off

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma exclude_renderers gles xbox360 ps3
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float4 color : COLOR;
            };

            fixed4 _Color;
            float _ShakeDisplacement;
            float _ShakeTime;
            float _ShakeWindspeed;
            float _ShakeBending;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.color = v.vertex;
                return o;
            }

            fixed4 frag(v2f i) : COLOR
            {
                // Your fragment shader logic here
                fixed4 col = _Color;
                return col;
            }
            ENDCG
        }
    }
    
    Fallback "Transparent/Cutout/VertexLit"
}
