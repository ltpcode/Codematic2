
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dotnetCHARTING;
using System.Drawing;

namespace Maticsoft.Common
{
    public delegate string SetXColumnHandler(string ora_Str);
    public class ChartHelper
    {
        public event SetXColumnHandler OnSetXColumn;
        public string SetXColumn(string ora_str)
        {
            if (OnSetXColumn != null)
            {
                return OnSetXColumn(ora_str);
            }
            return ora_str;
        }

        public void Create(Chart chart, string title, DataTable table, string xColumn, string yColumn, string style, bool user3D)
        {
            chart.Palette = new Color[] { Color.FromArgb(49, 255, 49), Color.FromArgb(255, 255, 0), Color.FromArgb(255, 99, 49), Color.FromArgb(0, 156, 255),
            Color.FromArgb(255, 125, 49), Color.FromArgb(125, 255, 49), Color.FromArgb(0, 255, 49) };
            chart.Use3D = user3D;
            SeriesCollection mySC = getRandomData(table, xColumn, yColumn);
            if (string.IsNullOrEmpty(style) || style == "线形")
            {
                chart.Type = ChartType.Combo;
                mySC = getRandomData2(table, xColumn, yColumn);
            }
            else if (style == "柱形")
            {
                chart.Type = ChartType.Combo;
            }
            else if (style == "金字塔")
            {
                chart.Type = ChartType.MultipleGrouped;
                chart.DefaultSeries.Type = SeriesTypeMultiple.Pyramid;
            }
            else if (style == "圆锥")
            {
                chart.Type = ChartType.MultipleGrouped;
                chart.DefaultSeries.Type = SeriesTypeMultiple.Cone;
            }
            chart.Title = title;
            if (string.IsNullOrEmpty(style) || style == "线形")
            {
                chart.DefaultSeries.Type = SeriesType.Line;
            }
          
            chart.DefaultElement.ShowValue = true;
            chart.PieLabelMode = PieLabelMode.Outside;
            chart.ShadingEffectMode = ShadingEffectMode.Three;
            chart.NoDataLabel.Text = "没有数据显示";
            chart.SeriesCollection.Add(mySC);
        }
        public void Create(Chart chart, string title, List<DataTable> tables, List<DateTime> dates, string xColumn, string yColumn, string style, bool user3D,string targetUrl)
        {
            chart.Palette = new Color[] { Color.FromArgb(49, 255, 49), Color.FromArgb(255, 255, 0), Color.FromArgb(255, 99, 49), Color.FromArgb(0, 156, 255),
            Color.FromArgb(255, 125, 49), Color.FromArgb(125, 255, 49), Color.FromArgb(0, 255, 49) };
            chart.Use3D = user3D;
            chart.Type = ChartType.Combo;
            chart.Title = title;
            chart.DefaultSeries.Type = SeriesTypeMultiple.Pyramid;
            SeriesCollection SC = new SeriesCollection();

            for (int i = 0; i < dates.Count; i++)
            {
                string dtStr = dates[i].ToString("yyyy-MM-dd");
                Series s = new Series(dtStr);               
                foreach (DataRow r in tables[i].Rows)
                {
                    Element e = new Element(r[xColumn].ToString());
                    e.URLTarget = "_self";
                    e.LegendEntry.URL = string.Concat(targetUrl, dtStr);
                    e.LegendEntry.URLTarget = "_self";
                    e.URL = string.Concat(targetUrl,dtStr);
                    e.YValue = Convert.ToDouble(r[yColumn]);
                    s.Elements.Add(e);
                    SC.Add(s);
                }
            }
            chart.DefaultElement.ShowValue = true;
            if (string.IsNullOrEmpty(style) || style == "线形")
            {
                chart.DefaultSeries.Type = SeriesType.Line;               
            }          
            chart.PieLabelMode = PieLabelMode.Outside;
            chart.ShadingEffectMode = ShadingEffectMode.Three;
            chart.NoDataLabel.Text = "没有数据显示";
            chart.SeriesCollection.Add(SC);

        }
        SeriesCollection getRandomData(DataTable table, string x,string y)
        {
            SeriesCollection SC = new SeriesCollection();
            foreach (DataRow r in table.Rows)
            {
                Series s = new Series(r[x].ToString());
                Element e = new Element(r[x].ToString());
                e.YValue = Convert.ToDouble(r[y]);
                s.Elements.Add(e);
                SC.Add(s);
            }
            return SC;
        }
        SeriesCollection getRandomData2(DataTable table, string x, string y)
        {
            SeriesCollection SC = new SeriesCollection();
            Series s = new Series();
            foreach (DataRow r in table.Rows)
            {
                Element e = new Element(r[x].ToString());
                e.YValue = Convert.ToDouble(r[y]);
                s.Elements.Add(e);
            }
            SC.Add(s);
            return SC;
        }

