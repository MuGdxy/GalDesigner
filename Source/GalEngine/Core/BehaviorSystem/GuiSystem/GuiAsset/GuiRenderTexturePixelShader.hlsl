#pragma pack_matrix(row_major) 

cbuffer RenderConfig : register(b0)
{
    float4 Color;
}

Texture2D texture0 : register(t0);
SamplerState samplerState : register(s0);

float4 main(
    float3 position : POSITION,
    float2 texcoord : TEXCOORD,
    float4 projection : SV_POSITION) : SV_TARGET
{
    return texture0.Sample(samplerState, texcoord) * Color;
}