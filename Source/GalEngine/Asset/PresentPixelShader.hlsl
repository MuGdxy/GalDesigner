#pragma pack_matrix(row_major) 

cbuffer RenderConfig : register(b0)
{
    float4 alpha;
}

Texture2D texture : register(t0);
SamplerState samplerState : register(s0);

float4 main(float2 texCoord : TEXCOORD) : SV_TARGET
{
    return float4(texture.Sample(samplerState, texCoord).xyz, alpha.a);
}