        public void Pie(dotnetCHARTING.Chart chart, int width, int height, string title,DataTable table, string xColumn, string yColumn)
        {

            SeriesCollection SC = new SeriesCollection();
            Series s = new Series("");
            DataView view = new DataView(table);
            view.Sort = yColumn + "  desc";
            int index = 0;
            DataTable table2 = view.ToTable();
            Element otherE = new Element("其他");

            bool other = false;
            double otherSum = 0;
            foreach (DataRow row in table2.Rows)
            {
                if (index > 9)
                {
                    otherSum += Convert.ToDouble(row[yColumn].ToString());
                    otherE.LabelTemplate = "%PercentOfTotal";
                    other = true;
                    continue;
                }
                string telType = row[xColumn].ToString();
                telType = SetXColumn(telType);
                Element e = new Element(telType);
                e.LabelTemplate = "%PercentOfTotal";

                e.YValue = Convert.ToDouble(row[yColumn].ToString());
                s.Elements.Add(e);
                index++;
            }
            if (other)
            {
                s.Elements.Add(otherE);
            }
            chart.TitleBox.Position = TitleBoxPosition.FullWithLegend;
            otherE.YValue = otherSum;
            SC.Add(s);
            chart.TempDirectory = "temp";
            chart.Use3D = false;
            chart.DefaultAxis.FormatString = "N";
            chart.DefaultAxis.CultureName = "zh-CN";
            chart.Palette = new Color[] { Color.FromArgb(49, 255, 49), Color.FromArgb(255, 255, 0), Color.FromArgb(255, 99, 49), Color.FromArgb(0, 156, 255)
            ,Color.FromArgb(255, 156, 255),Color.FromArgb(0, 156, 0),Color.FromArgb(0, 156, 99),Color.FromArgb(0, 99, 255),Color.FromArgb(99, 156, 255),
            Color.FromArgb(0, 0, 99),Color.FromArgb(0, 156, 126)};
            chart.DefaultElement.SmartLabel.AutoWrap = true;
            chart.Type = ChartType.Pies;
            chart.Size = width + "x" + height;
            chart.DefaultElement.SmartLabel.Text = "";
            chart.Title = title;
            chart.DefaultElement.ShowValue = true;
            chart.PieLabelMode = PieLabelMode.Outside;
            chart.ShadingEffectMode = ShadingEffectMode.Three;
            chart.DefaultElement.SmartLabel.AutoWrap = true;
            chart.NoDataLabel.Text = "没有数据显示";
            chart.SeriesCollection.Add(SC);
        }
        public void Create(dotnetCHARTING.Chart chart, string title, DataTable table, string xColumn, string yColumn, string style, int displayNum)
        {
            SeriesCollection SC = new SeriesCollection();          
            DataView view = new DataView(table);
            view.Sort = yColumn + "  desc";
            int index = 0;
            DataTable table2 = view.ToTable();
            Element otherE = new Element("其他");
            bool other = false;
            double otherSum = 0;
            Color c = Color.FromArgb(49, 255, 49);
            Random r = new Random(255);
            Color c1 = Color.FromArgb(255, 49, 255);
            List<Color> list = new List<Color>();
            list.Add(c);
            list.Add(c1);
            for (int i = 0; i < displayNum; i++)
            {
                Color cc = Color.FromArgb((c.A + r.Next(10000)) % 255, (c.B + r.Next(456)) % 255, (c.G + r.Next(1027)) % 100);
                list.Add(cc);
            }
            foreach (DataRow row in table2.Rows)
            {
                Series s = new Series("");
                if (index > displayNum - 2)
                {
                    otherSum += Convert.ToDouble(row[yColumn].ToString());
                    otherE.LabelTemplate = "%PercentOfTotal";
                    other = true;
                    continue;
                }

                string telType = row[xColumn].ToString();             
                telType = SetXColumn(telType);
                s.Name = telType;
                Element e = new Element(telType);
                e.LabelTemplate = "%PercentOfTotal";
                e.SmartLabel.Text = telType;


                e.YValue = Convert.ToDouble(row[yColumn].ToString());
                s.Elements.Add(e);
                index++;
                SC.Add(s);
            }
            if (other)
            {
                Series s = new Series("其他");
                s.Elements.Add(otherE);
                SC.Add(s);
            }
            otherE.YValue = otherSum;
            otherE.SmartLabel.Text = "其他";
      
            chart.TempDirectory = "temp";
            chart.Use3D = false;
            chart.DefaultAxis.FormatString = "N";
            chart.DefaultAxis.CultureName = "zh-CN";
            chart.Palette = list.ToArray();
            chart.DefaultElement.SmartLabel.AutoWrap = true;
            if (string.IsNullOrEmpty(style) || style == "线形")
            {
                chart.Type = ChartType.Combo;
                chart.DefaultSeries.Type = SeriesType.Line;
            }                  
            else if (style == "柱形")
            {
                chart.Type = ChartType.Combo;
            }
            else if (style == "横柱形")
            {
                chart.Type = ChartType.ComboHorizontal;
            }
            else if (style == "图片柱形")
            {
                chart.Type = ChartType.Combo;
                chart.DefaultSeries.ImageBarTemplate = "ethernetcable";
            }
            else if (style == "雷达")
            {
                chart.Type = ChartType.Radar;
            }
            else if (style == "圆锥")
            {
                chart.Type = ChartType.MultipleGrouped;
                chart.DefaultSeries.Type = SeriesTypeMultiple.Cone;
            }
            chart.DefaultElement.SmartLabel.Text = "";
            chart.Title = title;
            chart.DefaultElement.ShowValue = true;
            chart.PieLabelMode = PieLabelMode.Outside;
            chart.ShadingEffectMode = ShadingEffectMode.Three;
            chart.DefaultElement.SmartLabel.AutoWrap = true;
            chart.NoDataLabel.Text = "没有数据显示";
            chart.SeriesCollection.Add(SC);
        }
        public void Pie2(dotnetCHARTING.Chart chart, string title, DataTable table, string xColumn, string yColumn,string style,int displayNum)
        {
            SeriesCollection SC = new SeriesCollection();
            Series s = new Series("");
            DataView view = new DataView(table);
            view.Sort = yColumn + "  desc";
            int index = 0;
            DataTable table2 = view.ToTable();
            Element otherE = new Element("其他");
            bool other = false;
            double otherSum = 0;
            Color c = Color.FromArgb(49, 255, 49);
            Random r = new Random(255);
            Color c1 = Color.FromArgb(255, 49, 255);
            List<Color> list = new List<Color>();
            list.Add(c);
            list.Add(c1);
            for (int i = 0; i < displayNum; i++)
            {
                Color cc = Color.FromArgb((c.A + r.Next(10000)) % 255, (c.B + r.Next(456)) % 255, (c.G + r.Next(1027)) % 100);
                list.Add(cc);
            }
            foreach (DataRow row in table2.Rows)
            {
                if (index > displayNum - 2)
                {
                    otherSum += Convert.ToDouble(row[yColumn].ToString());
                    otherE.LabelTemplate = "%PercentOfTotal";
                    other = true;
                    continue;
                }
                string telType = row[xColumn].ToString();
                telType = SetXColumn(telType);
                Element e = new Element(telType);
                e.LabelTemplate = "%PercentOfTotal";
                e.SmartLabel.Text = telType;


                e.YValue = Convert.ToDouble(row[yColumn].ToString());
                s.Elements.Add(e);
                index++;
            }
            if (other)
            {
                s.Elements.Add(otherE);
            }
            otherE.YValue = otherSum;
            otherE.SmartLabel.Text = "其他";
            SC.Add(s);
            chart.TempDirectory = "temp";
            chart.Use3D = false;
            chart.DefaultAxis.FormatString = "N";
            chart.DefaultAxis.CultureName = "zh-CN";
            chart.Palette = list.ToArray();
            chart.DefaultElement.SmartLabel.AutoWrap = true;
            if (style == "饼形")
            {
                chart.Type = ChartType.Pies;
            }
            else if (style == "柱形")
            {
                chart.Type = ChartType.Combo;
            }
            else if (style == "横柱形")
            {
                chart.Type = ChartType.ComboHorizontal;
            }
            else if (style == "图片柱形")
            {
                chart.Type = ChartType.Combo;
                chart.DefaultSeries.ImageBarTemplate = "ethernetcable";
            }
            else if (style == "雷达")
            {
                chart.Type = ChartType.Radar;
            }
            else if (style == "圆锥")
            {
                chart.Type = ChartType.MultipleGrouped;
                chart.DefaultSeries.Type = SeriesTypeMultiple.Cone;
            }
            chart.DefaultElement.SmartLabel.Text = "";
            chart.Title = title;
            chart.DefaultElement.ShowValue = true;
            chart.PieLabelMode = PieLabelMode.Outside;
            chart.ShadingEffectMode = ShadingEffectMode.Three;
            chart.DefaultElement.SmartLabel.AutoWrap = true;
            chart.NoDataLabel.Text = "没有数据显示";
            chart.SeriesCollection.Add(SC);
        }
        public void Pie2(dotnetCHARTING.Chart chart, string title, DataTable table, string xColumn, string yColumn, string style, int displayNum, string targetUrl)
        {
            Pie2(chart, title, table, xColumn, yColumn, style, displayNum, targetUrl, "Jpg", "", false);
        }
        public void Pie2(dotnetCHARTING.Chart chart, string title, DataTable table, 
            string xColumn, string yColumn, string style,
            int displayNum,string targetUrl,string format,
            string legendBoxPos,bool user3d)
        {
            SeriesCollection SC = new SeriesCollection();
            Series s = new Series("");
            DataView view = new DataView(table);
            view.Sort = yColumn + "  desc";
            int index = 0;
            DataTable table2 = view.ToTable();
            Element otherE = new Element("其他");
            bool other = false;
            double otherSum = 0;
            Color c = Color.FromArgb(49, 255, 49);
            Random r = new Random(255);
            Color c1 = Color.FromArgb(255, 49, 255);
            List<Color> list = new List<Color>();
            list.Add(c);
            list.Add(c1);
            for (int i = 0; i < displayNum; i++)
            {
                Color cc = Color.FromArgb((c.A + r.Next(50000)) % 255, (c.B + r.Next(456)) % 255, (c.G + r.Next(1207)) % 100);
                list.Add(cc);
            }
            if (legendBoxPos.ToLower() == "title")
            {
                chart.TitleBox.Position = TitleBoxPosition.FullWithLegend;
            }
            foreach (DataRow row in table2.Rows)
            {
                if (index > displayNum)
                {
                    otherSum += Convert.ToDouble(row[yColumn].ToString());
                    otherE.LabelTemplate = "%Name: %PercentOfTotal";
                    other = true;
                    continue;
                }
                string telType = row[xColumn].ToString();
                telType = SetXColumn(telType);
                Element e = new Element(telType);
                e.ToolTip = telType;
                e.LabelTemplate = "%PercentOfTotal";
                e.LegendEntry.HeaderMode = LegendEntryHeaderMode.RepeatOnEachColumn;
                e.LegendEntry.SortOrder = 0;
                if (!string.IsNullOrEmpty(targetUrl))
                {
                    e.LegendEntry.URL = targetUrl + telType;
                    e.LegendEntry.URLTarget = "_self";
                    e.URL = targetUrl + telType;
                    e.URLTarget = "_self";
                }
                e.YValue = Convert.ToDouble(row[yColumn].ToString());
                s.Elements.Add(e);
                index++;
            }
            if (other)
            {
                s.Elements.Add(otherE);
            }
            otherE.YValue = otherSum;
            otherE.SmartLabel.Text = "其他";
            SC.Add(s);
            chart.TempDirectory = "temp";
            chart.Use3D = user3d;
            chart.DefaultAxis.FormatString = "N";
            chart.DefaultAxis.CultureName = "zh-CN";
            chart.Palette = list.ToArray();
            chart.DefaultElement.SmartLabel.AutoWrap = true;
            if (style == "饼形")
            {
                chart.Type = ChartType.Pies;
            }
            else if (style == "柱形")
            {
                chart.Type = ChartType.Combo;
            }
            else if (style == "横柱形")
            {
                chart.Type = ChartType.ComboHorizontal;
            }
            else if (style == "图片柱形")
            {
                chart.Type = ChartType.Combo;
                chart.DefaultSeries.ImageBarTemplate = "ethernetcable";
            }
            else if (style == "雷达")
            {
                chart.Type = ChartType.Radar;
            }
            else if (style == "圆锥")
            {
                chart.Type = ChartType.MultipleGrouped;
                chart.DefaultSeries.Type = SeriesTypeMultiple.Cone;
            }
            chart.Title = title;
            chart.PieLabelMode = PieLabelMode.Automatic;
            chart.DefaultElement.ShowValue = true;
            chart.ShadingEffectMode = ShadingEffectMode.Three;
            chart.LegendBox.DefaultEntry.PaddingTop = 5;
            switch (format)
            {
                case "Jpg":
                    {
                        chart.ImageFormat = ImageFormat.Jpg;
                        break;
                    }
                case "Png":
                    {
                        chart.ImageFormat = ImageFormat.Png;
                        break;
                    }
                case "Swf":
                    {
                        chart.ImageFormat = ImageFormat.Swf;
                        break;
                    }
            }
            chart.DefaultElement.SmartLabel.AutoWrap = true;
            chart.NoDataLabel.Text = "没有数据显示";
            chart.SeriesCollection.Add(SC);
        }


