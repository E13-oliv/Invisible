// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
 
Shader "Custom/Pixelate New"
{
    Properties
    {
        _PixelSizeFar("Pixel Size Far", Float) = 10
        _PixelSizeClose("Pixel Size Close", Float) = 50
    }
 
        SubShader
    {
        Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" }
        Blend Off
        Lighting Off
        Fog{ Mode Off }
        ZWrite Off
        LOD 200
        Cull Off
 
        GrabPass{ "_GrabTexture" }
 
        Pass
    {
        CGPROGRAM
 
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
 
        struct v2f
    {
        float4 pos : SV_POSITION;
        float4 uv : TEXCOORD0;
 
        float4 posWorld : TEXCOORD1;
 
    };
 
    float _PixelSizeFar;
    float _PixelSizeClose;
 
    v2f vert(appdata_base v)
    {
        v2f o;
        o.pos = UnityObjectToClipPos(v.vertex);
        o.uv = ComputeGrabScreenPos(o.pos);
       
        o.posWorld = mul(unity_ObjectToWorld, v.vertex);
        return o;
    }
 
    sampler2D _GrabTexture;
 
    float4 frag(v2f IN) : COLOR
    {
        float2 steppedUV = IN.uv.xy / IN.uv.w;
 
        float cameraDist = length(IN.posWorld.xyz - _WorldSpaceCameraPos.xyz);
 
        steppedUV /= (_PixelSizeFar / cameraDist) / _ScreenParams.xy;
        steppedUV = round(steppedUV);
        steppedUV *= (_PixelSizeFar / cameraDist) / _ScreenParams.xy;
 
       
 
        return tex2D(_GrabTexture, steppedUV);
    }
 
        ENDCG
    }
    }
}