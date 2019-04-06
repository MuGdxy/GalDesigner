#pragma pack_matrix(row_major) 

cbuffer RenderConfig : register(b0)
{
    float4 alpha;
};

Texture2D texture0 : register(t0);
SamplerState samplerState : register(s0);

float4 main(float4 projection : SV_POSITION, float2 texCoord : TEXCOORD) : SV_TARGET
{
    float4 color = texture0.Sample(samplerState, texCoord);

    return float4(color.xyz, color.a * alpha.a);
}