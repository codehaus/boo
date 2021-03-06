// $ANTLR 2.7.4rc1: "src/Boo.AntlrParser/booel.g" -> "BooExpressionLexer.cs"$

namespace Boo.AntlrParser
{
	// Generate header specific to lexer CSharp file
	using System;
	using Stream                          = System.IO.Stream;
	using TextReader                      = System.IO.TextReader;
	using Hashtable                       = System.Collections.Hashtable;
	using Comparer                        = System.Collections.Comparer;
	
	using TokenStreamException            = antlr.TokenStreamException;
	using TokenStreamIOException          = antlr.TokenStreamIOException;
	using TokenStreamRecognitionException = antlr.TokenStreamRecognitionException;
	using CharStreamException             = antlr.CharStreamException;
	using CharStreamIOException           = antlr.CharStreamIOException;
	using ANTLRException                  = antlr.ANTLRException;
	using CharScanner                     = antlr.CharScanner;
	using InputBuffer                     = antlr.InputBuffer;
	using ByteBuffer                      = antlr.ByteBuffer;
	using CharBuffer                      = antlr.CharBuffer;
	using Token                           = antlr.Token;
	using CommonToken                     = antlr.CommonToken;
	using SemanticException               = antlr.SemanticException;
	using RecognitionException            = antlr.RecognitionException;
	using NoViableAltForCharException     = antlr.NoViableAltForCharException;
	using MismatchedCharException         = antlr.MismatchedCharException;
	using TokenStream                     = antlr.TokenStream;
	using LexerSharedInputState           = antlr.LexerSharedInputState;
	using BitSet                          = antlr.collections.impl.BitSet;
	
	internal 	class BooExpressionLexer : antlr.CharScanner	, TokenStream
	 {
		public const int EOF = 1;
		public const int NULL_TREE_LOOKAHEAD = 3;
		public const int TIMESPAN = 4;
		public const int DOUBLE = 5;
		public const int LONG = 6;
		public const int ESEPARATOR = 7;
		public const int INDENT = 8;
		public const int DEDENT = 9;
		public const int COMPILATION_UNIT = 10;
		public const int PARAMETERS = 11;
		public const int PARAMETER = 12;
		public const int ELIST = 13;
		public const int DLIST = 14;
		public const int TYPE = 15;
		public const int CALL = 16;
		public const int STMT = 17;
		public const int BLOCK = 18;
		public const int FIELD = 19;
		public const int MODIFIERS = 20;
		public const int MODULE = 21;
		public const int LITERAL = 22;
		public const int LIST_LITERAL = 23;
		public const int UNPACKING = 24;
		public const int ABSTRACT = 25;
		public const int AND = 26;
		public const int AS = 27;
		public const int BREAK = 28;
		public const int CONTINUE = 29;
		public const int CALLABLE = 30;
		public const int CAST = 31;
		public const int CLASS = 32;
		public const int CONSTRUCTOR = 33;
		public const int DEF = 34;
		public const int ELSE = 35;
		public const int ENSURE = 36;
		public const int ENUM = 37;
		public const int EXCEPT = 38;
		public const int FAILURE = 39;
		public const int FINAL = 40;
		public const int FROM = 41;
		public const int FOR = 42;
		public const int FALSE = 43;
		public const int GET = 44;
		public const int GIVEN = 45;
		public const int IMPORT = 46;
		public const int INTERFACE = 47;
		public const int INTERNAL = 48;
		public const int IS = 49;
		public const int ISA = 50;
		public const int IF = 51;
		public const int IN = 52;
		public const int NOT = 53;
		public const int NULL = 54;
		public const int OR = 55;
		public const int OTHERWISE = 56;
		public const int OVERRIDE = 57;
		public const int PASS = 58;
		public const int NAMESPACE = 59;
		public const int PUBLIC = 60;
		public const int PROTECTED = 61;
		public const int PRIVATE = 62;
		public const int RAISE = 63;
		public const int RETURN = 64;
		public const int RETRY = 65;
		public const int SET = 66;
		public const int SELF = 67;
		public const int SUPER = 68;
		public const int STATIC = 69;
		public const int SUCCESS = 70;
		public const int TRY = 71;
		public const int TRANSIENT = 72;
		public const int TRUE = 73;
		public const int TYPEOF = 74;
		public const int UNLESS = 75;
		public const int VIRTUAL = 76;
		public const int WHEN = 77;
		public const int WHILE = 78;
		public const int YIELD = 79;
		public const int EOS = 80;
		public const int TRIPLE_QUOTED_STRING = 81;
		public const int ID = 82;
		public const int LPAREN = 83;
		public const int RPAREN = 84;
		public const int ASSIGN = 85;
		public const int LBRACK = 86;
		public const int COMMA = 87;
		public const int RBRACK = 88;
		public const int COLON = 89;
		public const int MULTIPLY = 90;
		public const int CMP_OPERATOR = 91;
		public const int ADD = 92;
		public const int SUBTRACT = 93;
		public const int BITWISE_OR = 94;
		public const int DIVISION = 95;
		public const int MODULUS = 96;
		public const int EXPONENTIATION = 97;
		public const int INCREMENT = 98;
		public const int DECREMENT = 99;
		public const int DOT = 100;
		public const int INT = 101;
		public const int DOUBLE_QUOTED_STRING = 102;
		public const int SINGLE_QUOTED_STRING = 103;
		public const int LBRACE = 104;
		public const int RBRACE = 105;
		public const int RE_LITERAL = 106;
		public const int LINE_CONTINUATION = 107;
		public const int SL_COMMENT = 108;
		public const int ML_COMMENT = 109;
		public const int WS = 110;
		public const int NEWLINE = 111;
		public const int ESCAPED_EXPRESSION = 112;
		public const int DQS_ESC = 113;
		public const int SQS_ESC = 114;
		public const int SESC = 115;
		public const int RE_CHAR = 116;
		public const int RE_ESC = 117;
		public const int ID_LETTER = 118;
		public const int DIGIT = 119;
		
		
	
