using System;

namespace MyOrange
{
	/// <summary>
	/// 一个自定义属性类
	/// </summary>
	public class DBAttribute:Attribute
	{
		public DBAttribute(bool pkfield,string fieldtype,
			bool ignoreBlankAndZero)
		{
			this._PKField			=pkfield;
			this._FieldType			=fieldtype;
			this._IgnoreBlankAndZero=ignoreBlankAndZero;
		}
		public DBAttribute(bool pkfield)
		{
			this._PKField			=pkfield;
		}
		public DBAttribute(string fieldtype)
		{
			this._FieldType			=fieldtype;
		}
		public DBAttribute(string fieldtype,
			bool ignoreBlankAndZero)
		{
			
			this._FieldType			=fieldtype;
			this._IgnoreBlankAndZero=ignoreBlankAndZero;
		}
		public DBAttribute()
		{
			
		}
		private bool _PKField=false;
		/// <summary>
		///  是否是主键
		/// </summary>
		public bool PKField
		{
			get {return _PKField;}
			set {_PKField=value;}
		}

		private string _FieldType="";
		/// <summary>
		/// 数据库对应类型
		/// 
		/// </summary>
		public string FieldType
		{
			get {return _FieldType;}
			set {_FieldType=value;}
		}
		private bool _IgnoreBlankAndZero=true;
		public bool IgnoreBlankAndZero
		{
			get {return _IgnoreBlankAndZero;}
			set {_IgnoreBlankAndZero=value;}
		}


	}
}