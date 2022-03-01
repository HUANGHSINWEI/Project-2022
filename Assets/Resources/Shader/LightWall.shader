Shader "LightWall/lightWall"
{
	Properties
	{
		_Text("(Texture)",2D) = "White"{}
		_Color("(Color)",color)=(1,1,1,1)
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			// #pragma Type MethodName  (�򥻫ŧi ���A �禡�W��)
			#pragma vertex vertexFunc
			#pragma fragment fragmentFunc

			// �T�w�g�k �h�Ѧ� UnityCG.cginc ���禡
			#include "UnityCG.cginc"

			// �ݧڪ�input���ƻ��Ʈ榡 �ثe�u�����
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			// �ݧڪ�output���ƻ��Ʈ榡 �ثe�u����� SV_POSITION(�T�w�g�k �i�D�t�Χڮ��o�ӷ��¦��)
			struct v2f
			{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			//�W�h�ܼƫŧi �Ӧ��ϥ�
			float4 _Color;
			sampler2D _Text;

			//�N appdata����ƫ��A �ন v2f ����ƫ��A
			v2f vertexFunc(appdata IN)
			{
				v2f OUT;
				OUT.position = UnityObjectToClipPos(IN.vertex);
				OUT.uv = IN.uv;

				return OUT;
			}

			///�N v2f ����ƫ��A �ন ����W���C��
			fixed4 fragmentFunc(v2f IN) :SV_Target
			{
				fixed4 pixelColor = tex2D(_Text,IN.uv);
				pixelColor *= _Color;
				return pixelColor;
			}
			ENDCG
		}
	}
}
