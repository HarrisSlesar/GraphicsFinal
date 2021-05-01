using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.XR;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class ShaderController : MonoBehaviour
{
    public int passNum;

    public Shader blendShader;
    public Shader blurShader;
    public Shader brightShader;
    

    private RenderTexture[] renderTextureList = new RenderTexture[10];

    [NonSerialized]
    private Material bright,blur,blend;


    private void Awake()
    {
        
        
    }



    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        CreateRenderTextures(Screen.width, Screen.height);
        if (bright == null)
        {
            bright = new Material(brightShader);
            bright.hideFlags = HideFlags.HideAndDontSave;
        }
        if (blend == null)
        {
            blend = new Material(blendShader);
            blend.hideFlags = HideFlags.HideAndDontSave;
        }
        if (blur == null)
        {
            blur = new Material(blurShader);
            blur.hideFlags = HideFlags.HideAndDontSave;
        }

        RenderTexture outputFBO = source;




        Graphics.Blit(outputFBO, renderTextureList[(int)BUFFERNAMES.fbo_c16_szHalf], bright);
        outputFBO = renderTextureList[(int)BUFFERNAMES.fbo_c16_szHalf];

        float x = 1.0f / outputFBO.width;
        blur.SetFloat("_AxisX", x);
        blur.SetFloat("_AxisY", 0.0f);
        Graphics.Blit(outputFBO, renderTextureList[(int)BUFFERNAMES.fbo_c16_szHalf + 1], blur);
        outputFBO = renderTextureList[(int)BUFFERNAMES.fbo_c16_szHalf + 1];

        float y = 1.0f / outputFBO.height;
        blur.SetFloat("_AxisX", 0.0f);
        blur.SetFloat("_AxisY", y);
        Graphics.Blit(outputFBO, renderTextureList[(int)BUFFERNAMES.fbo_c16_szHalf + 2], blur);
        outputFBO = renderTextureList[(int)BUFFERNAMES.fbo_c16_szHalf + 2];


        Graphics.Blit(outputFBO, renderTextureList[(int)BUFFERNAMES.fbo_c16_szQuarter], bright);
        outputFBO = renderTextureList[(int)BUFFERNAMES.fbo_c16_szQuarter];

        x = 1.0f / outputFBO.width;
        blur.SetFloat("_AxisX", x);
        blur.SetFloat("_AxisY", 0.0f);
        Graphics.Blit(outputFBO, renderTextureList[(int)BUFFERNAMES.fbo_c16_szQuarter + 1], blur);
        outputFBO = renderTextureList[(int)BUFFERNAMES.fbo_c16_szQuarter + 1];

        y = 1.0f / outputFBO.height;
        blur.SetFloat("_AxisX", 0.0f);
        blur.SetFloat("_AxisY", y);
        Graphics.Blit(outputFBO, renderTextureList[(int)BUFFERNAMES.fbo_c16_szQuarter + 2], blur);
        outputFBO = renderTextureList[(int)BUFFERNAMES.fbo_c16_szQuarter + 2];

        
        
        Graphics.Blit(outputFBO, renderTextureList[(int)BUFFERNAMES.fbo_c16_szEighth], bright);
        outputFBO = renderTextureList[(int)BUFFERNAMES.fbo_c16_szEighth];

        x = 1.0f / outputFBO.width;
        blur.SetFloat("_AxisX", x);
        blur.SetFloat("_AxisY", 0.0f);
        Graphics.Blit(outputFBO, renderTextureList[(int)BUFFERNAMES.fbo_c16_szEighth + 1], blur);
        outputFBO = renderTextureList[(int)BUFFERNAMES.fbo_c16_szEighth + 1];

        y = 1.0f / outputFBO.height;
        blur.SetFloat("_AxisX", 0.0f);
        blur.SetFloat("_AxisY", y);
        Graphics.Blit(outputFBO, renderTextureList[(int)BUFFERNAMES.fbo_c16_szEighth + 2], blur);
        outputFBO = renderTextureList[(int)BUFFERNAMES.fbo_c16_szEighth + 2];


        blend.SetTexture("_Texture0", source);
        blend.SetTexture("_Texture1", renderTextureList[(int)BUFFERNAMES.fbo_c16_szHalf + 2]);
        blend.SetTexture("_Texture2", renderTextureList[(int)BUFFERNAMES.fbo_c16_szQuarter + 2]);
        blend.SetTexture("_Texture3", renderTextureList[(int)BUFFERNAMES.fbo_c16_szEighth + 2]);
        Graphics.Blit(outputFBO, renderTextureList[(int)BUFFERNAMES.fbo_c16x4], blend);
        outputFBO = renderTextureList[(int)BUFFERNAMES.fbo_c16x4];



        switch (passNum)
        {
            case 0:
                Graphics.Blit(renderTextureList[(int)BUFFERNAMES.fbo_c16_szHalf], destination);
                break;
            case 1:
                Graphics.Blit(renderTextureList[(int)BUFFERNAMES.fbo_c16_szHalf+1], destination);
                break;
            case 2:
                Graphics.Blit(renderTextureList[(int)BUFFERNAMES.fbo_c16_szHalf+2], destination);
                break;
            case 3:
                Graphics.Blit(renderTextureList[(int)BUFFERNAMES.fbo_c16_szQuarter], destination);
                break;
            case 4:
                Graphics.Blit(renderTextureList[(int)BUFFERNAMES.fbo_c16_szQuarter+1], destination);
                break;
            case 5:
                Graphics.Blit(renderTextureList[(int)BUFFERNAMES.fbo_c16_szQuarter+2], destination);
                break;
            case 6:
                Graphics.Blit(renderTextureList[(int)BUFFERNAMES.fbo_c16_szEighth], destination);
                break;
            case 7:
                Graphics.Blit(renderTextureList[(int)BUFFERNAMES.fbo_c16_szEighth + 1], destination);
                break;
            case 8:
                Graphics.Blit(renderTextureList[(int)BUFFERNAMES.fbo_c16_szEighth + 2], destination);
                break;
            case 9:
                Graphics.Blit(renderTextureList[(int)BUFFERNAMES.fbo_c16x4], destination);
                break;
            default:
                Graphics.Blit(source, destination);
                break;

        }
        



    }



    void CreateRenderTextures(int width, int hight)
    {
        RenderTexture temp;
        for (int i = 0; i < 3; i++)
        {
            temp = new RenderTexture(width / 2, hight / 2,0);
            renderTextureList[(int)BUFFERNAMES.fbo_c16_szHalf + i] = temp;

            temp = new RenderTexture(width / 4, hight / 4, 0);
            renderTextureList[(int)BUFFERNAMES.fbo_c16_szQuarter + i] = temp;

            temp = new RenderTexture(width / 8, hight / 8, 0);
            renderTextureList[(int)BUFFERNAMES.fbo_c16_szEighth + i] = temp;
        }
        temp = new RenderTexture(width, hight, 0);
        renderTextureList[(int)BUFFERNAMES.fbo_c16x4] = temp;

    }

}


public enum BUFFERNAMES
{
    fbo_c16_szHalf,
    fbo_c16_szQuarter = 3,
    fbo_c16_szEighth = 6,
    fbo_c16x4 = 9
}