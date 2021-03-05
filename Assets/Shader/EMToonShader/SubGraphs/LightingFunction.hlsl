void MyFunction_float(float3 wpos, float3 wnormal, float3 viewDir, out float4 Color)
{
	Color = 0;
#if SHADERGRAPH_PREVIEW
	Color = 1;
#else
	half4 shadowCoord = TransformWorldToShadowCoord(wpos);
	ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
	half shadowStrength = GetMainLightShadowStrength();
	float ShadowAtten = SampleShadowmapFiltered(TEXTURE2D_SHADOW_ARGS(_MainLightShadowmapTexture, sampler_MainLightShadowmapTexture), shadowCoord, shadowSamplingData);

	Light mainLight = GetMainLight(shadowCoord);
	float3 mainLightDir = GetMainLight().direction;
	float mainLightAtten = ShadowAtten;
	float3 mainLightCol = mainLight.color;

	float spec = pow(max(0.0, dot(reflect(-mainLightDir, wnormal), viewDir)), 15)*0.00001;

	float3 ambient = SampleSH(wnormal);

	float3 Diffuse = (max(0,dot(mainLightDir, wnormal)) + spec)*mainLightCol*mainLightAtten+ambient;
	
	int lightCount = GetAdditionalLightsCount();
	float3 pixelLight = 0;
	for (int i = 0; i < lightCount; ++i)
	{
		Light pointlight = GetAdditionalLight(i, wpos);
		float3 lightCol = pointlight.color;
		float3 lightDir = pointlight.direction;
		float lightDot = max(0,dot(lightDir, wnormal));
		lightCol *= pointlight.distanceAttenuation*lightDot;
		pixelLight += lightCol;
	}
	Color.rgb = Diffuse + pixelLight;
#endif
}

void Shadows_float(float3 worldPos, out float Shadows)
{
#if SHADERGRAPH_PREVIEW
	Shadows = 1;
#else
	half4 shadowCoord = TransformWorldToShadowCoord(worldPos);
	ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
	half shadowStrength = GetMainLightShadowStrength();
	Shadows = SampleShadowmapFiltered(TEXTURE2D_SHADOW_ARGS(_MainLightShadowmapTexture, sampler_MainLightShadowmapTexture), shadowCoord, shadowSamplingData);
#endif
}

void PointLights_float(float3 worldPos, float3 worldNormals, out float3 PointLights)
{
#if SHADERGRAPH_PREVIEW
	PointLights = (0, 0, 0);
#else
	int lightCount = GetAdditionalLightsCount();
	float3 pixelLight = 0;
	for (int i = 0; i < lightCount; ++i)
	{
		Light pointlight = GetAdditionalLight(i, worldPos);
		float3 lightCol = pointlight.color;
		float3 lightDir = pointlight.direction;
		float lightDot = max(0, dot(lightDir, worldNormals));
		lightCol *= pointlight.distanceAttenuation*lightDot;
		PointLights += lightCol;
	}
#endif
}

void MainLight_float(out float3 LightDirection, out float3 LightColor)
{
#if SHADERGRAPH_PREVIEW
	LightDirection = (0.5, 0.5, 0);
	LightColor = (1, 0.95, 0.75);
#else
	Light mainLight = GetMainLight();
	LightDirection = GetMainLight().direction;
	LightColor = mainLight.color;
#endif
}

void AmbientLight_float(float3 WorldNormals, out float3 AmbientLight)
{
#if SHADERGRAPH_PREVIEW
	AmbientLight = (0, 0, 0);
#else
	AmbientLight = SampleSH(WorldNormals);
#endif
}