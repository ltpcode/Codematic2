
using System;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Xml;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace LTPTextEditor
{
	/// <summary>
	/// The Syntax reader handles all font and color settings.
	/// </summary>
	public class SyntaxReader
	{
		#region Private Members
		private Settings _settings;
		bool			_hideStartPage = true;
		Color			_keyWordColor = Color.Blue;
		Color			_operatorColor = Color.Gray;
		Color			_compareColor = Color.PaleVioletRed;
		Color			_commentColor = Color.Green;
		Color			_stringColor = Color.Red;
		Color			_defaultColor = Color.Black;

		int			_oleKeyWordColor	= 16711680;
		int			_oleOperatorColor	= 8421504;
		int			_oleCompareColor	= 9662683;
		int			_oleCommentColor	= 32768;
		int			_oleStringColor		= 255;
		int			_oleDefaultColor	= -9999997;

//		string			_fontFamily = "Courier New";
//		GraphicsUnit	_fontGraphicsUnit = GraphicsUnit.Point;
//		float			_fontSize = 9;
//		FontStyle		_fontStyle	= FontStyle.Regular;

		bool			_runWithIOStatistics = true;
		int				_differencialPercentage =101;
		bool			_showFrmDocumentHeader = true;

		ArrayList Keywords = new ArrayList();
		ArrayList  Functions = new ArrayList();
		ArrayList  Operands = new ArrayList();
		ArrayList  Compares = new ArrayList();
		Hashtable sqlReservedWords = new Hashtable();

		#endregion
		#region Public Members
		public string ReservedWordsRegExPath=@"QUERYCOMMANDER\s|";
		public Font EditorFont;
		public XmlDocument xmlReservedWords;
		public XmlNodeList xmlNodeList;
		public Color KeyWordColor
		{
			get{return _keyWordColor;}
			set {_keyWordColor = value;}
		}
		public Color OperatorColor
		{
			get{return _operatorColor;}
			set {_operatorColor = value;}
		}
		public Color CompareColor
		{
			get{return _compareColor;}
			set {_compareColor = value;}
		}
		public Color CommentColor
		{
			get{return _commentColor;}
			set {_commentColor = value;}
		}
		public Color StringColor
		{
			get{return _stringColor;}
			set {_stringColor = value;}
		}
		public Color DefaultColor
		{
			get{return _defaultColor;}
			set {_defaultColor = value;}
		}
		public bool RunWithIOStatistics
		{
			get{return _runWithIOStatistics;}
			set {_runWithIOStatistics = value;}
		}
		public int DifferencialPercentage
		{
			get{return _differencialPercentage;}
			set {_differencialPercentage = value;}
		}
		public bool ShowFrmDocumentHeader
		{
			get{return _showFrmDocumentHeader;}
			set {_showFrmDocumentHeader = value;}
		}
	
		public bool HideStartPage
		{
			get{return _hideStartPage;}
			set {_hideStartPage = value;}
		}

		public	int color_keyword 
		{
			get{return _oleKeyWordColor;}
			set {_oleKeyWordColor = value;}
		}
		public	int color_operator 
		{
			get{return _oleOperatorColor;}
			set {_oleOperatorColor = value;}
		}
		public	int color_compare 
		{
			get{return _oleCompareColor;}
			set {_oleCompareColor = value;}
		}
		public	int color_default 
		{
			get{return _oleDefaultColor;}
			set {_oleDefaultColor = value;}
		}
		public	int color_comment 
		{
			get{return _oleCommentColor;}
			set {_oleCommentColor = value;}
		}
		public	int color_string 
		{
			get{return _oleStringColor;}
			set {_oleStringColor = value;}
		}

		#endregion
		#region Methods
		public bool IsReservedWord(string word)
		{
			if(word==null)
				return false;
			else
				return sqlReservedWords.Contains(word.ToUpper());
		}
		public SyntaxReader()
		{
			LoadXMLDocuments();
		}
		private void LoadXMLDocuments()
		{
			System.Reflection.Assembly thisExe;
			thisExe = System.Reflection.Assembly.GetExecutingAssembly();
			System.IO.Stream file = thisExe.GetManifestResourceStream("WindowsApplication1.QCTextEditor.SQLReservedWords.xml");
			xmlReservedWords = new XmlDocument();
			xmlReservedWords.Load(file);
			xmlNodeList = xmlReservedWords.GetElementsByTagName("SQLReservedWords");
			
			ArrayList remove = new ArrayList(); // for debug
			//Reserved Words
			
			foreach(XmlNode node in xmlNodeList[0].ChildNodes)
			{

				if(sqlReservedWords.Contains(node.Name))
					remove.Add(node.Name);
				else
				{
					sqlReservedWords.Add(node.Name, node.Attributes["type"].Value);
					ReservedWordsRegExPath+=node.Name+@"\s|";
				}
			}

			string filename = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "EditorSettings.config");
			XmlSerializer ser = new XmlSerializer(typeof(Settings));
			TextReader reader = new StreamReader(filename);
			_settings = (Settings)ser.Deserialize(reader);
			reader.Close();
			
			// Show StartPage
			_hideStartPage = _settings.HideStartPage;
			// Color
			_keyWordColor = Color.FromName(_settings.keyWordColor);
			_commentColor = Color.FromName(_settings.commentColor);
			_compareColor = Color.FromName(_settings.compareColor);
			_defaultColor = Color.FromName(_settings.defaultColor);
			_operatorColor = Color.FromName(_settings.operatorColor);
			_stringColor = Color.FromName(_settings.stringColor);

			// Ole Colors
			_oleCommentColor	= _settings.Ole_commentColor;
			_oleCompareColor	= _settings.Ole_compareColor;
			_oleDefaultColor	= _settings.Ole_defaultColor;
			_oleKeyWordColor	= _settings.Ole_keyWordColor;
			_oleOperatorColor	= _settings.Ole_operatorColor;
			_oleStringColor		= _settings.Ole_stringColor;

			// Font
			EditorFont = new Font(_settings.fontFamily,
								_settings.fontSize,
								_settings.fontStyle,
								_settings.fontGraphicsUnit);

			_differencialPercentage = _settings.DifferencialPercentage;
			_runWithIOStatistics = _settings.RunWithIOStatistics;
			_showFrmDocumentHeader = _settings.ShowFrmDocumentHeader;
			if(_differencialPercentage==0 && _runWithIOStatistics==false)
			{
				_runWithIOStatistics = true;
				_differencialPercentage = 101;
			}
		}
		public void FillArrays()
		{
//			StringReader sr = new StringReader(TheFile);
//			string nextLine;
//
//			nextLine = sr.ReadLine();
//			nextLine = nextLine.Trim();
//
//			// find functions header
//			while (nextLine != null)
//			{
//				if (nextLine == "[FUNCTIONS]")
//				{
//					// read all of the functions into the arraylist
//					nextLine = sr.ReadLine();
//					if (nextLine != null)
//						nextLine = nextLine.Trim();
//					while (nextLine != null && nextLine[0] != '[' 
//						)
//					{
//						Functions.Add(nextLine);
//
//						nextLine = "";
//						while (nextLine != null && nextLine == "")
//						{
//							nextLine = sr.ReadLine();
//							if (nextLine != null)
//								nextLine = nextLine.Trim();
//						}
//					}
//				}
//
//				if (nextLine == "[KEYWORDS]")
//				{
//					// read all of the functions into the arraylist
//					nextLine = sr.ReadLine();
//					if (nextLine != null)
//						nextLine = nextLine.Trim();
//					while (nextLine != null && nextLine[0] != '[' 
//						)
//					{
//						Keywords.Add(nextLine);
//
//						nextLine = "";
//						while (nextLine != null && nextLine == "")
//						{
//							nextLine = sr.ReadLine();
//							if (nextLine != null)
//								nextLine = nextLine.Trim();
//						}
//						
//					}
//				}
//
//				if (nextLine == "[COMMENTS]")
//				{
//					// read all of the functions into the arraylist
//					nextLine = sr.ReadLine();
//					if (nextLine != null)
//						nextLine = nextLine.Trim();
//					while (nextLine != null && nextLine[0] != '[' 
//						)
//					{
//						Comments.Add(nextLine);
//
//						nextLine = "";
//						while (nextLine != null && nextLine == "")
//						{
//							nextLine = sr.ReadLine();
//							if (nextLine != null)
//								nextLine = nextLine.Trim();
//						}
//						
//					}
//				}
//
//				if (nextLine != null && nextLine.Length > 0 && nextLine[0] == '[')
//				{
//				}
//				else
//				{
//					nextLine = sr.ReadLine();
//					if (nextLine != null)
//						nextLine = nextLine.Trim();
//				}
//			}
//
//			Keywords.Sort();
//			Functions.Sort();
//			Comments.Sort();
											
		}

		public bool IsKeyword(string s)
		{
			int index = Keywords.BinarySearch(s);
			if (index >= 0)
				return true;

			return false;
		}

		public bool IsFunction(string s)
		{
			int index = Functions.BinarySearch(s);
			if (index >= 0)
				return true;

			return false;
		}
		
		public int GetColorRef(string word)
		{
			if(!sqlReservedWords.Contains(word))
				return -9999997;

			string type = sqlReservedWords[word].ToString();
			switch(type)
			{
				case "keyword":
					return _oleKeyWordColor;// 16711680; //_keyWordColor;
				case "operator":
					return _oleOperatorColor;// 8421504; //_operatorColor;
				case "compare":
					return _oleCompareColor; // 9662683; //_compareColor;
				default:
					return _oleDefaultColor;// -9999997; //Color.Black;
			}
		}
		
		public Color GetColor(string word)
		{
			if(!sqlReservedWords.Contains(word))
				return Color.Black;;

			string type = sqlReservedWords[word].ToString();
			switch(type)
			{
				case "keyword":
					return _keyWordColor;
				case "operator":
					return _operatorColor;
				case "compare":
					return _compareColor;
				default:
					return Color.Black;
			}
		}

		public void Save(Font f)
		{
			_settings.keyWordColor = _keyWordColor.Name;
			_settings.operatorColor = _operatorColor.Name;
			_settings.compareColor = _compareColor.Name;
			_settings.commentColor = _commentColor.Name;
			_settings.stringColor = _stringColor.Name;
			_settings.defaultColor = _defaultColor.Name;

			_settings.Ole_commentColor = _oleCommentColor;
			_settings.Ole_compareColor = _oleCompareColor;
			_settings.Ole_defaultColor = _oleDefaultColor;
			_settings.Ole_keyWordColor = _oleKeyWordColor;
			_settings.Ole_operatorColor = _oleOperatorColor;
			_settings.Ole_stringColor = _oleStringColor;

			_settings.fontFamily = f.FontFamily.Name;
			_settings.fontGraphicsUnit = f.Unit;
			_settings.fontSize = f.Size;
			_settings.fontStyle = f.Style;

			_settings.RunWithIOStatistics = _runWithIOStatistics;
			_settings.DifferencialPercentage = _differencialPercentage;
			_settings.HideStartPage=_hideStartPage;
			_settings.ShowFrmDocumentHeader = _showFrmDocumentHeader;

			string filename = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "EditorSettings.config");
			XmlSerializer ser = new XmlSerializer(typeof(Settings));
			TextWriter writer = new StreamWriter(filename);
			ser.Serialize(writer, _settings);
			writer.Close();
			LoadXMLDocuments();
			
		}
		public void Save()
		{
			_settings.keyWordColor = _keyWordColor.Name;
			_settings.operatorColor = _operatorColor.Name;
			_settings.compareColor = _compareColor.Name;
			_settings.commentColor = _commentColor.Name;
			_settings.stringColor = _stringColor.Name;
			_settings.defaultColor = _defaultColor.Name;

			_settings.Ole_commentColor = _oleCommentColor;
			_settings.Ole_compareColor = _oleCompareColor;
			_settings.Ole_defaultColor = _oleDefaultColor;
			_settings.Ole_keyWordColor = _oleKeyWordColor;
			_settings.Ole_operatorColor = _oleOperatorColor;
			_settings.Ole_stringColor = _oleStringColor;

			_settings.RunWithIOStatistics = _runWithIOStatistics;
			_settings.DifferencialPercentage = _differencialPercentage;
			_settings.HideStartPage=_hideStartPage;
			_settings.ShowFrmDocumentHeader = _showFrmDocumentHeader;

			string filename = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "EditorSettings.config");
			XmlSerializer ser = new XmlSerializer(typeof(Settings));
			TextWriter writer = new StreamWriter(filename);
			ser.Serialize(writer, _settings);
			writer.Close();
			LoadXMLDocuments();
			
		}
		#endregion

		[Serializable]
		public class Settings 
		{
			// Show startpage
			public bool HideStartPage;
			// Color
			public string  keyWordColor;
			public string  operatorColor;
			public string  compareColor;
			public string  commentColor;
			public string  stringColor;
			public string  defaultColor;
			// Ole Color
			public int  Ole_keyWordColor;
			public int  Ole_operatorColor;
			public int  Ole_compareColor;
			public int  Ole_commentColor;
			public int  Ole_stringColor;
			public int  Ole_defaultColor;

			// Font
			public string fontFamily;
			public GraphicsUnit fontGraphicsUnit;
			public float fontSize;
			public FontStyle fontStyle;
			// Query settings
			public bool RunWithIOStatistics;
			public int  DifferencialPercentage;
			public bool  ShowFrmDocumentHeader;

		}
	}
}
