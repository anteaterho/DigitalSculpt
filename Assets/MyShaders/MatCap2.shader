// Shader created with Shader Forge v1.17 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.17;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|emission-7969-RGB;n:type:ShaderForge.SFN_Tex2d,id:7969,x:32371,y:32815,ptovrint:False,ptlb:node_7969,ptin:_node_7969,varname:node_7969,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c16cc709b3e51e74697c875af8cf5227,ntxv:0,isnm:False|UVIN-3027-OUT;n:type:ShaderForge.SFN_RemapRange,id:3027,x:32195,y:32803,varname:node_3027,prsc:2,frmn:-1,frmx:1,tomn:0,tomx:1|IN-9127-OUT;n:type:ShaderForge.SFN_ComponentMask,id:9127,x:32010,y:32778,varname:node_9127,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-6225-XYZ;n:type:ShaderForge.SFN_Transform,id:6225,x:31842,y:32789,varname:node_6225,prsc:2,tffrom:0,tfto:3|IN-8042-OUT;n:type:ShaderForge.SFN_NormalVector,id:8042,x:31612,y:32779,prsc:2,pt:False;proporder:7969;pass:END;sub:END;*/

Shader "Shader Forge/MatCap2" {
    Properties {
        _node_7969 ("node_7969", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _node_7969; uniform float4 _node_7969_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float3 normalDir : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float2 node_3027 = (mul( UNITY_MATRIX_V, float4(i.normalDir,0) ).xyz.rgb.rg*0.5+0.5);
                float4 _node_7969_var = tex2D(_node_7969,TRANSFORM_TEX(node_3027, _node_7969));
                float3 emissive = _node_7969_var.rgb;
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
