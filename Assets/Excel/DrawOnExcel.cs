using NPOI.HSSF.Record;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static NPOI.HSSF.Util.HSSFColor;

public class DrawOnExcel : MonoBehaviour
{
    public int size;
    public Texture2D texture;

    private string filePath;
    private HSSFWorkbook wk;
    private FileStream fs;          //文件流

    private ISheet sheet;           //工作表
    private IRow row;               //行
    private ICell cell;             //列

    // Start is called before the first frame update
    void Start()
    {
    }

    [ContextMenu("save")]
    void Save()
    {
        Texture2D text = new Texture2D(256,256);
        //Color[] colors = texture.GetPixels();
        for (int i = 0; i < 256; i++)
        {
            for (int j = 0; j < 256; j++)
            {
                text.SetPixel(i,j, texture.GetPixel(i * 2, j * 2));
            }
        }
        text.Apply();
        // 编码纹理为PNG格式
        byte[] bytes = text.EncodeToPNG();
        string path = Application.dataPath + "/RawArt/test.png";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        // 将字节保存成图片，这个路径只能在PC端对图片进行读写操作
        File.WriteAllBytes(Application.dataPath + "/RawArt/test.png", bytes);
        #if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
        #endif
    }

    [ContextMenu("test")]
    void Test()
    {
        filePath = "E:/MyWork/test.xls";
        wk = new HSSFWorkbook();
        sheet = wk.CreateSheet("mySheet");

        HSSFWorkbook hssfWorkbook = new HSSFWorkbook();//工作簿实例
        HSSFPalette palette = hssfWorkbook.GetCustomPalette(); //调色板实例
        //palette.SetColorAtIndex((short)8, (byte)0, (byte)200, (byte)0);

        //PaletteRecord p = new PaletteRecord();
        PaletteRecord p = new PaletteRecord();


        //调色板实例
        //HSSFPalette palette = workbookAll.GetCustomPalette();
        //RGB颜色值，第一个值：8~64之间，后面三个值为RGB色值
        //palette.SetColorAtIndex((short)8, 179, 179, 179);
        //颜色实例
        //HSSFColor hSSFColor = palette.FindColor(179, 179, 179);
        //style.FillPattern = CellFillPattern.SOLID_FOREGROUND;
        //应用颜色到Style
        //style.FillForegroundColor = hSSFColor.GetIndex();

        //List<HSSFColor> colors = new List<HSSFColor>();
        Dictionary<int, ICellStyle> colDic = new Dictionary<int, ICellStyle>();

        for (int i = 0; i < size; i++)
        {
            row = sheet.CreateRow(i);
            for (int j = 0; j < size/4; j++)
            {
                cell = row.CreateCell(j);
                //cell.SetCellValue(i + "-" + j);

                Color col = texture.GetPixel(j * 4, size - i);
                //Color col = GetArvCol(j * 4, size - i);
                //if(col.a <= 0.1f)
                //{

                //}

                short similarColor = palette.FindSimilarColor((byte)(col.r * 255), (byte)(col.g * 255), (byte)(col.b * 255)).Indexed;
                //透明的地方用白色
                if (col.a <= 0.1f)
                {
                    similarColor = 9;
                }
                //Debug.LogError(similarColor.Indexed);
                if (!colDic.ContainsKey(similarColor))
                {
                    ICellStyle s = wk.CreateCellStyle();
                    colDic.Add(similarColor, s);
                    s.FillForegroundColor = similarColor;
                    s.FillPattern = FillPattern.SolidForeground;
                    cell.CellStyle = s;
                }
                else
                {
                    ICellStyle s = colDic[similarColor];
                    s.FillForegroundColor = similarColor;
                    s.FillPattern = FillPattern.SolidForeground;
                    cell.CellStyle = s;
                }

                //ICellStyle s = wk.CreateCellStyle();
                //s.FillForegroundColor = palette.FindSimilarColor((byte)(col.r * 255), (byte)(col.g * 255), (byte)(col.b * 255)).Indexed;
                //s.FillPattern = FillPattern.SolidForeground;
                //cell.CellStyle = s;
            }
        }

        fs = File.Create(filePath);
        wk.Write(fs);
        fs.Close();
        fs.Dispose();
        Debug.Log("创建表格成功");
    }

    Color GetArvCol(int j, int i)
    {
        Color col1 = texture.GetPixel(j * 4, size - i);
        Color col2 = texture.GetPixel(j * 4 + 1 , size - i);
        Color col3 = texture.GetPixel(j * 4 + 2, size - i);
        Color col4 = texture.GetPixel(j * 4 + 3, size - i);
        return (col1 + col2 + col3 + col4) /4 ;
    }

}