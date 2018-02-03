// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ConnoTaylor/WaterShader1"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.5
		#pragma surface surf Standard keepalpha noshadow exclude_path:deferred vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
		};


		float3 Random3 ( float3 c )
		{
			float fracMul = 512.0;float j = 4096.0*sin ( dot ( c, float3 ( 17.0, 59.4, 15.0 ) ) );float3 r;r.z = frac ( fracMul*j );j *= .125;r.x = frac ( fracMul*j );j *= .125;r.y = frac ( fracMul*j );return r - 0.5;
		}


		float3 Simplex3d ( float3 p )
		{
			float F3 = 0.3333333;float G3 = 0.1666667;float3 s = floor ( p + dot ( p, F3.xxx ) );float3 x = p - s + dot ( s,  G3.xxx );float3 e = step ( ( 0.0 ).xxx, x - x.yzx );float3 i1 = e*( 1.0 - e.zxy );float3 i2 = 1.0 - e.zxy*( 1.0 - e );float3 x1 = x - i1 + G3;float3 x2 = x - i2 + 2.0*G3;float3 x3 = x - 1.0 + 3.0*G3;float4 w, d;w.x = dot ( x, x );w.y = dot ( x1, x1 );w.z = dot ( x2, x2 );w.w = dot ( x3, x3 );w = max ( 0.6 - w, 0.0 );d.x = dot ( Random3 ( s ), x );d.y = dot ( Random3 ( s + i1 ), x1 );d.z = dot ( Random3 ( s + i2 ), x2 );d.w = dot ( Random3 ( s + 1.0 ), x3 );w *= w;w *= w;d *= w;return dot ( d, ( 52.0 ).xxx ).xxx;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertexNormal = v.normal.xyz;
			float3 ase_worldNormal = UnityObjectToWorldNormal( v.normal );
			float3 ase_worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
			float3x3 tangentToWorld = CreateTangentToWorldPerVertex( ase_worldNormal, ase_worldTangent, v.tangent.w );
			float3 tangentNormal31 = ase_vertexNormal;
			float3 modWorldNormal31 = (tangentToWorld[0] * tangentNormal31.x + tangentToWorld[1] * tangentNormal31.y + tangentToWorld[2] * tangentNormal31.z);
			float2 appendResult20 = (float2(float2( 0,0 ).x , ( unity_DeltaTime.x / 10.0 )));
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float3 temp_output_30_0 = Simplex3d(( float3( appendResult20 ,  0.0 ) + ( ase_worldPos / 1000.0 ) )*0.0)* 0.5 + 0.5;
			v.normal = ( modWorldNormal31 * ( temp_output_30_0 * 100.0 ) );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			o.Normal = cross( ddx( ase_worldPos ) , ddy( ase_worldPos ) );
			float2 appendResult20 = (float2(float2( 0,0 ).x , ( unity_DeltaTime.x / 10.0 )));
			float3 temp_output_30_0 = Simplex3d(( float3( appendResult20 ,  0.0 ) + ( ase_worldPos / 1000.0 ) )*0.0)* 0.5 + 0.5;
			float4 lerpResult7 = lerp( float4(0,0.8344827,1,0) , float4(0,0.4106999,0.6544118,0) , float4( temp_output_30_0 , 0.0 ));
			o.Albedo = lerpResult7.rgb;
			o.Smoothness = 0.2;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13801
1046;853;1798;792;2491.581;626.5969;2.525935;True;True
Node;AmplifyShaderEditor.RangedFloatNode;24;-2043.073,276.8696;Float;False;Constant;_Float2;Float 2;0;0;10;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.DeltaTime;18;-2056.726,105.1793;Float;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;26;-1908.073,536.8696;Float;False;Constant;_Float3;Float 3;0;0;1000;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.Vector2Node;21;-1669.073,31.86963;Float;False;Constant;_Vector0;Vector 0;0;0;0,0;0;3;FLOAT2;FLOAT;FLOAT
Node;AmplifyShaderEditor.WorldPosInputsNode;27;-2184.073,399.8696;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleDivideOpNode;19;-1758.073,183.8696;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleDivideOpNode;25;-1670.073,426.8696;Float;False;2;0;FLOAT3;0.0;False;1;FLOAT;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.DynamicAppendNode;20;-1471.073,170.8696;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SimpleAddOpNode;22;-1257.073,171.8696;Float;False;2;2;0;FLOAT2;0.0;False;1;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.RangedFloatNode;16;-844.0287,599.5735;Float;False;Constant;_Float1;Float 1;0;0;100;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.NormalVertexDataNode;13;-670.7149,293.6516;Float;False;0;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimplexNoiseNode;30;-1059.273,77.02123;Float;False;Simplex3D;2;0;FLOAT3;0,0,0;False;1;FLOAT;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.WorldPosInputsNode;1;-661,76;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WorldNormalVector;31;-469.3055,289.1264;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-689.6852,582.4243;Float;False;2;2;0;FLOAT3;0.0;False;1;FLOAT;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.ColorNode;5;-814,-196;Float;False;Constant;_Color0;Color 0;0;0;0,0.4106999,0.6544118,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.DdyOpNode;3;-390,175;Float;False;1;0;FLOAT3;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.DdxOpNode;2;-392,95;Float;False;1;0;FLOAT3;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.ColorNode;6;-562,-381;Float;False;Constant;_Color1;Color 1;0;0;0,0.8344827,1,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-253.5884,402.9893;Float;False;2;2;0;FLOAT3;0.0;False;1;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.CrossProductOpNode;4;-232,118;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.LerpOp;7;-239,-211;Float;False;3;0;COLOR;0.0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;12;-278.8191,-10.4347;Float;False;Constant;_Float0;Float 0;0;0;0.2;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;11;332.1346,25.66493;Float;False;True;3;Float;ASEMaterialInspector;0;0;Standard;ConnoTaylor/WaterShader1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;False;0;False;Opaque;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;0;15;10;25;False;0.5;False;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;19;0;18;1
WireConnection;19;1;24;0
WireConnection;25;0;27;0
WireConnection;25;1;26;0
WireConnection;20;0;21;0
WireConnection;20;1;19;0
WireConnection;22;0;20;0
WireConnection;22;1;25;0
WireConnection;30;0;22;0
WireConnection;31;0;13;0
WireConnection;15;0;30;0
WireConnection;15;1;16;0
WireConnection;3;0;1;0
WireConnection;2;0;1;0
WireConnection;14;0;31;0
WireConnection;14;1;15;0
WireConnection;4;0;2;0
WireConnection;4;1;3;0
WireConnection;7;0;6;0
WireConnection;7;1;5;0
WireConnection;7;2;30;0
WireConnection;11;0;7;0
WireConnection;11;1;4;0
WireConnection;11;4;12;0
WireConnection;11;12;14;0
ASEEND*/
//CHKSM=66CEF7077FB0C2D720B26981AC03082AB61A1DA7