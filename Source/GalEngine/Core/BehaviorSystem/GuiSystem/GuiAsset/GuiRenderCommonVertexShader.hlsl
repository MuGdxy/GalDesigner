#pragma pack_matrix(row_major) 

struct InputStruct
{
    float3 Position : POSITION;
    float2 Texcoord : TEXCOORD;
};

struct OutputStruct
{
    float3 Position : POSITION;
    float2 TexCoord : TEXCOORD;
    float4 Projects : SV_POSITION;
};

cbuffer Transform : register(b0)
{
    matrix World;
    matrix Project;
};

OutputStruct main(InputStruct input)
{
    OutputStruct pixel;

    pixel.Position = mul(float4(input.Position, 1.0f), World);
    pixel.Projects = mul(float4(pixel.Position, 1.0f), Project);
    pixel.TexCoord = input.Texcoord;

    return pixel;
}