	public override void uponEOF()
	{
		Error();
	}

	void Error()
	{		
		throw new SemanticException("Unterminated expression interpolation!", getFilename(), getLine(), getColumn());
	}
		public BooExpressionLexer(Stream ins) : this(new ByteBuffer(ins))
		{
		}
		
		public BooExpressionLexer(TextReader r) : this(new CharBuffer(r))
		{
		}
		
		public BooExpressionLexer(InputBuffer ib)		 : this(new LexerSharedInputState(ib))
		{
		}
		
		public BooExpressionLexer(LexerSharedInputState state) : base(state)
		{
			initialize();
		}
		private void initialize()
		{
			caseSensitiveLiterals = true;
			setCaseSensitive(true);
			literals = new Hashtable(100, (float) 0.4, null, Comparer.Default);
			literals.Add("public", 60);
			literals.Add("namespace", 59);
			literals.Add("break", 28);
			literals.Add("while", 78);
			literals.Add("otherwise", 56);
			literals.Add("raise", 63);
			literals.Add("typeof", 74);
			literals.Add("and", 26);
			literals.Add("failure", 39);
			literals.Add("not", 53);
			literals.Add("return", 64);
			literals.Add("pass", 58);
			literals.Add("from", 41);
			literals.Add("null", 54);
			literals.Add("def", 34);
			literals.Add("given", 45);
			literals.Add("protected", 61);
			literals.Add("retry", 65);
			literals.Add("when", 77);
			literals.Add("class", 32);
			literals.Add("except", 38);
			literals.Add("unless", 75);
			literals.Add("super", 68);
			literals.Add("set", 66);
			literals.Add("transient", 72);
			literals.Add("constructor", 33);
			literals.Add("interface", 47);
			literals.Add("is", 49);
			literals.Add("internal", 48);
			literals.Add("final", 40);
			literals.Add("yield", 79);
			literals.Add("or", 55);
			literals.Add("if", 51);
			literals.Add("success", 70);
			literals.Add("override", 57);
			literals.Add("as", 27);
			literals.Add("try", 71);
			literals.Add("enum", 37);
			literals.Add("isa", 50);
			literals.Add("for", 42);
			literals.Add("private", 62);
			literals.Add("false", 43);
			literals.Add("static", 69);
			literals.Add("abstract", 25);
			literals.Add("callable", 30);
			literals.Add("get", 44);
			literals.Add("continue", 29);
			literals.Add("cast", 31);
			literals.Add("else", 35);
			literals.Add("import", 46);
			literals.Add("in", 52);
			literals.Add("self", 67);
			literals.Add("ensure", 36);
			literals.Add("true", 73);
			literals.Add("virtual", 76);
		}
		
