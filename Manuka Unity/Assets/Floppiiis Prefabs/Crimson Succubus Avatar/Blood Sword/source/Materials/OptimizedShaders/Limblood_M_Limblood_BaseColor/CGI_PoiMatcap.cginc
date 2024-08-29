#ifndef MATCAP
    #define MATCAP
    #if defined(PROP_MATCAP) || !defined(OPTIMIZER_ENABLED)
        UNITY_DECLARE_TEX2D_NOSAMPLER(_Matcap); float4 _Matcap_ST;
    #endif
    #if defined(PROP_MATCAPMASK) || !defined(OPTIMIZER_ENABLED)
        POI_TEXTURE_NOSAMPLER(_MatcapMask);
    #endif
    float _MatcapMaskInvert;
    float _MatcapBorder;
    float4 _MatcapColor;
    float _MatcapIntensity_Limblood_M_Limblood_BaseColor;
    float _MatcapReplace;
    float _MatcapMultiply;
    float _MatcapAdd;
    float _MatcapEnable;
    float _MatcapLightMask;
    float _MatcapEmissionStrength_Limblood_M_Limblood_BaseColor;
    float _MatcapNormal;
    float _MatcapHueShiftEnabled;
    float _MatcapHueShiftSpeed;
    float _MatcapHueShift;
    void blendMatcap(inout float4 finalColor, float add, float multiply, float replace, float4 matcapColor, float matcapMask, inout float3 matcapEmission, float emissionStrength
    #ifdef POI_LIGHTING
    , float matcapLightMask
    #endif
    #ifdef POI_BLACKLIGHT
    , uint blackLightMaskIndex
    #endif
    )
    {
        #ifdef POI_LIGHTING
            if (matcapLightMask)
            {
                matcapMask *= lerp(1, poiLight.rampedLightMap, matcapLightMask);
            }
        #endif
        #ifdef POI_BLACKLIGHT
            if(blackLightMaskIndex != 4)
            {
                matcapMask *= blackLightMask[blackLightMaskIndex];
            }
        #endif
        finalColor.rgb = lerp(finalColor.rgb, matcapColor.rgb, replace * matcapMask * matcapColor.a * .999999);
        finalColor.rgb *= lerp(1, matcapColor.rgb, multiply * matcapMask * matcapColor.a);
        finalColor.rgb += matcapColor.rgb * add * matcapMask * matcapColor.a;
        matcapEmission += matcapColor.rgb * emissionStrength * matcapMask * matcapColor.a;
    }
    void applyMatcap(inout float4 finalColor, inout float3 matcapEmission)
    {
        float4 matcap = 0;
        float matcapMask = 0;
        float4 matcap2 = 0;
        float matcap2Mask = 0;
        half3 worldViewUp = normalize(half3(0, 1, 0) - poiCam.viewDir * dot(poiCam.viewDir, half3(0, 1, 0)));
        half3 worldViewRight = normalize(cross(poiCam.viewDir, worldViewUp));
        half2 matcapUV = half2(dot(worldViewRight, poiMesh.normals[(1.0 /*_MatcapNormal*/)]), dot(worldViewUp, poiMesh.normals[(1.0 /*_MatcapNormal*/)])) * (0.363 /*_MatcapBorder*/) + 0.5;
        #if defined(PROP_MATCAP) || !defined(OPTIMIZER_ENABLED)
            matcap = UNITY_SAMPLE_TEX2D_SAMPLER(_Matcap, _MainTex, TRANSFORM_TEX(matcapUV, _Matcap)) * float4(0.04539379,0,0,1);
        #else
            matcap = float4(0.04539379,0,0,1);
        #endif
        matcap.rgb *= _MatcapIntensity_Limblood_M_Limblood_BaseColor;
        #if defined(PROP_MATCAPMASK) || !defined(OPTIMIZER_ENABLED)
            matcapMask = POI2D_SAMPLER_PAN(_MatcapMask, _MainTex, poiMesh.uv[(0.0 /*_MatcapMaskUV*/)], float4(0,0,0,0));
        #else
            matcapMask = 1;
        #endif
        if ((0.0 /*_MatcapMaskInvert*/))
        {
            matcapMask = 1 - matcapMask;
        }
        
        if((0.0 /*_MatcapHueShiftEnabled*/))
        {
            matcap.rgb = hueShift(matcap.rgb, (0.0 /*_MatcapHueShift*/) + _Time.x * (0.0 /*_MatcapHueShiftSpeed*/));
        }
        blendMatcap(finalColor, (0.0 /*_MatcapAdd*/), (0.0 /*_MatcapMultiply*/), (1.0 /*_MatcapReplace*/), matcap, matcapMask, matcapEmission, _MatcapEmissionStrength_Limblood_M_Limblood_BaseColor
        #ifdef POI_LIGHTING
        , (0.0 /*_MatcapLightMask*/)
        #endif
        #ifdef POI_BLACKLIGHT
        , _BlackLightMaskMatcap
        #endif
        );
    }
#endif
