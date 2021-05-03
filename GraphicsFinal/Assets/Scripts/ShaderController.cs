using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.XR;

public enum BUFFERNAMES
{
    fbo_c16_szHalf,
    fbo_c16_szQuarter = 3,
    fbo_c16_szEighth = 6,
    fbo_c16x4 = 9
}

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

    //Runs when image needs to be rendered
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        CreateRenderTextures(Screen.width, Screen.height); //Creates the render textures
        
        //Checks to see if the materials exist, if not it creates them
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

        RenderTexture outputFBO = source; //Sets the starting output to the input source
        
        //BRIGHT PASS HALF
        Graphics.Blit(outputFBO, renderTextureList[(int)BUFFERNAMES.fbo_c16_szHalf], bright); //renders the current output to the correct location with bright shader
        outputFBO = renderTextureList[(int)BUFFERNAMES.fbo_c16_szHalf]; //Sets new output to be the correct render texture

        //BLUR PASS HALF X
        float x = 1.0f / outputFBO.width; //Calculates x value
        blur.SetFloat("_AxisX", x); //Sets the uniforms of the shader
        blur.SetFloat("_AxisY", 0.0f);
        Graphics.Blit(outputFBO, renderTextureList[(int)BUFFERNAMES.fbo_c16_szHalf + 1], blur); //renders to the correct texture with blur shader
        outputFBO = renderTextureList[(int)BUFFERNAMES.fbo_c16_szHalf + 1]; //sets new output to correct render texture

        //BLUR PASS HALF Y
        float y = 1.0f / outputFBO.height; //Calculates Y value
        blur.SetFloat("_AxisX", 0.0f); //Sets the uniforms
        blur.SetFloat("_AxisY", y);
        Graphics.Blit(outputFBO, renderTextureList[(int)BUFFERNAMES.fbo_c16_szHalf + 2], blur); //Renders from output to correct texture with blur shader
        outputFBO = renderTextureList[(int)BUFFERNAMES.fbo_c16_szHalf + 2]; //Sets output to be correct render texture


        //BRIGHT PASS QUARTER
        Graphics.Blit(outputFBO, renderTextureList[(int)BUFFERNAMES.fbo_c16_szQuarter], bright); //renders the current output to the correct location with bright shader
        outputFBO = renderTextureList[(int)BUFFERNAMES.fbo_c16_szQuarter];//Sets output to be correct render texture

        //BLUR PASS QUARTER X
        x = 1.0f / outputFBO.width;
        blur.SetFloat("_AxisX", x);//Sets the uniforms of the shader
        blur.SetFloat("_AxisY", 0.0f);
        Graphics.Blit(outputFBO, renderTextureList[(int)BUFFERNAMES.fbo_c16_szQuarter + 1], blur);//Renders from output to correct texture with blur shader
        outputFBO = renderTextureList[(int)BUFFERNAMES.fbo_c16_szQuarter + 1];//Sets output to be correct render texture

        //BLUR PASS QUARTER Y
        y = 1.0f / outputFBO.height;
        blur.SetFloat("_AxisX", 0.0f);//Sets the uniforms of the shader
        blur.SetFloat("_AxisY", y);
        Graphics.Blit(outputFBO, renderTextureList[(int)BUFFERNAMES.fbo_c16_szQuarter + 2], blur);//Renders from output to correct texture with blur shader
        outputFBO = renderTextureList[(int)BUFFERNAMES.fbo_c16_szQuarter + 2];//Sets output to be correct render texture


        //BRIGHT PASS EIGHTH
        Graphics.Blit(outputFBO, renderTextureList[(int)BUFFERNAMES.fbo_c16_szEighth], bright);//renders the current output to the correct location with bright shader
        outputFBO = renderTextureList[(int)BUFFERNAMES.fbo_c16_szEighth];//Sets output to be correct render texture

        //BLUR PASS EIGHT X
        x = 1.0f / outputFBO.width;
        blur.SetFloat("_AxisX", x);//Sets the uniforms of the shader
        blur.SetFloat("_AxisY", 0.0f);
        Graphics.Blit(outputFBO, renderTextureList[(int)BUFFERNAMES.fbo_c16_szEighth + 1], blur);//Renders from output to correct texture with blur shader
        outputFBO = renderTextureList[(int)BUFFERNAMES.fbo_c16_szEighth + 1];//Sets output to be correct render texture

        //BLUR PASS EIGHTH Y
        y = 1.0f / outputFBO.height;
        blur.SetFloat("_AxisX", 0.0f);//Sets the uniforms of the shader
        blur.SetFloat("_AxisY", y);
        Graphics.Blit(outputFBO, renderTextureList[(int)BUFFERNAMES.fbo_c16_szEighth + 2], blur);//Renders from output to correct texture with blur shader
        outputFBO = renderTextureList[(int)BUFFERNAMES.fbo_c16_szEighth + 2];//Sets output to be correct render texture

        //FINAL COMPOSITE BLEND PASS

        //sets all the uniform sampler textures
        blend.SetTexture("_Texture0", source);
        blend.SetTexture("_Texture1", renderTextureList[(int)BUFFERNAMES.fbo_c16_szHalf + 2]);
        blend.SetTexture("_Texture2", renderTextureList[(int)BUFFERNAMES.fbo_c16_szQuarter + 2]);
        blend.SetTexture("_Texture3", renderTextureList[(int)BUFFERNAMES.fbo_c16_szEighth + 2]);

        Graphics.Blit(outputFBO, renderTextureList[(int)BUFFERNAMES.fbo_c16x4], blend);//Renders from output to correct texture with blend shader
        outputFBO = renderTextureList[(int)BUFFERNAMES.fbo_c16x4];//Sets output to be correct render texture


        //Switch statement that checks an int to see which pass to display. Sets destination to the correct texture
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


        ClearRenderTextures(); //Clears render texture array

    }


    //Creates render textures
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

    //Clears render texture array
    void ClearRenderTextures()
    {
        for(int i = 0; i < renderTextureList.Length; i++)
        {
            DestroyImmediate(renderTextureList[i]);
            renderTextureList[i] = null;
        }
    }
}

