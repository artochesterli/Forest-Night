// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SpiritLine_Test"
{
	Properties
	{
		_Speed("Speed", Vector) = (-0.5,0,0,0)
		_MainTexture("Main Texture", 2D) = "white" {}
		_Float0("Float 0", Float) = 2
		_Color0("Color 0", Color) = (0.5377358,0.5377358,0.5377358,1)
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit alpha:fade keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float _Float0;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform sampler2D _TextureSample0;
		uniform float2 _Speed;
		uniform sampler2D _MainTexture;
		uniform float4 _Color0;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float2 temp_output_7_0 = ( i.uv_texcoord + ( _Speed * _Time.y ) );
			float4 tex2DNode18 = tex2D( _MainTexture, temp_output_7_0 );
			o.Emission = ( _Float0 * ( tex2D( _TextureSample1, uv_TextureSample1 ) * tex2D( _TextureSample0, temp_output_7_0 ) * tex2DNode18 ) * tex2DNode18 * i.vertexColor * _Color0 ).rgb;
			o.Alpha = ( tex2DNode18.a * _Color0.a );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
-17.20755;32.60378;1421;898;73.36865;1139.819;1.716086;True;False
Node;AmplifyShaderEditor.Vector2Node;10;-242.8818,-359.2634;Float;True;Property;_Speed;Speed;0;0;Create;True;0;0;False;0;-0.5,0;-2.36,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TimeNode;27;-276.343,-42.88853;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;6.85473,-286.6191;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;33;-223.4901,-668.9779;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;7;257.3243,-399.7053;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;18;556.6044,-455.2988;Float;True;Property;_MainTexture;Main Texture;1;0;Create;True;0;0;False;0;621775a5e4f9a6042ad84f2410e0c40f;621775a5e4f9a6042ad84f2410e0c40f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;46;559.8297,-718.1843;Float;True;Property;_TextureSample0;Texture Sample 0;4;0;Create;True;0;0;False;0;5c2bea22c6306204bb4c1b358dedf25e;5c2bea22c6306204bb4c1b358dedf25e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;47;560.0676,-938.798;Float;True;Property;_TextureSample1;Texture Sample 1;5;0;Create;True;0;0;False;0;5c2bea22c6306204bb4c1b358dedf25e;5c2bea22c6306204bb4c1b358dedf25e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;12;774.6008,194.7322;Float;False;Property;_Color0;Color 0;3;0;Create;True;0;0;False;0;0.5377358,0.5377358,0.5377358,1;0.7830189,0.7830189,0.7830189,0.5960785;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;36;658.3279,-95.82574;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;42;1466.867,-947.9944;Float;True;Property;_Float0;Float 0;2;0;Create;True;0;0;False;0;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;1174.408,-768.2657;Float;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;1696.142,-57.86057;Float;True;5;5;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;1365.165,308.0938;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;43;2009.756,-140.2877;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;SpiritLine_Test;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;0;10;0
WireConnection;6;1;27;2
WireConnection;7;0;33;0
WireConnection;7;1;6;0
WireConnection;18;1;7;0
WireConnection;46;1;7;0
WireConnection;48;0;47;0
WireConnection;48;1;46;0
WireConnection;48;2;18;0
WireConnection;13;0;42;0
WireConnection;13;1;48;0
WireConnection;13;2;18;0
WireConnection;13;3;36;0
WireConnection;13;4;12;0
WireConnection;37;0;18;4
WireConnection;37;1;12;4
WireConnection;43;2;13;0
WireConnection;43;9;37;0
ASEEND*/
//CHKSM=7F42CF26D44055355450E25D4C284A57CC29E0FD