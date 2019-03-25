#pragma pack_matrix(row_major) 

cbuffer Transform : register(b0)
{
    matrix World;
    matrix projection;
}

struct Output
{
    float4 projection : SV_POSITION;
    float2 texcoord : TEXCOORD;
};

Output main(float3 position : POSITION, float2 texcoord : TEXCOORD)
{
    Output result;

    result.projection = mul(float4(position, 1.0f), World);
    result.projection = mul(result.projection, projection);
    result.texcoord = texcoord;

    return result;
}