Shader "Custom/TileVertexColorInstanced"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }

        Pass
        {
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // REQUIRED for DrawMeshInstanced
            #pragma multi_compile_instancing
            #pragma instancing_options renderinglayer

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            float4 _BaseColor;

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_TRANSFER_INSTANCE_ID(IN, OUT);

                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);

                // Multiply vertex color by base color
                OUT.color = IN.color * _BaseColor;

                return OUT;
            }

            float4 frag (Varyings IN) : SV_Target
            {
                return IN.color;
            }

            ENDHLSL
        }
    }
}