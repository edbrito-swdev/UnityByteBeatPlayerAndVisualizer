Shader "Custom/SphereShader"
{
    Properties
    {
        [HDR] _Color("Color", Color) = (1,1,1,1)
        [HDR]_BackColor("Back Color", Color) = (1,1,1,1)
        _FrontTexture("Front Texture", 2D) = "white" {}
        _BackTexture("Back Texture", 2D) = "white" {}


        _DissolveMaskFront("Front Dissolve Texture" , 2D) = "white" {}
        _DissolveMaskBack("Back Dissolve Texture", 2D) = "white" {}
        _DissolveAmount("Dissolve Amount", Range(-1,1)) = 0
        _ScrollingDirection("Scrolling Direction", Vector) = (0,0,0,0)
        _ScrollingMultiplier("Scrolling Multiplier", Range(0,5)) = 1

        _ExtrudeAmount("Extrude Amount", Range(-1, 10)) = 0
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200
        Cull Back

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _FrontTexture;
        sampler2D _DissolveMaskFront;

        struct Input
        {
            float2 uv_FrontTexture;
            float2 uv_DissolveMaskFront;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float _DissolveAmount;
        float _ScrollingMultiplier;
        float4 _ScrollingDirection;
        float _ExtrudeAmount;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void vert(inout appdata_full v)
        {
            v.vertex.xyz += v.normal.xyz * _ExtrudeAmount;
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D(_FrontTexture, IN.uv_FrontTexture + frac((_ScrollingDirection.xy * _Time.yy))) * _Color;
            o.Albedo = c.rgb;

            float3 dissolveFront = tex2D(_DissolveMaskFront, IN.uv_DissolveMaskFront + frac((_ScrollingDirection.xy * _Time.yy)));//* (_Time.x * _ScrollingMultiplier));
            clip(dissolveFront.x - _DissolveAmount);

            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG

        Tags{ "RenderType" = "Opaque" }
        LOD 200
        Cull Front

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert
      
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
      
        sampler2D _BackTexture;
        sampler2D _DissolveMaskBack;
      
        struct Input
        {
            float2 uv_BackTexture;
            float2 uv_DissolveMaskBack;
        };
      
        half _Glossiness;
        half _Metallic;
        fixed4 _BackColor;
        float _DissolveAmount;
        float _ScrollingMultiplier;
        float4 _ScrollingDirection;
        float _ExtrudeAmount;
      
        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
        // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)
      
        void vert(inout appdata_full v)
        {
            v.vertex.xyz += v.normal.xyz * _ExtrudeAmount;
        }
      
        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D(_BackTexture, IN.uv_BackTexture + frac((_ScrollingDirection.xy * _Time.yy))) * _BackColor;
      
            float3 dissolveBack = tex2D(_DissolveMaskBack, IN.uv_DissolveMaskBack + frac((_ScrollingDirection.xy * _Time.yy))); //* (_Time.x * _ScrollingMultiplier));
            clip(dissolveBack.x - _DissolveAmount);
      
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
         }
      
       ENDCG
    }

   FallBack "Diffuse"
}
