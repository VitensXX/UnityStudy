using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

/// <summary>
/// Created by Vitens on 2021/10/21 20:06:52
/// 
/// Description : 
///     SDF贴图生成
/// </summary>
public class SDFImageMaker : MonoBehaviour
{
    class Pixel{
        public bool isIn;
        public float distance;
    }

    static Pixel[,] pixels;
    static Pixel[,] targetPixels;
    public static void GenerateSDF(Texture2D source, Texture2D destination, int serchDistance)
    {
        int sourceWidth = source.width;
        int sourceHeight = source.height;
        int targetWidth = destination.width;
        int targetHeight = destination.height;

        pixels = new Pixel[sourceWidth, sourceHeight];
        targetPixels = new Pixel[targetWidth, targetHeight];
        Debug.Log("sourceWidth" + sourceWidth);
        Debug.Log("sourceHeight" + sourceHeight);
        int x, y;
        Color targetColor = Color.white;
        for (y = 0; y < sourceWidth; y++)
        {
            for (x = 0; x < sourceHeight; x++)
            {
                pixels[x, y] = new Pixel();
                if (source.GetPixel(x, y) == targetColor)
                    pixels[x, y].isIn = true;
                else
                    pixels[x, y].isIn = false;
            }
        }

        int gapX = sourceWidth / targetWidth;
        int gapY = sourceHeight / targetHeight;
        int MAX_SEARCH_DIST = serchDistance;
        int minx, maxx, miny, maxy;
        float max_distance = -MAX_SEARCH_DIST;
        float min_distance = MAX_SEARCH_DIST;

        for (x = 0; x < targetWidth; x++)
        {
            for (y = 0; y < targetHeight; y++)
            {
                targetPixels[x, y] = new Pixel();
                int sourceX = x * gapX;
                int sourceY = y * gapY;
                int min = MAX_SEARCH_DIST;
                minx = sourceX - MAX_SEARCH_DIST;
                if (minx < 0)
                {
                    minx = 0;
                }
                miny = sourceY - MAX_SEARCH_DIST;
                if (miny < 0)
                {
                    miny = 0;
                }
                maxx = sourceX + MAX_SEARCH_DIST;
                if (maxx > (int)sourceWidth)
                {
                    maxx = sourceWidth;
                }
                maxy = sourceY + MAX_SEARCH_DIST;
                if (maxy > (int)sourceHeight)
                {
                    maxy = sourceHeight;
                }
                int dx, dy, iy, ix, distance;
                bool sourceIsInside = pixels[sourceX, sourceY].isIn;
                if (sourceIsInside)
                {
                    for (iy = miny; iy < maxy; iy++)
                    {
                        dy = iy - sourceY;
                        dy *= dy;
                        for (ix = minx; ix < maxx; ix++)
                        {
                            bool targetIsInside = pixels[ix, iy].isIn;
                            if (targetIsInside)
                            {
                                continue;
                            }
                            dx = ix - sourceX;
                            distance = (int)Mathf.Sqrt(dx * dx + dy);
                            if (distance < min)
                            {
                                min = distance;
                            }
                        }
                    }

                    if (min > max_distance)
                    {
                        max_distance = min;
                    }
                    targetPixels[x, y].distance = min;
                }
                else
                {
                    for (iy = miny; iy < maxy; iy++)
                    {
                        dy = iy - sourceY;
                        dy *= dy;
                        for (ix = minx; ix < maxx; ix++)
                        {
                            bool targetIsInside = pixels[ix, iy].isIn;
                            if (!targetIsInside)
                            {
                                continue;
                            }
                            dx = ix - sourceX;
                            distance = (int)Mathf.Sqrt(dx * dx + dy);
                            if (distance < min)
                            {
                                min = distance;
                            }
                        }
                    }

                    if (-min < min_distance)
                    {
                        min_distance = -min;
                    }
                    targetPixels[x, y].distance = -min;
                }
            }
        }

        //EXPORT texture
        float clampDist = max_distance - min_distance;
        for (x = 0; x < targetWidth; x++)
        {
            for (y = 0; y < targetHeight; y++)
            {
                targetPixels[x, y].distance -= min_distance;
                float value = targetPixels[x, y].distance / clampDist;
                destination.SetPixel(x, y, new Color(1, 1, 1, value));
            }
        }

        SaveTextureToFile(destination, "testSDF");
        AssetDatabase.Refresh();
    }

    static void SaveTextureToFile(Texture2D texture, string fileName)
    {
        // string path = "Assets/SDF";
        // byte[] bytes = texture.EncodeToPNG();
        // if (!Directory.Exists("Asset")){
        //     // Directory.CreateDirectory(contents);
        // }
        // FileStream file = File.Open(".png", FileMode.Create);
        // BinaryWriter writer = new BinaryWriter(file);
        // writer.Write(bytes);
        // file.Close();

        var bytes = texture.EncodeToPNG();
        var file = File.Open(Application.dataPath + "/SDF/"+fileName+".png",FileMode.Create);
        var binary= new BinaryWriter(file);
        binary.Write(bytes);
        file.Close();
    }

    public static void GenerateBinaryImage(Texture2D source){
        Texture2D target = new Texture2D(source.width, source.height);
        for (int x = 0; x < source.width; x++)
        {
            for (int y = 0; y < source.height; y++)
            {
                target.SetPixel(x,y, source.GetPixel(x,y).a > 0.1f ? Color.white : Color.black);
            }
        }

        SaveTextureToFile(target, "BI");
    }
}
