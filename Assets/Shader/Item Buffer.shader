Shader "Custom/Item Buffer" {
	Properties {
        _Color ("Main Color", Color) = (1,0,0,1)
    }

    SubShader {
        Tags { "RenderType" = "Opaque" "Queue" = "Transparent" }
        Blend OneMinusDstColor One
        ZTest Always
        Cull Off
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha 

        Pass {
            Color [_Color]
        }
    }
   FallBack "Diffuse"
}