		override public Token nextToken()			//throws TokenStreamException
		{
			Token theRetToken = null;
tryAgain:
			for (;;)
			{
				Token _token = null;
				int _ttype = Token.INVALID_TYPE;
				resetText();
				try     // for char stream error handling
				{
					try     // for lexical error handling
					{
						switch ( LA(1) )
						{
						case 'A':  case 'B':  case 'C':  case 'D':
						case 'E':  case 'F':  case 'G':  case 'H':
						case 'I':  case 'J':  case 'K':  case 'L':
						case 'M':  case 'N':  case 'O':  case 'P':
						case 'Q':  case 'R':  case 'S':  case 'T':
						case 'U':  case 'V':  case 'W':  case 'X':
						case 'Y':  case 'Z':  case '_':  case 'a':
						case 'b':  case 'c':  case 'd':  case 'e':
						case 'f':  case 'g':  case 'h':  case 'i':
						case 'j':  case 'k':  case 'l':  case 'm':
						case 'n':  case 'o':  case 'p':  case 'q':
						case 'r':  case 's':  case 't':  case 'u':
						case 'v':  case 'w':  case 'x':  case 'y':
						case 'z':
						{
							mID(true);
							theRetToken = returnToken_;
							break;
						}
						case '0':  case '1':  case '2':  case '3':
						case '4':  case '5':  case '6':  case '7':
						case '8':  case '9':
						{
							mINT(true);
							theRetToken = returnToken_;
							break;
						}
						case '.':
						{
							mDOT(true);
							theRetToken = returnToken_;
							break;
						}
						case ':':
						{
							mCOLON(true);
							theRetToken = returnToken_;
							break;
						}
						case ',':
						{
							mCOMMA(true);
							theRetToken = returnToken_;
							break;
						}
						case '|':
						{
							mBITWISE_OR(true);
							theRetToken = returnToken_;
							break;
						}
						case '(':
						{
							mLPAREN(true);
							theRetToken = returnToken_;
							break;
						}
						case ')':
						{
							mRPAREN(true);
							theRetToken = returnToken_;
							break;
						}
						case '[':
						{
							mLBRACK(true);
							theRetToken = returnToken_;
							break;
						}
						case ']':
						{
							mRBRACK(true);
							theRetToken = returnToken_;
							break;
						}
						case '{':
						{
							mLBRACE(true);
							theRetToken = returnToken_;
							break;
						}
						case '}':
						{
							mRBRACE(true);
							theRetToken = returnToken_;
							break;
						}
						case '%':
						{
							mMODULUS(true);
							theRetToken = returnToken_;
							break;
						}
						case '*':
						{
							mMULTIPLY(true);
							theRetToken = returnToken_;
							break;
						}
						case '/':
						{
							mDIVISION(true);
							theRetToken = returnToken_;
							break;
						}
						case '!':  case '<':  case '>':
						{
							mCMP_OPERATOR(true);
							theRetToken = returnToken_;
							break;
						}
						case '=':
						{
							mASSIGN(true);
							theRetToken = returnToken_;
							break;
						}
						case '\t':  case '\n':  case '\r':  case ' ':
						{
							mWS(true);
							theRetToken = returnToken_;
							break;
						}
						case '\'':
						{
							mSINGLE_QUOTED_STRING(true);
							theRetToken = returnToken_;
							break;
						}
						default:
							if ((LA(1)=='+') && (LA(2)=='+'))
							{
								mINCREMENT(true);
								theRetToken = returnToken_;
							}
							else if ((LA(1)=='-') && (LA(2)=='-')) {
								mDECREMENT(true);
								theRetToken = returnToken_;
							}
							else if ((LA(1)=='+') && (true)) {
								mADD(true);
								theRetToken = returnToken_;
							}
							else if ((LA(1)=='-') && (true)) {
								mSUBTRACT(true);
								theRetToken = returnToken_;
							}
						else
						{
							if (LA(1)==EOF_CHAR) { uponEOF(); returnToken_ = makeToken(Token.EOF_TYPE); }
				else {throw new NoViableAltForCharException((char)LA(1), getFilename(), getLine(), getColumn());}
						}
						break; }
						if ( null==returnToken_ ) goto tryAgain; // found SKIP token
						_ttype = returnToken_.Type;
						returnToken_.Type = _ttype;
						return returnToken_;
					}
					catch (RecognitionException e) {
							throw new TokenStreamRecognitionException(e);
					}
				}
				catch (CharStreamException cse) {
					if ( cse is CharStreamIOException ) {
						throw new TokenStreamIOException(((CharStreamIOException)cse).io);
					}
					else {
						throw new TokenStreamException(cse.Message);
					}
				}
			}
		}
		
