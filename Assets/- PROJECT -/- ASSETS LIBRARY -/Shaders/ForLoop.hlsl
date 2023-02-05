#ifndef FORLOOP_INCLUDED
#define FORLOOP_INCLUDED

void ForLoop_float(float3[] input, float3 worldPos, out float output)
{
	float result;
	for(i = 0; i < input.length; i++)
	{
		dist = 1 - (dist(input, worldPos) / 5);
		result += dist;
	}

	output = saturate(result);
}

#endif // FORLOOP_INCLUDED