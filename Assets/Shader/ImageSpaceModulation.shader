Shader "ImageSpaceModulation/Standard"
{
	Properties {
        [HideInInspector] _MainTex ("Base (RGB)", 2D) = "white" {}
        
        [Header(Modulation Blend Mode)]
        [KeywordEnum(Bailey (Add), Multiply, Screen, Overlay, Darken, Lighten)] _modulationBlendMode ("Blend Mode", int) = 0
        [Toggle] _automaticModulationIntensity ("Optimize for Perceived Brightness (Experimental)", int) = 0
        _modulationIntensity ("Modulation Intensity", Range (0, 1)) = 1.0
        
        [Header(Modulation Type)] 
        [KeywordEnum(None, Color, Brightness, Contrast, Saturation, Inverse Color, Inverse Brightness, Inverse Black and White, Noise)] _modulationType ("Type", int) = 1
        _modulationValueColor ("Color", Color) = (0,0,0,1)
        _modulationValueBrightness ("Brightness", float) = 1
        _modulationContrast ("Contrast", Range (0, 2)) = 1
        _modulationSaturation ("Saturation", Range (-1, 1)) = 0
        _noise ("Noise", 2D) = "white" {}
        _noiseScale ("Scale Noise", float) = 1
        
        [Space(10)] 
        [Header(Modulation Position)] 
        _modulationPositionX ("Modulation Position X", Range (0, 1)) = 0.5
        _modulationPositionY ("Modulation Position Y", Range (0, 1)) = 0.5
        
        [Space(10)] 
        [Header(Modulation Size)]
        [KeywordEnum(None, Linear, Gaussian)] _falloffType ("Falloff Type", int) = 1
        _kernelSize ("Gaussian Kernel Size", Range (0, 1)) = 0.4
        _size ("Size", Range (0, 5)) = 2.0 
    }
    
    SubShader {
        Pass {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
             
            #include "UnityCG.cginc"
             
            uniform sampler2D _MainTex;
            
            uniform int _modulationBlendMode;
            uniform int _automaticModulationIntensity;
            uniform float _modulationIntensity;
                 
            uniform int _modulationType;
            uniform float4 _modulationValueColor;
            uniform float _modulationValueBrightness;
            uniform float _modulationContrast;
            uniform float _modulationSaturation;
            uniform sampler2D _noise;
            uniform float _noiseScale;
            
            uniform float _modulationPositionX;
            uniform float _modulationPositionY;
            
            uniform int _falloffType;
            uniform float _kernelSize;
            uniform float _size;
            
            float Epsilon = 1e-10;
            
            float3 HUEtoRGB(in float H) {
                float R = abs(H * 6 - 3) - 1;
                float G = 2 - abs(H * 6 - 2);
                float B = 2 - abs(H * 6 - 4);
                return saturate(float3(R,G,B));
            }
 
            float3 RGBtoHCV(in float3 RGB) {
                float4 P = (RGB.g < RGB.b) ? float4(RGB.bg, -1.0, 2.0/3.0) : float4(RGB.gb, 0.0, -1.0/3.0);
                float4 Q = (RGB.r < P.x) ? float4(P.xyw, RGB.r) : float4(RGB.r, P.yzx);
                float C = Q.x - min(Q.w, Q.y);
                float H = abs((Q.w - Q.y) / (6 * C + Epsilon) + Q.z);
                return float3(H, C, Q.x);
            }
            
            float3 RGBtoHSL(in float3 RGB) {
                float3 HCV = RGBtoHCV(RGB);
                float L = HCV.z - HCV.y * 0.5;
                float S = HCV.y / (1 - abs(L * 2 - 1) + Epsilon);
                return float3(HCV.x, S, L);
            }
            
            float3 HSLtoRGB(in float3 HSL) {
                float3 RGB = HUEtoRGB(HSL.x);
                float C = (1 - abs(2 * HSL.z - 1)) * HSL.y;
                return (RGB - 0.5) * C + HSL.z;
            }
            
            float3 linearFalloff(float distance, float scaleFactor) {
                float aliasVal = smoothstep(_size / 2 * scaleFactor, 0, distance);
                return lerp(float3(0,0,0), float3(1,1,1), aliasVal);
            }
            
            float gaussianFalloff(float2 screenPoint) {
                float kernelSizeHalf = _kernelSize / 2;
                float kernelSizeHalfSquaredAndDoubled = 2 * kernelSizeHalf * kernelSizeHalf;
                return clamp((exp(-(((screenPoint.x * screenPoint.x) + (screenPoint.y * screenPoint.y)) / kernelSizeHalfSquaredAndDoubled))) / (3.14159265358 * kernelSizeHalfSquaredAndDoubled),0,1);
            }
            
            float calcPerceivedBrightness(float3 a) {
                return 0.299*a.r + 0.587*a.g + 0.114*a.b;
            }
            
            float3 blendColors(float3 colorB, float3 colorA, float falloff) {
            
                colorB = clamp(colorB, 0, 1);
                colorA = clamp(colorA, 0, 1);
            
                if (_modulationBlendMode == 1) {
                    colorA = colorA * colorB;
                } else if (_modulationBlendMode == 2) {
                    colorA = 1.0 - (1.0 - colorA) * (1.0 - colorB);
                }  else if (_modulationBlendMode == 3) {
                    colorA = colorA < .5 ? 2.0 * colorA * colorB : 1.0 - 2.0 * (1.0 - colorA) * (1.0 - colorB);
                } else if (_modulationBlendMode == 4) {
                    colorA = min(colorA, colorB);
                }  else if (_modulationBlendMode == 5) {
                    colorA = max(colorA, colorB);
                }
                                
                return ((colorA * _modulationIntensity) + colorB * (1 - _modulationIntensity)) * falloff + colorB * (1 - falloff);
                
            }
            
            float3 modulateColor(float3 color, float3 modulationColor, v2f_img i) {
            
                float scaleFactor = _ScreenParams.y / _ScreenParams.x;

                if (_falloffType == 0) {
                    float dis = sqrt(pow((_modulationPositionX - i.uv.x ), 2) + pow((_modulationPositionY * scaleFactor - (i.uv.y * scaleFactor)), 2));
                    float falloff = 1;
                    if (dis > _size * scaleFactor / 2) { falloff = 0; }
                    return blendColors(color, modulationColor, falloff);

                } else if (_falloffType == 1) {
                    float dis = sqrt(pow((_modulationPositionX - i.uv.x ), 2) + pow((_modulationPositionY * scaleFactor - (i.uv.y * scaleFactor)), 2));
                    return blendColors(color, modulationColor, linearFalloff(dis, scaleFactor));
                }
                
                float2 gaussianScreenPoint = float2((i.uv.x  - _modulationPositionX) / _size / scaleFactor * 2 , (i.uv.y - _modulationPositionY) /_size * 2);
                return blendColors(color, modulationColor, gaussianFalloff(gaussianScreenPoint));
            }
             
            float4 frag(v2f_img i) : COLOR {
                            
                float4 col = tex2D(_MainTex, i.uv);
                
                if (_automaticModulationIntensity == 1) {
                    _modulationIntensity = clamp(clamp(calcPerceivedBrightness(col.rgb), 0, 0.25) + _modulationIntensity, 0, 1);
                }
                
                if (_size > 0) {
                                    
                    if (_modulationType == 1 && _kernelSize > 0) {
                    
                        float3 rgb = modulateColor(col, _modulationValueColor.rgb, i);
                        return float4(rgb.r, rgb.g, rgb.b, 1); 
                        
                    } else if (_modulationType == 2 && _kernelSize > 0) {
                    
                        float3 hsl = RGBtoHSL(col.rgb);
                        hsl.b *=  _modulationValueBrightness;
                        float3 rgb = HSLtoRGB(hsl);
                        rgb = modulateColor(col, rgb, i);
                        return float4(rgb.r, rgb.g, rgb.b, 1);
                         
                    } else if (_modulationType == 3 && _kernelSize > 0) {  
                                          
                        float3 rgb = ((col.rgb - 0.5f) * max(_modulationContrast, 0)) + 0.5f;
                        rgb = modulateColor(col, rgb, i);
                        return float4(rgb.r, rgb.g, rgb.b, 1);
                        
                    }  else if (_modulationType == 4 && _kernelSize > 0) {
                                            
                        float3 hsl = RGBtoHSL(col.rgb);
                        hsl.g = clamp(hsl.g + _modulationSaturation, 0, 1);
                        float3 rgb = HSLtoRGB(hsl);
                        rgb = modulateColor(col, rgb, i);
                        return float4(rgb.r, rgb.g, rgb.b, 1);
                        
                    } else if (_modulationType == 5 && _kernelSize > 0) {
                    
                        float3 rgb = 1 - col;
                        rgb = modulateColor(col, rgb, i);
                        return float4(rgb.r, rgb.g, rgb.b, 1); 
                        
                    } else if (_modulationType == 6 && _kernelSize > 0) {
                    
                        float3 hsl = RGBtoHSL(col.rgb);
                        hsl.b = 1 - hsl.b;
                        float3 rgb = HSLtoRGB(hsl);
                        rgb = modulateColor(col, rgb, i);
                        return float4(rgb.r, rgb.g, rgb.b, 1);
                        
                    } else if (_modulationType == 7 && _kernelSize > 0) {
                    
                        float3 hsl = RGBtoHSL(col.rgb);
                        float3 rgb = float3(1,1,1);
                        if (hsl.b > 0.3) {
                            rgb = float3(0,0,0);
                        }
                        rgb = modulateColor(col, rgb, i);
                        return float4(rgb.r, rgb.g, rgb.b, 1);
                    } else if (_modulationType == 8 && _kernelSize > 0) {
                    
                        float3 rgb = tex2D(_noise, i.uv * max(_noiseScale, 0));
                        rgb = modulateColor(col, rgb, i);
                        return float4(rgb.r, rgb.g, rgb.b, 1);
                    }
                    
                } 
               
                return col;
    
            }
            ENDCG
        }
    }
}