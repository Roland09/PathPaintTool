// modified version
//
// original comment:
// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)
Shader "Hidden/TerrainTools/PathPaintTool/BrushPreview"
{
	Properties
	{
		_BrushColor("Brush Color", Color) = (0.5, 0.5, 1.0, 1.0)

	}
	SubShader
	{
		ZTest Always Cull Back ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		CGINCLUDE
		// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it uses non-square matrices
		#pragma exclude_renderers gles

		#include "UnityCG.cginc"
		#include "TerrainPreview.cginc"

		sampler2D _BrushTex;

		ENDCG

		Pass    // 0
		{
			//Name "TerrainPreviewProcedural"
			Name "BrushPreviewProcedural"

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			// make properties accessible
			float4 _BrushColor;

			struct v2f {
				float4 clipPosition : SV_POSITION;
				float3 positionWorld : TEXCOORD0;
				float3 positionWorldOrig : TEXCOORD1;
				float2 pcPixels : TEXCOORD2;
				float2 brushUV : TEXCOORD3;
			};

			v2f vert(uint vid : SV_VertexID)
			{
				// build a quad mesh, with one vertex per paint context pixel (pcPixel)
				float2 pcPixels = BuildProceduralQuadMeshVertex(vid);

				// compute heightmap UV and sample heightmap
				float2 heightmapUV = PaintContextPixelsToHeightmapUV(pcPixels);
				float heightmapSample = UnpackHeightmap(tex2Dlod(_Heightmap, float4(heightmapUV, 0, 0)));

				// compute brush UV
				float2 brushUV = PaintContextPixelsToBrushUV(pcPixels);

				// compute object position (in terrain space) and world position
				float3 positionObject = PaintContextPixelsToObjectPosition(pcPixels, heightmapSample);
				float3 positionWorld = TerrainObjectToWorldPosition(positionObject);

				v2f o;
				o.pcPixels = pcPixels;
				o.positionWorld = positionWorld;
				o.positionWorldOrig = positionWorld;
				o.clipPosition = UnityWorldToClipPos(positionWorld);
				o.brushUV = brushUV;
				return o;
			}


			float4 frag(v2f i) : SV_Target
			{
				float brushSample = UnpackHeightmap(tex2D(_BrushTex, i.brushUV));

			// out of bounds multiplier
			float oob = all(saturate(i.brushUV) == i.brushUV) ? 1.0f : 0.0f;

			// brush outline stripe
			float stripeWidth = 2.0f;       // pixels
			float stripeLocation = 0.2f;    // at 20% alpha
			float brushStripe = Stripe(brushSample, stripeLocation, stripeWidth);

			//float4 color = float4(0.5f, 0.5f, 1.0f, 1.0f) * saturate(brushStripe + 0.5f * brushSample);
			float4 color = _BrushColor * saturate(brushStripe + 0.5f * brushSample);
			color.a = _BrushColor.w * saturate(brushSample * 5.0f); // w = alpha
			return color * oob;
		}
		ENDCG
		}
	
	}

	Fallback Off
}