        public static void ComboHorizontal(dotnetCHARTING.Chart chart, int width, int height, string title, DataTable table, string xColumn, string yColumn)
        {
            SeriesCollection SC = new SeriesCollection();
            Series s = new Series();
            foreach (DataRow row in table.Rows)
            {
                string telType = row[xColumn].ToString();
                Element e = new Element();
                e.Name = telType;
                e.LabelTemplate = "%PercentOfTotal";
                e.YValue = Convert.ToDouble(row[yColumn].ToString());
                s.Elements.Add(e);
            }
            SC.Add(s);
            chart.TempDirectory = "temp";
            chart.Use3D = false;
            chart.DefaultAxis.Interval = 10;
            chart.DefaultAxis.CultureName = "zh-CN";
            chart.Palette = new Color[] { Color.FromArgb(49, 255, 49), Color.FromArgb(255, 255, 0), Color.FromArgb(255, 99, 49), Color.FromArgb(0, 156, 255) };
            chart.DefaultElement.SmartLabel.AutoWrap = true;
            chart.Type = ChartType.ComboHorizontal;
            chart.Size = width + "x" + height;
            chart.DefaultElement.SmartLabel.Text = "";
            chart.Title = title;
            chart.DefaultElement.ShowValue = true;
            chart.PieLabelMode = PieLabelMode.Outside;
            chart.ShadingEffectMode = ShadingEffectMode.Three;
            chart.NoDataLabel.Text = "没有数据显示";
            chart.SeriesCollection.Add(SC);
        }
    }
}
