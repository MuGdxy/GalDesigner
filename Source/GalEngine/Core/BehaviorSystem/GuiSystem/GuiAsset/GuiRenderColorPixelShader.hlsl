#pragma pack_matrix(row_major) 

cbuffer RenderConfig : register(b0)
{
    float4 Color;
}

float4 main(
    float3 position : POSITION, 
    float2 texcoord : TEXCOORD,
    float4 projection : SV_POSITION) : SV_TARGET
{
    return Color;
}