	public void mID(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = ID;
		
		mID_LETTER(false);
		{    // ( ... )*
			for (;;)
			{
				switch ( LA(1) )
				{
				case 'A':  case 'B':  case 'C':  case 'D':
				case 'E':  case 'F':  case 'G':  case 'H':
				case 'I':  case 'J':  case 'K':  case 'L':
				case 'M':  case 'N':  case 'O':  case 'P':
				case 'Q':  case 'R':  case 'S':  case 'T':
				case 'U':  case 'V':  case 'W':  case 'X':
				case 'Y':  case 'Z':  case '_':  case 'a':
				case 'b':  case 'c':  case 'd':  case 'e':
				case 'f':  case 'g':  case 'h':  case 'i':
				case 'j':  case 'k':  case 'l':  case 'm':
				case 'n':  case 'o':  case 'p':  case 'q':
				case 'r':  case 's':  case 't':  case 'u':
				case 'v':  case 'w':  case 'x':  case 'y':
				case 'z':
				{
					mID_LETTER(false);
					break;
				}
				case '0':  case '1':  case '2':  case '3':
				case '4':  case '5':  case '6':  case '7':
				case '8':  case '9':
				{
					mDIGIT(false);
					break;
				}
				default:
				{
					goto _loop3_breakloop;
				}
				 }
			}
_loop3_breakloop:			;
		}    // ( ... )*
		_ttype = testLiteralsTable(_ttype);
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	protected void mID_LETTER(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = ID_LETTER;
		
		{
			switch ( LA(1) )
			{
			case '_':
			{
				match('_');
				break;
			}
			case 'a':  case 'b':  case 'c':  case 'd':
			case 'e':  case 'f':  case 'g':  case 'h':
			case 'i':  case 'j':  case 'k':  case 'l':
			case 'm':  case 'n':  case 'o':  case 'p':
			case 'q':  case 'r':  case 's':  case 't':
			case 'u':  case 'v':  case 'w':  case 'x':
			case 'y':  case 'z':
			{
				matchRange('a','z');
				break;
			}
			case 'A':  case 'B':  case 'C':  case 'D':
			case 'E':  case 'F':  case 'G':  case 'H':
			case 'I':  case 'J':  case 'K':  case 'L':
			case 'M':  case 'N':  case 'O':  case 'P':
			case 'Q':  case 'R':  case 'S':  case 'T':
			case 'U':  case 'V':  case 'W':  case 'X':
			case 'Y':  case 'Z':
			{
				matchRange('A','Z');
				break;
			}
			default:
			{
				throw new NoViableAltForCharException((char)LA(1), getFilename(), getLine(), getColumn());
			}
			 }
		}
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	protected void mDIGIT(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = DIGIT;
		
		matchRange('0','9');
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mINT(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = INT;
		
		{ // ( ... )+
		int _cnt6=0;
		for (;;)
		{
			if (((LA(1) >= '0' && LA(1) <= '9')))
			{
				mDIGIT(false);
			}
			else
			{
				if (_cnt6 >= 1) { goto _loop6_breakloop; } else { throw new NoViableAltForCharException((char)LA(1), getFilename(), getLine(), getColumn());; }
			}
			
			_cnt6++;
		}
_loop6_breakloop:		;
		}    // ( ... )+
		{
			if ((LA(1)=='L'||LA(1)=='l'))
			{
				{
					switch ( LA(1) )
					{
					case 'l':
					{
						match('l');
						break;
					}
					case 'L':
					{
						match('L');
						break;
					}
					default:
					{
						throw new NoViableAltForCharException((char)LA(1), getFilename(), getLine(), getColumn());
					}
					 }
				}
				if (0==inputState.guessing)
				{
					_ttype = LONG;
				}
			}
			else {
				{
					{
						if (((LA(1)=='.'))&&(BooLexer.IsDigit(LA(2))))
						{
							{
								match('.');
								{ // ( ... )+
								int _cnt13=0;
								for (;;)
								{
									if (((LA(1) >= '0' && LA(1) <= '9')))
									{
										mDIGIT(false);
									}
									else
									{
										if (_cnt13 >= 1) { goto _loop13_breakloop; } else { throw new NoViableAltForCharException((char)LA(1), getFilename(), getLine(), getColumn());; }
									}
									
									_cnt13++;
								}
_loop13_breakloop:								;
								}    // ( ... )+
							}
							if (0==inputState.guessing)
							{
								_ttype = DOUBLE;
							}
						}
						else {
						}
						
					}
					{
						if ((LA(1)=='d'||LA(1)=='h'||LA(1)=='m'||LA(1)=='s'))
						{
							{
								switch ( LA(1) )
								{
								case 's':
								{
									match('s');
									break;
								}
								case 'h':
								{
									match('h');
									break;
								}
								case 'd':
								{
									match('d');
									break;
								}
								default:
									if ((LA(1)=='m') && (LA(2)=='s'))
									{
										match("ms");
									}
									else if ((LA(1)=='m') && (true)) {
										match('m');
									}
								else
								{
									throw new NoViableAltForCharException((char)LA(1), getFilename(), getLine(), getColumn());
								}
								break; }
							}
							if (0==inputState.guessing)
							{
								_ttype = TIMESPAN;
							}
						}
						else {
						}
						
					}
				}
			}
			
		}
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mDOT(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = DOT;
		
		match('.');
		{
			if (((LA(1) >= '0' && LA(1) <= '9')))
			{
				{ // ( ... )+
				int _cnt19=0;
				for (;;)
				{
					if (((LA(1) >= '0' && LA(1) <= '9')))
					{
						mDIGIT(false);
					}
					else
					{
						if (_cnt19 >= 1) { goto _loop19_breakloop; } else { throw new NoViableAltForCharException((char)LA(1), getFilename(), getLine(), getColumn());; }
					}
					
					_cnt19++;
				}
_loop19_breakloop:				;
				}    // ( ... )+
				if (0==inputState.guessing)
				{
					_ttype = DOUBLE;
				}
			}
			else {
			}
			
		}
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mCOLON(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = COLON;
		
		match(':');
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mCOMMA(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = COMMA;
		
		match(',');
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mBITWISE_OR(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = BITWISE_OR;
		
		match('|');
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mLPAREN(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = LPAREN;
		
		match('(');
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mRPAREN(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = RPAREN;
		
		match(')');
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mLBRACK(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = LBRACK;
		
		match('[');
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mRBRACK(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = RBRACK;
		
		match(']');
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mLBRACE(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = LBRACE;
		
		match('{');
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mRBRACE(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = RBRACE;
		
		match('}');
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mINCREMENT(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = INCREMENT;
		
		match("++");
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mDECREMENT(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = DECREMENT;
		
		match("--");
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mADD(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = ADD;
		
		{
			match('+');
		}
		{
			if ((LA(1)=='='))
			{
				match('=');
				if (0==inputState.guessing)
				{
					_ttype = ASSIGN;
				}
			}
			else {
			}
			
		}
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mSUBTRACT(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = SUBTRACT;
		
		{
			match('-');
		}
		{
			if ((LA(1)=='='))
			{
				match('=');
				if (0==inputState.guessing)
				{
					_ttype = ASSIGN;
				}
			}
			else {
			}
			
		}
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mMODULUS(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = MODULUS;
		
		match('%');
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mMULTIPLY(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = MULTIPLY;
		
		match('*');
		{
			switch ( LA(1) )
			{
			case '=':
			{
				match('=');
				if (0==inputState.guessing)
				{
					_ttype = ASSIGN;
				}
				break;
			}
			case '*':
			{
				match('*');
				if (0==inputState.guessing)
				{
					_ttype = EXPONENTIATION;
				}
				break;
			}
			default:
				{
				}
			break; }
		}
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mDIVISION(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = DIVISION;
		
		bool synPredMatched42 = false;
		if (((LA(1)=='/') && (tokenSet_0_.member(LA(2)))))
		{
			int _m42 = mark();
			synPredMatched42 = true;
			inputState.guessing++;
			try {
				{
					mRE_LITERAL(false);
				}
			}
			catch (RecognitionException)
			{
				synPredMatched42 = false;
			}
			rewind(_m42);
			inputState.guessing--;
		}
		if ( synPredMatched42 )
		{
			mRE_LITERAL(false);
			if (0==inputState.guessing)
			{
				_ttype = RE_LITERAL;
			}
		}
		else if ((LA(1)=='/') && (true)) {
			match('/');
			{
				if ((LA(1)=='='))
				{
					match('=');
					if (0==inputState.guessing)
					{
						_ttype = ASSIGN;
					}
				}
				else {
				}
				
			}
		}
		else
		{
			throw new NoViableAltForCharException((char)LA(1), getFilename(), getLine(), getColumn());
		}
		
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	protected void mRE_LITERAL(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = RE_LITERAL;
		
		match('/');
		{ // ( ... )+
		int _cnt66=0;
		for (;;)
		{
			if ((tokenSet_0_.member(LA(1))))
			{
				mRE_CHAR(false);
			}
			else
			{
				if (_cnt66 >= 1) { goto _loop66_breakloop; } else { throw new NoViableAltForCharException((char)LA(1), getFilename(), getLine(), getColumn());; }
			}
			
			_cnt66++;
		}
_loop66_breakloop:		;
		}    // ( ... )+
		match('/');
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mCMP_OPERATOR(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = CMP_OPERATOR;
		
		if ((LA(1)=='<') && (LA(2)=='='))
		{
			match("<=");
		}
		else if ((LA(1)=='>') && (LA(2)=='=')) {
			match(">=");
		}
		else if ((LA(1)=='!') && (LA(2)=='~')) {
			match("!~");
		}
		else if ((LA(1)=='!') && (LA(2)=='=')) {
			match("!=");
		}
		else if ((LA(1)=='<') && (true)) {
			match('<');
		}
		else if ((LA(1)=='>') && (true)) {
			match('>');
		}
		else
		{
			throw new NoViableAltForCharException((char)LA(1), getFilename(), getLine(), getColumn());
		}
		
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mASSIGN(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = ASSIGN;
		
		match('=');
		{
			if ((LA(1)=='='||LA(1)=='~'))
			{
				{
					switch ( LA(1) )
					{
					case '=':
					{
						match('=');
						break;
					}
					case '~':
					{
						match('~');
						break;
					}
					default:
					{
						throw new NoViableAltForCharException((char)LA(1), getFilename(), getLine(), getColumn());
					}
					 }
				}
				if (0==inputState.guessing)
				{
					_ttype = CMP_OPERATOR;
				}
			}
			else {
			}
			
		}
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mWS(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = WS;
		
		{ // ( ... )+
		int _cnt50=0;
		for (;;)
		{
			switch ( LA(1) )
			{
			case ' ':
			{
				match(' ');
				break;
			}
			case '\t':
			{
				match('\t');
				if (0==inputState.guessing)
				{
					tab();
				}
				break;
			}
			case '\r':
			{
				match('\r');
				break;
			}
			case '\n':
			{
				match('\n');
				if (0==inputState.guessing)
				{
					newline();
				}
				break;
			}
			default:
			{
				if (_cnt50 >= 1) { goto _loop50_breakloop; } else { throw new NoViableAltForCharException((char)LA(1), getFilename(), getLine(), getColumn());; }
			}
			break; }
			_cnt50++;
		}
_loop50_breakloop:		;
		}    // ( ... )+
		if (0==inputState.guessing)
		{
			_ttype = Token.SKIP;
		}
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	public void mSINGLE_QUOTED_STRING(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = SINGLE_QUOTED_STRING;
		
		int _saveIndex = 0;
		_saveIndex = text.Length;
		match('\'');
		text.Length = _saveIndex;
		{    // ( ... )*
			for (;;)
			{
				if ((LA(1)=='\\'))
				{
					mSQS_ESC(false);
				}
				else if ((tokenSet_1_.member(LA(1)))) {
					{
						match(tokenSet_1_);
					}
				}
				else
				{
					goto _loop54_breakloop;
				}
				
			}
_loop54_breakloop:			;
		}    // ( ... )*
		_saveIndex = text.Length;
		match('\'');
		text.Length = _saveIndex;
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	protected void mSQS_ESC(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = SQS_ESC;
		
		int _saveIndex = 0;
		_saveIndex = text.Length;
		match('\\');
		text.Length = _saveIndex;
		{
			switch ( LA(1) )
			{
			case '\\':  case 'n':  case 'r':  case 't':
			{
				mSESC(false);
				break;
			}
			case '\'':
			{
				match('\'');
				break;
			}
			default:
			{
				throw new NoViableAltForCharException((char)LA(1), getFilename(), getLine(), getColumn());
			}
			 }
		}
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	protected void mDQS_ESC(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = DQS_ESC;
		
		int _saveIndex = 0;
		_saveIndex = text.Length;
		match('\\');
		text.Length = _saveIndex;
		{
			switch ( LA(1) )
			{
			case '\\':  case 'n':  case 'r':  case 't':
			{
				mSESC(false);
				break;
			}
			case '"':
			{
				match('"');
				break;
			}
			case '$':
			{
				match('$');
				break;
			}
			default:
			{
				throw new NoViableAltForCharException((char)LA(1), getFilename(), getLine(), getColumn());
			}
			 }
		}
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	protected void mSESC(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = SESC;
		
		switch ( LA(1) )
		{
		case 'r':
		{
			{
				match('r');
				if (0==inputState.guessing)
				{
					text.Length = _begin; text.Append("\r");
				}
			}
			break;
		}
		case 'n':
		{
			{
				match('n');
				if (0==inputState.guessing)
				{
					text.Length = _begin; text.Append("\n");
				}
			}
			break;
		}
		case 't':
		{
			{
				match('t');
				if (0==inputState.guessing)
				{
					text.Length = _begin; text.Append("\t");
				}
			}
			break;
		}
		case '\\':
		{
			{
				match('\\');
				if (0==inputState.guessing)
				{
					text.Length = _begin; text.Append("\\");
				}
			}
			break;
		}
		default:
		{
			throw new NoViableAltForCharException((char)LA(1), getFilename(), getLine(), getColumn());
		}
		 }
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	protected void mRE_CHAR(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = RE_CHAR;
		
		if ((LA(1)=='\\'))
		{
			mRE_ESC(false);
		}
		else if ((tokenSet_2_.member(LA(1)))) {
			{
				match(tokenSet_2_);
			}
		}
		else
		{
			throw new NoViableAltForCharException((char)LA(1), getFilename(), getLine(), getColumn());
		}
		
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	protected void mRE_ESC(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
{
		int _ttype; Token _token=null; int _begin=text.Length;
		_ttype = RE_ESC;
		
		match('\\');
		{
			switch ( LA(1) )
			{
			case 'a':
			{
				match('a');
				break;
			}
			case 'b':
			{
				match('b');
				break;
			}
			case 'c':
			{
				match('c');
				matchRange('A','Z');
				break;
			}
			case 't':
			{
				match('t');
				break;
			}
			case 'r':
			{
				match('r');
				break;
			}
			case 'v':
			{
				match('v');
				break;
			}
			case 'f':
			{
				match('f');
				break;
			}
			case 'n':
			{
				match('n');
				break;
			}
			case 'e':
			{
				match('e');
				break;
			}
			case '0':  case '1':  case '2':  case '3':
			case '4':  case '5':  case '6':  case '7':
			case '8':  case '9':
			{
				{ // ( ... )+
				int _cnt72=0;
				for (;;)
				{
					if (((LA(1) >= '0' && LA(1) <= '9')) && (tokenSet_3_.member(LA(2))))
					{
						mDIGIT(false);
					}
					else
					{
						if (_cnt72 >= 1) { goto _loop72_breakloop; } else { throw new NoViableAltForCharException((char)LA(1), getFilename(), getLine(), getColumn());; }
					}
					
					_cnt72++;
				}
_loop72_breakloop:				;
				}    // ( ... )+
				break;
			}
			case 'x':
			{
				match('x');
				mDIGIT(false);
				mDIGIT(false);
				break;
			}
			case 'u':
			{
				match('u');
				mDIGIT(false);
				mDIGIT(false);
				mDIGIT(false);
				mDIGIT(false);
				break;
			}
			case '\\':
			{
				match('\\');
				break;
			}
			case 'w':
			{
				match('w');
				break;
			}
			case 'W':
			{
				match('W');
				break;
			}
			case 's':
			{
				match('s');
				break;
			}
			case 'S':
			{
				match('S');
				break;
			}
			case 'd':
			{
				match('d');
				break;
			}
			case 'D':
			{
				match('D');
				break;
			}
			case 'p':
			{
				match('p');
				break;
			}
			case 'P':
			{
				match('P');
				break;
			}
			case 'A':
			{
				match('A');
				break;
			}
			case 'z':
			{
				match('z');
				break;
			}
			case 'Z':
			{
				match('Z');
				break;
			}
			case 'g':
			{
				match('g');
				break;
			}
			case 'B':
			{
				match('B');
				break;
			}
			case 'k':
			{
				match('k');
				break;
			}
			case '/':
			{
				match('/');
				break;
			}
			case '(':
			{
				match('(');
				break;
			}
			case ')':
			{
				match(')');
				break;
			}
			case '|':
			{
				match('|');
				break;
			}
			case '.':
			{
				match('.');
				break;
			}
			case '*':
			{
				match('*');
				break;
			}
			case '?':
			{
				match('?');
				break;
			}
			case '$':
			{
				match('$');
				break;
			}
			case '^':
			{
				match('^');
				break;
			}
			case '[':
			{
				match('[');
				break;
			}
			case ']':
			{
				match(']');
				break;
			}
			default:
			{
				throw new NoViableAltForCharException((char)LA(1), getFilename(), getLine(), getColumn());
			}
			 }
		}
		if (_createToken && (null == _token) && (_ttype != Token.SKIP))
		{
			_token = makeToken(_ttype);
			_token.setText(text.ToString(_begin, text.Length-_begin));
		}
		returnToken_ = _token;
	}
	
	
	private static long[] mk_tokenSet_0_()
	{
		long[] data = new long[2048];
		data[0]=-140737488364552L;
		for (int i = 1; i<=1022; i++) { data[i]=-1L; }
		data[1023]=9223372036854775807L;
		for (int i = 1024; i<=2047; i++) { data[i]=0L; }
		return data;
	}
	public static readonly BitSet tokenSet_0_ = new BitSet(mk_tokenSet_0_());
	private static long[] mk_tokenSet_1_()
	{
		long[] data = new long[2048];
		data[0]=-549755823112L;
		data[1]=-268435457L;
		for (int i = 2; i<=1022; i++) { data[i]=-1L; }
		data[1023]=9223372036854775807L;
		for (int i = 1024; i<=2047; i++) { data[i]=0L; }
		return data;
	}
	public static readonly BitSet tokenSet_1_ = new BitSet(mk_tokenSet_1_());
	private static long[] mk_tokenSet_2_()
	{
		long[] data = new long[2048];
		data[0]=-140737488364552L;
		data[1]=-268435457L;
		for (int i = 2; i<=1022; i++) { data[i]=-1L; }
		data[1023]=9223372036854775807L;
		for (int i = 1024; i<=2047; i++) { data[i]=0L; }
		return data;
	}
	public static readonly BitSet tokenSet_2_ = new BitSet(mk_tokenSet_2_());
	private static long[] mk_tokenSet_3_()
	{
		long[] data = new long[2048];
		data[0]=-9224L;
		for (int i = 1; i<=1022; i++) { data[i]=-1L; }
		data[1023]=9223372036854775807L;
		for (int i = 1024; i<=2047; i++) { data[i]=0L; }
		return data;
	}
	public static readonly BitSet tokenSet_3_ = new BitSet(mk_tokenSet_3_());
	
}
}
