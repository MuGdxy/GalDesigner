#pragma pack_matrix(row_major) 

cbuffer RenderConfig : register(b0)
{
    float4 alpha;
};

Texture2D texture0 : register(t0);
SamplerState samplerState : register(s0);

float4 main(float4 projection : SV_POSITION, float2 texCoord : TEXCOORD) : SV_TARGET
{
    return float4(texture0.Sample(samplerState, texCoord).xyz, alpha.a);
}